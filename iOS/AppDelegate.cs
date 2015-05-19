using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Snapiro.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		App _app = null;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			_app = new App ();
			LoadApplication (_app);

			var vm = _app.MainPage.BindingContext as MainViewModel;
			vm.ShouldTakePic = () => {
				PresentViewController(imagePicker, true, null);
			};

			return base.FinishedLaunching (app, options);
		}
	}
}

