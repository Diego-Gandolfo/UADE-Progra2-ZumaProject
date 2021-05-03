using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonQueue : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    private QueueDymamic queueDymamic;
    private int counter;
    private Camera mainCamera;
    private Ball selectedSphere;
    [SerializeField] private float shootSpeed;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private QueueShootingTest queueShootingTest;
    [SerializeField] private Ball proyectile;
    private bool isAlredyShooted;

    private void Start()
    {
        mainCamera = Camera.main;
        InstanceProyectile();
    }

    private void Update()
    {
        Vector3 worldScreenPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 diff = worldScreenPosition - transform.position;
        transform.up = diff.normalized;
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            proyectile.imProyectile = true;
            proyectile.speed = shootSpeed;
            proyectile = null;        
        }
        else if (proyectile != null)
        {
            proyectile.transform.position = shootPoint.position;
            proyectile.transform.rotation = shootPoint.rotation;
        }
    }

    public void InstanceProyectile()
    {
            proyectile = queueShootingTest.CreateClone();
    }
}
