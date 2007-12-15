using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Minucia
    {
        // coordenadas y ángulo inferior con respecto al origne
        public int x = 0, y = 0;
        public double angulo = 0.0;

        // Indicaremos si es fiable, poco fiable o no fiable
        public int fiabilidad;

        // Indicaremos si es terminación o bifurcación
        public int tipo;

        // Círculos de puntos concéntricos en torno a la minucia
        Circulo[] circulos = null;

        public List<Minucia> vecinos;
        public Descriptor_base descriptor;

        // Pongo esto como entero porque los enum no me dejaba declararlos estáticos
        // y tengo que referenciarlos desde fuera a la hora de llamar al constructor
        public static int Fiable = 0;
        public static int PocoFiable = 1;
        public static int NoFiable = 2;
        public static int Terminacion = 0;
        public static int Bifurcacion = 1;
        public static int Desconocido = -1;

        public Minucia() : this(0, 0, Minucia.NoFiable, Minucia.Desconocido) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">i filas</param>
        /// <param name="y">j columnas</param>
        public Minucia(int x, int y, int fiabilidad, int tipo)
        {
            Atributos atr = Atributos.getInstance();

            this.x = x;
            this.y = y;
            this.fiabilidad = fiabilidad;
            this.tipo = tipo;

            //calcular angulo
            angulo = Math.Asin((double)y / Math.Sqrt(x * x + y * y));

            circulos = new Circulo[atr.radiosL.Length];

            for (int i = 0; i < circulos.Length; i++)
            {
                // Desacoplo el acceso a Atributos desde la clase círculo, así
                // ahora sólo se hace desde aquí. Le paso al círculo su radio y
                // el número de puntos que debe buscar.
                circulos[i] = new Circulo(i, angulo, atr.radiosL[i], atr.puntosK[i]);
            }

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
}
