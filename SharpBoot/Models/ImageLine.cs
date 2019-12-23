using System.IO;

namespace SharpBoot.Models
{
    internal enum ImageType
    {
        ISO = 1,
        IMG = 2
    }

    public class ImageLine
    {
        public ImageLine()
        {
            Name = "";
            FilePath = "";
            Category = "";
            Description = "";
            SizeB = 0;
            EntryType = EntryType.Nope;
        }

        public ImageLine(string n, string fp, string d, string cat = "", string code = "",
            EntryType typ = EntryType.Nope)
        {
            Name = n;
            FilePath = fp;
            Category = cat;
            Description = d;
            CustomCode = code;
            EntryType = typ;
            SizeB = new FileInfo(fp).Length;
        }

        public string Name { get; set; }
        public string FilePath { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public long SizeB { get; }

        public string CustomCode { get; set; }

        public EntryType EntryType { get; set; }
    }
}