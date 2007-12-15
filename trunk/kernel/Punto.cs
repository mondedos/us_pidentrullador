using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Punto
    {
        public int x = 0;
        public int y = 0;
        public int radio = 0;
        public double anguloGlobal = 0;
        public double anguloParcial = 0;

        public bool esValido = false;

        public Punto() : this(0, 0, 0, 0, 0) { }

        public Punto(int x, int y, int radio, double anguloGlobal, double anguloParcial)
        {
            Atributos atr = Atributos.getInstance();

            // coordenadas del punto en cartesiano
            this.x = x; 
            this.y = y;

            // angulo de la minucia a la que le corresponde al punto
            this.anguloGlobal = anguloGlobal;

            // coordenadas polares del punto del punto con respecto a la minucia
            this.radio = radio;
            this.anguloParcial = anguloParcial;

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
    }
}
