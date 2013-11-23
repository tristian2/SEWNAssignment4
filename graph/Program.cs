using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace graph
{
    class Program
    {
        static void Main(string[] args)
        {

            for (int n = 10; n < 500; n *= 2)
            {
                Graph am = new AdjacencyMatrix(n);
                Graph al = new AdjacencyMatrix(n);
                Console.WriteLine("Running tests on graphs of size " + n + "...");

                Testum.graphTests(am, al);
                Console.WriteLine("done");

            }
        }
    }
}
