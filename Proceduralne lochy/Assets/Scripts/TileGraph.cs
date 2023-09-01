using System.Collections.Generic;
using UnityEngine;

public class TileGraph
{
    public List<Tile> Tiles;
    public List<Edge> Edges;

    public TileGraph (List<Tile> mainTiles)
    {
        Tiles = mainTiles;

        Edges = new List<Edge> ();
        List<Tile> temp;
        Edge temp_edge;
        for (int i = 0; i < Tiles.Count; i++)
        {
            temp = new List<Tile> (Tiles);
            temp.Remove (Tiles[i]);
            temp.Sort ((tile1, tile2) => Vector3.Distance (tile1.Center, Tiles[i].Center).CompareTo (Vector3.Distance (tile2.Center, Tiles[i].Center)));

            temp_edge = new Edge (Tiles[i].ID, temp[0].ID, Random.Range(0f, 10f), Edges.Count);
            Edges.Add (temp_edge);

            temp_edge = new Edge (Tiles[i].ID, temp[1].ID, Random.Range (0f, 10f), Edges.Count);
            Edges.Add (temp_edge);

            temp_edge = new Edge (Tiles[i].ID, temp[2].ID, Random.Range (0f, 10f), Edges.Count);
            Edges.Add (temp_edge);

            temp_edge = new Edge (Tiles[i].ID, temp[3].ID, Random.Range (0f, 10f), Edges.Count);
            Edges.Add (temp_edge);
        }

        Edges.Sort ();

        List<Edge> mstEdges = new List<Edge> ();

        DisjointSet disjointSet = new DisjointSet (Tiles.Count);

        foreach (Edge edge in Edges)
        {
            if (disjointSet.Find (edge.Start) != disjointSet.Find (edge.End))
            {
                mstEdges.Add (edge);
                disjointSet.Union (edge.Start, edge.End);
            }
        }

        int percent = (int) (Edges.Count * 0.18);

        foreach(Edge edge in mstEdges)
        {
            Edges.Remove (edge);
        }

        int index;
        bool diferent;
        for (int i = 0; i < percent; i++)
        {
            index = Random.Range (0, Edges.Count);

            diferent = true;
            foreach (Edge e in mstEdges)
            {
                if (e.ID == Edges[index].ID || (e.Start == Edges[index].End && Edges[index].Start == e.End))
                {
                    diferent = false;
                    break;
                }
            }

            if (diferent)
                mstEdges.Add (Edges[index]);

            Edges.Remove (Edges[index]);
        }

        Edges = mstEdges;
    }
}

public class Edge : System.IComparable<Edge>
{
    public int Start { get; private set; }
    public int End { get; private set; }
    public float Weight { get; private set; }
    public int ID { get; private set; }

    public Edge (int start, int end, float weight, int id)
    {
        Start = start;
        End = end;
        Weight = weight;
        ID = id;
    }

    public int CompareTo (Edge other)
    {
        return Weight.CompareTo (other.Weight);
    }
}

public class DisjointSet
{
    private int[] parent;

    public DisjointSet (int size)
    {
        parent = new int[size];
        for (int i = 0; i < size; i++)
        {
            parent[i] = i;
        }
    }

    public int Find (int element)
    {
        if (parent[element] == element)
        {
            return element;
        }
        return Find (parent[element]);
    }

    public void Union (int set1, int set2)
    {
        int root1 = Find (set1);
        int root2 = Find (set2);
        parent[root2] = root1;
    }
}