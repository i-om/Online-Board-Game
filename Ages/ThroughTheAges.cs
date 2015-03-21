using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;


namespace Ages
{
    class ThroughTheAges:INotifyPropertyChanged
    {
        private const int BASIC_ACTION_COST = 1;

        bool firstRound = true;

        private Board gameBoard;
       
        private List<Player> players = new List<Player>();
        public List<Card> CloneList = new List<Card>();

        private Player currentP { get { return Players[IndexOfCurrentP]; } }
        private Player me { get { return Players[myIndex]; } }

        public int NumberOfPlayers{ get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event SendMessgeToRegister SendMessage;
        public event AddReversePack AddPack;
        public bool IsLastTurn { get; set; }

        private int playerIndex = 0;

        public int IndexOfCurrentP {
            get { return playerIndex; }
            set { playerIndex = value; 
                    OnPropertyChanged("PlayerIndex");}
        }


        private int myIndex = 0;
        public int MyIndex {
            get { return myIndex; }
            set {myIndex = value;
                   OnPropertyChanged("MyIndex");}
        }


        protected void OnPropertyChanged(string name)
        {        
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Board GameBoard {
            get { return gameBoard; }
            set { gameBoard = value; }
        }
             
        public List<Player> Players {
            get { return players; }
            set {Players = value;}
        }

        public List<ReversePack> reversePackList = new List<ReversePack>();
        public PlayerDataPack playerDataPack = new PlayerDataPack();
   
        #region Initialization 

        public void InitializeGame(List<string> nameList, List<int> random)
        {
            InitializePlayersAndBasicCards(nameList);
            InitializeCardBoard(random);
        }



        private void InitializePlayersAndBasicCards(List<string> nameList)
        {

            for (int i = 0; i < NumberOfPlayers; i++)
            {
           
               if(MainWindow.GameMode > 0)
                Players.Add(new Player(nameList[i],i,MyIndex));
                else
                Players.Add(new Player("Player "+i.ToString(),i,MyIndex));
   
                //Create six cards 
                UrbanBuildingCard Age_A_Philosophy = new UrbanBuildingCard("AgeAPhilosophy",0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Philosophy), 3, 0, 0, 0, 1,UrbanBuildingType.Lab) { Unit = 1 };
                UrbanBuildingCard Age_A_Religion = new UrbanBuildingCard("AgeAReligion",0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Religion), 3, 0, 1, 1, 0, UrbanBuildingType.Temple);
                AgriCard Age_A_Agri = new AgriCard("AgeAAgri",0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Agriculture), 2,0,1) { Unit = 2 };
                MineCard Age_A_Bronze = new MineCard("AgeABronze",0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Bronze), 2,0, 1) { Unit = 2 };
                MilitaryTechCard Age_A_Warriors = new MilitaryTechCard(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Warriors), 2,0,1,"Warriors") {Unit = 1 };
                GovCard Age_A_Despotism = new GovCard("Despotism",0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Despotism), 4, 2, 2);

                players[i].CardsOnBoard.Add(Age_A_Philosophy);
                players[i].CardsOnBoard.Add(Age_A_Religion);
                players[i].CardsOnBoard.Add(Age_A_Agri);
                players[i].CardsOnBoard.Add(Age_A_Bronze);
                players[i].CardsOnBoard.Add(Age_A_Warriors);
                players[i].CardsOnBoard.Add(Age_A_Despotism);

                players[i].Gov = Age_A_Despotism;
                Players[i].AgriCardList.Add(Age_A_Agri);
                players[i].MineCardList.Add(Age_A_Bronze);
                players[i].UrbanBuildingCardList.Add(Age_A_Philosophy);
                players[i].UrbanBuildingCardList.Add(Age_A_Religion);
                players[i].MiliTechCardList.Add(Age_A_Warriors);
             
            }
        }

        private void InitializeCardBoard(List<int> random)
        {
            GameBoard = new Board(NumberOfPlayers);
            
            //Finished  
            //Action
            DirectActionCard Age_A_Work_of_Art = new DirectActionCard("AgeAWorkOfArt",0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Work_of_Art),DataType.CulturePoint,6); 
            DirectActionCard Age_A_Revolutionary_Idea = new DirectActionCard("AgeARevolutionary",0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Revolutionary_Idea), DataType.SciencePoint, 1);
            DirectActionCard Age_I_Bountiful_Harvest = new DirectActionCard("AgeIHarvest", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Bountiful_Harvest), DataType.Food, 2);
            DirectActionCard Age_I_Mineral_Deposits = new DirectActionCard("AgeIDeposits", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Mineral_Deposits_dsn), DataType.Ore, 2);
            DirectActionCard Age_I_Revolutionary_Idea = new DirectActionCard("AgeIRevolutionary", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Revolutionary_Idea_dsn), DataType.SciencePoint, 2);
            DirectActionCard Age_I_Work_of_Art = new DirectActionCard("AgeIWorkOfArt", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Work_of_Art), DataType.CulturePoint, 5);

            RichLand Age_A_RichLand = new RichLand(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Rich_Land), "RichLand", 1);
            Frugality Age_A_Frugality = new Frugality(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Frugality), "Frugality", 1);  
            IdealBuildingSite Age_A_Ideal_Building_Site = new  IdealBuildingSite(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Ideal_Building_Site),"IdealBuildingSite",1);
            Frugality Age_I_Frugality = new Frugality(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Frugality), "Frugality_I", 2);
            IdealBuildingSite Age_I_Ideal_Building_Site = new IdealBuildingSite(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Ideal_Building_Site_dsn), "IdealBuildingSite_I", 2);
            RichLand Age_I_RichLand = new RichLand(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Rich_Land_dsn), "RichLand_I", 2);
            Patriotism Age_A_Patriotism = new Patriotism(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Patriotism), "Patriotism", 1);
            Patriotism Age_I_Patriotism = new Patriotism(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Patiotism_Age_1_dsn), "Patriotism_I", 2);
            Breakthrough Age_I_Breakthrough = new Breakthrough(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Breakthrough), "Breakthrough");
      
            //leader
            Leader.Hammurabi Age_A_Hammurabi = new Leader.Hammurabi(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Hammurabi),"Hammurabi");    
            Leader.Moses Age_A_Moses = new Leader.Moses(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Moses),"Moses");                
            Leader.Aristotle Age_A_Aristotle = new Leader.Aristotle(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Aristotle),"Aristotle");
            Leader.Alexander Age_A_Alexander = new Leader.Alexander(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Alexander_the_Great), "Alexander");
            Leader.Homer Age_A_Homer = new Leader.Homer(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Homer), "Homer");
            Leader.Caesar Age_A_Julius_Caesar = new Leader.Caesar(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Julius_Caesar), "Caser");
            Leader.Barbarossa Age_I_Barbarossa = new Leader.Barbarossa(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Frederick_Barbarrossa), "Barbarossa");
            Leader.Khan Age_I_Khan = new Leader.Khan(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Genghis_Khan),"Khan");
            Leader.Joan Age_I_Joan = new Leader.Joan(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Joan_of_Arc_dsn), "Joan");
            Leader.DaVinci Age_I_DaVinci = new Leader.DaVinci(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Leonardo_da_Vinci_dsn), "DaVinci");
            Leader.Mich Age_I_Mich = new Leader.Mich(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Michelangelo_dsn), "Mich");
               
            //Unit            
            AgriCard Age_I_Irrigation = new AgriCard("AgeIIrrigation",1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Irrigation_dsn), 4,3,2);

            MineCard Age_I_Iron = new MineCard("AgeIIron",1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Iron_dsn), 5, 5, 2);

            UrbanBuildingCard Age_I_Alchemy = new UrbanBuildingCard("AgeIAlchemy", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Alchemy), 6, 4, 0, 0, 2,UrbanBuildingType.Lab);
            UrbanBuildingCard Age_I_Bread = new UrbanBuildingCard("AgeIBread", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Bread_and_Circuses), 4, 3, 0, 2, 0,UrbanBuildingType.Arena);
            UrbanBuildingCard Age_I_PrintingPress = new UrbanBuildingCard("AgeIPrintingPress", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Printing_Press_1_dsn), 4, 3, 1, 0, 1,UrbanBuildingType.Lab);
            UrbanBuildingCard Age_I_Theology = new UrbanBuildingCard("AgeITheology", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Theology_dsn), 5, 2, 1, 2, 0, UrbanBuildingType.Temple);
            UrbanBuildingCard Age_I_Drama = new UrbanBuildingCard("AgeIDrama",1,BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Drama),5,4,2,1,0,UrbanBuildingType.Theater);

            MilitaryTechCard Age_I_Knights = new MilitaryTechCard(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___knights_dsn),3,4,2,"Knights");
          
            //Special
            DirectSpecialCard Age_I_CodeOfLaws = new DirectSpecialCard("AgeICodeOfLaws",1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Code_of_Laws), 6, DataType.CivilPoint, 1);
            DirectSpecialCard Age_I_CartoGraphy = new DirectSpecialCard("AgeICartoGraphy",1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Cartography), 4, DataType.Strength, 1);
            //Unfinished
            //Genhis han
            //breakthrough
            //barbarrossa
            //davinci
            //michenleglo
            //joan of arc
            //efficient upgrad
            Card Age_A_Engineering_Genius = new Card(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Engineering_Genius),"AgeAEngineering");
               
           
                             
            // In queue for finish
                                 
            List<Card> tempList = new List<Card>();  
         
         
            
            //Age A 13 cards

            tempList.Add(Age_A_Alexander);
            tempList.Add(Age_A_Aristotle);
            tempList.Add(Age_A_Engineering_Genius);
            tempList.Add(Age_A_Frugality);
            tempList.Add(Age_A_Hammurabi);
            tempList.Add(Age_A_Homer);
            tempList.Add(Age_A_Ideal_Building_Site);
            tempList.Add(Age_A_Julius_Caesar);
            tempList.Add(Age_A_Moses);
            tempList.Add(Age_A_Patriotism);

            tempList.Add(Age_A_Revolutionary_Idea);
            tempList.Add(Age_A_RichLand);
            tempList.Add(Age_A_Work_of_Art);  

            //Age I 23 cards

            tempList.Add(Age_I_Breakthrough);
            tempList.Add(Age_I_DaVinci);
            tempList.Add(Age_I_Khan);
            tempList.Add(Age_I_Joan);        
            tempList.Add(Age_I_Barbarossa);
            tempList.Add(Age_I_Knights);
            tempList.Add(Age_I_Bountiful_Harvest);
            tempList.Add(Age_I_Patriotism);
            tempList.Add(Age_I_Frugality); 
            tempList.Add(Age_I_Irrigation);

            tempList.Add(Age_I_Alchemy);
            tempList.Add(Age_I_Bread);
            tempList.Add(Age_I_Ideal_Building_Site);
            tempList.Add(Age_I_Iron);
            tempList.Add(Age_I_Mineral_Deposits);
            tempList.Add(Age_I_Revolutionary_Idea);
            tempList.Add(Age_I_RichLand);
            tempList.Add(Age_I_Work_of_Art);
            tempList.Add(Age_I_CodeOfLaws);
            tempList.Add(Age_I_PrintingPress);

            tempList.Add(Age_I_Theology);
            tempList.Add(Age_I_CartoGraphy);
            tempList.Add(Age_I_Drama);
            
           
            AddCards(tempList, random);  
         
            //Need redesign
            //Create Clone
            CloneList.Add(new DirectActionCard("AgeAWorkOfArt", 0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Work_of_Art), DataType.CulturePoint, 6));
            CloneList.Add(new DirectActionCard("AgeARevolutionary", 0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Revolutionary_Idea), DataType.SciencePoint, 1));
            CloneList.Add(new DirectActionCard("AgeIHarvest", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Bountiful_Harvest), DataType.Food, 2));
            CloneList.Add(new DirectActionCard("AgeIDeposits", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Mineral_Deposits_dsn), DataType.Ore, 2));
            CloneList.Add(new DirectActionCard("AgeIRevolutionary", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Revolutionary_Idea_dsn), DataType.SciencePoint, 2));
            CloneList.Add(new DirectActionCard("AgeIWorkOfArt", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Work_of_Art), DataType.CulturePoint, 5));

            CloneList.Add(new RichLand(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Rich_Land), "RichLand", 1));
            CloneList.Add(new Frugality(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Frugality), "Frugality", 1));
            CloneList.Add(new IdealBuildingSite(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Ideal_Building_Site), "IdealBuildingSite", 1));
            CloneList.Add(new Frugality(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Frugality), "Frugality_I", 2));
            CloneList.Add(new IdealBuildingSite(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Ideal_Building_Site_dsn), "IdealBuildingSite_I", 2));
            CloneList.Add(new RichLand(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Rich_Land_dsn), "RichLand_I", 2));
            CloneList.Add(new Patriotism(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Patriotism), "Patriotism", 1));
            CloneList.Add(new Patriotism(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Patiotism_Age_1_dsn), "Patriotism_I", 2));
            CloneList.Add(new Breakthrough(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Breakthrough), "Breakthrough"));

            //leader
            CloneList.Add(new Leader.Hammurabi(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Hammurabi), "Hammurabi"));
            CloneList.Add(new Leader.Moses(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Moses), "Moses"));
            CloneList.Add(new Leader.Aristotle(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Aristotle), "Aristotle"));
            CloneList.Add(new Leader.Alexander(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Alexander_the_Great), "Alexander"));
            CloneList.Add(new Leader.Homer(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Homer), "Homer"));
            CloneList.Add(new Leader.Caesar(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Julius_Caesar), "Caser"));
            CloneList.Add(new Leader.Barbarossa(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Frederick_Barbarrossa), "Barbarossa"));
            CloneList.Add(new Leader.Khan(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Genghis_Khan), "Khan"));
            CloneList.Add(new Leader.Joan(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Joan_of_Arc_dsn), "Joan"));
            CloneList.Add(new Leader.DaVinci(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Leonardo_da_Vinci_dsn), "DaVinci"));
            CloneList.Add(new Leader.Mich(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Michelangelo_dsn), "Mich"));

            //Unit            
            CloneList.Add(new AgriCard("AgeIIrrigation", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Irrigation_dsn), 4, 3, 2));

            CloneList.Add(new MineCard("AgeIIron", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Iron_dsn), 5, 5, 2));

            CloneList.Add(new UrbanBuildingCard("AgeIAlchemy", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Alchemy), 6, 4, 0, 0, 2,UrbanBuildingType.Lab));
            CloneList.Add(new UrbanBuildingCard("AgeIBread", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Bread_and_Circuses), 4, 3, 0, 2, 0,UrbanBuildingType.Arena));
            CloneList.Add(new UrbanBuildingCard("AgeIPrintingPress", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Printing_Press_1_dsn), 4, 3, 1, 0, 1,UrbanBuildingType.Lab));
            CloneList.Add(new UrbanBuildingCard("AgeITheology", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Theology_dsn), 5, 2, 1, 2, 0, UrbanBuildingType.Temple));
            CloneList.Add(new UrbanBuildingCard("AgeIDrama", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Drama), 5, 4, 2, 1, 0, UrbanBuildingType.Theater));

            CloneList.Add(new MilitaryTechCard(1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___knights_dsn), 3, 4, 2, "Knights"));

            //Special
            CloneList.Add(new DirectSpecialCard("AgeICodeOfLaws", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Code_of_Laws), 6, DataType.CivilPoint, 1));
            CloneList.Add(new DirectSpecialCard("AgeICartoGraphy", 1, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I___Cartography), 4, DataType.Strength, 1));
            //Unfinished
            //Genhis han
            //breakthrough
            //barbarrossa
            //davinci
            //michenleglo
            //joan of arc
            //efficient upgrad
            CloneList.Add(new Card(0, BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A___Engineering_Genius), "AgeAEngineering"));
        }


        private void AddCards(List<Card> list,List<int> random)
        {
            for (int i = 0; i < random.Count;i++)
            {
                GameBoard.CardList.Add(list[random[i]]);
            }
        }

        

        #endregion Initialization

    

        public void StartMyTurn() {

            if (firstRound)
            {          
                for (int i = 0; i < MyIndex + 1; i++)
                {
                    Players[i].PlayFirstRound();
                } 
                firstRound = false;
            }
            else
            {
                me.Play();
            }

            reversePackList.Add(new ReversePack(GameAction.DoNothing));
            SendMessage("Initial State");
            SavePlayerData();

        }

        public void StartATurn(int removedCards = 0)
        {
       
            if (GameManager.gameRound == 1)
            {       
               if(!IsMyTurn())
               currentP.PlayFirstRound();

               if (IndexOfCurrentP > MyIndex) {
                  GameBoard.UpdateUnreachableCards(MyIndex, IndexOfCurrentP, NumberOfPlayers);
               } 
            }
            else 
            {                      
                GameBoard.ReplaceCards(removedCards);
                GameBoard.UpdateUnreachableCards(MyIndex,IndexOfCurrentP,NumberOfPlayers);

                if(!IsMyTurn())
                currentP.Play();  
            }        
        }

        public void FinishATurn()   
        {
            if(IsMyTurn())
            reversePackList.Clear();

            Players[IndexOfCurrentP].FinishMyTurn(IndexOfCurrentP);

            if (GameBoard.IsLastRound && IndexOfCurrentP == NumberOfPlayers - 1)
            {
                IsLastTurn = true;
            }

        }

        public string GetWinner()
        {

            string final = "Final Culture Points\n\n";
            string winners = " ";

            int topCP = 0, numOfWinners = 1;

            foreach (Player player in Players)
            {

                int finalCP = player.CulturePoint;
                int temp = player.Happiness * 2;
                temp = temp > 16 ? 16 : temp;

                finalCP += temp;
                finalCP += player.Science;

                foreach (AgriCard card in player.AgriCardList)
                {
                    finalCP += card.Unit;
                }

                foreach (MineCard card in player.MineCardList)
                {
                    finalCP += card.Unit;
                }

                if (finalCP > topCP)
                {
                    topCP = finalCP;
                    numOfWinners = 1;
                    winners = player.MyName;
                }
                else if (finalCP == topCP)
                {
                    numOfWinners++;
                    winners += ", " + player.MyName;
                }

                final += player.MyName + ": " + finalCP.ToString() + "\n";
            }

            if (numOfWinners > 1) { final += "\nWinners are " + winners; }
            else { final += "\nWinner is " + winners; }

            return final;
        }


        public bool Destroy_A_Unit(UnitCard card) {
  
            int APcost = BASIC_ACTION_COST;

            if (GameManager.gameRound != 1)
            {
                if (currentP.CheckCAP(APcost))
                {
                    currentP.ConsumeCAP(APcost);
                    card.Unit--;
                    return true;
                }
            }
            else { FeedBackText.Instance.Text = "Cannot destory a unit in the first round"; }

           return false;
        }

        public bool UpGradeAUnit(UnitCard from, UnitCard to) {

            int APcost = BASIC_ACTION_COST;
            if (to.Age > from.Age&&to.MyClass == from.MyClass)
            {
                int cost = to.Cost - from.Cost;

                if (currentP.CheckCAP(APcost))
                {
                    if (currentP.Consume(cost, DataType.Ore))
                    {
                        currentP.ConsumeCAP(APcost);
                        from.Unit--;
                        to.Unit++;
                        return true;
                    }
                }
                
            }
            else { FeedBackText.Instance.Text = "Cannot upgrade this unit"; }
            return false;
        }

        public bool IsMyTurn() {
            return MyIndex == IndexOfCurrentP;
        }

        public bool CheckPickUpACard(Card card, int cardPos)
        {
            if (!GameBoard.CardList.Contains(card)) {
                FeedBackText.Instance.Text = "Card taken by others";
                return false;
            }


            if (cardPos < GameBoard.UnreachableCards) {
                FeedBackText.Instance.Text = "This card will not be reached in you turn";
                return false;
            }

          

            if (card.MyClass == CardClass.Leader)
            {
                if (!me.CheckLeader(card.Age))
                {
                    FeedBackText.Instance.Text = "You can only have one leadership card for the same age";
                    return false;
                }
            }
    
                                                      
            if (me.HandList.Count >= me.HandLimit)
            {
                FeedBackText.Instance.Text = "You Have Reached Your Hand Card Limit";
                return false;
            }
            
            if (!me.CheckAP(GameBoard.ActionCostArray[cardPos], "PickUpCard"))
            {
                   return false;
            }
       
            return true;
        }

        public void DoPickCard(int player,int card,int cardPos,double x, double y,bool reversible) {

            Player currentP = Players[player];
     
            int AP_cost = GameBoard.ActionCostArray[cardPos];

            currentP.ConsumeCAP(AP_cost);

            //FeedBackText.Instance.Text = "Card:" + card+ " CardPos:" + cardPos;

        
            Card tempCard = GameBoard.CardList[card];


        
            

            if (GameBoard.CardList.Contains(tempCard)) {
                FeedBackText.Instance.Text = "Take from the gameboard.cardlist"; 
                GameBoard.CardList.Remove(tempCard);
            }
                             
           
            currentP.CardsOnBoard.Add(tempCard);
            currentP.HandList.Add(tempCard);
            
            
            if (tempCard.MyClass == CardClass.Leader) {
                currentP.SetLeader(tempCard.Age);
            }
                              
            //Aristotle
            if (currentP.LongEffect.Contains("Aristotle") && currentP.IsTechnologyCard(tempCard.MyClass)) {
                FeedBackText.Instance.Text = "LeadershipCard: Aristotle takes effect";
                currentP.SciencePoint++;
            }

            if (reversible)
            {
                SendMessage("Take the card " + tempCard.MyName);
                AddPack(new PickCardPack(tempCard.MyName, tempCard, x, y, cardPos, card));
            }
        }


       

        public void UndoPickCard(PickCardPack pack) {


            Card tempCard = pack.pickedCard;

            gameBoard.CardList.Insert(pack.card, tempCard);
            me.CardsOnBoard.Remove(tempCard);
            me.HandList.Remove(tempCard);

            if (tempCard.MyClass == CardClass.Leader)
            {
                me.CancelLeader(tempCard.Age);
            }
             
        }






        public bool CheckIncreasePopulation()
        {     
            int AP_cost = BASIC_ACTION_COST;         
            int food_Cost = me.wBank.GetWorkerCost();
            string action = "IncreasePopulation";

            //Moses
            if (me.LongEffect.Contains("Moses"))
            {
                food_Cost--;
            }

            //Barbarossa
            if (me.TurnEffect.Contains("Barbarossa")) {
                food_Cost--;
            }

            if(me.CheckResource(food_Cost,CardClass.None,action,DataType.Food)){
                if(me.CheckAP(AP_cost,action)){
                    return true;
                }           
            }

           return false;
        }


        public void DoIncreasePopulation(int player,bool reversible)
        {
            Player currentP = Players[player];

            int AP_Cost = BASIC_ACTION_COST;
            int food_Cost = currentP.wBank.GetWorkerCost();
            string action = "IncreasePopulation";

            int tempCol = currentP.wBank.wCol;
            int tempRow = currentP.wBank.wRow;
            
            //Moses
            if (currentP.LongEffect.Contains("Moses")) 
            {
                food_Cost--;
            }

            //Barbarossa
            if (currentP.TurnEffect.Contains("Barbarossa"))
            {
                food_Cost--;
                currentP.SpecialAP_List.Add(new SpecialAP("BuildUnit", CardType.Military, CardClass.MilitaryTech));           
                currentP.SpecialR_List.Add(new SpecialResource(CardClass.MilitaryTech, "BuildUnit"));
                currentP.TurnEffect.Remove("Barbarossa");
            }

            food_Cost = currentP.ConsumeSpecialResource(food_Cost, CardClass.None, action);
            AP_Cost = currentP.ConsumeSpecialAP(AP_Cost,action);

            currentP.Consume(food_Cost, DataType.Food);
            currentP.ConsumeCAP(AP_Cost);

         
           
        
            currentP.unusedWorkerList.Add(currentP.wBank.RemoveW());
                 
            //Frugality Implementation
            if (currentP.TurnEffect.Contains("Frugality"))
            {
                currentP.Produce_Amount(1, DataType.Food);
                currentP.TurnEffect.Remove("Frugality");
            }
            if (currentP.TurnEffect.Contains("Frugality_I"))
            {
                currentP.Produce_Amount(2, DataType.Food);
                currentP.TurnEffect.Remove("Frugality_I");
            }

            //For reverse
            if (reversible)
            {
                SendMessage("Increase Population");
                AddPack(new IncreasePopuPack(tempCol, tempRow));


            }

        }

        public void UndoIncreasePopulation(IncreasePopuPack pack) {
             
            me.wBank.AddW(me.unusedWorkerList.Last());
            me.unusedWorkerList.Remove(me.unusedWorkerList.Last());
        }





        public bool CheckBuildUnit(UnitCard card)
        {
           
            int AP_Cost = BASIC_ACTION_COST;
            int resource_Cost = card.Cost;
            string action = "BuildUnit";


            if (card.GetType() != typeof(UrbanBuildingCard) || (card as UnitCard).Unit < me.Gov.Capacity)
            {
                if (me.CheckResource(resource_Cost, card.MyClass, action,DataType.Ore))
                {
                    if (me.CheckAP(AP_Cost, action,card.MyClass,card.MyType))
                    {                      
                        return true;
                    }
                }
            }
            else { FeedBackText.Instance.Text = "You have reached the limit of building urban units"; }

            return false;
        }

        public void DoBuildUnit(int player,UnitCard card,bool reversible) {

            Player currentP = Players[player];

            int AP_Cost = BASIC_ACTION_COST;
            int resource_Cost = card.Cost;
            string action = "BuildUnit";

            resource_Cost = currentP.ConsumeSpecialResource(resource_Cost, card.MyClass, action);
            AP_Cost = currentP.ConsumeSpecialAP(AP_Cost, action,card.MyClass,card.MyType);

           

            currentP.Consume(resource_Cost, DataType.Ore);
            currentP.ConsumeAP(AP_Cost,card.MyType);
            (card as UnitCard).Unit++;

            
            if (card.MyClass == CardClass.MilitaryTech && currentP.LongEffect.Contains("Alexander")) {
                currentP.Strength++;
            }

            if (reversible)
            {
                SendMessage("Build a Unit on " + card.MyName);
                AddPack(new BuildUnitPack(card));

            }
        }

        public void UndoBuildUnit(BuildUnitPack pack) {

       
            UnitCard tempCard = pack.card;
         
            tempCard.Unit--;
             
        }




        public bool CheckPlayACard(Card card)
        {
            Player currentP = me;
            int AP_cost = BASIC_ACTION_COST;

            if(!currentP.HandList.Contains(card)) {
                FeedBackText.Instance.Text = "This is not a hand card";
                return false;
            }

           
            if(card.GetType().IsSubclassOf(typeof(SpecialCard))) {
                if (!currentP.CheckScience((card as SpecialCard).ScienceCost))
                    return false;
            }

            if (card.GetType().IsSubclassOf(typeof(UnitCard))) {         
                if (!currentP.CheckScience((card as UnitCard).ScienceCost))
                    return false;
            }
                          
            if (!currentP.CheckAP(AP_cost,"PlayCard",card.MyClass))
            {
                return false;
            }

            return true;
        }

        public void DoPlay_A_Card(int player,Card card,bool reversible) {

            Player currentP = Players[player];
            int AP_cost = BASIC_ACTION_COST;

            if (card.GetType().IsSubclassOf(typeof(SpecialCard)))
            {
                currentP.ConsumeScience(card as SpecialCard);               
            }

            if (card.GetType().IsSubclassOf(typeof(UnitCard)))
            {
                currentP.ConsumeScience(card as UnitCard);                  
            }   
            
         


            AP_cost = currentP.ConsumeSpecialAP(AP_cost, "PlayCard", card.MyClass);
            currentP.ConsumeCAP(AP_cost);
            currentP.Play_A_Card(card);
            currentP.HandList.Remove(card);

               
            if (currentP.TurnEffect.Contains("Breakthrough") && currentP.IsTechnologyCard(card.MyClass))
            {
                currentP.SciencePoint += 2;         
                currentP.TurnEffect.Remove("Breakthrough");
            }

            if (currentP.LongEffect.Contains("DaVinci") && currentP.IsTechnologyCard(card.MyClass))
            {
                currentP.Produce_Amount(1, DataType.Food);
            }


            if (reversible)
            {
                SendMessage("Play the card " + card.MyName);
                if (card.MyClass == CardClass.Leader && Players[player].MyLeader != null)
                    AddPack(new PlayCardPack(card.MyClass, card, Players[player].MyLeader));
                else
                    AddPack(new PlayCardPack(card.MyClass, card));

            }
        
        }

        public void UndoPlayCard(PlayCardPack pack) {

            Card card = pack.card;
            me.HandList.Add(card);
      
            //Leader
            if (card.MyClass == CardClass.Leader)
            {
                me.MyLeader = null;
                if (pack.removedLeaderCard != null)
                {
                    me.MyLeader = pack.removedLeaderCard;
                }          
            }

            //UnitCard
            if (card.GetType().IsSubclassOf(typeof(UnitCard)))
            {
                switch (card.MyClass)
                {
                    case CardClass.Farm: me.AgriCardList.Remove(card as AgriCard); break;
                    case CardClass.Mine: me.MineCardList.Remove(card as MineCard); break;
                    case CardClass.UrbanBuilding: me.UrbanBuildingCardList.Remove(card as UrbanBuildingCard); break;
                    case CardClass.MilitaryTech: me.MiliTechCardList.Remove(card as MilitaryTechCard); break;

                }
            }

            /*//SpecialCard
            if (card.GetType().IsSubclassOf(typeof(SpecialCard)))
            {
                if (card.GetType() == typeof(DirectSpecialCard))
                {
                    (card as DirectSpecialCard).Act(this);
                }
                else { throw new NotImplementedException(); }

            }*/
      
        }

        public bool CheckDestroy_A_Unit(UnitCard card)
        {

            int APcost = BASIC_ACTION_COST;
          
                if (currentP.CheckAP(APcost,"DestroyUnit",CardClass.None,card.MyType))
                {
                    return true;
                }
          
    
            return false;
        }

        public void DoDestroy_A_Unit(int player,UnitCard card, bool reversible)
        {
            int APcost = BASIC_ACTION_COST;
            currentP.ConsumeAP(APcost, card.MyType);    
            card.Unit--;

          
            
            //Alexander
            if (card.MyClass == CardClass.MilitaryTech && currentP.LongEffect.Contains("Alexander"))
            {
                currentP.Strength--;
                FeedBackText.Instance.Text = "Alexander--";
            }

            if (reversible)
            {
                SendMessage("Destroy a Unit from " + card.MyName);
                AddPack(new DestroyUnitPack(card));

            }
        }

        public void UndoDestoryUnit(UnitCard card) {
            card.Unit++;
        }

        #region Game Planning

        public void GenerateLocalBoard() {

         
           /*// int count = GameBoard.NumOfCards();

            LocalBoard.CardList.Clear();

            for (int i = 0; i < 13;i++ )
                LocalBoard.cardsState[i] = GameBoard.cardsState[i];

            for (int i = 0; i < count; i++)
            {
                Card tempCard = FindClone(GameBoard.CardList[i].MyName);

                tempCard.IsFaceDown = false;
                tempCard.Height = Double.NaN;
                tempCard.Width = Double.NaN;

                LocalBoard.CardList.Add(tempCard);     
            }

            //FeedBackText.Instance.Text = "Count: " + count + " " + LocalBoard.CardList.Count();
            LocalBoard.InitializeCards();*/
        }

        public Card FindClone(string name) {

            foreach (Card card in CloneList) {
                if (card.MyName == name) {
                    return card;
                }
            }
            throw new NotImplementedException();            
        }

        public bool FindTheCardFromTheRow(string name,ref Card from,ref int card) {
            for (int i = 0; i < 13; i++) {
                if (GameBoard.CardList[i].MyName == name) {
                    from = GameBoard.CardList[i];
                    card = i;
                    return true;
                }       
            }
            return false;
        }

        public Card GetCardFromMyBoard(string name) {
            foreach (Card card in me.CardsOnBoard) {
                if (card.MyName == name)
                    return card;
            }
            return null;
        }

        public void SavePlayerData() {
            playerDataPack.GetPlayerData(me); 
        }
        public void LoadPlayerData() {
            playerDataPack.GiveBackPlayerData(me);
        }

#endregion

        #region others

        bool ReverseCheck(int playerIndex) {
            return (!GameManager.IsPlayingNetwork()) && playerIndex == myIndex;
        }

      
        #endregion

        public bool IsNotFromMe(int playerIndex) {
            return myIndex != playerIndex;
        }
  

        public bool IsTheLastPlayerThisRound() {
            return MyIndex == NumberOfPlayers - 1;
        }


    }//end class
}
