using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactGroundSensor : MonoBehaviour
{
    //private string specialObjectsTag = "Ground";
    public int touchingObjectCount;
    public string boolParameterName = "Grounded";
    public GameObject Player;
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 7)
        {
            touchingObjectCount++;
            if (touchingObjectCount > 0)
            {

                MainCharacter MC = Player.GetComponent<MainCharacter>();
                MC.InFloor();
                anim.SetBool(boolParameterName, true);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        touchingObjectCount--;
        if (touchingObjectCount < 1)
        {

            MainCharacter MC = Player.GetComponent<MainCharacter>();
            MC.OutofFloor();
            anim.SetBool(boolParameterName, false);
        }
    }
}
