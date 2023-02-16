using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceCharTest : MonoBehaviour
{
	[SerializeField] TextMeshPro AColor1Prefab;
	[SerializeField] TextMeshPro AColor2Prefab;
	[SerializeField] TextMeshPro BColor1Prefab;
	[SerializeField] TextMeshPro BColor2Prefab;
	TextMeshPro[,] SearchObjects = new TextMeshPro[5, 9];
	GameObject camObject;
	Transform camTransform;
	Vector3 camPosition;
	void Start()
	{
		camObject = GameObject.Find("Camera");
		camTransform = camObject.GetComponent<Transform>();

		// プレハブの色変更
		AColor1Prefab.color = Color.red;
		AColor2Prefab.color = Color.blue;
		BColor1Prefab.color = Color.red;
		BColor2Prefab.color = Color.blue;

		// 配列にプレハブを格納
		for(int i = 0 ; i < SearchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < SearchObjects.GetLength(1) ; j++)
			{
				if(i%2 == 0)
				{
					if(j%2 == 0)
					{
						SearchObjects[i, j] = AColor1Prefab;
					}
					else
					{
						SearchObjects[i, j] = AColor2Prefab;
					}
				}
				else
				{
					if(j%2 == 0)
					{
						SearchObjects[i, j] = BColor1Prefab;
					}
					else
					{
						SearchObjects[i, j] = BColor2Prefab;
					}
				}
			}
		}

		// camPosition = camTransform.position; // cameraの初期座標
		for(int i = 0 ; i < SearchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < SearchObjects.GetLength(1) ; j++)
			{
				Instantiate(SearchObjects[i, j], new Vector3(j * 0.1f, i * 0.1f, 1.0f), Quaternion.identity, this.transform);
			}
		}
	}

	void Update()
	{
		if (Input.GetKey (KeyCode.R)) 
			{
					ResetPlace();
			}
	}
	void ResetPlace(){
		camPosition = camTransform.position; // cameraの初期座標
		for(int i = 0 ; i < SearchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < SearchObjects.GetLength(1) ; j++)
			{
				this.transform.position = camPosition;
			}
		}
	}
}
