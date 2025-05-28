using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBallController : MonoBehaviour
{
    [Header("기본 설정")]
    public float power = 10f; //타격 힘
    public Sprite arrowSprite; //화살표

    private Rigidbody rb;
    private GameObject arrow; //화살표 오브젝트
    private bool isDragging = false; //드래그 중인지 확인하는 변수
    private Vector3 startPos;        //드래그 시작 위치

    
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
        rb = GetComponent<Rigidbody>(); //리지드바디 가져오기
        if (rb == null)     //물리 컴포넌트가 없을 경우
        {
            rb = gameObject.AddComponent<Rigidbody>(); // 추가해준다
        }
    }

    public bool IsMoving()
    {
        return rb.velocity.magnitude > 0.2f;
    }

    void HandleInput()
    {
        if (!SimploeTurnManager.canPlay) return;        //턴 매니저가 허용하지 않으면 바로 리턴값을 설정하여 움직이지 못하게 설정
        if (SimploeTurnManager.anyBallMoveing) return;

        if (IsMoving()) return;

        if (Input.GetMouseButtonDown(0))    //마우스 클릭 시작
        {
            StartDrag();
        }

        if (Input.GetMouseButtonUp(0) && isDragging == true)    //드래그중에 클릭을 뗐을 때
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mouseDelta = Input.mousePosition - startPos;    //
        float force = mouseDelta.magnitude * 0.01f * power;

        if (force < 5) force = 5;

        Vector3 direction = new Vector3(-mouseDelta.x, 0, - mouseDelta.y).normalized;        //방향 계산

        rb.AddForce(direction * force, ForceMode.Impulse);      //공에 힘 적용

        SimploeTurnManager.OnBallHit();     //턴 매니저에게 공을 쳤다고 알려줌

        isDragging = false;
        Destroy(arrow);
        arrow = null;

        Debug.Log("발사! 힘: " +  force);
    }

    void CreateArrow()
    {
        if (arrow != null)
        {
            Destroy(arrow);
        }

        arrow = new GameObject("Arrow");
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();       //스프라이트 렌더러를 가져온다.

        sr.sprite = arrowSprite;
        sr.color = Color.green;
        sr.sortingOrder = 10;

        arrow.transform.position = transform.position + Vector3.up;     //화살표 위치를 잡아준다.
        arrow.transform.localScale = Vector3.one;
    }

    void UpdateArrow()
    {
        if (!isDragging || arrow == null) return;

        Vector3 mouseDelta = Input.mousePosition - startPos;        //마우스 이동 거리 계산
        float distance = mouseDelta.magnitude;

        float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2.0f);     //화살표 크기를 힘에 따라 변경
        arrow.transform.localScale = Vector3.one * size;

        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();       //화살표 색상 초록 --> 빨강
        float colorRatio = Mathf.Clamp01(distance * 0.005f);
        sr.color = Color.Lerp(Color.green, Color.red, colorRatio);      //이동 거리가 길어질수록 초록에서 빨강으로 변함

        if (distance > 10f)
        {
            Vector3 direction = new Vector3(-mouseDelta.x, 0, -mouseDelta.y);

            //2D 평명 (탑뷰) 에서 direction 벡터가 가리키는 방햐을 각도로 변환
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;    //방향 각도를 변환 시켜주는 공식
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0);          //화살표 방향을 설정 한다.
        }
    }

    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //화면에서 ray를 쏴서
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))      //히트 된 것이 있을 경우
        {
            if (hit.collider.gameObject == gameObject)      //해당 오브젝트가 자신일 경우
            {
                isDragging = true;
                startPos = Input.mousePosition;
                CreateArrow();
                Debug.Log("드래그 시작");
            }
        }
    }
}
