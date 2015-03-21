using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages
{
    class DirectActionCard:ActionCard
    {
        private int Bonus { get; set; }
        private DataType BonusType { get; set; }


        public DirectActionCard(string name,int ageValue,BitmapSource bitmapImage,DataType type, int bonusValue ):base(name,ageValue,bitmapImage) {
            BonusType = type;
            Bonus = bonusValue;
        }

        public override void Act(Player player) {

            switch (BonusType) {
                case DataType.SciencePoint: player.SciencePoint += Bonus; break;
                case DataType.CulturePoint: player.CulturePoint += Bonus; break;
                case DataType.Food: player.Produce_Amount(Bonus, DataType.Food); break;
                case DataType.Ore: player.Produce_Amount(Bonus, DataType.Ore); break;
                default: throw new NotImplementedException();
                
            }
        }
       
    }
}
