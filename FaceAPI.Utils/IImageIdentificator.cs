namespace FaceAPI.Utils
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DotImaging;

    public interface IImageIdentificator
    {
        Task<IEnumerable<ImageIdentification>> Identify(IImage image, string groupId);
    }
}