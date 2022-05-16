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

        void StartCast_Click(System.Object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StartCasting();
        }
        void Refresh_Click(System.Object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GetReceivers();
        }

        //Start Stream video
        async void StartCasting()
        {
            var sender = new Sender();
            // Connect to the Chromecast
            await sender.ConnectAsync(SelectedCastDevice);
            // Launch the default media receiver application
            var mediaChannel = sender.GetChannel<IMediaChannel>();
            await sender.LaunchAsync(mediaChannel);
            // Load and play Big Buck Bunny video
            var mediaStatus = await mediaChannel.LoadAsync(
                new MediaInformation() { ContentId = CastUrl });
        }

        private async Task GetReceivers()
        {
            // Use the DeviceLocator to find all connected Chromecasts on our network
            IEnumerable<IReceiver> receivers = await new DeviceLocator().FindReceiversAsync();

            CastDevices = new ObservableCollection<IReceiver>();
            if (receivers != null && receivers.ToList().Count > 0)
            {
                foreach (var r in receivers)
                    CastDevices.Add(r);
            }
            else
            {
                lblNoReciverMessage.Text = "No receiver is found";
            }

            DataContext = this;

        }

    }
}
