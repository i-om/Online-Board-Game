using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ages
{
    public class FeedBackText:DependencyObject
    {
        public static readonly DependencyProperty FeedBackStringProperty =
        DependencyProperty.Register( "Text", typeof( string ),
        typeof( FeedBackText ), new UIPropertyMetadata( "Status bar" ) );
        public string Text {
        get { return (string) GetValue( FeedBackStringProperty ); }
        set { SetValue(FeedBackStringProperty, value); }
    }

    public static FeedBackText Instance { get; private set; }

    static FeedBackText()
    {
        Instance = new FeedBackText();
    }
    }
}
