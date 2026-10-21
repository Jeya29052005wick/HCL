using System;
using System.Collections.Generic;
using System.Text;

namespace Filehandle
{
    internal class Filehand
    {
        public void Run()
        {
            string Path = "demo.txt";
            string content = "Hello";

            

            FileInfo f1 = new FileInfo(Path);
            Console.WriteLine(f1.IsReadOnly);
            
        }
    }
}
