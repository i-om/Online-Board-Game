using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;


namespace Ages
{
    class GovCard:Card
    { 
        public int CivilAP { get; set; }
        public int MilitaryAP { get; set; }
       
        public int Capacity { get; set; }

        public GovCard(string name,int ageValue,BitmapSource bitmapImage,int cAPvalue ,int mAPvalue ,int capacityValue)
            : base(ageValue, bitmapImage,name)
        {
            MyClass = CardClass.Govt;
            CivilAP = cAPvalue;
            MilitaryAP = mAPvalue;
            Capacity = capacityValue;    
        }
    }
}
