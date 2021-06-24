using System;
using System.Collections.Generic;

class DijkstraTransform
{
    public static int[] distance;
    public static string[] nodos;

    private static int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
    {
        int min = int.MaxValue;
        int minIndex = 0;

        for (int v = 0; v < verticesCount; ++v)
        {
            // obtengo siempre el nodo con la menor distancia calculada
            // solo lo verifico en los nodos que no tienen seteado ya un camino (shortestPathTreeSet[v] == false)
            if (shortestPathTreeSet[v] == false && distance[v] <= min)
            {
                min = distance[v];
                minIndex = v;
            }
        }

        // devuelvo el nodo calculado
        return minIndex;
    }

    public static void Dijkstra(TDA_GrafoTransform grafo, int planet)
    {
        // obtengo la matriz de adyacencia del TDA_Grafo
        int[,] graph = grafo.MAdy;

        // obtengo la cantidad de nodos del TDA_Grafo
        int verticesCount = grafo.CantNodos;

        // obtengo el indice del nodo elegido como origen a partir de su valor
        var source = grafo.Vert2Indice(planet);

        // vector donde se van a guardar los resultados de las distancias entre 
        // el origen y cada vertice del grafo
        distance = new int[verticesCount];

        bool[] shortestPathTreeSet = new bool[verticesCount];

        int[] nodos1 = new int[verticesCount];
        int[] nodos2 = new int[verticesCount];

        for (int i = 0; i < verticesCount; ++i)
        {
            // asigno un valor maximo (inalcanzable) como distancia a cada nodo
            // cualquier camino que se encuentre va a ser menor a ese valor
            // si no se encuentra un camino, este valor maximo permanece y es el 
            // indica que no hay ningun camino entre el origen y ese nodo
            distance[i] = int.MaxValue;

            // seteo en falso al vector que guarda la booleana cuando se encuentra un camino
            shortestPathTreeSet[i] = false;

            nodos1[i] = nodos2[i] = -1;
        }

        // la distancia al nodo origen es 0
        distance[source] = 0;
        nodos1[source] = nodos2[source] = grafo.Etiqs[source];

        // recorro todos los nodos (vertices)
        for (int count = 0; count < verticesCount - 1; ++count)
        {
            int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
            shortestPathTreeSet[u] = true;

            // recorro todos los nodos (vertices)
            for (int v = 0; v < verticesCount; ++v)
            {
                // comparo cada nodo (que aun no se haya calculado) contra el que se encontro que tiene la menor distancia al origen elegido
                if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                {
                    // si encontré una distancia menor a la que tenia, la reasigno la nodo
                    distance[v] = distance[u] + graph[u, v];
                    // guardo los nodos para reconstruir el camino
                    nodos1[v] = grafo.Etiqs[u];
                    nodos2[v] = grafo.Etiqs[v];
                }
            }
        }

        // construyo camino de nodos
        nodos = new string[verticesCount];
        int nodOrig = grafo.Etiqs[source];
        for (int i = 0; i < verticesCount; i++)
        {
            if (nodos1[i] != -1)
            {
                List<int> l1 = new List<int>();
                l1.Add(nodos1[i]);
                l1.Add(nodos2[i]);
                while (l1[0] != nodOrig)
                {
                    for (int j = 0; j < verticesCount; j++)
                    {
                        if (j != source && l1[0] == nodos2[j])
                        {
                            l1.Insert(0, nodos1[j]);
                            break;
                        }
                    }
                }
                for (int j = 0; j < l1.Count; j++)
                {
                    if (j == 0)
                    {
                        nodos[i] = l1[j].ToString();
                    }
                    else
                    {
                        nodos[i] += "," + l1[j].ToString();
                    }
                }
            }
        }
    }
}
