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
        public List<Minucia> vecinos;

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

            vecinos = new List<Minucia>();
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

            return 0;
        }

        public double parecidos_en_vecinos(Minucia q)
        {
            double num = (vecinos.Count + 1) * (q.vecinos.Count + 1);
            return 0;
        }
        /// <summary>
        /// To determine the order in which to insert correspondences,
        /// a normalized similarity degree sn between two minutiae is
        /// defined based on similarity degree s:
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public double Sn(Minucia q)
        {
            int size = vecinos.Count + q.vecinos.Count + 1;
            double sij = s(q);
            double num = sij * size;

            double sumi = 0;

            foreach (Minucia k in vecinos)
            {
                sumi += k.s(q);
            }

            double sumj = 0;

            foreach (Minucia k in q.vecinos)
            {
                sumj += (s(k) - sij);
            }

            return num / (sumi + sumj);
        }
        /// <summary>
        /// s(pi,qi)
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private double s(Minucia q)
        {
            return 0;
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
