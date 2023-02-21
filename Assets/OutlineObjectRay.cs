using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObjectRay : MonoBehaviour
{
	Outline outline; // 輪郭
	Color pixelColor; // 射影の中心のピクセル色
	float pixelX; // 射影の中心のピクセル座標（x）
	float pixelY; // 射影の中心のピクセル座標（y）
	Transform myTransform; // このオブジェクトのTransform
	GameObject camViewObject; // CamViewのGameObject
	SetCamView setCamViewScript; // SetCamView.cs
	GameObject camObject; // Cameraオブジェクト
	Vector3 direction; // Cameraから見たこのオブジェクトの方向
	Ray ray; // カメラからオブジェクトへのRay
	const int MASK = 1 << 7; // Rayのマスク
	RaycastHit hit; // RayとCamViewのHit
	Vector3 hitWorld; // Hitのワールド座標
	Vector3 hitLocal; // Hitのローカル座標
	Transform camViewTransform; // CamViewのTransform
	float camViewPxW; // CamViewのピクセル（幅）
	float camViewPxH; // CamViewのピクセル（高さ）
	float camViewScaleW; // CamViewのスケール（幅）
	float camViewScaleH; // CamViewのスケール（高さ）
	float pixelR;
	float pixelG;
	float pixelB;
	float rgbMax;
	float rgbMin;
	Color negaposiColor = new Color(0.0f, 0.0f, 0.0f, 1.0f); // ネガポジ反転後の色
	Color compColor = new Color(0.0f, 0.0f, 0.0f, 1.0f); // 補色
	const float MONO = 0.1f; // モノトーンと判断する閾値
	const float BLACKWHITE = 0.5f; // 白と黒の閾値
	void Start()
	{
		outline = gameObject.AddComponent<Outline>();
		outline.OutlineMode = Outline.Mode.OutlineAll;
		outline.OutlineWidth = 5f; // 輪郭の幅

		camViewObject = GameObject.Find("CamView");
		setCamViewScript = camViewObject.GetComponent<SetCamView>();
		camViewTransform = setCamViewScript.camViewTransform;
		camViewPxW = setCamViewScript.camViewPxW;
		camViewPxH = setCamViewScript.camViewPxH;
		camViewScaleW = setCamViewScript.camViewScaleW;
		camViewScaleH = setCamViewScript.camViewScaleH;

		camObject = GameObject.Find("Camera");
		// camObject = Camera.main.gameObject; // TestScene用
	}
	void Update()
	{
		myTransform = this.transform;
		direction = myTransform.position - camObject.transform.position; // Cameraから見たこのオブジェクトの方向

		ray = new Ray(camObject.transform.position, direction); // CameraからこのオブジェクトへのRay
		if (Physics.Raycast(ray, out hit, MASK))
		{
			hitWorld = hit.point; // hitのワールド座標
			hitLocal = camObject.transform.InverseTransformPoint(hitWorld); // hitのローカル座標

			// camView上の座標に変換
			hitLocal.x *= (camViewTransform.localPosition.z / myTransform.localPosition.z);
			hitLocal.y *= (camViewTransform.localPosition.z / myTransform.localPosition.z);

			pixelX = hitLocal.x / (camViewScaleW / 2) * (camViewPxW / 2) + (camViewPxW / 2); // 射影の中心のピクセル座標（x）計算
			pixelY = hitLocal.y / (camViewScaleH / 2) * (camViewPxH / 2) + (camViewPxH / 2); // 射影の中心のピクセル座標（y）計算

			pixelColor = setCamViewScript.webcamTexture.GetPixel((int)pixelX, (int)pixelY); // 射影の中心のピクセル色取得
			pixelR = pixelColor.r;
			pixelG = pixelColor.g;
			pixelB = pixelColor.b;

			// 輪郭色変更
			// r, g, bの値の差が小さい時，黒または白と判断
			if(Mathf.Abs(pixelR - pixelG) <= MONO && Mathf.Abs(pixelG - pixelB) <= MONO && Mathf.Abs(pixelB - pixelR) <= MONO)
			{
				if(((pixelR + pixelG + pixelB)/3) >= BLACKWHITE) outline.OutlineColor = new Color(0.0f, 0.0f, 0.0f, 1.0f); // 白なら枠線を黒に
				else outline.OutlineColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); // 黒なら枠線を白に
				return;
			}
			else
			{
				// outline.OutlineColor = pixelColor;
				ChangeNegaPosiColor();
				// ChangeComplementaryColor();
				// ChangeShade();
			}
		}
		Debug.DrawRay(ray.origin, ray.direction, Color.red);
	}
	void ChangeNegaPosiColor()
	{ // ネガポジ反転
		negaposiColor.r = (1.0f - pixelR);
		negaposiColor.g = (1.0f - pixelG);
		negaposiColor.b = (1.0f - pixelB);

		// 色を濃くする
		rgbMax = negaposiColor.r; // ネガポジ反転したrgbの内最大の値
		rgbMax = (negaposiColor.g > rgbMax) ? negaposiColor.g : rgbMax; // gの方が大きい場合g
		rgbMax = (negaposiColor.b > rgbMax) ? negaposiColor.b : rgbMax; // bの方が大きい場合b
		negaposiColor.r *= 1.0f / rgbMax;
		negaposiColor.g *= 1.0f / rgbMax;
		negaposiColor.b *= 1.0f / rgbMax;
		
		outline.OutlineColor = negaposiColor;
	}
	void ChangeComplementaryColor()
	{ // 補色
		rgbMax = pixelR; // rgbの内最大の値
		rgbMax = (pixelG > rgbMax) ? pixelG : rgbMax; // gの方が大きい場合g
		rgbMax = (pixelB > rgbMax) ? pixelB : rgbMax; // bの方が大きい場合b

		rgbMin = pixelR; // rgbの内最小の値
		rgbMin = (pixelG < rgbMin) ? pixelG : rgbMin; // gの方が小さい場合g
		rgbMin = (pixelB < rgbMin) ? pixelB : rgbMin; // bの方が小さい場合b

		// 補色に変換
		compColor.r = (rgbMax + rgbMin - pixelR);
		compColor.g = (rgbMax + rgbMin - pixelG);
		compColor.b = (rgbMax + rgbMin - pixelB);

		// 色を濃くする
		compColor.r *= 1.0f / rgbMax;
		compColor.g *= 1.0f / rgbMax;
		compColor.b *= 1.0f / rgbMax;

		outline.OutlineColor = compColor;
	}
	void ChangeShade()
	{ // 同系統の色の薄い色/濃い色に変換

		Color shadeColor = new Color(0.0f, 0.0f, 0.0f, 1.0f); // 変換した色

		float rgbTotal = pixelColor.r + pixelColor.g + pixelColor.b;

		if (rgbTotal >= 1.5)
		{
			shadeColor = new Color(pixelColor.r - 0.5f, pixelColor.g - 0.5f, pixelColor.b - 0.5f);
		}
		else
		{

			shadeColor = new Color(pixelColor.r + 0.5f, pixelColor.g + 0.5f, pixelColor.b + 0.5f);
		}
		outline.OutlineColor = shadeColor;
	}
}