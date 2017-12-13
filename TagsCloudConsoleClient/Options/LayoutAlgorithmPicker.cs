using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.LayoutAlgorithms;

namespace TagsCloudConsoleClient.Options
{
    public class LayoutAlgorithmPicker: OptionPicker<ILayoutAlgorithm>
    {
        public LayoutAlgorithmPicker(IEnumerable<ILayoutAlgorithm> options) : base(options)
        {
        }
    }
}
