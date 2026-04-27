using MusicGlue.Stores;
using MusicGlue.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MusicGlue
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NavigationStore navigationStore;
        protected override void OnStartup(StartupEventArgs e)
        {
            navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new HomeViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}
