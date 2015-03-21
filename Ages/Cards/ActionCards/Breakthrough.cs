using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages
{
    class Breakthrough : ActionCard
    {
       
        private string myName;
  
  
        public Breakthrough(int ageValue,BitmapSource bitmapImage,string _name):base(_name,ageValue,bitmapImage) {
            myName = _name;
      
        }

   
        public override void Act(Player player) {

            player.SpecialAP_List.Add(new SpecialAP(myName));
            player.TurnEffect.Add(myName);
            FeedBackText.Instance.Text = myName + " Played";
           
        }

    }
}
