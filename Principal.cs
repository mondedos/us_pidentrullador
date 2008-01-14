using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using kernel;

namespace TratamientoHuellasDactilares
{
    public partial class Principal : Form
    {
        Bitmap RImagen;
        Bitmap[] RPasos;
        string[] RTxtDescripcion;

        Bitmap DImagen;
        Bitmap[] DPasos;
        string[] DTxtDescripcion;

        Bitmap CImagen0, CImagen1;
        Bitmap[] CPasos0, CPasos1;
        string[] CTxtDescripcion;

        int indiceImagenC;
        bool cargadaCImagen0, cargadaCImagen1;

        string DirectorioDefecto;
        bool noRamas;
        bool detalles;

        bool DManipulandoParametros;
        bool CManipulandoParametros;

        Atributos atributos;

        #region Inicializacion
        public Principal()
        {
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {

            AbrirLog.Enabled = false;

            establecerAnchura(Anchura.normal);
            establecerAltura(Altura.normal);
            detalles = false;

            ListaFiltros.Items.Add(new EnvoltorioFiltro(EnvoltorioFiltro.Tipo.Mediana, 0));

            RDesactivarEjecucion();
            DDesactivarEjecucion();
            CDesactivarEjecucion();

            indiceImagenC = 0;
            cargadaCImagen0 = false;
            cargadaCImagen1 = false;

            establecerDirectorioDefecto();

            noRamas = false;

            DManipulandoParametros = false;
            CManipulandoParametros = false;

            comprobarResolucion();

            //MODIFICARA
            inicializarAtributosPorDefecto();

            REstablecerParametrosDefecto();

            DEstablecerParametrosDefecto();
            DEstablecerAyudasParametros();

            CEstablecerParametrosDefecto();
            CEstablecerAyudasParametros();

            pulsado = false;
        }

        private void inicializarAtributosPorDefecto()
        {
            atributos = Atributos.getInstance();

            atributos.colorTerminacionFiable = Color.Blue;
            atributos.colorTerminacionPocoFiable = Color.BlueViolet;
            atributos.colorBifurcacionFiable = Color.Green;
            atributos.colorBifurcacionPocoFiable = Color.Olive;
            atributos.colorMinuciaNoFiable = Color.Red;
            atributos.colorPixelCercano = Color.Brown;
            atributos.colorLinea = Color.Red;
            atributos.colorRellenoFinPixelCercano = Brushes.Fuchsia;
            atributos.colorCirculo = Color.Green;
            atributos.colorCruz = Color.Red;
            atributos.radioCirculo = 8;
        }

        private void comprobarResolucion()
        {
            int anchura = Screen.PrimaryScreen.Bounds.Width;
            int altura = Screen.PrimaryScreen.Bounds.Height;

            if (anchura < 1280 || altura < 960)
            {
                MessageBox.Show("Se ha detectado que su resolución actual (" + anchura + "x" + altura + ") "
                            + "es insuficiente para que los controles del programa se muestren correctamente "
                            + "en su pantalla. Se recomienda usar 1280x960 o una resolucion mayor. "
                            + "Recuerde que puede arrastrar la ventana haciendo click en cualquier parte de ella",
                            "Aumente su resolución.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                this.AutoScroll = true;
            }
        }

        private void establecerDirectorioDefecto()
        {
            string path = System.Environment.CurrentDirectory + "\\Ejemplos";

            if (Directory.Exists(path))
                DirectorioDefecto = path;
            else
                DirectorioDefecto = System.Environment.GetFolderPath(
                        System.Environment.SpecialFolder.MyPictures);
        }

        #endregion Inicializacion

        #region Reescalado de la ventana principal

        private enum Anchura
        {
            normal,
            comparacion
        }

        private enum Altura
        {
            normal,
            detalles
        }

        private void etiquetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (etiquetas.SelectedIndex)
            {
                case 0:
                    establecerAnchura(Anchura.normal);
                    break;
                case 1:
                    establecerAnchura(Anchura.normal);
                    break;
                case 2:
                    establecerAnchura(Anchura.comparacion);
                    break;
            }
        }

        private void establecerAnchura(Anchura anchura)
        {
            switch (anchura)
            {
                case Anchura.normal:
                    Width = 885;
                    break;
                case Anchura.comparacion:
                    Width = 1262;
                    break;
            }
        }

        private void establecerAltura(Altura altura)
        {
            switch (altura)
            {
                case Altura.normal:
                    Height = 698;
                    break;
                case Altura.detalles:
                    Height = 815;
                    break;
            }
        }

        private void RDetalles_Click(object sender, EventArgs e)
        {
            clickDetalles();
        }

        private void DDetalles_Click(object sender, EventArgs e)
        {
            clickDetalles();
        }

        private void CDetalles_Click(object sender, EventArgs e)
        {
            clickDetalles();
        }

        private void clickDetalles()
        {
            detalles = !detalles;

            if (detalles)
            {
                RDetalles.Image = Properties.Resources.MenosDetalles;
                DDetalles.Image = Properties.Resources.MenosDetalles;
                CDetalles.Image = Properties.Resources.MenosDetalles;
                establecerAltura(Altura.detalles);
            }
            else
            {
                establecerAltura(Altura.normal);
                RDetalles.Image = Properties.Resources.MasDetalles;
                DDetalles.Image = Properties.Resources.MasDetalles;
                CDetalles.Image = Properties.Resources.MasDetalles;
            }
        }

        #endregion Reescalado de la ventana principal

        #region MoverFormulario
        bool pulsado;

        int inicialX;
        int inicialY;

        private void realce_MouseDown(object sender, MouseEventArgs e)
        {
            pulsar(e);
        }

        private void realce_MouseUp(object sender, MouseEventArgs e)
        {
            levantar();
        }

        private void realce_MouseMove(object sender, MouseEventArgs e)
        {
            mover(e);
        }

        private void descriptores_MouseDown(object sender, MouseEventArgs e)
        {
            pulsar(e);
        }

        private void descriptores_MouseUp(object sender, MouseEventArgs e)
        {
            levantar();
        }

        private void descriptores_MouseMove(object sender, MouseEventArgs e)
        {
            mover(e);
        }

        private void comparacion_MouseDown(object sender, MouseEventArgs e)
        {
            pulsar(e);
        }

        private void comparacion_MouseUp(object sender, MouseEventArgs e)
        {
            levantar();
        }

        private void comparacion_MouseMove(object sender, MouseEventArgs e)
        {
            mover(e);
        }

        private void RCuadroImagen_MouseDown(object sender, MouseEventArgs e)
        {
            pulsar(e);
        }

        private void RCuadroImagen_MouseUp(object sender, MouseEventArgs e)
        {
            levantar();
        }

        private void RCuadroImagen_MouseMove(object sender, MouseEventArgs e)
        {
            mover(e);
        }


        private void DCuadroImagen_MouseDown(object sender, MouseEventArgs e)
        {
            pulsar(e);
        }

        private void DCuadroImagen_MouseUp(object sender, MouseEventArgs e)
        {
            levantar();
        }

        private void DCuadroImagen_MouseMove(object sender, MouseEventArgs e)
        {
            mover(e);
        }

        private void etiquetas_MouseDown(object sender, MouseEventArgs e)
        {
            pulsar(e);
        }

        private void etiquetas_MouseUp(object sender, MouseEventArgs e)
        {
            levantar();
        }

        private void etiquetas_MouseMove(object sender, MouseEventArgs e)
        {
            mover(e);
        }

        private void CCuadroImagen0_MouseDown(object sender, MouseEventArgs e)
        {
            pulsar(e);
        }

        private void CCuadroImagen0_MouseUp(object sender, MouseEventArgs e)
        {
            levantar();
        }

        private void CCuadroImagen0_MouseMove(object sender, MouseEventArgs e)
        {
            mover(e);
        }

        private void CCuadroImagen1_MouseDown(object sender, MouseEventArgs e)
        {
            pulsar(e);
        }

        private void CCuadroImagen1_MouseUp(object sender, MouseEventArgs e)
        {
            levantar();
        }

        private void CCuadroImagen1_MouseMove(object sender, MouseEventArgs e)
        {
            mover(e);
        }

        private void pulsar(MouseEventArgs e)
        {
            pulsado = true;

            inicialX = e.X;
            inicialY = e.Y;
        }

        private void levantar()
        {
            pulsado = false;
        }

        private void mover(MouseEventArgs e)
        {
            if (pulsado)
            {
                int formX = this.Location.X;
                int formY = this.Location.Y;

                int mx = e.X;
                int my = e.Y;

                this.Location = new Point(formX + mx - inicialX,
                                          formY + my - inicialY);
            }
        }

        #endregion MoverFormulario

        #region Control de las barras de pasos

        private void RBarraPasos_ValueChanged(object sender, EventArgs e)
        {
            RCuadroImagen.Size = RPasos[RBarraPasos.Value].Size;
            RCuadroImagen.Image = RPasos[RBarraPasos.Value];
            RDescripcion.Text = RTxtDescripcion[RBarraPasos.Value];
        }

        private void RPrimero_Click(object sender, EventArgs e)
        {
            RBarraPasos.Value = 0;
        }

        private void RAnterior_Click(object sender, EventArgs e)
        {
            if (RBarraPasos.Value > 0)
                RBarraPasos.Value--;
        }

        private void RSiguiente_Click(object sender, EventArgs e)
        {
            if (RBarraPasos.Value < RBarraPasos.Maximum)
                RBarraPasos.Value++;
        }

        private void RUltimo_Click(object sender, EventArgs e)
        {
            RBarraPasos.Value = RBarraPasos.Maximum;
        }

        private void DBarraPasos_ValueChanged(object sender, EventArgs e)
        {
            DCuadroImagen.Image = DPasos[DBarraPasos.Value];
            DDescripcion.Text = DTxtDescripcion[DBarraPasos.Value];
        }

        private void DPrimero_Click(object sender, EventArgs e)
        {
            DBarraPasos.Value = 0;
        }

        private void DAnterior_Click(object sender, EventArgs e)
        {
            if (DBarraPasos.Value > 0)
                DBarraPasos.Value--;
        }

        private void DSiguiente_Click(object sender, EventArgs e)
        {
            if (DBarraPasos.Value < DBarraPasos.Maximum)
                DBarraPasos.Value++;
        }

        private void DUltimo_Click(object sender, EventArgs e)
        {
            DBarraPasos.Value = DBarraPasos.Maximum;
        }

        private void CBarraPasos_ValueChanged(object sender, EventArgs e)
        {
            CCuadroImagen0.Image = CPasos0[CBarraPasos.Value];
            CCuadroImagen1.Image = CPasos1[CBarraPasos.Value];
            CDescripcion.Text = CTxtDescripcion[CBarraPasos.Value];
        }

        private void CPrimero_Click(object sender, EventArgs e)
        {
            barraACero();
        }

        private void barraACero()
        {
            CBarraPasos.Value = 0;
        }

        private void CAnterior_Click(object sender, EventArgs e)
        {
            if (CBarraPasos.Value > 0)
                CBarraPasos.Value--;
        }

        private void CSiguiente_Click(object sender, EventArgs e)
        {
            if (CBarraPasos.Value < CBarraPasos.Maximum)
                CBarraPasos.Value++;
        }

        private void CUltimo_Click(object sender, EventArgs e)
        {
            CBarraPasos.Value = CBarraPasos.Maximum;
        }

        #endregion Botones de control de las barras de pasos

        #region Control de los parametros del realce

        private void MediaRadio_CheckedChanged(object sender, EventArgs e)
        {
            MediaBarra.Enabled = !MediaBarra.Enabled;
            MediaEtiqueta.Enabled = !MediaEtiqueta.Enabled;
        }

        private void FourierRadio_CheckedChanged(object sender, EventArgs e)
        {
            FourierBarra.Enabled = !FourierBarra.Enabled;
            FourierEtiqueta.Enabled = !FourierEtiqueta.Enabled;
        }

        private void AgregarBoton_Click(object sender, EventArgs e)
        {
            EnvoltorioFiltro ef = null;

            if (MedianaRadio.Checked)
                ef = new EnvoltorioFiltro(EnvoltorioFiltro.Tipo.Mediana, 0);
            else if (MediaRadio.Checked)
            {
                int param = MediaBarra.Value * 2 + 3;
                ef = new EnvoltorioFiltro(EnvoltorioFiltro.Tipo.Media, param);
            }
            else if (FourierRadio.Checked)
            {
                int param = 0;

                switch (FourierBarra.Value)
                {
                    case 0:
                        param = 256 / 6;
                        break;
                    case 1:
                        param = 256 / 5;
                        break;
                    case 2:
                        param = 256 / 3;
                        break;
                }

                ef = new EnvoltorioFiltro(EnvoltorioFiltro.Tipo.Fourier, param);
            }

            ListaFiltros.Items.Add(ef);

            ListaFiltros.SelectedIndex = ListaFiltros.Items.Count - 1;
        }

        private void QuitarBoton_Click(object sender, EventArgs e)
        {
            if(ListaFiltros.SelectedIndex>-1)
                ListaFiltros.Items.RemoveAt(ListaFiltros.SelectedIndex);

            ListaFiltros.SelectedIndex = ListaFiltros.Items.Count - 1;
        }

        private void NoRamas_CheckedChanged(object sender, EventArgs e)
        {
            noRamas = NoRamas.Checked;
        }

        private void RDefecto_Click(object sender, EventArgs e)
        {
            REstablecerParametrosDefecto();
        }

        private void REstablecerParametrosDefecto()
        {
            umbralBinarizacion.Value = 128;

            ListaFiltros.Items.Clear();
            ListaFiltros.Items.Add(new EnvoltorioFiltro(EnvoltorioFiltro.Tipo.Mediana, 0));

            NoRamas.Checked = true;
        }

        #endregion Control de los parametros del realce

        #region Ejecucion del realce

        private void REjecutar_Click(object sender, EventArgs e)
        {
            atributos.umbralBinarizacion = (byte)umbralBinarizacion.Value;

            RComenzarEjecucion();
            RBackWorker.RunWorkerAsync();
        }

        private void RBackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            EnvoltorioFiltro[] filtros = new EnvoltorioFiltro[ListaFiltros.Items.Count];
            ListaFiltros.Items.CopyTo(filtros, 0);

            int numPasosFiltro = 0;

            foreach (EnvoltorioFiltro ev in filtros)
            {
                if (ev.TipoFiltro == EnvoltorioFiltro.Tipo.Fourier)
                    numPasosFiltro += 3;
                else
                    numPasosFiltro++;
            }

            int numPasos = 1 + 4 + numPasosFiltro;

            if (noRamas)
                numPasos += 2;

            int progreso = 100 / numPasos;

            int i = 0;

            RPasos = new Bitmap[numPasos];
            RTxtDescripcion = new string[numPasos];

            Realce r = new Realce(RImagen);

            RPasos[i] = RImagen;

            RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.Original, 0);
            i++;
            RBackWorker.ReportProgress(progreso);

            RPasos[i] = r.ReescaladoBicubico(512, 512);
            RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.Reescalado, 0);
            i++;
            RBackWorker.ReportProgress(progreso);

            RPasos[i] = r.EscalaGrises();
            RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.EscalaGrises, 0);
            i++;
            RBackWorker.ReportProgress(progreso);

            foreach (EnvoltorioFiltro filtro in filtros)
            {
                switch (filtro.TipoFiltro)
                {
                    case EnvoltorioFiltro.Tipo.Mediana:
                        RPasos[i] = r.FiltroMediana();
                        RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.FiltroMediana, 0);
                        i++;
                        RBackWorker.ReportProgress(progreso);
                        break;
                    case EnvoltorioFiltro.Tipo.Media:
                        RPasos[i] = r.FiltroMedia(filtro.Parametro);
                        RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.FiltroMedia,
                                                                        filtro.Parametro);
                        i++;
                        RBackWorker.ReportProgress(progreso);
                        break;
                    case EnvoltorioFiltro.Tipo.Fourier:
                        RPasos[i] = r.ImagenCompleja();
                        RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.FiltroFourierTransformada, 0);
                        i++;
                        RBackWorker.ReportProgress(progreso);
                        RPasos[i] = r.FiltroPasoBajo(filtro.Parametro);
                        RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.FiltroFourierPasoBajo, filtro.Parametro);
                        i++;
                        RBackWorker.ReportProgress(progreso);
                        RPasos[i] = r.ImagenNormal();
                        RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.FiltroFourierInversa, 0);
                        i++;
                        RBackWorker.ReportProgress(progreso);
                        break;
                }
            }

            RPasos[i] = r.BinarizacionIterativa();
            RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.Binarizacion, r.umbral);
            i++;
            RBackWorker.ReportProgress(progreso);

            if (noRamas)
            {
                RPasos[i] = r.FiltroMediana();
                RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.FiltroMedianaNoRamas,0);
                i++;
                RBackWorker.ReportProgress(progreso);

                RPasos[i] = r.BinarizacionIterativa();
                RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.Binarizacion, r.umbral);
                i++;
                RBackWorker.ReportProgress(progreso);
            }

            RPasos[i] = r.Adelgazar();
            RTxtDescripcion[i] = RObtenerDescripcion(RTipoDescripcion.Adelgazamiento, 0);
            RBackWorker.ReportProgress(progreso);
        }

        enum RTipoDescripcion
        {
            Original,
            Reescalado,
            EscalaGrises,
            FiltroMediana,
            FiltroMedia,
            FiltroFourierTransformada,
            FiltroFourierPasoBajo,
            FiltroFourierInversa,
            Binarizacion,
            FiltroMedianaNoRamas,
            Adelgazamiento
        }

        private string RObtenerDescripcion(RTipoDescripcion tipo, int param)
        {
            switch (tipo)
            {
                case RTipoDescripcion.Original:
                    return "Imagen original. Imagen de una huella dactilar tomada con " +
                           "un scanner o cámara.";

                case RTipoDescripcion.Reescalado:
                    return "Reescalado bicúbico de la imagen anterior para que su tamaño sea de " +
                           "512x512. Según nuestro criterio, las imágenes de las huellas que se " +
                           "traten deben tener este tamaño.";

                case RTipoDescripcion.EscalaGrises:
                    return "Resultado de convertir a escala de grises la imagen anterior mediante " +
                           "la fórmula Y = 0.299*R + 0.587*G + 0.114*B.";

                case RTipoDescripcion.FiltroMediana:
                    return "Resultado tras aplicar el filtro de la mediana a la imagen del paso " +
                           "anterior.";

                case RTipoDescripcion.FiltroMedia:
                    return "Resultado tras aplicar el filtro de la media con una máscara de tamaño" +
                           param + "x" + param + " a la imagen del paso anterior.";

                case RTipoDescripcion.FiltroFourierTransformada:
                    return "Espectro de Fourier de la imagen del paso anterior.";

                case RTipoDescripcion.FiltroFourierPasoBajo:

                    string cadena = "";

                    switch (param)
                    {
                        case (256 / 6):
                            cadena = "1/6";
                            break;
                        case (256 / 5):
                            cadena = "1/5";
                            break;
                        case (256 / 3):
                            cadena = "1/3";
                            break;
                    }

                    return "Resultado de aplicar el filtro de paso bajo considerando un radio de " +
                           cadena + " veces el \" radio \" del espectro anterior.";

                case RTipoDescripcion.FiltroFourierInversa:
                    return "Resultado de aplicar la transformada inversa de Fourier al espectro anterior.";

                case RTipoDescripcion.Binarizacion:
                    return "Resultado de binarizar la imagen anterior buscando iterativamente el umbral " +
                           "adecuado, en este caso " + param + ", usando el algoritmo ISODATA.";

                case RTipoDescripcion.FiltroMedianaNoRamas:
                    return "Con el fin de eliminar puntos finales no deseados que provocan una " +
                           "esqueletización con ramas en las líneas, se le aplica el filtro de " +
                           "la mediana a la imagen anterior y posteriormente se binariza de nuevo.";

                case RTipoDescripcion.Adelgazamiento:
                    return "Resultado de adelgazar la imagen anterior con el método de adelgazamiento con " +
                           "HitOrMiss. 15 iteraciones son suficientes para adelgazar una huella válida. " +
                           "Esta huella esta preparada para ser procesada.";

                default:
                    return "ERROR";
            }
        }

        private void RBackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RBarraPasos.Maximum = RPasos.Length - 1;
            RGuardar.Enabled = true;
            RParametros.Enabled = true;
            RProgreso.Visible = false;
            REjecutar.Visible = true;
            REjecutar.Enabled = true;

            if (detalles)
                RBarraPasos.Value = 0;
            else
                RBarraPasos.Value = RBarraPasos.Maximum;

            MostrarEnDyC(RPasos[RPasos.Length - 1]);
        }

        private void RBackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RProgreso.Increment(e.ProgressPercentage);
        }

        private void RDesactivarEjecucion()
        {
            RGuardar.Enabled = false;
            RParametros.Enabled = false;
            REjecutar.Enabled = false;
            RBarraPasos.Maximum = 0;
        }

        private void RActivarEjecucion()
        {
            RBarraPasos.Maximum = 0;
            RDescripcion.Text = RObtenerDescripcion(RTipoDescripcion.Original, 0);
            RParametros.Enabled = true;
            REjecutar.Enabled = true;
            REjecutar.Visible = true;
            ListaFiltros.SelectedIndex = ListaFiltros.Items.Count - 1;
        }

        private void RComenzarEjecucion()
        {
            //RCuadroImagen.Image = RImagen;    
            Bitmap bmp = new Bitmap(RAbrirImagen.FileName);
            RCuadroImagen.Image = bmp;
            RCuadroImagen.Size = bmp.Size;

            RDescripcion.Text = RObtenerDescripcion(RTipoDescripcion.Original, 0);

            RDesactivarEjecucion();
            REjecutar.Visible = false;
            RProgreso.Value = 0;
            RProgreso.Visible = true;
        }

        #endregion Ejecucion del realce

        #region Control de los parametros de Descriptores

        private void DEstablecerAyudasParametros()
        {
            ayudaCursor.SetToolTip(DELinea, "Longitud de la línea que se prolonga para juzgar " +
                                            "si una minucia es fiable");
            ayudaCursor.SetToolTip(DERangoBusqueda, "Especifica el anillo en el cual se busca el "+
                                                    "gradiente perpendicular a una minucia");
            ayudaCursor.SetToolTip(DEVecinos, "Radio en el que se busca a las minucias vecinas " +
                                              "a una minucia dada");
            ayudaCursor.SetToolTip(DERadios, "Radios de los círculos donde se mostrarán " +
                                             "los puntos");
            ayudaCursor.SetToolTip(DEPuntos, "Numero de puntos que se mostrarán en los circulos " +
                                             "cuyos radios se especifican en el parámetro anterior");
            ayudaCursor.SetToolTip(DETamEntorno, "Tamaño del entorno en el que se busca los puntos");
            ayudaCursor.SetToolTip(DEFactor, "Factor de ponderación del comparador del descriptor " +
                                             "de textura");
            ayudaCursor.SetToolTip(DEPorcent, "Tolerancia al considerar un descriptor de textura " +
                                              "válido");
            ayudaCursor.SetToolTip(DEEjemplos, "Numero de ejemplos que sera mostrados");
        }

        private void DEstablecerParametrosDefecto()
        {
            DManipulandoParametros = true;
            DLinea.Value = 20;
            DRangoMenor.Value = 6;
            DRangoMayor.Value = 30;
            DVecinos.Value = 60;
            DR0.Value = 27; DR1.Value = 45; DR2.Value = 63; DR3.Value = 81;
            DP0.Value = 10; DP1.Value = 16; DP2.Value = 22; DP3.Value = 28;
            DTam.Value = 5;
            DFactor.Value = 5;
            DPorcentaje.Value = 25;
            DEjemplos.Value = 4;
            DManipulandoParametros = false;
        }

        private void DEstablecerParametrosEjecucion()
        {
            //Relativos a decidir la fiabilidad de una minucia
            atributos.longitudLinea = (int)DLinea.Value;
            atributos.minPasosAntesDeBuscarPunto = (int)DRangoMenor.Value;
            atributos.maxLongitudBuqueda = (int)DRangoMayor.Value;

            //Relativos al descriptor de minucia
            atributos.radioVecinos = (int)DVecinos.Value;

            //Relativos al descriptor de textura
            atributos.radiosL = new int[] { (int) DR0.Value, (int) DR1.Value, 
                                            (int) DR2.Value, (int) DR3.Value };
            atributos.puntosK = new int[] { (int) DP0.Value, (int) DP1.Value, 
                                            (int) DP2.Value, (int) DP3.Value };
            atributos.tamEntornoPunto = (int)DTam.Value;
            atributos.w = (double)DFactor.Value * 0.1;
            atributos.minPorcentajeValidos = (int)DPorcentaje.Value;

            //Relativo a los dos descriptores
            atributos.numEjemplos = (int)DEjemplos.Value;
        }

        private void DDefecto_Click(object sender, EventArgs e)
        {
            DEstablecerParametrosDefecto();
        }

        private void DRangoMenor_ValueChanged(object sender, EventArgs e)
        {
            DRespetarIntervalo();
        }

        private void DRangoMayor_ValueChanged(object sender, EventArgs e)
        {
            DRespetarIntervalo();
        }

        private void DRespetarIntervalo()
        {
            if (!DManipulandoParametros)
            {

                if (DRangoMayor.Value == 0)
                {
                    DRangoMayor.Value = 1;
                }
                else if (DRangoMenor.Value == DRangoMayor.Value)
                {
                    if (DRangoMenor.Value == 0)
                        DRangoMayor.Value++;
                    else
                        DRangoMenor.Value--;
                }
                else if (DRangoMenor.Value > DRangoMayor.Value)
                {
                    if (DRangoMenor.Value == 1)
                        DRangoMayor.Value = 2;
                    else
                        DRangoMenor.Value = DRangoMayor.Value - 1;
                }
            }
        }

        #endregion Control de los parametros de Descriptores

        #region Ejecucion de Descriptores

        private void DEjecutar_Click(object sender, EventArgs e)
        {
            //FALLA?
            DCuadroImagen.Image = DImagen;

            DComenzarEjecucion();

            //Hay que leer los controles, por eso se hace fuera
            DEstablecerParametrosEjecucion();

            DBackWorker.RunWorkerAsync();           
        }

        private void DBackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Tratamiento trat = new Tratamiento(new Bitmap(DImagen));
            DPasos = trat.getPasos();
            DTxtDescripcion = Tratamiento.textoPasos;

        }

        private void DBackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DTerminarEjecucion();

            if (detalles)
                DBarraPasos.Value = 0;
            else
                DBarraPasos.Value = DBarraPasos.Maximum;
        }

        private void DBackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DProgreso.Increment(e.ProgressPercentage);
        }

        private void DActivarEjecucion()
        {
            DEjecutar.Enabled = true;
            DParametros.Enabled = true;
        }

        private void DDesactivarEjecucion()
        {
            DEjecutar.Enabled = false;
            DParametros.Enabled = false;
            DBarraPasos.Maximum = 0;
        }
        private void DComenzarEjecucion()
        {
            DDesactivarEjecucion();
            DEjecutar.Visible = false;
            DProgreso.Value = 0;
            DProgreso.Visible = true;
        }

        private void DTerminarEjecucion()
        {
            DActivarEjecucion();
            DBarraPasos.Maximum = DPasos.Length - 1;
            DBarraPasos.Value = 0;
            DEjecutar.Visible = true;
            DProgreso.Visible = false;
        }

        #endregion Ejecucion de Descriptores

        #region Control de los parametros de Comparacion

        private void CEstablecerAyudasParametros()
        {
            ayudaCursor.SetToolTip(CELongitud, "Longitud de la línea que se prolonga para juzgar " +
                                               "si una minucia es fiable");
            ayudaCursor.SetToolTip(CERangoBusqueda, "Especifica el anillo en el cual se busca el " +
                                                    "gradiente perpendicular a una minucia");
            ayudaCursor.SetToolTip(CEVecinos, "Radio en el que se busca a las minucias vecinas " +
                                              "a una minucia dada");
            ayudaCursor.SetToolTip(CERadios, "Radios de los círculos donde se mostrarán " +
                                             "los puntos");
            ayudaCursor.SetToolTip(CEPuntos, "Numero de puntos que se mostrarán en los circulos " +
                                             "cuyos radios se especifican en el parámetro anterior");
            ayudaCursor.SetToolTip(CETamEntorno, "Tamaño del entorno en el que se busca los puntos");
            ayudaCursor.SetToolTip(CEFactor, "Factor de ponderación del comparador del descriptor " +
                                             "de textura");
            ayudaCursor.SetToolTip(CEPorcent, "Tolerancia al considerar un descriptor de textura " +
                                              "válido");
            ayudaCursor.SetToolTip(CEEjemplos, "Numero de ejemplos que sera mostrados");
            ayudaCursor.SetToolTip(CDistMax, "Porcentaje del radio efectivo en el que considerar " +
                                             "la importancia de una minucia vecina");
            ayudaCursor.SetToolTip(CEAngulo, "Parámetro discriminatorio para minucias cuyo ángulo" +
                                             "parcial es mayor que el dado");
            ayudaCursor.SetToolTip(CEEncaje, "Entorno en el que debe de aparecer una minucia");
        }

        private void CEstablecerParametrosDefecto()
        {
            CManipulandoParametros = true;
            CLongitud.Value = 20;
            CRangoMenor.Value = 6;
            CRangoMayor.Value = 30;
            CVecinos.Value = 60;
            CR0.Value = 27; CR1.Value = 45; CR2.Value = 63; CR3.Value = 81;
            CP0.Value = 10; CP1.Value = 16; CP2.Value = 22; CP3.Value = 28;
            CTamEntorno.Value = 5;
            CFactor.Value = 5;
            CPorcent.Value = 25;
            CEjemplos.Value = 4;
            CDistMax.Value = 80;
            CAngulo.Value = 10;
            CEncaje.Value = 5;
            CManipulandoParametros = false;
        }

        private void CEstablecerParametrosEjecucion()
        {
            //Relativos a decidir la fiabilidad de una minucia
            atributos.longitudLinea = (int)CLongitud.Value;
            atributos.minPasosAntesDeBuscarPunto = (int)CRangoMenor.Value;
            atributos.maxLongitudBuqueda = (int)CRangoMayor.Value;

            //Relativos al descriptor de minucia
            atributos.radioVecinos = (int)CVecinos.Value;

            //Relativos al descriptor de textura
            atributos.radiosL = new int[] { (int) CR0.Value, (int) CR1.Value, 
                                            (int) CR2.Value, (int) CR3.Value };
            atributos.puntosK = new int[] { (int) CP0.Value, (int) CP1.Value, 
                                            (int) CP2.Value, (int) CP3.Value };
            atributos.tamEntornoPunto = (int)CTamEntorno.Value;
            atributos.w = (double)CFactor.Value * 0.1;
            atributos.minPorcentajeValidos = (int)CPorcent.Value;

            //Relativo a los dos descriptores
            atributos.numEjemplos = (int)CEjemplos.Value;

            //Relativo Greedy Match
            atributos.maxDistancia = (double)CDistMax.Value;
            atributos.umbralAngulo = (double)(CAngulo.Value * (Math.PI / 2) / 10);
            atributos.radioEncaje = (int)CEncaje.Value;
        }

        private void CDefecto_Click(object sender, EventArgs e)
        {
            CEstablecerParametrosDefecto();
        }

        private void CRangoMenor_ValueChanged(object sender, EventArgs e)
        {
            CRespetarIntervalo();
        }

        private void CRangoMayor_ValueChanged(object sender, EventArgs e)
        {
            CRespetarIntervalo();
        }

        private void CRespetarIntervalo()
        {
            if (!CManipulandoParametros)
            {

                if (CRangoMayor.Value == 0)
                {
                    CRangoMayor.Value = 1;
                }
                else if (CRangoMenor.Value == CRangoMayor.Value)
                {
                    if (CRangoMenor.Value == 0)
                        CRangoMayor.Value++;
                    else
                        CRangoMenor.Value--;
                }
                else if (CRangoMenor.Value > CRangoMayor.Value)
                {
                    if (CRangoMenor.Value == 1)
                        CRangoMayor.Value = 2;
                    else
                        CRangoMenor.Value = CRangoMayor.Value - 1;
                }
            }
        }

        #endregion Control de los parametros de Comparacion

        #region Ejecucion de la comparacion

        private void CEjecutar_Click(object sender, EventArgs e)
        {

            CComenzarEjecucion();

            CEstablecerParametrosEjecucion();

            CBackWorker.RunWorkerAsync();
        }

        private void CBackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Emparejador emp = new Emparejador(CImagen0, CImagen1);
            CPasos0 = emp.huellas1Final;
            CPasos1 = emp.huellas2Final;
            CTxtDescripcion = Tratamiento.textoPasos;
        }

        private void CBackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CTerminarEjecucion();

            if (detalles)
                CBarraPasos.Value = 0;
            else
                CBarraPasos.Value = CBarraPasos.Maximum;

            AbrirLog.Enabled = true;
        }

        private void CBackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CProgreso.Increment(e.ProgressPercentage);
        }

        private void CComenzarEjecucion()
        {
            CDesactivarEjecucion();
            CEjecutar.Visible = false;
            CProgreso.Value = 0;
            CProgreso.Visible = true;
        }

        private void CTerminarEjecucion()
        {
            CActivarEjecucion();
            CDescripcion.Text = CTxtDescripcion[0];
            CBarraPasos.Maximum = CPasos0.Length - 1;
            CBarraPasos.Value = 0;
            CEjecutar.Visible = true;
            CProgreso.Visible = false;
        }

        private void CActivarEjecucion()
        {
            CEjecutar.Enabled = true;
            CParametros.Enabled = true;
        }

        private void CDesactivarEjecucion()
        {
            CEjecutar.Enabled = false;
            CParametros.Enabled = false;
            CBarraPasos.Maximum = 0;
        }

        private void AbrirLog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("wordpad", Emparejador.ruta);
        }

        #endregion Ejecucion de la comparacion

        #region Manipulacion de Archivos

        private void RNuevo_Click(object sender, EventArgs e)
        {
            RAbrirImagen.InitialDirectory = DirectorioDefecto;

            RAbrirImagen.ShowDialog();

            try
            {
                RImagen = new Bitmap(RAbrirImagen.FileName);
                RCuadroImagen.Size = RImagen.Size;
                RCuadroImagen.Image = RImagen;

                RActivarEjecucion();
            }
            catch { }
        }

        private void RGuardar_Click(object sender, EventArgs e)
        {
            RGuardarHuella.InitialDirectory =
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.MyPictures);

            RGuardarHuella.ShowDialog();

            try
            {
                string nombreArchivo = RGuardarHuella.FileName;
                BinaryFormatter BF = new BinaryFormatter();
                Stream Ruta = File.OpenWrite(nombreArchivo);
                BF.Serialize(Ruta, RPasos[RPasos.Length - 1]);
                Ruta.Close();
            }
            catch { }
        }

        private void RAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap ImagenCargada = AbrirHuellaDesdeArchivo(RAbrirHuella);
                MostrarEnTodas(ImagenCargada);
                RDesactivarEjecucion();
            }
            catch { }
        }

        private void DAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap ImagenCargada = AbrirHuellaDesdeArchivo(DAbrirHuella);
                MostrarEnDyC(ImagenCargada);

            }
            catch { }
        }

        private void CAbrir0_Click(object sender, EventArgs e)
        {
            try
            {
                barraACero();
                CImagen0 = AbrirHuellaDesdeArchivo(CAbrirHuella0);
                cargadaCImagen0 = true;
                MostrarEnC(CImagen0,0);
            }
            catch { }
        }

        private void CAbrir1_Click(object sender, EventArgs e)
        {
            try
            {
                barraACero();
                CImagen1 = AbrirHuellaDesdeArchivo(CAbrirHuella1);
                cargadaCImagen1 = true;
                MostrarEnC(CImagen1,1);
            }
            catch { }
        }

        private Bitmap AbrirHuellaDesdeArchivo(OpenFileDialog abrir)
        {
            abrir.InitialDirectory = DirectorioDefecto;
            abrir.ShowDialog();

            string nombreArchivo = abrir.FileName;
            BinaryFormatter BF = new BinaryFormatter();
            Stream Ruta = File.OpenRead(nombreArchivo);
            Bitmap ImagenCargada = (Bitmap)BF.Deserialize(Ruta);
            Ruta.Close();

            return ImagenCargada;
        }

        #endregion Manipulacion de Archivos

        #region Mostrar huellas cargadas

        private void MostrarEnTodas(Bitmap imagen)
        {
            RImagen = imagen;
            RCuadroImagen.Image = RImagen;
            RCuadroImagen.Size = RImagen.Size;

            RDescripcion.Text = "Huella Dactilar preparada para ser procesada.";
            MostrarEnDyC(imagen);
        }

        private void MostrarEnDyC(Bitmap imagen)
        {
            DImagen = new Bitmap(imagen);
            DCuadroImagen.Image = new Bitmap(imagen);
            DDescripcion.Text = "Huella dactilar preparada para ser procesada.";
            DActivarEjecucion();

            //COMPROBAR
            DBarraPasos.Maximum = 0;

            MostrarEnC(imagen, indiceImagenC);

            if (indiceImagenC == 0)
                indiceImagenC = 1;
            else
                indiceImagenC = 0;
        }

        private void MostrarEnC(Bitmap imagen, int indice)
        {
            if (indice == 0)
            {
                CImagen0 = new Bitmap(imagen);
                CCuadroImagen0.Image = new Bitmap(imagen);
                cargadaCImagen0 = true;
            }
            else
            {
                CImagen1 = new Bitmap(imagen);
                CCuadroImagen1.Image = new Bitmap(imagen);
                cargadaCImagen1 = true;
            }

            if (cargadaCImagen0 && cargadaCImagen1)
            {
                CActivarEjecucion();
                CDescripcion.Text = "Huellas dactilares preparadas para ser procesadas.";
            }
            else
            {
                CDescripcion.Text = "Huella dactilar preparada para ser procesada.";
            }

            CBarraPasos.Maximum = 0;
        }

        #endregion Mostrar huellas cargadas

        #region Acerca De
        private void AcercaDe_Click(object sender, EventArgs e)
        {
            (new VentanaAcercaDe()).ShowDialog();
        }
        #endregion Acerca de
    }

    public class EnvoltorioFiltro
    {
        public enum Tipo
        {
            Mediana,
            Media,
            Fourier
        };

        private Tipo tipo;

        private int parametro;

        public int Parametro
        {
            get { return parametro; }
        }

        public Tipo TipoFiltro
        {
            get { return tipo; }
        }

        public EnvoltorioFiltro(Tipo t, int param)
        {
            tipo = t;
            parametro = param;
        }

        public override string ToString()
        {
            switch (tipo)
            {
                case Tipo.Mediana: return "Mediana";
                case Tipo.Media: return "Media " + parametro + "x" + parametro;
                case Tipo.Fourier:
                    string param = "";
                    switch (parametro)
                    {
                        case (256 / 6):
                            param = "1/6";
                            break;
                        case (256 / 5):
                            param = "1/5";
                            break;
                        case (256 / 3):
                            param = "1/3";
                            break;
                    }
                    return "Fourier <" + param;
                default: return "ERROR";
            }
        }
    }
}