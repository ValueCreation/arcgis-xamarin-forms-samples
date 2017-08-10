using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.UI;
using Xamarin.Forms;

namespace ArcGISSamples.Views.Task
{
    public partial class ReverseGeocodePage : ContentPage
    {
		// The LocatorTask provides geocoding services
		private LocatorTask _geocoder;

		// Service Uri to be provided to the LocatorTask (geocoder)
		private Uri _serviceUri = new Uri("https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer");

		public ReverseGeocodePage()
        {
            InitializeComponent();

			// Create the UI, setup the control references and execute initialization
			Initialize();

			MyMapView.GeoViewTapped += MyMapView_GeoViewTapped;
        }

		private async void Initialize()
		{
			// Create new Map with basemap
			Map myMap = new Map(Basemap.CreateStreets());

			Envelope initialLocation = new Envelope(
				136.6, 35.72, 139.65, 35.68, SpatialReferences.Wgs84);

			myMap.InitialViewpoint = new Viewpoint(initialLocation);

			// Assign the map to the MapView
			MyMapView.Map = myMap;

			// Initialize the LocatorTask with the provided service Uri
			_geocoder = await LocatorTask.CreateAsync(_serviceUri);

		}

		private async void MyMapView_GeoViewTapped(object sender, Esri.ArcGISRuntime.Xamarin.Forms.GeoViewInputEventArgs e)
		{
			// Search for the graphics underneath the user's tap
			IReadOnlyList<IdentifyGraphicsOverlayResult> results = await MyMapView.IdentifyGraphicsOverlaysAsync(e.Position, 12, false);

			// Return gracefully if there was no result
			//if (results.Count < 1 || results.First().Graphics.Count < 1) { return; }

			// Reverse geocode to get addresses
			IReadOnlyList<GeocodeResult> addresses = await _geocoder.ReverseGeocodeAsync(e.Location);

			// Get the first result
			GeocodeResult address = addresses.First();
			// Use the city and region for the Callout Title
			//String calloutTitle = address.Attributes["City"] + ", " + address.Attributes["Region"];
			
            String calloutTitle = "日本住所";

			// Use the metro area for the Callout Detail
			String calloutDetail = address.Attributes["Address"].ToString();

			// Use the MapView to convert from the on-screen location to the on-map location
			MapPoint point = MyMapView.ScreenToLocation(e.Position);

			// Define the callout
			CalloutDefinition calloutBody = new CalloutDefinition(calloutTitle, calloutDetail);

			// Show the callout on the map at the tapped location
			MyMapView.ShowCalloutAt(point, calloutBody);
		
        }
    }
}
