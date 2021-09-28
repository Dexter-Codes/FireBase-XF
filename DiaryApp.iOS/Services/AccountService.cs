using System;
using System.Threading.Tasks;
using DiaryApp.iOS.Services;
using DiaryApp.Model;
using DiaryApp.Services;
using Firebase.Auth;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(AccountService))]
namespace DiaryApp.iOS.Services
{
    public class AccountService: IAccountService
    {        

        public Task<bool> LoginAsync(string username, string password)
        {
            var tcs = new TaskCompletionSource<bool>();
            Auth.DefaultInstance.SignInWithPasswordAsync(username, password)
                .ContinueWith((task) => OnAuthCompleted(task, tcs));
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
            // user is logged in
            tcs.SetResult(true);
        }

        public Task<AuthenticatedUser> GetUsersAsync()
        {
            var tcs = new TaskCompletionSource<AuthenticatedUser>();

            Firebase.CloudFirestore.Firestore.SharedInstance
                .GetCollection("users")
                .GetDocument(Auth.DefaultInstance.CurrentUser.Uid)
                .GetDocument((snapshot, error) =>
                {
                    if (error != null)
                    {
                        // something went wrong
                        tcs.TrySetResult(default(AuthenticatedUser));
                        return;
                    }
                    tcs.TrySetResult(new AuthenticatedUser
                    {
                        Id = snapshot.Id,
                        FirstName = snapshot.GetValue(new NSString("FirstName")).ToString(),
                        LastName = snapshot.GetValue(new NSString("LastName")).ToString()
                    });
                });

            return tcs.Task;
        }
    }
}
