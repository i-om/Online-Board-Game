using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages
{
    abstract class UnitCard:Card
    {
        public virtual int Unit { get; set; }
        public int Product { get; set; }
        public int Cost { get; set; }
        public int ScienceCost { get; set; }


         public UnitCard(string name,int ageValue,BitmapSource bitmapImage,int costValue,int scienceValue):base(ageValue,bitmapImage,name) {

             ScienceCost = scienceValue;
             Cost = costValue; 
        }

      
    }
}
