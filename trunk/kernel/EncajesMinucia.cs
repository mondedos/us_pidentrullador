using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    /// <summary>
    /// Dados dos set de vecinos, establecerá los encajes correspondientes
    /// para calcular el descriptor de la ParejaMinucia
    /// </summary>
    class EncajesMinucia
    {
        public double factor;

        List<MinuciaParcial> origen;
        List<MinuciaParcial> destino;

        public EncajesMinucia(List<MinuciaParcial> origen, List<MinuciaParcial> destino)
        {
            this.factor = 0.0;

            this.origen = origen;
            this.destino = destino;

            buscarEncajes();
            calcularFactor();
        }

        void buscarEncajes()
        {
            foreach (MinuciaParcial mp in origen)
            {
                if (hayAlgunEncaje(mp, destino))
                {
                    mp.encaja = true;
                    mp.deberiaEncajar = true;
                }
                else
                {
                    if (mp.minucia.fiabilidad != Minucia.PocoFiable &&
                        !estaEnRegionOcluida(mp) &&
                        !estaMuyLejosCentro(mp))
                        mp.deberiaEncajar = true;
                }
            }
        }

        void calcularFactor()
        {
            int numEncajes = 0;
            int numDeberiaEncajar = 0;

            foreach (MinuciaParcial mp in origen)
            {
                if (mp.encaja)
                    numEncajes++;

                if (mp.deberiaEncajar)
                    numDeberiaEncajar++;
            }

            this.factor = ((double)numEncajes + 1) / ((double)numDeberiaEncajar + 1);
        }

        //Dada una minucia mp trasladada, mirar si existe en los vecinos del destino
        bool hayAlgunEncaje(MinuciaParcial mp, List<MinuciaParcial> destino)
        {
            Atributos atr = Atributos.getInstance();
            bool encaje = false;

            foreach (MinuciaParcial mpdestino in destino)
            {
                int dx = mpdestino.minucia.x;
                int dy = mpdestino.minucia.y;

                double distancia = Funcion.distancia(dx, dy, mp.x, mp.y);

                if (distancia <= atr.radioEncaje)
                {
                    encaje = true;
                    break;
                }
            }

            return encaje;
        }

        bool estaEnRegionOcluida(MinuciaParcial mp)
        {
            return false;
        }

        bool estaMuyLejosCentro(MinuciaParcial mp)
        {
            Atributos atr = Atributos.getInstance();

            double maxDistancia = atr.maxDistancia * atr.radioVecinos;
            double distancia = Funcion.distancia(mp.minuciaCentral.x, mp.minuciaCentral.y, mp.minucia.x, mp.minucia.y);

            return distancia > maxDistancia;
        }
    }
}
