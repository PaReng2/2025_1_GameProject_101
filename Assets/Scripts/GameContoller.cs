using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContoller : MonoBehaviour
{
    public float GameTimer = 3.0f;
    public GameObject MonsterGo;

    // Update is called once per frame
    void Update()
    {
        GameTimer -= Time.deltaTime;

        if (GameTimer <= 0)
        {
            GameTimer = 3.0f;                           //GameTimer�� ��ġ�� 0���Ϸ� ���������� �ð��� 3���� ����

            GameObject Temp = Instantiate(MonsterGo);                               //���� ����, ����
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f); //��ġ ���� ����
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;                                                 //ray ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //ȭ�鿡�� ���̸� ���� ����

            if (Physics.Raycast(ray, out hit))                               //hit�� ���� ����
            {
                if (hit.collider != null)                                    //hit�� ������ �������
                {

                    //Debug.Log(hit.collider.name);                     //���
                    hit.collider.gameObject.GetComponent<Monster>().CharacterHit(50);
                }
            }
        }
    }
}
