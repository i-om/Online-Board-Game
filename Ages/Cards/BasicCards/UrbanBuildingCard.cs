using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace Ages
{
    class UrbanBuildingCard:UnitCard,INotifyPropertyChanged
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
                    ProducePoint("DestroyUrbanBuilding");
                }
                else { ProducePoint("AddUrbanBuilding"); }
                base.Unit = value;              
            }
        }

        public int Culture { get; set; }
        public int Science { get; set; }
        public int Happiness { get; set; }
        public UrbanBuildingType Utype { get; set; }

        public UrbanBuildingCard(string name,int ageValue, BitmapSource bitmapImage, int costValue, int scienceCost, int cultureValue, int happinessValue, int scienceValue,UrbanBuildingType utype)
            : base(name,ageValue, bitmapImage, costValue, scienceCost)
        {

                MyClass = CardClass.UrbanBuilding;
                Culture = cultureValue;
                Science = scienceValue; 
                Happiness = happinessValue;
                Utype = utype;
        }

        public void ProducePoint(string info) {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
