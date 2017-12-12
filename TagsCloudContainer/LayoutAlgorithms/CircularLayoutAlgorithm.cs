using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization;

namespace TagsCloudContainer.LayoutAlgorithms
{
    public class CircularLayoutAlgorithm: ILayoutAlgorithm
    {
        private readonly CircularCloudLayouter layouter;

        public CircularLayoutAlgorithm(CircularCloudLayouter layouter)
        {
            this.layouter = layouter;
        }

        public Rectangle[] Layout(Size[] elements)
        {
            foreach (var element in elements)
            {
                layouter.PutNextRectangle(element);
            }
            return layouter.WordRectangles.ToArray();
        }
    }
}
