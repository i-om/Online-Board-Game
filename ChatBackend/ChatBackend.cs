using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatBackend
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatBackend : IChatBackend
    {
      
     
       

        DisplayMessageDelegate _displayMessageDelegate = null;
        public event JoinDelegate OnJoin;
        public event StartGameDelegate OnStartGame;
        public event MoveCardDelegate OnMoveCard;
        public event PickCardDelegate OnPickCard;
        public event ConsumeCAPDelegate OnConsumeCAP;
        public event UpdatePlayerIndexDelegate OnUpdatePlayer;
        public event FinishATurnDelegate OnFinishTurn;
        public event IncreasePopulationDelegate OnIncreasePopulation;
        public event BuildUnityDelegate OnBuildUnit;
        public event PlayCardDelegate OnPlayCard;
        public event DestroyUnitDelegate OnDestroyUnit;
        public event ReverseDataDelegate OnReverseData;


        private int back_PlayerIndex;
        public int Back_PlayerIndex 
        {
            get { return back_PlayerIndex; }
        }

        private int gameRound = 1;

        /// <summary>
        /// The default constructor is only here for testing purposes.
        /// </summary>
        private ChatBackend()
        {
        }

        /// <summary>
        /// ChatBackend constructor should be called with a delegate that is capable of displaying messages.
        /// </summary>
        /// <param name="dmd">DisplayMessageDelegate</param>
        public ChatBackend(DisplayMessageDelegate dmd)
        {
            _displayMessageDelegate = dmd;
          

            ///StartService();
        }





        #region INTERFACE IMPLEMENTATION

        public void DisplayMessage(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (_displayMessageDelegate != null)
            {
                _displayMessageDelegate(composite);
            }
        }

        public void Join(string member)
        {
            if (OnJoin != null)
            {
                back_PlayerIndex++;
                OnJoin(member, back_PlayerIndex);
            }
        }

        public void BuildGame(int index,List<int> random)
        {
            if (OnStartGame != null)
            {
                OnStartGame(index,random);
            }
        }

        public void MoveCard(int player, int card, double x, double y) {
            if (OnMoveCard != null) {
                OnMoveCard(player, card, x, y);
            }
        }

        public void PickCard(int player, string name, double x, double y)
        {
            if(OnPickCard !=null ){
                OnPickCard( player, name,  x, y);
            }
        }

        public void ConsumeCAP(int player, int num) {
            if (OnConsumeCAP != null)
            {
                OnConsumeCAP(player, num);
            }
        }

   

        public void UpdatePlayerIndex(int player,int removedCards,List<string> cardsOnRow)
        {
            if (player > Back_PlayerIndex - 1) {
                gameRound++;
                player = 0;
            }

            if(OnUpdatePlayer != null)
            {
                OnUpdatePlayer(player,gameRound,removedCards,cardsOnRow);
            }
        }

        public void FinishATurn()
        {
            if (OnFinishTurn != null) {
                OnFinishTurn();
            }
        }

        public void IncreasePopulation(int player) {
            if (OnIncreasePopulation != null) {
                OnIncreasePopulation(player);
            }
        }

        public void BuildUnit(int player,int card) {
            if (OnBuildUnit != null) {
                OnBuildUnit(player,card);
            }
        }

        public void PlayACard(int player,int card) {
            if (OnPlayCard != null) {
                OnPlayCard(player,card);
            }
        }

        public void DestroyUnit(int player,int card) {
            if (OnDestroyUnit != null) {
                OnDestroyUnit(player,card);
            }
        }

        public void ReverseData(int player) {
            if (OnReverseData != null) {
                OnReverseData(player);
            }
        }


        #endregion






        #region BROADCAST

        public void _GameStart(List<int> random) {
            _channel.BuildGame(back_PlayerIndex,random);
        }
     
        public void _Join(string member)
        {
            _channel.Join(member);
        }

        public void _MoveCard(int player, int card, double x, double y) {
            _channel.MoveCard(player, card, x, y);
        }

        public void _PickCard(int player, string name, double x, double y)
        {

            _channel.PickCard(player, name, x, y);
        }

        public void _ConsumeCAP(int player, int num) {
            _channel.ConsumeCAP(player, num);
        }
 
        public void _UpdatePlayerIndex(int player,int removedCards,List<string> cardsOnRow) {
            _channel.UpdatePlayerIndex(player,removedCards,cardsOnRow);
        }

        public void _FinishATurn() {
            _channel.FinishATurn();
        }

        public void _IncreasePopulation(int player) {
            _channel.IncreasePopulation(player);
        }

        public void _BuildUnit(int player,int card) {
            _channel.BuildUnit(player,card);
        }

        public void _PlayACard(int player,int card) {
            _channel.PlayACard(player,card);
        }

        public void _DestroyUnit(int player,int card) {
            _channel.DestroyUnit(player,card);
        }

        public void _ReverseData(int player) {
            _channel.ReverseData(player);
        }


        public void SendMessage(string text)
        {
          
            if (text.StartsWith("setname:", StringComparison.OrdinalIgnoreCase))
            {
                _myUserName = text.Substring("setname:".Length).Trim();
                _displayMessageDelegate(new CompositeType("Event", "Setting your name to " + _myUserName));
            }
            else
            {
                // In order to send a message, we call our friends' DisplayMessage method
                _channel.DisplayMessage(new CompositeType(_myUserName, text));
            }
        }

      
        #endregion


      

        private string _myUserName = "Anonymous";
        private ServiceHost host = null;
        private ChannelFactory<IChatBackend> channelFactory = null;
        private IChatBackend _channel;

        public void StartService()
        {
            host = new ServiceHost(this);
            host.Open();
            channelFactory = new ChannelFactory<IChatBackend>("ChatEndpoint");
            _channel = channelFactory.CreateChannel();

          
            // Information to display locally
            _displayMessageDelegate(new CompositeType("Info", "Welcome to play Through the Ages"));

        }
        private void StopService()
        {
            if (host != null)
            {
                _channel.DisplayMessage(new CompositeType("Event", _myUserName + " is leaving the conversation."));
                if (host.State != CommunicationState.Closed)
                {
                    channelFactory.Close();
                    host.Close();
                }
            }
        }

        #region others

        
        
        #endregion


    }
}
