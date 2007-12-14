using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Minucia
    {
        public int x = 0, y = 0;
        public double angulo = 0.0;

        Fiabilidad clasificacion = Fiabilidad.NoFiable;
        Tipo topologia = Tipo.Terminacion;

        Circulo[] circulos = null;

        public Descriptor_base descriptor;

        public Minucia() : this(0, 0) { }
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

            //calculamos el tamaño del conjunto descriptor base donde cada elemento del conjunto es (angulo, frecuencia)
            int total = 0;
            foreach (Circulo cir in circulos)
            {
                total += cir.puntos.Length;
            }

            descriptor = new Descriptor_base(total);
        }
    }
    public enum Fiabilidad
    {
        NoFiable,
        Fiable,
        PocoFiable        
    }
    public enum Tipo
    {
        Terminacion,
        Bifurcacion
    }
}
