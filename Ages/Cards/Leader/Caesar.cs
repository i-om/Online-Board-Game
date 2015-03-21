using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class Caesar:LeaderCard
    {
        public Caesar(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {
  
            FeedBackText.Instance.Text = leaderName + " Played";
            player.Strength++;
            player.AP_Military++;
                   
        }

        public override void PlayEveryTurn(Player player)
        {          
            player.AP_Military++;
        }

        public override void Remove(Player player)
        {
            player.Strength--;
            player.AP_Military--;
   
            base.Remove(player);
        }
    }
}
