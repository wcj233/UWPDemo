using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            // Path to the file in the app package to launch
        //    Escape one:
        //    string imageFile = "Assets\\1.jpg";
        //    two
        //    string imageFile = @"Assets\1.jpg";

        //    var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(imageFile);

        //    if (file != null)
        //    {
        //        Launch the retrieved file
        //        var options = new Windows.System.LauncherOptions();
        //        options.DisplayApplicationPicker = true;

        //        Launch the retrieved file
        //        bool success = await Windows.System.Launcher.LaunchFileAsync(file, options);

        //        if (success)
        //        {
        //            File launched
        //        }
        //        else
        //        {
        //            File launch failed
        //        }
        //    }
        //    else
        //    {
        //        Could not find file
        //    }
        //    var uriA = new Uri("alsdk:");
        //    var promptOptions = new Windows.System.LauncherOptions();
        //    promptOptions.TreatAsUntrusted = true;
        //    promptOptions.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseMinimum;
        //    var success = await Launcher.LaunchUriAsync(uriA, promptOptions);
        //    if (success)
        //    {

        //    }
        //}

    }



        //common backgroundTask method
        public static BackgroundTaskRegistration RegisterBackgroundTask(
                                                string taskEntryPoint,
                                                string name,
                                                IBackgroundTrigger trigger,
                                                IBackgroundCondition condition)
        {
            //
            // Check for existing registrations of this background task.
            //

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == "ExampleBackgroundTask")
                {
                    //
                    // The task is already registered.
                    //
                    //cur.Value.Unregister(true);
                    return (BackgroundTaskRegistration)(cur.Value);
                }
            }

            //
            // Register the background task.
            //

            var builder = new BackgroundTaskBuilder();

            builder.Name = name;

            // in-process background tasks don't set TaskEntryPoint
            if (taskEntryPoint != null && taskEntryPoint != String.Empty)
            {
                builder.TaskEntryPoint = taskEntryPoint;
            }
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            return task;
        }

        //monitor progress
        private void OnProgress(IBackgroundTaskRegistration task, BackgroundTaskProgressEventArgs args) {
            var progress = "Progress: " + args.Progress + "%";
        }

        //out-process task finish notifi
        private void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {
            var settings = ApplicationData.Current.LocalSettings;
            var key = task.TaskId.ToString();
            var message = settings.Values[key].ToString();
            //UpdateUI(message);
        }

        async internal void DisplayImages(Windows.Storage.StorageFolder rootFolder)
        {
            // Display images from first folder in root\DCIM.
            var dcimFolder = await rootFolder.GetFolderAsync("DCIM");
            var folderList = await dcimFolder.GetFoldersAsync();
            var cameraFolder = folderList[0];
            var fileList = await cameraFolder.GetFilesAsync();
            for (int i = 0; i < fileList.Count; i++)
            {
                var file = (Windows.Storage.StorageFile)fileList[i];
                WriteMessageText(file.Name + "\n");
                DisplayImage(file, i);
            }
        }

        async private void DisplayImage(Windows.Storage.IStorageItem file, int index)
        {
            try
            {
                var sFile = (Windows.Storage.StorageFile)file;
                Windows.Storage.Streams.IRandomAccessStream imageStream =
                    await sFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                Windows.UI.Xaml.Media.Imaging.BitmapImage imageBitmap =
                    new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                imageBitmap.SetSource(imageStream);
                var element = new Image();
                element.Source = imageBitmap;
                element.Height = 100;
                Thickness margin = new Thickness();
                margin.Top = index * 100;
                element.Margin = margin;
                FilesCanvas.Children.Add(element);
            }
            catch (Exception e)
            {
                WriteMessageText(e.Message + "\n");
            }
        }

        // Write a message to MessageBlock on the UI thread.
        private Windows.UI.Core.CoreDispatcher messageDispatcher = Window.Current.CoreWindow.Dispatcher;

        private async void WriteMessageText(string message, bool overwrite = false)
        {
            await messageDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    if (overwrite)
                        FilesBlock.Text = message;
                    else
                        FilesBlock.Text += message;
                });
        }

        async internal void CopyImages(Windows.Storage.StorageFolder rootFolder)
        {
            // Copy images from first folder in root\DCIM.
            var dcimFolder = await rootFolder.GetFolderAsync("DCIM");
            var folderList = await dcimFolder.GetFoldersAsync();
            var cameraFolder = folderList[0];
            var fileList = await cameraFolder.GetFilesAsync();

            try
            {
                var folderName = "Images " + DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                Windows.Storage.StorageFolder imageFolder = await
                    Windows.Storage.KnownFolders.PicturesLibrary.CreateFolderAsync(folderName);

                foreach (Windows.Storage.IStorageItem file in fileList)
                {
                    CopyImage(file, imageFolder);
                }
            }
            catch (Exception e)
            {
                WriteMessageText("Failed to copy images.\n" + e.Message + "\n");
            }
        }

        async internal void CopyImage(Windows.Storage.IStorageItem file,
                                      Windows.Storage.StorageFolder imageFolder)
        {
            try
            {
                Windows.Storage.StorageFile sFile = (Windows.Storage.StorageFile)file;
                await sFile.CopyAsync(imageFolder, sFile.Name);
                WriteMessageText(sFile.Name + " copied.\n");
            }
            catch (Exception e)
            {
                WriteMessageText("Failed to copy file.\n" + e.Message + "\n");
            }
        }


    }
}
