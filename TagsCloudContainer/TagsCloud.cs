using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.LayoutAlgorithms;
using TagsCloudContainer.WordProcessors;
using TagsCloudContainer.WordStores;

namespace TagsCloudContainer
{
    public class TagsCloud
    {
        private readonly ILayoutAlgorithm layoutAlgorithm;
        private readonly IWordStore words;
        private readonly IWordProcessor[] wordProcessors;

        public TagsCloud(ILayoutAlgorithm layoutAlgorithm, IWordStore words, IWordProcessor[] wordProcessors)
        {
            this.layoutAlgorithm = layoutAlgorithm;
            this.words = words;
            this.wordProcessors = wordProcessors;
        }

        public Image CreateTagsCloud()
        {
            
            throw new NotImplementedException();
        }
    }
}
