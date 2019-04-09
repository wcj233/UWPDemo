using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPDemo
{
    public class Picture {
        public string picturePath;
    }

    public class PictureModel {
        private ObservableCollection<Picture> myPictures = new ObservableCollection<Picture>();
        public ObservableCollection<Picture> pictures { get { return this.myPictures; } }
        public PictureModel(){
            this.pictures.Add(new Picture { picturePath = "Assets/1.jpg" });
            this.pictures.Add(new Picture { picturePath = "Assets/2.jpg" });
            this.pictures.Add(new Picture { picturePath = "Assets/3.jpg" });
        }
    }
}
