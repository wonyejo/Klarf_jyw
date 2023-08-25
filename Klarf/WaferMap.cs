using System;
using System.Windows.Media.Imaging;

using System.Windows;
using System.Windows.Media;

namespace Klarf.Model
{
    class WaferMap
    {
       

        private BitmapSource VisualizeWaferMap(Die[] Dies, BitmapImage baseImage)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                foreach (Die die in Dies)
                {
                    Rect rect = new Rect(die.X, die.Y, 10, 10);
                    drawingContext.DrawRectangle(new SolidColorBrush(), null, rect);
                }
            }

            RenderTargetBitmap targetBitmap = new RenderTargetBitmap(
                (int)baseImage.Width, (int)baseImage.Height,
                baseImage.PixelWidth, baseImage.PixelHeight,
                PixelFormats.Pbgra32);

            targetBitmap.Render(drawingVisual);
            return targetBitmap;
        }
       


    }
}
