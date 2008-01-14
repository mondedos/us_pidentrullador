using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    class Funcion
    {
        public static String doubleBien(double num)
        {
            if (num == 1)
                return "1.000";

            else if (num == 0)
                return "0.000";

            else
                return num.ToString();
        }

        public static String intBien(int num)
        {
            if (num < 10)
                return "00" + num.ToString();

            else if (num < 100)
                return "0" + num.ToString();

            else
                return num.ToString();
        }


        public static double anguloEntrePuntos(int x1, int y1, int x2, int y2)
        {
            int difx = x2 - x1;
            int dify = y2 - y1;

            return Math.Asin((double)dify / Math.Sqrt(difx * difx + dify * dify));
        }

        public static int distancia(int x1, int y1, int x2, int y2)
        {
            return (int)(Math.Sqrt(
                                    (Math.Abs(x1 - x2) * Math.Abs(x1 - x2)) 
                                    +
                                    (Math.Abs(y1 - y2) * Math.Abs(y1 - y2))
                                    )
                        );

        }

        public static String recortarDigitos(double num, int numDigitos)
        {
            double factor = Math.Pow(10, numDigitos);
            double res = ((double)((int)(num * factor))) / factor;

            return doubleBien(res);
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