using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Portal;

using Xamarin.Forms;

namespace ArcGISSamples.Views.Data
{
    public partial class GetRelateFeaturesPage : ContentPage
    {

        Credential credential = null;
        private ServiceFeatureTable relateFeatureTable;
        private FeatureLayer relauteFeatureLayer;

        private const string SecureMapServiceUrl = "https://services.arcgis.com/wlVTGRSYTzAbjjiC/ArcGIS/rest/services/for_runtime_test/FeatureServer/1";
        //private const string SecureMapServiceUrl = "https://sampleserver6.arcgisonline.com/arcgis/rest/services/ServiceRequest/FeatureServer/0";

        private const string SecureLayerName = "鷗_ail_point_fc_須";

        public GetRelateFeaturesPage()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {

			AuthenticationManager.Current.ChallengeHandler = new ChallengeHandler(CreateKnownCredentials);

			MyMapView.GeoViewTapped += OnMapViewTapped;

            Map myMap = new Map(Basemap.CreateStreets());

			// Create feature table for the incident feature service
			relateFeatureTable = new ServiceFeatureTable(new Uri(SecureMapServiceUrl));
            //relateFeatureTable.FeatureRequestMode = FeatureRequestMode.ManualCache;
            relateFeatureTable.Credential = credential;


            relauteFeatureLayer = new FeatureLayer(relateFeatureTable);
            relauteFeatureLayer.Name = SecureLayerName;
			
            myMap.OperationalLayers.Add(relauteFeatureLayer);


			SecureLayerStatusPanel.BindingContext = relauteFeatureLayer;

            var relateTable = new ServiceFeatureTable(new Uri("https://services.arcgis.com/wlVTGRSYTzAbjjiC/ArcGIS/rest/services/for_runtime_test/FeatureServer/4"));
			//var relateTable = new ServiceFeatureTable(new Uri("https://sampleserver6.arcgisonline.com/arcgis/rest/services/ServiceRequest/FeatureServer/1"));

			myMap.Tables.Add(relateTable);

            Envelope initialLocation = new Envelope(
                15553110.387026, 4256755.33486891, 15561439.4870176, 4260269.00856292, SpatialReferences.WebMercator);

			//Envelope initialLocation = new Envelope(
	        //    -9814091.251948113, 5127179.36590259, -9812874.166180609, 5128639.868823595, SpatialReferences.WebMercator);

            myMap.InitialViewpoint = new Viewpoint(initialLocation);

            MyMapView.Map = myMap;

        }


        private async void OnMapViewTapped(object sender, Esri.ArcGISRuntime.Xamarin.Forms.GeoViewInputEventArgs e)
        {
            try
            {

                IdentifyLayerResult idResult = await MyMapView.IdentifyLayerAsync(relauteFeatureLayer, e.Position, 5, false);
				ArcGISFeature serviceRequestFeature = idResult.GeoElements.FirstOrDefault() as ArcGISFeature;

                if (serviceRequestFeature == null) { return; }

                ArcGISFeatureTable serviceRequestTable = serviceRequestFeature.FeatureTable as ArcGISFeatureTable;

                IReadOnlyList<ArcGISFeatureTable> relatedTables = serviceRequestTable.GetRelatedTables();

	            if (relatedTables.Count > 0)
	            {
                    // Get the comments from the relationship results
                    ArcGISFeatureTable relatedComments = relatedTables.FirstOrDefault();

                    await relatedComments.LoadAsync();
                    if (relatedComments.LoadStatus == LoadStatus.Loaded)
                    {

						ArcGISFeature newComment = relatedComments.CreateFeature() as ArcGISFeature;
						
                        newComment.Attributes["鷗_FLN_TEXT_須"] = "Please show up on time!";

						// Relate the selected service request to the new comment
						serviceRequestFeature.RelateFeature(newComment);


						/*
                        var getUpdatedFeature = await relatedComments.GetUpdatedFeaturesAsync();
                        ArcGISFeature test = (ArcGISFeature)getUpdatedFeature.FirstOrDefault();

                        await test.LoadAsync();
                        test.Attributes["鷗_FLN_TEXT_須"] = "鷗_JPSTRING_須 | 神谷";

						await relatedComments.UpdateFeatureAsync(test);
						serviceRequestFeature.RelateFeature(test);

                        ArcGISFeature newComment = relatedComments.CreateFeature() as ArcGISFeature;
                        */
						//serviceRequestFeature.RelateFeature(f);
					}

	            }

			}
            catch (Exception ex)
            {
                await DisplayAlert("検索のエラー", ex.ToString(), "OK");
            }

        }

		private async Task<Credential> CreateKnownCredentials(CredentialRequestInfo info)
        {

            Credential credential = null;

            try
            {
                // ArcGIS Online / Portal for ArcGIS へ接続して認証情報を取得
                info.ServiceUri = new Uri("https://www.arcgis.com/sharing/rest");


                // Username and password is hard-coded for this resource
                // (Would be better to read them from a secure source)
                string username = "kamiya";
                string password = "kamiya123";

                // Create a credential for this resource
                credential = await AuthenticationManager.Current.GenerateCredentialAsync
                                        (info.ServiceUri,
                                         username,
                                         password,
                                         info.GenerateTokenOptions);

                ArcGISPortal arcgisPortal = await ArcGISPortal.CreateAsync(info.ServiceUri, credential);
                Esri.ArcGISRuntime.LicenseInfo licenseInfo = arcgisPortal.PortalInfo.LicenseInfo;

                // ArcGIS Runtime にライセンスを設定
                Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.SetLicense(licenseInfo);

            }
            catch (Exception ex)
            {
                DisplayAlert("Credential Error", "Access to " + info.ServiceUri.AbsoluteUri + " denied. " + ex.Message, "OK");
            }
            // Return the credential
			return credential;
		}
    }
}
