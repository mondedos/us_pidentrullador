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
        Bitmap[] pasos;
        int filas, cols;

        static int[,] direcciones = { { 3, 2, 1 },
                                      { 4, 9, 0 }, 
                                      { 5, 6, 7 } };

        static Point puntoError = new Point(0, 0);

        /// <summary>
        /// Estos atributos se utilizan para indexar los pasos
        /// </summary>
        public static int busquedaTerminaciones = 0;
        public static int compruebaTerminaciones = 1;
        public static int guardaTerminaciones = 2;

        public static int busquedaBifurcaciones = 3;
        public static int compruebaBifurcaciones = 4;
        public static int guardaBifurcaciones = 5;

        public static int muestraTodasMinucias = 6;
        public static int muestraDatosTextura = 7;
        public static int muestraDatosMinucia = 8;

        public static int totalPasos = 9;

        // Su tamaño es totalPasos + 1
        public static String [] textoPasos = new String[]{ 
            "Comprobar terminaciones",
            "Chequear terminaciones fiables",
            "Mostrar terminaciones fiables",
            "Comprobar bifurcaciones",
            "Chequear bifurcaciones fiables",
            "Mostrar bifurcaciones fiables",
            "Mostrar todas las minucias encontrada",
            "Mostrar datos relativos al descriptor de textura",
            "Mostrar datos relativos al descriptor de minucia",
            "Mostrar correspondencias de minucias fiables"
        };

        List<TerminacionPotencial> terminacionesPotenciales = new List<TerminacionPotencial>();
        List<BifurcacionPotencial> bifurcacionesPotenciales = new List<BifurcacionPotencial>();

        List<Point> terminacionesFiables = new List<Point>();
        List<Point> terminacionesPocoFiables = new List<Point>();
        List<Point> terminacionesNoFiables = new List<Point>();

        List<Point> bifurcacionesFiables = new List<Point>();
        List<Point> bifurcacionesPocoFiables = new List<Point>();
        List<Point> bifurcacionesNoFiables = new List<Point>();

        public List<Minucia> minucias = new List<Minucia>();

        /// <summary>
        /// Aplica los agoritmos de busqueda de terminaciones y bifurcaciones
        /// a una huella, con los atributos dados por el ususuario.
        /// </summary>
        /// <param name="huella"></param>
        /// <param name="atr"></param>
        public Tratamiento(Bitmap huella)
        {
            this.pasos = new Bitmap[totalPasos];
            this.huella = new Bitmap(huella);
            
            this.matriz = Adaptador.Adaptar(this.huella);
            this.filas = huella.Width;
            this.cols = huella.Height;

            Matriz m = Matriz.getInstance();
            m.matriz = this.matriz;
            m.filas = this.filas;
            m.cols = this.cols;

            buscarTerminaciones();
            comprobarTerminaciones();
            guardarTerminaciones();

            buscarBifurcaciones();
            comprobarBifurcaciones();
            guardarBifurcaciones();

            // En este punto ya tenemos guardadas todas las minucias
            // Llamamos a un método auxiliar para actualizar sus minucias vecinas
            // También actualizamos los índices de cada minucia
            actualizarVecinos();

            mostrarTodasMinucias();
            mostrarDatosTextura();
            mostrarDatosMinucia();
        }

        /// <summary>
        /// 
        /// Busca las Terminaciones y las imprime por panatalla
        /// </summary>
        void buscarTerminaciones()
        {
            int i, j;

            this.pasos[busquedaTerminaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[busquedaTerminaciones]);
            Atributos atr = Atributos.getInstance();

            for (i = 1; i < filas - 1; i++)
            {
                for (j = 1; j < cols - 1; j++)
                {
                    if (matriz[i, j] == 1 && buscarEnEntorno(i, j, 2))
                    {

                        añadirTerminacionPotencial(i, j);

                        g.DrawEllipse(new Pen(atr.colorMinuciaNoFiable),
                            i - atr.radioCirculo / 2, j - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// Busca las bifurcaciones y las imprime por pantalla
        /// </summary>
        void buscarBifurcaciones()
        {
            int i, j;

            this.pasos[busquedaBifurcaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[busquedaBifurcaciones]);
            Atributos atr = Atributos.getInstance();

            for (i = 1; i < filas - 1; i++)
            {
                for (j = 1; j < cols - 1; j++)
                {
                    if (matriz[i, j] == 1 && buscarEnEntorno(i, j, 4)
                        && numCompConexIgual(i, j, 3))
                    {

                        añadirBifurcacionPotencial(i, j);

                        g.DrawEllipse(new Pen(atr.colorMinuciaNoFiable),
                            i - atr.radioCirculo / 2, j - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// Comprueba que las terminaciones encontradas son correctas
        /// Para ello recorre la línea que llega a la terminación y 4
        /// más de píxeles cercanos
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

                    List<Point> visitadosTemporal;

                    bool prol0 = false, prol1 = false, prol2 = false, prol3 = false, prol4 = false;

                    visitadosTemporal = new List<Point>();
                    if (seProlongaSuficiente(tp.actual, tp.prolongacion, visitadosTemporal, atr.longitudLinea, g))
                        prol0 = true;

                    visitadosTemporal = new List<Point>();
                    if (unidos1[0] != puntoError && seProlongaSuficiente(cercano1, unidos1[0], visitadosTemporal, atr.longitudLinea, g))
                        prol1 = true;

                    visitadosTemporal = new List<Point>();
                    if (unidos1[1] != puntoError && seProlongaSuficiente(cercano1, unidos1[1], visitadosTemporal, atr.longitudLinea, g))
                        prol2 = true;

                    visitadosTemporal = new List<Point>();
                    if (unidos2[0] != puntoError && seProlongaSuficiente(cercano2, unidos2[0], visitadosTemporal, atr.longitudLinea, g))
                        prol3 = true;

                    visitadosTemporal = new List<Point>();
                    if (unidos2[1] != puntoError && seProlongaSuficiente(cercano2, unidos2[1], visitadosTemporal, atr.longitudLinea, g))
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
        /// 
        /// Comprueba que las bifurcaciones encontradas son correctas
        /// Para ello recorre las 3 líneas que salen del punto de bifurcación
        /// más 4 más de pixeles cercanos
        /// </summary>
        void comprobarBifurcaciones()
        {
            this.pasos[compruebaBifurcaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[compruebaBifurcaciones]);
            Atributos atr = Atributos.getInstance();

            foreach (BifurcacionPotencial bp in bifurcacionesPotenciales)
            {
                bool fiable = true;
                Point[] prolongaciones = bp.prolongaciones;

                int gradiente1 = direcciones[prolongaciones[0].X - bp.actual.X + 1, prolongaciones[0].Y - bp.actual.Y + 1];
                int gradiente2 = direcciones[prolongaciones[1].X - bp.actual.X + 1, prolongaciones[1].Y - bp.actual.Y + 1];
                int gradiente3 = direcciones[prolongaciones[2].X - bp.actual.X + 1, prolongaciones[2].Y - bp.actual.Y + 1];

                Point[] cercanos1 = PuntosCercanosDeOtraLinea(bp.actual, gradiente1);
                Point[] cercanos2 = PuntosCercanosDeOtraLinea(bp.actual, gradiente2);
                Point[] cercanos3 = PuntosCercanosDeOtraLinea(bp.actual, gradiente3);

                Point cercanoTemp1 = puntoError;
                Point cercanoTemp2 = puntoError;

                if (cercanos1[0] != puntoError && cercanos1[1] != puntoError)
                {
                    cercanoTemp1 = cercanos1[0];
                    cercanoTemp2 = cercanos1[1];
                }
                else if (cercanos2[0] != puntoError && cercanos2[1] != puntoError)
                {
                    cercanoTemp1 = cercanos2[0];
                    cercanoTemp2 = cercanos2[1];
                }
                else if (cercanos3[0] != puntoError && cercanos3[1] != puntoError)
                {
                    cercanoTemp1 = cercanos3[0];
                    cercanoTemp2 = cercanos3[1];
                }

                if (cercanoTemp1 != puntoError && cercanoTemp2 != puntoError)
                {
                    g.DrawLine(new Pen(atr.colorLinea), bp.actual, cercanoTemp1);
                    g.DrawLine(new Pen(atr.colorLinea), bp.actual, cercanoTemp2);

                    Point[] unidos1 = buscaUnidos(cercanoTemp1);
                    Point[] unidos2 = buscaUnidos(cercanoTemp2);
                    List<Point> visitadosTemporal;

                    bool prol0 = false, prol1 = false, prol2 = false, prol3 = false, prol4 = false, prol5 = false, prol6 = false;

                    visitadosTemporal = new List<Point>();
                    visitadosTemporal.Add(prolongaciones[1]);
                    visitadosTemporal.Add(prolongaciones[2]);
                    if (seProlongaSuficiente(bp.actual, prolongaciones[0], visitadosTemporal, atr.longitudLinea, g))
                        prol0 = true;

                    visitadosTemporal = new List<Point>();
                    visitadosTemporal.Add(prolongaciones[0]);
                    visitadosTemporal.Add(prolongaciones[2]);
                    if (seProlongaSuficiente(bp.actual, prolongaciones[1], visitadosTemporal, atr.longitudLinea, g))
                        prol1 = true;

                    visitadosTemporal = new List<Point>();
                    visitadosTemporal.Add(prolongaciones[0]);
                    visitadosTemporal.Add(prolongaciones[1]);
                    if (seProlongaSuficiente(bp.actual, prolongaciones[2], visitadosTemporal, atr.longitudLinea, g))
                        prol2 = true;

                    visitadosTemporal = new List<Point>();
                    if (unidos1[0] != puntoError && seProlongaSuficiente(cercanoTemp1, unidos1[0], visitadosTemporal, atr.longitudLinea, g))
                        prol3 = true;

                    visitadosTemporal = new List<Point>();
                    if (unidos1[1] != puntoError && seProlongaSuficiente(cercanoTemp1, unidos1[1], visitadosTemporal, atr.longitudLinea, g))
                        prol4 = true;

                    visitadosTemporal = new List<Point>();
                    if (unidos2[0] != puntoError && seProlongaSuficiente(cercanoTemp2, unidos2[0], visitadosTemporal, atr.longitudLinea, g))
                        prol5 = true;

                    visitadosTemporal = new List<Point>();
                    if (unidos2[1] != puntoError && seProlongaSuficiente(cercanoTemp2, unidos2[1], visitadosTemporal, atr.longitudLinea, g))
                        prol6 = true;

                    if (prol0 && prol1 && prol2 && prol3 && prol4 && prol5 && prol6)
                    {
                        bifurcacionesFiables.Add(bp.actual);
                        g.DrawEllipse(new Pen(atr.colorBifurcacionFiable),
                                bp.actual.X - atr.radioCirculo / 2, bp.actual.Y - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
                    }
                    else
                        fiable = false;

                }
                else
                    fiable = false;

                if (!fiable)
                {
                    bifurcacionesPocoFiables.Add(bp.actual);
                    g.DrawRectangle(new Pen(atr.colorBifurcacionPocoFiable),
                            bp.actual.X - atr.radioCirculo / 2, bp.actual.Y - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void guardarTerminaciones()
        {
            this.pasos[guardaTerminaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[guardaTerminaciones]);
            Atributos atr = Atributos.getInstance();

            foreach (Point terminacion in terminacionesFiables)
            {
                minucias.Add(new Minucia(terminacion.X, terminacion.Y,Minucia.Fiable,Minucia.Terminacion));
                g.DrawEllipse(new Pen(atr.colorTerminacionFiable),
                    terminacion.X - atr.radioCirculo / 2, terminacion.Y - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
            }

            foreach (Point terminacion in terminacionesPocoFiables)
            {
                minucias.Add(new Minucia(terminacion.X, terminacion.Y,Minucia.PocoFiable,Minucia.Terminacion));
            }

            foreach (Point terminacion in terminacionesNoFiables)
            {
                minucias.Add(new Minucia(terminacion.X, terminacion.Y, Minucia.NoFiable, Minucia.Terminacion));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void guardarBifurcaciones()
        {
            this.pasos[guardaBifurcaciones] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[guardaBifurcaciones]);
            Atributos atr = Atributos.getInstance();

            foreach (Point bifurcacion in bifurcacionesFiables)
            {
                minucias.Add(new Minucia(bifurcacion.X, bifurcacion.Y, Minucia.Fiable, Minucia.Bifurcacion));
                g.DrawEllipse(new Pen(atr.colorBifurcacionFiable),
                    bifurcacion.X - atr.radioCirculo / 2, bifurcacion.Y - atr.radioCirculo / 2, atr.radioCirculo, atr.radioCirculo);
            }

            foreach (Point bifurcacion in bifurcacionesPocoFiables)
            {
                minucias.Add(new Minucia(bifurcacion.X, bifurcacion.Y, Minucia.PocoFiable, Minucia.Bifurcacion));
            }

            foreach (Point bifurcacion in bifurcacionesNoFiables)
            {
                minucias.Add(new Minucia(bifurcacion.X, bifurcacion.Y, Minucia.NoFiable, Minucia.Bifurcacion));
            }
        }

        void mostrarTodasMinucias()
        {
            this.pasos[muestraTodasMinucias] = new Bitmap(huella);
            Graphics g = Graphics.FromImage(pasos[muestraTodasMinucias]);
            Atributos atr = Atributos.getInstance();

            foreach (Minucia minucia in minucias)
            {
                Color cFiable = Color.Black;
                Color cPocoFiable = Color.Black;

                switch (minucia.tipo)
                {
                    case Minucia.Terminacion: cFiable = atr.colorTerminacionFiable;
                                              cPocoFiable = atr.colorTerminacionPocoFiable;
                                              break;
                    case Minucia.Bifurcacion: cFiable = atr.colorBifurcacionFiable;
                                              cPocoFiable = atr.colorBifurcacionPocoFiable;
                                              break;
                }

                switch (minucia.fiabilidad)
                {
                    case Minucia.Fiable:
                        g.DrawEllipse(new Pen(cFiable),
                                minucia.x - atr.radioCirculo / 2, minucia.y - atr.radioCirculo / 2, 
                                atr.radioCirculo, atr.radioCirculo); 
                    
                    break;
                    case Minucia.PocoFiable:
                        g.DrawRectangle(new Pen(cPocoFiable),
                            minucia.x - atr.radioCirculo / 2, minucia.y - atr.radioCirculo / 2, 
                            atr.radioCirculo, atr.radioCirculo); 
                    break;
                }
            }
        }

        void mostrarDatosTextura()
        {
            this.pasos[muestraDatosTextura] = new Bitmap(this.pasos[muestraTodasMinucias]);
            Graphics g = Graphics.FromImage(pasos[muestraDatosTextura]);
            Atributos atr = Atributos.getInstance();

            // Selecciona las minucias que se van a imprimir por pantalla
            int numMinucia = 0;
            Random r = new Random();
            List<int> lista = new List<int>();
            for (int i = 0; i < atr.numEjemplos; i++)
                lista.Add(r.Next(minucias.Count));

            foreach (Minucia minucia in minucias)
            {               
                if (seEncuentraEnLista(lista, numMinucia))
                {
                    dibujaCruz(g, minucia);

                    foreach (Circulo circulo in minucia.circulos)
                    {                    
                        g.DrawEllipse(new Pen(atr.colorCirculo), 
                            minucia.x - circulo.radio, minucia.y - circulo.radio, circulo.radio*2, circulo.radio*2);

                        foreach (Punto punto in circulo.puntos)
                        {
                            g.DrawLine(new Pen(Color.Red), minucia.x, minucia.y, punto.x, punto.y);

                            if (punto.esValido)
                                g.FillRectangle(atr.colorRellenoFinPixelCercano,
                                    punto.x - atr.tamEntornoPunto / 2, punto.y - atr.tamEntornoPunto / 2,
                                    atr.tamEntornoPunto, atr.tamEntornoPunto);

                        }
                    }
                }

                numMinucia++;
            }
        }

        void mostrarDatosMinucia()
        {
            this.pasos[muestraDatosMinucia] = new Bitmap(this.pasos[muestraTodasMinucias]);
            Graphics g = Graphics.FromImage(pasos[muestraDatosMinucia]);
            Atributos atr = Atributos.getInstance();

            // Selecciona las minucias que se van a imprimir por pantalla
            int numMinucia = 0;
            Random r = new Random();
            List<int> lista = new List<int>();
            for (int i = 0; i < atr.numEjemplos; i++)
                lista.Add(r.Next(minucias.Count));

            foreach (Minucia minucia in minucias)
            {            
                if (seEncuentraEnLista(lista, numMinucia))
                {

                    dibujaCruz(g, minucia);

                    g.DrawEllipse(new Pen(atr.colorCirculo),
                                minucia.x - atr.radioVecinos, minucia.y - atr.radioVecinos,
                                atr.radioVecinos*2, atr.radioVecinos*2);

                    foreach (Minucia m in minucia.vecinos)
                    {
                        g.FillEllipse(atr.colorRellenoFinPixelCercano,
                                    m.x - atr.radioCirculo / 4, m.y - atr.radioCirculo / 4,
                                    atr.radioCirculo / 2, atr.radioCirculo / 2);

                        //g.DrawLine(new Pen(Color.Red), minucia.x, minucia.y, m.x, m.y);
                    }
                }

                numMinucia++;
            }
        }

        void dibujaCruz(Graphics g, Minucia minucia)
        {
            Atributos atr = Atributos.getInstance();

            g.DrawLine(new Pen(atr.colorCruz),
                minucia.x - atr.radioCirculo / 2, minucia.y - atr.radioCirculo / 2,
                minucia.x + atr.radioCirculo / 2, minucia.y + atr.radioCirculo / 2);

            g.DrawLine(new Pen(atr.colorCruz),
                minucia.x - atr.radioCirculo / 2, minucia.y + atr.radioCirculo / 2,
                minucia.x + atr.radioCirculo / 2, minucia.y - atr.radioCirculo / 2);

            g.DrawRectangle(new Pen(atr.colorCruz),
                minucia.x - atr.radioCirculo / 2, minucia.y - atr.radioCirculo / 2,
                atr.radioCirculo, atr.radioCirculo);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        // De ahí para abajo son funciones auxiliares
        ////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Actualiza los vecinos de una minucia dada
        /// Ha de ser llamado cuando se han encontrado todas las minucias
        /// </summary>
        void actualizarVecinos()
        {
            int i = 0;
            foreach (Minucia minucia in minucias)
            {
                minucia.indice = i++;
                minucia.agregarVecinos(minucias);
            }
        }

        /// <summary>
        /// Devuelve un array de los dos pixeles cercanos a uno dado que son negros y no son él mismo
        /// Servirá más adelanta para buscar las prolongaciones y poder lanzar el método
        /// seProlongaSuficiente(...)
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
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
            int pasos = Atributos.getInstance().maxLongitudBuqueda - maxLongitud;
            Atributos atr = Atributos.getInstance();

            if (maxLongitud > 0 && !Funcion.seSaleDeCoordenadas(nuevoPunto.X, nuevoPunto.Y, filas, cols, 1))
            {
                if (pasos <= atr.minPasosAntesDeBuscarPunto)
                {
                    nuevoPunto = BuscaPuntoEnDireccion(nuevoPunto, dir, maxLongitud - 1);
                }
                else
                {
                    if (matriz[nuevoPunto.X, nuevoPunto.Y] == 0)
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
                }  
            }
            else
            {
                nuevoPunto = puntoError;
            }

            return nuevoPunto;
        }

        /// <summary>
        /// Comprueba que la línea a partir de una minucia se prolonga longitudLinea pixels
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="prolongacion"></param>
        /// <param name="longitudLinea"></param>
        /// <param name="g"></param>
        /// <returns>Devuelve verdadero si la condición es cierta</returns>
        bool seProlongaSuficiente(Point actual, Point prolongacion, List<Point> visitados, int longitudLinea, Graphics g)
        {
            bool dev = false ;
            Atributos atr = Atributos.getInstance();

            if (Funcion.seSaleDeCoordenadas(prolongacion.X, prolongacion.Y, filas, cols, 1))
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
                g.DrawEllipse(new Pen(atr.colorPixelCercano), nuevoActual.X, nuevoActual.Y, 1, 1);

                for (i = 0; i < 3 && !enc; i++)
                {
                    for (j = 0; j < 3 && !enc; j++)
                    {
                        if (nuevo[i, j] == 1 && !seEncuentraEnLista(visitados,x+i-1,y+j-1))
                        {
                            enc = true;
                            Point nuevoProlongacion = new Point(prolongacion.X + i - 1, prolongacion.Y + j - 1);
                            visitados.Add(nuevoProlongacion);
                            dev = seProlongaSuficiente(nuevoActual, nuevoProlongacion, visitados, longitudLinea - 1, g);
                        }
                    }
                }
            }
            else
            {
                dev = true;
                g.FillRectangle(atr.colorRellenoFinPixelCercano,
                    actual.X - atr.radioCirculo / 4, actual.Y - atr.radioCirculo / 4, atr.radioCirculo / 2, atr.radioCirculo / 2);
            }

            return dev;
        }

        bool seEncuentraEnLista(List<Point> visitados, int x, int y)
        {
            bool seEncuentra = false;
            foreach (Point punto in visitados)
            {
                if (punto.X == x && punto.Y == y)
                {
                    seEncuentra = true;
                    break;
                }
            }
            return seEncuentra;
        }

        bool seEncuentraEnLista(List<int> lista, int num)
        {
            bool seEncuentra = false;

            foreach (int x in lista)
            {
                if (x == num)
                {
                    seEncuentra = true;
                    break;
                }
            }

            return seEncuentra;
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
        /// junto con datos de su entorno para poder después clasificar
        /// la minucia como fiable o no fiable
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void añadirTerminacionPotencial(int x, int y)
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

        void añadirBifurcacionPotencial(int x, int y)
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