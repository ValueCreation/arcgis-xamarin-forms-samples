using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

using Xamarin.Forms;

namespace ArcGISSamples.Views.Data
{
    public partial class SearchFeatureTablePage : ContentPage
    {

        private ServiceFeatureTable _incidentsFeatureTable;

        public SearchFeatureTablePage()
        {
            InitializeComponent();

			Initialize();
        }

		private void Initialize()
		{
			// Create new Map with basemap
			Map myMap = new Map(Basemap.CreateTopographic());

			Envelope initialLocation = new Envelope(
				15587803.087799, 4238597.9999, 15602636.0001, 4257356.0001, SpatialReferences.WebMercator);

			myMap.InitialViewpoint = new Viewpoint(initialLocation);

			// Create uri to the used feature service
			var serviceUri = new Uri(
			   "https://services.arcgis.com/wlVTGRSYTzAbjjiC/arcgis/rest/services/%E7%AE%A1%E7%90%86%E7%89%A9%E4%BB%B6_%E6%96%B0%E7%AF%89_%E4%B8%AD%E5%8F%A4/FeatureServer/0");

			// Create feature table for the incident feature service
			_incidentsFeatureTable = new ServiceFeatureTable(serviceUri);

			// Define the request mode
			_incidentsFeatureTable.FeatureRequestMode = FeatureRequestMode.ManualCache;

			// When feature table is loaded, populate data
			_incidentsFeatureTable.LoadStatusChanged += OnLoadedPopulateData;

			// Create FeatureLayer that uses the created table
			FeatureLayer incidentsFeatureLayer = new FeatureLayer(_incidentsFeatureTable);

			// Add created layer to the map
			myMap.OperationalLayers.Add(incidentsFeatureLayer);

			// Assign the map to the MapView
			MyMapView.Map = myMap;
		}

		private async void OnLoadedPopulateData(object sender, Esri.ArcGISRuntime.LoadStatusEventArgs e)
		{
			// If layer isn't loaded, do nothing
			if (e.Status != Esri.ArcGISRuntime.LoadStatus.Loaded)
				return;

			// Create new query object that contains parameters to query specific request types
			QueryParameters queryParameters = new QueryParameters()
			{
				WhereClause = "新築_中古 = '中古マンション'"
			};

			// Create list of the fields that are returned from the service
			var outputFields = new string[] { "*" };

			// Populate feature table with the data based on query
			await _incidentsFeatureTable.PopulateFromServiceAsync(queryParameters, true, outputFields);
		}

    }
}
