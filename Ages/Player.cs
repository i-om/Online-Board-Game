using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Controls;



namespace Ages
{
    class Player:INotifyPropertyChanged
    {
        int playerIndex;
        public int PlayerIndex {
            get { return playerIndex; }
        }

        private int myIndex;
        public string MyName { get; set; }

        int tempTimer = 0;
        DispatcherTimer timer = new DispatcherTimer();

        private int amountToProduce;
        private int amountProduced;
   
              
        //Basic Events
        public event PropertyChangedEventHandler PropertyChanged;     
        public event SwitchButton switchButton;

        private int civilAP;   
        public int AP_Civil {
            get { return civilAP; }
            set { civilAP = value;
            OnPropertyChanged("CivilAP");
            }
        }

        private int militaryAP;
        public int AP_Military {
            get { return militaryAP; }
            set { militaryAP = value;
            OnPropertyChanged("MilitaryAP");
            }     
        }

        private int handLimit;
        public int HandLimit { get { return handLimit; } }
       

        public int AP_Bonus_Civil {get;set;}

   
               
        //Cards
        private List<Card> handList = new List<Card>();
        public List<Card> HandList
        {
            get { return handList; }
            set { handList = value; }
        }

       

        private List<Card> cardsOnBoard = new List<Card>();
        public List<Card> CardsOnBoard
        {
            get { return cardsOnBoard; }
            set { cardsOnBoard = value; }
        }

        private List<AgriCard> agriCardList = new List<AgriCard>();
        public List<AgriCard> AgriCardList
        {
            get { return agriCardList; }
            set { agriCardList = value; }
        }

        private List<MilitaryTechCard> miliTechCardList = new List<MilitaryTechCard>();
        public List<MilitaryTechCard> MiliTechCardList
        {
            get { return miliTechCardList; }
            set { miliTechCardList = value; }
        }

        private List<MineCard> mineCardList = new List<MineCard>();
        public List<MineCard> MineCardList
        {
            get { return mineCardList; }
            set { mineCardList = value; }
        }

        private List<UrbanBuildingCard> urbanBuildingCardList = new List<UrbanBuildingCard>();
        public List<UrbanBuildingCard> UrbanBuildingCardList
        {
            get { return urbanBuildingCardList; }
            set { urbanBuildingCardList = value; }
        }

        private GovCard gov;
        public GovCard Gov{
            get { return gov; }
            set { gov = value; }
        }

        public LeaderCard MyLeader{get;set;}

       // Leadercard register
        private int[] leaderRegister = { 0, 0, 0, 0 };
      
     
        //Banks
        public WorkerBank wBank = new WorkerBank();
        public ResourceBank rBank;
        public List<Worker> unusedWorkerList = new List<Worker>();

       
        //Effect
        public List<string> TurnEffect = new List<string>();
        public List<string> LongEffect = new List<string>();

  
        //Score
        private int culture;
        public int Culture
        {
            get { return culture; }
            set { culture = value; OnPropertyChanged("Culture");}
        }
   

        private int culturePoint;
        public int CulturePoint
        {
            get { return culturePoint; }
            set { culturePoint = value; OnPropertyChanged("CulturePoint"); }
        }  

        private int science;
        public int Science
        {
            get { return science; }
            set { science = value; OnPropertyChanged("Science"); }
        }
        


        private int sciencePoint;
        public int SciencePoint
        {
            get { return sciencePoint; }
            set { sciencePoint = value; OnPropertyChanged("SciencePoint"); }
        }   

        private int happiness;
        public int Happiness
        {
            get { return happiness; }
            set { happiness = value; OnPropertyChanged("Happiness"); }
        }
     
      
        private int strength;
        public int Strength
        {
            get { return strength; }
            set { strength = value; OnPropertyChanged("Strength"); }
        }
       
  
        protected void OnPropertyChanged(string name)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private List<SpecialResource> SpResource_List = new List<SpecialResource>();
        public List<SpecialResource> SpecialR_List
        {
            get { return SpResource_List; }
            set { SpResource_List = value; }
        }
    

        private List<SpecialAP> list_SpecialAP = new List<SpecialAP>();
        public List<SpecialAP> SpecialAP_List {
            get { return list_SpecialAP; }
            set { list_SpecialAP = value; }
        }

        public int currentPlayer;





        public Player() { }

        public Player(string name,int playerindex,int myindex)
        {
            MyName = name;

            Science = 1;
            Strength = 1;

            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0,0,tempTimer);

            playerIndex = playerindex;
            myIndex = myindex;
            rBank = new ResourceBank(playerindex);
        }

        
        bool ScorePoints = false;
        bool Production_Finished = false;
        bool Consumption_Finished = true;

        void PrepareForNewTurn() {

            ScorePoints = false;
            Production_Finished = false;
          //Consumption_Finished = false;
            amountProduced = 0;
            
            if (myIndex == currentPlayer)
            {           
                switchButton("Go To Next Player");
            }
        
            Maintain();
            ActivateActionCards();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!ScorePoints) {
              
                SciencePoint += Science;
                CulturePoint += Culture;
                ScorePoints = true;
                FeedBackText.Instance.Text = "---Producing Food and Resources---";
            }


            else if (ScorePoints && (!Production_Finished))
            {
            
                for (int i = AgriCardList.Count - 1; i > -1; i--)
                {
                    if (AgriCardList[i].Unit > amountProduced)
                    {
                        AgriCardList[i].ProduceOne(rBank);
                    }
                }



                for (int i = MineCardList.Count - 1; i > -1; i--)
                {
                    if (MineCardList[i].Unit > amountProduced)
                    {
                        MineCardList[i].ProduceOne(rBank);
                    }
                }

                amountProduced++;
                amountToProduce--;

                if (amountToProduce == 0) { Production_Finished = true; }
            }

            if (ScorePoints&&Production_Finished&&Consumption_Finished) {
                timer.Stop();
                PrepareForNewTurn();
            }
        }

        public void Play()
        {
           
            AP_Civil = Gov.CivilAP + AP_Bonus_Civil;
            handLimit = AP_Civil;
            AP_Military = Gov.MilitaryAP;
            
            if (MyLeader != null) { 
                MyLeader.PlayEveryTurn(this); 
            }
        }

        public void PlayFirstRound( )
        { 
            AP_Civil = PlayerIndex + 1;
            handLimit = AP_Civil;
            AP_Military = 0;
        }

        private void ClearBonus() {

            TurnEffect.Clear();
            SpecialAP_List.Clear();
            SpecialR_List.Clear();
                 
        }

        public void FinishMyTurn(int value) {

            currentPlayer = value;
            ClearBonus();   
            Produce();
      
        }

        /// <summary>
        /// Production
        /// </summary>
        public void Produce() {           

            //Food
            for (int i = AgriCardList.Count - 1; i > -1; i--)
            {
                if (AgriCardList[i].Unit > amountToProduce) { amountToProduce = AgriCardList[i].Unit; }
            }

            //Ore

            for (int i = MineCardList.Count - 1; i > -1; i--) {
                if (MineCardList[i].Unit > amountToProduce) { amountToProduce = MineCardList[i].Unit; }
            }

            timer.Start();
            FeedBackText.Instance.Text = "---Score Culture and Science Points---";
            
        }

        public void Produce_Amount(int num,DataType dataType) {

            if (rBank.TokenInBank > 0)
            {
                int amountToBeProduced = num;
                              
                    if (dataType == DataType.Food)
                    {  
                        int i = AgriCardList.Count;
                        while (amountToBeProduced != 0)
                        {
                            i--;
                            amountToBeProduced = AgriCardList[i].Replace(amountToBeProduced, rBank);
                        }
                    }
                    else if (dataType == DataType.Ore)
                    {
                        int i = MineCardList.Count;
                        while (amountToBeProduced != 0)
                        {
                            i--;
                            amountToBeProduced = MineCardList[i].Replace(amountToBeProduced, rBank);
                        }
                    }
            }           
            else { FeedBackText.Instance.Text = "No resources available"; }
        
        }



        //Maintain

        public void Maintain() {
            Consume(wBank.GetFoodCost(),DataType.Food);        
        }


        /// <summary>
        /// Consumption
        /// </summary>
        /// <param name="num"></param>
        /// <param name="type"></param>

        public bool Consume(int num,DataType type)
        {
            if (num < 0) { throw new NotImplementedException(); }
                                            
                int amountToBeConsumed = num;
                int i = -1;

                //Consume
                while (amountToBeConsumed > 0)
                {
                    i++;
                        if(type == DataType.Food)
                        {
                            amountToBeConsumed = AgriCardList[i].Consume(amountToBeConsumed, rBank);
                        }
                        else if(type == DataType.Ore)
                        {
                            amountToBeConsumed = MineCardList[i].Consume(amountToBeConsumed, rBank);
                        }
                
                }

                if(amountToBeConsumed < 0){
                    int _amountToBeConsumed = Math.Abs(amountToBeConsumed); 
                    while (_amountToBeConsumed != 0)
                    {
                        i--;
                        if (type == DataType.Food)
                        {
                            _amountToBeConsumed = AgriCardList[i].Replace(_amountToBeConsumed, rBank);
                        }
                        else if (type == DataType.Ore)
                        {
                            _amountToBeConsumed = MineCardList[i].Replace(_amountToBeConsumed, rBank);
                        }  
                    }   
                }

                return true;
                                                                                         
        }

        public bool CheckConsumption(int num, DataType tempC)
        {
            int amount = 0;
            if (tempC == DataType.Food)
            {
                foreach (AgriCard card in AgriCardList)
                {
                    amount += card.Product;
                }
            }
            else if (tempC == DataType.Ore)
            {
              
                foreach (MineCard card in MineCardList)
                {
                    amount += card.Product;
                }
            }

            if (amount < num) {
                FeedBackText.Instance.Text = "Don't have enough resources";
                return false; }


            return true;
        }

       

        public void ConsumeCAP(int value) { 
                AP_Civil -= value; 
        }

        public void ConsumeMAP(int value) {
            AP_Military -= value;    
        }

        public void ConsumeAP(int APcost, CardType type)
        {
            if (type == CardType.Civil){
                ConsumeCAP(APcost);
            }
            else{
                ConsumeMAP(APcost);
            }
        }

       /* public void GiveBackCAP(int value) {
            AP_Civil += value;
        }

        public void GiveBackMAP(int value)
        {
            AP_Military += value;
        }

        public void GiveBackAP(int APcost, CardType type)
        {

            if (type == CardType.Civil)
            {
                GiveBackCAP(APcost);
            }
            else
            {
                GiveBackMAP(APcost);
            }
        }*/

        public bool CheckCAP(int value) {       

                if (AP_Civil < value)
                {
                    FeedBackText.Instance.Text = "Don't have enough Civil Action Points";
                    return false;
                }
            
            return true;
        }

        public bool CheckMAP(int value) {
            if (AP_Military < value) {
                FeedBackText.Instance.Text = "Don't have enough Military Action Points";
                return false;
            }
            return true;
        }

       

        public bool CheckScience(int cost)
        {
            if (SciencePoint < cost)
            {
                FeedBackText.Instance.Text = "Don't have enough Science Points";
                return false;
            }
            return true;
        }

        public void ActivateActionCards() {
            foreach (Card card in HandList) {
                if (card.IsPlayable == false && card.MyClass == CardClass.Action) {
                    card.IsPlayable = true;
                    }
            }
        }
        
        
        
        public bool CheckLeader(int cardAge) {
           
            return leaderRegister[cardAge] == 0;
        }

        public void SetLeader(int cardAge) {

            leaderRegister[cardAge] = 1;
            
        }

        public void CancelLeader(int cardAge) {
            leaderRegister[cardAge] = 0;
        }

        



        //new
        public bool CheckResource(int resourceCost, CardClass cardClass, string action,DataType type)
        {

            for (int i = 0; i < SpecialR_List.Count; i++)
            {
                if (SpecialR_List[i].isMatched(cardClass, action))
                {
                    resourceCost--;
                }
            }

            int myResource = 0;

            if (type == DataType.Ore)
            {
                myResource = GetTotalOre();
            }

            if (type == DataType.Food) {

                myResource = GetTotalFood();
            }

            if (myResource < resourceCost)
            {
                FeedBackText.Instance.Text = "Don't have enough resources";
                return false;
            }

            return true;
        }


        public bool CheckAP(int APcost, string action,CardClass _class = CardClass.Other,CardType type = CardType.Civil)
        {
             for (int i = 0; i < SpecialAP_List.Count; i++) {
                if(SpecialAP_List[i].isMatched(action,_class,type)){
                    APcost--;
                }
            }
            return type == CardType.Civil ? CheckCAP(APcost) : CheckMAP(APcost);
        }



        public void ConsumeScience(SpecialCard card) {
            SciencePoint -= card.ScienceCost;
        }

        public void ConsumeScience(UnitCard card) {
            SciencePoint -= card.ScienceCost;
        }

     

        public int ConsumeSpecialResource(int resourceCost, CardClass cardClass, string action)
        {
           
                for (int i = 0; i < SpecialR_List.Count && resourceCost > 0; )
                {
                    if (SpecialR_List[i].isMatched(cardClass, action))
                    {

                        resourceCost--;
                        SpecialR_List.RemoveAt(i);
                        
                    }
                    else {
                        i++;
                    }
                }
          

            return resourceCost;
        }

        public int ConsumeSpecialAP(int APcost,string action,CardClass _class = CardClass.None, CardType type = CardType.Civil ) {

            for (int i = 0; i < SpecialAP_List.Count && APcost > 0; )
            {
                if (SpecialAP_List[i].isMatched(action, _class, type))
                {
                    APcost--;
                    SpecialAP_List.RemoveAt(i);         
                }
                else {
                    i++;
                }
            }

            return APcost;
        }



        public bool IsTechnologyCard(CardClass cardClass) {
            return cardClass == CardClass.Farm || cardClass == CardClass.Mine || cardClass == CardClass.UrbanBuilding
                || cardClass == CardClass.Special || cardClass == CardClass.MilitaryTech || cardClass == CardClass.Govt;
    }

        public void Play_A_Card(Card card)
        {         
                //ActionCard
                if (card.MyClass == CardClass.Action)
                {
                    (card as ActionCard).Act(this);
                    (card as ActionCard).GoToWaste(this);
                }

                //Leader
                if (card.MyClass == CardClass.Leader)
                {
                    if (MyLeader != null)
                    {
                        MyLeader.Remove(this);
                    }                
                    (card as LeaderCard).PlayOnce(this);
                    MyLeader = (card as LeaderCard);
                }

                //UnitCard
                if (card.GetType().IsSubclassOf(typeof(UnitCard)))
                {
                    switch (card.MyClass) {
                        case CardClass.Farm: this.AgriCardList.Add(card as AgriCard); break;
                        case CardClass.Mine: this.MineCardList.Add(card as MineCard); break;
                        case CardClass.UrbanBuilding: this.UrbanBuildingCardList.Add(card as UrbanBuildingCard); break;
                        case CardClass.MilitaryTech: this.MiliTechCardList.Add(card as MilitaryTechCard); break;
                    
                    }                                 
                }

                //SpecialCard
                if (card.GetType().IsSubclassOf(typeof(SpecialCard)))
                {                                                          
                        if (card.GetType() == typeof(DirectSpecialCard))
                        {
                            (card as DirectSpecialCard).Act(this);
                        }
                        else { throw new NotImplementedException(); }
                 
                }

                //currentEffect
                if (IsTechnologyCard(card.MyClass))
                {
                    if (TurnEffect.Contains("DaVinci"))
                    {
                        Produce_Amount(1, DataType.Ore);
                        TurnEffect.Remove("DaVinci");
                    }
                    if (TurnEffect.Contains("Breakthrough"))
                    {
                        Science += 2;
                        TurnEffect.Remove("Breakthrough");
                    }
                }       
        }

        public int GetTotalFood() {

            int amount = 0;

            foreach (AgriCard card in AgriCardList)
            {
                amount += card.Product;
            }

            return amount;
        }

        public int GetTotalOre() {

            int amount = 0;

            foreach (MineCard card in MineCardList)
            {
                amount += card.Product;
            }

            return amount;
        }

        public string MyInfo() {
            string info = "";
            info += "\nSp:" + SciencePoint;
            info += " S:" + Science;
            info += " Cp:" + CulturePoint;
            info += " C:"+Culture;
            info += "\nHap:" + Happiness;
            info += " Str:" + Strength;
            info += " Food:" + GetTotalFood();
            info += " Ore:" + GetTotalOre();
            info += "\nSpecialResource:" + SpResource_List.Count;
            info += "\nSpecailAP" + SpecialAP_List.Count;
            return info;
        }
       
    }
}
