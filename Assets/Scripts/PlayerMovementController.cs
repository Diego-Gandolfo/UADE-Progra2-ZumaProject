using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * movementSpeed);
        this.transform.Rotate(new Vector3(0, Time.deltaTime * Input.GetAxis("Horizontal") * rotationSpeed ,0));
    }
}
