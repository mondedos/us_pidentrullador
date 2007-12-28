using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    class TransformacionT
    {
        int difx;
        int dify;
        double difa;

        // provenientes de la primera parte
        public List<MinuciaParcial> dm_minucia_t;
        
        // provenientes de la segunda parte
        public ParAlineado[] parejas;
        public ParejaMinuciaNormalizada inicial;

        /// <summary>
        /// Minucia1 y Minucia2 son las dos minucias centrales p y q 
        /// ó q y p correspondientes cada caso
        /// </summary>
        /// <param name="minucia1"></param>
        /// <param name="minucia2"></param>
        /// <param name="dm_minucia"></param>
        public TransformacionT(Minucia minucia1, Minucia minucia2, List<MinuciaParcial> dm_minucia)
        {
            difx = minucia2.x - minucia1.x;
            dify = minucia2.y - minucia1.y;
            difa = minucia2.angulo - minucia1.angulo;
            //difa = Funcion.anguloEntrePuntos(minucia1.x, minucia1.y, minucia2.x, minucia2.y);

            // Conjunto dm(p) habiéndole aplicado la transformación rígida
            dm_minucia_t = aplicarTransformacionRigidaMinuciaParcial(dm_minucia);
        }

        /// <summary>
        /// Aplica la transformada rígida a un conjunto de minucias parciales
        /// con respecto a los centros de dos minucias centrales
        /// </summary>
        /// <param name="listaOriginal"></param>
        /// <param name="difx"></param>
        /// <param name="dify"></param>
        /// <param name="difa"></param>
        /// <returns></returns>
        List<MinuciaParcial> aplicarTransformacionRigidaMinuciaParcial(List<MinuciaParcial> listaOriginal)
        {
            List<MinuciaParcial> listaNueva = new List<MinuciaParcial>();

            foreach (MinuciaParcial mp in listaOriginal)
            {
                double nx = (double)mp.minucia.x * Math.Cos(difa) + (double)mp.minucia.y * Math.Sin(difa) + difx;
                double ny = (double)mp.minucia.x * -Math.Sin(difa) + (double)mp.minucia.y * Math.Cos(difa) + dify;
                double na = mp.teta - difa;

                listaNueva.Add(new MinuciaParcial(mp.minucia, mp.minuciaCentral, (int)nx, (int)ny, na));
            }

            return listaNueva;
        }

        public TransformacionT(ParejaMinuciaNormalizada[] vectorParejas)
        {
            inicial = vectorParejas[0];

            difx = inicial.pm.minucia2.x - inicial.pm.minucia1.x;
            dify = inicial.pm.minucia2.y - inicial.pm.minucia1.y;
            difa = inicial.pm.minucia2.angulo - inicial.pm.minucia1.angulo;

            aplicarTransformacionRigidaParejaMinuciaNormalizada(vectorParejas);
        }

        /// <summary>
        /// cada minucia de la huella 1 es trasladada y rotada a su pareja de la huella2
        /// para ver si encajan
        /// </summary>
        /// <param name="vectorParejas"></param>
       void aplicarTransformacionRigidaParejaMinuciaNormalizada(ParejaMinuciaNormalizada[] vectorParejas)
        {
            parejas = new ParAlineado[vectorParejas.Length];

            for (int k = 0; k < vectorParejas.Length; k++)
            {

                double nx = (double)vectorParejas[k].pm.minucia1.x * Math.Cos(difa) + 
                            (double)vectorParejas[k].pm.minucia1.y * Math.Sin(difa) + difx;
                double ny = (double)vectorParejas[k].pm.minucia1.x * -Math.Sin(difa) + 
                            (double)vectorParejas[k].pm.minucia1.y * Math.Cos(difa) + dify;

                int x2 = vectorParejas[k].pm.minucia2.x;
                int y2 = vectorParejas[k].pm.minucia2.y;

                parejas[k] = new ParAlineado(vectorParejas[k].pm.minucia1, (int)nx, (int)ny, 
                                             vectorParejas[k].pm.minucia2, x2, y2);
            }
        }
    }
}
