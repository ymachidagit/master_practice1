using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamView : MonoBehaviour
{
	// int width = 1200;
	// int height = 720;
	// int fps = 60;
	public WebCamTexture webcamTexture;
	public GameObject camViewObject; // CamViewのGameObject
	public Transform camViewTransform; // CamViewのTransform
	public Vector3 camViewPos; // CamViewの座標
	public float camViewPxW; // CamViewのピクセル（幅）
	public float camViewPxH; // CamViewのピクセル（高さ）
	public float camViewScaleW; // CamViewのスケール（幅）
	public float camViewScaleH; // CamViewのスケール（高さ）
	void Start () 
	{
		WebCamDevice[] devices = WebCamTexture.devices;
		// for(int i = 0 ; i <= devices.Length ; i++){
		// 	Debug.Log("デバイス:" + devices[i]);
		// }
		webcamTexture = new WebCamTexture(devices[0].name);
		GetComponent<Renderer> ().material.mainTexture = webcamTexture;
		webcamTexture.Play();

		camViewTransform = camViewObject.transform;
		camViewPos = camViewTransform.position; // CamViewの座標取得
		camViewScaleW = camViewTransform.lossyScale.x; // CamViewのスケール（幅）取得
		camViewScaleH = camViewTransform.lossyScale.y; // CamViewのスケール（高さ）取得
		camViewPxW = webcamTexture.width; // CamViewのピクセル（幅）取得
		camViewPxH = webcamTexture.height; // CamViewのピクセル（高さ）取得

		Debug.Log("camViewPos:" + camViewPos);
		Debug.Log("camViewScaleW:" + camViewScaleW);
		Debug.Log("camViewScaleH:" + camViewScaleH);
		Debug.Log("camViewPxW:" + camViewPxW);
		Debug.Log("camViewPxH:" + camViewPxH);
	}
	void Update()
	{
			
	}
}
