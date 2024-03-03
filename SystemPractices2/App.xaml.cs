using System.Configuration;
using System.Data;
using SimpleInjector;
using System.Windows;
using SystemPractices2.Views.Windows;
using SystemPractices2.ViewModels.WindowViewModels;

namespace SystemPractices2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        public static SimpleInjector.Container? _Container;
        private void StartPoint(object sender, StartupEventArgs e)
        {
            _Container = new();

            _Container.RegisterSingleton<MainWindow>();
            _Container.RegisterSingleton<MainWindowViewModel>();

            _Container.RegisterSingleton<AddWindow>();
            _Container.RegisterSingleton<AddWindowViewModel>();

            var mainWindow = _Container.GetInstance<MainWindow>();
            mainWindow.DataContext = _Container.GetInstance<MainWindowViewModel>();
            mainWindow.ShowDialog();
        }
    }
}
