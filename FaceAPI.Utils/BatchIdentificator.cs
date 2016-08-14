namespace FaceAPI.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using DotImaging;
    using Serilog;

    public class BatchIdentificator : IBatchIdentificator
    {
        private readonly ImageIdentificator identificator;

        public BatchIdentificator(ImageIdentificator identificator)
        {
            this.identificator = identificator;
        }

        public IObservable<T> Identify<T>(IObservable<IImage> images, Func<int, IEnumerable<ImageIdentification>, T> selector, int maxConcurrent = 5)
        {
            return images.Select(
                    (image, i) => Observable.FromAsync(
                        async () =>
                        {
                            Log.Debug("Identifying frame {FrameId}", i);
                            var identifications = await identificator.Identify(image, "testing");
                            Log.Debug("Identified {FrameId}", i);

                            return new {Identification = identifications, FrameId = i};
                        }))
                .Merge(maxConcurrent)
                .Select(arg => selector(arg.FrameId, arg.Identification));
        }
    }
}