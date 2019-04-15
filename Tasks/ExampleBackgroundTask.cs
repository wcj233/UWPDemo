using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace Tasks
{
    public sealed class ExampleBackgroundTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral = null;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            //storage image
            
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
        }


        //protected async Task<WriteableBitmap> GetWriteableBitmapAsync(string url)
        //{
        //    try
        //    {
        //        IBuffer buffer = await GetBufferAsync(url);
        //        if (buffer != null)
        //        {
        //            BitmapImage bi = new BitmapImage();
        //            WriteableBitmap wb = null;
        //            Stream stream2Write;
        //            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
        //            {

        //                stream2Write = stream.AsStreamForWrite();

        //                await stream2Write.WriteAsync(buffer.ToArray(), 0, (int)buffer.Length);

        //                await stream2Write.FlushAsync();
        //                stream.Seek(0);

        //                await bi.SetSourceAsync(stream);

        //                wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);
        //                stream.Seek(0);
        //                await wb.SetSourceAsync(stream);

        //                return wb;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //async static public Task<IBuffer> GetBufferAsync(string url)
        //{

        //    HttpClient httpClient = new HttpClient();

        //    var ResultStr = await httpClient.GetBufferAsync(new Uri(url));
        //    return ResultStr;
        //}
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //
            // Indicate that the background task is canceled.
            //
           
        }
    }
}
