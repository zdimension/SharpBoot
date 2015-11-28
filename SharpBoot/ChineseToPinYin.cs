using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Pinyin4net.Format;

namespace SharpBoot
{
    public static class ChineseToPinYin
    {
        public static string ChineseToPinyin(this string s)
        {
            var sb = new StringBuilder();
            foreach(char c in s)
            {
                if (char.GetUnicodeCategory(c) == UnicodeCategory.OtherLetter)
                {
                    sb.Append(string.Join(" ", Program.GetCulture()
                        .TextInfo.ToTitleCase(string.Join(" ",
                            Pinyin4net.PinyinHelper.ToHanyuPinyinStringArray(c,
                                new HanyuPinyinOutputFormat() {ToneType = HanyuPinyinToneType.WITH_TONE_NUMBER})[0]))) + " ");
                }
                else sb.Append(c);
            }
            return sb.ToString().Trim();
        }
    }
}
