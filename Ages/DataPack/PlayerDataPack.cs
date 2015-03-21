using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class PlayerDataPack
    {
        int science,sp,culture,cp,happyface,strength,food,ore;
        int ap_c, ap_m;

        public PlayerDataPack() {
            science = 0;
            sp = 0;
            culture = 0;
            cp = 0;
            happyface = 0;
            strength = 0;
            ap_c = 0;
            ap_m = 0;
            food = 0;
            ore = 0;
        }

        public void GetPlayerData(Player player){
            science = player.Science;
            sp = player.SciencePoint;
            culture = player.Culture;
            cp = player.CulturePoint;
            happyface = player.Happiness;
            strength = player.Strength;
            ap_c = player.AP_Civil;
            ap_m = player.AP_Military;
            food = player.GetTotalFood();
            ore = player.GetTotalOre();
        }

        public void GiveBackPlayerData(Player player){

            player.Science = science;
            player.SciencePoint = sp;
            player.Culture = culture;
            player.CulturePoint = cp;
            player.Happiness = happyface;
            player.Strength = strength;
            player.AP_Civil = ap_c;
            player.AP_Military = ap_m;

            GetBackResource(player,DataType.Food, player.GetTotalFood() - food);
            GetBackResource(player,DataType.Ore, player.GetTotalOre() - ore);

            player.TurnEffect.Clear();
            player.SpecialR_List.Clear();
            player.SpecialAP_List.Clear();
        }

        private void GetBackResource(Player player,DataType type,int difference) {
            if (difference != 0) {
                if (difference > 0)
                {
                    player.Consume(difference, type);
                }
                else {
                    int temp = -difference;
                    player.Produce_Amount(temp, type);               
                }       
            }
        }
    }
}
