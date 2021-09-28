using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiaryApp.Model;
using Plugin.CloudFirestore;
using Xamarin.Forms;

namespace DiaryApp.Services
{
    public class DiaryRepository : IDataRepository<Notes>
    {
        public DiaryRepository()
        {
            if (Application.Current.Properties.ContainsKey("id"))
            {
                var id = Application.Current.Properties["id"] as string;
                token = id;
            }
        }

        public string token { get; set; } = string.Empty;

        public string DocumentPath  => "users/" + token + "/Notes";

        public async Task<bool> Delete(Notes item)
        {
            await CrossCloudFirestore.Current
                        .Instance
                        .Collection(DocumentPath)
                        .Document(item.Id)
                        .DeleteAsync();

            return true;
        }

        public async Task<Notes> Get(string id)
        {
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection(DocumentPath)
                                        .Document(id)
                                        .GetAsync();

            var notes = document.ToObject<Notes>();
            return notes;
        }

        public async Task<IEnumerable<Notes>> GetAll()
        {
            var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(DocumentPath)
                                     .OrderBy("DateEntered", true)
                                     .GetAsync();

            var notes = query.ToObjects<Notes>();

            return notes;
        }

        public async Task<string> Save(Notes item)
        {
            var id = await CrossCloudFirestore.Current
                            .Instance
                            .Collection(DocumentPath)
                            .AddAsync(item);

            return id.ToString();
        }

        public async Task<bool> Update(Notes item)
        {
            await CrossCloudFirestore.Current
                            .Instance
                            .Collection(DocumentPath)
                            .Document(item.Id)
                            .UpdateAsync(item);

            return true;
        }
    }
}
