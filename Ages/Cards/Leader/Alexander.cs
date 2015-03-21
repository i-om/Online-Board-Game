using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class Alexander : LeaderCard
    {
        private int unitNumber = 0;
        public Alexander(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {      
            FeedBackText.Instance.Text = leaderName + " Played";
            foreach (MilitaryTechCard card in player.MiliTechCardList) {
                unitNumber += card.Unit;           
            }
            player.Strength += unitNumber;
            player.LongEffect.Add(leaderName);
        }

        public override void PlayEveryTurn(Player player)
        {
         
  
        }

        public override void Remove(Player player)
        {
            foreach (MilitaryTechCard card in player.MiliTechCardList)
            {
                unitNumber += card.Unit;
            }
            player.Strength -= unitNumber;
            player.LongEffect.Remove(leaderName);
            base.Remove(player);
        }
    }
}
