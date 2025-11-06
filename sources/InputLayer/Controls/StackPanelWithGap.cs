using System;
using System.Windows;
using System.Windows.Controls;

namespace InputLayer.Controls
{
    public class StackPanelWithGap : StackPanel
    {
        public static readonly DependencyProperty GapProperty =
            DependencyProperty.Register(nameof(Gap), typeof(double), typeof(StackPanelWithGap), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Gap
        {
            get => (double)this.GetValue(GapProperty);
            set => this.SetValue(GapProperty, value);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var isHorizontal = this.Orientation == Orientation.Horizontal;
            double offset = 0;

            foreach (UIElement child in this.InternalChildren)
            {
                var desiredSize = child.DesiredSize;
                Rect childRect;

                if (isHorizontal)
                {
                    childRect = new Rect(offset, 0, desiredSize.Width, arrangeSize.Height);
                    offset += desiredSize.Width + this.Gap;
                }
                else
                {
                    childRect = new Rect(0, offset, arrangeSize.Width, desiredSize.Height);
                    offset += desiredSize.Height + this.Gap;
                }

                child.Arrange(childRect);
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var isHorizontal = this.Orientation == Orientation.Horizontal;
            var childCount = this.InternalChildren.Count;
            var totalGap = Math.Max(0, childCount - 1) * this.Gap;

            var availableSize = isHorizontal
                ? new Size(Math.Max(0, constraint.Width - totalGap), constraint.Height)
                : new Size(constraint.Width, Math.Max(0, constraint.Height - totalGap));

            double totalPrimarySize = 0;
            double maxSecondarySize = 0;

            foreach (UIElement child in this.InternalChildren)
            {
                child.Measure(availableSize);
                var desiredSize = child.DesiredSize;

                if (isHorizontal)
                {
                    totalPrimarySize += desiredSize.Width;
                    maxSecondarySize = Math.Max(maxSecondarySize, desiredSize.Height);
                }
                else
                {
                    totalPrimarySize += desiredSize.Height;
                    maxSecondarySize = Math.Max(maxSecondarySize, desiredSize.Width);
                }
            }

            return isHorizontal
                ? new Size(totalPrimarySize + totalGap, maxSecondarySize)
                : new Size(maxSecondarySize, totalPrimarySize + totalGap);
        }
    }
}