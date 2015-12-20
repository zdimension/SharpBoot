using System.Globalization;
using System.Text;
using Pinyin4net;

namespace SharpBoot
{
    public static class ChineseToPinYin
    {
        public static string ChineseToPinyin(this string s)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (char.GetUnicodeCategory(c) == UnicodeCategory.OtherLetter)
                {
                    sb.Append(string.Join(" ", Program.GetCulture()
                        .TextInfo.ToTitleCase(string.Join(" ",
                            PinyinHelper.ToHanyuPinyinStringArray(c)[0]))) + " ");
                }
                else sb.Append(c);
            }
            return sb.ToString().Trim();
        }
    }
}