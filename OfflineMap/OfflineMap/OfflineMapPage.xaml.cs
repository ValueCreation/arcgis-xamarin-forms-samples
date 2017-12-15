using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Tasks;
using Esri.ArcGISRuntime.Tasks.Offline;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Esri.ArcGISRuntime.ArcGISServices;
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Xamarin.Forms;

#if WINDOWS_UWP
using Colors = Windows.UI.Colors;
#else

using Colors = System.Drawing.Color;

#endif

namespace OfflineMap
{
    public partial class OfflineMapPage : ContentPage
    {
        private enum EditState
        {
            NotReady, // ジオデータベースは、生成されていない
            Ready // ジオデータベースは、同期または追加編集の準備ができている
        }

        // ArcGIS Online フィーチャ レイヤーサービスの URL
        private Uri FEATURELAYER_SERVICE_URL = new Uri("https://services5.arcgis.com/HzGpeRqGvs5TMkVr/arcgis/rest/services/SampleData_LatLon/FeatureServer/");

        // ローカルジオデータベースのパス
        private string _gdbPath;

        // ジオデータベースとの編集・同期に使用する
        private GeodatabaseSyncTask _gdbSyncTask;

        // ジオデータベース
        private Geodatabase _resultGdb;

        private GeodatabaseFeatureTable mGdbFeatureTable;

        // 編集プロセスのどの段階にいるかを示すフラグ
        private EditState _readyForEdits = EditState.NotReady;

        public OfflineMapPage()
        {
            InitializeComponent();

            Initialize();
        }

        private async void Initialize()
        {
            // タイル キャッシュをを読み込む
            TileCache tileCache = new TileCache(GetTpkPath());

            // タイル キャッシュレイヤーの作成
            ArcGISTiledLayer tileLayer = new ArcGISTiledLayer(tileCache);

            // ベースマップクラスにタイル キャッシュ レイヤーを設定
            Basemap sfBasemap = new Basemap(tileLayer);

            // マップにタイル キャッシュ レイヤーのベースマップを設定
            Map myMap = new Map(sfBasemap);

            Envelope initialLocation = new Envelope(
                15539982.3500528, 4255968.158699, 15545870.466201, 4262306.70411199,
                SpatialReferences.WebMercator);
            
            myMap.InitialViewpoint = new Viewpoint(initialLocation);

            // MapView に作成したマップを設定
            myMapView.Map = myMap;

            // ジオデータベース ファイルのローカルデータパスを取得
            _gdbPath = GetGdbPath();

            // ジオデータベースを生成するためのタスクを作成する
            _gdbSyncTask = await GeodatabaseSyncTask.CreateAsync(FEATURELAYER_SERVICE_URL);

            // サービスから地図にフィーチャ レイヤーを追加する
            foreach (IdInfo layer in _gdbSyncTask.ServiceInfo.LayerInfos)
            {
                Uri onlineTableUri = new Uri(FEATURELAYER_SERVICE_URL + "/" + layer.Id);

                // ServiceFeatureTableを作成する
                ServiceFeatureTable onlineTable = new ServiceFeatureTable(onlineTableUri);

                await onlineTable.LoadAsync();

                // ロードが成功した場合は、マップの操作レイヤーにレイヤーを追加
                if (onlineTable.LoadStatus == Esri.ArcGISRuntime.LoadStatus.Loaded)
                {
                    myMap.OperationalLayers.Add(new FeatureLayer(onlineTable));
                }
            }

        }

        private void GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            if (_readyForEdits == EditState.NotReady) { return; }

            // 新しいポイントデータを作成
            var mapClickPoint = e.Location;

            MapPoint wgs84Point = (MapPoint)GeometryEngine.Project(mapClickPoint, SpatialReferences.Wgs84);

            // 新しいポイントデータを追加する
            addFeature(wgs84Point);

            // 編集状態を更新する
            _readyForEdits = EditState.Ready;

            // 同期ボタンを有効にする
            mySyncButton.IsEnabled = true;
        }

        private async void addFeature(MapPoint pPoint) {
            // 新しいポイントデータを作成して mGdbFeatureTable に反映する
            var attributes = new Dictionary<string, object>();
            attributes.Add("BuildingName", "Xamarin はいいぞ！");
            attributes.Add("Age", 10);

            Feature addedFeature = mGdbFeatureTable.CreateFeature(attributes, pPoint);

            try {
                await mGdbFeatureTable.AddFeatureAsync(addedFeature);
            }
            catch (Exception ex)
            {
                DisplayAlert("ポイント追加", ex.Message, "OK");
            }
            			
        }

        // GeoDatabase を新規に作成する
        private void GenerateButton_Clicked(object sender, EventArgs e)
        {
            // GeoDatabase の作成
            StartGeodatabaseGeneration();
        }

        private async void StartGeodatabaseGeneration()
        {
            // 同期させたいレイヤーで ジオデータベースタスク オブジェクトを作成する (GeodatabaseSyncTask)
            _gdbSyncTask = await GeodatabaseSyncTask.CreateAsync(FEATURELAYER_SERVICE_URL);

            // ジオデータベース作成のためのパラメータを取得する
            Envelope extent = myMapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry).TargetGeometry as Envelope;

            // ジオデータベース作成　タスクオブジェクトのデフォルトパラメータを取得する
            GenerateGeodatabaseParameters generateParams = await _gdbSyncTask.CreateDefaultGenerateGeodatabaseParametersAsync(extent);

            // ジオデータベースの作成ジョブオブジェクトを作成する
            GenerateGeodatabaseJob generateGdbJob = _gdbSyncTask.GenerateGeodatabase(generateParams, _gdbPath);

            // ジョブ変更イベントを処理する
            generateGdbJob.JobChanged += GenerateGdbJobChanged;

            //進行状況を変更したイベントをインライン（ラムダ）関数で処理してプログレスバーを表示する
            generateGdbJob.ProgressChanged += ((object sender, EventArgs e) =>
            {
                // ジョブを取得
                GenerateGeodatabaseJob job = sender as GenerateGeodatabaseJob;

                // プログレスバーの更新
                UpdateProgressBar(job.Progress);
            });

            // ジオデータベース作成のジョブをスタートする
            generateGdbJob.Start();
        }

        // Handler for the job changed event
        private void GenerateGdbJobChanged(object sender, EventArgs e)
        {
            //ジョブオブジェクトを取得します。 HandleGenerationStatusChangeに渡されます。
            GenerateGeodatabaseJob job = sender as GenerateGeodatabaseJob;

            // スレッド化された実装の性質上、
            // UI と対話するためにディスパッチャを使用する必要がある
            Device.BeginInvokeOnMainThread(() =>
            {
                // ジョブが終了したらプログレスバーを非表示にする
                if (job.Status == JobStatus.Succeeded || job.Status == JobStatus.Failed)
                {
                    myProgressBar.IsVisible = false;
                }
                else 
                {
                    myProgressBar.IsVisible = true;
                }

                // ジョブステータスの残りの部分を変更
                HandleGenerationStatusChange(job);
            });
        }

        private async void HandleGenerationStatusChange(GenerateGeodatabaseJob job)
        {
            JobStatus status = job.Status;

            // Job が成功したら作成した ローカル ジオデータベース をマップに追加する
            if (status == JobStatus.Succeeded)
            {
                // 既存のレイヤーをクリア
                //myMapView.Map.OperationalLayers.Clear();
                myMapView.Map.OperationalLayers.RemoveAt(0);

                // 新しいローカル ジオデータベース を取得
                _resultGdb = await job.GetResultAsync();

                mGdbFeatureTable = _resultGdb.GeodatabaseFeatureTables.FirstOrDefault();

                // テーブから新しいフィーチャ レイヤを作成する
                FeatureLayer layer = new FeatureLayer(mGdbFeatureTable);

                // 新しいレイヤーをマップに追加する
                myMapView.Map.OperationalLayers.Add(layer);

                // 編集機能を有効にする
                _readyForEdits = EditState.Ready;
            }

            // ジオデータベースの作成ジョブに失敗した時
            if (status == JobStatus.Failed)
            {
                // エラーメッセージの作成
                string message = "ジオデータベースの作成に失敗";

                // エラーメッセージを表示する（存在する場合）
                if (job.Error != null)
                {
                    message += ": " + job.Error.Message;
                }
                else
                {
                    // エラーがなければ、ジョブからのメッセージを表示する
                    foreach (JobMessage m in job.Messages)
                    {
                        // JobMessage からテキストを取得し、出力文字列に追加します
                        message += "\n" + m.Message;
                    }
                }

                // メッセージを表示する
                ShowStatusMessage(message);
            }
        }

        // ArcGIS Online との同期を行う
        private void SyncButton_Click(object sender, EventArgs e)
        {
            SyncGeodatabase();
        }

        private void SyncGeodatabase()
        {
            // Return if not ready
            if (_readyForEdits != EditState.Ready) { return; }

            // 同期タスクのパラメータを作成する
            SyncGeodatabaseParameters parameters = new SyncGeodatabaseParameters()
            {
                GeodatabaseSyncDirection = SyncDirection.Bidirectional,
                RollbackOnFailure = false
            };

            // ジオデータベース内の各フィーチャテーブルのレイヤー ID を取得してから、同期ジョブに追加する
            foreach (GeodatabaseFeatureTable table in _resultGdb.GeodatabaseFeatureTables)
            {
                // レイヤーのIDを取得する
                long id = table.ServiceLayerId;

                // CSyncLayerOption を作成する
                SyncLayerOption option = new SyncLayerOption(id);

                // オプションを追加する
                parameters.LayerOptions.Add(option);
            }

            // ジョブを作成する
            SyncGeodatabaseJob job = _gdbSyncTask.SyncGeodatabase(parameters, _resultGdb);

            // ステータス更新
            job.JobChanged += Job_JobChanged;

            // プログレスバーの更新
            job.ProgressChanged += Job_ProgressChanged;

            // 同期を開始する
            job.Start();
        }

        private void Job_JobChanged(object sender, EventArgs e)
        {
            // ジョブオブジェクトを取得します。 HandleGenerationStatusChangeに渡されます。
            SyncGeodatabaseJob job = sender as SyncGeodatabaseJob;

            // スレッド化された実装の性質上、
            // UIと対話するためにディスパッチャを使用する必要がある
            Device.BeginInvokeOnMainThread(() =>
            {
                // 適切にプログレスバーを更新する
                if (job.Status == JobStatus.Succeeded || job.Status == JobStatus.Failed)
                {
                    // プログレスバーの値を更新する
                    UpdateProgressBar(0);

                    // プログレスバーを非表示にする
                    myProgressBar.IsVisible = false;
                }
                else
                {
                    // プログレスバーの値を更新する
                    UpdateProgressBar(job.Progress);

                    // プログレスバーを表示する
                    myProgressBar.IsVisible = true;
                }

                // /ジョブステータスの残りの部分を変更するか
                HandleSyncStatusChange(job);
            });
        }

        private void HandleSyncStatusChange(SyncGeodatabaseJob job)
        {
            JobStatus status = job.Status;

            // ユーザーにジョブの完了を伝える
            if (status == JobStatus.Succeeded)
            {
                ShowStatusMessage("ジオデータベースの同期が完了しました。");
            }

            // ジョブが失敗したかどうかを確認する
            if (status == JobStatus.Failed)
            {
                // ユーザーを表示するメッセージを作成する
                string message = "ジオデータベースの同期に失敗";

                // エラーメッセージを表示する（存在する場合）
                if (job.Error != null)
                {
                    message += ": " + job.Error.Message;
                }
                else
                {
                    // エラーがなければ、ジョブからのメッセージを表示する
                    foreach (JobMessage m in job.Messages)
                    {
                        // JobMessageからテキストを取得し、出力文字列に追加
                        message += "\n" + m.Message;
                    }
                }

                // メッセージを表示する
                ShowStatusMessage(message);
            }
        }

  
        private void Job_ProgressChanged(object sender, EventArgs e)
        {
            // ジョブオブジェクトを取得する
            SyncGeodatabaseJob job = sender as SyncGeodatabaseJob;

            // プログレスバーを更新する
            UpdateProgressBar(job.Progress);
        }

        private void UpdateProgressBar(int progress)
        {
            // スレッド化された実装の性質上、
            // UIと対話するためにディスパッチャを使用する必要がある
            Device.BeginInvokeOnMainThread(() =>
            {
                // プログレスバーの値を更新する
                myProgressBar.Progress = progress / 100.0;
            });
        }

        private void ShowStatusMessage(string message)
        {
            // ユーザーにメッセージを表示する
            DisplayAlert("同期", message, "OK");
        }

        // タイル パッケージのパスを取得する
        private string GetTpkPath()
        {
            #region offlinedata
            // タイル パッケージ 
            string filename = "xamarin.tpk";
            // ディレクトリを取得
            var folder =
#if NETFX_CORE
                Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#elif __ANDROID__
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
#elif __IOS__
                "/Library/";
#endif
            // Return the full path; Item ID is 3f1bbf0ec70b409a975f5c91f363fe7d
            return Path.Combine(folder, "sampleData", "EditAndSyncFeatures", filename);
            #endregion offlinedata
        }

        private string GetGdbPath()
        {

            // ジオデータベースを格納するためのプラットフォーム固有のパスを設定する
            String folder = "";

#if NETFX_CORE //UWP
            folder = Windows.Storage.ApplicationData.Current.LocalFolder.Path.ToString();
#elif __IOS__
            folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif __ANDROID__
            folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif
            // パスを設定
            return Path.Combine(folder, "wildfire.geodatabase");
        }

    }
}
