using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SharpBoot.Properties;

namespace SharpBoot
{
    public class ISOV
    {
        public string Hash { get; set; }

        public string Name { get; set; }

        public string DownloadLink { get; set; }
        public bool Latest { get; set; }
        public string Filename { get; set; }

        public ISOInfo Parent { get; set; }

        public ISOV(string h, string n, string d = "", string fn = "", bool l = false)
        {
            Hash = h;
            Name = n;
            DownloadLink = d == "" ? "www.google.com/search?q=" + n : d;
            Latest = l;
            Filename = fn;
        }
    }

    public class ISOInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        //public string DownloadLink { get; set; }

        public string Filename { get; set; }

        public List<ISOV> Versions { get; set; }

        public ISOV LatestVersion
        {
            get
            {
                var r = Versions.FirstOrDefault(x => x.Latest);
                return r ?? Versions.OrderByDescending(x => x.Name).First();
            }
        }

        public ISOInfo(string name, string desc, string cat, string fn = "", params ISOV[] vers)
        {
            Name = name;
            Description = desc;
            Category = cat;
            Filename = fn;
            Versions = vers.ToList();
            Versions.ForEach(x => x.Parent = this);
        }

        public static void RefreshISOs()
        {
            var th = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Lang);
                ISOs = new List<ISOInfo>
                {
                    new ISOInfo(
                        "Offline Windows NT Password & Registry Editor",
                        ISODesc.chntpw,
                        ISOCat.Password,
                        "^(cd[0-9]{6}).iso$",
                        new ISOV("06bbed5b81475c6899dbb549a755b00d", "chntpw 2014-02-01",
                            "https://dl.dropboxusercontent.com/u/98959151/chntpw_140201.iso", "cd140201.iso"))
                    ,
                    new ISOInfo(
                        "Kon-Boot",
                        ISODesc.chntpw,
                        ISOCat.Password,
                        "^kon-boot(.*).(img|iso)$",
                        new ISOV("eed910d2ef9b058cf3eec28294bd303c", "Kon-Boot 2.5",
                            "https://dl.dropboxusercontent.com/u/98959151/konboot.img", "konboot.img"))
                            ,
                    new ISOInfo(
                        "Clonezilla",
                        ISODesc.clonezilla,
                        ISOCat.Partition,
                        "^clonezilla-live-([0-9]{8})-([a-z]+)-([a-z0-9]+).iso$",
                        new ISOV("95aed0dd50964c3adcb17183ab1dff37", "Clonezilla 2.4.2-61",
                            "http://downloads.sourceforge.net/project/clonezilla/clonezilla_live_stable/2.4.2-61/clonezilla-live-2.4.2-61-amd64.iso",
                            @"/clonezilla-live-2\.4\.2-61-(.*).iso")
                        )
                    ,
                    new ISOInfo("Darik's Boot and Nuke",
                        ISODesc.dban,
                        ISOCat.Partition, "^dban-([0-9.]+)_([a-z0-9]+).iso$",
                        new ISOV("d076d4bc510eb39f57196773172ad072", "Darik's Boot and Nuke 2.2.8",
                            "http://downloads.sourceforge.net/project/dban/dban/dban-2.2.8/dban-2.2.8_i586.iso",
                            "/dban-2.2.8_(.*).iso"),
                        new ISOV("33a1df4171e649462ef9679ac207aa77", "Darik's Boot and Nuke 2.3.0",
                            "http://downloads.sourceforge.net/project/dban/dban/dban-2.3.0/dban-2.3.0_i586.iso",
                            "/dban-2.3.0_(.*).iso"))
                    ,
                    new ISOInfo("GParted",
                        ISODesc.gparted,
                        ISOCat.Partition, "^gparted-live-([0-9.-]+)([0-9a-z]+).iso$",
                        new ISOV("d57b2f94a54a0f48899f8e21d070cb40", "GParted 0.19.0-1",
                            "http://downloads.sourceforge.net/project/gparted/gparted-live-stable/0.19.0-1/gparted-live-0.19.0-1-i486.iso",
                            "/gparted-live-0.19.0-1-(.*).iso"),
                        new ISOV("f6e11d722ab8167568bdf6e393c50651", "GParted 0.24.0-2",
                            "http://downloads.sourceforge.net/project/gparted/gparted-live-stable/0.24.0-2/gparted-live-0.24.0-2-i586.iso",
                            "/gparted-live-0.24.0-2-(.*).iso")),
                    new ISOInfo(
                        "Memtest86+",
                        ISODesc.memtest86,
                        ISOCat.Utility,
                        @"^(?:memtest86(\+|plus)[-_]([0-9.]+).iso|MEMTEST\.IMG)$",
                        new ISOV("0f3d162f0c2f42da1455993ac4df396b",
                            "Memtest86+ 5.01",
                            "https://dl.dropboxusercontent.com/u/98959151/memtest86plus-501.iso",
                            "memtest86plus_501.iso")),
                    new ISOInfo(
                        "Memtest86",
                        ISODesc.memtest86,
                        ISOCat.Utility,
                        @"^Memtest86-([0-9.]+).iso$",
                        new ISOV("0c7dd6227dabcb1a5e56c96f178f381d",
                            "Memtest86 4.3.7",
                            "https://dl.dropboxusercontent.com/u/98959151/Memtest86-4.3.7.iso",
                            "Memtest86-4.3.7.iso"),
                        new ISOV("ad1310faef37409e1a1b845a5268cb76",
                            "Memtest86 6.2.0",
                            "https://dl.dropboxusercontent.com/u/98959151/Memtest86-6.2.0.iso",
                            "Memtest86-6.2.0.iso")),
                    new ISOInfo(
                        "Ophcrack",
                        ISODesc.ophcrack,
                        ISOCat.Password,
                        @"^ophcrack-(\w+)-livecd-([0-9.]+).iso$",
                        new ISOV("b23afa62f670dee41c8f01c436c0a092",
                            "Ophcrack (XP) 3.6.0",
                            "http://downloads.sourceforge.net/project/ophcrack/ophcrack-livecd/3.6.0/ophcrack-xp-livecd-3.6.0.iso",
                            "/ophcrack-xp-livecd-(.*).iso"),
                        new ISOV("f0753acfe2fce5249ceceec7dfeacea9",
                            "Ophcrack (Vista/7/8) 3.6.0",
                            "http://downloads.sourceforge.net/project/ophcrack/ophcrack-livecd/3.6.0/ophcrack-vista-livecd-3.6.0.iso",
                            "/ophcrack-vista-livecd-(.*).iso")),
                    new ISOInfo(
                        "Redo Backup and Recovery",
                        ISODesc.redobackup,
                        ISOCat.Utility,
                        @"^redobackup-livecd-([0-9.]+).iso$",
                        new ISOV("f84b757242c1b050f8cbed7142197d81",
                            "Redo Backup and Recovery 1.0.4",
                            "http://downloads.sourceforge.net/project/redobackup/redobackup-livecd-1.0.4.iso",
                            "redobackup-livecd-1.0.4.iso")),
                    new ISOInfo(
                        "Ultimate Boot CD 4 DOS",
                        ISODesc.ubcd4dos,
                        ISOCat.Utility,
                        @"^ubcd[0-9]{2,3}.iso$",
                        new ISOV("a5617e0bdd3ccc3cb3a1b1dd20396d7a",
                            "Ultimate Boot CD 4 DOS 5.3.0",
                            "http://ftp.cc.uoc.gr/mirrors/linux/ubcd/ubcd530.iso",
                            "ubcd530.iso"),
                        new ISOV("",
                            "Ultimate Boot CD 4 DOS 5.3.5",
                            "http://ftp.cc.uoc.gr/mirrors/linux/ubcd/ubcd535.iso",
                            "ubcd535.iso")),
                    new ISOInfo(
                        "Kali Linux",
                        ISODesc.kalilinux,
                        ISOCat.Linux,
                        @"^kali-linux-([0-9.]+)-([0-9a-z]+).iso$",
                        new ISOV("sha1:8e0f63bc97842b2af6ff34986790efeb10d4d1a0",
                            "Kali Linux 1.0.7",
                            "http://cdimage.kali.org/kali-1.0.7/kali-linux-1.0.7-i386.iso",
                            "/kali-linux-1.0.7-(.*).iso"),
                        new ISOV("sha1:6e5e6390b9d2f6a54bc980f50d6312d9c77bf30b",
                            "Kali Linux 2.0 x86",
                            "http://cdimage.kali.org/kali-2.0/kali-linux-2.0-i386.iso",
                            "kali-linux-2.0-i386.iso"),
                        new ISOV("sha1:aaeb89a78f155377282f81a785aa1b38ee5f8ba0",
                            "Kali Linux 2.0 x64",
                            "http://cdimage.kali.org/kali-2.0/kali-linux-2.0-amd64.iso",
                            "kali-linux-2.0-amd64.iso")),
                    new ISOInfo(
                        "Fedora",
                        ISODesc.fedora,
                        ISOCat.Linux,
                        @"^Fedora-Live-([A-Za-z]+)-([0-9a-z]+)-([0-9-]+).iso$",
                        new ISOV("sha256:b115c5653b855de2353e41ff0c72158350f14a020c041462f35ba2a47bd1e33b",
                            "Fedora 20.1",
                            "http://download.fedoraproject.org/pub/fedora/linux/releases/20/Live/i386/Fedora-Live-Desktop-i686-20-1.iso",
                            "Fedora-Live-Desktop-i686-20-1.iso"),
                        new ISOV("sha256:6e4c47b582ece2b431ee95d6f453945d11e28c712f7619b178cb31979138f884",
                            "Fedora 22.3 x86",
                            "http://mir01.syntis.net/fedora/linux/releases/22/Workstation/i386/iso/Fedora-Live-Workstation-i686-22-3.iso",
                            "Fedora-Live-Workstation-i686-22-3.iso"),
                        new ISOV("sha256:615abfc89709a46a078dd1d39638019aa66f62b0ff8325334f1af100551bb6cf",
                            "Fedora 22.3 x64",
                            "http://mir01.syntis.net/fedora/linux/releases/22/Workstation/x86_64/iso/Fedora-Live-Workstation-x86_64-22-3.iso",
                            "Fedora-Live-Workstation-x86_64-22-3.iso")),
                    new ISOInfo(
                        "Fusion Linux",
                        ISODesc.fusionlinux,
                        ISOCat.Linux,
                        @"^Fusion-Linux-([0-9]+).iso$",
                        new ISOV("6e17614e3cb23db3f63446f55b179d17",
                            "Fusion Linux 15",
                            "http://fusion.nestabilni.com/Fusion-Linux-15/Fusion-Linux-15.iso",
                            "Fusion-Linux-15.iso"),
                        new ISOV("4caa6e3c438b645839711fa8da4f17fe",
                            "Fusion Linux 16 64-bit",
                            "http://fusion.nestabilni.com/current/Fusion-Linux-16-64bit.iso",
                            "Fusion-Linux-16-64bit.iso")),
                    new ISOInfo(
                        "Inquisitor",
                        ISODesc.inquisitor,
                        ISOCat.Linux,
                        @"^inq-live-([0-9a-z.]+)-([0-9a-z-]+).iso$",
                        new ISOV("25ab080c12f540236bca7fad901f7ff0",
                            "Inquisitor Live 3.0",
                            "http://downloads.sourceforge.net/project/inq/inquisitor/3.0/inq-live-3.0-x86.iso",
                            "inq-live-3.0-(.*).iso"),
                        new ISOV("c7fef1289f36b4c25ee4057a9988d975",
                            "Inquisitor Live 3.1 beta 2 x64",
                            "http://downloads.sourceforge.net/project/inq/inquisitor/3.1-beta2/inq-live-3.1beta2-amd64-debian.iso",
                            "/inq-live-3.1(.*).iso")),
                    new ISOInfo(
                        "Knoppix",
                        ISODesc.knoppix,
                        ISOCat.Linux,
                        @"^KNOPPIX_V([0-9.]+)CD-[0-9-]{10}-[A-Z]{2}.iso$",
                        new ISOV("43e1bf11bd52d88d61379fdd38fe869c",
                            "Knoppix 7.2.0",
                            "http://ftp.free.fr/pub/Distributions_Linux/knoppix/KNOPPIX_V7.2.0CD-2013-06-16-EN.iso",
                            "/KNOPPIX_(.*).iso")),
                    new ISOInfo(
                        "NetbootCD",
                        ISODesc.netbootcd,
                        ISOCat.Linux,
                        @"^NetbootCD-[0-9.]+.iso$",
                        new ISOV("75ef50099a4887df5195a31adb474db0",
                            "NetbootCD 5.3.3",
                            "http://downloads.tuxfamily.org/netbootcd/5.3.3/NetbootCD-5.3.3.iso",
                            "/NetbootCD-(.*).iso")),
                    new ISOInfo(
                        "Ubuntu",
                        ISODesc.ubuntu,
                        ISOCat.Linux,
                        @"^ubuntu-[0-9.]{5}-desktop-[0-9a-z]+.iso$",
                        new ISOV("09eb43dcfce2b7246bdd6e8108e755df",
                            "Ubuntu 12.04.5 LTS x86",
                            "http://releases.ubuntu.com/12.04.5/ubuntu-12.04.5-desktop-i386.iso",
                            "/ubuntu-12.04(.*)-i386.iso"),
                        new ISOV("48b4edf237c489eebbfef208c2650d11",
                            "Ubuntu 12.04.5 LTS x64",
                            "http://releases.ubuntu.com/12.04.5/ubuntu-12.04.5-desktop-amd64.iso",
                            "/ubuntu-12.04(.*)-amd64.iso"),
                        new ISOV("c4d4d037d7d0a05e8f526d18aa25fb5e",
                            "Ubuntu 14.04 x86",
                            "http://releases.ubuntu.com/14.04/ubuntu-14.04-desktop-i386.iso",
                            "/ubuntu-14.04-(.*).iso"),
                        new ISOV("7d483b990de4e1369b76b7b693737191",
                            "Ubuntu 15.10 x86",
                            "http://releases.ubuntu.com/15.10/ubuntu-15.10-desktop-i386.iso",
                            "/ubuntu-15.10(.*)-i386.iso"),
                        new ISOV("ece816e12f97018fa3d4974b5fd27337",
                            "Ubuntu 15.10 x64",
                            "http://releases.ubuntu.com/15.10/ubuntu-15.10-desktop-amd64.iso",
                            "/ubuntu-15.10(.*)-amd64.iso", true)),
                    new ISOInfo(
                        "Edubuntu",
                        ISODesc.edubuntu,
                        ISOCat.Linux,
                        @"^edubuntu-[0-9.]{5}-dvd-[0-9a-z]+.iso$",
                        new ISOV("1919086a97af4092342933a5eccbab62",
                            "Edubuntu 14.04",
                            "http://cdimage.ubuntu.com/edubuntu/releases/14.04/release/edubuntu-14.04-dvd-i386.iso",
                            "")),
                    new ISOInfo(
                        "Kubuntu",
                        ISODesc.kubuntu,
                        ISOCat.Linux,
                        @"^kubuntu-[0-9.]{5}-desktop-[0-9a-z]+.iso$",
                        new ISOV("327cf4202f8e2601ce0d772082c84b86",
                            "Kubuntu 14.04",
                            "http://cdimage.ubuntu.com/kubuntu/releases/14.04/release/kubuntu-14.04-desktop-i386.iso",
                            "")),
                    new ISOInfo(
                        "Lubuntu",
                        ISODesc.lubuntu,
                        ISOCat.Linux,
                        @"^lubuntu-[0-9.]{5}-desktop-[0-9a-z]+.iso$",
                        new ISOV("b0d1c58c8515ab40382d01f59655ba85",
                            "Lubuntu 14.04",
                            "http://cdimage.ubuntu.com/lubuntu/releases/14.04/release/lubuntu-14.04-desktop-i386.iso",
                            "")),
                    new ISOInfo(
                        "Ubuntu GNOME",
                        ISODesc.ubuntugnome,
                        ISOCat.Linux,
                        @"^ubuntu-gnome-[0-9.]{5}-desktop-[0-9a-z]+.iso$",
                        new ISOV("ab5c39caef103694fe97bda23412ff00",
                            "Ubuntu GNOME 14.04",
                            "http://cdimage.ubuntu.com/ubuntu-gnome/releases/14.04/release/ubuntu-gnome-14.04-desktop-i386.iso",
                            "")),
                    new ISOInfo(
                        "Ubuntu Server",
                        ISODesc.ubuntuserver,
                        ISOCat.Linux,
                        @"^ubuntu-[0-9.]{5}-server-[0-9a-z]+.iso$",
                        new ISOV("08d25bf879e353686a974b7b14ae7d81",
                            "Ubuntu Server 14.04",
                            "http://releases.ubuntu.com/14.04/ubuntu-14.04-server-i386.iso",
                            "")),
                    new ISOInfo(
                        "Ubuntu Studio",
                        ISODesc.ubuntustudio,
                        ISOCat.Linux,
                        @"^ubuntustudio-[0-9.]{5}-dvd-[0-9a-z]+.iso$",
                        new ISOV("75cdb9b7cb42e4bd04d2554e6142764c",
                            "Ubuntu Studio 14.04",
                            "http://cdimage.ubuntu.com/ubuntustudio/releases/14.04/release/ubuntustudio-14.04-dvd-i386.iso",
                            "")),
                    new ISOInfo(
                        "Xubuntu",
                        ISODesc.xubuntu,
                        ISOCat.Linux,
                        @"^xubuntu-[0-9.]{5}-desktop-[0-9a-z]+.iso$",
                        new ISOV("ccd326466b705bc324a20dd45cb3de82",
                            "Xubuntu 14.04",
                            "http://cdimage.ubuntu.com/xubuntu/releases/14.04/release/xubuntu-14.04-desktop-i386.iso",
                            ""))
                }.OrderBy(item => item.Category).ToList();
            });
            Task.WaitAll(th);
        }

        public static List<ISOInfo> ISOs = new List<ISOInfo>();


        public static ISOInfo FromFile(string f)
        {
            return null;
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public static ISOV GetFromFile(string filename)
        {
            ISOV resk = null;

            var s = ISOs.FirstOrDefault(x => Regex.IsMatch(Path.GetFileName(filename), x.Filename));
            if (s != null)
            {
                resk = s.LatestVersion;
            }
            else
            {
                var md5 = Utils.FileHash(filename, "md5");
                var sta = ISOs.SelectMany(x => x.Versions);
                var st =
                    sta.FirstOrDefault(
                        x =>
                            x.Filename.StartsWith("/")
                                ? Regex.IsMatch(Path.GetFileName(filename), x.Filename.Substring(1))
                                : string.Equals(Path.GetFileName(filename).Trim(), x.Filename.Trim(),
                                    StringComparison.CurrentCultureIgnoreCase)) ??
                    sta.FirstOrDefault(
                        x =>
                            x.Hash ==
                            (x.Hash.Contains(':') ? Utils.FileHash(filename, x.Hash.Split(':')[0]) : md5));

                if (st != null)
                {
                    resk = st;
                }
            }

            return resk;
        }
    }
}