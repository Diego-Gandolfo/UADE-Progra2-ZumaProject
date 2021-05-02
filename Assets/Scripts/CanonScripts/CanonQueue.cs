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
    [SerializeField]private float shootSpeed;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private QueueShootingTest queueShootingTest;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 worldScreenPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 diff = worldScreenPosition - transform.position;
        transform.up = diff.normalized;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var proyectile = queueShootingTest.CreateClone();
            proyectile.transform.position = shootPoint.position;
            proyectile.transform.rotation = shootPoint.rotation;
            proyectile.imProyectile = true;
            proyectile.speed = shootSpeed;
        }
    }
}
