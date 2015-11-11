using System.IO;

namespace SharpBoot
{
    internal enum ImageType
    {
        ISO = 1,
        IMG = 2
    }

    public class ImageLine
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public long SizeB { get; private set; }

        public string CustomCode { get; set; }

        public ImageLine()
        {
            Name = "";
            FilePath = "";
            Category = "";
            Description = "";
            SizeB = 0;
        }

        public ImageLine(string n, string fp, string d, string cat = "", string code = "")
        {
            Name = n;
            FilePath = fp;
            Category = cat;
            Description = d;
            CustomCode = code;

            SizeB = new FileInfo(fp).Length;
        }
    }
}