using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Snapiro
{
	public class MainViewModel:INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyChanged(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public MainViewModel ()
		{
		}

		ImageSource _image = null;
		public ImageSource Picture
		{
			get{ return _image;}
			set{ 
				if (_image != value) {
					_image = value;
					NotifyChanged ("Picture");
				}
			}
		}

		//This would be called by each platform implementation
		public void ShowImage(Func<System.IO.Stream> func)
		{
			Picture = ImageSource.FromStream (func);
		}

		//this would be set by each platform implementation
		public Action ShouldTakePic{ get; set;}

		Command _takePicCmd = null;
		public Command TakePicCmd
		{
			get{ 
				return _takePicCmd ?? new Command (o => {
					ShouldTakePic ();
				});
			}
		}
	}
}



