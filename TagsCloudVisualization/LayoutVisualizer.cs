using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class LayoutVisualizer
    {
        private const int MarginSize = 2;
        public static Rectangle GetBoundingBox(IEnumerable<Rectangle> rectangles)
        {
            int leftBound = 0;
            int rightBound = 0;
            int topBound = 0;
            int bottomBound = 0;
            foreach (Rectangle rectangle in rectangles)
            {
                leftBound = rectangle.Left < leftBound ? rectangle.Left : leftBound;
                rightBound = rectangle.Right > rightBound ? rectangle.Right : rightBound;
                topBound = rectangle.Top < topBound ? rectangle.Top : topBound;
                bottomBound = rectangle.Bottom > bottomBound ? rectangle.Bottom : bottomBound;
            }
            return new Rectangle(leftBound, topBound, (rightBound - leftBound), (bottomBound - topBound));
        }

        public static Bitmap Visualize(IEnumerable<Rectangle> rectangles)
        {
            Rectangle boundingBox = GetBoundingBox(rectangles);
            Bitmap result = new Bitmap(boundingBox.Width + MarginSize * 2, boundingBox.Height + MarginSize * 2);
            Graphics gr = Graphics.FromImage(result);
            Pen pen = new Pen(Color.Gold);
            foreach (Rectangle rectangle in rectangles)
            {
                Point relativeLocation = new Point(rectangle.Left - boundingBox.Left + MarginSize, rectangle.Top - boundingBox.Top + MarginSize);
                gr.DrawRectangle(pen, new Rectangle(relativeLocation, new Size(rectangle.Width, rectangle.Height)));
            }
            return result;
        }
    }
}
