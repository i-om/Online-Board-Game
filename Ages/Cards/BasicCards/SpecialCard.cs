using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages
{
    class SpecialCard:Card
    {
        public int ScienceCost { get; set; }
        public SpecialCard(string name,int ageValue,BitmapSource bitmapImage,int scienceCost):base(ageValue,bitmapImage,name) {

            ScienceCost = scienceCost;
            MyClass = CardClass.Special;
        }

    
    
    }
}
