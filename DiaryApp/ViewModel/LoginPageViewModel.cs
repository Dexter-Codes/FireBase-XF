using System;
using System.Windows.Input;
using DiaryApp.Services;
using DiaryApp.View;
using DiaryApp.ViewModel;
using Plugin.FirebaseAuth;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DiaryApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        IAccountService _accountService;

        IPageDialogService _dialogService;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; set; }

        public bool IsSaved { get; set; }

        INavigationService navigationService;
        public LoginPageViewModel(INavigationService navigationService, IAccountService accountService,
            IPageDialogService dialogService)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _accountService = accountService;
            this.navigationService = navigationService;
            LoginCommand = new Command(OnLoginBtnClick);
        }

        public override  void OnNavigatedTo(INavigationParameters parameters)
        {
            // store the email and password
            //var userEmail = await SecureStorage.GetAsync("Email");

            //if (!string.IsNullOrEmpty(userEmail))
            //    Email = userEmail;

            //var password= await SecureStorage.GetAsync("Password");

            //if (!string.IsNullOrEmpty(password))
            //    Password = password;

            //if(userEmail!=null && password!=null)
            //{
            //    IsSaved = true;
            //}
            //else
            //{
            //    IsSaved = false;
            //}

        }

        private async void OnLoginBtnClick(object obj)
        {

            var result = await CrossFirebaseAuth.Current.Instance.SignInAnonymouslyAsync();

            if(result!=null)
            {
                var id = result.User.Uid;
                SetUserDetails(id);
            }

            // Sign in with Email
            //var isValidated = ValidateFields();
            //if (isValidated)
            //{
            //    var loginAttempt = await _accountService.LoginAsync(Email, Password);
            //    if (loginAttempt)
            //    {
            //        try
            //        {
            //            if (!IsSaved)
            //            {
            //                await SecureStorage.SetAsync("Email", Email);
            //                await SecureStorage.SetAsync("Password", Password);
            //            }
            //            GetUserDetails();                      
            //        }
            //        catch (Exception ex)
            //        {

            //        }                   
            //    }
            //    else
            //    {
            //        await _dialogService.DisplayAlertAsync("Login Failed", "Please enter the correct Email & Password to login", "OK");
            //    }
            //}
            //else
            //{
            //    await _dialogService.DisplayAlertAsync("Alert", "Please enter the Email & Password to login", "OK");
            //}
        }

        private async void SetUserDetails(string id)
        {
            //Gets the user data logged with email
            //var userdata=await _accountService.GetUsersAsync();

            Application.Current.Properties["id"] = id;

            await navigationService.NavigateAsync("NavigationPage/HomePage");

            Email = string.Empty;
            Password = string.Empty;
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(Email))
                return false;
            else if (string.IsNullOrEmpty(Password))
                return false;
            else
                return true;
        }

    }

}
