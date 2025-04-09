using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int totalCubes = 10;
    public float cubeSpacing = 1.0f;        //����
    // Start is called before the first frame update
    void Start()
    {
        GenCube();
    }

    // Update is called once per frame
    public void GenCube()
    {
        Vector3 myPosition = transform.position;
        GameObject firestCube = Instantiate(cubePrefab, myPosition, Quaternion.identity); //ù��° ť�� ���� (�� ��ġ��)

        for (int i = 0; i < totalCubes; i++)
        {
            Vector3 position = new Vector3(myPosition.x , myPosition.y , myPosition.z + (i * cubeSpacing));
            Instantiate(cubePrefab, position, Quaternion.identity);
        }
    }
}
