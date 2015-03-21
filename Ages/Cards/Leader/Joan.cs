using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class Joan : LeaderCard
    {
        private int templeNum = 0;
        private int happyFaceNum = 0;
        public Joan(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {      
            FeedBackText.Instance.Text = leaderName + " Played";

            foreach (UrbanBuildingCard card in player.UrbanBuildingCardList) {
                if (card.Utype == UrbanBuildingType.Temple) {
                    templeNum++; 
                    happyFaceNum += card.Happiness;
                }
            }

            player.Strength += happyFaceNum;
        }

        public override void PlayEveryTurn(Player player)
        {
            int tempA = 0;
            int tempB = 0;

            foreach (UrbanBuildingCard card in player.UrbanBuildingCardList)
            {
                if (card.Utype == UrbanBuildingType.Temple)
                {
                    tempA++;
                    tempB += card.Happiness;
                }
            }

            if (tempA > templeNum) {
                player.Strength += tempB - happyFaceNum;
                templeNum = tempA;
                happyFaceNum = tempB;
            }

        }

        public override void Remove(Player player)
        {
            player.Strength -= happyFaceNum;
            base.Remove(player);
        }
    }
}
