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

        public Minucia minuciaMasFiable1;
        public Minucia minuciaMasFiable2;

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
        public ParejaMinuciaNormalizada[] vectorParejas;

        // Array de parejas normalizadas en orden
        public ParejaMinuciaNormalizada inicial;
        public ParAlineado [] parejas;

        // Lista final de correspondencias obtenidas entre huellas
        public List<Correspondencia> correspondencias;

        public TransformacionT transformacionT;

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

            foreach (ParejaMinucia pm in parejasMinucias)
                this.arrayParejas[pm.minucia1.indice, pm.minucia2.indice] = pm;
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
            Atributos atr = Atributos.getInstance();
            
            //las que tienen una rotación relativa mayor que un umbral las desechamos
            foreach (ParejaMinuciaNormalizada pmn in normalizadas)
                if (pmn.pm.rotacionRelativa > atr.umbralAngulo)
                    pmn.sn = 0;
            
            vectorParejas = normalizadas.ToArray();
            Array.Sort(vectorParejas);
            Array.Reverse(vectorParejas);

            this.inicial = buscarParejaInicial(vectorParejas);

            minuciaMasFiable1 = inicial.pm.minucia1;
            minuciaMasFiable2 = inicial.pm.minucia2;

            transformacionT = new TransformacionT(inicial, vectorParejas);
            this.parejas = transformacionT.parejas;
        }

        ParejaMinuciaNormalizada buscarParejaInicial(ParejaMinuciaNormalizada[] vectorParejas)
        {
            ParejaMinuciaNormalizada tmp = null;

            double maximo = -1;

            // localizamos el mayor sn
            for (int i = 0; i < vectorParejas.Length; i++)
            {
                if (vectorParejas[i].sn >= maximo)
                    maximo = vectorParejas[i].sn;
            }

            List<ParejaMinuciaNormalizada> listaPm = new List<ParejaMinuciaNormalizada>();

            // Buscamos las parejas que comparten dicho sn, es decir, las que empatan arriba
            for (int i = 0; i < vectorParejas.Length; i++)
            {
                if (vectorParejas[i].sn == maximo)
                    listaPm.Add(vectorParejas[i]);
            }

            bool flag = false;

            // De dicha lista intentamos devolver una pareja que sea fiable, si no pues uno de los encontrados
            foreach (ParejaMinuciaNormalizada pmn in listaPm)
            {
                if (pmn.pm.minucia1.fiabilidad == Minucia.Fiable &&
                    pmn.pm.minucia2.fiabilidad == Minucia.Fiable)
                {
                    tmp = pmn;
                    flag = true;        
                    break;
                }
            }

            // En caso contrario devolvemos uno cualquiera
            if (!flag)
                tmp = listaPm.ToArray()[0];

            return tmp;
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

            flagI[inicial.i] = true;
            flagJ[inicial.j] = true;

            for (k = 1; k < N1 * N2; k++)
            {
                i = parejas[k].minucia1.indice;
                j = parejas[k].minucia2.indice;

                bool encajan = sonEncajables(parejas[k]);

                if (!flagI[i] && !flagJ[j] && encajan)
                {
                    flagI[i] = true;
                    flagJ[j] = true;
                    correspondencias.Add(
                        new Correspondencia(parejas[k].minucia1, parejas[k].minucia2));
                }
            }
        }

        bool sonEncajables(ParAlineado pa)
        {
            Atributos atr = Atributos.getInstance();

            double distancia = Funcion.distancia(pa.xT, pa.yT, pa.xDestino, pa.yDestino);

            bool cercanosEnDistancia = distancia <= (double)atr.radioEncaje;
            bool diferenciaDireccionPequeña = true;

            return cercanosEnDistancia && diferenciaDireccionPequeña;
        }
    }
}
