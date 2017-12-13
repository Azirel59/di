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
        Rectangle[] Layout(Point center, Size[] elements);
    }
}
