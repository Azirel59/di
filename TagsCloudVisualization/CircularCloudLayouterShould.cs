using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        private const string ImageSubDir = "LayoutTestResults";
        private CircularCloudLayouter layouter;
        private Random rnd;

        [SetUp]
        public void SetUp()
        {
            rnd = new Random();
        }

        private void PrepareTestLayout(Point center, Size minRectangleSize, Size maxRectangleSize, int sampleCount)
        {
            layouter = new CircularCloudLayouter(center);
            for (int i = 0; i < sampleCount; i++)
            {
                layouter.PutNextRectangle(GetRandomRectangle(minRectangleSize, maxRectangleSize));
            }
        }

        public static IEnumerable<TestCaseData> EvenAndOddRectangleSizes
        {
            get
            {
                yield return new TestCaseData(new Size(25, 15), new Rectangle(-12, -7, 25, 15), new Point(0,0)).SetName("Both sizes odd. Center at zero");
                yield return new TestCaseData(new Size(30, 15), new Rectangle(-15, -7, 30, 15), new Point(0, 0)).SetName("Width even. Center at zero");
                yield return new TestCaseData(new Size(25, 20), new Rectangle(-12, -10, 25, 20), new Point(0, 0)).SetName("Height even. Center at zero");
                yield return new TestCaseData(new Size(30, 20), new Rectangle(-15, -10, 30, 20), new Point(0, 0)).SetName("Both sizes even. Center at zero");
                yield return new TestCaseData(new Size(25, 15), new Rectangle(-112, 93, 25, 15), new Point(-100, 100)).SetName("Both sizes odd. Center at -100, 100");
                yield return new TestCaseData(new Size(30, 15), new Rectangle(-115, 93, 30, 15), new Point(-100, 100)).SetName("Width even. Center at -100, 100");
                yield return new TestCaseData(new Size(25, 20), new Rectangle(-112, 90, 25, 20), new Point(-100, 100)).SetName("Height even. Center at -100, 100");
                yield return new TestCaseData(new Size(30, 20), new Rectangle(-115, 90, 30, 20), new Point(-100, 100)).SetName("Both sizes even. Center at z-100, 100");
            }
        }

        Size GetRandomRectangle(Size minRectangleSize, Size maxRectangleSize)
        {
            return new Size(rnd.Next(minRectangleSize.Width, maxRectangleSize.Width + 1), rnd.Next(minRectangleSize.Height, maxRectangleSize.Height + 1));
        }

        double GetCircularity()
        {
            Rectangle boundingBox = LayoutVisualizer.GetBoundingBox(layouter.WordRectangles);
            double circularity = 1;
            if (boundingBox.Height != 0 && boundingBox.Width != 0)
            {
                if (boundingBox.Width > boundingBox.Height)
                {
                    circularity = (double)boundingBox.Height / boundingBox.Width;
                }
                else
                {
                    circularity = (double)boundingBox.Width / boundingBox.Height;
                }
            }
            return circularity;
        }

        [TestCaseSource(nameof(EvenAndOddRectangleSizes))]
        public void PutNextRectangle_ShouldCenterFirstRectangle(Size sourceRect, Rectangle resultRect, Point center)
        {
            layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(sourceRect).ShouldBeEquivalentTo(resultRect);
        }

        public static IEnumerable<TestCaseData> RandomGenerationTestData
        {
            get
            {
                yield return new TestCaseData(new Point(0,0), new Size(10,10), new Size(200, 200), 10).SetName("Wide size range. 10 rectangles. Center at zero");
                yield return new TestCaseData(new Point(0, 0), new Size(10, 10), new Size(200, 200), 10).SetName("Wide size range. 50 rectangles. Center at zero");
                yield return new TestCaseData(new Point(0, 0), new Size(10, 10), new Size(200, 200), 10).SetName("Wide size range. 100 rectangles. Center at zero");
                yield return new TestCaseData(new Point(-100, 100), new Size(10, 10), new Size(200, 200), 10).SetName("Wide size range. 10 rectangles. Center at -100, 100");
                yield return new TestCaseData(new Point(-100, 100), new Size(10, 10), new Size(200, 200), 10).SetName("Wide size range. 50 rectangles. Center at -100, 100");
                yield return new TestCaseData(new Point(-100, 100), new Size(10, 10), new Size(200, 200), 10).SetName("Wide size range. 100 rectangles. Center at ze-100, 100");
            }
        }

        [TestCaseSource(nameof(RandomGenerationTestData))]
        public void WordRectangles_ShouldNotIntersect(Point center, Size minSize, Size maxSize, int sampleCount)
        {
            PrepareTestLayout(center, minSize, maxSize, sampleCount);
            layouter.WordRectangles.All(firstRect => layouter.WordRectangles.All(secondRect =>
                secondRect == firstRect || secondRect.IntersectsWith(firstRect) == false)).Should().BeTrue();
        }

        public static IEnumerable<TestCaseData> RandomGenerationTestDataForCircularity
        {
            get
            {
                foreach (var data in RandomGenerationTestData)
                {
                    var argsList = data.Arguments.ToList();
                    var successList = argsList.ToList();
                    successList.Add(0.4);
                    successList.Add(true);
                    yield return new TestCaseData(successList.ToArray()).SetName(data.TestName + ". Irregularity under 0.3");
                    var failureList = argsList.ToList();
                    failureList.Add(0.05);
                    failureList.Add(false);
                    yield return new TestCaseData(successList.ToArray()).SetName(data.TestName + ". Irregularity over 0.05");
                }
            }
        }

        [TestCaseSource(nameof(RandomGenerationTestDataForCircularity))]
        public void WordRectangles_ShouldBeCircular(Point center, Size minSize, Size maxSize, int sampleCount, double irregularityThreshold, bool shouldSucceed)
        {
            PrepareTestLayout(center, minSize, maxSize, sampleCount);
            double circularity = GetCircularity();
            double irregularity = 1 - circularity;
            if (shouldSucceed)
            {
                irregularity.Should().BeLessOrEqualTo(irregularityThreshold);
            }
            else
            {
                irregularity.Should().BeGreaterThan(irregularityThreshold);
            }
        }

        public static IEnumerable<TestCaseData> RandomGenerationTestDataForCompactness
        {
            get
            {
                foreach (var data in RandomGenerationTestData)
                {
                    var argsList = data.Arguments.ToList();
                    var successList = argsList.ToList();
                    successList.Add(0.4);
                    successList.Add(true);
                    yield return new TestCaseData(successList.ToArray()).SetName(data.TestName + ". Fill over 40%");
                    var failureList = argsList.ToList();
                    failureList.Add(0.7);
                    failureList.Add(false);
                    yield return new TestCaseData(successList.ToArray()).SetName(data.TestName + ". Fill under 70%");
                }
            }
        }

        [TestCaseSource(nameof(RandomGenerationTestDataForCompactness))]
        public void WordRectangles_ShouldBeCompact(Point center, Size minSize, Size maxSize, int sampleCount, double fillThreshold, bool shouldSucceed)
        {
            PrepareTestLayout(center, minSize, maxSize, sampleCount);
            Rectangle boundingBox = LayoutVisualizer.GetBoundingBox(layouter.WordRectangles);
            double circleRadius = (double)Math.Max(boundingBox.Width, boundingBox.Height) / 2;
            double circleArea = Math.PI * circleRadius * circleRadius;
            double rectanglesArea = layouter.WordRectangles.Select(r => r.Height * r.Width).Sum();
            double fillRate = rectanglesArea / circleArea;
            if (shouldSucceed)
            {
                fillRate.Should().BeGreaterOrEqualTo(fillThreshold);
            }
            else
            {
                fillRate.Should().BeLessThan(fillThreshold);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Equals(ResultState.Success))
            {
                string testName = TestContext.CurrentContext.Test.Name;
                DateTime now = DateTime.Now;
                string workDirectory = TestContext.CurrentContext.TestDirectory;
                string path = Path.Combine(workDirectory, ImageSubDir);
                Directory.CreateDirectory(path);
                string fileName =
                    $"{now.Year}-{now.Month}-{now.Day} {now.Hour}-{now.Minute}-{now.Second} {now.Millisecond}ms - {testName}.png";
                LayoutVisualizer.Visualize(layouter.WordRectangles).Save(Path.Combine(path, fileName), ImageFormat.Gif);
            }
        }
    }
}
