using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page5 : Page
    {
        public Page5()
        {
            this.InitializeComponent();
            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            // Set the value of the badge in the XML to our number
            XmlElement badgeElement = badgeXml.SelectSingleNode("/badge") as XmlElement;
            badgeElement.SetAttribute("value", "2");
            // Create the badge notification
            BadgeNotification badge = new BadgeNotification(badgeXml);
            // Create the badge updater for the application
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            // And update the badge
            badgeUpdater.Update(badge);
            //inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen;
            //InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            //drawingAttributes.Color = Windows.UI.Colors.Black;
            //drawingAttributes.IgnorePressure = false;
            //drawingAttributes.FitToCurve = true;
            //inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);

            //touchRectangle.PointerPressed += touchRectangle_PointerPressed;
            //touchRectangle.PointerReleased += touchRectangle_PointerReleased;
            //touchRectangle.PointerExited += touchRectangle_PointerExited;
            //touchRectangle.ManipulationDelta += TouchRectangle_ManipulationDelta;
            //touchRectangle.ManipulationStarted += TouchRectangle_ManipulationStarted;
            //touchRectangle.ManipulationStarting += TouchRectangle_ManipulationStarting;

            //RegisterBackgroundTask(null, "ToastBackgroundTask2", null, null);

            //var notification = new TileNotification(content.GetXml());
            //TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

     

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Custom text here"; // Sets custom UI text
            //e.DragUIOverride.SetContentFromBitmapImage(null); // Sets a custom glyph
            e.DragUIOverride.IsCaptionVisible = true; // Sets if the caption is visible
            e.DragUIOverride.IsContentVisible = true; // Sets if the dragged content is visible
            e.DragUIOverride.IsGlyphVisible = true; // Sets if the glyph is visibile
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    var storageFile = items[0] as StorageFile;
                    string path = storageFile.Path;
                    string name = storageFile.Name;
                    var thumbnail = await storageFile.GetScaledImageAsThumbnailAsync(ThumbnailMode.DocumentsView);
                    BitmapImage bitmapImage = new BitmapImage();
                    InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                    await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
                    randomAccessStream.Seek(0);
                    bitmapImage.SetSource(randomAccessStream);
                    Image.Source = bitmapImage;
                    fileInfo.Text = name + "    " + path;

                    //var bitmapImage = new BitmapImage();
                    //bitmapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
                    // Set the image on the main page to the dropped image
                    //Image.Source = bitmapImage;
                }
            }
        }

        private void TouchRectangle_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TouchRectangle_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TouchRectangle_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            TranslateTransform dragTranslation = new TranslateTransform();
            dragTranslation.X += e.Delta.Translation.X;
            dragTranslation.Y += e.Delta.Translation.Y;
            //touchRectangle.RenderTransform = dragTranslation;
        }

        private void touchRectangle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            // Pointer moved outside Rectangle hit test area.
            // Reset the dimensions of the Rectangle.
            if (null != rect)
            {
                rect.Width = 200;
                rect.Height = 100;
            }
        }
        // Handler for pointer released event.
        private void touchRectangle_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            // Reset the dimensions of the Rectangle.
            if (null != rect)
            {
                rect.Width = 200;
                rect.Height = 100;
            }
        }

        // Handler for pointer pressed event.
        private void touchRectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            // Change the dimensions of the Rectangle.
            if (null != rect)
            {
                rect.Width = 250;
                rect.Height = 150;
            }
        }

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

                if (cur.Value.Name == "ToastBackgroundTask2")
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
            IBackgroundTrigger trigger1 = new TimeTrigger(5, false);
            builder.SetTrigger(trigger1);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            return task;
        }

        private void bgTaskToast()
        {
            const string taskName = "ToastBackgroundTask";

            // If background task is already registered, do nothing
            if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals(taskName)))
                return;

            // Otherwise request access
            //BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            // Create the background task
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
            {
                Name = taskName
            };

            // Assign the toast action trigger
            //builder.SetTrigger(new ToastNotificationActionTrigger());
            builder.SetTrigger(new TimeTrigger(1, false));

            // And register the task
            BackgroundTaskRegistration registration = builder.Register();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText("hh");
            Clipboard.SetContent(dataPackage);



            //if (selectListView.SelectedItem != null)
            //{
            //    string selectText = "Selected item: " + selectListView.SelectedItem.ToString();
            //}
            //else
            //{
            //    string selectText = "Selected item: null";
            //}
            //string text =
            //    "Selected index: " + selectListView.SelectedIndex.ToString();
            //text =
            //    "Items selected: " + selectListView.SelectedItems.Count.ToString();
            //text =
            //    "Added: " + e.AddedItems.Count.ToString();
            //text =
            //    "Removed: " + e.RemovedItems.Count.ToString();
            //        int conversationId = 384928;
            //        ToastActionsCustom actions = new ToastActionsCustom()
            //        {
            //            Inputs = {
            //                new ToastTextBox("tbReply"){
            //                    PlaceholderContent = "Type a response"
            //                }
            //            },
            //            Buttons =
            //{
            //    new ToastButton("Reply", new QueryString()
            //    {
            //        { "action", "reply" },
            //        { "conversationId", conversationId.ToString() }

            //    }.ToString())
            //    {
            //        ActivationType = ToastActivationType.Background,
            //        ImageUri = "Assets/8.jpg",

            //        // Reference the text box's ID in order to
            //        // place this button next to the text box
            //        TextBoxId = "tbReply"
            //    },

            //    new ToastButton("Like", new QueryString()
            //    {
            //        { "action", "like" },
            //        { "conversationId", conversationId.ToString() }

            //    }.ToString())
            //    {
            //        ActivationType = ToastActivationType.Background
            //    },

            //    new ToastButton("View", new QueryString()
            //    {
            //        { "action", "viewImage" },
            //        { "imageUrl", "https://picsum.photos/360/202?image=883" }

            //    }.ToString())
            //}
            //        };


            //        ToastContent content = new ToastContent()
            //        {
            //            Visual = new ToastVisual()
            //            {
            //                BindingGeneric = new ToastBindingGeneric()
            //                {
            //                    Children = {
            //                        new AdaptiveText(){
            //                            Text = "Andrew sent you a picture"
            //                        },
            //                        new AdaptiveText(){
            //                            Text = "Check this out, Happy Canyon in Utah!"
            //                        },
            //                        new AdaptiveImage(){
            //                            Source = "Assets/3.jpg"
            //                        }
            //                    },
            //                    AppLogoOverride = new ToastGenericAppLogo
            //                    {
            //                        HintCrop = ToastGenericAppLogoCrop.Circle,
            //                        Source = "Assets/2.jpg"
            //                    }
            //                }
            //            },
            //            Actions = actions,
            //            Launch = new QueryString() {
            //                { "action", "viewConversation" },
            //                { "conversationId", conversationId.ToString() }
            //            }.ToString()
            //        };

            //        var toast = new ToastNotification(content.GetXml());
            //        ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
