using System;
using System.Drawing;

namespace Function.Util {

    /// <summary>
    /// Represents a rectangle with top-left and bottom-right coordinates
    /// </summary>
    public struct Box {

        public int X1, Y1, X2, Y2;

        public Point TopLeft => new Point(Math.Min(X1, X2), Math.Min(Y1, Y2));
        public Point BottomRight => new Point(Math.Max(X1, X2), Math.Max(Y1, Y2));

        public Size Size => new Size(Math.Abs(TopLeft.X - BottomRight.X), Math.Abs(TopLeft.Y - BottomRight.Y));

        public int Width => Size.Width;
        public int Height => Size.Height;

        public Rectangle ToRectangle() => new Rectangle(TopLeft.X, TopLeft.Y, Width, Height);

        public override string ToString() {
            return $"{{X1={X1}, Y1={Y1}, X2={X2}, Y2={Y2},\nTopLeft={TopLeft}, BottomRight={BottomRight},\nSize={Size},\nWidth={Width}, Height={Height}}}";
        }

        public static implicit operator BoxF(Box box) {
            return new BoxF {
                X1 = box.X1,
                Y1 = box.Y1,
                X2 = box.X2,
                Y2 = box.Y2
            };
        }
    }

    public struct BoxF {

        public float X1, Y1, X2, Y2;

        public PointF TopLeft => new PointF(Math.Min(X1, X2), Math.Min(Y1, Y2));
        public PointF BottomRight => new PointF(Math.Max(X1, X2), Math.Max(Y1, Y2));

        public SizeF Size => new SizeF(Math.Abs(TopLeft.X - BottomRight.X), Math.Abs(TopLeft.Y - BottomRight.Y));

        public float Width => Size.Width;
        public float Height => Size.Height;

        public RectangleF ToRectangle() => new RectangleF(TopLeft.X, TopLeft.Y, Width, Height);

        public override string ToString() {
            return $"{{X1={X1}, Y1={Y1}, X2={X2}, Y2={Y2},\nTopLeft={TopLeft}, BottomRight={BottomRight},\nSize={Size},\nWidth={Width}, Height={Height}}}";
        }

        public static explicit operator Box(BoxF boxf) {
            return new Box {
                X1 = (int) boxf.X1,
                Y1 = (int) boxf.Y1,
                X2 = (int) boxf.X2,
                Y2 = (int) boxf.Y2
            };
        }
    }
}