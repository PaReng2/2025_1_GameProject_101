using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGame : MonoBehaviour
{

    public GameObject[] fruitPrefabs;   //���� ������ �迭 ����

    public float[] fruitSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f }; //���� ũ��

    public GameObject currentFruit;     //���� ����ִ� ����
    public int currentFruitType;        //����ִ� ���� Ÿ��(��ȣ)

    public float fruitStartHeight = 6.0f;       //���� ���� �� ���� ����(�ν����Ϳ��� ���� ����)
    public float gameWidth = 5.0f;              //������ �ʺ�
    public bool isGameOver = false;             //���� ����
    public Camera mainCamera;                   //ī�޶� ����(���콺 ��ġ�� ���� ��ȭ)

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
        if (isGameOver) return; //���� ������ ����

        if (fruitTimer >= 0)
        {
            fruitTimer -= Time.deltaTime;   //Ÿ�̸��� �ð��� 0���� Ŭ ��� �۵�
        }

        if (fruitTimer < 0 && fruitTimer > -2)  //Ÿ�̸� �ð��� 0�� -2 ���̿� ���� �� �� �Լ��� ȣ���ϰ� �ٸ� �ð���� ������.
        {
            CheckGameOver();
            SpawnNewFriot();        
            fruitTimer = -3.0f;     //Ÿ�̸� �ð��� -3���� ������.
        }


        if (isGameOver) return; //���� ������ ����

        if (currentFruit != null) //���� ������ ������ ���� ���� ó��
        {
            Vector3 mousePosition = Input.mousePosition;        //���콺 ��ġ �޾ƿ���
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);       //���콺 ��ġ�� ���� ��ǥ�� ��ȯ

            Vector3 newPosition = currentFruit.transform.position; //���� ��ġ ������Ʈ
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSizes[currentFruitType] / 2f;        //�� ��Ż ����
            if (newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = gameWidth / 2 + halfFruitSize;
            }

            currentFruit.transform.position = newPosition; //���� ��ǥ ����
        }

        if (Input.GetMouseButtonDown(0) && fruitTimer == -3.0f) //��Ŭ�� �ϸ� ���� ��� && Ÿ�̸� �ð��� -3
        {
            DropFruit();
        }
    }

    void SpawnNewFriot()    //���� ���� �Լ�
    {
        if (!isGameOver)        //���� ������ �ƴҶ�
        {
            currentFruitType = Random.Range(0, 3); //0 ~ 2������ ������ ���� Ÿ��

            Vector3 mousePosition = Input.mousePosition;        //���콺 ��ġ �޾ƿ���
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);       //���콺 ��ġ�� ���� ��ǥ�� ��ȯ

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeight, 0);      // x��ǥ�� ����ϰ� �������� ������ ���

            float halfFruitSize = fruitSizes[currentFruitType] / 2;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPrefabs[currentFruitType], spawnPosition, Quaternion.identity); //���� ����
            currentFruit.transform.localScale = new Vector3(fruitSizes[currentFruitType], fruitSizes[currentFruitType], 1);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;       //�߷��� 0����
            }
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;  //�߷°� �߰�

            currentFruit = null;    //

            fruitTimer = 1.0f;  //Ÿ�̸�
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
        Fruit[] allFruits = FindObjectsOfType<Fruit>();   //���� �ִ� ��� ���� ������Ʈ�� �پ��ִ� ������Ʈ�� �����´�. ���� ���ӿ����� ��� ����� ��

        float gameOverHeight = gameHeight;

        for (int i = 0; allFruits.Length > i; i++)      //��� ������ �˻��Ѵ�.
        {
            Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();

            //������ ���� �����̰� ���� ��ġ�� �ִٸ�
            if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
            {
                isGameOver = true;
                Debug.Log("���� ����");

                break;
            }
        }
    }
}
