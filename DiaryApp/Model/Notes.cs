using System;
using DiaryApp.Services;
using Plugin.CloudFirestore.Attributes;

namespace DiaryApp.Model
{
    public class Notes : IIdentifiable
    {
        [Id]
        public string Id { get; set; }
        public string Title { get; set; }
        public string NotesEntered { get; set; }
        public DateTime DateEntered { get; set; }
    }
}
