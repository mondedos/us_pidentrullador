using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Minucia
    {
        public int x = 0, y = 0;
        double angulo = 0.0;

        Fiabilidad clasificacion = Fiabilidad.NoFiable;
        Circulo[] circulos = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">i filas</param>
        /// <param name="y">j columnas</param>
        public Minucia(int x, int y)
        {
            Atributos atr = Atributos.getInstance();

            this.x = x;
            this.y = y;

            //calcular angulo
            angulo = Math.Asin((double)y / Math.Sqrt(x * x + y * y));

            circulos = new Circulo[atr.radiosL.Length];
        }
    }
    public enum Fiabilidad
    {
        NoFiable,
        Fiable,
        PocoFiable        
    }
}
