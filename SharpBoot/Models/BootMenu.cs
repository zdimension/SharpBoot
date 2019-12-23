using System.Collections.Generic;
using System.IO;

namespace SharpBoot.Models
{
    public class BootMenuItem
    {
        public BootMenuItem(string n, string d, EntryType t, string ison = "", bool st = true, string code = "")
        {
            Name = n;
            Description = d;
            Type = t;
            Start = st;
            IsoName = Path.GetFileName(ison);
            CustomCode = code;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public EntryType Type { get; set; }

        public bool Start { get; set; }

        public string CustomCode { get; set; }


        public string IsoName { get; set; }
    }

    public class BootMenu
    {
        public BootMenu(string title, bool main)
        {
            Title = title;
            MainMenu = main;
            Items = new List<BootMenuItem>();
        }

        public string Title { get; set; }

        public List<BootMenuItem> Items { get; set; }

        public bool MainMenu { get; set; }
    }
}