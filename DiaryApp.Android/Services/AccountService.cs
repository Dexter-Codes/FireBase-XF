using System;
using System.Threading.Tasks;
using DiaryApp.Droid.ServiceListeners;
using DiaryApp.Droid.Services;
using DiaryApp.Model;
using DiaryApp.Services;
using Firebase.Auth;
using Firebase.Firestore;
using Xamarin.Forms;

[assembly: Dependency(typeof(AccountService))]
namespace DiaryApp.Droid.Services
{
    public class AccountService: IAccountService
    {
        private string _verificationId;

        public Task<bool> LoginAsync(string username, string password)
        {
            var tcs = new TaskCompletionSource<bool>();
            FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password)
                .ContinueWith((task) => OnAuthCompleted(task, tcs));
            return tcs.Task;
        }

        public Task<AuthenticatedUser> GetUsersAsync()
        {
            var tcs = new TaskCompletionSource<AuthenticatedUser>();

            FirebaseFirestore.Instance
                .Collection("users")
                .Document(FirebaseAuth.Instance.CurrentUser.Uid)
                .Get()
                .AddOnCompleteListener(new OnCompleteListener(tcs));

            return tcs.Task;
        }

        private void OnAuthCompleted(Task task, TaskCompletionSource<bool> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // something went wrong
                tcs.SetResult(false);
                return;
            }
            _verificationId = null;
            tcs.SetResult(true);
        }
    }
}
