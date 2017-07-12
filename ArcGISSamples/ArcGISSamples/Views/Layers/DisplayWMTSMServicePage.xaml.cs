using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Ogc;
using Esri.ArcGISRuntime.Geometry;

using Xamarin.Forms;

namespace ArcGISSamples.Views.Layers
{
    public partial class DisplayWMTSServicePage : ContentPage
    {
        public DisplayWMTSServicePage()
        {
            InitializeComponent();

			Initialize();
		}

        private async void Initialize()
		{
            
            Map myMap = new Map();

            Envelope initialLocation = new Envelope(
				136.6, 35.72, 139.65, 35.68, SpatialReferences.Wgs84);

			myMap.InitialViewpoint = new Viewpoint(initialLocation);

			// create a WMTS service from a URL
			//var WmtsUrl = new System.Uri("https://esrijapan.github.io/gsi-wmts/chizu_shashin.xml");
            var WmtsUrl = new System.Uri("https://gbank.gsj.jp/seamless/tilemap/basic/WMTSCapabilities.xml");

			WmtsService wmtsService = new WmtsService(WmtsUrl);

            await wmtsService.LoadAsync();

            if (wmtsService.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
            {

                WmtsServiceInfo wmtsServiceInfo = wmtsService.ServiceInfo;
                IReadOnlyList<WmtsLayerInfo> layerInfos = wmtsServiceInfo.LayerInfos;

                WmtsLayer wmtsLayer = new WmtsLayer(layerInfos[0]);
                //WmtsLayer wmtsLayer = new WmtsLayer();

                myMap.Basemap = new Basemap(wmtsLayer);

            }
            else
            {
                // todo
            }

			MyMapView.Map = myMap;

        }
    }
}
