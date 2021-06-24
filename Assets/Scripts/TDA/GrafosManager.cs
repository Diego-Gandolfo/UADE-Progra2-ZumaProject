using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IGrafosManager
{
    Transform[] GetDijkstra(int startingIndex);
}

public class GrafosManager : MonoBehaviour, IGrafosManager
{
    [SerializeField] private Transform[] pointsArray = new Transform[0];
    [SerializeField] private Transform[] pointsTry;
    [SerializeField] private GameObject points = null;
    private TDA_GrafoTransform pointsGraph = new TDA_GrafoTransform();
    private int endingIndex;

    void Awake()
    {
        if (points != null)
        {
            pointsArray = new Transform[points.transform.childCount];
            for (int i = 0; i < points.transform.childCount; i++)
            {
                pointsArray[i] = points.transform.GetChild(i).GetComponent<Transform>();
            }
        }
        else
        {
            print("error con grafos");
        }
    }

    void Start()
    {
        pointsGraph.InicializarGrafo(pointsArray.Length);

        for (int i = 0; i < pointsArray.Length; i++)
        {
            pointsGraph.AgregarVertice(i);
        }

        for (int i = 0; i < pointsArray.Length - 1; i++)
        {
            pointsGraph.AgregarArista(i, (i + 1), 1); //La posicion actual, la que sigue y el peso que siempre es el mismo
        }

        endingIndex = pointsArray.Length - 1; //menos uno para que no sea out of bounds
    }

    public Transform[] GetDijkstra(int startingIndex)
    {
        DijkstraTransform.Dijkstra(pointsGraph, startingIndex);
        var index = pointsGraph.Vert2Indice(endingIndex);
        var resultado = DijkstraTransform.nodos[index];
        var peso = DijkstraTransform.distance[index];

        string[] splitString = resultado.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        Transform[] recorrido = new Transform[splitString.Length];

        for (int i = 0; i < splitString.Length; i++)
        {
            recorrido[i] = pointsArray[Convert.ToInt32(splitString[i])];
        }

        return recorrido;
    }
}
