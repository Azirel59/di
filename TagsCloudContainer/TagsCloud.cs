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
                .ToList();
            throw new NotImplementedException();
        }


    }
}
