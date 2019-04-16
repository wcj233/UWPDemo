using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
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
    public sealed partial class Page5 : Page
    {
        public Page5()
        {
            this.InitializeComponent();
            bgTaskToast();

            //TileContent content = new TileContent()
            //{
            //    Visual = new TileVisual()
            //    {


            //        TileWide = new TileBinding()
            //        {
            //            Branding = TileBranding.NameAndLogo,
            //            Content = new TileBindingContentAdaptive()
            //            {
            //                Children =
            //    {
            //        new AdaptiveText()
            //        {
            //            Text = "Jennifer Parker",
            //            HintStyle = AdaptiveTextStyle.Subtitle
            //        },

            //        new AdaptiveText()
            //        {
            //            Text = "Photos from our trip",
            //            HintStyle = AdaptiveTextStyle.CaptionSubtle
            //        },

            //        new AdaptiveText()
            //        {
            //            Text = "Check out these awesome photos I took while in New Zealand!",
            //            HintStyle = AdaptiveTextStyle.CaptionSubtle
            //        }
            //    }
            //            }
            //        }
            //    }
            //};

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            //updater.EnableNotificationQueueForWide310x150(true);
            //updater.EnableNotificationQueueForSquare150x150(true);
            //updater.EnableNotificationQueueForSquare310x310(true);
            //updater.EnableNotificationQueue(true);
            updater.Clear();

            for (int i = 1; i < 3; i++)
            {
                TileContent content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileWide = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children = {
                                    new AdaptiveImage(){
                                        Source = "Assets/"+i+".jpg"
                                    }

                                }
                            }
                        }
                    }
                };
                var notification = new TileNotification(content.GetXml());
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }





            //var notification = new TileNotification(content.GetXml());
            //TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        private async void bgTaskToast()
        {
            const string taskName = "ToastBackgroundTask";

            // If background task is already registered, do nothing
            if (BackgroundTaskRegistration.AllTasks.Any(i => i.Value.Name.Equals(taskName)))
                return;

            // Otherwise request access
            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            // Create the background task
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
            {
                Name = taskName
            };

            // Assign the toast action trigger
            builder.SetTrigger(new ToastNotificationActionTrigger());

            // And register the task
            BackgroundTaskRegistration registration = builder.Register();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            int conversationId = 384928;
            ToastActionsCustom actions = new ToastActionsCustom()
            {
                Inputs = {
                    new ToastTextBox("tbReply"){
                        PlaceholderContent = "Type a response"
                    }
                },
                Buttons =
    {
        new ToastButton("Reply", new QueryString()
        {
            { "action", "reply" },
            { "conversationId", conversationId.ToString() }

        }.ToString())
        {
            ActivationType = ToastActivationType.Background,
            ImageUri = "Assets/8.jpg",
 
            // Reference the text box's ID in order to
            // place this button next to the text box
            TextBoxId = "tbReply"
        },

        new ToastButton("Like", new QueryString()
        {
            { "action", "like" },
            { "conversationId", conversationId.ToString() }

        }.ToString())
        {
            ActivationType = ToastActivationType.Background
        },

        new ToastButton("View", new QueryString()
        {
            { "action", "viewImage" },
            { "imageUrl", "https://picsum.photos/360/202?image=883" }

        }.ToString())
    }
            };


            ToastContent content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children = {
                            new AdaptiveText(){
                                Text = "Andrew sent you a picture"
                            },
                            new AdaptiveText(){
                                Text = "Check this out, Happy Canyon in Utah!"
                            },
                            new AdaptiveImage(){
                                Source = "Assets/3.jpg"
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo
                        {
                            HintCrop = ToastGenericAppLogoCrop.Circle,
                            Source = "Assets/2.jpg"
                        }
                    }
                },
                Actions = actions,
                Launch = new QueryString() {
                    { "action", "viewConversation" },
                    { "conversationId", conversationId.ToString() }
                }.ToString()
            };

            var toast = new ToastNotification(content.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
