using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float reachDistance;
    private Transform nextWaypoint;
    private int index;

    [SerializeField] private float movementSpeed;

    private void Start()
    {
        transform.position = waypoints[0].position;
        index = 0;
        LoadNextWaipont();
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, Time.deltaTime * movementSpeed);

        if (HasReachWaypoint())
            LoadNextWaipont();
    }

    private bool HasReachWaypoint()
    {
        var distance = Vector3.Distance(transform.position, nextWaypoint.position);
        return (distance <= reachDistance);
    }

    private void LoadNextWaipont()
    {
        index++;
        if (index >= waypoints.Length) EndOfRoad();
        else nextWaypoint = waypoints[index];
        transform.right = nextWaypoint.position - transform.position;
    }

    private void EndOfRoad()
    {
        print($"{gameObject.name} llego a destino");
        Destroy(this);
    }
}
