namespace Sample
{
    using System.IO;
    using System.IO.Compression;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Bravent.FaceRecAzure.Services.Picasso;
    using FaceAPI.Utils;
    using Microsoft.ProjectOxford.Face;
    using Nito.AsyncEx;
    using Serilog;

    class Program
    {
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .MinimumLevel.Debug()
                .CreateLogger();

            AsyncContext.Run(Identify);
        }

        public static async Task Identify()
        {
            var identificator = new BatchIdentificator(new ImageIdentificator(new AutoRetryFaceClient(new FaceServiceClient("____REPLACE_ME_WITH_API_KEY____"))));
            
            var identifications = Observable.Using(() => CreateCapture("ZippedImages-Lite-1.zip"),
                capture =>
                {
                    return identificator.Identify(capture.ToObservable(), (i, idents) => new { i, idents });
                });

            await identifications.ToList();           
        }

        private static ZipCapture CreateCapture(string path)
        {
            return new ZipCapture(new ZipArchive(File.OpenRead(path)));
        }
    }
}
