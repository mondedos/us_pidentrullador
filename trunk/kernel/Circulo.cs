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

        Minucia _minucia;

        public Circulo(Minucia minucia ,int indice)
        {
            Atributos atr = Atributos.getInstance();


            _minucia = minucia;
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
        /// <summary>
        /// the orientation at the sampling point is alfa-l,k(−pi/2l,k <pi/2)
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public double get_relative_orientation(int indice)
        {
            double angulo_punto_k = get_fi_polar_punto(indice);
            return Circulo.lambda(angulo_punto_k - _minucia.angulo);
        }
        /// <summary>
        /// Función de lambda relativa
        /// </summary>
        /// <param name="alfa"></param>
        /// <returns></returns>
        public static double lambda(double alfa)
        {
            double pimedio = Math.PI / 2;

            if (alfa >= pimedio)
                return alfa - Math.PI;
            else if (alfa < -pimedio)
                return alfa + Math.PI;
            else
                return alfa;
        }
    }
}
