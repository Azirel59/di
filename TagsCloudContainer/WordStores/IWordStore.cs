using System.Collections.Generic;
using TagsCloudContainer.WordProcessors;

namespace TagsCloudContainer.WordStores
{
    public interface IWordStore
    {
        IEnumerable<string> Words { get; }
        IWordStore Process(IWordProcessor processor);
    }
}
