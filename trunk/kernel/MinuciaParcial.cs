using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    class MinuciaParcial
    {
        public int x;
        public int y;
        public double teta;

        public Minucia minuciaCentral;
        public Minucia minucia;

        public bool encaja;
        public bool deberiaEncajar;

        /// <summary>
        /// Constructor aplicado cuando queremos guardar la relación de vecindad
        /// entre una minucia y su minucia asociada central
        /// </summary>
        /// <param name="minucia"></param>
        /// <param name="minuciaCentral"></param>
        public MinuciaParcial(Minucia minucia, Minucia minuciaCentral)
        {
            this.minucia = minucia;
            this.minuciaCentral = minuciaCentral;

            this.x = minucia.x;
            this.y = minucia.y;
            this.teta = Funcion.anguloEntrePuntos(minuciaCentral.x, minuciaCentral.y, minucia.x, minucia.y);

            this.encaja = false;
            this.deberiaEncajar = false;
        }

        /// <summary>
        /// Constructor aplicado cuando queremos construir una minucia parcial
        /// aplicándole las coordenadas de una transformada T
        /// </summary>
        /// <param name="minucia"></param>
        /// <param name="minuciaCentral"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="teta"></param>
        public MinuciaParcial(Minucia minucia, Minucia minuciaCentral, int x, int y, double teta)
        {
            this.minucia = minucia;
            this.minuciaCentral = minuciaCentral;

            this.x = x;
            this.y = y;
            this.teta = teta;

            this.encaja = false;
            this.deberiaEncajar = false;
        }
    }
}
