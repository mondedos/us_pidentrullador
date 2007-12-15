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
        public double anguloGlobal = 0;
        public int numPuntos = 0;

        public Punto[] puntos;

        /// <summary>
        /// Constructor de la clase. Está desacoplado al máximo de la clase Atributos, esta clase
        /// ha sido consultada para crear el circulo a la hora de llamar al constructor
        /// </summary>
        /// <param name="indice">Círculo i-ésimo relativo a la minucia a la que él está asociado</param>
        /// <param name="anguloGlobal">Ángulo inferior de la minucia con respecto al origen</param>
        /// <param name="radio">Radio del círculo actual. Viene determinado por el usuario</param>
        /// <param name="numPuntos">Número de puntos que contendrá el círculo actual</param>
        public Circulo(int indice, double anguloGlobal, int radio, int numPuntos)
        {
            Atributos atr = Atributos.getInstance();

            this.indice = indice;
            this.anguloGlobal = anguloGlobal;
            this.radio = radio;
            this.numPuntos = numPuntos;

            this.puntos = new Punto[numPuntos];

            // Ahora buscaremos los puntos asociados a cada círculo
            for (int i = 0; i < numPuntos; i++)
            {
                double anguloParcial = get_teta_polar_punto(i);
                double x = anguloParcial * Math.Sin((double)radio);
                double y = anguloParcial * Math.Cos((double)radio);

                puntos[i] = new Punto((int)x, (int)y, radio, anguloGlobal, anguloParcial);
            }
        }

        /// <summary>
        /// Using the minutia as origin
        /// and the direction of the minutia as the positive direction
        /// of x axis of polar coordinate system, the coordinate of the kth
        /// sampling point on the lth circle is defined as
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public double get_teta_polar_punto(int indice)
        {
            return (Math.PI * 2 * indice) / numPuntos;
        }
        /// <summary>
        /// the orientation at the sampling point is alfa-l,k(−pi/2l,k <pi/2)
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public double get_relative_orientation(int indice)
        {
            double angulo_punto_k = get_teta_polar_punto(indice);
            return Circulo.lambda(angulo_punto_k - anguloGlobal);
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
