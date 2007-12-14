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

            descriptor = new Descriptor_base();
        }

        public void Calcular_Conjunto_Descriptor_Base()
        {
            foreach (Circulo c in circulos)
            {
                descriptor.Insertar_puntos_circulos(c);
            }
        }

        public double similitud(Minucia q)
        {
            Atributos atr = Atributos.getInstance();
            double diff = 1 - atr.w;
            double w = atr.w;


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
