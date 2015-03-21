using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ages
{
    class BlueYellowToken:Image
    {
        private const int TokenRadius = 20;
        private bool isPlayable;
        public bool IsPlayable
        {
            get { return isPlayable; }
            set { isPlayable = value; }
        }

        public BlueYellowToken() {
            Height = TokenRadius;
        }

        public BlueYellowToken(MouseButtonEventHandler down, MouseEventHandler move, MouseButtonEventHandler up,bool playable = true)
        {
            Height = TokenRadius;
            MouseLeftButtonDown += down;
            MouseMove += move;
            MouseLeftButtonUp += up;
            isPlayable = playable;
        }
    }
}
