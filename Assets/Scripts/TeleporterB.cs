using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterB : MonoBehaviour
{

    public GameObject PositionA;
    public GameObject ActivatedPortal;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PositionA.SetActive(true);
            ActivatedPortal.SetActive(true);
            Destroy(ActivatedPortal, 5);
            Destroy(this);
        }
    }
}
