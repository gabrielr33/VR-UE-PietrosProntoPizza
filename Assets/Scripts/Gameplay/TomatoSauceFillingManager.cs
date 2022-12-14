using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoSauceFillingManager : MonoBehaviour
{

    [SerializeField] private bool isSpoonFilled = false;

    // Start is called before the first frame update
    void Start()
    {
        isSpoonFilled = true;
        FillSpoon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmptySpoon()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        isSpoonFilled=false;
    }

    public void FillSpoon()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        isSpoonFilled = true;
    }


    public bool GetIsSpoonFilled()
    {
        return isSpoonFilled;
    }

}
