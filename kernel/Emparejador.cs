using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    public class Emparejador
    {

        public static String ruta = "resultados.txt";

        Bitmap[] huellas1;
        Bitmap[] huellas2;

        List<Minucia> minucias1;
        List<Minucia> minucias2;

        List<ParejaMinucia> parejasMinucias;
        GreedyMatch gm;

        List<Correspondencia> correspondencias;

        public Bitmap[] huellas1Final;
        public Bitmap[] huellas2Final;

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

            // Redistribución de los arrays con respecto al tratamiento anterior
            redistribuirArrays();

            // Mostrar par más fiable
            mostrarParMasFiable();

            // Aplicar resultado final a huella
            ponerPasoUltimo();
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

        void escribirResultados()
        {
            String texto = "";

            texto += "-----------------------------------------------\r\n";
            texto += "Parejas con descriptores de textura relevantes:\r\n";
            texto += "-----------------------------------------------\r\n";

            foreach (ParejaMinucia pareja in parejasMinucias)
                if (pareja.esDescriptorTexturaRelevante)
                    texto += pareja.ToString() + "\r\n";

            System.IO.StreamWriter sw =
                new System.IO.StreamWriter(ruta, false, System.Text.Encoding.Default);
            sw.WriteLine(texto);
            sw.Close();

            ///////////////////////

            String texto2 = "\r\n\r\n";

            texto2 += "------------------------------------------\r\n";
            texto2 += "Parejas de minucias normalizadas en orden:\r\n";
            texto2 += "------------------------------------------\r\n";

            foreach (ParejaMinuciaNormalizada pareja in gm.vectorParejas)
                if (pareja.sn != 0)
                    texto2 += pareja.ToString() + "\r\n";

            System.IO.StreamWriter sw2 =
                new System.IO.StreamWriter(ruta, true, System.Text.Encoding.Default);
            sw2.WriteLine(texto2);
            sw2.Close();

            //////////////////////////

            String texto3 = "\r\n\r\n";

            texto3 += "------------------------------------------------------------\r\n";
            texto3 += "Pareja escogida para escoger parámetros de transformación T:\r\n";
            texto3 += "------------------------------------------------------------\r\n";

            texto3 += "\r\n";

            texto3 += gm.inicial.ToString() + "\r\n";
            texto3 += gm.inicial.pm.minucia1.ToString() + "\r\n";
            texto3 += gm.inicial.pm.minucia2.ToString() + "\r\n";

            texto3 += "\r\n";
            texto3 += "Transformación T: <dx:" + gm.transformacionT.difx + ", dy:" + gm.transformacionT.dify + ", da:" + gm.transformacionT.difa + ">";

            System.IO.StreamWriter sw3 =
                new System.IO.StreamWriter(ruta, true, System.Text.Encoding.Default);
            sw3.WriteLine(texto3);
            sw3.Close();

            //////////////////////////

            String texto4 = "";

            texto4 += "\r\n";

            texto4 += "--------------------------------------\r\n";
            texto4 += "Parejas de correspondencias obtenidas:\r\n";
            texto4 += "--------------------------------------\r\n";

            foreach (Correspondencia correspondencia in correspondencias)
                texto4 += correspondencia.minucia1 + " <---> " + correspondencia.minucia2 + "\r\n";

            System.IO.StreamWriter sw4 =
                new System.IO.StreamWriter(ruta, true, System.Text.Encoding.Default);
            sw4.WriteLine(texto4);
            sw4.Close();

            /////////////////////////
        }

        void redistribuirArrays()
        {
            huellas1Final = new Bitmap[Tratamiento.totalPasos + 2];
            huellas2Final = new Bitmap[Tratamiento.totalPasos + 2];
            for (int i = 0; i < Tratamiento.totalPasos; i++)
            {
                huellas1Final[i] = huellas1[i];
                huellas2Final[i] = huellas2[i];
            }
        }

        void mostrarParMasFiable()
        {
            Atributos atr = Atributos.getInstance();

            huellas1Final[Tratamiento.totalPasos] = new Bitmap(huellas1[0]);
            huellas2Final[Tratamiento.totalPasos] = new Bitmap(huellas2[0]);

            Graphics g1 = Graphics.FromImage(huellas1Final[Tratamiento.totalPasos]);
            Graphics g2 = Graphics.FromImage(huellas2Final[Tratamiento.totalPasos]);

            Minucia masFiable1 = gm.minuciaMasFiable1;
            Minucia masFiable2 = gm.minuciaMasFiable2;

            for (int i = 0; i < atr.radioCirculo*4; i+=4)
            {
                g1.DrawEllipse(new Pen(Color.Green),
                    masFiable1.x - atr.radioCirculo - i/2, masFiable1.y - atr.radioCirculo - i/2,
                    atr.radioCirculo*2 + i, atr.radioCirculo*2 + i);
            }

            for (int i = 0; i < atr.radioCirculo*4; i+=4)
            {
                g2.DrawEllipse(new Pen(Color.Green),
                    masFiable2.x - atr.radioCirculo - i / 2, masFiable2.y - atr.radioCirculo - i / 2,
                    atr.radioCirculo*2 + i, atr.radioCirculo*2 + i);
            }
        }

        void ponerPasoUltimo()
        {
            Atributos atr = Atributos.getInstance();

            huellas1Final[Tratamiento.totalPasos+1] = new Bitmap(huellas1[Tratamiento.muestraTodasMinucias]);
            huellas2Final[Tratamiento.totalPasos+1] = new Bitmap(huellas2[Tratamiento.muestraTodasMinucias]);

            Graphics g1 = Graphics.FromImage(huellas1Final[Tratamiento.totalPasos+1]);
            Graphics g2 = Graphics.FromImage(huellas2Final[Tratamiento.totalPasos+1]);

            /*
            int numPosiciones = atr.numEjemplos * 2;
            Correspondencia[] arrayCorrespondencia = gm.correspondencias.ToArray();
            Correspondencia [] elegidas = new Correspondencia[numPosiciones];
            Random r = new Random();

            int k = 0;
            while (k < numPosiciones)
            {
                int nuevoPos = r.Next(arrayCorrespondencia.Length);
                Correspondencia c = arrayCorrespondencia[nuevoPos];

                if (c.minucia1.fiabilidad == Minucia.Fiable &&
                    c.minucia2.fiabilidad == Minucia.Fiable)
                {
                    elegidas[k++] = c;
                }
            }

            foreach (Correspondencia c in elegidas)
            {
                g1.DrawLine(new Pen(Color.Red), c.minucia1.x, c.minucia1.y,
                                       c.minucia2.x + 512, c.minucia2.y);
                g2.DrawLine(new Pen(Color.Red), c.minucia1.x - 512, c.minucia1.y,
                                       c.minucia2.x, c.minucia2.y);
            }
            */

            bool flag = false;

            foreach (Correspondencia c in gm.correspondencias)
            {
                if (c.minucia1.fiabilidad == Minucia.Fiable &&
                    c.minucia2.fiabilidad == Minucia.Fiable)
                {
                    flag = true;

                    g1.DrawLine(new Pen(Color.Red), c.minucia1.x, c.minucia1.y,
                                       c.minucia2.x + 512, c.minucia2.y);
                    g2.DrawLine(new Pen(Color.Red), c.minucia1.x - 512, c.minucia1.y,
                                           c.minucia2.x, c.minucia2.y);
                }
            }

            if (!flag)
            {
                foreach (Correspondencia c in gm.correspondencias)
                {
                    g1.DrawLine(new Pen(Color.Fuchsia), c.minucia1.x, c.minucia1.y,
                                       c.minucia2.x + 512, c.minucia2.y);
                    g2.DrawLine(new Pen(Color.Fuchsia), c.minucia1.x - 512, c.minucia1.y,
                                           c.minucia2.x, c.minucia2.y);
                }
            }
        }
    }
}
