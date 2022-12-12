using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaPositionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = transform.position;


        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
