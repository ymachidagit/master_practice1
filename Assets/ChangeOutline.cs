using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uWindowCapture;

public class ChangeOutline : MonoBehaviour
{
	Outline outline; // 輪郭
	Color outlineColor; // 輪郭色
	Color pixelColor; // 射影の中心のピクセル色
	int pixelX; // 射影の中心のピクセル座標（x）
	int pixelY; // 射影の中心のピクセル座標（y）
	Transform myTransform; // このオブジェクトのTransform
	Vector3 myPos; // このオブジェクトの座標
	Vector3 posInWindow; // このオブジェクトのWindow上の座標
	GameObject windowObject; // WindowのGameObject
	SetWindow setWindowScript; // SetWindow.cs

	void Start()
	{
		outline = gameObject.AddComponent<Outline>();
		outline.OutlineMode = Outline.Mode.OutlineAll;
		outline.OutlineWidth = 10f; // 輪郭の幅

		windowObject = GameObject.Find("Window");
		setWindowScript = windowObject.GetComponent<SetWindow>();
	}
	void Update()
	{
		myTransform = this.transform;
		myPos = myTransform.position; // このオブジェクトの座標
		
		posInWindow = new Vector3 (myPos.x * setWindowScript.windowPos.z / myPos.z, myPos.y * setWindowScript.windowPos.z / myPos.z, setWindowScript.windowPos.z); // 射影の座標計算

		pixelX = (int)(posInWindow.x / (setWindowScript.windowScaleW/2) * setWindowScript.windowPxW/2 + setWindowScript.windowPxW/2); // 射影の中心のピクセル座標（x）計算
		pixelY = (int)(posInWindow.y / (setWindowScript.windowScaleH/2) * setWindowScript.windowPxH/2 + setWindowScript.windowPxH/2); // 射影の中心のピクセル座標（y）計算
		
		// Debug.Log("posInWindow:" + posInWindow);
		// Debug.Log("pixelX:" + pixelX);
		// Debug.Log("pixelY:" + pixelY);

		if (setWindowScript.window == null)
		{
			return;
		}
		else
		{
			pixelColor = setWindowScript.window.GetPixel(pixelX, pixelY); // 射影の中心のピクセル色取得

			// outline.OutlineColor = pixelColor;
			// ChangeNegaPosiColor();
			ChangeComplementaryColor();
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
}
