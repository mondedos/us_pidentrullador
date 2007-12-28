using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    public class Emparejador
    {
        Bitmap[] huellas1;
        Bitmap[] huellas2;

        List<Minucia> minucias1;
        List<Minucia> minucias2;

        List<ParejaMinucia> parejasMinucias;
        GreedyMatch gm;

        List<Correspondencia> correspondencias;

        // Las dos huellas unidas en una misma imagen
        public Bitmap[] huellas;

        public Emparejador(Bitmap huella1, Bitmap huella2)
        {
            Tratamiento trat1 = new Tratamiento(huella1);
            Tratamiento trat2 = new Tratamiento(huella2);

            huellas1 = trat1.getPasos();
            huellas2 = trat2.getPasos();

            this.correspondencias = new List<Correspondencia>();

            this.minucias1 = trat1.minucias;
            this.minucias2 = trat2.minucias;
            
            // Creamos el set de minucias de todas las posibles parejas
            // no repetidas entre minucias de ambas huellas
            crearParejasMinucias();
            
            // Aplica el algoritmo de emparejado voraz
            emparejar();

            // Escribe los resultados en un fichero
            escribirResultados();
            imprimirNormalizadas();

            // Aplicar resultado final a huella
            mezclarHuellas();
        }

        void crearParejasMinucias()
        {         
            this.parejasMinucias = new List<ParejaMinucia>();

            foreach (Minucia m1 in minucias1)
                foreach (Minucia m2 in minucias2)
                    parejasMinucias.Add(new ParejaMinucia(m1, m2));  
        }

        void emparejar()
        {
            gm = new GreedyMatch(minucias1, minucias2, parejasMinucias);
            this.correspondencias = gm.correspondencias;
        }

        void imprimirNormalizadas()
        {

            String texto2 = "\r\n\r\n";

            texto2 += "Parejas de minucias normalizadas en orden:\r\n";
            texto2 += "------------------------------------------\r\n";

            foreach (ParejaMinuciaNormalizada pareja in gm.vectorParejas)
                if (pareja.sn != 0)
                    texto2 += pareja.ToString() + "\r\n";

            const string ruta2 = @"resultados.txt";
            System.IO.StreamWriter sw2 =
                new System.IO.StreamWriter(ruta2, true, System.Text.Encoding.Default);
            sw2.WriteLine(texto2);
            sw2.Close();
        }

        void escribirResultados()
        {
            String texto = "";

            texto += "Parejas con descriptores de textura relevantes:\r\n";
            texto += "-----------------------------------------------\r\n";

            foreach (ParejaMinucia pareja in parejasMinucias)
                if (pareja.esDescriptorTexturaRelevante)
                    texto += pareja.ToString() + "\r\n";

            texto += "\r\n";
            texto += "--------------------------------------\r\n";
            texto += "Parejas de correspondencias obtenidas:\r\n";
            texto += "--------------------------------------\r\n";

            foreach (Correspondencia correspondencia in correspondencias)
                texto += correspondencia.minucia1 + "\t" + correspondencia.minucia2 + "\r\n";

            const string ruta = @"resultados.txt";
            System.IO.StreamWriter sw = 
                new System.IO.StreamWriter(ruta, false, System.Text.Encoding.Default);
            sw.WriteLine(texto);
            sw.Close();
        }

        void mezclarHuellas()
        {
            huellas = new Bitmap[Tratamiento.totalPasos + 1];
            for (int i = 0; i < Tratamiento.totalPasos; i++)
            {
                mezclar(i, huellas1[i], huellas2[i]);
            }

            
            mezclar(Tratamiento.totalPasos, 
                huellas1[Tratamiento.muestraTodasMinucias], 
                huellas2[Tratamiento.muestraTodasMinucias]);

            Graphics g = Graphics.FromImage(huellas[Tratamiento.totalPasos]);

            foreach (Correspondencia c in correspondencias)
                if (c.minucia1.fiabilidad == Minucia.Fiable &&
                    c.minucia2.fiabilidad == Minucia.Fiable)
                        g.DrawLine(new Pen(Color.Red), c.minucia1.x, c.minucia1.y, 
                                               c.minucia2.x + huellas1[0].Width, c.minucia2.y);
                
        }

        void mezclar(int indice, Bitmap huella1, Bitmap huella2)
        {
            huellas[indice] = new Bitmap(huella1.Width + huella2.Width,huella1.Height);

            for (int y=0; y<huellas[indice].Height; y++)
            {
                for (int x=0; x<huellas[indice].Width; x++)
                {
                    if (x >= huella1.Width)
                        huellas[indice].SetPixel(x, y, huella2.GetPixel(x - huella1.Width, y));
                    else
                        huellas[indice].SetPixel(x, y, huella1.GetPixel(x, y));
                }
            }
        }
    }
}
