using System;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
namespace Klarf.Model
{
    public class Wafer
    {


        public string FileTimestamp;
        public string LotID;
        public string WaferID;
       
        // Wafer 클래스에 추가된 필드
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }


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
        public void CalculateGridDimensions()
        {
            if (Dies == null || Dies.Count == 0)
            {
                GridWidth = 0;
                GridHeight = 0;
                return;
            }

            double minDieX = Dies.Min(die => die.X);
            double maxDieX = Dies.Max(die => die.X);
            double minDieY = Dies.Min(die => die.Y);
            double maxDieY = Dies.Max(die => die.Y);

            double dieWidth = maxDieX - minDieX;
            double dieHeight = maxDieY - minDieY;

            GridWidth = (int)Math.Ceiling(dieWidth / 25);
            GridHeight = (int)Math.Ceiling(dieHeight / 10);
        }





    }
}
