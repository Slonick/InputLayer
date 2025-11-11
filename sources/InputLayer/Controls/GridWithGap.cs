using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace InputLayer.Controls
{
    public class GridWithGap : Grid
    {
        public static readonly DependencyProperty ColumnGapProperty =
            DependencyProperty.Register(nameof(ColumnGap), typeof(double), typeof(GridWithGap),
                                        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RowGapProperty =
            DependencyProperty.Register(nameof(RowGap), typeof(double), typeof(GridWithGap),
                                        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double ColumnGap
        {
            get => (double)this.GetValue(ColumnGapProperty);
            set => this.SetValue(ColumnGapProperty, value);
        }

        public double RowGap
        {
            get => (double)this.GetValue(RowGapProperty);
            set => this.SetValue(RowGapProperty, value);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var occupiedCells = this.GetOccupiedCells();

            var totalRowGap = Math.Max(0, occupiedCells.Rows.Count - 1) * this.RowGap;
            var totalColumnGap = Math.Max(0, occupiedCells.Columns.Count - 1) * this.ColumnGap;

            var availableSize = new Size(Math.Max(0, arrangeSize.Width - totalColumnGap),
                                         Math.Max(0, arrangeSize.Height - totalRowGap));

            base.ArrangeOverride(availableSize);

            var totalColumns = Math.Max(1, this.ColumnDefinitions.Count);
            var totalRows = Math.Max(1, this.RowDefinitions.Count);

            foreach (FrameworkElement child in this.InternalChildren)
            {
                if (child.Visibility == Visibility.Collapsed)
                {
                    continue;
                }

                var row = GetRow(child);
                var column = GetColumn(child);
                var rowSpan = GetRowSpan(child);
                var columnSpan = GetColumnSpan(child);

                var spansAllColumns = column == 0 && columnSpan >= totalColumns;
                var spansAllRows = row == 0 && rowSpan >= totalRows;

                var rowOffset = spansAllRows ? 0 : this.CountGapsBefore(row, occupiedCells.Rows) * this.RowGap;
                var columnOffset = spansAllColumns ? 0 : this.CountGapsBefore(column, occupiedCells.Columns) * this.ColumnGap;

                var currentRect = LayoutInformation.GetLayoutSlot(child);

                var occupiedColumnsInSpan = occupiedCells.Columns.Count(c => c >= column && c < column + columnSpan);
                var occupiedRowsInSpan = occupiedCells.Rows.Count(r => r >= row && r < row + rowSpan);

                var gapsInElement = spansAllColumns ? 0 : Math.Max(0, occupiedColumnsInSpan - 1) * this.ColumnGap;
                var rowGapsInElement = spansAllRows ? 0 : Math.Max(0, occupiedRowsInSpan - 1) * this.RowGap;

                var widthAdjustment = spansAllColumns ? totalColumnGap : -gapsInElement;
                var heightAdjustment = spansAllRows ? totalRowGap : -rowGapsInElement;

                var newRect = new Rect(currentRect.X + columnOffset,
                                       currentRect.Y + rowOffset,
                                       Math.Max(0, currentRect.Width + widthAdjustment),
                                       Math.Max(0, currentRect.Height + heightAdjustment));

                child.Arrange(newRect);
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var occupiedCells = this.GetOccupiedCells();

            var totalRowGap = Math.Max(0, occupiedCells.Rows.Count - 1) * this.RowGap;
            var totalColumnGap = Math.Max(0, occupiedCells.Columns.Count - 1) * this.ColumnGap;

            var availableSize = new Size(Math.Max(0, constraint.Width - totalColumnGap),
                                         Math.Max(0, constraint.Height - totalRowGap));

            var result = base.MeasureOverride(availableSize);

            return new Size(result.Width + totalColumnGap, result.Height + totalRowGap);
        }

        private int CountGapsBefore(int position, HashSet<int> occupied)
            => occupied.Count(p => p < position);

        private OccupiedCells GetOccupiedCells()
        {
            var occupiedRows = new HashSet<int>();
            var occupiedColumns = new HashSet<int>();

            foreach (UIElement child in this.InternalChildren)
            {
                if (child.Visibility == Visibility.Collapsed)
                {
                    continue;
                }

                var row = GetRow(child);
                var column = GetColumn(child);
                var rowSpan = GetRowSpan(child);
                var columnSpan = GetColumnSpan(child);

                for (var r = row; r < row + rowSpan; r++)
                {
                    occupiedRows.Add(r);
                }

                for (var c = column; c < column + columnSpan; c++)
                {
                    occupiedColumns.Add(c);
                }
            }

            return new OccupiedCells(occupiedRows, occupiedColumns);
        }

        private class OccupiedCells
        {
            public OccupiedCells(HashSet<int> rows, HashSet<int> columns)
            {
                this.Rows = rows;
                this.Columns = columns;
            }

            public HashSet<int> Columns { get; }

            public HashSet<int> Rows { get; }
        }
    }
}