using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    /// <summary>
    /// The texture-based descriptor of the minutia is represented as
    /// Do(p) = {{(Bl,k;wl,k)}Kl−1 k=0 }L−1
    /// </summary>
    public class Descriptor_base
    {
        /*
        // Lista de descriptores de textura para una minucia dada
        List<DescriptorTextura> descriptoresTextura;

        // Minucia asociada al descriptor base
        Minucia minucia;

        public Descriptor_base(Minucia minucia)
        {
            descriptoresTextura = new List<DescriptorTextura>();
            this.minucia = minucia;
        }
        /// <summary>
        /// Inserta las caracteristicas descriptoras de los puntos de un circulo a nuestro
        /// conjunto descriptor de textura
        /// ¡¡¡ Lo de los círculos es para los descriptores de textura !!!
        /// Falta acoplar lo de los minutiae-based descriptors
        /// </summary>
        /// <param name="c"></param>
        public void Insertar_puntos_circulos(Circulo circulo)
        {
            for (int i = 0; i < circulo.puntos.Length; i++)
            {
                DescriptorTextura descriptorTextura = new DescriptorTextura();
                
                
                // * Sea válido o no, el punto hay que insertarlo en el descriptor.
                // * Si no es válido hay que darle un valor erróneo, pero hay que insertarlo
                // * en su posición correspondiente para no perder la referencia más tarde
                // * al compararlo con otras minucias
                 
                if (circulo.puntos[i].esValido)
                {
                    descriptorTextura.angulo = circulo.get_relative_orientation(i);
                    descriptorTextura.frecuencia = (double)1 / circulo.get_teta_polar_punto(i);
                }
                else
                {
                    descriptorTextura.angulo = Punto.valorErroneo;
                    descriptorTextura.frecuencia = Punto.valorErroneo;
                }

                descriptoresTextura.Add(descriptorTextura); 
            }
        }

        /// <summary>
        /// Sobrecarga del operador vector para acceder al conjunto como si fuese un vector
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DescriptorTextura this[int i]
        {
            get
            {
                return descriptoresTextura.ToArray()[i];
            }
            set
            {
                if (i > descriptoresTextura.Count)
                {
                    descriptoresTextura.Add(value);
                }
                else
                {
                    descriptoresTextura.ToArray()[i] = value;
                }
            }
        }
    }

    public class DescriptorTextura
    {
        public double angulo = 0;
        public double frecuencia = 0;

        private static double diff,w;

        public DescriptorTextura()
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
        public double similitud(DescriptorTextura d)
        {
            Atributos atr = Atributos.getInstance();
            return w * similar_angle(d) + diff * similar_frecuency(d);
        }

        private double similar_frecuency(DescriptorTextura q)
        {
            double exp = -(this.frecuencia - q.frecuencia) / 3;
            return Math.Exp(exp);
        }

        private double similar_angle(DescriptorTextura q)
        {
            double exp = Circulo.lambda(this.angulo - q.angulo);
            exp = -exp * (Math.PI / 16);

            return Math.Exp(exp);
        }

        */
    }
}
