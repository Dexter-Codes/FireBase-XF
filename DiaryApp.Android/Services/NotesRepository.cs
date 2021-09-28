using System;
using DiaryApp.Droid.Services;
using DiaryApp.Model;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotesRepository))]
namespace DiaryApp.Droid.Services
{
  public class NotesRepository : BaseRepository<Notes>
  {
    public override string DocumentPath =>
        "users/"
        + Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid
        + "/Notes";       

    }
}
