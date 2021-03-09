using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public Material pointerMaterial;
    public Transform PlayerTransform;
    public PlayerController PlayerController;
    public AudioSource CollectingPoint;
    public Text PointCountText;

    public int PointCount { get; private set; }

    private GameObject magicBallsMinDistance;
    private int idMagicBallsMinDistance;
    private List<GameObject> magicBalls = new List<GameObject>();


    private void Start()
    {
        // Находим все поинты на карте и переносим в список
        GameObject[] gameObjectMagicBalls = GameObject.FindGameObjectsWithTag("MagicBall");
        for (int i = 0; i < gameObjectMagicBalls.Length; i++)
        {
            magicBalls.Add(gameObjectMagicBalls[i]);
        }
        PointCountText.text = "0"; // обнуление при старие
    }


    private void FixedUpdate()
    {
        // ищем ближайший поинт
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < magicBalls.Count; i++)
        {
            float distance = Vector3.Distance(PlayerTransform.position, magicBalls[i].transform.position);
            if (minDistance > distance)
            {
                minDistance = distance;
                magicBallsMinDistance = magicBalls[i];
                idMagicBallsMinDistance = i;
            }
        }

        // Собираем поинт
        if (minDistance < 1)
        {
            magicBalls.RemoveAt(idMagicBallsMinDistance);
            Destroy(magicBallsMinDistance);
            CrazePoints(1);
        }

        // Включаем и выключаем стрелку
        if (magicBallsMinDistance)
        {
            pointerMaterial.color = new Color(0, 1, 0.9f, 1); // Если она была выключенна ранее
        }
        else
        {
            pointerMaterial.color = new Color(0, 1, 0.9f, 0); // Если поинты кончились
        }
    }


    void Update()
    {
        if (magicBallsMinDistance)
        {
            // Перемещаем стрелку на новую позицию игрока (поднимаем над играком на длину её радиуса)
            transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y + (PlayerController.size / 2), PlayerTransform.position.z);

            // Находим позиции игрока и ближайшего поинта с обнулёными координатами по Y (чтобы стрелка не двигалась вверх - вниз)
            Vector3 zeroXBallPosition = new Vector3(magicBallsMinDistance.transform.position.x, magicBallsMinDistance.transform.position.y, magicBallsMinDistance.transform.position.z);
            Vector3 zeroXMyPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            // Поворачиваем стрелку в сторону ближайшего поинта
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(zeroXBallPosition - zeroXMyPosition), 0.05f);
        }
    }


    public void CrazePoints(int value)
    {
        PointCount += value;
        CollectingPoint.Play();
        PointCountText.text = PointCount.ToString();
    }
}
