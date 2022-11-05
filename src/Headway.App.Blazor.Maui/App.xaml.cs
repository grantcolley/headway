using Microsoft.Maui.Controls;

namespace Headway.App.Blazor.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}