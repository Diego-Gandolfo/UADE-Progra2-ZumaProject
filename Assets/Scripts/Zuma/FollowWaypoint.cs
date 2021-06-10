using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float distance;
    [SerializeField] private int currentIndex;

    private void Start()
    {
        currentIndex = 0;
        target = Waypoints.current.GetTransform(currentIndex);
    }

    void Update()
    {
        RotateAndFollow();
        CheckDistance();
    }

    private void RotateAndFollow()
    {
        Vector3 relativePos = (target.position + new Vector3(0, .25f, 0)) - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
        transform.Translate(0, 0, 1.5f * Time.deltaTime);
    }

    private void CheckDistance()
    {
        distance = Vector3.Distance(target.position, transform.position);

        if (distance <= 1f) 
        {
            currentIndex++;
            target = Waypoints.current.GetTransform(currentIndex); 
        }

        if (target == null) Destroy(this.gameObject);
    }
}
