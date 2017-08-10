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

			var searchFeatureTableButton = new Button
			{
				Text = "フィーチャーの属性検索",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			searchFeatureTableButton.Clicked += OnSearchFeatureTableButtonClicked;


			Content = new StackLayout
			{
				Margin = new Thickness(0, 20, 0, 0),
				Children = {
					getRelateFeaturesButton,
                    searchFeatureTableButton
				}
			};

		}

		async void OnGetRelateFeaturesButtonButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Data.GetRelateFeaturesPage());
		}

		async void OnSearchFeatureTableButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Data.SearchFeatureTablePage());
		}

	}
}
