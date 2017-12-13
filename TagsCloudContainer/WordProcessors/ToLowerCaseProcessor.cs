using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.WordProcessors
{
    [Description("Привести слова к нижнему регистру")]
    public class ToLowerCaseProcessor : BasicTransformProcessor
    {
        public ToLowerCaseProcessor() : base(input => input.ToLower())
        {

        }
    }
}
