using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimploeTurnManager : MonoBehaviour
{
    public static bool canPlay = true;              //���� ĥ �� �ִ���
    public static bool anyBallMoveing = false;      //� ���̶� �����̰� �ִ���

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls();

        if (!anyBallMoveing && !canPlay) //��� ���� ���߸� �ٽ� ĥ �� ����
        {
            canPlay = true;
            Debug.Log("�� ����! �ٽ� ĥ �� �ֽ��ϴ�.");
        }
    }

    void CheckAllBalls()    //���� ������� Ȯ��
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();    //���� �ִ� SimpleBallcontroller�� ��� �ϴ� ��� ������Ʈ�� �迭�� �ִ´�.
        anyBallMoveing = false;     //�ʱ�ȭ

        foreach (SimpleBallController ball in allBalls)     //�迭 ��ü Ŭ������ ��ȯ �ϸ鼭
        {
            if (ball.IsMoving())            //���� �����̰� �ִ��� Ȯ�� �ϴ� �Լ��� ȣ��
            {
                anyBallMoveing = true;      //���� �����δٰ� ���� ����
                break;                  //�������� ���� ���´�.
            }
        }
    }

    public static void OnBallHit()  //���� �÷��� ���� �� ȣ��
    {
        canPlay = false;            //�÷��� �Ұ������� �ٸ� ���鵵 �������� ����
        anyBallMoveing = true;      //���� �����̱� �����ϱ� ������ bool �� ����
        Debug.Log("�� ����! ���� ���� �� ���� ��ٸ�����");
    }
}
