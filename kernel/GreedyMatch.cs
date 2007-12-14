using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class GreedyMatch
    {

        /// <summary>
        /// Algoritmo final, que iremos resolviendo para ver que es lo que vamos necesitando :P
        /// </summary>
        public static void GreedyMatchAlgoritmo()
        {
//            int size = 10;

//            normalized_similarity[] L = new normalized_similarity[size];

//            List<MP> mp = new List<MP>();

//            int i, j;

//            bool[] flag1 = new bool[L.Length];
//            bool[] flag2 = new bool[L.Length];


////The first minutiae pair (i1, j1) in L is used as the initial
////minutiae pair, and two minutiae sets are aligned using the initial
////minutiae pair

//            normalized_similarity inicial = L[0];

//            flag1[inicial.i] = true;
//            flag2[inicial.j] = true;

//            for (int m = 0; m < L.Length; m++)
//            {
//                i = L[m].i;
//                j = L[m].j;

//                Minucia mi = L[m].getMinuciaI();
//                Minucia mj = L[m].getMinuciaJ();

//                if (!flag1[i] && !flag2[j] && esMacheable(mi, mj))
//                {
//                    MP ij = new MP();
//                    ij.i = i;
//                    ij.j = j;
//                    mp.Add(ij);

//                    flag1[i] = true;
//                    flag2[j] = true;
//                }
//            }
        }

        private static bool esMacheable(Minucia i, Minucia j)
        {
            //TODO
            return true;
        }
    }
    public struct normalized_similarity
    {
        public int i, j;
        public double sn;
    }
    public struct MP
    {
        public Minucia i, j;
    }
}
