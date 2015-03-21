using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class DaVinci : LeaderCard
    {
        private UrbanBuildingCard bestLabOrLib;
        int temp = -1;
        public DaVinci(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {      
            FeedBackText.Instance.Text = leaderName + " Played";

            player.LongEffect.Add(leaderName);

            foreach (UrbanBuildingCard card in player.UrbanBuildingCardList) {
                if ( card.Age > temp && (card.Utype == UrbanBuildingType.Lab || card.Utype == UrbanBuildingType.Library)) {
                    bestLabOrLib = card;
                    temp = card.Age;
                }
            }
            player.SciencePoint += bestLabOrLib.Age;
        }

        public override void PlayEveryTurn(Player player)
        {
           
            foreach (UrbanBuildingCard card in player.UrbanBuildingCardList)
            {
                if ( card.Age > temp && (card.Utype == UrbanBuildingType.Lab || card.Utype == UrbanBuildingType.Library) )
                {
                    bestLabOrLib = card;
                    temp = card.Age;
                }
            }
            player.SciencePoint += bestLabOrLib.Age;
        
        }

        public override void Remove(Player player)
        {
            player.LongEffect.Remove(leaderName);
            base.Remove(player);
        }
    }
}
