using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avaruz.FrameWork.Utils.Common;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var google2 = new Googlelizer();
            Console.WriteLine(google2.GetMyGoogle("TALAVERA ANTELO OSCAR"));

            Console.ReadLine();
        }
    }
}
