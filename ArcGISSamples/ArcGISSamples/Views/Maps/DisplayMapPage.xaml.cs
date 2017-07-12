using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;
using Xamarin.Forms;

namespace ArcGISSamples.Views.Maps
{
    public partial class DisplayMapPage : ContentPage
    {
        public DisplayMapPage()
        {
            InitializeComponent();

			Initialize();
		}

		private void Initialize()
		{

			Map myMap = new Map(Basemap.CreateImagery());

            MyMapView.Map = myMap;

		}

    }
}
