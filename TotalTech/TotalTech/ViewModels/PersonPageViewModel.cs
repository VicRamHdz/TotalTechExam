using System;
using Prism.Navigation;
using Prism.Services;
using TotalTech.Framework;

namespace TotalTech.ViewModels
{
    public class PersonPageViewModel : BaseViewModel
	{
        public PersonPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigation = navigationService;
            _dialogService = dialogService;
        }
    }
}
