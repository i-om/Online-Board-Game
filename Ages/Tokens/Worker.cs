using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace Ages
{
    class Worker:BlueYellowToken
    {
       
        public Worker(MouseButtonEventHandler down,MouseEventHandler move, MouseButtonEventHandler up, bool playable = true):base(down,move,up,playable)
        {
            Source = BitmapConversion.ToWpfBitmap(Ages_Resource.YellowTokenSingle2);          
        }

        public Worker() {
            Source = BitmapConversion.ToWpfBitmap(Ages_Resource.YellowTokenSingle2);
        }

    }
}
