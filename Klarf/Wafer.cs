using System;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
namespace Klarf.Model
{
    public class Wafer
    {

    
        DateTime FileTimestamp;
        string LotID;
        string WaferID;
        string DeviceID;

        public List<Die> Dies { get; set; } = new List<Die>();

        //private BitmapSource VisualizeWaferMap(Die[] Dies, BitmapImage baseImage)
        //{
        //    DrawingVisual drawingVisual = new DrawingVisual();
        //    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        //    {
        //        foreach (Die die in Dies)
        //        {
        //            Rect rect = new Rect(die.X, die.Y, 10, 10);
        //            drawingContext.DrawRectangle(new SolidColorBrush(), null, rect);
        //        }
        //    }

        //    RenderTargetBitmap targetBitmap = new RenderTargetBitmap(
        //        (int)baseImage.Width, (int)baseImage.Height,
        //        baseImage.PixelWidth, baseImage.PixelHeight,
        //        PixelFormats.Pbgra32);

        //    targetBitmap.Render(drawingVisual);
        //    return targetBitmap;
        //}



    }
}
