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

    public float SpeedOfResizing = 1;

    private bool isGrounded;

    public Transform CameraViewDirection;

    private Rigidbody rigidbody;
    private AudioSource audioSource;

    public float Speed = 10f;

    private Vector3 move;
    private Vector2 target;
    [HideInInspector] public float size { get; private set; }
    private float targetSize;

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

        // Проверка свободного места рядом с шаром
        float freeSpace = CheckingAmountSpaceToIncrease(size / 2 - 0.02f);

        
        if (Input.GetKey(Key_Decrease))
        {
            targetSize -= SpeedOfResizing;
        }
        if (Input.GetKey(Key_Increase))
        {
            if (freeSpace > 2f)
            {
                targetSize += SpeedOfResizing;
            }
        }
       

        ReturningValue(freeSpace);


        targetSize = Mathf.Clamp(targetSize, 0.5f, 1.5f);
        size = Mathf.MoveTowards(size, targetSize, 0.1f);
            

        if (Input.GetKey(Key_Up)) target.x = 1;
        if (Input.GetKey(Key_Down)) target.x = -1;
        if (Input.GetKey(Key_Left)) target.y = 1;
        if (Input.GetKey(Key_Right)) target.y = -1;

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
        //rigidbody.AddForce(0, -1, 0);

        rigidbody.mass = size = (float)System.Math.Round(Mathf.Clamp(size, 0.5f, 1.5f), 1);
        transform.localScale = Vector3.one * size;
    }

    private void ReturningValue(float freeSpace)
    {
        // Если не нажаты кнопки изменения размера, то возвращаем размер к стандартному
        if (size == 1 || Input.GetKey(Key_Decrease) || Input.GetKey(Key_Increase)) return;

        if (size != 1 && size > 1)
        {
            targetSize -= SpeedOfResizing;
        }
        else
        if (size != 1 && size < 1)
        {
            if (freeSpace > 1.5f)
            {
                targetSize += SpeedOfResizing;
            }
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

    private float CheckingAmountSpaceToIncrease(float currentRadius)
    {
        float amountFreeSpace = Mathf.Infinity;

        amountFreeSpace = ChoosingSmallest(amountFreeSpace,CheckingSpaceOnAxis(Vector3.forward, currentRadius));
        amountFreeSpace = ChoosingSmallest(amountFreeSpace, CheckingSpaceOnAxis(Vector3.right, currentRadius));
        amountFreeSpace = ChoosingSmallest(amountFreeSpace, CheckingSpaceOnAxis(Vector3.up, currentRadius));
        amountFreeSpace = ChoosingSmallest(amountFreeSpace, CheckingSpaceOnAxis(transform.forward, currentRadius));
        amountFreeSpace = ChoosingSmallest(amountFreeSpace, CheckingSpaceOnAxis(transform.right, currentRadius));
        amountFreeSpace = ChoosingSmallest(amountFreeSpace, CheckingSpaceOnAxis(transform.up, currentRadius));
        return amountFreeSpace;
    }
    private float CheckingSpaceOnAxis(Vector3 axis, float currentRadius)
    {

        float summDistance = 0f;

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, currentRadius, axis, out hit))
        {
            summDistance += hit.distance + currentRadius;
        }
        else
        {
            summDistance += Mathf.Infinity;
        }

        if (Physics.SphereCast(transform.position, currentRadius, -axis, out hit))
        {
            summDistance += hit.distance + currentRadius;
        }
        else
        {
            summDistance += Mathf.Infinity;
        }

        return summDistance;
    }

    private float ChoosingSmallest(float value1, float value2)
    {
        if (value2 < value1)
        {
            value1 = value2;
        }

        return value1;
    }
}