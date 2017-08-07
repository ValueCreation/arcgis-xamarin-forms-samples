using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ArcGISSamples.Views
{
    public partial class DataListPage : ContentPage
    {
        public DataListPage()
        {
            InitializeComponent();

			var getRelateFeaturesButton = new Button
			{
				Text = "リレートテーブル取得",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			getRelateFeaturesButton.Clicked += OnGetRelateFeaturesButtonButtonClicked;

			Content = new StackLayout
			{
				Margin = new Thickness(0, 20, 0, 0),
				Children = {
					getRelateFeaturesButton
				}
			};

		}

		async void OnGetRelateFeaturesButtonButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Data.GetRelateFeaturesPage());
		}

    }
}
