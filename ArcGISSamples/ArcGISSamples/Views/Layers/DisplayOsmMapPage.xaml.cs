using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Xamarin.Forms;

namespace ArcGISSamples.Views.Layers
{
    public partial class DisplayOsmMapPage : ContentPage
    {
        public DisplayOsmMapPage()
        {
            InitializeComponent();

             Initialize();
		}

		private void Initialize()
		{

            Map myMap = new Map(Basemap.CreateOpenStreetMap());

            Envelope initialLocation = new Envelope(
                136.6, 35.72, 139.65, 35.68, SpatialReferences.Wgs84);
            
			myMap.InitialViewpoint = new Viewpoint(initialLocation);

			MyMapView.Map = myMap;

		}

	}
}
