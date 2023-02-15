using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceChar : MonoBehaviour
{
    [SerializeField] GameObject AredPrefab;
    [SerializeField] GameObject AbluePrefab;
    [SerializeField] GameObject BredPrefab;
    [SerializeField] GameObject BbluePrefab;
    // Color colorRed = Color.red;
    // Color colorBlue = Color.blue;
    GameObject[,] objects = new GameObject[5, 9]; // 
    [SerializeField] Transform camTransform;
    Vector3 camPosition;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0 ; i < objects.GetLength(0) ; i++)
        {
            for(int j = 0 ; j < objects.GetLength(1) ; j++)
            {
                if(i%2 == 0)
                {
                    if(j%2 == 0)
                    {
                        objects[i, j] = AredPrefab;
                    }
                    else
                    {
                        objects[i, j] = AbluePrefab;
                    }
                }
                else
                {
                    if(j%2 == 0)
                    {
                        objects[i, j] = BredPrefab;
                    }
                    else
                    {
                        objects[i, j] = BbluePrefab;
                    }
                }
            }
        }

        // camPosition = camTransform.position; // cameraの初期座標
        for(int i = 0 ; i < objects.GetLength(0) ; i++)
        {
            for(int j = 0 ; j < objects.GetLength(1) ; j++)
            {
                Instantiate(objects[i, j], new Vector3(j * 0.1f, i * 0.1f, 1.0f), Quaternion.identity, this.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.R)) 
        {
            ResetPlace();
        }
        
    }

    void ResetPlace(){
        camPosition = camTransform.position; // cameraの初期座標
        for(int i = 0 ; i < objects.GetLength(0) ; i++)
        {
            for(int j = 0 ; j < objects.GetLength(1) ; j++)
            {
                this.transform.position = camPosition;
            }
        }
    }
}
