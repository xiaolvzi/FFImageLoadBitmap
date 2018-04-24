using Android.App;
using Android.Widget;
using Android.OS;
using FFImageLoading.Views;
using FFImageLoading;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Java.IO;
using Android.Graphics;

namespace FFimageLoadTest
{
    [Activity(Label = "FFimageLoadTest", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Bitmap bmp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ImageViewAsync imageView = FindViewById<ImageViewAsync>(Resource.Id.mainImage);
            

            ImageService.Instance
            .LoadStream(GetStreamFromImageByte)
            .Into(imageView);

        }

        private Task<Stream> GetStreamFromImageByte(CancellationToken arg)
        {
            bmp = BitmapFactory.DecodeResource(Resources, Resource.Drawable.play);
            MemoryStream stream = new MemoryStream();
            bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            byte[] imageInBytes = stream.ToArray();


            //Since we need to return a Task<Stream> we will use a TaskCompletionSource>
            TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>();

            tcs.TrySetResult(new MemoryStream(imageInBytes));

            return tcs.Task;
        }
    }
}

