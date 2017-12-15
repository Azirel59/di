using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.LayoutAlgorithms
{
    public interface ILayoutAlgorithm
    {
        IEnumerable<Rectangle> Layout(Point center, IEnumerable<Size> elements);
    }
}
