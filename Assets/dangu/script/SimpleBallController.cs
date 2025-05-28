using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBallController : MonoBehaviour
{
    [Header("�⺻ ����")]
    public float power = 10f; //Ÿ�� ��
    public Sprite arrowSprite; //ȭ��ǥ

    private Rigidbody rb;
    private GameObject arrow; //ȭ��ǥ ������Ʈ
    private bool isDragging = false; //�巡�� ������ Ȯ���ϴ� ����
    private Vector3 startPos;        //�巡�� ���� ��ġ

    
    void Start()
    {
        SetupBall();
    }

    
    void Update()
    {
        HandleInput();
        UpdateArrow();
    }

    void SetupBall()
    {
        rb = GetComponent<Rigidbody>(); //������ٵ� ��������
        if (rb == null)     //���� ������Ʈ�� ���� ���
        {
            rb = gameObject.AddComponent<Rigidbody>(); // �߰����ش�
        }
    }

    public bool IsMoving()
    {
        return rb.velocity.magnitude > 0.2f;
    }

    void HandleInput()
    {
        if (!SimploeTurnManager.canPlay) return;        //�� �Ŵ����� ������� ������ �ٷ� ���ϰ��� �����Ͽ� �������� ���ϰ� ����
        if (SimploeTurnManager.anyBallMoveing) return;

        if (IsMoving()) return;

        if (Input.GetMouseButtonDown(0))    //���콺 Ŭ�� ����
        {
            StartDrag();
        }

        if (Input.GetMouseButtonUp(0) && isDragging == true)    //�巡���߿� Ŭ���� ���� ��
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mouseDelta = Input.mousePosition - startPos;    //
        float force = mouseDelta.magnitude * 0.01f * power;

        if (force < 5) force = 5;

        Vector3 direction = new Vector3(-mouseDelta.x, 0, - mouseDelta.y).normalized;        //���� ���

        rb.AddForce(direction * force, ForceMode.Impulse);      //���� �� ����

        SimploeTurnManager.OnBallHit();     //�� �Ŵ������� ���� �ƴٰ� �˷���

        isDragging = false;
        Destroy(arrow);
        arrow = null;

        Debug.Log("�߻�! ��: " +  force);
    }

    void CreateArrow()
    {
        if (arrow != null)
        {
            Destroy(arrow);
        }

        arrow = new GameObject("Arrow");
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();       //��������Ʈ �������� �����´�.

        sr.sprite = arrowSprite;
        sr.color = Color.green;
        sr.sortingOrder = 10;

        arrow.transform.position = transform.position + Vector3.up;     //ȭ��ǥ ��ġ�� ����ش�.
        arrow.transform.localScale = Vector3.one;
    }

    void UpdateArrow()
    {
        if (!isDragging || arrow == null) return;

        Vector3 mouseDelta = Input.mousePosition - startPos;        //���콺 �̵� �Ÿ� ���
        float distance = mouseDelta.magnitude;

        float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2.0f);     //ȭ��ǥ ũ�⸦ ���� ���� ����
        arrow.transform.localScale = Vector3.one * size;

        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();       //ȭ��ǥ ���� �ʷ� --> ����
        float colorRatio = Mathf.Clamp01(distance * 0.005f);
        sr.color = Color.Lerp(Color.green, Color.red, colorRatio);      //�̵� �Ÿ��� ��������� �ʷϿ��� �������� ����

        if (distance > 10f)
        {
            Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y);

            //2D ��� (ž��) ���� direction ���Ͱ� ����Ű�� ������ ������ ��ȯ
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;    //���� ������ ��ȯ �����ִ� ����
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0);          //ȭ��ǥ ������ ���� �Ѵ�.
        }
    }

    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //ȭ�鿡�� ray�� ����
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))      //��Ʈ �� ���� ���� ���
        {
            if (hit.collider.gameObject == gameObject)      //�ش� ������Ʈ�� �ڽ��� ���
            {
                isDragging = true;
                startPos = Input.mousePosition;
                CreateArrow();
                Debug.Log("�巡�� ����");
            }
        }
    }
}
