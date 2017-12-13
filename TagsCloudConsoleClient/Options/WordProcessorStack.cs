using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.WordProcessors;

namespace TagsCloudConsoleClient.Options
{
    public class WordProcessorStack
    {
        public IEnumerable<IWordProcessor> WordProcessors { get; }

        public WordProcessorStack(IEnumerable<IWordProcessor> processors)
        {
            WordProcessors = processors;
        }
    }
}
