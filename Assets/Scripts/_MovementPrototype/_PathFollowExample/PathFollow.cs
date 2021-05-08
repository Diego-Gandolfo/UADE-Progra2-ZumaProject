using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    [SerializeField] private List<Transform> targets;
    [SerializeField] private Transform nextTarget;
    [SerializeField] private float speed;
    [SerializeField] private float lookatSpeed;
    [SerializeField] private int indexTarget;
    [SerializeField] private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = targets[0].position;
        indexTarget = 0;
        LoadNextTarget();
        this.transform.LookAt(nextTarget);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = nextTarget.position - this.transform.position;
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * lookatSpeed);
        
        transform.position = Vector3.MoveTowards(transform.position, nextTarget.position, Time.deltaTime * speed);

        if (IsOverTarget())
        {
            LoadNextTarget();
        }
    }

    private void LoadNextTarget()
    {
        indexTarget++;
        nextTarget = targets[indexTarget];
        
        destination = nextTarget.position;
    }

    private bool IsOverTarget()
    {
        var distance = Vector3.Distance(this.transform.position, nextTarget.position);
        return distance < .1f ? true : false;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "End Of Road")
        {
            Destroy(this.gameObject);
        }
    }
}
