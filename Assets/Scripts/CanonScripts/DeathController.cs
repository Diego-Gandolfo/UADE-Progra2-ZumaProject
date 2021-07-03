using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField] private float timer1;

    void Start()
    {
        timer1 += Time.time;
    }

    void Update()
    {
        if(Time.time > timer1)         //Primer timer, para instanciar luego de la animacion de muerte, la reward.
        {
            Destroy(gameObject);
        }
    }

    public void SetColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
