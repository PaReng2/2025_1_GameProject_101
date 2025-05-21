using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitType;  //���� Ÿ��(0:���, 1:��纣��...)
    public bool hasMerged = false; //������ ���������� Ȯ��

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMerged)  //�̹� ������ ������ ����
            return;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>(); //�ٸ� ���ϰ� �浹�ߴ��� Ȯ��

        if (otherFruit != null && !otherFruit.hasMerged && otherFruit.fruitType == fruitType) //�浹�� ���� �����̰� Ÿ���� ���ٸ� (�������� �ʾ��� ���)
        {
            hasMerged = true;
            otherFruit.hasMerged = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f; //�� ������ �߰� ��ġ ���

            //���� �Ŵ������� Merge ������ ���� ȣ�� (������)
            FruitGame gameManager = FindAnyObjectByType<FruitGame>();
            if (gameManager != null)
            {
                gameManager.MergeFruits(fruitType, mergePosition);      //�Լ��� �����ϰ� ȣ�� �Ѵ�.
            }

            //�������� �� ���ϵ� ����
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }

}
