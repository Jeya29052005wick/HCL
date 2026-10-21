
using System;

namespace LinqMastery
{
    internal class Dele
    {
        public void CheckString(string s, Predicate<string> condition)
        {
            Console.WriteLine(condition(s));
        }
        public bool IsPalindrome(string s)
        {
            return s == new string(s.Reverse().ToArray());
        }
        public void Run()
        {
            CheckString("malayalam", IsPalindrome);
            CheckString("Hello", (s) => s.Distinct().Count() == s.Length);
            CheckString("programming", s => s.Length > 8);
            CheckString("level", s => s[0] == s[s.Length - 1]);
        }



    }

}
