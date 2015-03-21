using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class DestroyUnitPack:ReversePack
    {
        public UnitCard card;
        public DestroyUnitPack(UnitCard _card) : base(GameAction.DestroyUnit) {
            card = _card;
        }
    }
}
