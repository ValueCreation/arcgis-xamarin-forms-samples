using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

using Xamarin.Forms;

namespace ArcGISSamples.Views.Layers
{
    public partial class FeatureLayerFilterPage : ContentPage
    {

	    //Create and hold reference to the feature layer
		private FeatureLayer _featureLayer;

        public FeatureLayerFilterPage()
        {
            InitializeComponent();
			//setup the control references and execute initialization 
			Initialize();
		}

		private async void Initialize()
		{
			// Create new Map with basemap
			Map myMap = new Map(Basemap.CreateTopographic());

			Envelope initialLocation = new Envelope(
				15587803.087799, 4238597.9999, 15602636.0001, 4257356.0001, SpatialReferences.WebMercator);

			myMap.InitialViewpoint = new Viewpoint(initialLocation);

			// Provide used Map to the MapView
			MyMapView.Map = myMap;

			// Create the uri for the feature service
			Uri featureServiceUri = new Uri(
			   "https://services.arcgis.com/wlVTGRSYTzAbjjiC/arcgis/rest/services/%E7%AE%A1%E7%90%86%E7%89%A9%E4%BB%B6_%E6%96%B0%E7%AF%89_%E4%B8%AD%E5%8F%A4/FeatureServer/0");

			// Initialize feature table using a url to feature server url
			ServiceFeatureTable featureTable = new ServiceFeatureTable(featureServiceUri);

			// Initialize a new feature layer based on the feature table
			_featureLayer = new FeatureLayer(featureTable);

			//Add the feature layer to the map
			myMap.OperationalLayers.Add(_featureLayer);

			// TODO: https://github.com/Esri/arcgis-runtime-samples-xamarin/issues/96
			if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Other)
			{
				await _featureLayer.RetryLoadAsync();
			}
		}

		private void OnApplyExpressionClicked(object sender, EventArgs e)
		{
            // Adding definition expression to show specific features only
            _featureLayer.DefinitionExpression = "新築_中古 = '中古マンション'";
		}

		private void OnResetButtonClicked(object sender, EventArgs e)
		{
			// Reset the definition expression to see all features again
			_featureLayer.DefinitionExpression = "";
		}
    }
}
