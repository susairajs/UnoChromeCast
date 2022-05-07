using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UnoApp1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public string CastUrl { get; set; } = "https://icecast-qmusicnl-cdp.triple-it.nl/Qmusic_nl_live_32.aac";

        public ObservableCollection<IReceiver> CastDevices { get; set; }
        public IReceiver SelectedCastDevice { get; set; }

        //public ICommand StartCastCommand { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            GetReceivers();
        }

        void Button_Click(System.Object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StartCasting();
        }

        //Start Stream video
        async void StartCasting()
        {
            var sender = new Sender();
            await sender.ConnectAsync(SelectedCastDevice);
            var mediaChannel = sender.GetChannel<IMediaChannel>();
            await sender.LaunchAsync(mediaChannel);
            var mediaStatus = await mediaChannel.LoadAsync(
                new MediaInformation() { ContentId = CastUrl });
        }
        // Use the DeviceLocator to find all connected Chromecasts devices on our network
        private async Task GetReceivers()
        {
            var receivers = await new DeviceLocator().FindReceiversAsync();

            CastDevices = new ObservableCollection<IReceiver>();
            if (CastDevices.Count > 0)
            {
                foreach (var r in receivers)
                    CastDevices.Add(r);
            }

            DataContext = this;
        }
    }
}
