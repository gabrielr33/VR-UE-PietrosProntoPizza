using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoSauceFiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<TomatoSauceFillingManager>() != null)
        { 
            other.transform.GetComponent<TomatoSauceFillingManager>().FillSpoon(); 
        }
        
    }

}
