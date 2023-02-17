using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPixelColor : MonoBehaviour
{
	Outline outline; // 輪郭
	Color pixelColor; // 射影の中心のピクセル色
	int pixelX; // 射影の中心のピクセル座標（x）
	int pixelY; // 射影の中心のピクセル座標（y）
	Transform myTransform; // このオブジェクトのTransform
	Vector3 myPos; // このオブジェクトの座標
	GameObject camViewObject; // CamViewのGameObject
	SetCamView setCamViewScript; // SetCamView.cs
	public GameObject camObject;
	Ray ray; // カメラからオブジェクトへのレイ
	RaycastHit hit; // レイとcamViewのヒット
	Vector2 pixelUV; // hitの座標
	int mask = 1 << 7; // 
	void Start()
	{
		outline = gameObject.AddComponent<Outline>();
		outline.OutlineMode = Outline.Mode.OutlineAll;
		outline.OutlineWidth = 20f; // 輪郭の幅

		camViewObject = GameObject.Find("CamView");
		setCamViewScript = camViewObject.GetComponent<SetCamView>();
	}
	void Update()
	{
		myTransform = this.transform;
		myPos = myTransform.position; // このオブジェクトの座標

		Debug.Log("myPos" + myPos);

		var direction = myPos - camObject.transform.position;
		Debug.Log("direction" + direction);

		ray = new Ray(camObject.transform.position, direction);
		Debug.Log("ray.origin" + ray.origin);
		Debug.Log("ray.direction" + ray.direction);
		if(Physics.Raycast(ray, out hit, mask)){
			pixelUV = hit.textureCoord;
			Debug.Log("pixelUVx" + pixelUV.x);
			Debug.Log("pixelUVy" + pixelUV.y);

			pixelUV.x *= setCamViewScript.camViewPxW;
			pixelUV.y *= setCamViewScript.camViewPxH;
			pixelUV.x += setCamViewScript.camViewPxW/2;
			pixelUV.y += setCamViewScript.camViewPxH/2;

			pixelColor = setCamViewScript.webcamTexture.GetPixel((int)pixelUV.x, (int)pixelUV.y); // 射影の中心のピクセル色取得
			Debug.Log("pixelUVx" + pixelUV.x);
			Debug.Log("pixelUVy" + pixelUV.y);

			outline.OutlineColor = pixelColor;
		}
		Debug.DrawRay(ray.origin, ray.direction, Color.red);
	}
	void ChangeNegaPosiColor(){ // ネガポジ反転

		Color negaposiColor = new Color(0.0f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f); // ネガポジ反転後の色

		negaposiColor.r = (1.0f - pixelColor.r);
		negaposiColor.g = (1.0f - pixelColor.g);
		negaposiColor.b = (1.0f - pixelColor.b);

		outline.OutlineColor = negaposiColor;
	}
	void ChangeComplementaryColor(){ // 補色の計算

		Color compColor = new Color(0.0f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f); // 補色

		float rgbMax = pixelColor.r; // rgbの内最大の値
		float rgbMin = pixelColor.r; // rgbの内最小の値

		rgbMax = (pixelColor.g > rgbMax) ? pixelColor.g : rgbMax; // gの方が大きい場合g
		rgbMax = (pixelColor.b > rgbMax) ? pixelColor.b : rgbMax; // bの方が大きい場合b
		rgbMin = (pixelColor.g < rgbMin) ? pixelColor.g : rgbMin; // gの方が小さい場合g
		rgbMin = (pixelColor.b < rgbMin) ? pixelColor.b : rgbMin; // bの方が小さい場合b

		float rgbMaxMin = rgbMax + rgbMin; // rgbの内最大の値と最小の値の和

		compColor.r = (rgbMaxMin - pixelColor.r);
		compColor.g = (rgbMaxMin - pixelColor.g);
		compColor.b = (rgbMaxMin - pixelColor.b);

		outline.OutlineColor = compColor;
	}
	void ChangeShade(){ // 同系統の色の薄い色/濃い色に変換

		Color shadeColor = new Color(0.0f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f); // 変換した色

		float rgbTotal = pixelColor.r + pixelColor.g + pixelColor.b;

		if(rgbTotal >= 1.5){
			shadeColor = new Color(pixelColor.r -0.5f, pixelColor.g -0.5f, pixelColor.b -0.5f);
		}
		else{
			
			shadeColor = new Color(pixelColor.r + 0.5f, pixelColor.g + 0.5f, pixelColor.b +0.5f);
		}
		outline.OutlineColor = shadeColor;
	}
}
