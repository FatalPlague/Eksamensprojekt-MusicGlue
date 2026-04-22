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
        public App()
        {
            var repo = new ConsignmentRepository();
        }
    }

}
