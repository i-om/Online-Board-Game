using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class SpecialResource
    {
        private CardClass classA, classB, classC;
        private string actionA, actionB;

        public SpecialResource(CardClass _classA, string _actionA , CardClass _classB = CardClass.Other, string _actionB = "other", CardClass _classC = CardClass.Other)
        {
            classA = _classA;
            classB = _classB;
            classC = _classC;

            actionA = _actionA;
            actionB = _actionB;
        }

        public bool isMatched(CardClass _class, string _action)
        {

            return (_class == classA || _class == classB || _class == classC) && (_action == actionA || _action == actionB);

        }
    }
}
