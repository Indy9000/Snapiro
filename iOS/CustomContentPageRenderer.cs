using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly:ExportRenderer(typeof(ContentPage), typeof(CustomContentPageRenderer))]
public class CustomContentPageRenderer: PageRenderer
{
	public override void ViewDidAppear (bool animated)
	{
		base.ViewDidAppear (animated);
		var imagePicker = new UIImagePickerController { SourceType = UIImagePickerControllerSourceType.Camera };

		App.Instance.ShouldTakePicture += () => PresentViewController(imagePicker, true, null);
	}
}
