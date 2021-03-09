using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode Key_Up;
    public KeyCode Key_Down;
    public KeyCode Key_Right;
    public KeyCode Key_Left;
    public KeyCode Key_Decrease;
    public KeyCode Key_Increase;
    public KeyCode Key_Jump;

    private bool isGrounded;

    public Transform CameraViewDirection;

    private Rigidbody rigidbody;
    private AudioSource audioSource;

    public float Speed = 10f;

    private Vector3 move;
    private Vector2 target;
    [HideInInspector] public float size { get; private set; }

    public bool changingSize = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 500;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(Key_Jump) && isGrounded)
        {
            rigidbody.AddForce(CameraViewDirection.up * 5, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        target = Vector3.zero;

        if (Input.GetKey(Key_Decrease)) {
            size += 0.1f;
        }
        if (Input.GetKey(Key_Increase)) {
            size -= 0.1f;
        }

        ReturningValue();

        if (Input.GetKey(Key_Up))
        {
            target.x = 1;
        }
        if (Input.GetKey(Key_Down))
        {
            target.x = -1;
        }
        if (Input.GetKey(Key_Right))
        {
            target.y = -1;
        }
        if (Input.GetKey(Key_Left))
        {
            target.y = 1;
        }

        // Тут пытался как-то насроить звучание
        if (isGrounded)
        {
            audioSource.volume = rigidbody.velocity.magnitude / 5;
            audioSource.pitch = Mathf.Clamp((0.8f + rigidbody.velocity.magnitude / 20), 0.8f, 1.5f);
        }
        else { audioSource.volume -= 0.3f; }

        // тут приравниваю движение чтобы было одинаковое при разных размерах шара
        move = (CameraViewDirection.right * target.x) + (CameraViewDirection.forward * target.y);
        move *= Speed / (Mathf.PI * size);

        rigidbody.AddTorque(move, ForceMode.VelocityChange);
        rigidbody.AddForce(0, -1, 0);

        rigidbody.mass = size = (float)System.Math.Round(Mathf.Clamp(size, 0.5f, 1.5f), 1);
        transform.localScale = Vector3.one * size;
    }

    private void ReturningValue()
    {
        // Если не нажаты кнопки изменения размера, то возвращаем размер к стандартному
        if (size == 1 || Input.GetKey(Key_Decrease) || Input.GetKey(Key_Increase) ) return;

        if (size != 1 && size > 1)
        {
            size -= 0.1f;
        }
        else
        if (size != 1 && size < 1)
        {
            size += 0.1f;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Просматриваем возможен ли прыжок
        if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5)
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}