using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    class Funcion
    {
        public static int distancia(int x1, int y1, int x2, int y2)
        {
            return (int)(Math.Sqrt(
                                    (Math.Abs(x1 - x2) * Math.Abs(x1 - x2)) 
                                    +
                                    (Math.Abs(y1 - y2) * Math.Abs(y1 - y2))
                                    )
                        );

        }

        public static bool EsBlanco(Color c)
        {
            return ((c.ToArgb() & 0xFFFFFF) == 0xFFFFFF);
        }

        public static bool EsNegro(Color c)
        {
            return ((c.ToArgb() & 0xFFFFFF) == 0);
        }

        public static bool seSaleDeCoordenadas(int x, int y, int filas, int cols, int margen)
        {
            return (x + margen >= filas    ||
                    y - margen <= 0        ||
                    y + margen >= cols ||
                    x - margen <= 0);
        }

        public static bool hayAlgunoEnEntorno(int x, int y, int[,] matriz, int filas, int cols)
        {
            Atributos atr = Atributos.getInstance();
            int i, j;
            bool enc = false;

            if (matriz[x, y] == 0)
            {
                for (i = x - atr.tamEntornoPunto / 2; i <= x + atr.tamEntornoPunto / 2 && !enc; i++)
                {
                    for (j = y - atr.tamEntornoPunto / 2; j <= y + atr.tamEntornoPunto / 2 && !enc; j++)
                    {
                        enc = matriz[i, j] == 1;
                    }
                }
            }
            else
            {
                enc = true;
            }

            return enc;
        }
    }
}