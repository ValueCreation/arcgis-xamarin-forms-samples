using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ArcGISSamples.ViewModels
{
    public class MapsListPageViewModel : BindableBase
    {
		private readonly INavigationService _navigationService;
		public ICommand GoBack1Command { get; }
        public MapsListPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			GoBack1Command = new DelegateCommand(() =>
			{
				_navigationService.GoBackAsync();
			});

		}
    }
}
