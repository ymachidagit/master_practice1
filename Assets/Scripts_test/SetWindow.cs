using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uWindowCapture;

public class SetWindow : MonoBehaviour
{
	[SerializeField] UwcWindowTexture uwcTexture;
	public UwcWindow window;
	public GameObject windowObject; // WindowのGameObject
	public Transform windowTransform; // WindowのTransform
	public Vector3 windowPos; // Windowの座標
	public float windowPxW; // Windowのピクセル（幅）
	public float windowPxH; // Windowのピクセル（高さ）
	public float windowScaleW; // Windowのスケール（幅）
	public float windowScaleH; // Windowのスケール（高さ）

	void Start()
	{
		windowObject = GameObject.Find("Window");
	}
	void Update()
	{
		window = uwcTexture.window;

		windowTransform = windowObject.transform;
		windowPos = windowTransform.position; // Windowの座標取得
		windowScaleW = windowTransform.lossyScale.x; // Windowのスケール（幅）取得
		windowScaleH = windowTransform.lossyScale.y; // Windowのスケール（高さ）取得
		windowPxW = window.width; // windowのピクセル（幅）取得
		windowPxH = window.height; // windowのピクセル（高さ）取得

		Debug.Log("windowScaleW:" + windowScaleW);
		Debug.Log("windowScaleH:" + windowScaleH);
		Debug.Log("windowPxW:" + windowPxW);
		Debug.Log("windowPxH:" + windowPxH);
	}
}
