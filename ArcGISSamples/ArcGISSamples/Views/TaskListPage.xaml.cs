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

			Content = new StackLayout
			{
				Margin = new Thickness(0, 20, 0, 0),
				Children = {
					POISearchButton
				}
			};

		}

		async void OnPOISearchButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Task.POISearchPage());
		}

    }
}
