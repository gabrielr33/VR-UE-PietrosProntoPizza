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
        fillSpoon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void emptySpoon()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        isSpoonFilled=false;
    }

    public void fillSpoon()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        isSpoonFilled = true;
    }


    public bool getIsSpoonFilled()
    {
        return isSpoonFilled;
    }

}
