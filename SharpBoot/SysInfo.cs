using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SharpBoot
{
    public class SysInfo
    {
        public static bool Is64 => Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE").IndexOf("64") > 0;

        public static string FileMD5(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    var hash = md5.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileSHA1(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (var sha1 = new SHA1Managed())
                {
                    var hash = sha1.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileSHA256(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (SHA256 sha256 = new SHA256Managed())
                {
                    var hash = sha256.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileSHA384(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (SHA384 sha384 = new SHA384Managed())
                {
                    var hash = sha384.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileSHA512(string fileName)
        {
            StringBuilder formatted;
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var bs = new BufferedStream(fs))
            {
                using (SHA512 sha512 = new SHA512Managed())
                {
                    var hash = sha512.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (var b in hash)
                    {
                        formatted.AppendFormat("{0:x2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        public static string FileHash(string fileName, string hash)
        {
            switch (hash)
            {
                case "md5":
                    return FileMD5(fileName);
                case "sha1":
                    return FileSHA1(fileName);
                case "sha256":
                    return FileSHA256(fileName);
            }

            return "";
        }
    }
}