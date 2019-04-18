using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
using Windows.Storage;
using Windows.ApplicationModel.Background;
using Microsoft.QueryStringDotNET;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace UWPDemo
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Boolean isInBackgroundMode = false;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            string familyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.EnteredBackground += AppEnteredBackground;
            this.LeavingBackground += AppLeavingBackground;
            Windows.System.MemoryManager.AppMemoryUsageLimitChanging += MemoryManager_AppMemoryUsageLimitChanging;
            Windows.System.MemoryManager.AppMemoryUsageIncreased += MemoryManager_AppMemoryUsageIncreased;
            //WriteTimestamp();
            register();
        }

        public async void register() {
            //BackgroundExecutionManager.RemoveAccess();
            await BackgroundExecutionManager.RequestAccessAsync();
        }

        //receive data
        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            base.OnShareTargetActivated(args);
            // Code to handle activation goes here.
            ShareOperation sp = args.ShareOperation;
            sp.ReportStarted();
            if (sp.Data.Contains(StandardDataFormats.Text)) {
                string text = await sp.Data.GetTextAsync();
            }
            if (sp.Data.Contains(StandardDataFormats.StorageItems))
            {
                IReadOnlyList <IStorageItem> files = await sp.Data.GetStorageItemsAsync();
            }

            if (sp.Data.Contains(StandardDataFormats.WebLink)) {

            }
            if (sp.Data.Contains(StandardDataFormats.ApplicationLink))
            {

            }
            if (sp.Data.Contains(StandardDataFormats.Html))
            {

            }
            if (sp.Data.Contains(StandardDataFormats.Bitmap))
            {

            }

        }

        async void ReportCompleted(ShareOperation shareOperation, string quickLinkId, string quickLinkTitle)
        {
            QuickLink quickLinkInfo = new QuickLink
            {
                Id = quickLinkId,
                Title = quickLinkTitle,

                // For quicklinks, the supported FileTypes and DataFormats are set 
                // independently from the manifest
                SupportedFileTypes = { "*" },
                SupportedDataFormats = { StandardDataFormats.Text,
                StandardDataFormats.Bitmap, StandardDataFormats.StorageItems }
            };

            StorageFile iconFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFileAsync(
                    "assets\\1.jpg", CreationCollisionOption.OpenIfExists);
            quickLinkInfo.Thumbnail = RandomAccessStreamReference.CreateFromFile(iconFile);
            shareOperation.ReportCompleted(quickLinkInfo);
        }

        //in-process background task
        //respond to your background trigger when it fires
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            //IBackgroundTaskInstance taskInstance = args.TaskInstance;
            //DoYourBackgroundWork(taskInstance);
            var deferral = args.TaskInstance.GetDeferral();

            if (args.TaskInstance.Task.Name is "ToastBackgroundTask2") {
                args.TaskInstance.Task.Unregister(true);
            }

            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children = {
                        new AdaptiveText(){
                            Text = "Pictures are updated."
                        }
                    }
                }
            };

            ToastActionsCustom actions = new ToastActionsCustom()
            {
                Buttons = {
                    new ToastButton("Back",new QueryString{
                        { "action","background" }
                    }.ToString()){
                        ActivationType = ToastActivationType.Background
                    },
                    new ToastButton("Fore",new QueryString{
                        { "action","foreground" }
                    }.ToString()){
                        ActivationType = ToastActivationType.Foreground
                    }
                }

            };

           
                //toast noti
                ToastContent toastContent = new ToastContent()
                {
                    Visual = visual,
                    Actions = actions
                };
            

                var toast = new ToastNotification(toastContent.GetXml());
                ToastNotificationManager.CreateToastNotifier().Show(toast);

            //switch (args.TaskInstance.Task.Name)
            //{
            //    case "ToastBackgroundTask":
            //        var details = args.TaskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            //        if (details != null)
            //        {
            //            string arguments = details.Argument;
            //            var userInput = details.UserInput;

            //            // Perform tasks
            //        }
            //        break;
            //}

            deferral.Complete();

        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            if (args.Verb == "show")
            {
                Frame rootFrame = (Frame)Window.Current.Content;
                Page4 page = (Page4)rootFrame.Content;

                // Call DisplayImages with root folder from camera storage.
                page.DisplayImages((Windows.Storage.StorageFolder)args.Files[0]);
            }
            else if (args.Verb == "copy")
            {
                Frame rootFrame = (Frame)Window.Current.Content;
                Page4 page = (Page4)rootFrame.Content;

                // Call CopyImages with root folder from camera storage.
                page.CopyImages((Windows.Storage.StorageFolder)args.Files[0]);
            }
            else {
                // TODO: Handle file activation
                // The number of files received is args.Files.Size
                // The name of the first file is args.Files[0].Name
                string name = args.Files[0].Name;
                var rootFrame = new Frame();
                rootFrame.Navigate(typeof(MainPage), args);
                Window.Current.Content = rootFrame;
                Window.Current.Activate();
            }

            
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null)
                frame = new Frame();
            Window.Current.Content = frame;
            if (args.Kind == ActivationKind.Protocol)
            {
                //no result
                ProtocolActivatedEventArgs eventArgs = args as ProtocolActivatedEventArgs;
                frame.Navigate(typeof(Page4), eventArgs.Uri);

            }
            else if (args.Kind == ActivationKind.ProtocolForResults)
            {
                //result
                ProtocolForResultsActivatedEventArgs eventArgs = args as ProtocolForResultsActivatedEventArgs;
                frame.Navigate(typeof(MainPage), eventArgs);
            }
            else if (args is ToastNotificationActivatedEventArgs) {
                ContentDialog contentDialog = new ContentDialog {
                    Title = "No wifi connection",
                    Content = "Check your connection and try again.",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await contentDialog.ShowAsync();

                var toastArgs = args as ToastNotificationActivatedEventArgs;
                QueryString qs = QueryString.Parse(toastArgs.Argument);

            }


            Window.Current.Activate();  
        }

        async void WriteTimestamp()
        {
            //StorageFolder testFolder = await StorageFolder.GetFolderFromPathAsync(@"C:\Intel\Logs");
            //StorageFile sourceFile = await testFolder.GetFileAsync("IntelGFX.log");
            //string name = sourceFile.Path;
            Windows.Storage.StorageFolder localFolder =
    Windows.Storage.ApplicationData.Current.LocalFolder;
            string name2 = localFolder.Path;
            Windows.Globalization.DateTimeFormatting.DateTimeFormatter formatter =
                new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("longtime");

            StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt",
                CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, formatter.Format(DateTimeOffset.Now));
        }

        async void ReadTimestamp()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync("dataFile.txt");
                String timestamp = await FileIO.ReadTextAsync(sampleFile);
                // Data is contained in timestamp
            }
            catch (Exception)
            {
                // Timestamp not found
            }
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(Page5), e.Arguments);
                    ReadTimestamp();
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void AppEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            isInBackgroundMode = true;
        }

        private void AppLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            isInBackgroundMode = false;

            // Restore view content if it was previously unloaded
            if (Window.Current.Content == null)
            {
                CreateRootFrame(ApplicationExecutionState.Running, string.Empty);
            }
        }

        private void MemoryManager_AppMemoryUsageLimitChanging(object sender, AppMemoryUsageLimitChangingEventArgs e)
        {
            // If app memory usage is over the limit, reduce usage within 2 seconds
            // so that the system does not suspend the app
            if (MemoryManager.AppMemoryUsage >= e.NewLimit)
            {
                ReduceMemoryUsage(e.NewLimit);
            }
        }

        private void MemoryManager_AppMemoryUsageIncreased(object sender, object e)
        {
            // Obtain the current usage level
            var level = MemoryManager.AppMemoryUsageLevel;

            // Check the usage level to determine whether reducing memory is necessary.
            // Memory usage may have been fine when initially entering the background but
            // the app may have increased its memory usage since then and will need to trim back.
            if (level == AppMemoryUsageLevel.OverLimit || level == AppMemoryUsageLevel.High)
            {
                ReduceMemoryUsage(MemoryManager.AppMemoryUsageLimit);
            }
        }

        public void ReduceMemoryUsage(ulong limit)
        {
            if (isInBackgroundMode && Window.Current.Content != null)
            {
               /* How you release memory depends on the specifics of your app, 
                * but one recommended way to free up memory is to dispose of your UI and 
                * the other resources associated with your app view. 
                * To do so,
                * ensure that you are running in the background state 
                * then set the Content property of your app's window to null 
                * and unregister your UI event handlers 
                * and remove any other references you may have to the page*/
                Window.Current.Content = null;
            }

            // Run the GC to collect released resources.
            GC.Collect();
        }

        //background->foreground
        private void CreateRootFrame(ApplicationExecutionState previousExecutionState, string arguments)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (previousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(Page4), arguments);
            }

        }
    }
}
