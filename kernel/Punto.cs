using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Punto
    {
        public int x = 0;
        public int y = 0;

        public Minucia minucia = null;
        public bool esValido = false;

        public const double valorErroneo = -1000;

        public double frecuencia;
        public double orientacionRelativa;

        public Punto() : this(0, 0, 0, null) { }

        public Punto(int x, int y, double anguloParcial, Minucia minucia)
        {
            Atributos atr = Atributos.getInstance();            

            // coordenadas del punto en cartesiano
            this.x = x; 
            this.y = y;

            double anguloParcialEnRango = meterEnRango(anguloParcial);

            // Datos válidos para el cálculo del futuro descriptor de textura
            if (anguloParcialEnRango != 0)
                this.frecuencia = (double)1 / anguloParcialEnRango;
            else
                this.frecuencia = Double.MaxValue;

            this.orientacionRelativa = get_relative_orientation(anguloParcialEnRango, minucia.angulo);

            // minucia a la que está asociada el citado punto
            this.minucia = minucia;

            Matriz m = Matriz.getInstance();

            if (!Funcion.seSaleDeCoordenadas(x, y, m.filas, m.cols, atr.tamEntornoPunto/2) && 
                 Funcion.hayAlgunoEnEntorno(x,y,m.matriz,m.filas,m.cols))
            {
                this.esValido = true;
            }
        }

        public override string ToString()
        {
            return "{x="+x+",y="+y+"}";
        }

        /// QUEDA COMPROBAR QUE ESTÉN BIEN COGIDOS LOS ÁNGULOS
        /// <summary>
        /// Coloca el ángulo en el rango [-PI/2,PI/2)
        /// </summary>
        /// <param name="anguloParcial"></param>
        /// <returns></returns>
        public double meterEnRango(double anguloParcial)
        {
            double pimedio = Math.PI / (double)2;
            double ang = 0;

            // primer cuadrante [0,PI/2)
            if (anguloParcial < pimedio)
                ang = anguloParcial;

            // segundo cuadrante [PI/2,PI)
            else if (anguloParcial >= pimedio && anguloParcial < Math.PI)
                ang = anguloParcial - Math.PI;

            // tercer cuadrante [PI, 3PI/2)
            else if (anguloParcial >= Math.PI && anguloParcial < 3 * pimedio)
                ang = anguloParcial - Math.PI;

            else if (anguloParcial >= 3 * pimedio)
                ang = anguloParcial - 2*Math.PI;

            return ang;
        }

        /// <summary>
        /// the orientation at the sampling point is alfa-l,k(−pi/2l,k <pi/2)
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public static double get_relative_orientation(double anguloParcialEnRango, double anguloGlobal)
        {
            return Punto.lambda(anguloParcialEnRango - anguloGlobal);
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
