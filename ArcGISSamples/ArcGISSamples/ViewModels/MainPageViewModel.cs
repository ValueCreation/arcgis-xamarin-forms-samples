using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ArcGISSamples.ViewModels
{
	public class MainPageViewModel : BindableBase, INavigationAware
	{
		private string _title;
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		private readonly INavigationService _navigationService;
		public ICommand NavigateMapsListPageCommand { get; }
        public ICommand NavigateLayersListPageCommand { get; }
		public ICommand NavigateSymbologyListPageCommand { get; }

		public MainPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;

			NavigateMapsListPageCommand = new DelegateCommand(() =>
			{
				_navigationService.NavigateAsync("MapsListPage");
			});

			NavigateLayersListPageCommand = new DelegateCommand(() =>
            {
	            _navigationService.NavigateAsync("LayersListPage");
            });

			NavigateSymbologyListPageCommand = new DelegateCommand(() =>
			{
				_navigationService.NavigateAsync("SymbologyListPage");
			});
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			if (parameters.ContainsKey("title"))
				Title = (string)parameters["title"] + " and Prism";
		}
	}
}

