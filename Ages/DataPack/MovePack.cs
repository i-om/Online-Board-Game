using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class MovePack
    {
        public Card card;
        public double x;
        public double y;

        public MovePack(Card _card,double _x,double _y) {
            card = _card;
            x = _x;
            y = _y;
        }
    }
}
