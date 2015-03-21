using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class Hammurabi:LeaderCard
    {
        public Hammurabi(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {
         
            FeedBackText.Instance.Text = leaderName + " Played";
            player.AP_Civil++;
            player.AP_Military--;
                   
        }

        public override void PlayEveryTurn(Player player)
        {
            player.AP_Civil++;
            player.AP_Military--;
        }

        public override void Remove(Player player)
        {
            player.AP_Civil--;
            player.AP_Military++;
   
            base.Remove(player);
        }
    }
}
