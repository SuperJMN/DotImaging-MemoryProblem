namespace FaceAPI.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using DotImaging;
    using Microsoft.ProjectOxford.Face;
    using Microsoft.ProjectOxford.Face.Contract;




    public class ImageIdentificator : IImageIdentificator
    {
        private readonly IFaceServiceClient client;

        public ImageIdentificator(IFaceServiceClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<ImageIdentification>> Identify(IImage image, string groupId)
        {
            var faces = await Detect(image);
            var identifications = await Identify(groupId, faces);

            return MergeFacesWithIdentifications(faces, identifications).ToList();
        }

        private static IEnumerable<ImageIdentification> MergeFacesWithIdentifications(Face[] faces, List<IdentifyResult> identifications)
        {
            var facesByIdentifications = from face in faces
                join identifyResult in identifications on face.FaceId equals identifyResult.FaceId
                select new ImageIdentification
                {
                    Candidates = identifyResult.Candidates,
                    FaceId = face.FaceId,
                    Rectangle = face.FaceRectangle
                };
            return facesByIdentifications;
        }

        private async Task<List<IdentifyResult>> Identify(string groupId, IEnumerable<Face> faces)
        {
            var faceIds = faces.Select(face => face.FaceId).ToList();
            List<IdentifyResult> identifications;
            if (faceIds.Any())
            {
                identifications = (await client.IdentifyAsync(groupId, faceIds.ToArray())).ToList();
            }
            else
            {
                identifications = new List<IdentifyResult>();
            }
            return identifications;
        }

        private async Task<Face[]> Detect(IImage image)
        {
            Face[] faces;
            using (var stream = new MemoryStream(image.ToBgr().EncodeAsJpeg()))
            {
                faces = await client.DetectAsync(stream);
            }
            return faces;
        }
    }
}