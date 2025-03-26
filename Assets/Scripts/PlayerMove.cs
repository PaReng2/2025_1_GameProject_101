using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;

    public bool isGrounded;
    public int jumpForce = 5;

    public int coinCount = 0;
    public int totalCoins = 7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
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

}
