using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class IncreasePopuPack:ReversePack
    {
        public int col, row;
   

        public IncreasePopuPack(int wCol, int wRow):base(GameAction.IncreasePopu) {

            col = wCol;
            row = wRow;
        }
    }
}
