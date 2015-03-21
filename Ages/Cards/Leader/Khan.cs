using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class Khan:LeaderCard
    {
        private int unitNumber = 0;
        public Khan(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {
            FeedBackText.Instance.Text = leaderName + " Played";
            foreach (MilitaryTechCard card in player.MiliTechCardList)
            {
                if (card.MyName == "Knights")
                    unitNumber = card.Unit;
            }      
                player.Strength += unitNumber;
                player.CulturePoint += unitNumber;
            
        }

        public override void PlayEveryTurn(Player player)
        {
            int temp = 0;
            foreach (MilitaryTechCard card in player.MiliTechCardList)
            {
                if(card.MyName == "Knights")
                temp = card.Unit;
            }
            if (temp > unitNumber)
            {

                player.Strength += temp - unitNumber;
                unitNumber = temp;
                player.CulturePoint += unitNumber;
            }
        }

        public override void Remove(Player player)
        {
            player.Strength -= unitNumber;
            base.Remove(player);
        }
    }
}
