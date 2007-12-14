using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    public class Atributos
    {
        public int radioCirculo;

        //colores de minucias
        public Color colorTerminacionFiable;
        public Color colorTerminacionPocoFiable;

        public Color colorBifurcacionFiable;
        public Color colorBifurcacionPocoFiable;

        public Color colorMinuciaNoFiable;
        public Color colorLinea;

        //atributos para el descriptor de textura
        public int longitudLinea;
        public int maxLongitudBuqueda;
        public int[] radiosL;
        public int[] puntosK;
        public double w;

        //atributos para el descriptor de minucia

        static object o = new object();

        static Atributos instancia = null;

        public static Atributos getInstance()
        {
            lock (o)
            {
                if (instancia == null)
                    instancia = new Atributos();
                return instancia;
            }
        }

    }
}
