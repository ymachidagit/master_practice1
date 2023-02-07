using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uWindowCapture;

public class ChangeOutline : MonoBehaviour
{
	Outline outline; // 輪郭
	Color outlineColor = new Color(255f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f); // 輪郭色
	Color pixelColor = new Color(255f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f); // 射影の中心のピクセル色
	Color negaposiColor = new Color(255f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f); // ネガポジ反転後の色
	Color compColor = new Color(255f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f); // 補色
	float rgbMax; // 補色計算用 rgbの内最大の値
	float rgbMin; // 補色計算用 rgbの内最小の値
	float rgbMaxMin; // 補色計算用 rgbの内最大の値と最小の値の和
	[SerializeField] UwcWindowTexture uwcTexture;
	UwcWindow window;
	GameObject windowObject; // WindowのGameObject
	Transform windowTransform; // WindowのTransform
	Vector3 windowPos; // Windowの座標
	int pixelX; // 射影の中心のピクセル座標（x）
	int pixelY; // 射影の中心のピクセル座標（y）
	float windowPxW; // windowのピクセル（幅）
	float windowPxH; // windowのピクセル（高さ）
	float windowScaleW; // windowのスケール（幅）
	float windowScaleH; // windowのスケール（高さ）
	Transform myTransform; // このオブジェクトのTransform
	Vector3 myPos; // このオブジェクトの座標
	Vector3 posInWindow; // このオブジェクトのWindow上の座標

	void Start()
	{
		outline = gameObject.AddComponent<Outline>();
		outline.OutlineMode = Outline.Mode.OutlineAll;
		outline.OutlineColor = outlineColor; // 輪郭の色
		outline.OutlineWidth = 10f; // 輪郭の幅

		windowObject = GameObject.Find("Window");
		windowTransform = windowObject.transform;
		windowPos = windowTransform.position;
		windowScaleW = windowTransform.lossyScale.x;
		windowScaleH = windowTransform.lossyScale.y;

		Debug.Log("windowScaleW:" + windowScaleW);
		Debug.Log("windowScaleH:" + windowScaleH);
	}

	void Update()
	{
		window = uwcTexture.window;

		myTransform = this.transform;
		myPos = myTransform.position; // このオブジェクトの座標
		posInWindow = new Vector3 (myPos.x * windowPos.z / myPos.z, myPos.y * windowPos.z / myPos.z, windowPos.z); // 射影の座標計算

		windowPxW = window.width; // windowのピクセル（幅）取得
		windowPxH = window.height; // windowのピクセル（高さ）取得

		Debug.Log("windowW:" + windowPxW);
		Debug.Log("windowH:" + windowPxH);

		pixelX = (int)(posInWindow.x / (windowScaleW/2) * windowPxW/2 + windowPxW/2); // 射影の中心のピクセル座標（x）計算
		pixelY = (int)(posInWindow.y / (windowScaleH/2) * windowPxH/2 + windowPxH/2); // 射影の中心のピクセル座標（y）計算

		Debug.Log("pos.x:" + myPos.x);
		Debug.Log("posInScreen.x:" + posInWindow.x);
		Debug.Log("pixelX:" + pixelX);

		if (window == null)
		{
			return;
		}
		else
		{
			pixelColor = window.GetPixel(pixelX, pixelY); // 射影の中心のピクセル色取得

			// ネガポジ反転
			negaposiColor.r = (1.0f - pixelColor.r);
			negaposiColor.g = (1.0f - pixelColor.g);
			negaposiColor.b = (1.0f - pixelColor.b);

			// 補色の計算
			rgbMax = pixelColor.r;
			rgbMin = pixelColor.r;

			rgbMax = (pixelColor.g > rgbMax) ? pixelColor.g : rgbMax; // gの方が大きい場合g
			rgbMax = (pixelColor.b > rgbMax) ? pixelColor.b : rgbMax; // bの方が大きい場合b
			rgbMin = (pixelColor.g < rgbMin) ? pixelColor.g : rgbMin; // gの方が小さい場合g
			rgbMin = (pixelColor.b < rgbMin) ? pixelColor.b : rgbMin; // bの方が小さい場合b

			rgbMaxMin = rgbMax + rgbMin; // rgbの内最大の値と最小の値の和

			compColor.r = (rgbMaxMin - pixelColor.r);
			compColor.g = (rgbMaxMin - pixelColor.g);
			compColor.b = (rgbMaxMin - pixelColor.b);
			
			// outline.OutlineColor = negaposiColor; // 輪郭色をネガポジ反転色に変更
			outline.OutlineColor = compColor; // 輪郭色を補色に変更
		}
	}
}
