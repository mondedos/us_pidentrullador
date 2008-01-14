using System;
using System.Collections.Generic;
using System.Text;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing;

namespace kernel
{
    class Realce
    {
        private Bitmap imagen;
        private ComplexImage cimage;

        public int umbral;
        
        public Realce(Bitmap imagen)
        {
            this.imagen = imagen;
        }

        public Bitmap ReescaladoBicubico(int anchura, int altura)
        {
            ResizeBicubic resize = new ResizeBicubic(anchura, altura);
            imagen = resize.Apply(imagen);
            return imagen;
        }

        public Bitmap EscalaGrises()
        {
            GrayscaleY filter = new GrayscaleY();
            imagen = filter.Apply(imagen);
            return imagen;
        }

        public Bitmap FiltroMediana()
        {
            Mean mean = new Mean();
            imagen = mean.Apply(imagen);
            return imagen;
        }

        public Bitmap FiltroMedia(int n)
        {
            Median media = new Median(n);
            imagen = media.Apply(imagen);
            return imagen;
        }

        public Bitmap ImagenCompleja()
        {
            cimage = ComplexImage.FromBitmap(imagen);
            cimage.ForwardFourierTransform();
            imagen = cimage.ToBitmap();
            return imagen;
        }

        public Bitmap FiltroPasoBajo(int n)
        {
            cimage.FrequencyFilter(new IntRange(0,n));
            imagen = cimage.ToBitmap();
            return imagen;
        }

        public Bitmap ImagenNormal()
        {
            cimage.BackwardFourierTransform();
            imagen = cimage.ToBitmap();
            return imagen;
        }

        public Bitmap BinarizacionIterativa()
        {
            Atributos atr = Atributos.getInstance();

            IterativeThreshold it = new IterativeThreshold(2, (byte)atr.umbralBinarizacion);
            imagen = it.Apply(imagen);
            umbral = it.ThresholdValue;
            return imagen;
        }

        public Bitmap Adelgazar()
        {
            Invert ivert = new Invert();
            imagen = ivert.Apply(imagen);

            FiltersSequence filterSequence = new FiltersSequence();

            filterSequence.Add(new HitAndMiss(
                new short[,] { { 0, 0, 0 }, 
                               { -1, 1, -1 }, 
                               { 1, 1, 1 } },
                HitAndMiss.Modes.Thinning));
            filterSequence.Add(new HitAndMiss(
                new short[,] { { -1, 0, 0 }, 
                               { 1, 1, 0 }, 
                               { -1, 1, -1 } },
                HitAndMiss.Modes.Thinning));
            filterSequence.Add(new HitAndMiss(
                new short[,] { { 1, -1, 0 }, 
                               { 1, 1, 0 }, 
                               { 1, -1, 0 } },
                HitAndMiss.Modes.Thinning));
            filterSequence.Add(new HitAndMiss(
                new short[,] { { -1, 1, -1 }, 
                               { 1, 1, 0 }, 
                               { -1, 0, 0 } },
                HitAndMiss.Modes.Thinning));
            filterSequence.Add(new HitAndMiss(
                new short[,] { { 1, 1, 1 }, 
                               { -1, 1, -1 }, 
                               { 0, 0, 0 } },
                HitAndMiss.Modes.Thinning));
            filterSequence.Add(new HitAndMiss(
                new short[,] { { -1, 1, -1 }, 
                               { 0, 1, 1 }, 
                               { 0, 0, -1 } },
                HitAndMiss.Modes.Thinning));
            filterSequence.Add(new HitAndMiss(
                new short[,] { { 0, -1, 1 }, 
                               { 0, 1, 1 }, 
                               { 0, -1, 1 } },
                HitAndMiss.Modes.Thinning));
            filterSequence.Add(new HitAndMiss(
                new short[,] { { 0, 0, -1 }, 
                               { 0, 1, 1 }, 
                               { -1, 1, -1 } },
                HitAndMiss.Modes.Thinning));

            FilterIterator filterIterator = new FilterIterator(filterSequence, 15);

            imagen = filterIterator.Apply(imagen);

            imagen = ivert.Apply(imagen);

            return imagen;
        }
    }
}
