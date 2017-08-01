using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ArcGISSamples.Views
{
    public partial class SymbologyListPage : ContentPage
    {
        public SymbologyListPage()
        {
            InitializeComponent();

			var heatmapRendererButton = new Button
			{
				Text = "ヒートマップ表示",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			heatmapRendererButton.Clicked += OnHeatmapRendererButtonClicked;

			Content = new StackLayout
			{
				Margin = new Thickness(0, 20, 0, 0),
				Children = {
					heatmapRendererButton
				}
			};

		}

		async void OnHeatmapRendererButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Symbology.HeatmapRendererMyPage());
		}

	}
}
