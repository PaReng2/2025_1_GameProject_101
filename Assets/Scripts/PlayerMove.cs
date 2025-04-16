using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header ("기본 이동 설정")]
    public float moveSpeed = 5f;
    public int jumpForce = 5;
    public float turnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float fallMultiplier = 2.5f;          //하강 중력 배율
    public float lowJumpMultiplier = 2.0f;      //짧은 점프 배율

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public Rigidbody rb;

    public bool isGrounded;
    
    public int coinCount = 0;
    public int totalCoins = 7;
    // Start is called before the first frame update
    void Start()
    {
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        gliderTimeLeft = gliderMaxTime;
        
        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGroundedState();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0 , moveVertical); // 이동 방향 감지

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);          //이동 방향을 바라보도록 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            realGrouned = false;
            coyoteTimeCounter = 0; //코요테 타임 리셋
        }


        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if (!isGliding)
            {
                EnableGlider();
            }
        }
        //글라이더 사용 시간 감소
        gliderTimeLeft -= Time.deltaTime;

        if (gliderTimeLeft <= 0) //시간 다되면 글라이더 없어짐
        {
            DisableGilder();//글라이더 비활성화 함수
        }
        else if (isGliding)
        {
            DisableGilder();
        }

        if (isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }



    void EnableGlider()
    {
        isGliding = true;
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
    }

    void DisableGilder()
    {
        isGliding = false;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        rb.velocity = new Vector3(rb.velocity.x, 0 , rb.velocity.z);
    }

    void ApplyGliderMovement(float horizontal, float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horizontal * gliderMoveSpeed, -gliderFallSpeed, vertical * gliderMoveSpeed);

        rb.velocity = gliderVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인 수집 : {coinCount}/ {totalCoins}");
        }

        if (other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            transform.position = new Vector3(9.34f, 1, -3.678f);
            Debug.Log("Clear!");
        }
    }

    void UpdateGroundedState()
    {
        if (realGrouned)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                isGrounded = false;     //만약 코요테 타임 내에 있을 경우 지면으로 감지
            }
        }
    }

}
