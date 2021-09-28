using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DiaryApp.View
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            var oauthToken =  SecureStorage.GetAsync("Email");
            if (oauthToken != null)
                return true;

            else
                return false;
        }
    }
}
