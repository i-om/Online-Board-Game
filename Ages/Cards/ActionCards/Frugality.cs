using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages
{
    class Frugality:ActionCard
    {
       
        private string myName;
        private int myValue;

        public Frugality(int ageValue, BitmapSource bitmapImage, string _name, int _value)
            : base(_name,ageValue, bitmapImage)
        {
            myName = _name;
            myValue = _value;
        }

   

        public override void Act(Player player) {

            player.SpecialAP_List.Add(new SpecialAP("IncreasePopulation"));
            player.TurnEffect.Add(myName);
            FeedBackText.Instance.Text = myName + " Played";
           
        }

    }
}
