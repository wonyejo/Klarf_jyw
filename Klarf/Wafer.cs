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


        public string FileTimestamp;
        public string LotID;
        public string WaferID;
       

        public List<Die> Dies { get; set; }

        public Wafer()
        {

        }
        public Wafer(string fileTimestamp, string lotID, string waferID)
        {
            FileTimestamp = fileTimestamp;
            LotID = lotID;
            WaferID = waferID;
           
        }

        public void setDies(List<Die> dies)
        {
            Dies = dies;
        }
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
