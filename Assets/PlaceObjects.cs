using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
	public GameObject SpherePrefab;
	public GameObject CubePrefab;
	public GameObject CylinderPrefab;
	public GameObject ConePrefab;
	Color colorRed = Color.red;
	Color colorBlue = Color.blue;
	Color colorGreen = Color.green;
	Color colorYellow = Color.yellow;
	Color colorPurple = new Color(1, 0, 1, 1);
	GameObject[,] searchObjects = new GameObject[2, 2];
	// GameObject[,] searchObjects = new GameObject[5, 10];
	GameObject camObject;
	Transform camTransform;
	Vector3 camPosition;
	float angleDiff; // オブジェクト間の角度差（y軸周り．横方向）
	float angle; // オブジェクトの配置角度
	float rangeAngle = 30f; // オブジェクト配置範囲（角度）
	float radius = 1.0f; // カメラと探索オブジェクトの距離（半径）
	float distanceY; // オブジェクト間の距離（y軸方向．縦方向）
	float positionY; // オブジェクトの配置座標（y）
	float rangeY = 0.4f; // オブジェクトの配置範囲（y座標）
	void Start()
	{
		camObject = GameObject.Find("Camera");
		camTransform = camObject.GetComponent<Transform>();

		angleDiff = rangeAngle / (float)(searchObjects.GetLength(1) - 1);
		distanceY = rangeY / (float)(searchObjects.GetLength(0) - 1);

		// プレハブを格納
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				if(i == 0)
				{
					if(j % 2 == 0) searchObjects[i, j] = SpherePrefab;
					else searchObjects[i, j] = CubePrefab;
				}
				else if(i == 1)
				{
					if(j % 2 == 0) searchObjects[i, j] = SpherePrefab;
					else searchObjects[i, j] = CubePrefab;
				}
				else if(i == 2)
				{
					if(j % 2 == 0) searchObjects[i, j] = SpherePrefab;
					else searchObjects[i, j] = CubePrefab;
				}
				else if(i == 3)
				{
					if(j % 2 == 0) searchObjects[i, j] = SpherePrefab;
					else searchObjects[i, j] = CubePrefab;
				}
				else
				{
					if(j % 2 == 0) searchObjects[i, j] = SpherePrefab;
					else searchObjects[i, j] = CubePrefab;
				}
			}
		}

		// 一度プレハブを生成（GetComponentで色変更するため）
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				searchObjects[i, j] = Instantiate(searchObjects[i, j], new Vector3(0, 0, 0), Quaternion.identity, this.transform);
				Debug.Log("Instantiate");
			}
		}

		// プレハブの色変更
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				if(i == 0) searchObjects[i, j].GetComponent<Renderer>().material.color = colorRed;
				else if(i == 1) searchObjects[i, j].GetComponent<Renderer>().material.color = colorBlue;
				else if(i == 2) searchObjects[i, j].GetComponent<Renderer>().material.color = colorGreen;
				else if(i == 3) searchObjects[i, j].GetComponent<Renderer>().material.color = colorYellow;
				else searchObjects[i, j].GetComponent<Renderer>().material.color = colorPurple;
			}
		}

		// to do 配列の要素をシャッフル

		camPosition = camTransform.position; // cameraの座標
		this.transform.position = camPosition;
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				angle = (angleDiff * j) * Mathf.Deg2Rad;
				positionY = distanceY * i - rangeY/2;
				searchObjects[i, j].transform.position = new Vector3 (radius * Mathf.Cos(angle), positionY, radius * Mathf.Sin(angle));
			}
		}
	}
	void Update()
	{
		if (Input.GetKey (KeyCode.R)) ResetPlace();
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
		Debug.Log("Reset");
	}
}
