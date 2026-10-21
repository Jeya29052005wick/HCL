
using System;
using System.Linq;

public class DelegatePractice
{
    // Example 1: Custom Delegate

    /*
    public delegate void delegateName(string m);

    public void Display(string msg)
    {
        Console.WriteLine(msg);
    }

    public void Run()
    {
        delegateName a = Display;
        a("hello");
    }
    */


    // Example 2: Action<string>

    /*
    // Action<string> accepts a string and returns nothing.

    public void Display(string msg)
    {
        Console.WriteLine(msg);
    }

    public void Run()
    {
        Action<string> m = Display;
        m("hello all");
    }
    */


    // Example 3: Func<string, char, int>

    /*
    // Func<string, char, int> accepts a string and a char,
    // and returns an integer.

    public int Check(string s, char ch)
    {
        return s.IndexOf(ch);
    }

    public void Run()
    {
        Func<string, char, int> m = Check;

        Console.WriteLine(m("world", 'r'));
        Console.WriteLine(m("hello", 'z'));
    }
    */


    // Example 4: Predicate<string>

    /*
    // Predicate<string> accepts a string and returns a bool.

    public bool Check(string s)
    {
        return s == new string(s.Reverse().ToArray());
    }

    public void Run()
    {
        Predicate<string> m = Check;

        Console.WriteLine(m("world"));
        Console.WriteLine(m("helleh"));
    }
    */


    // Example 5: Predicate<string> with Lambda Expression

    /*
    public void Run()
    {
        Predicate<string> m =
            msg => msg == new string(msg.Reverse().ToArray());

        Console.WriteLine(m("world"));
        Console.WriteLine(m("helleh"));
    }
    */


    // Example 6: Passing a Predicate as an Argument

    /*
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

        CheckString(
            "Hello",
            s => s.Distinct().Count() == s.Length
        );

        CheckString(
            "programming",
            s => s.Length > 8
        );

        CheckString(
            "level",
            s => s[0] == s[s.Length - 1]
        );
    }
    */


    // Example 7A: Generic Method with Predicate<T>

    /*
    public void CheckValue<T>(T t, Predicate<T> condition)
    {
        Console.WriteLine(condition(t));
    }

    public void Run()
    {
        CheckValue(
            "abcba",
            s => s == new string(s.Reverse().ToArray())
        );

        CheckValue(
            "level",
            s => s[0] == s[s.Length - 1]
        );

        CheckValue(
            new int[] { 1, 2, 4, 5 },
            arr => arr[0] == arr[arr.Length - 1]
        );
    }
    */


    // Example 7B: Generic Method with Func<T, TResult>

    /*
    public void CheckValue<T, TResult>(
        T t,
        Func<T, TResult> condition)
    {
        Console.WriteLine(condition(t));
    }

    public void Run()
    {
        CheckValue(
            "abcba",
            s => s == new string(s.Reverse().ToArray())
        );

        CheckValue(
            "level",
            s => s[0] + "" + s[s.Length - 1]
        );

        CheckValue(
            new int[] { 1, 2, 4, 5 },
            arr => arr[0] + arr[arr.Length - 1]
        );
    }
    */
}
