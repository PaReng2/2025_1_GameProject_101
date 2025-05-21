using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGame : MonoBehaviour
{

    public GameObject[] fruitPrefabs;   //과일 프리팹 배열 선언

    public float[] fruitSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f }; //과일 크기

    public GameObject currentFruit;     //현재 들고있는 과일
    public int currentFruitType;        //들고있는 과일 타입(번호)

    public float fruitStartHeight = 6.0f;       //과일 시작 시 높이 설정(인스펙터에서 조절 가능)
    public float gameWidth = 5.0f;              //게임판 너비
    public bool isGameOver = false;             //게임 상태
    public Camera mainCamera;                   //카메라 참조(마우스 위치에 따른 변화)

    public float fruitTimer;


    public float gameHeight;
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewFriot();
        fruitTimer = -3.0f;
        gameHeight = fruitStartHeight + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return; //게임 오버면 리턴

        if (fruitTimer >= 0)
        {
            fruitTimer -= Time.deltaTime;   //타이머의 시간이 0보다 클 경우 작동
        }

        if (fruitTimer < 0 && fruitTimer > -2)  //타이머 시간이 0과 -2 사이에 있을 때 잰 함수를 호출하고 다른 시간대로 보낸다.
        {
            CheckGameOver();
            SpawnNewFriot();        
            fruitTimer = -3.0f;     //타이머 시간을 -3으로 보낸다.
        }


        if (isGameOver) return; //게임 오버면 리턴

        if (currentFruit != null) //현재 생성된 과일이 있을 때만 처리
        {
            Vector3 mousePosition = Input.mousePosition;        //마우스 위치 받아오기
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);       //마우스 위치를 월드 좌표로 변환

            Vector3 newPosition = currentFruit.transform.position; //과일 위치 업데이트
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSizes[currentFruitType] / 2f;        //맵 이탈 방지
            if (newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = gameWidth / 2 + halfFruitSize;
            }

            currentFruit.transform.position = newPosition; //과일 좌표 갱신
        }

        if (Input.GetMouseButtonDown(0) && fruitTimer == -3.0f) //좌클릭 하면 과일 드랍 && 타이머 시간은 -3
        {
            DropFruit();
        }
    }

    void SpawnNewFriot()    //과일 생성 함수
    {
        if (!isGameOver)        //게임 오버가 아닐때
        {
            currentFruitType = Random.Range(0, 3); //0 ~ 2사이의 랜덤한 과일 타입

            Vector3 mousePosition = Input.mousePosition;        //마우스 위치 받아오기
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);       //마우스 위치를 월드 좌표로 변환

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeight, 0);      // x좌표만 사용하고 나머지는 설정한 대로

            float halfFruitSize = fruitSizes[currentFruitType] / 2;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPrefabs[currentFruitType], spawnPosition, Quaternion.identity); //과일 생성
            currentFruit.transform.localScale = new Vector3(fruitSizes[currentFruitType], fruitSizes[currentFruitType], 1);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;       //중력을 0으로
            }
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;  //중력값 추가

            currentFruit = null;    //

            fruitTimer = 1.0f;  //타이머
        }
 
    }

    public void MergeFruits(int fruitType, Vector3 position)
    {
        if (fruitType < fruitPrefabs.Length - 1)
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1], position, Quaternion.identity);
            newFruit.transform.localScale = new Vector3(fruitSizes[fruitType + 1], fruitSizes[fruitType], 1.0f);
        }
    }

    public void CheckGameOver()
    {
        Fruit[] allFruits = FindObjectsOfType<Fruit>();   //씬에 있는 모든 과일 컴포넌트가 붙어있는 오브젝트를 가져온다. 작은 게임에서만 사용 비용이 쌤

        float gameOverHeight = gameHeight;

        for (int i = 0; allFruits.Length > i; i++)      //모든 과일을 검사한다.
        {
            Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();

            //과일이 정지 상태이고 높은 위치에 있다면
            if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
            {
                isGameOver = true;
                Debug.Log("게임 오버");

                break;
            }
        }
    }
}
