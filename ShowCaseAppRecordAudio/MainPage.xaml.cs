using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=391641 dokumentiert.

namespace ShowCaseAppRecordAudio
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Rahmen angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private MediaCapture audioCapture = null;

        async private void RecordAudio(string filepath)
        {
            audioCapture = new MediaCapture();
            var audio = await GetAudioId();
            await audioCapture.InitializeAsync(GetMediaCaptureSettings(audio.Id));
            var folder = KnownFolders.MusicLibrary;
            var myAudioFile = await folder.CreateFileAsync("audio.wma", CreationCollisionOption.ReplaceExisting);

            await audioCapture.StartRecordToStorageFileAsync(MediaEncodingProfile.CreateWma(AudioEncodingQuality.Auto), myAudioFile);
        }

        private MediaCaptureInitializationSettings GetMediaCaptureSettings(string audioId)
        {
            var settings = new MediaCaptureInitializationSettings();
            settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
            settings.AudioDeviceId = audioId;
            return new MediaCaptureInitializationSettings();
        }

        private static async Task<DeviceInformation> GetAudioId()
        {
            DeviceInformation deviceID = null;
            deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture)).FirstOrDefault();

            return deviceID;
        }

        async private void StopAudioRecorder()
        {
            if (audioCapture != null)
            {
                await audioCapture.StopRecordAsync();
                audioCapture = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btn.Content.Equals("Record"))
            {
                statusText.Text = "Record...";
                RecordAudio("");
                btn.Content = "Stop";
            }
            else
            {
                StopAudioRecorder();
                statusText.Text = "Stopped";
                btn.Content = "Record";
            }
            
        }
    }
}
