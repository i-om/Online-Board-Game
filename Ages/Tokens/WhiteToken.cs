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
    class WhiteToken:Image
    {
        public WhiteToken(MouseButtonEventHandler down, MouseEventHandler move, MouseButtonEventHandler up)
        {
               Source = BitmapConversion.ToWpfBitmap(Ages_Resource.GrayToken_fw);
               Height = 20;
               MouseLeftButtonDown += down;
               MouseMove += move;
               MouseLeftButtonUp += up;                         
        }
    }
}
