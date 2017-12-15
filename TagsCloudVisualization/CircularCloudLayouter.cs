using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point centerPoint;
        private readonly List<Rectangle> wordRectangles = new List<Rectangle>();
        private const double AngleStep = Math.PI / 120;
        private const int DistanceStep = 1;

        public IEnumerable<Rectangle> WordRectangles => wordRectangles.AsEnumerable();

        private Point CompactTowardsCenter(Rectangle rectangle)
        {
            Point resultLocation = rectangle.Location;
            Point rectangleCenter = new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
            int compactDirectionHor = rectangleCenter.X > centerPoint.X ? -1 : 1;
            int compactDirectionVer = rectangleCenter.Y > centerPoint.Y ? -1 : 1;
            bool moved = true;
            while (moved)
            {
                moved = false;
                Point newCenter = Point.Empty;
                Point movedXPoint = new Point(resultLocation.X + compactDirectionHor, resultLocation.Y);
                Rectangle movedX = new Rectangle(movedXPoint, rectangle.Size);
                if (wordRectangles.All(r => r.IntersectsWith(movedX) == false))
                {
                    resultLocation = movedXPoint;
                    newCenter = new Point(movedX.Left + movedX.Width / 2, movedX.Top + movedX.Height / 2);
                    moved = true;
                }
                Point movedYPoint = new Point(resultLocation.X, resultLocation.Y + compactDirectionVer);
                Rectangle movedY = new Rectangle(movedYPoint, rectangle.Size);
                if (wordRectangles.All(r => r.IntersectsWith(movedY) == false))
                {
                    resultLocation = movedYPoint;
                    newCenter = new Point(movedY.Left + movedY.Width / 2, movedY.Top + movedY.Height / 2);
                    moved = true;
                }
                if (newCenter != Point.Empty)
                {
                    int newCompactDirectionHor = newCenter.X > centerPoint.X ? -1 : 1;
                    int newCompactDirectionVer = newCenter.Y > centerPoint.Y ? -1 : 1;
                    if (newCompactDirectionHor != compactDirectionHor || newCompactDirectionVer != compactDirectionVer)
                    {
                        moved = false;
                    }
                }
            }
            return resultLocation;
        }
        private Rectangle GetResultingRectanglePosition(Size rectangleSize)
        {
            Rectangle result = new Rectangle(centerPoint.X - rectangleSize.Width / 2, centerPoint.Y - rectangleSize.Height / 2, rectangleSize.Width,
                rectangleSize.Height);
            Point initialLocation = result.Location;
            double currentAngle = 0;
            int currentDistance = 0;
            while (wordRectangles.Any(r => r.IntersectsWith(result)))
            {
                double newLeft = currentDistance * Math.Cos(currentAngle);
                double newTop = currentDistance * Math.Sin(currentAngle);
                Point newLocation = new Point(initialLocation.X + (int)Math.Ceiling(newLeft), initialLocation.Y + (int)Math.Ceiling(newTop));
                result.Location = newLocation;
                currentAngle += AngleStep;
                currentDistance += DistanceStep;
            }
            if (wordRectangles.Count > 0)
            {
                result.Location = CompactTowardsCenter(result);
            }
            return result;
        }
        public CircularCloudLayouter(Point center)
        {
            centerPoint = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle result = GetResultingRectanglePosition(rectangleSize);
            wordRectangles.Add(result);
            return result;
        }
    }
}
