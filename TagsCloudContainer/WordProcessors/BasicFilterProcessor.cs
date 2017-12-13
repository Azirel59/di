using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.WordStores;

namespace TagsCloudContainer.WordProcessors
{
    public abstract class BasicFilterProcessor: IWordProcessor
    {
        private readonly Func<string, bool> filter;

        public BasicFilterProcessor(Func<string, bool> filter)
        {
            this.filter = filter;
        }

        public IEnumerable<string> Process(IEnumerable<string> word)
        {
            return word.Where(filter);
        }
    }
}
