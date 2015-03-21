using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages
{
    class RichLand:ActionCard
    {
       
        private string myName;
        private int myValue;
    
        public RichLand(int ageValue,BitmapSource bitmapImage,string _name,int _value):base(_name,ageValue,bitmapImage) {
            myName = _name;
            myValue = _value;
        }

   

        public override void Act(Player player) {

            player.SpecialAP_List.Add(new SpecialAP("BuildUnit", CardType.Civil, CardClass.Mine, CardClass.Farm));

            for(int i =0;i<myValue;i++)
            player.SpecialR_List.Add(new SpecialResource(CardClass.Farm,"BuildUnit",CardClass.Mine));

            FeedBackText.Instance.Text = myName + " Played";
           
        }

    }
}
