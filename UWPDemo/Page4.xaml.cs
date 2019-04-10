using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page4 : Page
    {
        DispatcherTimer dispatcherTimer;

        public int[] getRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int tmp = 0;
            int sum = 0;
            for (int i = 0; i <= num - 2; i++)
            {
                tmp = ra.Next(minValue, maxValue); //随机取数
                arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra); //取出值赋到数组中
                sum += tmp;
            }
            arrNum[2] = 3 - sum;
            return arrNum;
        }

        public int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int id = Array.IndexOf(arrNum, tmp);
            if (id != -1)
            {
                tmp = ra.Next(minValue, maxValue);
                getNum(arrNum, tmp, minValue, maxValue, ra);
            }

            return tmp;
        }
        public Page4()
        {
            this.InitializeComponent();
            this.ViewModel = new PictureModel();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(10);
            dispatcherTimer.Start();

        }
         public PictureModel ViewModel { get; set; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void dispatcherTimer_Tick(object sender, object e) {
            int[] arr = getRandomNum(3, 0, 3);
            string[] imagePath = { "Assets/1.jpg", "Assets/2.jpg", "Assets/3.jpg" };
            ObservableCollection<Picture> newPictures = new ObservableCollection<Picture>();
            newPictures.Add(new Picture { picturePath = "Assets/2.jpg" });
            newPictures.Add(new Picture { picturePath = "Assets/1.jpg" });
            newPictures.Add(new Picture { picturePath = "Assets/3.jpg" });
            //newPictures = this.ViewModel.pictures;
            //this.ViewModel.pictures[0].picturePath = "Assets/2.jpg";
            //this.ViewModel.pictures[1].picturePath = "Assets/1.jpg";
            //this.ViewModel.pictures[2].picturePath = "Assets/3.jpg";
            this.ViewModel.pictures = newPictures;
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            
            var uriA = new Uri("alsdk:");
            var promptOptions = new Windows.System.LauncherOptions();
            promptOptions.TreatAsUntrusted = true;
            promptOptions.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseMinimum;
            var success = await Launcher.LaunchUriAsync(uriA,promptOptions);
            if (success) {

            }
        }
    }
}
