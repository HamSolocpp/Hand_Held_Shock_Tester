using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GraphsWithGeometrics
{
    class GraphBuilder
    {
        public PointF[] Create_polygon(int[] Y, int Y_Offset, int X_Offset, int length, int height)
        {
            int max = 0;
            for (int x = 0; x < Y.Length; x++) if (Y[x] > max) max = Y[x];
            float factor = (float)height / max;
            PointF[] polygon = new PointF[Y.Length+2];
            polygon[0].X = X_Offset;
            polygon[0].Y = Y_Offset;
            polygon[Y.Length + 1].X = length ;
            polygon[Y.Length + 1].Y = Y_Offset;

            for (int z = 0; z < Y.Length; z++)
            {
                polygon[z + 1].X = X_Offset + Convert.ToInt16(z * length/polygon.Length );
                polygon[z + 1].Y = Y_Offset - (Y[z]*factor);
            }
            return polygon;
        }

        public PointF[] Create_Lines(int[] Y, int Y_Offset, int X_Offset)
        {
            PointF[] polygon = new PointF[Y.Length];

            for (int z = 0; z < 100; z++)
            {
                polygon[z].X = X_Offset + z * 5;
                polygon[z].Y = Y_Offset - Y[z];
            }
            return polygon;
        }

        public void DrawScaleX(int Length, int X, int Y, int ticks, Panel panel)
        {
            Graphics graphics = panel.CreateGraphics();
            Pen pen = Pens.Black;
            graphics.DrawLine(pen, new Point(X, Y), new Point(X + Length, Y));
            for (int z = 1; z < ticks; z++)
            {
                graphics.DrawLine(pen, new Point(X + z * (Length / ticks), Y + 5), new Point(X + z * (Length / ticks), Y - 5));
            }
            graphics.DrawLine(pen, new Point(X + Length - 5, Y - 5), new Point(X + Length, Y));
            graphics.DrawLine(pen, new Point(X + Length - 5, Y + 5), new Point(X + Length, Y));
        }

        public void DrawScaleY(int Height, int X, int Y, int ticks, Panel panel)
        {
            Graphics graphics = panel.CreateGraphics();
            Pen pen = Pens.Black;
            graphics.DrawLine(pen, new Point(X, Y), new Point(X, Y - Height));

            for (int z = 1; z < ticks; z++)
            {
                graphics.DrawLine(pen, new Point(X + 5, Y - z * (Height / ticks)), new Point(X - 5, Y - z * (Height / ticks)));
            }

            graphics.DrawLine(pen, new Point(X - 5, Y - Height + 5), new Point(X, Y - Height));
            graphics.DrawLine(pen, new Point(X + 5, Y - Height + 5), new Point(X, Y - Height));
        }
    }
}
