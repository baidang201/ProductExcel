using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Signalway.CommThemes.Modules
{
    /// <summary>
    /// 圆形点
    /// </summary>
    public class CirclePoint : DrawingVisual
    {
        private bool isSelected = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
            }
        }

        private double circleSize = 6D;
        /// <summary>
        /// 园点大小
        /// </summary>
        public double CircleSize
        {
            get { return this.circleSize; }
            set
            {
                this.circleSize = value;
            }
        }

        private Brush drawingBrush = Brushes.Black;
        /// <summary>
        /// 正常绘制画刷
        /// </summary>
        public Brush DrawingBrush
        {
            get { return this.drawingBrush; }
            set
            {
                this.drawingBrush = value;
            }
        }

        private Brush selectedBrush = Brushes.Red;
        /// <summary>
        /// 选中画刷
        /// </summary>
        public Brush SelectedBrush
        {
            get { return this.selectedBrush; }
            set
            {
                this.selectedBrush = value;
            }
        }

        private Point centerPoint;
        /// <summary>
        /// 中心点
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                return this.centerPoint;
            }
            set
            {
                this.centerPoint = value;
                Drawing();
            }
        }

        public CirclePoint()
        {
        }

        public CirclePoint(double X, double Y)
        {
            centerPoint = new Point(X,Y);
        }

        /// <summary>
        /// 绘制
        /// </summary>
        public new void Drawing()
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                Brush brush = drawingBrush;
                if (isSelected) brush = selectedBrush;

                Pen drawingPen = new Pen(brush, 1);
                dc.DrawEllipse(brush, drawingPen, centerPoint, circleSize, circleSize);
            }
        }
    }

    public class CircleComparer : IComparer<CirclePoint>
    {
        public int Compare(CirclePoint x, CirclePoint y)
        {
            if (x == null && y == null)
                return 0;
            else if (x != null && y == null)
                return 1;
            else if (x == null && y != null)
                return -1;
            else
            {
                if (x.CenterPoint.X == y.CenterPoint.X)
                    return 0;
                else if (x.CenterPoint.X > y.CenterPoint.X)
                    return 1;
                else
                    return -1;
            }
        }
    }
}
