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

        public Punto() : this(0, 0, null) { }

        public Punto(int x, int y, Minucia minucia)
        {
            Atributos atr = Atributos.getInstance();

            // coordenadas del punto en cartesiano
            this.x = x; 
            this.y = y;

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
    }
}
