namespace Sample
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using DotImaging;
    using DotImaging.Primitives2D;

    public class SizeRetriever
    {
        public IObservable<Size> ImageSizeRetriever(IObservable<IImage> images)
        {
            return images
                .SelectMany(image => Observable.FromAsync(() => GetSize(image)));
        }

        private static async Task<Size> GetSize(IImage image)
        {
            await Task.Delay(100);  // Simulate some delay
            return image.Size;
        }
    }
}