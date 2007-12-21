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

        public int minPasosAntesDeBuscarPunto;
        public int longitudLinea;
        public int maxLongitudBuqueda;
        public int[] radiosL;
        public int[] puntosK;
        public double w;
        public int minPorcentajeValidos;
        public int tamEntornoPunto;
        public int numEjemplos;
        public int radioVecinos;
        public double maxDistancia;
        public int radioEncaje;
        public double umbralAngulo;

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
