using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.WordProcessors
{
    [Description("Удалить крайние пробелы")]
    public class TrimSpacesProcessor : BasicTransformProcessor
    {
        public TrimSpacesProcessor() : base(input => input.Trim())
        {

        }
    }
}
