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
  Color colorRed = Color.red;
  Color colorBlue = Color.blue;
  Color colorGreen = Color.green;
  Color colorYellow = Color.yellow;
  Color colorPurple = new Color(1, 0, 1, 1);TextMeshPro[,] searchObjects = new TextMeshPro[4, 10]; // 5行10列
	GameObject camObject;
	Transform camTransform;
	float angleDiff; // オブジェクト間の角度差（y軸周り．横方向）
	float angle; // オブジェクトの配置角度（Degree）
	float angleRad; // オブジェクトの配置角度（Radian）
	float rangeAngle = 180f; // オブジェクト配置範囲（角度）
	float radius = 1.0f; // カメラと探索オブジェクトの距離（半径）
	float distanceY; // オブジェクト間の距離（y軸方向．縦方向）
	float positionY; // オブジェクトの配置座標（y）
	float rangeY = 1.6f; // オブジェクトの配置範囲（y座標）
	void Start()
	{
		camObject = GameObject.Find("Camera");
		// camObject = GameObject.Find("Main Camera");
		camTransform = camObject.GetComponent<Transform>();

		angleDiff = rangeAngle / (float)(searchObjects.GetLength(1) - 1);
		distanceY = rangeY / (float)(searchObjects.GetLength(0) - 1);

		// プレハブの色変更
		AColor1Prefab.color = colorRed;
		AColor2Prefab.color = colorBlue;
		BColor1Prefab.color = colorRed;
		BColor2Prefab.color = colorBlue;

		// プレハブを格納 to do 一つだけ正解のプレハブを入れる
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				if(i%2 == 0)
				{
					if(j%2 == 0) searchObjects[i, j] = AColor1Prefab;
					else searchObjects[i, j] = AColor2Prefab;
				}
				else
				{
					if(j%2 == 0) searchObjects[i, j] = BColor1Prefab;
					else searchObjects[i, j] = BColor2Prefab;
				}
			}
		}

		// to do 配列の要素をシャッフル

		// プレハブを生成
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
				searchObjects[i, j] = Instantiate(searchObjects[i, j], new Vector3(0, 0, 0), Quaternion.identity, this.transform);
			}
		}

		ResetPlace(); // 配置
	}
	void Update()
	{
		if (Input.GetKey (KeyCode.R)) ResetPlace(); // Rキーを押したときにプレハブを配置
	}
	void ResetPlace()
	{
		this.transform.position = camTransform.position; // SearchObjectsの座標をCameraの座標に
		this.transform.rotation = camTransform.rotation; // SearchObjectsの回転をCameraの回転に
		for(int i = 0 ; i < searchObjects.GetLength(0) ; i++)
		{
			for(int j = 0 ; j < searchObjects.GetLength(1) ; j++)
			{
        angle = (angleDiff * j); // このプレハブの回転角度（Degree）
        angleRad = angle * Mathf.Deg2Rad; // このプレハブの回転角度（Radian）
        positionY = distanceY * i - rangeY / 2; // このプレハブのy座標
        searchObjects[i, j].transform.localPosition = new Vector3(radius * Mathf.Cos(angleRad), positionY, radius * Mathf.Sin(angleRad)); // プレハブの配置
        searchObjects[i, j].transform.rotation = camTransform.rotation; // プレハブの回転をCameraに合わせる
        searchObjects[i, j].transform.Rotate(0.0f, 90 - angle, 0.0f); // プレハブがCameraに対して正面を向くように回転
			}
		}
	}
}
