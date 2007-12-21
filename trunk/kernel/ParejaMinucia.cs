using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    class ParejaMinucia
    {
        public Minucia minucia1;
        public Minucia minucia2;

        // descriptor de textura
        double st;

        // descriptor de minucia
        double sm;

        // descriptor combinado
        public double sc;

        public bool esDescriptorTexturaRelevante;
        double porcentajePuntosTextura;

        public ParejaMinucia(Minucia m1, Minucia m2)
        {
            this.minucia1 = m1;
            this.minucia2 = m2;

            this.st = 0.0;
            this.sm = 0.0;
            this.sc = 0.0;

            // Más adelante analizaremos si no lo es
            this.esDescriptorTexturaRelevante = true;

            calcularDescriptorTextura();
            calcularDescriptorMinucia();

            this.sc = this.st * this.sm;
        }

        void calcularDescriptorTextura()
        {
            int i, j, numPuntosTotal = 0, numPuntosValidos = 0;
            Atributos atr = Atributos.getInstance();

            List<TexturaParcial> listaParcial = new List<TexturaParcial>();

            // i recorre el número de círculos
            for (i = 0; i < atr.radiosL.Length; i++)
            {
                // j recorre el número de puntos de cada círculo
                for (j = 0; j < atr.puntosK[i]; j++)
                {
                    numPuntosTotal++;

                    Punto p1 = minucia1.circulos[i].puntos[j];
                    Punto p2 = minucia2.circulos[i].puntos[j];

                    // Para que la pareja sea válida los puntos j-ésimos
                    // del círculo i-ésimo han de ser válidos
                    if (p1.esValido && p2.esValido)
                    {
                        numPuntosValidos++;

                        listaParcial.Add(new TexturaParcial(
                            hallarS0(p1.orientacionRelativa,p2.orientacionRelativa),
                            hallarSf(p1.frecuencia, p2.frecuencia)));
                    }
                }
            }

            this.porcentajePuntosTextura = 100 * (double)numPuntosValidos / (double) numPuntosTotal;

            if (this.porcentajePuntosTextura >= (double)atr.minPorcentajeValidos)
            {
                TexturaParcial tp = hallarTexturaMedia(listaParcial);
                this.st = atr.w * tp.s0 + (1 - atr.w) * tp.sf;
            }
            else
            {
                this.st = 0;
                this.esDescriptorTexturaRelevante = false;
            }
        }

        void calcularDescriptorMinucia()
        {

            List<MinuciaParcial> dm_p = new List<MinuciaParcial>();
            List<MinuciaParcial> dm_q = new List<MinuciaParcial>();

            // Calculamos Dm(p), vecinos de p(minucia1)
            foreach (Minucia vecino in minucia1.vecinos)
                dm_p.Add(new MinuciaParcial(vecino, minucia1));

            // calculamos Dm(q), vecinos de q(minucia2)
            foreach (Minucia vecino in minucia2.vecinos)
                dm_q.Add(new MinuciaParcial(vecino, minucia2));

            // Conjunto dm(p) habiéndole aplicado la transformación rígida de p a q
            List<MinuciaParcial> dm_p_t = new TransformacionT(minucia1, minucia2, dm_p).dm_minucia_t;

            // Conjunto dm(q) habiéndole aplicado la transformación rígida de q a p
            List<MinuciaParcial> dm_q_t = new TransformacionT(minucia2, minucia1, dm_q).dm_minucia_t;

            EncajesMinucia em_p = new EncajesMinucia(dm_p_t, dm_q);
            EncajesMinucia em_q = new EncajesMinucia(dm_q_t, dm_p);

            this.sm = em_p.factor * em_q.factor;
        }

        public override String ToString()
        {
            String s1 = "Par<"+minucia1.indice+","+minucia2.indice+">";
            String s2 = "%t<"+Funcion.recortarDigitos(this.porcentajePuntosTextura,0)+">";
            String s3 = "st<"+Funcion.recortarDigitos(this.st,3)+">";
            String s4 = "sm<"+Funcion.recortarDigitos(this.sm,3)+">";
            String s5 = "sc<"+Funcion.recortarDigitos(this.sc,3)+">";

            return s1 + "\t-\t" + s2 + "\t-\t" + s3 + "\t-\t" + s4 + "\t-\t" + s5;  
        }

        /// <summary>
        /// Dada una lista de texturas, saca la media de entre todas
        /// </summary>
        /// <param name="listaParcial"></param>
        /// <returns></returns>
        TexturaParcial hallarTexturaMedia(List<TexturaParcial> listaParcial)
        {
            double s0Acum = 0;
            double sfAcum = 0;

            foreach (TexturaParcial tp in listaParcial)
            {
                s0Acum += tp.s0;
                sfAcum += tp.sf;
            }

            s0Acum = s0Acum / (double)listaParcial.Count;
            sfAcum = sfAcum / (double)listaParcial.Count;

            return new TexturaParcial(s0Acum, sfAcum);
        }

        /// <summary>
        /// Calcula la orientación relativa media a partir de dos orientaciones
        /// relativas parcialas
        /// </summary>
        /// <param name="or1"></param>
        /// <param name="or2"></param>
        /// <returns></returns>
        double hallarS0(double or1, double or2)
        {
            double divisor = Math.Abs(Punto.lambda(or1 - or2));
            double dividendo = Math.PI / (double)16;
            double cociente = divisor / dividendo;

            return Math.Exp(-cociente);
        }

        /// <summary>
        /// Calcula la frecuencia media a partir de frecuencias parciales
        /// </summary>
        /// <param name="fr1"></param>
        /// <param name="fr2"></param>
        /// <returns></returns>
        double hallarSf(double fr1, double fr2)
        {
            double divisor = Math.Abs(fr1 - fr2);
            double dividendo = (double)3;
            double cociente = divisor / dividendo;
            return Math.Exp(-cociente);
        }

        class TexturaParcial
        {
            public double s0;
            public double sf;

            public TexturaParcial(double s0, double sf)
            {
                this.s0 = s0;
                this.sf = sf;
            }
        }

        class MinuciaParcial
        {
            public int x;
            public int y;
            public double teta;

            public Minucia minuciaCentral;
            public Minucia minucia;

            public bool encaja;
            public bool deberiaEncajar;

            /// <summary>
            /// Constructor aplicado cuando queremos guardar la relación de vecindad
            /// entre una minucia y su minucia asociada central
            /// </summary>
            /// <param name="minucia"></param>
            /// <param name="minuciaCentral"></param>
            public MinuciaParcial(Minucia minucia, Minucia minuciaCentral)
            {
                this.minucia = minucia;
                this.minuciaCentral = minuciaCentral;

                this.x = minucia.x;
                this.y = minucia.y;
                this.teta = Funcion.anguloEntrePuntos(minuciaCentral.x, minuciaCentral.y, minucia.x, minucia.y);

                this.encaja = false;
                this.deberiaEncajar = false;
            }

            /// <summary>
            /// Constructor aplicado cuando queremos construir una minucia parcial
            /// aplicándole las coordenadas de una transformada T
            /// </summary>
            /// <param name="minucia"></param>
            /// <param name="minuciaCentral"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="teta"></param>
            public MinuciaParcial(Minucia minucia, Minucia minuciaCentral, int x, int y, double teta)
            {
                this.minucia = minucia;
                this.minuciaCentral = minuciaCentral;

                this.x = x;
                this.y = y;
                this.teta = teta;

                this.encaja = false;
                this.deberiaEncajar = false;
            }
        }

        class TransformacionT
        {
            int difx;
            int dify;
            double difa;
            public List<MinuciaParcial> dm_minucia_t;

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
                difa = Funcion.anguloEntrePuntos(minucia1.x, minucia1.y, minucia2.x, minucia2.y);

                // Conjunto dm(p) habiéndole aplicado la transformación rígida
                dm_minucia_t = aplicarTransformacionRigida(dm_minucia, difx, dify, difa);
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
            List<MinuciaParcial> aplicarTransformacionRigida(List<MinuciaParcial> listaOriginal, int difx, int dify, double difa)
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
        }

        /// <summary>
        /// Dados dos set de vecinos, establecerá los encajes correspondientes
        /// para calcular el descriptor de la ParejaMinucia
        /// </summary>
        class EncajesMinucia
        {
            public int encajan;
            public int deberianEncajar;
            public double factor;

            List<MinuciaParcial> origen;
            List<MinuciaParcial> destino;

            public EncajesMinucia(List<MinuciaParcial> origen, List<MinuciaParcial> destino)
            {
                this.factor = 0.0;
                this.encajan = 0;
                this.deberianEncajar = 0;

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
                return true;
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
}
