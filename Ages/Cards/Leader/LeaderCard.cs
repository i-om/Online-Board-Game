using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace Ages
{
    class LeaderCard:Card
    {
        protected string leaderName;
        public string LeaderName {
            get { return leaderName; }
        }

        public LeaderCard(int ageValue, BitmapSource bitmapImage, string name="unimplemented")
            : base(ageValue, bitmapImage,name)
        {
            MyClass = CardClass.Leader;
            leaderName = name;
        }

        public event GoToWasteDelegate goToWaste;

        public virtual void PlayOnce(Player player) {
            
        }

        public virtual void PlayEveryTurn(Player player) {
        
        }

        public virtual void Remove(Player player) {
            goToWaste(this,player.PlayerIndex);
        }
    }
}
