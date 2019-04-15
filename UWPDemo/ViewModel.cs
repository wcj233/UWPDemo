using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UWPDemo
{
    public class Picture
    {
        public string picturePath;
    }

    public class PictureModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private ObservableCollection<Picture> myPictures = new ObservableCollection<Picture>();
        public ObservableCollection<Picture> pictures {
            get { return this.myPictures; }
            set {
                this.myPictures = value;
                this.OnPropertyChanged();
            }
        }
        public PictureModel()
        {
            //this.myPictures.Add(new Picture { picturePath = "Assets/1.jpg" });
            //this.myPictures.Add(new Picture { picturePath = "Assets/2.jpg" });
            //this.myPictures.Add(new Picture { picturePath = "Assets/3.jpg" });
            this.myPictures.Add(new Picture { picturePath = "http://img15.3lian.com/2015/f3/18/d/27.jpg" });
            this.myPictures.Add(new Picture { picturePath = "http://img15.3lian.com/2015/f3/18/d/27.jpg" });
            this.myPictures.Add(new Picture { picturePath = "http://img15.3lian.com/2015/f3/18/d/27.jpg" });




        }


        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
