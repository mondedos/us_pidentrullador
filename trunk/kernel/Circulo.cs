using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Circulo
    {
        //es el circulo i-esimo del array de circulos en una minucia
        int indice = 0;
        int radio = 0;

        Punto[] puntos;

        public Circulo(int indice)
        {
            Atributos atr = Atributos.getInstance();

            this.indice = indice;
            this.radio = atr.radiosL[indice];
            this.puntos = new Punto[atr.puntosK[indice]];
            
        }
    }
}
