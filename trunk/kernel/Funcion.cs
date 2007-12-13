using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    class Funcion
    {
        public static bool EsBlanco(Color c)
        {
            return ((c.ToArgb() & 0xFFFFFF) == 0xFFFFFF);
        }

        public static bool EsNegro(Color c)
        {
            return ((c.ToArgb() & 0xFFFFFF) == 0);
        }

    }
}
