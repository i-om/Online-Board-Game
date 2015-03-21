using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class SpecialAP
    {
        private CardType cardType = CardType.Civil;
        private CardClass classA = CardClass.None,classB = CardClass.None,classC = CardClass.None,classD = CardClass.None,classE = CardClass.None;
        private string actionA;

        //For BuildUnit
        public SpecialAP(string _action,CardType _type,CardClass _class,CardClass _classB = CardClass.Other) {
            
            actionA = _action;
            cardType = _type;
            classA = _class;
            classB = _classB;
        }

        //For increasepopulation
        public SpecialAP(string _action) {

            if (_action == "IncreasePopulation")
            {
                actionA = _action;
            }

            if (_action == "Breakthrough") {

                actionA = "PlayCard";
                classA = CardClass.Farm;
                classB = CardClass.Mine;
                classC = CardClass.UrbanBuilding;
                classD = CardClass.MilitaryTech;
                classE = CardClass.Special;
            
            }
        }

    



        public bool isMatched(string _action,CardClass _class,CardType _type)
        {
            if (actionA == _action) {
                if (_action == "IncreasePopulation") return true;
                if (classA == _class || classB == _class || classC == _class || classD == _class || classE == _class) {
                    if (_action == "PlayCard") return true;
                    if (cardType == _type) {
                        return true;
                    }              
                }
            }

            return false;
        }

    }
}
