using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObjectRayTest : MonoBehaviour
{
    Outline outline; // 輪郭
	Color pixelColor; // 射影の中心のピクセル色
	float pixelX; // 射影の中心のピクセル座標（x）
	float pixelY; // 射影の中心のピクセル座標（y）
	Transform myTransform; // このオブジェクトのTransform
	GameObject camViewObject; // CamViewのGameObject
	SetCamView setCamViewScript; // SetCamView.cs
	GameObject camObject; // Cameraオブジェクト
	Ray ray; // カメラからオブジェクトへのray
	RaycastHit hit; // rayとcamViewのヒット
	Vector3 hitWorld; // hitのワールド座標
	Vector3 hitLocal; // hitのローカル座標
	Transform camViewTransform; // CamViewのTransform
	float camViewPxW; // CamViewのピクセル（幅）
	float camViewPxH; // CamViewのピクセル（高さ）
	float camViewScaleW; // camViewのスケール（幅）
	float camViewScaleH; // camViewのスケール（高さ）
	int mask = 1 << 7; // rayのマスク
	void Start()
	{
		outline = gameObject.AddComponent<Outline>();
		outline.OutlineMode = Outline.Mode.OutlineAll;
		outline.OutlineWidth = 20f; // 輪郭の幅

		camViewObject = GameObject.Find("CamView");
		setCamViewScript = camViewObject.GetComponent<SetCamView>();

		camViewTransform = setCamViewScript.camViewTransform;
		camViewPxW = setCamViewScript.camViewPxW;
		camViewPxH = setCamViewScript.camViewPxH;
		camViewScaleW = setCamViewScript.camViewScaleW;
		camViewScaleH = setCamViewScript.camViewScaleH;

		camObject = GameObject.Find("Camera");
	}
	void Update()
	{
		myTransform = this.transform;

		var direction = myTransform.position - camObject.transform.position;

		ray = new Ray(camObject.transform.position, direction);
		if(Physics.Raycast(ray, out hit, mask)){
			hitWorld = hit.point; // hitのワールド座標
			hitLocal = camObject.transform.InverseTransformPoint(hitWorld); // hitのローカル座標

			// camView上の座標に変換
			hitLocal.x *= (camViewTransform.localPosition.z / myTransform.localPosition.z);
			hitLocal.y *= (camViewTransform.localPosition.z / myTransform.localPosition.z);

			// pixel座標を計算
			pixelX = hitLocal.x / (camViewScaleW/2) * (camViewPxW/2) + (camViewPxW/2); // 射影の中心のピクセル座標（x）計算
			pixelY = hitLocal.y / (camViewScaleH/2) * (camViewPxH/2) + (camViewPxH/2); // 射影の中心のピクセル座標（y）計算

			pixelColor = setCamViewScript.webcamTexture.GetPixel((int)pixelX, (int)pixelY); // 射影の中心のピクセル色取得
			// Debug.Log("pixelx" + pixelX + "pixely" + pixelY);

            // 輪郭色変更
			outline.OutlineColor = pixelColor;
			// ChangeNegaPosiColor();
			// ChangeComplementaryColor();
			// ChangeShade();
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