using System;
using DiaryApp.Model;
using DiaryApp.Services;
using DiaryApp.View;
using DiaryApp.ViewModel;
using DiaryApp.ViewModels;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiaryApp
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            NavigateToLogin();
        }

        private async void NavigateToLogin()
        {              
          await NavigationService.NavigateAsync("LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<NewNotePage, NewNotePageViewModel>();
            containerRegistry.Register<IDataRepository<Notes>, DiaryRepository>();
            containerRegistry.RegisterInstance(DependencyService.Get<IAccountService>());
            containerRegistry.RegisterInstance(DependencyService.Get<IRepository<Notes>>());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
