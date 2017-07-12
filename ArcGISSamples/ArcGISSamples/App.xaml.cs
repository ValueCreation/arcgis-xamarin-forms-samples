using Prism.Unity;
using ArcGISSamples.Views;
using Xamarin.Forms;

namespace ArcGISSamples
{
	public partial class App : PrismApplication
	{
		public App(IPlatformInitializer initializer = null) : base(initializer) { }

		protected override void OnInitialized()
		{
			InitializeComponent();

			//NavigationService.NavigateAsync("MainPage?title=Hello%20from%20Xamarin.Forms");
			NavigationService.NavigateAsync("NavigationPage/MainPage");
		}

		protected override void RegisterTypes()
		{
			Container.RegisterTypeForNavigation<NavigationPage>();
			Container.RegisterTypeForNavigation<MainPage>();

			Container.RegisterTypeForNavigation<MapsListPage>();
            Container.RegisterTypeForNavigation<LayersListPage>();

		}
	}
}

