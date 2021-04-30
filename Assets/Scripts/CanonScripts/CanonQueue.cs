using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonQueue : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private GameObject ballPrefab;
    private QueueDymamic queueDymamic;
    private int counter;
    private Camera mainCamera;
    private Ball selectedSphere;
    [SerializeField]private float shootSpeed;
    [SerializeField] private Transform shootPoint;

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
            var proyectile = Instantiate(ballPrefab, shootPoint.position,shootPoint.rotation);
            var proyectileScript =  proyectile.GetComponent<Ball>();
            proyectileScript.imProyectile = true;
            proyectileScript.speed = shootSpeed ;
        }
    }
}
