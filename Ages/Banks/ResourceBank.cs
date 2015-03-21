using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace Ages
{
    class ResourceBank:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static int CAPACITY = 18;
        private int tokenInBnak = 18;
        public int rRow = 3;
        public int rCol = 1;
        public int myOwner = 0;

        public int TokenInBank {
            get { return tokenInBnak; }
            set {
                tokenInBnak = value; 
               OnPropertyChanged("R_inBnak");
               if (tokenInBnak > 18) 
               { 
                   MessageBox.Show("Overflow_R_inBank"); 
               }

            }
        }

        protected void OnPropertyChanged(string name)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public ResourceBank(int from) {
            myOwner = from;
        }
    }
}
