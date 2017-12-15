using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using TagsCloudContainer.LayoutAlgorithms;
using TagsCloudContainer.Options;
using TagsCloudContainer.WordProcessors;
using TagsCloudContainer.WordStores;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public class TagsCloud
    {
        private readonly ILayoutAlgorithm layoutAlgorithm;
        private readonly IWordStore words;
        private readonly IEnumerable<IWordProcessor> wordProcessors;
        private const int ImageMargin = 5;
        private const int WordLimit = 100;

        public TagsCloud(ILayoutAlgorithm layoutAlgorithm, IWordStore words, IEnumerable<IWordProcessor> wordProcessors)
        {
            this.layoutAlgorithm = layoutAlgorithm;
            this.words = words;
            this.wordProcessors = wordProcessors;
        }

        public Image CreateTagsCloud()
        {
            return CreateTagsCloud(new TagCloudOptions
            {
                Font = SystemFonts.DefaultFont,
                MinimumFontSize = 10,
                MaximumFontSize = 16
            });

        }
        public Image CreateTagsCloud(TagCloudOptions options)
        {
            var wordsForCloud = wordProcessors.Aggregate(words, (current, processor) => current.Process(processor));
            var wordFrequencies = new Dictionary<string, WordData>();
            foreach (var word in wordsForCloud.Words)
            {
                if (wordFrequencies.ContainsKey(word))
                {
                    wordFrequencies[word].Frequency++;
                }
                else
                {
                    wordFrequencies[word] = new WordData
                    {
                        Word = word,
                        Frequency = 1
                    };
                }
            }
            IEnumerable<WordData> resultWords = wordFrequencies
                .Select(w => w.Value)
                .OrderByDescending(w => w.Frequency)
                .ThenBy(w => w.Word)
                .Take(WordLimit)
                .ToList();
            CalculateFontSizes(resultWords, options.MinimumFontSize, options.MaximumFontSize);
            CalculateColors(resultWords);
            CalculateTextSizes(resultWords, options.Font);
            var rectangles = layoutAlgorithm.Layout(new Point(0, 0), resultWords.Select(w => w.Size));
            resultWords = resultWords.Zip(rectangles, (word, rectangle) =>
            {
                word.Rectangle = rectangle;
                return word;
            });
            return GenerateImage(resultWords, options.Font);
        }

        private static Rectangle GetResultBoundingBox(IEnumerable<Rectangle> boundingBoxes)
        {
            var top = 0;
            var left = 0;
            var bottom = 0;
            var right = 0;
            foreach (var box in boundingBoxes)
            {
                top = box.Top < top ? box.Top : top;
                left = box.Left < left ? box.Left : left;
                bottom = box.Bottom > bottom ? box.Bottom : bottom;
                right = box.Right > right ? box.Right : right;
            }
            return new Rectangle(left, top, right - left, bottom - top);
        }

        private static Image GenerateImage(IEnumerable<WordData> words, Font font)
        {
            var boundingBox = GetResultBoundingBox(words.Select(word => word.Rectangle));
            boundingBox.Inflate(ImageMargin, ImageMargin);
            var result = new Bitmap(boundingBox.Width, boundingBox.Height);
            using (var graphics = Graphics.FromImage(result))
            {
                foreach (var word in words)
                {
                    var relativePosition = word.Rectangle.Location - new Size(boundingBox.Location);
                    var wordFont = new Font(font.FontFamily, word.FontSize, font.Style, font.Unit, font.GdiCharSet);
                    Brush wordBrush = new SolidBrush(word.Color);
                    graphics.DrawString(word.Word, wordFont, wordBrush, relativePosition);
                }
            }
            return result;
        }

        private static void CalculateFontSizes(IEnumerable<WordData> words, int minFontSize, int maxFontSize)
        {
            var fontSizes = Enumerable.Range(minFontSize, maxFontSize - minFontSize + 1).Reverse().ToArray();
            var wordsArray = words as WordData[] ?? words.ToArray();
            var fontSizePeriod = wordsArray.Length / fontSizes.Length + 1;
            var wordIndex = 0;
            foreach (var word in wordsArray)
            {
                word.FontSize = fontSizes[wordIndex / fontSizePeriod];
                wordIndex++;
            }
        }

        private static void CalculateColors(IEnumerable<WordData> words)
        {
            foreach (var word in words)
            {
                word.Color = Color.Blue;
            }
        }

        private static void CalculateTextSizes(IEnumerable<WordData> words, Font font)
        {
            var graphics = Graphics.FromHwnd(IntPtr.Zero);
            foreach (var word in words)
            {
                var wordFont = new Font(font.FontFamily, word.FontSize, font.Style, font.Unit, font.GdiCharSet);
                word.Size = Size.Ceiling(graphics.MeasureString(word.Word, wordFont, new PointF(0, 0), StringFormat.GenericDefault));
            }
        }


    }
}
