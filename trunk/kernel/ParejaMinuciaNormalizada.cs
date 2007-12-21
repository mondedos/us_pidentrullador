using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class ParejaMinuciaNormalizada : IComparable<ParejaMinuciaNormalizada>
    {
        public int i;               // 0
        public int j;               // 1
        public double sn;           // 2
        public ParejaMinucia pm;

        public ParejaMinuciaNormalizada(ParejaMinucia pm, int i, int j, double sn)
        {
            this.pm = pm;
            this.i = i;
            this.j = j;
            this.sn = sn;
        }

        public int CompareTo(ParejaMinuciaNormalizada pmn)
        {
            if (this.sn > pmn.sn)
                return 1;

            else if (this.sn < pmn.sn)
                return -1;

            else
                return 0;
        }

        public override string ToString()
        {
            return "sn<" + Funcion.recortarDigitos(sn, 3) + ">\t-\t<" + i + "," + j + ">";
        }
    }
}
