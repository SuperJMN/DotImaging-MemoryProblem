namespace Sample
{
    using System.IO;
    using System.IO.Compression;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using DotImaging;
    using Nito.AsyncEx;

    class Program
    {
        static void Main()
        {
            AsyncContext.Run(Identify);
        }

        public static async Task Identify()
        {
            // LOW MEMORY CONSUMPTION
            //var images =  new FileCapture("http://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_1mb.mp4");

            // HUGE MEMORY CONSUMPTION!!!!
            var images = new ZipCapture(new ZipArchive(new FileStream("ZippedImages.zip", FileMode.Open)));

            var processor = new SizeRetriever();
            var ids = processor.ImageSizeRetriever(images.ToObservable());

            await ids.ToList();
            images.Dispose();
        }       
    }
}
