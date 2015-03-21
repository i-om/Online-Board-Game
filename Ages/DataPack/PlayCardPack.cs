using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class PlayCardPack:ReversePack
    {
        public Card card;
        public CardClass cardClass;
        public LeaderCard removedLeaderCard;
        public PlayCardPack(CardClass _class,Card _card,LeaderCard _removedLeaderCard = null):base(GameAction.PlayCard) {
            cardClass = _class;
            card = _card;
            removedLeaderCard = _removedLeaderCard;
            
        }
    }
}
