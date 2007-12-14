using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Circulo
    {
        //es el circulo i-esimo del array de circulos en una minucia
        public int indice = 0;
        public int radio = 0;

        public Punto[] puntos;

        public Circulo(int indice)
        {
            Atributos atr = Atributos.getInstance();

            this.indice = indice;
            this.radio = atr.radiosL[indice];
            this.puntos = new Punto[atr.puntosK[indice]];

        }
        /// <summary>
        /// Using the minutia as origin
        ///and the direction of the minutia as the positive direction
        ///of x axis of polar coordinate system, the coordinate of the kth
        ///sampling point on the lth circle is defined as
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public double get_fi_polar_punto(int indice)
        {
            return (Math.PI * 2 * indice) / puntos.Length;
        }
    }
}
