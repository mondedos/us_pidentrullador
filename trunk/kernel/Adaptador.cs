using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    class Adaptador
    {
        /// <summary>
        /// Adapta un bitmap a una matriz de 0 (blanco) y 1 (negro)
        /// </summary>
        /// <param name="huella"></param>
        /// <returns></returns>
        public static int[,] Adaptar(Bitmap huella)
        {
            int[,] matriz = new int[huella.Width, huella.Height];
            int i, j;

            for (i = 0; i < huella.Width; i++)
            {
                for (j = 0; j < huella.Height; j++)
                {
                    if (Funcion.EsBlanco(huella.GetPixel(i, j)))
                        matriz[i, j] = 0;
                    else
                        matriz[i, j] = 1;
                }
            }

            return matriz;
        }

        public static Bitmap Desadaptar(int [,] matriz)
        {
            Bitmap huella = new Bitmap(matriz.Length, matriz.Length);
            int i, j;

            for (i = 0; i < huella.Width; i++)
            {
                for (j = 0; j < huella.Height; j++)
                {
                    if (matriz[i, j] == 1)
                        huella.SetPixel(i, j, Color.White);
                    else
                        huella.SetPixel(i, j, Color.Black);
                }
            }

            return huella;
        }
    }
}
