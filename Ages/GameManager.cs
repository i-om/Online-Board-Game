using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class GameManager
    {
        public static int gameRound;
        public static GameState State = GameState.NetWork;
        public static bool IsPlayingNetwork() {
            return State == GameState.NetWork;
        }
       
   
    }



    enum GameState{
        Local,
        NetWork,  
    }
}
