using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI요소 - inspector 에서 연결")]
    public GameObject DialoguePanel;                //대화창 전체 패널
    public Image characterImage;                    //캐릭터 이미지
    public TextMeshProUGUI characternameText;       //캐릭터 이름
    public TextMeshProUGUI dialogueText;            //대화 내용 표시 텍스트
    public Button nextButton;                       //다음 버튼

    [Header("기본 설정")]
    public Sprite defaultCharacterImage;            //캐릭터 기본 이미지

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;               //글자 하나당 출력 속도
    public bool skipTypingOnClick = true;           //클릭 시 타이핑 즉시 완료 여부

    private DialogueDataSO currentDialogue;         //현재 진행중인 대화 데이터
    private int currentLineIndex = 0;               //현재 몇 번째 대화 중인지(0 부터 시작)
    private bool isDialogueActive = false;          //대화 진행중인지 확인하는 플래그
    private bool isTyping = false;                  //타이핑 효과 코루틴 참조 (중지용)
    private Coroutine typingCoroutine;              //타이핑 효과 코루틴 참조 (중지용)

    void Start()
    {
        DialoguePanel.SetActive(false);             //시작 시 대화창 숨기기
        nextButton.onClick.AddListener(HandleNextInput);        //"다음" 버튼에 새로운 입력 처리 연결
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }

    IEnumerator TypeText(string textToType)             //타이핑 할 전체 텍스트
    {
        isTyping = true;                                //타이핑 시작
        dialogueText.text = "";                         //텍스트 초기화

        for (int i = 0; i < textToType.Length; i++)     //텍스트를 한 글자씩 추가
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);       //대기 시간 설정
        }

        isTyping = false;                               //타이핑 완료
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)                    //타이핑 효과를 즉시 종료하는 함수
        {
            StopCoroutine(typingCoroutine);             //코루틴 중지
        }
        isTyping=false;                                 //타이핑 상태 해제

        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }
    }

    void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {   
            if (typingCoroutine != null)                //이전 타이핑 효과가 있다면 중지
            {
                StopCoroutine(typingCoroutine);         //코루틴 중지
            }
        }
        //현재 줄의 대화 내용으로 타이핑 효과 시작
        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    void EndDialogue()                      //대화 완전히 종료
    {
        if (typingCoroutine != null)        //타이핑 효과 정리
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;           //대화창 비활성화
        isTyping = false;                   //타이핑 상태 해제
        DialoguePanel.SetActive(false);     //대화창 숨기기
        currentLineIndex = 0;               //인덱스 초기화
    }

    public void ShowNextLine()              //다음 대화 줄로 이동 시키는 함수
    {
        currentLineIndex++;                 //다음 줄로 인덱스 증가

        if (currentLineIndex >= currentDialogue.dialogueLines.Count)            //마지막 대화였는지 확인
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();              //대화가 남아있으면 다음 줄 표시
        }
    }

    public void HandleNextInput()           //스페이스바나 버튼 클릭 시 호출되는 입력 처리 함수
    {
        if (isTyping && skipTypingOnClick)
        {
            CompleteTyping();               //타이핑 중이면 즉시 완료
        }
        else if (!isTyping)
        {
            ShowNextLine();                 //타이핑 완료 상태면 다음 줄 표시
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

    public void StartDialogue(DialogueDataSO dialogue)                      //새로운 대화를 시작하는 함수
    {
        if(dialogue == null || dialogue.dialogueLines.Count == 0) return;   //대화 데이터가 없거나 대화 내용이 비어있으면 실행 하지 않음

        currentDialogue = dialogue;                //현재 대화 데이터 설정
        currentLineIndex = 0;                      //첫 번째 대화 부터 시작
        isDialogueActive= true;                     //대화 활성화 플래그 On

        //UI 업데이트
        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if (characterImage != null)
        {
            if (dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;        //대화 데이터의 이미지 사용
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;          //기본 이미지 사용
            }
        }

        ShowCurrentLine();                      //첫 번째 대화 내용 표시
    }
}
