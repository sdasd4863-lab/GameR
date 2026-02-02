using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prisosa : MonoBehaviour
{
    public Text box;
    public int i;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "zz" && i <=3)
        {
            i++;


        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "zz")
        {
            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;

           
            box.text = i.ToString();
        }
    }
}
