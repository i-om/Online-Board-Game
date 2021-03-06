﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.ComponentModel;

namespace Ages
{
    class MineCard : UnitCard,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
             
        private int productValue;
        private int token;
        public int Token {
            get { return token; }
            set { token = value;
            if (token < 0) token = 0;
            OnPropertyChanged("OreToken");
            Product = Token * productValue;
            }
        }

        public MineCard(string name,int ageValue, BitmapSource bitmapImage,int costValue,int scienceValue,int oreValue)
            : base(name,ageValue, bitmapImage, costValue, scienceValue)
        {
            MyClass = CardClass.Mine;
            productValue = oreValue;
        }

        protected void OnPropertyChanged(string name)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        public void ProduceOne(ResourceBank tempRB)
        {
            if (tempRB.TokenInBank > 0)
            {

                Token++;
                tempRB.TokenInBank--;
            }
        }

        public void Produce(ResourceBank tempRB)
        {

            if (tempRB.TokenInBank > 0)
            {
                int actualProductionAmount = tempRB.TokenInBank > Unit ? Unit : tempRB.TokenInBank;

                Token += actualProductionAmount;
                tempRB.TokenInBank -= actualProductionAmount;
            }

        }

        public int Replace(int num, ResourceBank rBank)
        {

            int temp = num;

            if (num >= productValue)
            {
                int expectedToken = num / productValue;  

                temp = num - expectedToken * productValue;

                if (rBank.TokenInBank < expectedToken)
                {
                    expectedToken = rBank.TokenInBank;
                    temp = 0;
                };

                Token += expectedToken; 
                
                rBank.TokenInBank -= expectedToken;

             
              
            }

            return temp;
        }

        public int Consume(int num, ResourceBank tempRB)
        {
            int temp = num;

            if (Token > 0)
            {
                int actual_FTBC, expected_FTBC;

                expected_FTBC = num % productValue == 0 ? num / productValue : num / productValue + 1;

                actual_FTBC = Token >= expected_FTBC ? expected_FTBC : Token;

                Token -= actual_FTBC;

                temp = num - productValue * actual_FTBC;

                tempRB.TokenInBank += actual_FTBC;
            }

            return temp;
        }




    }
}

