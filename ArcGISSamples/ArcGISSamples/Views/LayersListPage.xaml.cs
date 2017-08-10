using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ArcGISSamples.Views
{
    public partial class LayersListPage : ContentPage
    {
        public LayersListPage()
        {
            InitializeComponent();

			var displayOsmButton = new Button
			{
				Text = "OpenStreetMap",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			displayOsmButton.Clicked += OnDisplayOsmClicked;

			var wmtsServiceButton = new Button
			{
				Text = "WMTS Service",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			wmtsServiceButton.Clicked += OnWmtsServiceClicked;

            var featureLayerLabelButton = new Button
			{
				Text = "フィーチャ レイヤーのラベル設定",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			featureLayerLabelButton.Clicked += OnFeatureLayerLabelButtonClicked;

			var webTiledLayerButton = new Button
			{
				Text = "WebTiledLayer",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			webTiledLayerButton.Clicked += OnWebTiledLayerButtonClicked;

			var bingMapLayerButton = new Button
			{
				Text = "BingMapLayer",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			bingMapLayerButton.Clicked += OnBingMapLayerButtonClicked;

			var FeatureLayerFilterButton = new Button
			{
				Text = "フィーチャのフィルタ設定",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.FromHex("ECECEC")
			};
			FeatureLayerFilterButton.Clicked += OnFeatureLayerFilterButtonClicked;


			Content = new StackLayout
			{
				Margin = new Thickness(0, 20, 0, 0),
				Children = {
					displayOsmButton,
					wmtsServiceButton,
                    featureLayerLabelButton,
                    webTiledLayerButton,
                    bingMapLayerButton,
                    FeatureLayerFilterButton
				}
			};        
        
        }

		async void OnDisplayOsmClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Layers.DisplayOsmMapPage());
		}

		async void OnWmtsServiceClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Layers.DisplayWMTSServicePage());
		}

        async void OnFeatureLayerLabelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Layers.FeatureLayerLabelConfiguration());           
        }

		async void OnWebTiledLayerButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Layers.WebTiledLayerPage());
		}

		async void OnBingMapLayerButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Layers.BingMapsLayerPage());
		}

		async void OnFeatureLayerFilterButtonClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new Layers.FeatureLayerFilterPage());
		}

		
	}
}
