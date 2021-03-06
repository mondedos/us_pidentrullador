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

        public Minucia minucia = null;
        public Punto[] puntos;

        /// <summary>
        /// Constructor de la clase. Está desacoplado al máximo de la clase Atributos, esta clase
        /// ha sido consultada para crear el circulo a la hora de llamar al constructor
        /// </summary>
        /// <param name="indice">Círculo i-ésimo relativo a la minucia a la que él está asociado</param>
        /// <param name="anguloGlobal">Ángulo inferior de la minucia con respecto al origen</param>
        /// <param name="radio">Radio del círculo actual. Viene determinado por el usuario</param>
        /// <param name="numPuntos">Número de puntos que contendrá el círculo actual</param>
        public Circulo(int indice, double anguloGlobal, int radio, int numPuntos, Minucia minucia)
        {
            Atributos atr = Atributos.getInstance();

            this.indice = indice;
            this.anguloGlobal = anguloGlobal;
            this.radio = radio;
            this.numPuntos = numPuntos;

            // minucia a la que está asociada el citado punto
            this.minucia = minucia;

            // array donde guardaremos los puntos asociados al citado círculo
            this.puntos = new Punto[numPuntos];

            // Ahora buscaremos los puntos asociados a cada círculo
            for (int i = 0; i < numPuntos; i++)
            {
                // angulo del punto con respecto al centro de la minucia
                double anguloParcial = get_teta_polar_punto(i);

                // coordenadas globales del nuevo punto
                int nuevox = (int)(radio * Math.Sin((double)anguloParcial)) + minucia.x;
                int nuevoy = (int)(radio * Math.Cos((double)anguloParcial)) + minucia.y;

                puntos[i] = new Punto(nuevox, nuevoy, anguloParcial, minucia);
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
    }
}
