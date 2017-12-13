using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.Options
{
    public struct TagCloudOptions
    {
        public Font Font { get; set; }
        public int MinimumFontSize { get; set; }
        public int MaximumFontSize { get; set; }
    }
}
