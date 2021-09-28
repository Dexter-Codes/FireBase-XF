using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DiaryApp.Model;
using DiaryApp.Services;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using DiaryApp.Extensions;
using Prism.Services;
using Xamarin.Essentials;
using DiaryApp.ViewModels;
using Plugin.CloudFirestore;
using NavigationMode = Prism.Navigation.NavigationMode;
using Plugin.FirebaseAuth;
using IListenerRegistration = Plugin.CloudFirestore.IListenerRegistration;
using DiaryApp.Helper;

namespace DiaryApp.ViewModel
{
    public class HomePageViewModel : ViewModelBase
    {
        public ICommand EditCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand AddNewDiaryCommand { get; set; }

        public ICommand SignoutCommand { get; set; }

        public ICommand ViewCommand { get; set; }

        public ObservableCollection<Notes> DiariesList { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        IDataRepository<Notes> _dataRepository;

        IPageDialogService _dialogService;

        INavigationService _navigationService;

        IListenerRegistration listener;

        public HomePageViewModel(INavigationService navigationService,
            IPageDialogService dialogService, IDataRepository<Notes> dataRepository)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataRepository = dataRepository;
            EditCommand = new Command(OnEditNotes);
            DeleteCommand = new Command(OnDelete);
            AddNewDiaryCommand = new Command(OnAddNote);
            SignoutCommand = new Command(OnSignout);
            ViewCommand = new Command(OnViewNotes);
            DiariesList = new ObservableCollection<Notes>();
        }
        
        private async void OnSignout(object obj)
        {
            var answer = await _dialogService.DisplayAlertAsync(AppTitle.SignOut, AppTitle.Signout_Question, AppTitle.Yes, AppTitle.No);
            if (answer)
            {
                //SecureStorage.RemoveAll();
                CrossFirebaseAuth.Current.Instance.SignOut();
                listener.Remove();
                await _navigationService.GoBackAsync();
            }
            else
            {
                return;
            }
        }

        private void OnAddNote(object obj)
        {
            var notes = obj as Notes;
            var navParameters = new NavigationParameters
            {
              {"isAdd",true},
              {"isEdit", false},
              {"UID",token}
            };
            _navigationService.NavigateAsync("NewNotePage",navParameters);
        }

        private async void OnDelete(object obj)
        {
            var notes = obj as Notes;
            var answer = await _dialogService.DisplayAlertAsync(AppTitle.Delete, AppTitle.Delete_Question, AppTitle.Yes, AppTitle.No);
            if(answer)
            {
                // var isDeleted = await _repository.Delete(notes);

                var isdelete=await _dataRepository.Delete(notes);

                if(isdelete)
                    await _dialogService.DisplayAlertAsync(AppTitle.Success, AppTitle.Delete_Title, AppTitle.OK);
            }
            else
            {
                return;
            }
        }

        private void OnEditNotes(object obj)
        {
            var notes = obj as Notes;
            var navParameters = new NavigationParameters
            {
              {"isAdd",false},
              { "id", notes.Id },
              { "isEdit", true},
              {"UID",token }
            };
            _navigationService.NavigateAsync("NewNotePage", navParameters);
        }

        private void OnViewNotes(object obj)
        {
            var notes = obj as Notes;
            var navParameters = new NavigationParameters
            {
              { "id", notes.Id },
              { "isEdit", false},
              {"UID",token }
            };
            _navigationService.NavigateAsync("NewNotePage", navParameters);
        }


        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
          
        }

        public override  void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                if (Application.Current.Properties.ContainsKey("id"))
                {
                    var id = Application.Current.Properties["id"] as string;
                    token = id;
                }

                SubscribeUpdates();
            }
        }

        private async void LoadNotes()
        {
            DiariesList.Clear();
            IsLoading = true;

            //var notes = await _repository.GetAll();

            var notes = await _dataRepository.GetAll();

            if (notes != null)
            {
                DiariesList.Clear();
                IsLoading = false;
                DiariesList.AddRange(notes);
            }
        }

        public void SubscribeUpdates()
        {
            listener = CrossCloudFirestore.Current
            .Instance
            .Collection(DocumentPath)
            .AddSnapshotListener((snapshot, error) =>
            {
                if (snapshot != null)
                {
                    foreach (var documentChange in snapshot.DocumentChanges)
                    {
                        switch (documentChange.Type)
                        {
                            case DocumentChangeType.Added:
                                LoadNotes();
                                break;
                            case DocumentChangeType.Modified:
                                LoadNotes();
                                break;
                            case DocumentChangeType.Removed:
                                LoadNotes();
                                break;
                        }
                    }
                }
            });
        }

    }
}
