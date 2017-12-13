using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudConsoleClient.Options
{
    public struct OptionInfo<T>
    {
        public string Description { get; set; }
        public T Implementation { get; set; }
    }
}
