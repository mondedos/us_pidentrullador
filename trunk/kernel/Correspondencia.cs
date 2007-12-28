using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Correspondencia
    {
        public Minucia minucia1;
        public Minucia minucia2;

        public Correspondencia(Minucia minucia1, Minucia minucia2)
        {
            this.minucia1 = minucia1;
            this.minucia2 = minucia2;
        }
    }
}
