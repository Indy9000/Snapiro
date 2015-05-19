using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using System.IO;
using Android.Graphics;

namespace Snapiro.Droid
{
	[Activity (Label = "Snapiro.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		private Java.IO.File _file;

		private static int CAPTURE_IMAGE_ACTIVITY_REQUEST_CODE = 100;

		App _app = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			_app = new App ();
			LoadApplication (_app);
			//CreateDirectoryForPictures ();

			var filename = String.Format ("{0}.jpg", Guid.NewGuid ());
			_file = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), filename);

			var vm = _app.MainPage.BindingContext as MainViewModel;

			vm.ShouldTakePic = () => {
				var intent = new Intent(MediaStore.ActionImageCapture);
				var r = intent.Categories;

				intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));

				StartActivityForResult(intent, CAPTURE_IMAGE_ACTIVITY_REQUEST_CODE);
			};
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == CAPTURE_IMAGE_ACTIVITY_REQUEST_CODE) {
				if (resultCode == Result.Ok) {
					base.OnActivityResult (requestCode, resultCode, data);

					// make it available in the gallery
					var mediaScanIntent = new Intent (Intent.ActionMediaScannerScanFile);
					var contentUri = Android.Net.Uri.FromFile (_file);
					mediaScanIntent.SetData (contentUri);
					SendBroadcast (mediaScanIntent);

					//set it to the viewmodel
					var vm = _app.MainPage.BindingContext as MainViewModel;

					vm.ShowImage (()=>{
						var ms = ResizeImageAndroid (_file.AbsolutePath, 1024, 768);
						return ms;
					});
				}
			}
		}

		public MemoryStream ResizeImageAndroid (string filename, int width, int height)
		{
			var bitmap = LoadAndResizeBitmap (filename, width, height);

			var ms = new MemoryStream ();
			bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
			ms.Seek (0, SeekOrigin.Begin);
			return ms;
		}

		public Bitmap LoadAndResizeBitmap(string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight
					? outHeight / height
					: outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;

			var resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

			return resizedBitmap;
		}
	}
}

