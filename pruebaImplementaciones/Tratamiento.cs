using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace pruebaImplementaciones
{
    class Tratamiento
    {
        Bitmap huella;
        int[,] matriz;
        Atributos atr;
        Bitmap[] pasos;
        int filas, cols;

        static int busquedaTerminaciones = 0;
        static int busquedaBifurcaciones = 1;
        static int totalPasos = 2;

        public Tratamiento(Bitmap huella, Atributos atr)
        {
            this.pasos = new Bitmap[totalPasos];

            this.huella = new Bitmap(huella);
            this.matriz = Adaptador.Adaptar(huella);
            this.atr = atr;

            this.filas = huella.Width;
            this.cols = huella.Height;

            buscarTerminaciones();
            buscarBifurcaciones();
        }

        void buscarTerminaciones()
        {
            int i, j;
            
            this.pasos[busquedaTerminaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[busquedaTerminaciones]);

            for (i = 1; i < filas-1; i++)
            {
                for (j = 1; j < cols-1; j++)
                {
                    if (matriz[i,j]==1 && buscarEnEntorno(i, j, matriz, 2))
                    {
                        g.DrawEllipse(new Pen(Color.Blue),
                            i - atr.radioCirculo/2, j - atr.radioCirculo/2, atr.radioCirculo, atr.radioCirculo);
                    }
                }
            }
        }

        void buscarBifurcaciones()
        {
            int i, j;

            this.pasos[busquedaBifurcaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[busquedaBifurcaciones]);

            for (i = 1; i < filas - 1; i++)
            {
                for (j = 1; j < cols - 1; j++)
                {
                    if (matriz[i, j] == 1 && buscarEnEntorno(i, j, matriz, 4))
                        //&& numCompConexMayUno(i,j,matriz))
                    {
                        g.DrawEllipse(new Pen(Color.Red),
                            i - atr.radioCirculo / 2, j - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
                    }
                }
            }
        }

        bool numCompConexMayUno(int x, int y, int[,] matriz)
        {
            int[,] nuevo = new int[,]{ { matriz[x-1,y-1], matriz[x-1,y], matriz[x-1,y+1]},
                                       { matriz[x,y-1], 0, matriz[x,y+1]},
                                       { matriz[x+1,y-1], matriz[x+1,y], matriz[x+1,y+1]} };

            int i, j;
            bool cambio = true;

            while (cambio)
            {
                for (i = 0; i <= 2; i++)
                {
                    for (j = 0; j <= 2; j++)
                    {
                        
                        if (nuevo[i, j] == 0 && nuevo[i, j + 1] == 0 && nuevo[i + 1, j] == 0)
                        {
                            nuevo[i, j] = 1;
                            cambio = true;
                        }

                        if (nuevo[i, j] == 1 && nuevo[i, j + 1] == 0 && nuevo[i + 1, j] == 0 && 
                            nuevo[i + 1, j + 1] == 0 && true)
                            nuevo[i, j] = 0;

                    }
                }
            }

            return true;
        }

        bool buscarEnEntorno(int x, int y, int[,] matriz, int numPuntos)
        {
            bool b = false;
            int i, j;
            int contador = 0;

            for (i = x - 1; i <= x + 1; i++)
            {
                for (j = y - 1; j <= y + 1; j++)
                {
                    contador += matriz[i, j];
                }
            }

            if (contador==numPuntos)
                b = true;

            return b;
        }

        public Bitmap getBitmapFinal()
        {
            return pasos[busquedaBifurcaciones];
        }

        public Bitmap[] getPasos()
        {
            return pasos;
        }
    }
}
