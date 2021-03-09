using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterA : MonoBehaviour
{

    public Transform PositionB;
    public AudioSource Sound;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = new Vector3(PositionB.position.x, PositionB.position.y + (other.GetComponent<PlayerController>().size / 2), PositionB.position.z);
            Sound.Play();
        }
    }
}
