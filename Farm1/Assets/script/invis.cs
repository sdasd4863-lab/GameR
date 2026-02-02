using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invis : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ss")
        {
            
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ss")
        {

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;

        }
    }
}
