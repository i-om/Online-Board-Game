using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Ages
{
    
        public class SimpleAdorner : Adorner
        {
            // Be sure to call the base class constructor.
            public SimpleAdorner(UIElement adornedElement)
                : base(adornedElement)
            {
                VisualBrush _brush = new VisualBrush(adornedElement);


                _child = new Rectangle();
                _child.Width = adornedElement.RenderSize.Width;
                _child.Height = adornedElement.RenderSize.Height;
                _child.Opacity = 0.7;


                //DoubleAnimation animation = new DoubleAnimation(0.5, 0.5, new Duration(TimeSpan.FromSeconds(1)));
               // animation.AutoReverse = true;
               // animation.RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever;
                //_brush.BeginAnimation(System.Windows.Media.Brush.OpacityProperty, animation);

                _child.Fill = _brush;
            }

            // A common way to implement an adorner's rendering behavior is to override the OnRender
            // method, which is called by the layout subsystem as part of a rendering pass.
            protected override void OnRender(DrawingContext drawingContext)
            {
                // Get a rectangle that represents the desired size of the rendered element
                // after the rendering pass.  This will be used to draw at the corners of the 
                // adorned element.
                /* Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

                  // Some arbitrary drawing implements.
                  SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
                  renderBrush.Opacity = 0.2;
                  Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
                  double renderRadius = 5.0;

                  // Just draw a circle at each corner.
                  drawingContext.DrawRectangle(renderBrush, renderPen, adornedElementRect);
                  drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
                  drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
                  drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
                  drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);*/
            }

            protected override Size MeasureOverride(Size constraint)
            {
                _child.Measure(constraint);
                return _child.DesiredSize;
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                _child.Arrange(new Rect(finalSize));
                return finalSize;
            }

            protected override Visual GetVisualChild(int index)
            {
                return _child;
            }

            protected override int VisualChildrenCount
            {
                get
                {
                    return 5;
                }
            }

            public double LeftOffset
            {
                get
                {
                    return _leftOffset;
                }
                set
                {
                    _leftOffset = value;
                    UpdatePosition();
                }
            }

            public double TopOffset
            {
                get
                {
                    return _topOffset;
                }
                set
                {
                    _topOffset = value;
                    UpdatePosition();

                }
            }

            private void UpdatePosition()
            {
                AdornerLayer adornerLayer = this.Parent as AdornerLayer;
                //MessageBox.Show(this.Parent.ToString());
                if (adornerLayer != null)
                {
                    adornerLayer.Update(AdornedElement);
                }
            }

            public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
            {
                GeneralTransformGroup result = new GeneralTransformGroup();
                result.Children.Add(base.GetDesiredTransform(transform));
                result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));
                return result;
            }

            private Rectangle _child = null;
            private double _leftOffset = 0;
            private double _topOffset = 0;
        }
    
}
