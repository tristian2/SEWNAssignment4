using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace graph
{
    /**
     * A utility class with some static methods for testing List implementations
     * @author morin
     */
    public class Testum
    {


        public static void graphTests(Graph g1, Graph g2)
        {
            int n = g1.nVertices();
            Random rand = new Random();
            for (int k = 0; k < 50 * n * n; k++)
            {
                int i = rand.Next(n);
                int j = rand.Next(n);
                if (i != j)
                {
                    if (g1.hasEdge(i, j))
                    {
                        g1.removeEdge(i, j);
                        g2.removeEdge(i, j);
                    }
                    else
                    {
                        g1.addEdge(i, j);
                        g2.addEdge(i, j);
                    }
                }
                graphCmp(g1, g2);
            }
        }

        
        public static void graphCmp(Graph g1, Graph g2) {
		    /*int n = g1.nVertices();
		    Utils.myassert(n == g2.nVertices());
		    for (int i = 0; i < n; i++) {
			    for (int j = 0; j < n; j++) {
				    Utils.myassert(g1.hasEdge(i,j) == g2.hasEdge(i,j));
			    }
		    }
		    for (int i = 0; i < n; i++) {
			    List<int> l1 = g1.outEdges(i);
			    List<int> l2 = g2.outEdges(i);
			    for (int x: l1) Utils.myassert(l2.contains(x));
			    for (int x: l2) Utils.myassert(l1.contains(x));
		    }		
		    for (int i = 0; i < n; i++) {
			    List<int> l1 = g1.inEdges(i);
			    List<int> l2 = g2.inEdges(i);
			    for (int x: l1) Utils.myassert(l2.contains(x));
			    for (int x: l2) Utils.myassert(l1.contains(x));
		    }	* */	
	    }
         

    }
}




	

	
	

	