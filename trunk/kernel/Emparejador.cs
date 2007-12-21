using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    public class Emparejador
    {
        public Bitmap[] huellas1;
        public Bitmap[] huellas2;

        List<Minucia> minucias1;
        List<Minucia> minucias2;

        List<ParejaMinucia> parejasMinucias;

        public Emparejador(Bitmap huella1, Bitmap huella2)
        {
            Tratamiento trat1 = new Tratamiento(huella1);
            Tratamiento trat2 = new Tratamiento(huella2);

            huellas1 = trat1.getPasos();
            huellas2 = trat2.getPasos();

            this.minucias1 = trat1.minucias;
            this.minucias2 = trat2.minucias;
            
            // Creamos el set de minucias de todas las posibles parejas
            // no repetidas entre minucias de ambas huellas
            crearParejasMinucias();
            
            // Aplica el algoritmo de emparejado voraz
            emparejar();

            // Escribe los resultados en un fichero
            escribirResultados();
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
            

        }

        void escribirResultados()
        {
            String texto = "";

            texto += "Parejas con descriptores de textura relevantes:\r\n";
            texto += "-----------------------------------------------\r\n";

            foreach (ParejaMinucia pareja in parejasMinucias)
                if (pareja.esDescriptorTexturaRelevante)
                    texto += pareja.ToString() + "\r\n";

            const string ruta = @"resultados.txt";
            System.IO.StreamWriter sw = 
                new System.IO.StreamWriter(ruta, false, System.Text.Encoding.Default);
            sw.WriteLine(texto);
            sw.Close();
        }
    }
}
