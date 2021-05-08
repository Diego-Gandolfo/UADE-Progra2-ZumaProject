using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnceUponAMemory.Diego
{
    public class PatrolPoints : MonoBehaviour
    {
        [SerializeField] private bool random = false; // Variable para determinar si se mueve de forma aleatoria o secuencial

        [Header("Patrol Settings")]
        [SerializeField] private float movemetSpeed = 3.0f; // La velocidad que se desplaza al estar patrullando
        [SerializeField] private float waitTime = 1.0f; // El tiempo que espera hasta en un punto antes de empezar a moverse al siguiente
        private float timer = 0.0f; // Variable que usaremos para llevar el control del tiempo

        [Header("Patrol Points")]
        [SerializeField] private Transform[] patrolPoints = null; // Array (vector o "lista") donde almacenaremos los puntos a patrullar
        private int nextPoint = 0;

        private void Start()
        {
            if (random) nextPoint = Random.Range(0, patrolPoints.Length); // Inicializamos un punto aleatorio
            if (!random) nextPoint = 0; // Inicializamos el punto de comienzo para cuando recorremos de forma secuencial
            timer = waitTime; // Inicializamos el contador al tiempo de espera deseado
        }

        private void Update()
        {
            if (random) // Si random es TRUE es que queremos que se mueva de forma aleatoria
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[nextPoint].position, movemetSpeed * Time.deltaTime); // Nos movemos al punto indicado (randomPoint)

                if (Vector2.Distance(transform.position, patrolPoints[nextPoint].position) < 0.2f) // Nos fijamos si ya estamos cerca del punto indicado (randomPoint)
                {
                    if (timer <= 0) // Comprobamos si ya paso el tiempo de espera deseado
                    {
                        nextPoint = Random.Range(0, patrolPoints.Length); // Asignamos un nuevo punto aleatorio
                        timer = waitTime; // Reinicializamos el Timer
                    }
                    else
                    {
                        timer -= Time.deltaTime; // Si el tiempo no paso, vamos restando Time.deltaTime
                    }
                }
            }
            else if (!random) // Si random es FALSE es que queremos que se mueva de forma secuencial
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[nextPoint].position, movemetSpeed * Time.deltaTime); // Nos movemos al punto indicado (currentPoint)

                if (Vector2.Distance(transform.position, patrolPoints[nextPoint].position) < 0.2f) // Nos fijamos si ya estamos cerca del punto indicado (currentPoint)
                {
                    if (timer <= 0) // Comprobamos si ya paso el tiempo de espera deseado
                    {
                        nextPoint++; // Vamos al siguiente punto

                        if (nextPoint >= patrolPoints.Length) // Nos fijamos si el siguiente punto se sale del rango del Array
                        {
                            nextPoint = 0; // Volvemos el currentPoint al punto de inicio
                        }

                        timer = waitTime; // Reseteamos el contador
                    }
                    else
                    {
                        timer -= Time.deltaTime; // Si el tiempo no paso, vamos restando Time.deltaTime
                    }
                }
            }
        }
    }
}
