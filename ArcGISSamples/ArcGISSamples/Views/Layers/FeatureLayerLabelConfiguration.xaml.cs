using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;

using Xamarin.Forms;

namespace ArcGISSamples.Views.Layers
{
    public partial class FeatureLayerLabelConfiguration : ContentPage
    {
        public FeatureLayerLabelConfiguration()
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
                15539982.3500528, 4257132.45180773, 15545386.0541707, 4262306.70411199,
                SpatialReferences.WebMercator);

            myMap.InitialViewpoint = new Viewpoint(initialLocation, 50000);

            // Create uri to the used feature service
            var serviceUri = new Uri(
                "https://services5.arcgis.com/HzGpeRqGvs5TMkVr/arcgis/rest/services/SampleData_LatLon/FeatureServer/0");

			var label = @"{ 
                            ""labelExpressionInfo"":
                        {
                            ""expression"": ""return 'ビル名:' + $feature.BuildingName;""
                        },
                        ""labelPlacement"": ""esriServerPointLabelPlacementBelowRight"",
                        ""symbol"": {
                             ""color"": [204,0,0,123],
                             ""font"": { ""size"": 16, ""weight"" : ""bold"" },
                             ""type"": ""esriTS""
                            }
                        }";

            FeatureLayer sampleLayer = new FeatureLayer(serviceUri){
                LabelsEnabled = true
            };

            var labelDefinition = LabelDefinition.FromJson(label);
            sampleLayer.LabelDefinitions.Add(labelDefinition);

            myMap.OperationalLayers.Add(sampleLayer);

			// Assign the map to the MapView
			MyMapView.Map = myMap;
		}
    }
}
