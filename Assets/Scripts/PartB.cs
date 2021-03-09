using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartB : MonoBehaviour
{

    public GameObject WorldA; // 1 этап
    public GameObject WorldB; // 2 этап
    public GameObject Sea; // Лёд
    public GameObject EffrctPartB; // Эфект точки перехода между этапами

    public AudioSource SoundBackGround; // Фоновая музыка
    public AudioClip SoundBackGroundPart2; // Клип для второго этапа 

    // Для смены этапа
    bool transition1 = false;
    bool transition2 = false;


    private void Update()
    {
        // Если смена этапа запущена
        if (transition1)
        {
            // Море поднимается и музыка затихает
            Sea.transform.position = Vector3.MoveTowards(Sea.transform.position, new Vector3(0, 15f, 0), Time.deltaTime * 1);
            SoundBackGround.volume = Mathf.MoveTowards(SoundBackGround.volume, 0, Time.deltaTime * 0.015f);

            // Когда море поднялось
            if (Sea.transform.position.y == 15) 
            {
                // Смена музыки
                SoundBackGround.Stop();
                SoundBackGround.clip = SoundBackGroundPart2;
                SoundBackGround.Play();

                // Включение 2го этапа
                Destroy(EffrctPartB);
                Destroy(WorldA);
                transition1 = false;
                transition2 = true;
                WorldB.transform.position = new Vector3(-44.64f, 0, 3.48f);
            }
        }
        if (transition2)
        {
            // Море опускается и музыка становится громче
            Sea.transform.position = Vector3.MoveTowards(Sea.transform.position, new Vector3(0, -5, 0), Time.deltaTime * 1);
            SoundBackGround.volume = Mathf.MoveTowards(SoundBackGround.volume, 0.18f, Time.deltaTime * 0.015f);
            if (Sea.transform.position.y == -5)
            {
                // удаляем море (в принцепе незачем)
                Destroy(gameObject); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Запуск смены этапов
            transition1 = true;
            Destroy(GetComponent<Collider>());
        }
    }
}
