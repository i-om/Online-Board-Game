using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ages
{
    abstract class ActionCard:Card
    {
         public ActionCard(string name,int ageValue,BitmapSource bitmapImage):base(ageValue,bitmapImage,name) {

             MyClass = CardClass.Action;
             this.IsPlayable = true;
        }

      
         public event GoToWasteDelegate goToWaste;

         public virtual void Act(Player player) { } 

       
         public void GoToWaste(Player player) { goToWaste(this,player.PlayerIndex); }
    }
}
