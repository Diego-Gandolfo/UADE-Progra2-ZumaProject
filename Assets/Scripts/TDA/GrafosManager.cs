using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IGrafosManager
{
    Transform[] GetDijkstra(int startingIndex, int pathNumber);
}

public class GrafosManager : MonoBehaviour, IGrafosManager
{
    [SerializeField] private GameObject path_1 = null;
    [SerializeField] private GameObject path_2 = null;
    private Transform[] arrayPathOne = new Transform[0];
    private Transform[] arrayPathTwo = new Transform[0];
    private TDA_GrafoTransform pointsGraphOne = new TDA_GrafoTransform();
    private TDA_GrafoTransform pointsGraphTwo = new TDA_GrafoTransform();

    void Awake()
    {
        if (path_1 != null)
            arrayPathOne = ConvertToArray(path_1, arrayPathOne);
        
        if (path_2 != null)
            arrayPathTwo = ConvertToArray(path_2, arrayPathOne);
    }

    void Start()
    {
        if (path_1 != null)
            InitializeGraph(pointsGraphOne, arrayPathOne);

        if (path_2 != null)
            InitializeGraph(pointsGraphTwo, arrayPathTwo);
    }

    public Transform[] GetDijkstra(int startingIndex, int pathNumber) //TODO: Ver si se puede mejorar esta parte
    {
        int index;
        print("dijkstra: " + pathNumber);
        if (pathNumber == 1 && path_1 != null)
        {
            DijkstraTransform.Dijkstra(pointsGraphOne, startingIndex);
            index = pointsGraphOne.Vert2Indice(arrayPathOne.Length - 1);
        }
        else if(pathNumber == 2 && path_2 != null)
        {
            DijkstraTransform.Dijkstra(pointsGraphTwo, startingIndex);
            index = pointsGraphTwo.Vert2Indice(arrayPathTwo.Length - 1);
        }
        else
        {
            print("Huston, tenemos un problema");
            return null;
        }

        var resultado = DijkstraTransform.nodos[index];
        var peso = DijkstraTransform.distance[index];

        string[] splitString = resultado.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        Transform[] recorrido = new Transform[splitString.Length];

        for (int i = 0; i < splitString.Length; i++)
        {
            recorrido[i] = arrayPathOne[Convert.ToInt32(splitString[i])];
        }

        return recorrido;
    }

    private Transform[] ConvertToArray(GameObject path, Transform[] transformArray) //Convierte el GameObject que tiene los puntos en un array de Transforms
    {
        transformArray = new Transform[path_1.transform.childCount];
        for (int i = 0; i < path_1.transform.childCount; i++)
        {
            transformArray[i] = path.transform.GetChild(i).GetComponent<Transform>();
        }
        return transformArray;
    }

    private void InitializeGraph(TDA_GrafoTransform graph, Transform[] arrayTransform) //Convierte el array de transform en un tda de grafos. 
    {
        print("inicializo grafos, array: " + arrayTransform.Length);
        graph.InicializarGrafo(arrayTransform.Length);

        for (int i = 0; i < arrayTransform.Length; i++)
        {
            graph.AgregarVertice(i);
        }

        for (int i = 0; i < arrayTransform.Length - 1; i++)
        {
            graph.AgregarArista(i, (i + 1), 1); //La posicion actual, la que sigue y el peso que siempre es el mismo
        }
    }
}
