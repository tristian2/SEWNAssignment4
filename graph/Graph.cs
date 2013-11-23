using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace graph
{
    /**
     * This interface represents a directed graph whose vertices are
     * indexed by 0,...,nVertices()-1
     * @author morin
     *
     */
    public interface Graph {
	     int nVertices();
	     void addEdge(int i, int j);
	     void removeEdge(int i, int j);
	     Boolean hasEdge(int i, int j);
	     List<int> outEdges(int i);
	     List<int> inEdges(int i);
	     int outDegree(int i);
	     int inDegree(int i);
    }
}



