using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class Mich : LeaderCard
    {
       
  
        public Mich(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {      
          
            player.TurnEffect.Add(leaderName);
            FeedBackText.Instance.Text = leaderName + " Played";
            foreach (UrbanBuildingCard card in player.UrbanBuildingCardList) {
                if (card.Utype == UrbanBuildingType.Temple || card.Utype == UrbanBuildingType.Theater) {
                    player.CulturePoint += card.Happiness;
                }   
            }
        }

        public override void PlayEveryTurn(Player player)
        {
            
            player.TurnEffect.Add(leaderName);
            foreach (UrbanBuildingCard card in player.UrbanBuildingCardList)
            {
                if (card.Utype == UrbanBuildingType.Temple || card.Utype == UrbanBuildingType.Theater)
                {
                    player.CulturePoint += card.Happiness;
                }
            }
        }

        public override void Remove(Player player)
        {
            player.TurnEffect.Remove(leaderName);
            base.Remove(player);
        }
    }
}
