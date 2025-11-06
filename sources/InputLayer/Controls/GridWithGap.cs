using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace InputLayer.Controls
{
    public class GridWithGap : Grid
    {
        public static readonly DependencyProperty ColumnGapProperty =
            DependencyProperty.Register(nameof(ColumnGap), typeof(double), typeof(GridWithGap), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RowGapProperty =
            DependencyProperty.Register(nameof(RowGap), typeof(double), typeof(GridWithGap), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

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
            var totalRowGap = Math.Max(0, this.RowDefinitions.Count - 1) * this.RowGap;
            var totalColumnGap = Math.Max(0, this.ColumnDefinitions.Count - 1) * this.ColumnGap;

            var availableSize = new Size(Math.Max(0, arrangeSize.Width - totalColumnGap),
                                         Math.Max(0, arrangeSize.Height - totalRowGap));

            base.ArrangeOverride(availableSize);

            foreach (FrameworkElement child in this.InternalChildren)
            {
                var row = GetRow(child);
                var column = GetColumn(child);
                var rowSpan = GetRowSpan(child);
                var columnSpan = GetColumnSpan(child);

                var rowOffset = row * this.RowGap;
                var columnOffset = column * this.ColumnGap;

                var currentRect = LayoutInformation.GetLayoutSlot(child);

                var gapsInElement = (columnSpan - 1) * this.ColumnGap;
                var rowGapsInElement = (rowSpan - 1) * this.RowGap;

                var newRect = new Rect(currentRect.X + columnOffset,
                                       currentRect.Y + rowOffset,
                                       Math.Max(0, currentRect.Width - gapsInElement),
                                       Math.Max(0, currentRect.Height - rowGapsInElement));

                child.Arrange(newRect);
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var totalRowGap = Math.Max(0, this.RowDefinitions.Count - 1) * this.RowGap;
            var totalColumnGap = Math.Max(0, this.ColumnDefinitions.Count - 1) * this.ColumnGap;

            var availableSize = new Size(Math.Max(0, constraint.Width - totalColumnGap),
                                         Math.Max(0, constraint.Height - totalRowGap));


            var result = base.MeasureOverride(availableSize);

            return new Size(result.Width + totalColumnGap, result.Height + totalRowGap);
        }
    }
}