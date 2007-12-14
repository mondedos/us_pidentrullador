using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Punto
    {
        int x = 0, y = 0;

        public bool esValido = true;

        public Punto() : this(0, 0) { }

        public Punto(int x, int y)
        {
            this.x = x; this.y = y;
        }

        public override string ToString()
        {
            return "{x="+x+",y="+y+"}";
        }
    }
}
