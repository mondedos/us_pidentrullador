using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    class GreedyMatch
    {

        List<Minucia> minucias1;
        List<Minucia> minucias2;
        List<ParejaMinucia> parejasMinucias;

        /// <summary>
        ///  Número de minucias en la huella1 y en la huella2
        /// </summary>
        int N1;
        int N2;

        /// <summary>
        ///  Array de parejas de minucias de la huella 1 y la huella 2
        /// </summary>
        ParejaMinucia [,] arrayParejas;

        // Lista de parejas de minucia normalizadas
        List<ParejaMinuciaNormalizada> normalizadas;

        // Array de parejas normalizadas en orden
        public ParejaMinuciaNormalizada [] vectorParejas;

        // Lista final de correspondencias obtenidas entre huellas
        public List<Correspondencia> correspondencias;

        public GreedyMatch(List<Minucia> minucias1, List<Minucia> minucias2, List<ParejaMinucia> parejasMinucias)
        {
            this.minucias1 = minucias1;
            this.minucias2 = minucias2;
            this.parejasMinucias = parejasMinucias;

            this.N1 = minucias1.Count;
            this.N2 = minucias2.Count;

            this.correspondencias = new List<Correspondencia>();
            normalizadas = new List<ParejaMinuciaNormalizada>();

            generarArrayParejas();
            calcularDescriptoresNormalizados();
            generarCorrespondenciasEnOrden();

            algoritmo();
        }

        void generarArrayParejas()
        {
            this.arrayParejas = new ParejaMinucia[N1, N2];

            int i, j;

            for (i = 0; i < N1; i++)
                for (j = 0; j < N2; j++)
                    this.arrayParejas[i, j] = colocaEnPosicion(parejasMinucias, i, j);
        }

        void calcularDescriptoresNormalizados()
        {
            int i, j, k;

            for (i = 0; i < N1; i++)
            {
                for (j = 0; j < N2; j++)
                {
                    double dividendo = arrayParejas[i, j].sc * (double)(N1 + N2 - 1);

                    double divisor1 = 0;
                    for (k = 0; k < N1; k++)
                        divisor1 += arrayParejas[k, j].sc;

                    double divisor2 = 0;
                    for (k = 0; k < N2; k++)
                        divisor2 += arrayParejas[i, k].sc;

                    double cociente = 0;
                    double divisor = divisor1 + divisor2 - arrayParejas[i, j].sc;

                    if (divisor == 0)
                        cociente = 0;
                    else
                        cociente = dividendo / divisor;

                    normalizadas.Add(new ParejaMinuciaNormalizada(this.arrayParejas[i,j],i,j,cociente));
                }
            }  
        }

        /// <summary>
        /// Hace la ordenación y las coloca en un vector de parejas
        /// </summary>
        void generarCorrespondenciasEnOrden()
        {
            this.vectorParejas = normalizadas.ToArray();
            Array.Sort(this.vectorParejas);
            Array.Reverse(this.vectorParejas);
        }

        void algoritmo()
        {
            int i, j, k;

            // inicializar flags a 0
            bool [] flagI = new bool[N1];
            bool [] flagJ = new bool[N2];

            for (i = 0; i < N1; i++)
                flagI[i] = false;

            for (j = 0; j < N2; j++)
                flagJ[j] = false;

            ParejaMinuciaNormalizada primera = vectorParejas[0];
            flagI[primera.i] = true;
            flagJ[primera.j] = true;

            for (k = 1; k < N1 * N2; k++)
            {
                i = vectorParejas[k].i;
                j = vectorParejas[k].j;

                if (!flagI[i] && !flagJ[j] && sonEncajables(vectorParejas[k],primera))
                {
                    flagI[i] = true;
                    flagJ[j] = true;
                    correspondencias.Add(
                        new Correspondencia(vectorParejas[k].pm.minucia1, vectorParejas[k].pm.minucia2));
                }
            }
        }

        bool sonEncajables(ParejaMinuciaNormalizada pmn, ParejaMinuciaNormalizada primera)
        {
            Atributos atr = Atributos.getInstance();

            double distancia = Funcion.distancia(pmn.pm.minucia1.x, pmn.pm.minucia1.y,
                                                 pmn.pm.minucia2.x, pmn.pm.minucia2.y);

            bool cercanosEnDistancia = distancia <= atr.radioEncaje;

            // ????????
            bool diferenciaDireccionPequeña = true;

            return cercanosEnDistancia && diferenciaDireccionPequeña;
        }

        /// <summary>
        /// Devuelve de la lista la pareja con las minucias de indices iesimos y jesimos
        /// </summary>
        /// <param name="parejasMinucias"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        ParejaMinucia colocaEnPosicion(List<ParejaMinucia> parejasMinucias, int i, int j)
        {
            ParejaMinucia temp = null;

            foreach (ParejaMinucia pareja in parejasMinucias)
            {
                if (pareja.minucia1.indice == i && pareja.minucia2.indice == j)
                {
                    temp = pareja;
                    break;
                }
            }

            return temp;
        }
    }
}
