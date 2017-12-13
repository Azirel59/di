using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer
{
    public class WordData
    {
        public string Word { get; set; }
        public long Frequency { get; set; }
        public Rectangle Rectangle { get; set; }
        public int FontSize { get; set; }
    }
}
