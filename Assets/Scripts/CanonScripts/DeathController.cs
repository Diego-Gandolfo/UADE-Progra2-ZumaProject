using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DeathController : MonoBehaviour //Necesitamos esto para setearle el color a la explosion y para decirle que se destruya luego de animarse. 
{
    [SerializeField] private float timer1;

    void Start()
    {
        timer1 += Time.time;
    }

    void Update()
    {
        if(Time.time > timer1)
        {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
