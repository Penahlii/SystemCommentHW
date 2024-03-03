using System.Windows;
using SystemPractices2.ViewModels.WindowViewModels;

namespace SystemPractices2.Views.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
