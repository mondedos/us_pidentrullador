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
        public Circulo[] circulos = null;

        public List<Minucia> vecinos;

        // Pongo esto como entero porque los enum no me dejaba declararlos estáticos
        // y tengo que referenciarlos desde fuera a la hora de llamar al constructor
        public const int Fiable = 0;
        public const int PocoFiable = 1;
        public const int NoFiable = 2;
        public const int Terminacion = 0;
        public const int Bifurcacion = 1;
        public const int Desconocido = -1;

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
                circulos[i] = new Circulo(i, angulo, atr.radiosL[i], atr.puntosK[i], this);
            }

            vecinos = new List<Minucia>();
        }

        public void agregarVecinos(List<Minucia> listaMinucias)
        {
            Atributos atr = Atributos.getInstance();
            vecinos = new List<Minucia>();

            foreach (Minucia minucia in listaMinucias)
                if (this != minucia && Funcion.distancia(this.x,this.y,minucia.x,minucia.y) <= atr.radioVecinos)
                    this.vecinos.Add(minucia);
        }

        /*

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
        /// <summary>
        /// Esta funcion implementa lo que en el artículo se denomina Sm(p,q)
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public double parecidos_en_vecinos(Minucia q)
        {
            double num = (vecinos.Count + 1) * (q.vecinos.Count + 1);
            int Mp = 0, Mq = 0;

            foreach (Minucia m in vecinos)
            {
                if (Podemos_hacer_match(m))
                    Mp++;
            }
            Mp++;

            foreach (Minucia m in q.vecinos)
            {
                if (q.Podemos_hacer_match(m))
                    Mq++;
            }
            Mq++;

            return num/(Mp*Mq);
        }
        /// <summary>
        /// Comprueba si de cumplen las condiciones necesarias y suficientes para decir que una minucia es buena.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private bool Podemos_hacer_match(Minucia q)
        {
            return es_real(q) && !esta_oculta(q) && esta_dentro_radio(q);
        }

        private bool es_real(Minucia q)
        {
        //            pi is an unreliable minutia. An example is shown in Fig.
        //4(a) to illustrate this case.

            return true;
        }
        private bool esta_oculta(Minucia q)
        {
        //            pi is located at the occluded region of template fingerprint.
        //An example is shown in Fig. 4(b) to illustrate this case.

            return false;
        }
        /// <summary>
        /// Comprueba si una minucia q está en el radio de acción de una minucia p
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private bool esta_dentro_radio(Minucia q)
        {
            Atributos atr = Atributos.getInstance();

            int difx = x - q.x, dify = y - q.y;
            double distancia = Math.Sqrt(difx * difx + dify * dify);

            return distancia <= (0.8 * atr.distanciaPixel_entreMinucias);
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
        /// s(pi,qi) es el grado de similitud entre dos minucias
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private double s(Minucia q)
        {
            return parecidos_en_vecinos(q);
        }
         * 
         */
    }
}
