using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    class Matriz
    {
        public int[,] matriz;
        public int filas;
        public int cols;

        static object o = new object();

        static Matriz instancia = null;

        /// <summary>
        /// Singleton que guardará información sobre la matriz de puntos de la imagen
        /// He optado por esta decisión ya que ha de ser visible desde cada punto, y
        /// guardar la matriz en cada punto es inaceptable desde el punto de vista de la memoria
        /// </summary>
        /// <returns></returns>
        public static Matriz getInstance()
        {
            lock (o)
            {
                if (instancia == null)
                    instancia = new Matriz();
                return instancia;
            }
        }
    }
}
