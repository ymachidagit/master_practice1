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
	TextMeshPro[,] searchObjects = new TextMeshPro[5, 10];
	GameObject camObject;
	Transform camTransform;
	Vector3 camPosition;
	float angleDiff; // オブジェクト間の角度差（y軸周り．横方向）
	float angle; // オブジェクトの配置角度
	float rangeAngle = 180f; // オブジェクト配置範囲（角度）
	float radius = 1.0f; // カメラと探索オブジェクトの距離（半径）
	float distanceY; // オブジェクト間の距離（y軸方向．縦方向）
	float positionY; // オブジェクトの配置座標（y）
	float rangeY = 1.6f; // オブジェクトの配置範囲（y座標）
	
	void Start()
	{
		camObject = GameObject.Find("Camera");
		camTransform = camObject.GetComponent<Transform>();

		angleDiff = rangeAngle / (float)(searchObjects.GetLength(1) - 1);
		distanceY = rangeY / (float)(searchObjects.GetLength(0) - 1);

		// プレハブの色変更
		AColor1Prefab.color = Color.red;
		AColor2Prefab.color = Color.blue;
		BColor1Prefab.color = Color.red;
		BColor2Prefab.color = Color.blue;

		// 配列にプレハブを格納
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				if(i%2 == 0)
				{
					if(j%2 == 0)
					{
						searchObjects[i, j] = AColor1Prefab;
					}
					else
					{
						searchObjects[i, j] = AColor2Prefab;
					}
				}
				else
				{
					if(j%2 == 0)
					{
						searchObjects[i, j] = BColor1Prefab;
					}
					else
					{
						searchObjects[i, j] = BColor2Prefab;
					}
				}
			}
		}

		// to do 配列の要素をシャッフル

		camPosition = camTransform.position; // cameraの初期座標
		this.transform.position = camPosition;
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				angle = (angleDiff * j) * Mathf.Deg2Rad;
				positionY = distanceY * i - rangeY/2;
				Instantiate(searchObjects[i, j], new Vector3(radius * Mathf.Cos(angle), positionY, radius * Mathf.Sin(angle)), Quaternion.identity, this.transform);
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
		camPosition = camTransform.position; // cameraの座標
		this.transform.position = camPosition;
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				angle = (angleDiff * j) * Mathf.Deg2Rad;
				positionY = distanceY * i - rangeY/2;
				searchObjects[i, j].transform.localPosition = new Vector3 (radius * Mathf.Cos(angle), positionY, radius * Mathf.Sin(angle));
			}
		}
	}
}
