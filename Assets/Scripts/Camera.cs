using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Transform PlayerTransform;
    public Rigidbody PlayerRigidbody;

    // Хранит последние скорости движения
    private List<Vector2> velocities  = new List<Vector2>();


    private void Start()
    {
        // Обнуляем чтобы не было ошибок
        for (int i = 0; i < 10; i++)
        {
            velocities.Add(Vector2.zero);
        }
    }


    private void FixedUpdate()
    {
        // Обновляем скорости только если происходит движение
        if (PlayerRigidbody.velocity != Vector3.zero)
        {
            velocities.Add(new Vector2(PlayerRigidbody.velocity.x, PlayerRigidbody.velocity.z));
            velocities.RemoveAt(0);
        }
    }


    void LateUpdate()
    {
        Vector2 summ = Vector2.zero;

        for (int i = 0; i < velocities.Count; i++)
        {
            summ += velocities[i];
        }

        transform.position = PlayerTransform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3( summ.x,0,summ.y)), Time.deltaTime * 1);
    }
}
