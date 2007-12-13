using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Minucia
    {
        public int x = 0, y = 0;
        double angulo = 0.0;

        Circulo[] circulos = null;

        public Minucia(int x, int y)
        {
            Atributos atr = Atributos.getInstance();

            this.x = x;
            this.y = y;

            //calcular angulo

            circulos = new Circulo[atr.radiosL.Length];
        }
    }
}
