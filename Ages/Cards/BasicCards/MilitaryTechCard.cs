using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace Ages
{
    class MilitaryTechCard:UnitCard,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public override int Unit
        {
            get
            {
                return base.Unit;
            }
            set
            {
                if (value < base.Unit)
                {
                    ProducePoint("DestroyMilitaryTech");
                }
                else { ProducePoint("AddMilitaryTech"); }

                base.Unit = value;              
            }
        }

        public int Strength { get; set; }
     

        public MilitaryTechCard(int ageValue, BitmapSource bitmapImage, int costValue, int scienceCost,int strengthValue,string _name)
            : base(_name,ageValue, bitmapImage, costValue, scienceCost)
        {
            Strength = strengthValue;
            MyClass = CardClass.MilitaryTech;
            MyType = CardType.Military;
                   
        }

        public void ProducePoint(string info) {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
