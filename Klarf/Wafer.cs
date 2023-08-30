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
       



    }
}
