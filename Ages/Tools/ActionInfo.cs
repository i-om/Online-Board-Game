using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;

namespace Ages
{
    class ActionInfo:INotifyPropertyChanged
    {
     public string name { get; set; }

     private SolidColorBrush pbrush = Brushes.Black;
     public SolidColorBrush brush { 
         get{return pbrush;}
         set{pbrush = value;OnPropertyChanged("brush");}
     }
     private Visibility pvision;
     public Visibility vision
     {
         get { return pvision; }
         set { pvision = value; OnPropertyChanged("vision"); }
     }

     public event PropertyChangedEventHandler PropertyChanged;

     private void OnPropertyChanged(string name)
     {

         if (PropertyChanged != null)
         {
             PropertyChanged(this, new PropertyChangedEventArgs(name));
         }
     }
    

     public ActionInfo(string str)
     {
                name = str;
                vision = Visibility.Hidden;                            
     }

     public void DisplayButton() {
         vision = Visibility.Visible;
     }

     public void HideButton() {
         vision = Visibility.Hidden;
     }

     public void TurnBlack() {
         brush = Brushes.Black;
     }

     public void TurnRed() {
         brush = Brushes.Red;
     }

     public void TurnGray() {
         brush = Brushes.LightGray;
     }

     public bool IsRed() {
         return brush == Brushes.Red;
     }
    }
}
