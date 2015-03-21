using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class BuildUnitPack:ReversePack
    {
        public UnitCard card;
        public BuildUnitPack(UnitCard _card):base(GameAction.BuildUnit){
            card = _card;
        }
    }
}
