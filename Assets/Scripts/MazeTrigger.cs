using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTrigger : MonoBehaviour
{

    public GameObject AngleCam;

    private bool mazePlau = false; // Играем лабиринт?
    private bool updatePlay = false; // Выполнять проверки?


    private void Update()
    {
        if (updatePlay) 
        {
            if (mazePlau)
            {
                // Поворот камеры в положение прохождения лабиринта
                AngleCam.transform.localRotation = Quaternion.Lerp(AngleCam.transform.localRotation, new Quaternion(0.5f, 0, 0, 1), Time.deltaTime * 1); 
            }
            else
            {
                // Поворот камеры в стандартное положение
                AngleCam.transform.localRotation = Quaternion.Lerp(AngleCam.transform.localRotation, new Quaternion(0, 0, 0, 1), Time.deltaTime * 1);

                // Поворот окончен, отключаем проверки (типо оптимизация)
                if (AngleCam.transform.localRotation == new Quaternion(0, 0, 0, 1))
                {
                    updatePlay = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        mazePlau = true;
        updatePlay = true;
    }

    private void OnTriggerExit(Collider other)
    {
        mazePlau = false;
    }
}
