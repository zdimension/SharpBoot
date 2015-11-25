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
            try
            {
                var sb = new StringBuilder();
                foreach(char c in s)
                {
                    if (char.GetUnicodeCategory(c) == UnicodeCategory.OtherLetter)
                    {
                        sb.Append(string.Join(" ", Program.GetCulture()
                            .TextInfo.ToTitleCase(string.Join(" ",
                                Pinyin4net.PinyinHelper.ToHanyuPinyinStringArray(c,
                                    new HanyuPinyinOutputFormat() {ToneType = HanyuPinyinToneType.WITHOUT_TONE})))) + " ");
                    }
                    else sb.Append(c);
                    //sb.Append(" ");
                }
                if(sb.ToString().Trim() == "M i   M a")
                {
                    var a = 5;
                }
                return sb.ToString().Trim();
                /*return string.Join(" ",
                    s.Select(
                        x => (char.GetUnicodeCategory(x) == UnicodeCategory.OtherLetter) ?
                            Program.GetCulture()
                                .TextInfo.ToTitleCase(string.Join(" ",
                                    Pinyin4net.PinyinHelper.ToHanyuPinyinStringArray(x,
                                        new HanyuPinyinOutputFormat() {ToneType = HanyuPinyinToneType.WITHOUT_TONE}))) : x.ToString()));*/
            }
            catch
            {
                var a = 1;
                throw;
            }
        }
    }
}
