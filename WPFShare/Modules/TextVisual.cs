using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Signalway.CommThemes.Modules
{
    public class TextVisual : DrawingVisual
    {
        /// <summary>
        /// 文字内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// 绘制颜色
        /// </summary>
        public Brush DrawBrush { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; }

        public TextVisual(string text, Point pt)
        {
            Text = text;
            Position = pt;

            DrawBrush = Brushes.Red;

            FontSize = 12;
        }

        public new void Drawing()
        {
            using (DrawingContext dc = this.RenderOpen())
            {
                FormattedText formattedText = new FormattedText(
                    Text,
                    CultureInfo.GetCultureInfo("zh-Hans"),
                    FlowDirection.LeftToRight,
                    new Typeface("微软雅黑"),
                    FontSize,
                    DrawBrush);

                formattedText.SetFontWeight(FontWeights.ExtraBold);

                double X = Position.X;
                if (X > (formattedText.Width / 2)) X -= (formattedText.Width / 2);
                double Y = Position.Y;
                if (Y > (formattedText.Height / 2)) Y -= (formattedText.Height / 2);

                dc.DrawText(formattedText, new Point(X, Y));
            }
        }
    }
}
