using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace graph
{
  


        /**
 * This class represents a directed graph with no parallel edges
 * 
 * @author morin
 *
 */
    class AdjacencyMatrix : Graph
    {
        protected int n;
        protected Boolean[][] a;

        /**
         * Create a new adjacency matrix with n vertices
         * @param n
         */
        public AdjacencyMatrix(int n0) {
		n = n0;

        //bool[,] seatArray = new bool[rows, cols]; //10 rows, 10 collums

        bool[,] a = new Boolean[n, n];
	}

        public void addEdge(int i, int j)
        {
            a[i][j] = true;
        }

        public void removeEdge(int i, int j)
        {
            a[i][j] = false;
        }

        public Boolean hasEdge(int i, int j)
        {
            try
            {
                return a[i][j];
            }
            
            catch(NullReferenceException nex)
            {
                return false;
            }

        }

        public List<int> outEdges(int i)
        {
            List<int> edges = new List<int>();
            for (int j = 0; j < n; j++)
                if (a[i][j]) edges.Add(j);
            return edges;
        }

        public List<int> inEdges(int i)
        {
            List<int> edges = new List<int>();
            for (int j = 0; j < n; j++)
                if (a[j][i]) edges.Add(j);
            return edges;
        }

        public int inDegree(int i)
        {
            int deg = 0;
            for (int j = 0; i < n; i++)
                if (a[j][i]) deg++;
            return deg;
        }

        public int outDegree(int i)
        {
            int deg = 0;
            for (int j = 0; i < n; i++)
                if (a[i][j]) deg++;
            return deg;
        }

        public int nVertices()
        {
            return n;
        }

    }
    
}


