using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI��� - inspector ���� ����")]
    public GameObject DialoguePanel;                //��ȭâ ��ü �г�
    public Image characterImage;                    //ĳ���� �̹���
    public TextMeshProUGUI characternameText;       //ĳ���� �̸�
    public TextMeshProUGUI dialogueText;            //��ȭ ���� ǥ�� �ؽ�Ʈ
    public Button nextButton;                       //���� ��ư

    [Header("�⺻ ����")]
    public Sprite defaultCharacterImage;            //ĳ���� �⺻ �̹���

    [Header("Ÿ���� ȿ�� ����")]
    public float typingSpeed = 0.05f;               //���� �ϳ��� ��� �ӵ�
    public bool skipTypingOnClick = true;           //Ŭ�� �� Ÿ���� ��� �Ϸ� ����

    private DialogueDataSO currentDialogue;         //���� �������� ��ȭ ������
    private int currentLineIndex = 0;               //���� �� ��° ��ȭ ������(0 ���� ����)
    private bool isDialogueActive = false;          //��ȭ ���������� Ȯ���ϴ� �÷���
    private bool isTyping = false;                  //Ÿ���� ȿ�� �ڷ�ƾ ���� (������)
    private Coroutine typingCoroutine;              //Ÿ���� ȿ�� �ڷ�ƾ ���� (������)

    void Start()
    {
        DialoguePanel.SetActive(false);             //���� �� ��ȭâ �����
        nextButton.onClick.AddListener(HandleNextInput);        //"����" ��ư�� ���ο� �Է� ó�� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }

    IEnumerator TypeText(string textToType)             //Ÿ���� �� ��ü �ؽ�Ʈ
    {
        isTyping = true;                                //Ÿ���� ����
        dialogueText.text = "";                         //�ؽ�Ʈ �ʱ�ȭ

        for (int i = 0; i < textToType.Length; i++)     //�ؽ�Ʈ�� �� ���ھ� �߰�
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);       //��� �ð� ����
        }

        isTyping = false;                               //Ÿ���� �Ϸ�
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)                    //Ÿ���� ȿ���� ��� �����ϴ� �Լ�
        {
            StopCoroutine(typingCoroutine);             //�ڷ�ƾ ����
        }
        isTyping=false;                                 //Ÿ���� ���� ����

        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }
    }

    void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {   
            if (typingCoroutine != null)                //���� Ÿ���� ȿ���� �ִٸ� ����
            {
                StopCoroutine(typingCoroutine);         //�ڷ�ƾ ����
            }
        }
        //���� ���� ��ȭ �������� Ÿ���� ȿ�� ����
        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    void EndDialogue()                      //��ȭ ������ ����
    {
        if (typingCoroutine != null)        //Ÿ���� ȿ�� ����
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;           //��ȭâ ��Ȱ��ȭ
        isTyping = false;                   //Ÿ���� ���� ����
        DialoguePanel.SetActive(false);     //��ȭâ �����
        currentLineIndex = 0;               //�ε��� �ʱ�ȭ
    }

    public void ShowNextLine()              //���� ��ȭ �ٷ� �̵� ��Ű�� �Լ�
    {
        currentLineIndex++;                 //���� �ٷ� �ε��� ����

        if (currentLineIndex >= currentDialogue.dialogueLines.Count)            //������ ��ȭ������ Ȯ��
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();              //��ȭ�� ���������� ���� �� ǥ��
        }
    }

    public void HandleNextInput()           //�����̽��ٳ� ��ư Ŭ�� �� ȣ��Ǵ� �Է� ó�� �Լ�
    {
        if (isTyping && skipTypingOnClick)
        {
            CompleteTyping();               //Ÿ���� ���̸� ��� �Ϸ�
        }
        else if (!isTyping)
        {
            ShowNextLine();                 //Ÿ���� �Ϸ� ���¸� ���� �� ǥ��
        }
    }

    public void SkipDialogue()
    {
        EndDialogue();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StartDialogue(DialogueDataSO dialogue)                      //���ο� ��ȭ�� �����ϴ� �Լ�
    {
        if(dialogue == null || dialogue.dialogueLines.Count == 0) return;   //��ȭ �����Ͱ� ���ų� ��ȭ ������ ��������� ���� ���� ����

        currentDialogue = dialogue;                //���� ��ȭ ������ ����
        currentLineIndex = 0;                      //ù ��° ��ȭ ���� ����
        isDialogueActive= true;                     //��ȭ Ȱ��ȭ �÷��� On

        //UI ������Ʈ
        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if (characterImage != null)
        {
            if (dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;        //��ȭ �������� �̹��� ���
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;          //�⺻ �̹��� ���
            }
        }

        ShowCurrentLine();                      //ù ��° ��ȭ ���� ǥ��
    }
}
