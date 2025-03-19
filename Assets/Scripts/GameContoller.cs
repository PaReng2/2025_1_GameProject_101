using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContoller : MonoBehaviour
{
    public float GameTimer = 3.0f;
    public GameObject MonsterGo;

    // Update is called once per frame
    void Update()
    {
        GameTimer -= Time.deltaTime;

        if (GameTimer <= 0)
        {
            GameTimer = 3.0f;                           //GameTimer의 수치가 0이하로 내려갔을때 시간을 3으로 리셋

            GameObject Temp = Instantiate(MonsterGo);                               //옵젝 복사, 생성
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f); //위치 생성 랜덤
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;                                                 //ray 선언
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //화면에서 레이를 쏴서 검출

            if (Physics.Raycast(ray, out hit))                               //hit된 옵젝 검출
            {
                if (hit.collider != null)                                    //hit된 옵젝이 있을경우
                {

                    //Debug.Log(hit.collider.name);                     //출력
                    hit.collider.gameObject.GetComponent<Monster>().CharacterHit(50);
                }
            }
        }
    }
}
