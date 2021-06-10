using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    static public Waypoints current;

    [SerializeField] private List<Transform> waypoints;

    void Awake()
    {
        if (current != null) Destroy(this.gameObject);
        current = this;
    }

    public Transform GetTransform(int index)
    {
        return IsLastWaypoint(index) ? null : waypoints[index];
    }

    public bool IsLastWaypoint(int index)
    {
        return index >= waypoints.Count;
    }
}
