using Core.Enums;
using EnglishBySongs.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EnglishBySongs.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            _pageService = new PageService();

            _autoTranslatingIsSwitchedOn = Preferences.Get("AutoTranslating", true);

            _wordsSortingMode = (WordsSortingModes)Preferences.Get("WordsSortingMode", 2);
            _wordsSortingModes = new List<string>
            {
                "По дате добавления",
                "По дате добавления (обр.)",
                "По слову на иностранном",
                "По слову на иностранном (обр.)",
                "По переводу",
                "По переводу (обр.)",
                "По песням",
                "По песням (обр.)"
            };

            _songsSortingMode = (SongsSortingModes)Preferences.Get("SongsSortingMode", 2);
            _songsSortingModes = new List<string>
            {
                "По дате добавления",
                "По дате добавления (обр.)",
                "По названию песни",
                "По названию песни (обр.)",
                "По исполнителю",
                "По исполнителю (обр.)"
            };
        }

        protected IPageService _pageService;

        private bool _autoTranslatingIsSwitchedOn;

        public bool AutoTranslatingIsSwitchedOn
        {
            get { return _autoTranslatingIsSwitchedOn; }
            set
            {
                SetValue(ref _autoTranslatingIsSwitchedOn, value);
                OnPropertyChanged(nameof(AutoTranslatingIsSwitchedOn));
                Preferences.Set("AutoTranslating", _autoTranslatingIsSwitchedOn);
            }
        }

        private List<string> _wordsSortingModes;

        public List<string> WordsSortingModes
        {
            get { return _wordsSortingModes; }
            set
            {
                SetValue(ref _wordsSortingModes, value);
                OnPropertyChanged(nameof(WordsSortingModes));
            }
        }

        private WordsSortingModes _wordsSortingMode;

        public WordsSortingModes WordsSortingMode
        {
            get { return _wordsSortingMode; }
            set
            {
                SetValue(ref _wordsSortingMode, value);
                OnPropertyChanged(nameof(WordsSortingMode));
                Preferences.Set("WordsSortingMode", (int)_wordsSortingMode);
                MessagingCenter.Send(this, "WordsSortingModeChanged");
            }
        }

        private List<string> _songsSortingModes;

        public List<string> SongsSortingModes
        {
            get { return _songsSortingModes; }
            set
            {
                SetValue(ref _songsSortingModes, value);
                OnPropertyChanged(nameof(SongsSortingModes));
            }
        }

        private SongsSortingModes _songsSortingMode;

        public SongsSortingModes SongsSortingMode
        {
            get { return _songsSortingMode; }
            set
            {
                SetValue(ref _songsSortingMode, value);
                OnPropertyChanged(nameof(SongsSortingMode));
                Preferences.Set("SongsSortingMode", (int)_songsSortingMode);
                MessagingCenter.Send(this, "SongsSortingModeChanged");
            }
        }
    }
}
