using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisMover : MonoBehaviour
{
    public float speed = 5.0f;
    public float timer = 5.0f;
   
    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,0,speed *  Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);        //시간 내에 먹지 못하면 없어짐
        }
    }
}
