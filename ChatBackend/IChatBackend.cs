using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatBackend
{
    [ServiceContract]
    public interface IChatBackend
    {
        [OperationContract(IsOneWay = true)]
        void DisplayMessage(CompositeType composite);

        [OperationContract(IsOneWay = true)]
        void Join(string member);

        [OperationContract(IsOneWay = true)]
        void BuildGame(int index,List<int> random);

        [OperationContract(IsOneWay = true)]
        void MoveCard(int player,int card,double x,double y);

        [OperationContract(IsOneWay = true)]
        void PickCard(int player, string name, double x, double y);

        [OperationContract(IsOneWay = true)]
        void ConsumeCAP(int player, int num);

        [OperationContract(IsOneWay = true)]
        void UpdatePlayerIndex(int player,int removedCards,List<string> cardsOnRow);

        [OperationContract(IsOneWay = true)]
        void FinishATurn();

        [OperationContract(IsOneWay = true)]
        void IncreasePopulation(int player);

        [OperationContract(IsOneWay = true)]
        void BuildUnit(int player,int card);

        [OperationContract(IsOneWay = true)]
        void PlayACard(int player,int card);

        [OperationContract(IsOneWay = true)]
        void DestroyUnit(int player,int card);

        [OperationContract(IsOneWay = true)]
        void ReverseData(int player);

        void SendMessage(string text);

    }

    [DataContract]
    public class CompositeType
    {
        private string _username = "Anonymous";
        private string _message = "";

        public CompositeType() { }
        public CompositeType(string u, string m)
        {
            _username = u;
            _message = m;
        }

        [DataMember]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        [DataMember]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }

    public delegate void DisplayMessageDelegate(CompositeType data);
    public delegate void StartGameDelegate(int index,List<int> random);
    public delegate void JoinDelegate(string member, int num);
    public delegate void MoveCardDelegate(int player,int card,double x,double y);
    public delegate void PickCardDelegate(int player, string name, double x, double y);
    public delegate void ConsumeCAPDelegate (int player,int num);
    public delegate void UpdatePlayerIndexDelegate(int player,int round,int removedCards,List<string> cardsOnRow);
    public delegate void FinishATurnDelegate();
    public delegate void IncreasePopulationDelegate(int player);
    public delegate void BuildUnityDelegate(int player,int card);
    public delegate void PlayCardDelegate(int player,int card);
    public delegate void DestroyUnitDelegate(int player,int card);
    public delegate void ReverseDataDelegate(int player);

}
