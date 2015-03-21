using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages
{
    class DirectSpecialCard:SpecialCard
    {
        private int Bonus { get; set; }
        private DataType BonusType { get; set; }


        public DirectSpecialCard(string name,int ageValue,BitmapSource bitmapImage,int scienceCost,DataType type, int bonusValue ):base(name,ageValue,bitmapImage,scienceCost) {
            BonusType = type;
            Bonus = bonusValue;
        }

        public void Act(Player player) {

            switch (BonusType) {
                case DataType.SciencePoint: player.SciencePoint += Bonus; break;
                case DataType.CulturePoint: player.CulturePoint += Bonus; break;
                case DataType.Strength: player.Strength += Bonus; break;
                case DataType.Food: player.Produce_Amount(Bonus, DataType.Food); break;
                case DataType.Ore: player.Produce_Amount(Bonus, DataType.Ore); break;
                case DataType.CivilPoint: player.AP_Civil++; player.AP_Bonus_Civil += Bonus; break;
                default: throw new NotImplementedException();
                
            }
        }
    }
}
