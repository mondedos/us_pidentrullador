using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    class TerminacionPotencial
    {
        public Point actual;
        public Point prolongacion;

        public TerminacionPotencial(Point actual, Point prolongacion)
        {
            this.actual = actual;
            this.prolongacion = prolongacion;
        }
    }
}
