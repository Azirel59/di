using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.WordStores;

namespace TagsCloudContainer.WordProcessors
{
    public interface IWordProcessor
    {
        IEnumerable<string> Process(IEnumerable<string> words);
    }
}
