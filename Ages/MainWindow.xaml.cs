using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

//Date 3/10/2013
namespace Ages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int GameMode = 1;
           
        const int CardHeight = 150;
        const int CardWidth = 100;
        const int TokenDiameter = 20;
        const int CardLeftOffset = 5;
        const int CardTopOffset = 40;
        const int UnitCardTokenSpacing = 15;
        const int WorkerPoolTokenSpacing = 10;
        const int InitialWorkerPosY = 10;
        const int InitialWorkerPosX = 5;
       

        Player Me {
            get { return mainGame.Players[index]; }
        }

        Grid myWB {
            get { return gridList_WorkerBank[index]; }
        }

        Canvas myWP {
            get { return canvasList_WorkerPool[index]; }
        }

        Canvas myPC {
            get { return canvasList_Player[index]; }
        }

        int PackCount { get { return mainGame.reversePackList.Count; } }

        

        //My Data
        private int index = 0;
        private int cardsAlreadyRemoved = 0;     
        private int moveAbleCards = 6;
        private int currentState = -1;

        List<string> pNameList = new List<string>();
        ObservableCollection<ActionInfo> actionRegister = new ObservableCollection<ActionInfo>();
        private List<string> CardsOnTheRow = new List<string>();
        private List<MovePack> CardsToMoveList = new List<MovePack>();
 
        
        List<Canvas> canvasList_Player = new List<Canvas>();
        List<Canvas> canvasList_WorkerPool = new List<Canvas>();
        List<Grid> gridList_WorkerBank = new List<Grid>();
        List<Grid> gridList_ResourceBank = new List<Grid>();
        List<TabItem> tabItem_Players = new List<TabItem>();

        private bool btnInStateFMT = true;


        ThroughTheAges mainGame = new ThroughTheAges();

        private void InitializeBackEnd() {
            
            //for test
            btn_Connect.IsEnabled = false;
            textBoxChatPane.Text += "Connected\n";
            _backend = new ChatBackend.ChatBackend(this.DisplayMessage);


            _backend.OnJoin += OnJoin;
            _backend.OnStartGame += GameStart;
            _backend.OnMoveCard += OnMoveACardHandler;
            _backend.OnPickCard += OnPickCardHandler;
            _backend.OnConsumeCAP += OnConsumeCAPHandler;    
            _backend.OnUpdatePlayer += OnUpdatePlayerHandler;
            _backend.OnFinishTurn += OnFinishTurnHandler;
            _backend.OnIncreasePopulation += OnIncreasePopulationHandler;
            _backend.OnBuildUnit += OnBuildUnitHandler;
            _backend.OnPlayCard += OnPlayCardHandler;
            _backend.OnDestroyUnit += OnDestroyUnitHandler;
            _backend.OnReverseData += OnReverseDataHandler;
                

            _backend.StartService();
            btn_Join.IsEnabled = true;
        }

     

        public MainWindow()
        {
              
            InitializeComponent();

            grid_StartMenu.Visibility = Visibility.Visible;
            mainTabControl.DataContext = mainGame;
            Lbox_Actions.ItemsSource = actionRegister;
    
            InitializeBackEnd();
        }



        #region Game Initialization

        public void InitializeGameVisual() {

            AddLayoutControls();
            InitializeBoardAndCards();
            InitializeBanksAndWorkerPools();
            mainGame.GameBoard.InitializeCards();
            CardsOnTheRow = GetCardsOnTheRowRoundOne();

            InitializeTabs();

        }


         void InitializeTabs(){

             for (int i = 0; i < mainGame.NumberOfPlayers; i++) {
                 tabItem_Players[i].Visibility = Visibility.Visible;
                 tabItem_Players[i].Header = mainGame.Players[i].MyName;
                 tabItem_Players[i].MinWidth = 50;
             }
             HighlightTab(0);
         }

     

        public void AddLayoutControls() {

            gridList_ResourceBank.Add(gridOne_ResourceBank);
            gridList_ResourceBank.Add(gridTwo_ResourceBank);
            gridList_ResourceBank.Add(gridThree_ResourceBank);
            gridList_ResourceBank.Add(gridFour_ResourceBank);

            gridList_WorkerBank.Add(gridOne_WorkerBank);
            gridList_WorkerBank.Add(gridTwo_WorkerBank);
            gridList_WorkerBank.Add(gridThree_WorkerBank);
            gridList_WorkerBank.Add(gridFour_WorkerBank);
            
            canvasList_Player.Add(canvas_PlayerOne);
            canvasList_Player.Add(canvas_PlayerTwo);
            canvasList_Player.Add(canvas_PlayerThree);
            canvasList_Player.Add(canvas_PlayerFour);

            canvasList_WorkerPool.Add(canvas_WorkerPool);
            canvasList_WorkerPool.Add(canvasTwo_WorkerPool);
            canvasList_WorkerPool.Add(canvasThree_WorkerPool);
            canvasList_WorkerPool.Add(canvasFour_WorkerPool);

            tabItem_Players.Add(tabItem_PlayerOne);
            tabItem_Players.Add(tabItem_PlayerTwo);
            tabItem_Players.Add(tabItem_PlayerThree);
            tabItem_Players.Add(tabItem_PlayerFour);
            
            
            mainGame.GameBoard.RefreshCardBoard += GameBoard_RefreshBoard;
           // mainGame.LocalBoard.RefreshCardBoard += LocalBoard_RefreshCardBoard;

            //refactor?
            btn_Finish.IsEnabled = mainGame.IsMyTurn();
 
           
           
            //Listbox
            mainGame.SendMessage += SendMessageHandler;
            mainGame.AddPack += AddPackHandler;
                    
                    //GameGrid
                    GameGrid.AllowDrop = true;
                    GameGrid.PreviewDragOver += GameGrid_PreviewDragOver;
                    //GameGrid.PreviewMouseMove +=GameGrid_PreviewMouseMove;
                   //InitializePlayerBoards
                    canvas_PlayerOne.AllowDrop = true;
                    canvas_PlayerTwo.AllowDrop = true;
                    canvas_PlayerThree.AllowDrop = true;
                    canvas_PlayerFour.AllowDrop = true;

                    canvas_PlayerOne.Drop += Canvas_Drop;
                    canvas_PlayerOne.DragOver += Canvas_DragOver;

                    canvas_PlayerTwo.Drop += Canvas_Drop;
                    canvas_PlayerTwo.DragOver += Canvas_DragOver;

                    canvas_PlayerThree.Drop += Canvas_Drop;
                    canvas_PlayerThree.DragOver += Canvas_DragOver;

                    canvas_PlayerFour.Drop += Canvas_Drop;
                    canvas_PlayerFour.DragOver += Canvas_DragOver;


                    //WorkerPools
                    canvas_WorkerPool.AllowDrop = true;
                    canvasTwo_WorkerPool.AllowDrop = true;
                    canvasThree_WorkerPool.AllowDrop = true;
                    canvasFour_WorkerPool.AllowDrop = true;

                    canvas_WorkerPool.PreviewDrop += WorkerPool_PreviewDrop;
                    canvas_WorkerPool.PreviewDragOver += WorkerPool_PreviewDragOver;

                    canvasTwo_WorkerPool.PreviewDrop += WorkerPool_PreviewDrop;
                    canvasTwo_WorkerPool.PreviewDragOver += WorkerPool_PreviewDragOver;

                    canvasThree_WorkerPool.PreviewDrop += WorkerPool_PreviewDrop;
                    canvasThree_WorkerPool.PreviewDragOver += WorkerPool_PreviewDragOver;

                    canvasFour_WorkerPool.PreviewDrop += WorkerPool_PreviewDrop;
                    canvasFour_WorkerPool.PreviewDragOver += WorkerPool_PreviewDragOver;

              
        }

        void SendMessageHandler(string message)
        {
            currentState++;   
            actionRegister.Insert(currentState,new ActionInfo(message));
            Lbox_Actions.SelectedIndex = currentState;
                  
        }

        void CheckAndDoOtherActions(int index) {
            Tbox5.Text = "CheckAndDOother";
            DoAllActions(PackCount,currentState);
            ReverseDataAndActions(currentState + 1);
            DoAllActions(index + 1);

        }

        void AddPackHandler(ReversePack pack) {
            mainGame.reversePackList.Insert(currentState, pack);
            if (currentState > 0 && currentState != PackCount - 1)
                CheckAndDoOtherActions(currentState);
        }

        bool IsLboxSelected() {
            return Lbox_Actions.SelectedIndex > -1;
        }

          
        public void InitializeBoardAndCards() {

    
            //Each Player's board
            for (int i = 0; i < mainGame.NumberOfPlayers; i++)
            {
                for (int j = 0; j < 6; j++)  //six cards at the beginning
                {
                    Card tempC = mainGame.Players[i].CardsOnBoard[j];

                    tempC.Height = CardHeight;
                    tempC.Width = CardWidth;

                    //Initialize Card position
                    Canvas.SetLeft(tempC, 20 + j * 110);
                    Canvas.SetTop(tempC, 20);

                    canvasList_Player[i].Children.Add(tempC);
                 
                    if (j == 0 || j == 4)
                    {
                        Worker tempW = new Worker(Token_MouseLeftButtonDown, Token_MouseMove, Token_MouseLeftButtonUp);
                             
                        Canvas.SetTop(tempW, CardTopOffset);
                        Canvas.SetLeft(tempW, CardLeftOffset);

                        tempC.Children.Add(tempW);
                    }

                    if (j == 2 || j == 3)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Worker tempW = new Worker(Token_MouseLeftButtonDown, Token_MouseMove, Token_MouseLeftButtonUp);
                             
                            Canvas.SetTop(tempW, CardTopOffset);
                            Canvas.SetLeft(tempW, CardLeftOffset + k * UnitCardTokenSpacing);
                         
                            tempC.Children.Add(tempW);
                        }
                    }

          
                    AssignControlsToCard(tempC);                 
                } 

                //ScoreBoard
                DataGrid_Score.Items.Add(mainGame.Players[i]);

                //PropertyChanged
                mainGame.Players[i].PropertyChanged += ActionPoints_PropertyChanged;
                mainGame.Players[i].rBank.PropertyChanged += rBank_PropertyChanged;
                mainGame.Players[i].switchButton += SwitchFinishTurnButtonHandler;
            }                
        }

        public void InitializeBanksAndWorkerPools()
        {
         
            for (int i = 0; i < mainGame.NumberOfPlayers; i++)
            {
                Player tempP = mainGame.Players[i];
             
                //WorkerBank
                for (int j = 0; j < WorkerBank.CAPACITY; j++)
                {
                    Worker tempW = new Worker(Token_MouseLeftButtonDown, Token_MouseMove, Token_MouseLeftButtonUp);

                    tempP.wBank.AddW(tempW);

                    Grid.SetColumn(tempW, tempP.wBank.wCol);
                    Grid.SetRow(tempW, tempP.wBank.wRow);

                    gridList_WorkerBank[i].Children.Add(tempW);
        
                                         
                }


                //ResourceBank
                for (int k = 0; k < ResourceBank.CAPACITY; k++)
                {
                    Resource tempR = new Resource(Token_MouseLeftButtonDown, Token_MouseMove, Token_MouseLeftButtonUp,false);

                    Grid.SetColumn(tempR, tempP.rBank.rCol);
                    Grid.SetRow(tempR, tempP.rBank.rRow);

                    gridList_ResourceBank[i].Children.Add(tempR);

                    if (tempP.rBank.rRow == 1)
                    {
                        tempP.rBank.rCol += 2;
                    }
                    tempP.rBank.rRow = 4 - tempP.rBank.rRow;
                }

                //WorkerPool
                Worker tempW2 = new Worker(Token_MouseLeftButtonDown,Token_MouseMove,Token_MouseLeftButtonUp);

                Canvas.SetLeft(tempW2, InitialWorkerPosX);
                Canvas.SetTop(tempW2, InitialWorkerPosY);
                            
                tempP.unusedWorkerList.Add(tempW2);           
                canvasList_WorkerPool[i].Children.Add(tempW2);

            }
        }

        public void AssignControlsToCard(Card card) {

            card.MouseLeftButtonDown += new MouseButtonEventHandler(Card_MouseLeftButtonDown);
            card.MouseMove += new MouseEventHandler(Card_MouseMove);
            card.MouseLeftButtonUp += new MouseButtonEventHandler(Card_MouseLeftButtonUp);
            card.PreviewMouseWheel += new MouseWheelEventHandler(Card_PreviewMouseWheel); 
            card.MouseRightButtonUp += new MouseButtonEventHandler(Card_MouseRightButtonUp);
            card.MouseEnter += new MouseEventHandler(Card_MouseEnter);
            card.MouseLeave += new MouseEventHandler(Card_MouseLeave);

            if(card.GetType().IsSubclassOf(typeof(UnitCard))){      
                card.AllowDrop = true;
                card.Drop += Card_Drop;
                card.DragOver += Card_DragOver;
                switch (card.MyClass) {
                    case CardClass.Farm:
                        (card as AgriCard).PropertyChanged += resource_PropertyChanged;break;
                    case CardClass.Mine:
                        (card as MineCard).PropertyChanged += resource_PropertyChanged;break;
                    case CardClass.MilitaryTech:
                        (card as MilitaryTechCard).PropertyChanged += Game_AdjustIndicator;break;
                    case CardClass.UrbanBuilding:
                        (card as UrbanBuildingCard).PropertyChanged += Game_AdjustIndicator;break;
                                    
                }
            }
            else if (card.GetType() == typeof(GovCard))
            {
                card.AllowDrop = true;
                card.Drop += GovCard_Drop;
                card.DragOver += GovCard_DragOver;
                     
            }
           
            else if(card.GetType().IsSubclassOf(typeof(ActionCard))){

                (card as ActionCard).goToWaste += ActionCard_goToWaste;

            }
            
            else if (card.GetType().IsSubclassOf(typeof(LeaderCard)))
            {
                (card as LeaderCard).goToWaste += ActionCard_goToWaste;
            }

        }

        #endregion Initialization




        #region Card Mouse event handlers

        void Card_MouseLeave(object sender, MouseEventArgs e)
        {
           
            if (Me.HandList.Contains(sender as Card))
            {
                if (!isDragging)
                {
                    (sender as Card).IsFaceDown = true;
                }
            }
          
        }

        void Card_MouseEnter(object sender, MouseEventArgs e)
        {
           
            if (Me.HandList.Contains(sender as Card)) {
                (sender as Card).IsFaceDown = false;
            }
         
        }

        private void Card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            _parentPanel = (Panel)VisualTreeHelper.GetParent(sender as UIElement);
    
            Type t = _parentPanel.GetType();
            _originalElement = sender as UIElement;


            if (t == typeof(Grid))
            {

                cardColumnPosition = Grid.GetColumn(sender as Card);
                oldPoint = e.GetPosition(_parentPanel);

            }
            else if (t == typeof(Canvas))
            {
                oldPoint = e.GetPosition(_parentPanel);
                _originalLeft = Canvas.GetLeft(sender as UIElement);
                _originalTop = Canvas.GetTop(sender as UIElement);
                // Tracker.Text = "ccccsadfsadf";
            }
            else
            {
                MessageBox.Show("Error in Card_MouseLeftButtonDown");
            }
            isMouseDown = true;

        }


        private void Card_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
              
                if (BasicCheck()&&(sender as Card).IsPlayable == true)
                {
                    if(mainGame.CheckPlayACard(sender as Card)){            
                            DoPlayCard_Local(Me.CardsOnBoard.IndexOf(sender as Card));          
                    }
                }
                else { FeedBackText.Instance.Text = "Not playable this turn"; }
           
        }



        private void Card_MouseMove(object sender, MouseEventArgs e)
        {
            //status_bar.Text = _originalLeft.ToString() +"^^^"+ _originalTop.ToString();
            if (isMouseDown && _originalElement == sender as UIElement)//&& e.LeftButton == MouseButtonState.Pressed)
            {
                DragStarted();

                DataObject data = new DataObject(typeof(Card), sender as Card);
                DragDrop.DoDragDrop(sender as Card, data, DragDropEffects.Move);

                DragFinished();
                isMouseDown = false;
                _parentPanel = null;
            }
            // e.Handled = true;
        }

        private void Card_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {

            isMouseDown = false;
            _parentPanel = null;
        }

   

        void GovCard_DragOver(object sender, DragEventArgs e)
        {

            if (!e.Data.GetDataPresent(typeof(WhiteToken)) && !e.Data.GetDataPresent(typeof(RedToken)))
            {

                //MessageBox.Show(mainGame.Players[mainGame.PlayerIndex].CurrentActionPoint.ToString());

                e.Effects = DragDropEffects.None;

            }

            else { e.Handled = true; }

        }

        void GovCard_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(WhiteToken)))
            {

                Panel _panel = sender as Panel;

                WhiteToken tempR = e.Data.GetData(typeof(WhiteToken)) as WhiteToken;

                if (_parentPanel == _panel)
                {

                    Point CurrentPosition = e.GetPosition(_panel);
                    Canvas.SetTop(tempR, _originalTop + CurrentPosition.Y - oldPoint.Y);
                    Canvas.SetLeft(tempR, _originalLeft + CurrentPosition.X - oldPoint.X);

                }
                else { FeedBackText.Instance.Text = "Invalid Move"; }

            }

            else if (e.Data.GetDataPresent(typeof(RedToken)))
            {

                Panel _panel = sender as Panel;

                RedToken tempR = e.Data.GetData(typeof(RedToken)) as RedToken;

                if (_parentPanel == _panel)
                {

                    Point CurrentPosition = e.GetPosition(_panel);
                    Canvas.SetTop(tempR, _originalTop + CurrentPosition.Y - oldPoint.Y);
                    Canvas.SetLeft(tempR, _originalLeft + CurrentPosition.X - oldPoint.X);

                }
                else { FeedBackText.Instance.Text = "Invalid Move"; }

            }

        }


        void Card_DragOver(object sender, DragEventArgs e)
        {

            if ((!e.Data.GetDataPresent(typeof(Worker)) && !e.Data.GetDataPresent(typeof(Resource))) || Me.HandList.Contains(sender as Card))
            {

                //MessageBox.Show(mainGame.Players[mainGame.PlayerIndex].CurrentActionPoint.ToString());
                //MessageBox.Show("fafafaf");
                e.Effects = DragDropEffects.None;

            }


            else { e.Handled = true; }
        }

        void Card_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(Resource)))
            {

                Panel _panel = sender as Panel;

                Resource tempR = e.Data.GetData(typeof(Resource)) as Resource;

                if (_parentPanel == _panel)
                {

                    Point CurrentPosition = e.GetPosition(_panel);
                    Canvas.SetTop(tempR, _originalTop + CurrentPosition.Y - oldPoint.Y);
                    Canvas.SetLeft(tempR, _originalLeft + CurrentPosition.X - oldPoint.X);

                }
                else { FeedBackText.Instance.Text = "Invalid Move"; }
            }


            if (e.Data.GetDataPresent(typeof(Worker)))
            {
                Panel _panel = sender as Panel;

                Worker tempW = e.Data.GetData(typeof(Worker)) as Worker;

                Point CurrentPosition = e.GetPosition(_panel);

                if (_parentPanel == _panel)
                {
                    //Point CurrentPosition = e.GetPosition(_panel);
                    Canvas.SetTop(tempW, _originalTop + CurrentPosition.Y - oldPoint.Y);
                    Canvas.SetLeft(tempW, _originalLeft + CurrentPosition.X - oldPoint.X);
                }
                else
                {
                    if (BasicCheck())
                    {
                        if (_parentPanel.GetType().IsSubclassOf(typeof(UnitCard)) && _panel.GetType().IsSubclassOf(typeof(UnitCard)))
                        {
                            if (mainGame.UpGradeAUnit((_parentPanel as UnitCard), (_panel as UnitCard)))
                            {

                                _parentPanel.Children.Remove(tempW);
                                _panel.Children.Add(tempW);

                                //Point CurrentPosition = e.GetPosition(_panel);
                                Canvas.SetTop(tempW, CurrentPosition.Y - TokenDiameter / 2);
                                Canvas.SetLeft(tempW, CurrentPosition.X - TokenDiameter / 2);
                            }
                        }

                        else if (_parentPanel.GetType() == typeof(Canvas))
                        {
                            if (_panel.GetType().IsSubclassOf(typeof(UnitCard)))
                            {
                                if (mainGame.CheckBuildUnit(_panel as UnitCard))
                                {            
                                        DoBuildUnit_Local(Me.CardsOnBoard.IndexOf(_panel as Card));                                                 
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: Invalid Parent\n\nSource:CardDrop\n" + _parentPanel.GetType().ToString());
                        }
                    }            
                }
            }
        }

        #endregion




        #region STG Start a game

        void GameStart(int playerNum,List<int> random) {

            grid_StartMenu.Visibility = Visibility.Collapsed;

            Restart();

            if (GameMode > 0)
            {
                mainGame.NumberOfPlayers = playerNum;
                mainGame.MyIndex = index;
                TestPanel.Visibility = Visibility.Collapsed;
            }else{

            mainGame.NumberOfPlayers = 4;
            mainGame.MyIndex = Int32.Parse(CB_PlayerNum.Text);
            
            }

            //Check
            Tbox1.Text = string.Format("MyIndex: {0}\nNumOfPlayers:{1}", index, playerNum);

    
            switch (mainGame.MyIndex) {
                case 0:
                     HideCanvas_P1.Visibility = Visibility.Collapsed;break;
                case 1:
                     HideCanvas_P2.Visibility = Visibility.Collapsed;break;
                case 2:
                     HideCanvas_P3.Visibility = Visibility.Collapsed;break;
                case 3:
                     HideCanvas_P4.Visibility = Visibility.Collapsed;break;
                default:
                    throw new NotImplementedException();
            }

           

            mainGame.InitializeGame(pNameList,random);
            InitializeGameVisual();
            AssignControlsToCards();

           // mainGame.InitilaizeEachPlayer();
            StartMyTurn();
        }


        void AssignControlsToCards() {

            for (int i = 0; i < mainGame.GameBoard.CardList.Count; i++)
            {
                Card tempC = mainGame.GameBoard.CardList[i];
                AssignControlsToCard(tempC);
            }
         
            for (int i = 0; i < mainGame.CloneList.Count; i++)
            {
                Card tempC = mainGame.CloneList[i];
                
                AssignControlsToCard(tempC);
            } 
        }
        
        void GameStart_Button_Click_1(object sender, RoutedEventArgs e)
        {
            _backend._GameStart(GetRandomCardOrder());
        }

        void Restart()
        {

            if (mainGame.GameBoard != null)
            {
                mainGame.GameBoard.CardList.Clear();
                mainGame.GameBoard.IsLastRound = false;
            }
            
            grid_Board.Children.Clear();
            mainGame.Players.Clear();
            DataGrid_Score.Items.Clear(); 
            stackPanel_Waste.Children.Clear();

            canvas_WorkerPool.Children.Clear();
            canvasTwo_WorkerPool.Children.Clear();
            canvasThree_WorkerPool.Children.Clear();
            canvasFour_WorkerPool.Children.Clear();

            gridOne_ResourceBank.Children.Clear() ;
            gridOne_WorkerBank.Children.Clear();
            gridTwo_ResourceBank.Children.Clear();
            gridTwo_WorkerBank.Children.Clear();       
            gridThree_ResourceBank.Children.Clear();
            gridThree_WorkerBank.Children.Clear();
            gridFour_ResourceBank.Children.Clear();
            gridFour_WorkerBank.Children.Clear();
     
            canvas_PlayerOne.Children.Clear();    
            canvas_PlayerTwo.Children.Clear(); 
            canvas_PlayerThree.Children.Clear(); 
            canvas_PlayerFour.Children.Clear();

            mainGame.IndexOfCurrentP = 0;
            mainGame.IsLastTurn = false;
            GameManager.gameRound = 1;

        }
        #endregion


        bool HasInvalidAction() {
            bool result = false;
            foreach (ActionInfo a in actionRegister) {
                if (a.IsRed()) result = true;
            }
            return result;
        }

        

        private void TurnFinishClick(object sender, RoutedEventArgs e)
        {
            if (!HasInvalidAction())
            {

                if (btnInStateFMT)
                {
                    btn_Finish.IsEnabled = false;
                    _backend._ReverseData(mainGame.MyIndex);
                    ConfirmAllActions();
                    _backend._FinishATurn();
                }
                else
                {

                    if (!mainGame.IsLastTurn)
                    {
                        btn_Finish.Content = "Confirm and Finish my Turn";
                        _backend._UpdatePlayerIndex(mainGame.MyIndex + 1, cardsAlreadyRemoved, SetCardsRowForNextTurn());
                        StartMyTurn();
                        btnInStateFMT = true;
                    }
                    else
                    {
                        MessageBox.Show(mainGame.GetWinner());
                    }
                }
            }
            else {
                MessageBox.Show("Remove invalid action before you confirm your turn");
            }
          
        }

        void StartMyTurn() {

            moveAbleCards = Me.CardsOnBoard.Count;
            cardsAlreadyRemoved = 0;
            
            //currentState must come first
            currentState = -1;
            actionRegister.Clear();

           // actionRegister.Add(new ActionInfo("Initial State"));

            CardsToMoveList.Clear();
            
            mainGame.StartMyTurn();

        }
       

        
      
      
        /// <summary>
        /// Worker Mouse Handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

   
        private void Token_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
              
                if (sender.GetType().IsSubclassOf(typeof(BlueYellowToken))&&!(sender as BlueYellowToken).IsPlayable)
                {
                   
                }
                else {
                    _parentPanel = (Panel)VisualTreeHelper.GetParent(sender as UIElement);
               
                    _originalElement = sender as UIElement;

                    oldPoint = e.GetPosition(_parentPanel);
                    _originalLeft = Canvas.GetLeft(sender as UIElement);
                    _originalTop = Canvas.GetTop(sender as UIElement);
                    
                    isMouseDown = true;
                             
                }
               e.Handled = true;
        } 
        
        
        private void Token_MouseMove(object sender, MouseEventArgs e)
        {
            //Tbox2.Text = "bb\n";
            //tb_test.Text = isMouseDown.ToString();
            if (isMouseDown&&_originalElement == sender as UIElement)//&& e.LeftButton == MouseButtonState.Pressed)
            {
                DragStarted();
                Tbox2.Text = "WM\n" + _originalElement.ToString();
                //Tbox2.Text = "asdfasg\n";
                if (sender.GetType() == typeof(Worker))
                {
                    DataObject data = new DataObject(typeof(Worker), sender as UIElement);
                    DragDrop.DoDragDrop(sender as Worker, data, DragDropEffects.Move);
                }
                else if (sender.GetType() == typeof(Resource))
                {
                    DataObject data = new DataObject(typeof(Resource), sender as UIElement);
                    DragDrop.DoDragDrop(sender as Resource, data, DragDropEffects.Move);
                }
                else if (sender.GetType() == typeof(WhiteToken)) {
                    DataObject data = new DataObject(typeof(WhiteToken), sender as UIElement);
                    DragDrop.DoDragDrop(sender as WhiteToken, data, DragDropEffects.Move);
                }

                else if (sender.GetType() == typeof(RedToken))
                {
                    DataObject data = new DataObject(typeof(RedToken), sender as UIElement);
                    DragDrop.DoDragDrop(sender as RedToken, data, DragDropEffects.Move);
                }

                //Tbox5.Text = "lolololo_worker";
                DragFinished();
                isMouseDown = false;
                _parentPanel = null;     
                e.Handled = true;
            
            }
           
        }

        private void Token_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            _parentPanel = null;
        }




        #region Network Event _Handlers

       
        void OnMoveACardHandler(int player, int card, double x, double y) {
              
            DoMoveACard(mainGame.Players[player].CardsOnBoard[card], x, y);
          
        }

        void DoMoveACard(Card tempCard,double x,double y) {
            Canvas.SetLeft(tempCard, x);
            Canvas.SetTop(tempCard, y);
        }

        void OnPickCardHandler(int player, string name, double x, double y)
        {


            if (mainGame.IsNotFromMe(player))
            {
                Card tempCard = new Card();
                int card = 0, cardPos = 0;
                if (!mainGame.FindTheCardFromTheRow(name, ref tempCard, ref card))
                    MessageBox.Show("Error didn't find the original card from " + mainGame.MyIndex);
                cardPos = Grid.GetColumn(tempCard) / 2;
                DoPickCard(player, card, cardPos, x, y, false);
            }
        }
          

        void OnConsumeCAPHandler(int player, int num)
        {
            mainGame.Players[player].ConsumeCAP(num);
        }


        void OnUpdatePlayerHandler(int player,int round,int removedCards,List<string> cardsOnRow){

            mainGame.IndexOfCurrentP = player;
            HighlightTab(player);

            GameManager.gameRound = round;
            
            if (GameManager.gameRound == 1 && mainGame.IsMyTurn()) {
                cardsAlreadyRemoved += removedCards;
            }



            if (btn_Finish.IsEnabled = mainGame.IsMyTurn())
            {
                FeedBackText.Instance.Text = "Your turn begins";
            }
            else {
                FeedBackText.Instance.Text = mainGame.Players[mainGame.IndexOfCurrentP].MyName +"'s turn begins";
            }

            CardsOnTheRow = cardsOnRow;
                           
            mainGame.StartATurn(removedCards);        
        }

        void HighlightTab(int number) {

            for (int i = 0; i < mainGame.NumberOfPlayers;i++ )
            {
                tabItem_Players[i].FontWeight = FontWeights.Normal;
            }

            tabItem_Players[number].FontWeight = FontWeights.Bold;
        }

        void OnFinishTurnHandler() {

            if(!mainGame.IsMyTurn())
            DoAllActions(PackCount);

            Lbox_Actions.SelectedIndex = currentState;
            mainGame.FinishATurn();
        }

        void OnIncreasePopulationHandler(int player){

            if (mainGame.IsNotFromMe(player))
            DoIncreasePopulation(player,false);
     
        }

        void OnBuildUnitHandler(int player,int card) {

            if (mainGame.IsNotFromMe(player))
            DoBuildUnit(player,card,false);
           
        }

        void OnPlayCardHandler(int player,int card) {
           
            if(mainGame.IsNotFromMe(player))
            DoPlayCard(player, card,false);

        }

        void OnDestroyUnitHandler(int player,int card) {

            if (mainGame.IsNotFromMe(player)) 
            DoDestroyUnit(player, card, false);
                        
        }

        void OnReverseDataHandler(int player){
            if (!mainGame.IsMyTurn())
                ReverseDataAndActions(currentState+1);
        }

        #endregion






        /// <summary>
        /// DragOver&Drop handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        /// 
        
 
        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
                           
            if (!e.Data.GetDataPresent(typeof(Card)))
            {   
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;            
            }
            
        }

       
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            IDataObject data = new DataObject();
            data = e.Data;
            
            if (data.GetDataPresent(typeof(Card)))
            {
                Canvas _panel = sender as Canvas;

                Card tempCard = data.GetData(typeof(Card)) as Card;

                Type parentType = _parentPanel.GetType();

                Point currentPosition = e.GetPosition(_panel);  
               
                    if (parentType == typeof(Canvas))
                    {
                        int index = Me.CardsOnBoard.IndexOf(tempCard);
                                      
                        DoMoveACard(tempCard, _originalLeft + currentPosition.X - oldPoint.X, _originalTop + currentPosition.Y - oldPoint.Y);
                        CardsToMoveList.Add(new MovePack(tempCard, _originalLeft + currentPosition.X - oldPoint.X, _originalTop + currentPosition.Y - oldPoint.Y));

                        /*if(index < moveAbleCards)
                          _backend._MoveCard(mainGame.MyIndex, mainGame.Players[mainGame.MyIndex].CardsOnBoard.IndexOf(tempCard),
                               _originalLeft + currentPosition.X - oldPoint.X, _originalTop + currentPosition.Y - oldPoint.Y);*/
                    }
                    else if (parentType == typeof(Grid))
                    {
                        if (BasicCheck())
                        {
                            if (mainGame.CheckPickUpACard(tempCard, cardColumnPosition/2))
                            {                                                                                  
                                DoPickCard_Local(mainGame.GameBoard.CardList.IndexOf(tempCard), cardColumnPosition / 2,
                                  currentPosition.X - CardWidth / 2, currentPosition.Y - CardHeight / 2);                                        
                            }
                        }                                           
                    } else { MessageBox.Show("Error:Wrong _parent___CanvasDrop"); }           
            }
        }
       

        private void WorkerPool_PreviewDragOver(object sender, DragEventArgs e) {

            if (!e.Data.GetDataPresent(typeof(Worker)))
            {
                
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void WorkerPool_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Worker)))
            {
                Canvas _panel = sender as Canvas;
             
                Worker worker = e.Data.GetData(typeof(Worker)) as Worker;

                Type type = _parentPanel.GetType();

                Point currentPosition = e.GetPosition(_panel);
  
                if (_parentPanel == _panel)
                {             
                    Canvas.SetTop(worker, _originalTop + currentPosition.Y - oldPoint.Y);
                    Canvas.SetLeft(worker, _originalLeft + currentPosition.X - oldPoint.X);
                }
                else if (BasicCheck())
                {
                    if (type == typeof(Grid)) //From worker bank
                    {
                        if (mainGame.CheckIncreasePopulation())
                        {                            
                                DoIncreasePopulation_Local();                      
                        }
                    }
                    else if (type.IsSubclassOf(typeof(UnitCard))) //From a unit card
                    {
                        if (mainGame.CheckDestroy_A_Unit(_parentPanel as UnitCard))
                        {     
                                DoDestroyUnit_Local(Me.CardsOnBoard.IndexOf(_parentPanel as Card));                 
                        }
                    }
                    else { MessageBox.Show("Error:Cannot find _parent"); }
                }
            }
        }





        //Card field
        int cardColumnPosition;
        bool isMouseDown;  
        bool isDragging = false;
        Panel _parentPanel;
        Point oldPoint = new Point();
        private double _originalLeft;
        private double _originalTop;
        SimpleAdorner _overlayElement;
        UIElement _originalElement;
     

        private void test_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

           
            Tbox1.Text = (sender as Canvas).Name;
            Tbox2.Text = Me.MyLeader == null ? "Yes" : "NO";
            Tbox3.Text = "";
            Tbox4.Text = "MyIndex: "+ mainGame.MyIndex + 
                "\nCurrentPlayer: " + mainGame.IndexOfCurrentP + "\nRound: " + GameManager.gameRound;
            Tbox5.Text = "";
            FeedBackText.Instance.Text = "";
          
        }


        #region MouseWheel handlers

        const int MaxWheelHeight = 550;
        const int MinWheelHeight = 225;
        const int WheelRate = 20;

        private void Card_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!(sender as Card).IsFaceDown && e.Delta < 0)
            {
                grid_Zoom.Visibility = Visibility.Visible;
                image_Zoom.Source = (sender as Card).MyImage.ImageSource;
            }
        }


        private void grid_Zoom_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (image_Zoom.Height < MaxWheelHeight)
                {
                    image_Zoom.Height += WheelRate;
                    image_Zoom.Width += WheelRate;
                }
            }
            if (e.Delta > 0) {
                if (image_Zoom.Height < MinWheelHeight)(sender as Panel).Visibility = Visibility.Collapsed; 
            else {
                image_Zoom.Height -= WheelRate;
                image_Zoom.Width -= WheelRate;
            } 
            }
        }

        #endregion





        private void DragStarted() {
               
            _overlayElement = new SimpleAdorner(_originalElement);
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_originalElement);
            layer.Add(_overlayElement);            
            isDragging = true;
        }

        private void DragFinished() {
                   
                AdornerLayer.GetAdornerLayer(_overlayElement.AdornedElement).Remove(_overlayElement);
                _overlayElement = null;
                _originalElement = null;
                isDragging = false;
            
        }

      
        
        private void GameGrid_PreviewDragOver(object sender, DragEventArgs e)
        {
      
            //Tbox2.Text = "Grid"+b.ToString();
           // status_Bar.Text ="GameGrid_PreviewDragOver"+b.ToString();
            if (isDragging)
            {
                if (_parentPanel != null)
                {
                    Point CurrentPosition = e.GetPosition(_parentPanel as Panel);
                    _overlayElement.LeftOffset = CurrentPosition.X - oldPoint.X;
                    _overlayElement.TopOffset = CurrentPosition.Y - oldPoint.Y;
                }
            }
            
        }

      

        void ActionPoints_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Player currentP = sender as Player;
            if(e.PropertyName == "CivilAP"||e.PropertyName == "MilitaryAP"){
            currentP.Gov.Children.Clear();
 
                for (int i = 0, tempTopOffset = 0, j = 0; i < currentP.AP_Civil; i++, j++)
                {
                    WhiteToken tempWT = new WhiteToken(Token_MouseLeftButtonDown, Token_MouseMove, Token_MouseLeftButtonUp);

                    if (i > 0 && i % 4 == 0)
                    {
                        tempTopOffset += 5;
                        j = 0;
                    };

                    Canvas.SetTop(tempWT, CardTopOffset + tempTopOffset);
                    Canvas.SetLeft(tempWT, CardLeftOffset + j * UnitCardTokenSpacing);
                    currentP.Gov.Children.Add(tempWT);
                }
            

          
                for (int i = 0, tempTopOffset = 25, j = 0; i < currentP.AP_Military; i++, j++)
                {
                    RedToken temp = new RedToken(Token_MouseLeftButtonDown, Token_MouseMove, Token_MouseLeftButtonUp);


                    if (i > 0 && i % 4 == 0)
                    {
                        tempTopOffset += 5;
                        j = 0;
                    };

                    Canvas.SetTop(temp, CardTopOffset + tempTopOffset);
                    Canvas.SetLeft(temp, CardLeftOffset + j * UnitCardTokenSpacing);
                    currentP.Gov.Children.Add(temp);
                }
            }
            
        }

        void rBank_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            int player = (sender as ResourceBank).myOwner;
            Grid currentRB = gridList_ResourceBank[player];
            Player currentP = mainGame.Players[player];
            currentRB.Children.Clear();

            currentP.rBank.rCol = 1;
            currentP.rBank.rRow = 3;

            for (int i = 0; i < currentP.rBank.TokenInBank; i++)
            {

                Resource tempR = new Resource(Token_MouseLeftButtonDown,Token_MouseMove,Token_MouseLeftButtonUp,false);

                Grid.SetColumn(tempR, currentP.rBank.rCol);
                Grid.SetRow(tempR, currentP.rBank.rRow);            

                currentRB.Children.Add(tempR);

        

                if (currentP.rBank.rRow == 1)
                {
                    currentP.rBank.rCol += 2;

                }
                currentP.rBank.rRow = 4 - currentP.rBank.rRow;
            }         
        }

        void Game_AdjustIndicator(object sender, PropertyChangedEventArgs e)
        {
        
            if (e.PropertyName == "AddUrbanBuilding")
            {
                mainGame.Players[mainGame.IndexOfCurrentP].Science += (sender as UrbanBuildingCard).Science;
                mainGame.Players[mainGame.IndexOfCurrentP].Culture += (sender as UrbanBuildingCard).Culture;
                mainGame.Players[mainGame.IndexOfCurrentP].Happiness += (sender as UrbanBuildingCard).Happiness;
            }
            else if (e.PropertyName == "DestroyUrbanBuilding")
            {
                mainGame.Players[mainGame.IndexOfCurrentP].Science -= (sender as UrbanBuildingCard).Science;
                mainGame.Players[mainGame.IndexOfCurrentP].Culture -= (sender as UrbanBuildingCard).Culture;
                mainGame.Players[mainGame.IndexOfCurrentP].Happiness -= (sender as UrbanBuildingCard).Happiness;
            }
            else if(e.PropertyName == "AddMilitaryTech"){
                mainGame.Players[mainGame.IndexOfCurrentP].Strength += (sender as MilitaryTechCard).Strength;
            }
            else if(e.PropertyName == "DestroyMilitaryTech"){
                mainGame.Players[mainGame.IndexOfCurrentP].Strength -= (sender as MilitaryTechCard).Strength;
            }

        }

        void resource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
    
            if (e.PropertyName == "FoodToken")
            {               
                //Clear
                for (int i = 0; i < (sender as AgriCard).Children.Count; )
                {
                    if ((sender as AgriCard).Children[i].GetType() == typeof(Resource))
                    {
                        (sender as AgriCard).Children.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                //Add
                for (int i = 0,j = 0,tempTopOffset = 0; i < (sender as AgriCard).Token; i++,j++)
                {

                    Resource tempR = new Resource(Token_MouseLeftButtonDown, Token_MouseMove, Token_MouseLeftButtonUp);

                    if (i > 0 && i % 4 == 0) { 
                        tempTopOffset += 5;
                        j = 0;
                    };

                    Canvas.SetTop(tempR, MainWindow.CardTopOffset * 2+tempTopOffset);
                    Canvas.SetLeft(tempR, MainWindow.CardLeftOffset + j * MainWindow.UnitCardTokenSpacing);
                    (sender as AgriCard).Children.Add(tempR);
                }
            }

            else if (e.PropertyName == "OreToken") {
                //Clear
                for (int i = 0; i < (sender as MineCard).Children.Count; )
                {
                    if ((sender as MineCard).Children[i].GetType() == typeof(Resource))
                    {
                        (sender as MineCard).Children.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                //Add
                for (int i = 0, j = 0, tempTopOffset = 0; i < (sender as MineCard).Token; i++, j++)
                {

                    Resource tempR = new Resource(Token_MouseLeftButtonDown, Token_MouseMove, Token_MouseLeftButtonUp);

                    if (i > 0 && i % 4 == 0)
                    {
                        tempTopOffset += 5;
                        j = 0;
                    };

                    Canvas.SetTop(tempR, MainWindow.CardTopOffset * 2 + tempTopOffset);
                    Canvas.SetLeft(tempR, MainWindow.CardLeftOffset + j * MainWindow.UnitCardTokenSpacing);
                    (sender as MineCard).Children.Add(tempR);
                }                           
            }
        }


        void GameBoard_RefreshBoard(object sender, EventArgs e)
        {
            //Tbox5.Text += "\nrefresh";

            grid_Board.Children.Clear();
            Board tempB = mainGame.GameBoard;

            int count = CardsOnTheRow.Count > 0?CardsOnTheRow.Count:13;

            for (int i = 0,j = 0; i < count; i++) {
                Card tempC = tempB.CardList[j];
                if (GameManager.gameRound == 1||tempC.MyName == CardsOnTheRow[i])
                {
                    Grid.SetColumn(tempC, i * 2);
                    Grid.SetRow(tempC, 1);
                    grid_Board.Children.Add(tempC);
                    j++;
                } 
            }
        }

        

        

        void ActionCard_goToWaste(object sender, int player)
        {
            int topOffset = -30;
            Card tempCard = sender as Card;

            if (stackPanel_Waste.Children.Count != 0)
            {
                tempCard.Margin = new Thickness(0, topOffset, 0, 0);
            }
            canvasList_Player[player].Children.Remove(tempCard);

            tempCard.MouseLeftButtonDown -= Card_MouseLeftButtonDown;       

            stackPanel_Waste.Children.Add(tempCard);          
        }

        #region Button tests

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int a = 1;
            Me.Consume(a, DataType.Food);
            Me.Consume(a,DataType.Ore);
            Me.AP_Civil--;
           
               
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Me.AP_Civil++;
            mainGame.Players[index].SciencePoint += 10;
         
         
 
        }

        private void GameGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DoAllActions(PackCount);
            
        }

        #endregion


        #region Network

        private ChatBackend.ChatBackend _backend;

        void OnJoin(string member, int num)
        {  
            if (String.IsNullOrEmpty(member))
                member = "Player " + (num-1).ToString();      
            pNameList.Add(member);

            textBoxChatPane.Text += String.Format("[{0} has joined;Current NumOfPlayers: {1}]" + Environment.NewLine, member, num);
            if (num > 1) btn_StartGame.IsEnabled = true;
            Tbox3.Text += pNameList.Count.ToString();
        }

        void DisplayMessage(ChatBackend.CompositeType composite)
        {
            string username = composite.Username == null ? "" : composite.Username;
            string message = composite.Message == null ? "" : composite.Message;
            textBoxChatPane.Text += (username + ": " + message + Environment.NewLine);
        }

       
      
        private void Join_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(textBoxName.Text)||true)
            {
                textBoxChatPane.Text += "\nMy index is: " + _backend.Back_PlayerIndex + "\n";
                index = _backend.Back_PlayerIndex;

                _backend._Join(textBoxName.Text);
                btn_Join.IsEnabled = false;
            }
            else {
                MessageBox.Show("Input your name first");
            }
        }
        
        
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            btn_Connect.IsEnabled = false;
            _backend = new ChatBackend.ChatBackend(this.DisplayMessage);
            _backend.OnJoin += OnJoin;
            _backend.OnStartGame += GameStart;
            _backend.StartService();
            btn_Join.IsEnabled = true;
        }

        #endregion network

        #region others

        private List<int> GetRandomCardOrder() {

            int numOfAgeA = 13;
            int numOfAgeI = 23;


            List<int> a = new List<int>();
            List<int> b = new List<int>();

            for(int i =0;i<numOfAgeA;i++){
                a.Add(i);
            }
            GenericShuffle.Shuffle<int>(a);

            for (int i = numOfAgeA; i < numOfAgeI + numOfAgeA; i++)
            {
                b.Add(i);
            }
            GenericShuffle.Shuffle<int>(b);

            for (int i = 0; i < b.Count(); i++) {
                a.Add(b[i]);
            }
            return a;
        }


        bool BasicCheck() {         
            if(!btnInStateFMT){
                MessageBox.Show("You have finished your turn");
                return false;
            }
           
            return true;
        }

        private int GetPlayerIndex() {
            return GameManager.IsPlayingNetwork() ? mainGame.IndexOfCurrentP : mainGame.MyIndex;
        }


        #endregion

        private void btn_StartGame_Click(object sender, RoutedEventArgs e)
        {
            _backend._GameStart(GetRandomCardOrder());
            GameMode = 1;
        }

        private void btn_TestStart_Click(object sender, RoutedEventArgs e)
        {
            _backend._GameStart(GetRandomCardOrder());
            GameWindow.WindowState = WindowState.Maximized;
            GameMode = -1;
          
            
        }

        #region Actions

        private void DoPickCard(int player, int card, int cardPosition, double x, double y,bool reversible) {

            Card tempCard = mainGame.GameBoard.CardList[card];
   
            Tbox4.Text = card + " " + cardPosition;

            if(grid_Board.Children.Contains(tempCard))
            grid_Board.Children.Remove(tempCard);

            tempCard.Height = CardHeight;
            tempCard.Width = CardWidth;

            canvasList_Player[player].Children.Add(tempCard);

            mainGame.DoPickCard(player,card, cardPosition,x,y,reversible);

            DoMoveACard(tempCard, x, y);

            if (mainGame.IsNotFromMe(player)) {
                tempCard.IsFaceDown = true;
            }
        }

        private void DoPickCard_Local(int card, int cardPosition, double x, double y,bool reversible = true)
        {          
            DoPickCard(index, card, cardPosition, x, y,reversible);
        }

        private void DoIncreasePopulation(int player,bool reversible) {
                
            Grid currentWB = gridList_WorkerBank[player];
            Canvas currentWP = canvasList_WorkerPool[player]; 
            
            mainGame.DoIncreasePopulation(player,reversible);
           
            Worker worker = currentWB.Children[currentWB.Children.Count - 1] as Worker;

            //Remove
            currentWB.Children.Remove(worker);

            //Add
            int count = currentWP.Children.Count;
            int tempY = count / 4 * WorkerPoolTokenSpacing;
            int tempX = count % 4 * WorkerPoolTokenSpacing;

            Canvas.SetLeft(worker, InitialWorkerPosX + tempX);
            Canvas.SetTop(worker, InitialWorkerPosY + tempY);

            currentWP.Children.Add(worker);
           
        }

        private void DoIncreasePopulation_Local(bool reversible = true) {

            DoIncreasePopulation(index,reversible);
        }

        private void DoBuildUnit(int player,int card,bool reversible) {

            Canvas currentWP = canvasList_WorkerPool[player];
            Worker worker = currentWP.Children[currentWP.Children.Count - 1] as Worker;
            UnitCard unitCard = mainGame.Players[player].CardsOnBoard[card] as UnitCard;

            //Remove
            currentWP.Children.Remove(worker);

            //Add
            int count = unitCard.Unit;
            int tempX = count % 5 * UnitCardTokenSpacing;
            int tempY = count / 5 * UnitCardTokenSpacing;

            Canvas.SetLeft(worker, CardLeftOffset + tempX);
            Canvas.SetTop(worker, CardTopOffset + tempY);

            (unitCard as Canvas).Children.Add(worker);

            mainGame.DoBuildUnit(player,unitCard,reversible);
        
        }

        private void DoBuildUnit_Local(int card,bool reversible = true)
        {
  
            DoBuildUnit(mainGame.MyIndex, card, reversible);
        }

        public void DoPlayCard(int player,int card,bool reversible) {

            Card tempCard = mainGame.Players[player].CardsOnBoard[card];
            tempCard.IsFaceDown = false;
   
            mainGame.DoPlay_A_Card(player,tempCard,reversible);

        }

        public void DoPlayCard_Local(int card,bool reversible = true) {
  
            DoPlayCard(mainGame.MyIndex, card,reversible);
        }

        public void DoDestroyUnit(int player, int card, bool reversible) {

            Player currentP = mainGame.Players[player];
            UnitCard unitCard = currentP.CardsOnBoard[card] as UnitCard;            
            //Canvas cardCanvas = currentP.CardsOnBoard[card] as Canvas;
            Canvas currentWP = canvasList_WorkerPool[player];
            Worker worker = new Worker();

            mainGame.DoDestroy_A_Unit(player,unitCard,reversible);

            
            //Find last worker
            for (int i = unitCard.Children.Count - 1; i > -1; i--)
            {
                if (unitCard.Children[i].GetType() == typeof(Worker))
                {
                    worker = unitCard.Children[i] as Worker;
                    unitCard.Children.RemoveAt(i);        
                    break;
                }
            }
            //Add
            int count = currentWP.Children.Count;
            int tempY = count / 4 * WorkerPoolTokenSpacing;
            int tempX = count % 4 * WorkerPoolTokenSpacing;

            Canvas.SetLeft(worker, InitialWorkerPosX + tempX);
            Canvas.SetTop(worker, InitialWorkerPosY + tempY);

            currentWP.Children.Add(worker);    
        }

        void DoDestroyUnit_Local(int card,bool reversible = true)
        {
            DoDestroyUnit(mainGame.MyIndex,card,reversible);
        }



        #endregion

        #region _ReverseAction

        void Reverse_IncreasePopulation(IncreasePopuPack pack) {

     
                mainGame.UndoIncreasePopulation(pack);

                Worker tempW = myWP.Children[myWP.Children.Count - 1] as Worker;

                //Remove
                myWP.Children.Remove(tempW);

                //Add
                Grid.SetColumn(tempW, pack.col);
                Grid.SetRow(tempW, pack.row);

                myWB.Children.Add(tempW);
                             
        }

        void Reverse_BuildUnit(BuildUnitPack pack) {
            
                mainGame.UndoBuildUnit(pack);

                Canvas tempCanvas = pack.card as Canvas;
                Worker tempW = new Worker();

                //Tbox5.Text +="\nNumOfChildren" + tempCanvas.Children.Count;

                for(int i = tempCanvas.Children.Count-1;i>-1;i--){
                    if (tempCanvas.Children[i].GetType() == typeof(Worker)) {
                        tempW = tempCanvas.Children[i] as Worker;
                        tempCanvas.Children.RemoveAt(i);
                        //Tbox3.Text += "\nRemovedPosition"+ i;
                        break;
                    }
                }

                //Add
                int count = myWP.Children.Count;
                int tempY = count / 4 * WorkerPoolTokenSpacing;
                int tempX = count % 4 * WorkerPoolTokenSpacing;

                Canvas.SetLeft(tempW, InitialWorkerPosX + tempX);
                Canvas.SetTop(tempW, InitialWorkerPosY + tempY);

                myWP.Children.Add(tempW);              
        }


        void Reverse_PickUpCard(PickCardPack pack) {


            Card tempCard = pack.pickedCard;
             
                myPC.Children.Remove(tempCard);
                                       
                mainGame.UndoPickCard(pack);

                tempCard.Height = Double.NaN;
                tempCard.Width = Double.NaN;
                tempCard.IsFaceDown = false;

                grid_Board.Children.Add(tempCard);
                Grid.SetColumn(tempCard, FindActualPosition(pack.name) * 2);
                Grid.SetRow(tempCard, 1);
                       
        }

        int FindActualPosition(string name) {
            int cardPos = 0;
            for (int i = 0; i < CardsOnTheRow.Count; i++) {
                if (CardsOnTheRow[i] == name) cardPos = i;
            }
            return cardPos;
        }

        void Reverse_PlayCard(PlayCardPack pack) {



            if (pack.cardClass == CardClass.Action) {
                stackPanel_Waste.Children.Remove(pack.card);
                myPC.Children.Add(pack.card);
                pack.card.MouseLeftButtonDown += Card_MouseLeftButtonDown;
            }

            if (pack.removedLeaderCard != null) {
                stackPanel_Waste.Children.Remove(pack.removedLeaderCard);
                myPC.Children.Add(pack.removedLeaderCard);
                pack.removedLeaderCard.MouseLeftButtonDown += Card_MouseLeftButtonDown;
            }

            mainGame.UndoPlayCard(pack);
                
        }

        void Reverse_DestroyUnit(DestroyUnitPack pack) {

            Worker tempW = myWP.Children[myWP.Children.Count - 1] as Worker;
      
            //Remove
            myWP.Children.Remove(tempW);
            //Add
            int count = pack.card.Unit;
            int tempX = count % 5 * UnitCardTokenSpacing;
            int tempY = count / 5 * UnitCardTokenSpacing;

            Canvas.SetLeft(tempW, CardLeftOffset + tempX);
            Canvas.SetTop(tempW, CardTopOffset + tempY);

            (pack.card as Canvas).Children.Add(tempW);

            mainGame.UndoDestoryUnit(pack.card);
        
        }

 
        #endregion

       

        private void ShowInfo(object sender, RoutedEventArgs e)
        {
           // ActionRegister.Document.Blocks.Clear();
           // mainGame.ShowPlayerInfo(0);

            Tbox1.Text = "SelectedIndex:" + Lbox_Actions.SelectedIndex + "\nCurrentState:" + currentState;
            Tbox2.Clear();
            foreach (ReversePack pack in mainGame.reversePackList) {
                Tbox2.Text += pack.action.ToString() + "\n";
            }

            Tbox5.Clear();

            List<string> cards = SetCardsRowForNextTurn();

            foreach (string s in cards) {
                Tbox5.Text += s + "\n";
            }

            Tbox5.Text += "========\n\n";

            foreach (string s in CardsOnTheRow)
            {
                Tbox5.Text += s + "\n";
            }
           
           

        }

     

        void ReverseAllActions(int count) {

           

            for (int i = count - 1; i > -1; i--)
            {
                ReversePack pack = mainGame.reversePackList[i];
                switch (pack.action)
                {

                    case GameAction.IncreasePopu: Reverse_IncreasePopulation(pack as IncreasePopuPack); break;
                    case GameAction.PickUpCard: Reverse_PickUpCard(pack as PickCardPack); break;
                    case GameAction.BuildUnit: Reverse_BuildUnit(pack as BuildUnitPack); break;
                    case GameAction.PlayCard: Reverse_PlayCard(pack as PlayCardPack); break;
                    case GameAction.DestroyUnit: Reverse_DestroyUnit(pack as DestroyUnitPack); break;
                    case GameAction.DoNothing: break;
                    default: throw new NotImplementedException();
                }
            }
                                         
        }

        void ReportFailure(int index) {

            //currentState = index - 1;
            Tbox1.Text = "error in:" + index;
            actionRegister[index].TurnRed();
            for (int i = index+1; i < Lbox_Actions.Items.Count; i++) {
                   actionRegister[i].TurnGray();
            }
    
        }

        void ReportSuccess(int index)
        {
            actionRegister[index].TurnBlack();
        }

    
        private void btn_ConfirmAction_Click(object sender, RoutedEventArgs e)
        {
            ConfirmAllActions();                                 
        }

        private void ConfirmAllActions() {

            //Actions
            for (int index = 0; index < mainGame.reversePackList.Count;index++ )
            {
                ReversePack pack = mainGame.reversePackList[index];

                switch (pack.action)
                {
                    case GameAction.IncreasePopu:
                        _backend._IncreasePopulation(mainGame.MyIndex);
                        break;

                    case GameAction.PickUpCard:
                        PickCardPack ppack = pack as PickCardPack;

                        int cardPos = Grid.GetColumn(ppack.pickedCard) / 2;
                        if (cardPos < mainGame.GameBoard.Num_CardsToBeRemoved)
                            cardsAlreadyRemoved++;

                        _backend._PickCard(mainGame.MyIndex, ppack.name, ppack.x, ppack.y);
                        break;

                    case GameAction.BuildUnit:
                        BuildUnitPack bpack = pack as BuildUnitPack;
                        _backend._BuildUnit(mainGame.MyIndex, Me.CardsOnBoard.IndexOf(bpack.card));
                        break;

                    case GameAction.PlayCard:
                        PlayCardPack pcpack = pack as PlayCardPack;
                        _backend._PlayACard(mainGame.MyIndex, Me.CardsOnBoard.IndexOf(pcpack.card));
                        break;

                    case GameAction.DestroyUnit:
                        DestroyUnitPack dpack = pack as DestroyUnitPack;
                        _backend._DestroyUnit(mainGame.MyIndex, Me.CardsOnBoard.IndexOf(dpack.card));
                        break;

                    case GameAction.DoNothing:
                        break;

                    default: throw new NotImplementedException();
                }
            }

            //Moves
            for (int i = 0; i < CardsToMoveList.Count; i++) {

                Card tempC = CardsToMoveList[i].card;
                if (Me.CardsOnBoard.Contains(tempC)) {
                    _backend._MoveCard(mainGame.MyIndex, Me.CardsOnBoard.IndexOf(tempC), CardsToMoveList[i].x, CardsToMoveList[i].y);
                }
            }   
        }

       
        private void SwitchFinishTurnButtonHandler(string message) {

            btnInStateFMT = false;
            btn_Finish.Content = message;
            btn_Finish.IsEnabled = true;
        }

        private void RedoAll(object sender, RoutedEventArgs e)
        {
            ReverseDataAndActions(PackCount);
        }

        private void ReverseDataAndActions(int reverseNum) {

           
            ReverseAllActions(reverseNum); 

            if(mainGame.reversePackList.Count > 0)
            mainGame.LoadPlayerData();
      

        }

       

     

      

        private void DoAllActions(int count,int startIndex = -1) {

            currentState = startIndex;
            int start = startIndex + 1;

            for (int i = start,item = start; i < count;item++ ) {     
                bool actionFinished = false;
                ReversePack pack = mainGame.reversePackList[i];
                switch (pack.action)
                {
                    case GameAction.IncreasePopu:
                        if (mainGame.CheckIncreasePopulation())
                        {
                            DoIncreasePopulation_Local(false);
                            actionFinished = true;
                        }
                        break;

                    case GameAction.PickUpCard:
                        PickCardPack ppack = pack as PickCardPack;

                        int cardPos = Grid.GetColumn(ppack.pickedCard) / 2;


                        if (mainGame.CheckPickUpACard(ppack.pickedCard, cardPos))
                        {       
                            DoPickCard_Local(mainGame.GameBoard.CardList.IndexOf(ppack.pickedCard), cardPos, ppack.x, ppack.y, false);
                            actionFinished = true;
                        }
                      
                    
                        break;

                    case GameAction.BuildUnit:
                        BuildUnitPack bpack = pack as BuildUnitPack;
                        if (mainGame.CheckBuildUnit(bpack.card)) {          
                            DoBuildUnit_Local(Me.CardsOnBoard.IndexOf(bpack.card),false);
                            actionFinished = true;
                        }
                        break;

                    case GameAction.PlayCard:
                        PlayCardPack pcpack = pack as PlayCardPack;
                        if (mainGame.CheckPlayACard(pcpack.card)) {
                  
                            DoPlayCard_Local(Me.CardsOnBoard.IndexOf(pcpack.card),false);
                            actionFinished = true;
                        }
                        break;

                    case GameAction.DestroyUnit:
                        DestroyUnitPack dpack = pack as DestroyUnitPack;
                        if (mainGame.CheckDestroy_A_Unit(dpack.card))
                        {
                            DoDestroyUnit_Local(Me.CardsOnBoard.IndexOf(dpack.card),false);
                            actionFinished = true;
                        }
                        break;

                    case GameAction.DoNothing: 
                        actionFinished = true; 
                        break;

                    default: throw new NotImplementedException();
                }

                if (!actionFinished)
                {                   
                    ReportFailure(item);
                    break;              
                }
                else {
                    currentState++;
                    i++;
                    ReportSuccess(item);
                }
            }
        }

    


        //9999
        private void Lbox_Actions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = Lbox_Actions.SelectedIndex;

            if (index == currentState||index == -1) {

                foreach (ActionInfo a in actionRegister)
                {
                    a.HideButton();
                }

                if (index > 0)
                {
                    actionRegister[index].DisplayButton();
                }
                return;
            }


            if (BasicCheck())
            {           

                foreach (ActionInfo a in actionRegister)
                {
                    a.HideButton();
                }

                if (actionRegister[index].brush == Brushes.Red||actionRegister[index].brush == Brushes.LightGray) {
                    actionRegister[index].DisplayButton();
                    return;
                }

             
                ReverseDataAndActions(currentState + 1);
                          
                if (index > 0)
                {
                    actionRegister[index].DisplayButton();
                   
                }  
                DoAllActions(index + 1);  
                       
            }
            
          
           
        }

        private void CloseListBoxItem(object sender, RoutedEventArgs e)
        {
            if (BasicCheck())
            {
                int index = Lbox_Actions.SelectedIndex;

                if (actionRegister[index].brush == Brushes.Gray)
                {
                    actionRegister.RemoveAt(index);

                }
                else
                {
                    ReverseDataAndActions(currentState + 1);
                    actionRegister.RemoveAt(index);
                    mainGame.reversePackList.RemoveAt(index);
                    DoAllActions(PackCount);
                }
            }
            
           
        }

        List<string> SetCardsRowForNextTurn() {

          

            if (GameManager.gameRound == 1 && !mainGame.IsTheLastPlayerThisRound())
            {
                return GetCardsOnTheRowRoundOne();
            } 
            
                List<string> temp = new List<string>();
            int count = mainGame.GameBoard.ActualCardsOnDisplay();


            int startIndex = mainGame.GameBoard.Num_CardsToBeRemoved - cardsAlreadyRemoved;
            for (int i = 0; i < count ; i++,startIndex++) {
                if(startIndex < mainGame.GameBoard.CardList.Count)
                temp.Add(mainGame.GameBoard.CardList[startIndex].MyName);
            }
            return temp;
        }

        List<string> GetCardsOnTheRowRoundOne() {

            List<string> temp = new List<string>();
            for (int i = 0; i < 13; i++)
            {
                    temp.Add(mainGame.GameBoard.CardList[i].MyName);
            }
            return temp;
        }
  
        
    }
}
