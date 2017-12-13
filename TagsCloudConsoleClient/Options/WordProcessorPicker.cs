using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.WordProcessors;

namespace TagsCloudConsoleClient.Options
{
    public class WordProcessorPicker: OptionPicker<IWordProcessor>
    {
        public WordProcessorPicker(IEnumerable<IWordProcessor> options) : base(options)
        {
        }
    }
}
