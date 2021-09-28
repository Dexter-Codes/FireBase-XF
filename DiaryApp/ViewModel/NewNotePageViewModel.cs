using System;
using System.Windows.Input;
using DiaryApp.Helper;
using DiaryApp.Model;
using DiaryApp.Services;
using Plugin.CloudFirestore;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace DiaryApp.ViewModel
{
    public class NewNotePageViewModel: ViewModelBase
    {
        INavigationService _navigationService;

        IPageDialogService _dialogService;

        IDataRepository<Notes> _dataRepository;

        public ICommand SaveCommand { get; set; }

        private string _title;
        public  new string  Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private bool _isEditable=true;
        public bool IsEditable
        {
            get => _isEditable;
            set => SetProperty(ref _isEditable, value);
        }

        private string _buttonTitle;
        public string ButtonTitle
        {
            get => _buttonTitle;
            set => SetProperty(ref _buttonTitle, value);
        }

        public bool IsEdit { get; set; }
        public bool IsAdd { get; set; }
        public string NotesId { get; set; }

        private Notes Note { get; set; }

        public NewNotePageViewModel(INavigationService navigationService,
            IPageDialogService dialogService, IDataRepository<Notes> dataRepository)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataRepository = dataRepository;
            SaveCommand = new Command(OnSaveClick);
        }

        public override  void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override  void OnNavigatedTo(INavigationParameters parameters)
        {
            IsAdd = parameters.GetValue<bool>("isAdd");
            IsEdit = parameters.GetValue<bool>("isEdit");
            NotesId = parameters.GetValue<string>("id");
            token = parameters.GetValue<string>("UID");

            if (!IsAdd)
            {
                if (!IsEdit)
                {
                    ButtonTitle = AppTitle.Edit_BtnTitle;
                    IsEditable = false;
                }
                else
                {
                    ButtonTitle = AppTitle.Save_BtnTitle;
                    IsEditable = true;
                }
                LoadNotes(NotesId);
            }
            else
            {
                ButtonTitle = AppTitle.Save_BtnTitle;
                IsEditable = true;
            }
               
        }

        private async void LoadNotes(string id)
        {
            //var notes = await _repository.Get(id);            

            var notes = await _dataRepository.Get(id);

            if (notes != null)
            {
                Note = new Notes()
                {
                    Title=notes.Title,
                    NotesEntered=notes.NotesEntered,
                    DateEntered=notes.DateEntered,
                    Id=notes.Id
                };

                Title = Note.Title;
                Notes = Note.NotesEntered;
            }
        }

        private async void OnSaveClick(object obj)
        {
            var isValidated = ValidateFields();
            if (isValidated)
            {
                if (IsEditable)
                {
                    if (!IsEdit)
                    {
                        SaveNewDiary();
                    }
                    else
                    {
                        UpdateDiary();
                    }
                   
                }
                else
                {
                    ButtonTitle = AppTitle.Save_BtnTitle;
                    IsEditable = true;
                    IsEdit = true;
                }
            }
            else
            {
                await _dialogService.DisplayAlertAsync(AppTitle.Error, AppTitle.Validate_title, AppTitle.OK);
            }

        }

        private async void SaveNewDiary()
        {
            NotesId = IsEdit ? Note.Id : "";

            var notes = new Notes()
            {
                Title = Title,
                NotesEntered = Notes,
                DateEntered = DateTime.Now,
                Id = NotesId
            };

            //var id = await _repository.Save(notes);

            var id = await _dataRepository.Save(notes);

            if (id != null)
            {
                await _dialogService.DisplayAlertAsync(AppTitle.Success, AppTitle.Add_Title, AppTitle.OK);
                await _navigationService.GoBackAsync();
            }
        }

        private async void UpdateDiary()
        {
            var notes = new Notes()
            {
                Title = Title,
                NotesEntered = Notes,
                DateEntered = DateTime.Now,
                Id = NotesId
            };

            var isUpdated = await _dataRepository.Update(notes);

            if (isUpdated)
                await _dialogService.DisplayAlertAsync(AppTitle.Success, AppTitle.Edit_Title, AppTitle.OK);
            await _navigationService.GoBackAsync();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(Title))
                return false;
            else if (string.IsNullOrEmpty(Notes))
                return false;
            else
                return true;
        }
    }
}
