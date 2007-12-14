using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    class BifurcacionPotencial
    {
        public Point actual;
        public Point [] prolongaciones;

        public BifurcacionPotencial(Point actual, Point [] prolongaciones)
        {
            this.actual = actual;
            this.prolongaciones = prolongaciones;
        }
    }
}
