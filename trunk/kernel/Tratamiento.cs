using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kernel
{
    public class Tratamiento
    {
        Bitmap huella;
        int[,] matriz;
        Atributos atr;
        Bitmap[] pasos;
        int filas, cols;

        static int[,] direcciones = { { 3, 2, 1 },
                                      { 4, 9, 0 }, 
                                      { 5, 6, 7 } };

        static Point puntoError = new Point(0, 0);

        /// <summary>
        /// Estos atributos se utilizan para indexar los pasos
        /// </summary>
        static int busquedaTerminaciones = 0;
        static int compruebaTerminaciones = 1;
        static int guardaTerminaciones = 2;
        static int busquedaBifurcaciones = 3;
        static int compruebaBifurcaciones = 4;
        static int guardaBifurcaciones = 5;
        static int totalPasos = 6;

        List<TerminacionPotencial> terminacionesPotenciales = new List<TerminacionPotencial>();
        List<BifurcacionPotencial> bifurcacionesPotenciales = new List<BifurcacionPotencial>();

        List<Point> terminacionesFiables = new List<Point>();
        List<Point> bifurcacionesFiables = new List<Point>();

        List<Point> terminacionesPocoFiables = new List<Point>();
        List<Point> bifurcacionesPocoFiables = new List<Point>();

        /// <summary>
        /// Aplica los agoritmos de busqueda de terminaciones y bifurcaciones
        /// a una huella, con los atributos dados por el ususuario.
        /// </summary>
        /// <param name="huella"></param>
        /// <param name="atr"></param>
        public Tratamiento(Bitmap huella, Atributos atr)
        {
            this.pasos = new Bitmap[totalPasos];

            this.huella = new Bitmap(huella);
            this.matriz = Adaptador.Adaptar(this.huella);
            this.atr = atr;

            this.filas = huella.Width;
            this.cols = huella.Height;

            buscarTerminaciones();
            comprobarTerminaciones();
            guardarTerminaciones();

            buscarBifurcaciones();
            comprobarBifurcaciones();
            guardarBifurcaciones();
        }

        void guardarTerminaciones()
        {
            this.pasos[guardaTerminaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[guardaTerminaciones]);
            Atributos atr = Atributos.getInstance();

            foreach (Point terminacion in terminacionesFiables)
            {
                g.DrawEllipse(new Pen(atr.colorTerminacionFiable),
                    terminacion.X - atr.radioCirculo / 2, terminacion.Y - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
            }
        }

        void guardarBifurcaciones()
        {
            this.pasos[guardaBifurcaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[guardaBifurcaciones]);
            Atributos atr = Atributos.getInstance();
        }

        /// <summary>
        /// Comprueba que las terminaciones encontradas son correctas
        /// </summary>
        void comprobarTerminaciones()
        {
            this.pasos[compruebaTerminaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[compruebaTerminaciones]);
            Atributos atr = Atributos.getInstance();

            foreach (TerminacionPotencial tp in terminacionesPotenciales)
            {
                bool fiable = true;

                int gradiente = direcciones[tp.prolongacion.X - tp.actual.X + 1, tp.prolongacion.Y - tp.actual.Y + 1];
                Point[] cercanos = PuntosCercanosDeOtraLinea(tp.actual, gradiente);

                Point cercano1 = cercanos[0];
                Point cercano2 = cercanos[1];

                if (cercano1 != puntoError && cercano2 != puntoError)
                {
                    g.DrawLine(new Pen(atr.colorLinea), tp.actual, cercano1);
                    g.DrawLine(new Pen(atr.colorLinea), tp.actual, cercano2);

                    Point[] unidos1 = buscaUnidos(cercano1);
                    Point[] unidos2 = buscaUnidos(cercano2);

                    bool prol0 = false, prol1 = false, prol2 = false, prol3 = false, prol4 = false;

                    if (seProlongaSuficiente(tp.actual, tp.prolongacion, atr.longitudLinea, g))
                        prol0 = true;

                    if (unidos1[0] != puntoError && seProlongaSuficiente(cercano1, unidos1[0], atr.longitudLinea, g))
                        prol1 = true;

                    if (unidos1[1] != puntoError && seProlongaSuficiente(cercano1, unidos1[1], atr.longitudLinea, g))
                        prol2 = true;

                    if (unidos2[0] != puntoError && seProlongaSuficiente(cercano2, unidos2[0], atr.longitudLinea, g))
                        prol3 = true;

                    if (unidos2[1] != puntoError && seProlongaSuficiente(cercano2, unidos2[1], atr.longitudLinea, g))
                        prol4 = true;

                    if (prol0 && prol1 && prol2 && prol3 && prol4)
                    {
                        terminacionesFiables.Add(tp.actual);
                        g.DrawEllipse(new Pen(atr.colorTerminacionFiable),
                                tp.actual.X - atr.radioCirculo / 2, tp.actual.Y - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
                    }
                    else
                        fiable = false;

                    }
                    else
                        fiable = false;

                if (!fiable)
                {
                    terminacionesPocoFiables.Add(tp.actual);
                    g.DrawRectangle(new Pen(atr.colorTerminacionPocoFiable),
                            tp.actual.X - atr.radioCirculo / 2, tp.actual.Y - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
                }
            }
        }

        /// <summary>
        /// Comprueba que las bifurcaciones encontradas son correctas
        /// </summary>
        void comprobarBifurcaciones()
        {
            this.pasos[compruebaBifurcaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[compruebaBifurcaciones]);
            Atributos atr = Atributos.getInstance();

            foreach (BifurcacionPotencial bp in bifurcacionesPotenciales)
            {

            }
        }

        Point[] buscaUnidos(Point actual)
        {

            Point[] unidos = new Point[] { puntoError, puntoError };
            int i, j, x = actual.X, y = actual.Y;
            int contador = 0, puntero = 0;

            for (i = x - 1; i <= x + 1; i++)
            {
                for (j = y - 1; j <= y + 1; j++)
                {
                    if (!(i==x && j==y))
                        contador += matriz[i, j];
                }
            }

            if (contador == 2)
            {
                for (i = x - 1; i <= x + 1; i++)
                {
                    for (j = y - 1; j <= y + 1; j++)
                    {
                        if (!(i == x && j == y) && matriz[i, j] == 1)
                            unidos[puntero++] = new Point(i, j);
                    }
                }
            }

            return unidos;
        }

        Point[] PuntosCercanosDeOtraLinea(Point actual, int gradiente)
        {
            int ngra1 = 0, ngra2 = 0;
            Point dir1 = new Point(0,0), dir2 = new Point(0,0);
            Point[] cercanos = new Point[2];

            Atributos atr = Atributos.getInstance();

            switch (gradiente)
            {
                case 0: ngra1 = 2; ngra2 = 6; break;
                case 1: ngra1 = 3; ngra2 = 7; break;
                case 2: ngra1 = 0; ngra2 = 4; break;
                case 3: ngra1 = 1; ngra2 = 5; break;
                case 4: ngra1 = 2; ngra2 = 6; break;
                case 5: ngra1 = 3; ngra2 = 7; break;
                case 6: ngra1 = 0; ngra2 = 4; break;
                case 7: ngra1 = 1; ngra2 = 5; break;
            }

            switch (ngra1)
            {
                case 0: dir1 = new Point(0, 1); break;
                case 1: dir1 = new Point(-1, 1); break;
                case 2: dir1 = new Point(-1, 0); break;
                case 3: dir1 = new Point(-1, -1); break;
                case 4: dir1 = new Point(0, -1); break;
                case 5: dir1 = new Point(1, -1); break;
                case 6: dir1 = new Point(1, 0); break;
                case 7: dir1 = new Point(1, 1); break;
            }

            switch (ngra2)
            {
                case 0: dir2 = new Point(0, 1); break;
                case 1: dir2 = new Point(-1, 1); break;
                case 2: dir2 = new Point(-1, 0); break;
                case 3: dir2 = new Point(-1, -1); break;
                case 4: dir2 = new Point(0, -1); break;
                case 5: dir2 = new Point(1, -1); break;
                case 6: dir2 = new Point(1, 0); break;
                case 7: dir2 = new Point(1, 1); break;
            }

            cercanos[0] = BuscaPuntoEnDireccion(actual, dir1, atr.maxLongitudBuqueda);
            cercanos[1] = BuscaPuntoEnDireccion(actual, dir2, atr.maxLongitudBuqueda);

            return cercanos;
        }

        Point BuscaPuntoEnDireccion(Point actual, Point dir, int maxLongitud)
        {
            Point nuevoPunto = new Point(actual.X + dir.X, actual.Y + dir.Y);

            if (maxLongitud > 0 && !seSaleDeCoordenadas(nuevoPunto))
            {
                if (matriz[nuevoPunto.X, nuevoPunto.Y] == 0)
                {

                    if ((Atributos.getInstance().maxLongitudBuqueda - maxLongitud) > 3)
                    {
                        if (matriz[nuevoPunto.X + 1, nuevoPunto.Y] == 1)
                            nuevoPunto = new Point(nuevoPunto.X + 1, nuevoPunto.Y);

                        else if (matriz[nuevoPunto.X - 1, nuevoPunto.Y] == 1)
                            nuevoPunto = new Point(nuevoPunto.X - 1, nuevoPunto.Y);

                        else if (matriz[nuevoPunto.X, nuevoPunto.Y + 1] == 1)
                            nuevoPunto = new Point(nuevoPunto.X, nuevoPunto.Y + 1);

                        else if (matriz[nuevoPunto.X, nuevoPunto.Y - 1] == 1)
                            nuevoPunto = new Point(nuevoPunto.X, nuevoPunto.Y - 1);
                        else
                            nuevoPunto = BuscaPuntoEnDireccion(nuevoPunto, dir, maxLongitud - 1);
                    }
                    else
                        nuevoPunto = BuscaPuntoEnDireccion(nuevoPunto, dir, maxLongitud - 1);
                } 
            }
            else
            {
                nuevoPunto = puntoError;
            }

            return nuevoPunto;
        }

        bool seProlongaSuficiente(Point actual, Point prolongacion, int longitudLinea, Graphics g)
        {
            bool dev = false ;

            if (seSaleDeCoordenadas(prolongacion))
            {
                dev = false;
            }
            else if (longitudLinea > 0)
            {
                int x = prolongacion.X;
                int y = prolongacion.Y;

                int[,] nuevo = new int[,]{ { matriz[x-1,y-1], matriz[x-1,y], matriz[x-1,y+1]},
                                           { matriz[x,y-1], 0, matriz[x,y+1]},
                                           { matriz[x+1,y-1], matriz[x+1,y], matriz[x+1,y+1]} };

                int difX = actual.X - prolongacion.X + 1;
                int difY = actual.Y - prolongacion.Y + 1;

                nuevo[difX, difY] = 0;

                int i, j;
                bool enc = false;
                Point nuevoActual = new Point(prolongacion.X, prolongacion.Y);
                g.DrawEllipse(new Pen(Color.Green), nuevoActual.X, nuevoActual.Y, 1, 1);

                for (i = 0; i < 3 && !enc; i++)
                {
                    for (j = 0; j < 3 && !enc; j++)
                    {
                        if (nuevo[i, j] == 1)
                        {
                            enc = true;
                            Point nuevoProlongacion = new Point(prolongacion.X + i - 1, prolongacion.Y + j - 1);
                            dev = seProlongaSuficiente(nuevoActual, nuevoProlongacion, longitudLinea - 1, g);
                        }
                    }
                }

                if (!enc)
                    dev = false;
            }
            else
            {
                dev = true;
            }

            return dev;
        }

        bool seSaleDeCoordenadas(Point punto)
        {
            return (punto.X + 1 >= filas ||
                    punto.X - 1 <= 0 ||
                    punto.Y + 1 >= cols ||
                    punto.Y - 1 <= 0 );
        }

        /// <summary>
        /// Busca las Terminaciones y las imprime por panatalla
        /// </summary>
        void buscarTerminaciones()
        {
            int i, j;
            
            this.pasos[busquedaTerminaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[busquedaTerminaciones]);

            for (i = 1; i < filas-1; i++)
            {
                for (j = 1; j < cols-1; j++)
                {
                    if (matriz[i,j]==1 && buscarEnEntorno(i, j, 2))
                    {

                        a�adirTerminacionPotencial(i, j);

                        g.DrawEllipse(new Pen(atr.colorMinuciaNoFiable),
                            i - atr.radioCirculo/2, j - atr.radioCirculo/2, atr.radioCirculo, atr.radioCirculo);
                    }
                }
            }
        }
        /// <summary>
        /// Busca las bifurcaciones y las imprime por panatalla
        /// </summary>
        void buscarBifurcaciones()
        {
            int i, j;

            this.pasos[busquedaBifurcaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[busquedaBifurcaciones]);

            for (i = 1; i < filas - 1; i++)
            {
                for (j = 1; j < cols - 1; j++)
                {
                    if (matriz[i, j] == 1 && buscarEnEntorno(i, j, 4)
                        && numCompConexIgual(i,j,3))
                    {

                        a�adirBifurcacionPotencial(i, j);

                        g.DrawEllipse(new Pen(atr.colorMinuciaNoFiable),
                            i - atr.radioCirculo / 2, j - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
                    }
                }
            }
        }
        /// <summary>
        /// Detecta si una mascara centrada en un entorno de un punto
        /// tiene tres componentes conexas.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="matriz"></param>
        /// <returns></returns>
        bool numCompConexIgual(int x, int y, int numCompConexas)
        {
            int[,] nuevo = new int[,]{ { matriz[x-1,y-1], matriz[x-1,y], matriz[x-1,y+1]},
                                       { matriz[x,y-1], 0, matriz[x,y+1]},
                                       { matriz[x+1,y-1], matriz[x+1,y], matriz[x+1,y+1]} };

            int[,] etiquetas = new int[,] { { 0,0,0 },
                                            { 0,0,0 },
                                            { 0,0,0 }};
            int i, j;

            int contEtiqueta = 1;
            LinkedList<Point> listaPuntos = new LinkedList<Point>();

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (nuevo[i, j] == 1)
                    {

                        if (i == 0 && j == 0)
                        {
                            etiquetas[i, j] = contEtiqueta++;
                        }

                        else if (i == 0 && j>0)
                        {
                            if (nuevo[i, j - 1] == 1)
                                etiquetas[i, j] = etiquetas[i, j - 1];
                            else
                                etiquetas[i, j] = contEtiqueta++;

                        }

                        else if (j == 0 && i > 0)
                        {
                            if (nuevo[i - 1, j] == 1)
                                etiquetas[i, j] = etiquetas[i - 1, j];
                            else
                                etiquetas[i, j] = contEtiqueta++;
                        }

                        else if (i > 0)
                        {
                            if (nuevo[i - 1, j] == 1 && nuevo[i, j - 1] == 1)
                            {
                                // arriba negro y izquierda negro
                                etiquetas[i, j] = etiquetas[i - 1, j];
                                listaPuntos.AddFirst(new Point(etiquetas[i - 1, j], etiquetas[i, j - 1]));
                            }
                            else if (nuevo[i - 1, j] == 1 && nuevo[i, j - 1] == 0)
                            {
                                // arriba negro y izquierda blanco
                                etiquetas[i, j] = etiquetas[i - 1, j];
                            }
                            else if (nuevo[i - 1, j] == 0 && nuevo[i, j - 1] == 1)
                            {
                                // izquierda negro y arriba blanco
                                etiquetas[i, j] = etiquetas[i, j - 1];
                            }
                            else
                            {
                                // si ni arriba ni izquierda es negro doy una nueva etiqueta
                                etiquetas[i, j] = contEtiqueta++;
                            }
                        } 
                    }
                }
            }

            /*
            Console.WriteLine("Minucia " + x + "," + y + ":");
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    Console.Write(nuevo[i, j] + ",");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    Console.Write(etiquetas[i,j] + ",");
                }
                Console.Write("\n");
            }
            Console.WriteLine("-------------");
            */

            int[] vector = new int[contEtiqueta];

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (etiquetas[i,j]>=1)
                        vector[etiquetas[i, j]-1]++;   
                }
            }

            int compConexas = 0;

            for (i = 0; i < contEtiqueta; i++)
            {
                if (vector[i] > 0)
                    compConexas++; ;
            }

            return compConexas == numCompConexas;
        }

        /// <summary>
        /// Inserta en la lista de terminaciones potenciales el punto dado
        /// junto con datos de su entorno para poder despu�s clasificar
        /// la minucia como fiable o no fiable
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void a�adirTerminacionPotencial(int x, int y)
        {
            int[,] nuevo = new int[,]{ { matriz[x-1,y-1], matriz[x-1,y], matriz[x-1,y+1]},
                                       { matriz[x,y-1], 0, matriz[x,y+1]},
                                       { matriz[x+1,y-1], matriz[x+1,y], matriz[x+1,y+1]} };

            int i, j;
            bool enc = false;

            Point actual = new Point(x, y);
            Point prolongacion = new Point(0, 0);

            for (i = 0; i < 3 && !enc ; i++)
            {
                for (j = 0; j < 3 && !enc ; j++)
                {
                    if (nuevo[i, j] == 1)
                    {
                        enc = true;
                        prolongacion = new Point(x + i - 1, y + j - 1);
                    }
                }
            }

            terminacionesPotenciales.Add(new TerminacionPotencial(actual, prolongacion));
        }

        void a�adirBifurcacionPotencial(int x, int y)
        {
            int[,] nuevo = new int[,]{ { matriz[x-1,y-1], matriz[x-1,y], matriz[x-1,y+1]},
                                       { matriz[x,y-1], 0, matriz[x,y+1]},
                                       { matriz[x+1,y-1], matriz[x+1,y], matriz[x+1,y+1]} };

            int i, j;

            Point actual = new Point(x, y);
            Point[] prolongaciones = new Point[3];
            int puntero = 0;

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (nuevo[i, j] == 1)
                    {
                        prolongaciones[puntero++] = new Point(x + i - 1, y + j - 1);
                    }
                }
            }

            bifurcacionesPotenciales.Add(new BifurcacionPotencial(actual, prolongaciones));
        }

        /// <summary>
        /// dado un punto devuelve el numero de pixeles negros de su entrorno
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="matriz"></param>
        /// <param name="numPuntos"></param>
        /// <returns>cierto si el numero de pixeles negros es igual al pasado por parametro</returns>
        bool buscarEnEntorno(int x, int y, int numPuntos)
        {
            bool b = false;
            int i, j;
            int contador = 0;

            for (i = x - 1; i <= x + 1; i++)
            {
                for (j = y - 1; j <= y + 1; j++)
                {
                    contador += matriz[i, j];
                }
            }

            if (contador == numPuntos)
                b = true;

            return b;
        }
        /// <summary>
        /// Nos da un array de pasos
        /// </summary>
        /// <returns></returns>
        public Bitmap[] getPasos()
        {
            return pasos;
        }
    }
}