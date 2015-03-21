using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class Moses:LeaderCard
    {
        public Moses(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {            
            FeedBackText.Instance.Text = leaderName + " Played";
            player.LongEffect.Add(leaderName);
        }

        public override void PlayEveryTurn(Player player)
        {
            
        }

        public override void Remove(Player player)
        {
            player.LongEffect.Remove(leaderName);               
            base.Remove(player);
        }
    }
}
