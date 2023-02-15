using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uWindowCapture;

public class OutlineObject : MonoBehaviour
{
	Outline outline; // 輪郭
	Color outlineColor; // 輪郭色
	Color pixelColor; // 射影の中心のピクセル色
	int pixelX; // 射影の中心のピクセル座標（x）
	int pixelY; // 射影の中心のピクセル座標（y）
	Transform myTransform; // このオブジェクトのTransform
	Vector3 myPos; // このオブジェクトの座標
	Vector3 posInCamView; // このオブジェクトのCamView上の座標
	GameObject camViewObject; // CamViewのGameObject
	SetCamView setCamViewScript; // SetCamView.cs

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
		
		posInCamView = new Vector3 (myPos.x * setCamViewScript.camViewPos.z / myPos.z, myPos.y * setCamViewScript.camViewPos.z / myPos.z, setCamViewScript.camViewPos.z); // 射影の座標計算

		pixelX = (int)(posInCamView.x / (setCamViewScript.camViewScaleW/2) * setCamViewScript.camViewPxW/2 + setCamViewScript.camViewPxW/2); // 射影の中心のピクセル座標（x）計算
		pixelY = (int)(posInCamView.y / (setCamViewScript.camViewScaleH/2) * setCamViewScript.camViewPxH/2 + setCamViewScript.camViewPxH/2); // 射影の中心のピクセル座標（y）計算
		
		Debug.Log("posInCamView:" + posInCamView);
		Debug.Log("pixelX:" + pixelX);
		Debug.Log("pixelY:" + pixelY);

		if (!setCamViewScript.webcamTexture.isPlaying)
		{
			return;
		}
		else
		{
			pixelColor = setCamViewScript.webcamTexture.GetPixel(pixelX, pixelY); // 射影の中心のピクセル色取得

			// 輪郭色変更
			outline.OutlineColor = pixelColor;
			// ChangeNegaPosiColor();
			// ChangeComplementaryColor();
			// ChangeShade();
		}
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
