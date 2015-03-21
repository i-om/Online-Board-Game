using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages.Leader
{
    class Homer:LeaderCard
    {
       
        public Homer(int ageValue, BitmapSource bitmapImage, string name)
            : base(ageValue, bitmapImage,name)
        { }

        public override void PlayOnce(Player player)
        {
            FeedBackText.Instance.Text = leaderName + " Played";
  
            int temp = player.MiliTechCardList[0].Unit > 2 ? 2 : player.MiliTechCardList[0].Unit;
            player.CulturePoint += temp;

            player.SpecialR_List.Add(new SpecialResource(CardClass.MilitaryTech, "BuildUnit"));   
        }

        public override void PlayEveryTurn(Player player)
        {
            int temp = player.MiliTechCardList[0].Unit > 2 ? 2 : player.MiliTechCardList[0].Unit;
            player.CulturePoint += temp;    
        
            player.SpecialR_List.Add(new SpecialResource(CardClass.MilitaryTech,"BuildUnit"));     
        }

        public override void Remove(Player player)
        {
            
            base.Remove(player);
        }
    }
}
