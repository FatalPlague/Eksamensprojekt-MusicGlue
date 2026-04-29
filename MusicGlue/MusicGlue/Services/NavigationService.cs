using MusicGlue.Stores;
using MusicGlue.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<BaseViewModel> _viewModelFactory;

        public NavigationService(NavigationStore navigationStore, Func<BaseViewModel> viewModelFactory)
        {
            this._navigationStore = navigationStore;
            this._viewModelFactory = viewModelFactory;
        }
        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _viewModelFactory();
        }
    }
}

