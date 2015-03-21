using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class PickCardPack:ReversePack
    {
        public string name;
        public Card pickedCard;
        public double x, y;
        public int cardPos,card;
        public PickCardPack(string _name,Card _pickedCard,double _x,double _y,int _cardPos,int _card) : base(GameAction.PickUpCard) {
            name = _name;
            pickedCard = _pickedCard;
            card = _card;
            cardPos = _cardPos;
            x = _x;
            y = _y;
        }
    }
}
