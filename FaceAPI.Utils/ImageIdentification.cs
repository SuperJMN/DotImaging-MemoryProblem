namespace FaceAPI.Utils
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ProjectOxford.Face.Contract;

    public class ImageIdentification
    {
        public Guid FaceId { get; set; }
        public FaceRectangle Rectangle { get; set; }
        public IEnumerable<Candidate> Candidates { get; set; }

        public override string ToString()
        {
            return $"{nameof(FaceId)}: {FaceId}, {nameof(Rectangle)}: {Rectangle}, {nameof(Candidates)}: " + string.Join(", ", Candidates);
        }
    }
}