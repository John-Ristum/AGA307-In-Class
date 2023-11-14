using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 20;
    public float lifeSpan = 5;      //Seconds until projectile is destroyed

    void Start()
    {
        Destroy(this.gameObject, lifeSpan);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if(collision.gameObject.GetComponent<Target>() != null)
            {
                collision.gameObject.GetComponent<Target>().Hit();
            }
            
            //Change the colour ot the target
            //collision.gameObject.GetComponent<Renderer>().material.color = Color.red;
            //Destroy th target after 1 second
            //Destroy(collision.gameObject, 1);
            //Destroy this object
            //Destroy(this.gameObject);
        }
    }
}
