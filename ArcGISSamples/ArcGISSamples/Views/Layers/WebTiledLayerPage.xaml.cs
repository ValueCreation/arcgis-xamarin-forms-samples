using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Xamarin.Forms;

namespace ArcGISSamples.Views.Layers
{
    public partial class WebTiledLayerPage : ContentPage
    {
        public WebTiledLayerPage()
        {
            InitializeComponent();

            Initialize();
        }

		private async void Initialize()
		{

			var WmtsUrl = new System.Uri("http://cyberjapandata.gsi.go.jp/xyz/std/{z}/{x}/{y}.png");

			var templateUri = "https://cyberjapandata.gsi.go.jp/xyz/std/{level}/{col}/{row}.png";

			//var templateUri = "https://{subDomain}.tile.stamen.com/terrain/{level}/{col}/{row}.png";
			//List<String> subDomains = new List<String> { "a", "b", "c", "d" };

			var webTiledLayer = new WebTiledLayer(templateUri);

			await webTiledLayer.LoadAsync();

            if (webTiledLayer.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
            {

                Map myMap = new Map(new Basemap(webTiledLayer));
				
                Envelope initialLocation = new Envelope(
					136.6, 35.72, 139.65, 35.68, SpatialReferences.Wgs84);

				myMap.InitialViewpoint = new Viewpoint(initialLocation);

                MyMapView.Map = myMap;


                   var test = ("Map tiles by <a href=\"http://stamen.com/\">Stamen Design</a>, " +
							"under <a href=\"http://creativecommons.org/licenses/by/3.0\">CC BY 3.0</a>. " +
							"Data by <a href=\"http://openstreetmap.org/\">OpenStreetMap</a>, " +
							"under <a href=\"http://creativecommons.org/licenses/by-sa/3.0\">CC BY SA</a>.");
                //var attribution = @"<a href=""http://maps.gsi.go.jp/development/ichiran.html"" target=""_blank"">地理院タイル</a>";
                webTiledLayer.Attribution = test;

            }
            else
            {
                // todo
            }


		}
    }
}
