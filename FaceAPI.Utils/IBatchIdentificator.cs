namespace FaceAPI.Utils
{
    using System;
    using System.Collections.Generic;
    using DotImaging;

    public interface IBatchIdentificator
    {
        IObservable<T> Identify<T>(IObservable<IImage> images, Func<int, IEnumerable<ImageIdentification>, T> selector, int maxConcurrent);
    }
}