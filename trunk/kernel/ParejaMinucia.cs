using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class ParejaMinucia
    {
        public Minucia minucia1;
        public Minucia minucia2;

        // descriptor de textura
        double st;

        // descriptor de minucia
        double sm;

        // descriptor combinado
        public double sc;

        // rotación relativa
        public double rotacionRelativa;

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

            this.rotacionRelativa = m2.angulo - m1.angulo;
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
    }
}
