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
    class Resource:BlueYellowToken
    {
        public int RepresentativeValue{get;set;}

        public Resource(MouseButtonEventHandler down, MouseEventHandler move, MouseButtonEventHandler up, bool playable = true)
            : base(down, move, up, playable)
        {
            RepresentativeValue = 0;
            Source = BitmapConversion.ToWpfBitmap(Ages_Resource.BlueToken_fw);    
        }
    }
}
