using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public class Recording {
        public string ArtistName { get; set; }
        public string CompositionName { get; set; }
    }

    public class HostViewModel : INotifyPropertyChanged
    {
        private string nextButtonText;

        public event PropertyChangedEventHandler PropertyChanged;

        public HostViewModel()
        {
            this.nextButtonText = "Next";
        }

        public string NextButtonText
        {
            get { return this.nextButtonText; }
            set
            {
                this.nextButtonText = value;
                this.OnPropertyChanged();
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RecordingViewModel
    {
        private ObservableCollection<Recording> recordings = new ObservableCollection<Recording>();
        public ObservableCollection<Recording> Recordings { get { return this.recordings; } }
        public RecordingViewModel() {
            this.recordings.Add(new Recording
            {
                ArtistName = "Johann Sebastian Bach",
                CompositionName = "Mass in B minor"
            });
            this.recordings.Add(new Recording
            {
                ArtistName = "Ludwig van Beethoven",
                CompositionName = "Third Symphony"
            });
            this.recordings.Add(new Recording
            {
                ArtistName = "George Frideric Handel",
                CompositionName = "Serse"
            });
        }

    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page3 : Page
    {
        public Frame RootFrame { get { return Window.Current.Content as Frame; } }
        public Page3()
        {
            this.InitializeComponent();        
            this.ViewModel = new RecordingViewModel();
            this.buttonModel = new HostViewModel();
            this.DataContext = buttonModel;
        }
        public RecordingViewModel ViewModel { get; set; }
        public HostViewModel buttonModel { get; set; }

        private void buttonClickAction(object sender, RoutedEventArgs e)
        {
            this.buttonModel.NextButtonText = "Updated Next button text";
            //tryButton.Content = "Bye";
            //string name = this.buttonModel.NextButtonText;
        }

        private void write_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string text = write.Text;
            //this.buttonModel.NextButtonText = "Updated Next button text";
        }
    }
}
