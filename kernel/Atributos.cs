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
        public Color colorPixelCercano;
        public Color colorCirculo;
        public Color colorCruz;

        public Brush colorRellenoFinPixelCercano;

        //atributos para el descriptor de textura
        public int minPasosAntesDeBuscarPunto;
        public int longitudLinea;
        public int maxLongitudBuqueda;
        public int[] radiosL;
        public int[] puntosK;
        public double w;
        public int tamEntornoPunto;
        public int numEjemplos;

        //atributos para el descriptor de minucia
        public int radioVecinos;

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
