using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Control;

namespace MediaLink
{
    internal class LinkManager
    {
        public static GlobalSystemMediaTransportControlsSession CurrentSession { get; private set; }
        public static WebSocketManager WebSocketManager { get; private set; }

        private static MediaInfoWrapper lastMediaWrapper;

        private async static Task Run()
        {
            WebSocketManager = new WebSocketManager();

            GlobalSystemMediaTransportControlsSessionManager sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

            while (true)
            {
                // Really this shouldn't be here but I don't want another thread running
                bool isRunning = WebSocketManager.IsRunning();
                TrayApplicationContext.StatusMenuItem.Text = "Status: " + (isRunning ? "Running" : "Stopped");
                TrayApplicationContext.StatusMenuItem.Image = isRunning ? Properties.Resources.Running : Properties.Resources.Stopped;

                CurrentSession = sessionManager.GetCurrentSession();

                MediaInfoWrapper mediaWrapper = new MediaInfoWrapper();

                if (CurrentSession == null)
                {
                    mediaWrapper.HasData = false;
                    mediaWrapper.Data = null;
                }
                else
                {
                    GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties = await CurrentSession.TryGetMediaPropertiesAsync();

                    MediaInfo mediaInfo = new MediaInfo();
                    mediaInfo.Artist = mediaProperties.Artist;
                    mediaInfo.Title = mediaProperties.Title;
                    mediaInfo.PlaybackStatus = CurrentSession.GetPlaybackInfo().PlaybackStatus.ToString();

                    // Convert the thumb to a jpg and base64
                    if (mediaProperties.Thumbnail != null)
                    {
                        Stream stream = (await mediaProperties.Thumbnail.OpenReadAsync()).GetInputStreamAt(0).AsStreamForRead(short.MaxValue);

                        Image image = Image.FromStream(stream);

                        MemoryStream ms = new MemoryStream();
                        image.Save(ms, ImageFormat.Jpeg);
                        mediaInfo.Thumbnail = Convert.ToBase64String(ms.ToArray());
                    }

                    mediaWrapper.HasData = true;
                    mediaWrapper.Data = mediaInfo;
                }

                // Check if data has changed
                if (!mediaWrapper.Equals(lastMediaWrapper))
                {
                    // Send data over websocket
                    Debug.WriteLine("Updating websocket");
                    WebSocketManager.BroadcastMessage(mediaWrapper);

                    lastMediaWrapper = mediaWrapper;
                }

                Thread.Sleep(100);
            }
        }

        public static void Start()
        {
            Task.Factory.StartNew(() => Run());
        }
    }
}
