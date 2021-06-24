using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueStartTrigger : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision) //Todas las pelotas que empiezan esta con el collider desactivado
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null && !ball.IsProjectile)
        {
            ball.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null && !ball.IsProjectile)
        {
            ball.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
