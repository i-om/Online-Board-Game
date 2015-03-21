using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class Board
    {
      
        const int maxCardNum = 13;

        public event EventHandler RefreshCardBoard;
        
        private int[] actionCostArray = { 1,1,1,1,1,2,2,2,2,3,3,3,3 }; 
      
        public int Num_CardsToBeRemoved { get; set; }
          
        private List<Card> cardList = new List<Card>();

        public bool IsLastRound { get; set; }

        public int UnreachableCards { get; set; }
    

        public List<Card> CardList {
            get { return cardList; }
            set { cardList = value; }
        }


        public int MaxCardNum {
            get { return maxCardNum; }
        }
        
       
        public int[] ActionCostArray {
            get { return actionCostArray; }
        }

        public Board(int value) {
            Num_CardsToBeRemoved = 5 - value;
            UnreachableCards = 0;
        }

        

   
        public void InitializeCards() {
            RefreshCardBoard(this, new EventArgs());
        }

        public void ReplaceCards(int removedCards) {

            int actualNum = Num_CardsToBeRemoved - removedCards;

                for (int i = actualNum; i > 0; i--)
                {
                    if (CardList.Count > 0)
                    {
                        CardList.Remove(CardList.First());
                    }
                }
              
                RefreshCardBoard(this, new EventArgs());
                                                           
        }

        public void UpdateUnreachableCards(int myIndex,int indexOfcurrentP,int numOfPlayers) {

            UnreachableCards = 0;

            if (indexOfcurrentP > myIndex) {
                UnreachableCards = (numOfPlayers - (indexOfcurrentP - myIndex)) * Num_CardsToBeRemoved;
            }
            else if (indexOfcurrentP < myIndex) {
                UnreachableCards = (myIndex - indexOfcurrentP) * Num_CardsToBeRemoved;
            }        
        }

        public int ActualCardsOnDisplay() {
            return CardList.Count > MaxCardNum ? MaxCardNum : CardList.Count;
        }
    
      
    }
}
