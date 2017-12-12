using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.WordProcessors
{
    public class BasicTransformProcessor: IWordProcessor
    {
        private readonly Func<string, string> transform;

        public BasicTransformProcessor(Func<string, string> transform)
        {
            this.transform = transform;
        }

        public IEnumerable<string> Process(IEnumerable<string> words)
        {
            return words.Select(transform);
        }
    }
}
