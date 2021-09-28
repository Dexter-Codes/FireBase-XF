using System;
using DiaryApp.iOS.Services;
using DiaryApp.Model;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotesRepository))]
namespace DiaryApp.iOS.Services
{
    public class NotesRepository:BaseRepository<Notes>
    {
        public override string DocumentPath =>
            "users/"
            + Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid
            + "/Notes";
      
    }
}
