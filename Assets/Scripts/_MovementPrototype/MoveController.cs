using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; // Esto debería estar en el LevelManager, son los puntos a recorrer
    private Transform nextWaypoint; // cual es el siguiente punto al que tenemos que dirigirnos
    private int index; // el index por el que vamos

    [SerializeField] private float movementSpeed;

    private void Start()
    {
        transform.position = waypoints[0].position; // que arranque en el primer punto
        index = 0;
        LoadNextWaipont(); // que asigne el siguiente punto
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, Time.deltaTime * movementSpeed); // nos movemos hacia el punto

        if (HasReachWaypoint()) // si ya estamos cerca
            LoadNextWaipont(); // que asigne el siguiente punto
    }

    private bool HasReachWaypoint()
    {
        var distance = Vector3.Distance(transform.position, nextWaypoint.position); // calculamos la distancia entre nosotros y el punto
        return (distance <= .1f); // si estamos cerca devuelve true
    }

    private void LoadNextWaipont()
    {
        index++;
        if (index >= waypoints.Length) EndOfRoad(); // si es el ultimo punto avisa que llegamos al final
        else nextWaypoint = waypoints[index]; // sino asigna el siguiente punto
        transform.right = nextWaypoint.position - transform.position; // nos deja mirando al ultimo punto
    }

    private void EndOfRoad()
    {
        print($"{gameObject.name} llego a destino");
        Destroy(this);
    }
}
