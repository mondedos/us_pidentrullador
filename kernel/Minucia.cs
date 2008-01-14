using System;
using System.Collections.Generic;
using System.Text;

namespace kernel
{
    public class Minucia
    {
        // coordenadas y �ngulo inferior con respecto al origne
        public int x = 0, y = 0;
        public double angulo = 0.0;

        // Indicaremos si es fiable, poco fiable o no fiable
        public int fiabilidad;

        // Indicaremos si es terminaci�n o bifurcaci�n
        public int tipo;

        // C�rculos de puntos conc�ntricos en torno a la minucia
        public Circulo[] circulos = null;

        // Vecinos que tiene cada minucia. Servir� para el descriptor de minucia
        public List<Minucia> vecinos;

        // Indicar� el n�mero de minucia en la huella. Se asigna al final
        public int indice;

        // Pongo esto como entero porque los enum no me dejaba declararlos est�ticos
        // y tengo que referenciarlos desde fuera a la hora de llamar al constructor
        public const int Fiable = 0;
        public const int PocoFiable = 1;
        public const int NoFiable = 2;
        public const int Terminacion = 0;
        public const int Bifurcacion = 1;
        public const int Desconocido = -1;

        public Minucia() : this(0, 0, Minucia.NoFiable, Minucia.Desconocido) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">i filas</param>
        /// <param name="y">j columnas</param>
        public Minucia(int x, int y, int fiabilidad, int tipo)
        {
            Atributos atr = Atributos.getInstance();

            this.x = x;
            this.y = y;
            this.fiabilidad = fiabilidad;
            this.tipo = tipo;

            //calcular angulo global de minucia
            angulo = Funcion.anguloEntrePuntos(0, 0, x, y);

            circulos = new Circulo[atr.radiosL.Length];

            for (int i = 0; i < circulos.Length; i++)
            {
                // Desacoplo el acceso a Atributos desde la clase c�rculo, as�
                // ahora s�lo se hace desde aqu�. Le paso al c�rculo su radio y
                // el n�mero de puntos que debe buscar.
                circulos[i] = new Circulo(i, angulo, atr.radiosL[i], atr.puntosK[i], this);
            }

            vecinos = new List<Minucia>();
        }

        public void agregarVecinos(List<Minucia> listaMinucias)
        {
            Atributos atr = Atributos.getInstance();
            vecinos = new List<Minucia>();

            foreach (Minucia minucia in listaMinucias)
                if (this != minucia && Funcion.distancia(this.x,this.y,minucia.x,minucia.y) <= atr.radioVecinos)
                    this.vecinos.Add(minucia);
        }

        public override string ToString()
        {
            return "M<" + Funcion.intBien(indice) + ">" + ": (" + Funcion.intBien(x) + "," + Funcion.intBien(y) + ")";
        }
    }
}
