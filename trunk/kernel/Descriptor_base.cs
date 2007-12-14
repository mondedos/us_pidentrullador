using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    /// <summary>
    /// The texture-based descriptor of the minutia is represented as
    ///Do(p) = {{(Bl,k;wl,k)}Kl−1
    ///k=0 }L−1
    /// </summary>
    public class Descriptor_base
    {
        List<Descriptor> _descriptores;

        public Descriptor_base()
        {
            _descriptores = new List<Descriptor>();
        }
        /// <summary>
        /// Inserta las caracteristicas descriptoras de los puntos de un circulo a nuestro
        /// conjunto descriptor.
        /// </summary>
        /// <param name="c"></param>
        public void Insertar_puntos_circulos(Circulo c)
        {
            for (int i = 0; i < c.puntos.Length; i++)
            {
                if (c.puntos[i].esValido)
                {
                    Descriptor d = new Descriptor();
                    d.angulo = c.get_relative_orientation(i);

                    //TODO
                    //añadir frecuencia

                    _descriptores.Add(d);
                }
            }
        }

        /// <summary>
        /// Sobre carga del operador vector para acceder al conjunto como si fuese un vector
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Descriptor this[int i]
        {
            get
            {
                return _descriptores.ToArray()[i];
            }
            set
            {
                if (i > _descriptores.Count)
                {
                    _descriptores.Add(value);
                }
                else
                {
                    _descriptores.ToArray()[i] = value;
                }
            }
        }
    }

    public class Descriptor
    {
        public double angulo=0;
        public double frecuencia = 0;

        private static double diff,w;

        public Descriptor()
        {
            Atributos atr = Atributos.getInstance();
            diff = 1 - atr.w;
            w = atr.w;
        }

        /// <summary>
        /// Comprueba cuanto de similares son dos descriptores.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>nivel de similitud</returns>
        public double similitud(Descriptor d)
        {
            Atributos atr = Atributos.getInstance();
            return w * similar_angle(d) + diff * similar_frecuency(d);
        }
        private double similar_frecuency(Descriptor q)
        {
            double exp = -(this.frecuencia - q.frecuencia) / 3;
            return Math.Exp(exp);
        }
        private double similar_angle(Descriptor q)
        {
            double exp = Circulo.lambda(this.angulo - q.angulo);
            exp = -exp * (Math.PI / 16);

            return Math.Exp(exp);
        }
    }
}
