using System.Windows;


namespace Richy_WPF_MVVM.Common.Navigation
{

    public partial class ComplexDialog : Window
    {

        public event DialogResultAvailable? DialogResultEvent;

    
        public ComplexDialog()
        {
            InitializeComponent();
        }

        
    }
}
