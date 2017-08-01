using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;

using Xamarin.Forms;

namespace ArcGISSamples.Views.Symbology
{
    public partial class HeatmapRendererMyPage : ContentPage
    {
        public HeatmapRendererMyPage()
        {
            InitializeComponent();
            Initialize();
		}

		private void Initialize()
		{
			// Create new Map with basemap
			Map myMap = new Map(Basemap.CreateStreets());

			// Create and set initial map location
			Envelope initialLocation = new Envelope(
				138.66, 36.94, 140.86, 34.77,
                SpatialReferences.Wgs84);
            
			myMap.InitialViewpoint = new Viewpoint(initialLocation);

			// Create uri to the used feature service
			var serviceUri = new Uri(
				"https://services5.arcgis.com/HzGpeRqGvs5TMkVr/arcgis/rest/services/minatokushisetsujoho_201606/FeatureServer/0");


            FeatureLayer sampleLayer = new FeatureLayer(serviceUri);

			myMap.OperationalLayers.Add(sampleLayer);

			// Assign the map to the MapView
			MyMapView.Map = myMap;
		}
    }
}
