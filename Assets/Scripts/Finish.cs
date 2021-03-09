using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{

    public GameObject FinishEffects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FinishEffects.SetActive(true);
        }
    }
}
