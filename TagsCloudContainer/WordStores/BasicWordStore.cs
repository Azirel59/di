using System;
using System.Collections.Generic;
using TagsCloudContainer.WordProcessors;

namespace TagsCloudContainer.WordStores
{
    public class BasicWordStore : IWordStore
    {
        public IEnumerable<string> Words { get; }

        public IWordStore Process(IWordProcessor processor)
        {
            return new BasicWordStore(processor.Process(Words));
        }

        public BasicWordStore(IEnumerable<string> words)
        {
            Words = words;
        }
    }
}
