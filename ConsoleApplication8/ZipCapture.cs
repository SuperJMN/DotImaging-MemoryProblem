namespace Sample
{
    using System.IO.Compression;
    using System.Linq;
    using DotImaging;

    internal class ZipCapture : ImageStreamReader
    {
        private readonly ZipArchiveEntry[] allImages;
        private readonly ZipArchive zipArchive;

        public ZipCapture(ZipArchive zipArchive)
        {
            this.zipArchive = zipArchive;
            var allImagesEnumerable = from entry in zipArchive.Entries
                select entry;
            allImages = allImagesEnumerable.ToArray();
        }

        public override bool CanSeek => true;

        public override long Length => allImages.Length;

        protected override bool ReadInternal(out IImage image)
        {
            var zipArchiveEntry = allImages[Position];
            var length = zipArchiveEntry.Length;
            using (var stream = zipArchiveEntry.Open())
            {
                var buffer = new byte[length];
                stream.Read(buffer, 0, (int) length);
                image = buffer.DecodeAsColorImage().Lock();
            }

            return true;
        }

        public override void Open()
        {
        }


        public override void Close()
        {
            zipArchive.Dispose();
        }
    }
}