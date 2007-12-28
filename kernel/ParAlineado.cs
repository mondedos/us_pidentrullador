using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    class ParAlineado
    {
        public Minucia minucia1;
        public Minucia minucia2;

        public int xDestino;
        public int yDestino;
        public int xT;
        public int yT;

        public ParAlineado(Minucia m1, int xT, int yT, Minucia m2, int xDestino, int yDestino)
        {
            this.minucia1 = m1;
            this.minucia2 = m2;
            this.xT = xT;
            this.yT = yT;
            this.xDestino = xDestino;
            this.yDestino = yDestino;
        }
    }
}
