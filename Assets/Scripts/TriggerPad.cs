using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPad : MonoBehaviour
{
    public GameObject triggeredObject;

    private void OnTriggerEnter(Collider other)
    {
        //Change the colour of the triggered object
        triggeredObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    private void OnTriggerStay(Collider other)
    {
        //Increase the size of the triggered object by 0.01f
        triggeredObject.transform.localScale += Vector3.one * 0.01f;
        //Move the triggered object up
        //triggeredObject.transform.localPosition += Vector3.up * 0.01f;
    }

    private void OnTriggerExit(Collider other)
    {
        //reset the size of the triggered object
        triggeredObject.transform.localScale = Vector3.one;
        //reset the position of the triggered object
        //triggeredObject.transform.localPosition = Vector3.one; (broken)
        //revert the colour of the triggered object
        triggeredObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
