using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Xamarin.Forms;

namespace ArcGISSamples.Views.Layers
{
    public partial class BingMapsLayerPage : ContentPage
    {
        public BingMapsLayerPage()
        {
            InitializeComponent();

			Initialize();
		}

		private void Initialize()
		{

            Map myMap = new Map();

            var bingKey = "AtuE9JPKhNEUj037BMaockdj546C6UQLdK_5cv59kCxOjJoGXteSxqysRWZseWlM";

            var bingMapLayer = new BingMapsLayer(bingKey, BingMapsLayerStyle.Road);

			Envelope initialLocation = new Envelope(
				136.6, 35.72, 139.65, 35.68, SpatialReferences.Wgs84);

			myMap.InitialViewpoint = new Viewpoint(initialLocation);

            myMap.Basemap = new Basemap(bingMapLayer);

			MyMapView.Map = myMap;

        }
    }
}
