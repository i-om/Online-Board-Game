using System;
using System.Windows.Media;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Ages
{
    public class Card:Canvas
    {
        private CardClass myClass;
        private CardType myType;
        private bool isFaceDown;
        //Determine if a card is moveable
        private bool isPlayable;
        private int age;
        private string name;
 
        private ImageBrush myImage;
        private ImageBrush myBack;

       public CardClass MyClass
       {
            get { return myClass; }
           set { myClass = value; }
        }

       public CardType MyType
       {
           get { return myType; }
           set { myType = value; }
       }

       public string MyName {
           get { return name; }
           set { name = value; }
       }

        public int Age
        {
            get { return age; }
        }

        public bool IsFaceDown {
            get { return isFaceDown; }
            set { isFaceDown = value;
            Background = isFaceDown ? myBack : MyImage;
            }
        }

        public bool IsPlayable
        {
            get { return isPlayable; }
            set { isPlayable = value; }
        }

        public ImageBrush MyImage {
            get { return myImage; }
            set { myImage = value; }
        }

        public Card() {
            
            IsFaceDown = false;
            IsPlayable = true;
            MyClass = CardClass.Other;
            age = 0;
        }


        public Card(int ageValue,BitmapSource bitmapImage,string name)
        {
            MyImage = new ImageBrush(bitmapImage);
            MyName = name;

            IsPlayable = true;
            ClipToBounds = true;

            MyClass = CardClass.Other;
            MyType = CardType.Civil;
            age = ageValue;

            switch(Age){
                case 0:
                    if(MyType == CardType.Civil){
                     myBack = new ImageBrush(BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A_Civil___Card_Back));
                    }else{
                        myBack = new ImageBrush(BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A_Military___Card_Back));
                    }                 
                    break;
                case 1:
                     if(MyType == CardType.Civil){
                     myBack = new ImageBrush(BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I_Civil___Card_Back));
                    }else{
                        myBack = new ImageBrush(BitmapConversion.ToWpfBitmap(Ages_Resource.Age_I_Military___Card_Back));
                    }                 
                    break;
                case 2:
                     if(MyType == CardType.Civil){
                     myBack = new ImageBrush(BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A_Civil___Card_Back));
                    }else{
                        myBack = new ImageBrush(BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A_Military___Card_Back));
                    }                 
                    break;
                case 3:
                     if(MyType == CardType.Civil){
                     myBack = new ImageBrush(BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A_Civil___Card_Back));
                    }else{
                        myBack = new ImageBrush(BitmapConversion.ToWpfBitmap(Ages_Resource.Age_A_Military___Card_Back));
                    }                 
                    break;
            }  
  
            IsFaceDown = false;      
        }     
    }
}
