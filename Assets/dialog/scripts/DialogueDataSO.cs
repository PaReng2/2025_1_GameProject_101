using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewDialogue", menuName ="Dialogue?dialodueData" )]
public class DialogueDataSO : ScriptableObject
{
    [Header("ĳ���� ����")]
    public string characterName = "ĳ����";        //ĳ���� �̸�
    public Sprite characterImage;                   //ĳ���� �� �̹���

    [Header("��ȭ ����")]
    [TextArea(3, 10)]                       //�ν����Ϳ��� ���� �� �Է� �����ϰ� ����
    public List<string> dialogueLines = new List<string>();     //��ȭ �����(������� ���)
}
