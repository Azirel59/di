﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization;

namespace TagsCloudContainer.LayoutAlgorithms
{
    [Description("Раскладка в круг методом \"Спираль + упаковка\"")]
    public class CircularLayoutAlgorithm: ILayoutAlgorithm
    {
        public IEnumerable<Rectangle> Layout(Point center, IEnumerable<Size> elements)
        {
            var layouter = new CircularCloudLayouter(center);
            foreach (var element in elements)
            {
                layouter.PutNextRectangle(element);
            }
            return layouter.WordRectangles;
        }
    }
}
