using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ArcGISSamples.Views
{
    public partial class TaskListPage : ContentPage
    {
        public TaskListPage()
        {
            InitializeComponent();

			var POISearchButton = new Button
			{
				Text = "POIの検索",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			POISearchButton.Clicked += OnPOISearchButtonClicked;

			var FindAdressButton = new Button
			{
				Text = "住所の検索",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			FindAdressButton.Clicked += OnFindAdressButtonClicked;

			var ReverseGeocodeButton = new Button
			{
				Text = "リバースジオコーディング",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			ReverseGeocodeButton.Clicked += OnReverseGeocodeButtonClicked;

			Content = new StackLayout
			{
				Margin = new Thickness(0, 20, 0, 0),
				Children = {
					POISearchButton,
                    FindAdressButton,
                    ReverseGeocodeButton
				}
			};

		}

		async void OnPOISearchButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Task.POISearchPage());
		}

		async void OnFindAdressButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Task.FindAddressPage());
		}

		async void OnReverseGeocodeButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Task.ReverseGeocodePage());
		}
    }
}
