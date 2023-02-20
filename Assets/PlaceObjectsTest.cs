using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlaceObjectsTest : MonoBehaviour
{
  Color colorRed = Color.red;
  Color colorBlue = Color.blue;
  GameObject[,] searchObjects = new GameObject[3, 14]; // 5行10列
  GameObject camObject;
  Transform camTransform;
  float angleDiff; // オブジェクト間の角度差（y軸周り．横方向）
  float angle; // オブジェクトの配置角度（Degree）
  float angleRad; // オブジェクトの配置角度（Radian）
  float rangeAngle = 150f; // オブジェクト配置範囲（角度）
  float radius = 1.0f; // カメラと探索オブジェクトの距離（半径）
  float distanceY; // オブジェクト間の距離（y軸方向．縦方向）
  float positionY; // オブジェクトの配置座標（y）
  float rangeY = 0.9f; // オブジェクトの配置範囲（y座標）
  GameObject cameraRigObject;
  InputController inputControllerScript;
  Vector2 posRight; // トラックパッド上のタッチしている座標
  bool isPressedRight; // トラックパッドを押しているか
  float squeezeRight; // トリガーを引いているか
  int iSelect; // 現在選択されているオブジェクトの行
  int jSelect; // 現在選択されているオブジェクトの列
  bool pressFlag; // トラックパッドを押したときに連続で入力されるのを防ぐためのフラグ
  GameObject selectCube; // 現在選択しているオブジェクトを囲うCube
  int answerNum; // 正解オブジェクトのassets/Prefabs/AnswerObjectPrefabsでの番号
  int loopCount = 0;
  GameObject answerObject; // 正解のオブジェクト
  int answerI; // 正解のオブジェクトの行
  int answerJ; // 正解のオブジェクトの列

  void Start()
  {
    // カメラオブジェクトを取得
    camObject = GameObject.Find("Camera");
    // camObject = GameObject.Find("Main Camera");
    camTransform = camObject.GetComponent<Transform>();

    // オブジェクト同士の角度差と距離を算出
    angleDiff = rangeAngle / (float)(searchObjects.GetLength(1) - 1);
    distanceY = rangeY / (float)(searchObjects.GetLength(0) - 1);

    // 正解オブジェクトの生成
    answerNum = Random.Range(0, 8); // 0~7
    var guids_answer_prefab = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/AnswerObjectPrefabs"});
    string path_answer_prefab = AssetDatabase.GUIDToAssetPath(guids_answer_prefab[answerNum]);
    answerObject = AssetDatabase.LoadAssetAtPath<GameObject>(path_answer_prefab);
    answerObject = Instantiate(answerObject, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
    if(answerObject.name.Contains("Color1")) answerObject.GetComponent<Renderer>().material.color = colorRed;
    else answerObject.GetComponent<Renderer>().material.color = colorBlue;

    // assetからプレハブのアタッチ
    var guids_prefab = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/ObjectPrefabs"});
    // preloadPrefabs = new GameObject[guids_prefab.Length - 6];
    var preloadPrefabs = new List<GameObject>();

    for(int i = 0 ; i < 8 ; i++)
    {
      if(i != answerNum)
      {
        for(int j = 0 ; j < 6 ; j++)
        {
          string path_prefab = AssetDatabase.GUIDToAssetPath(guids_prefab[i * 6 + j]);
          preloadPrefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(path_prefab));
        }
      }
    }

    // 格納したオブジェクトのシャッフル
    ShuffleObjects(preloadPrefabs);

    // preloadPrefabsのオブジェクトをsearchObjectsに移動
    for(int i = 0 ; i < 3 ; i++)
    {
      for(int j = 0 ; j < 14 ; j++)
      {
        searchObjects[i, j] = preloadPrefabs[loopCount];
        loopCount += 1;
      }
    }

    // 正解オブジェクトの行と列
    answerI = Random.Range(0, searchObjects.GetLength(0));
    answerJ = Random.Range(0, searchObjects.GetLength(1));

    // 一度プレハブを生成（GetComponentで色変更するため）
    for (int i = 0; i < searchObjects.GetLength(0); i++)
    {
      for (int j = 0; j < searchObjects.GetLength(1); j++)
      {
        if(i == answerI && j == answerJ) searchObjects[i, j] = answerObject;
        else searchObjects[i, j] = Instantiate(searchObjects[i, j], new Vector3(0, 0, 0), Quaternion.identity, this.transform);
      }
    }

    // プレハブの色変更
    for (int i = 0; i < searchObjects.GetLength(0); i++)
    {
      for (int j = 0; j < searchObjects.GetLength(1); j++)
      {
        if(!(i == answerI && j == answerJ))
        {
          if(searchObjects[i, j].name.Contains("Color1")) searchObjects[i, j].GetComponent<Renderer>().material.color = colorRed;
          else searchObjects[i, j].GetComponent<Renderer>().material.color = colorBlue;
        }
      }
    }

    ResetPlace(); // 配置

    // コントローラ関連のスクリプトを取得
    cameraRigObject = GameObject.Find("[CameraRig]");
		inputControllerScript = cameraRigObject.GetComponent<InputController>();

    // 選択オブジェクトの初期位置設定
    iSelect = 2;
    jSelect = 7;
    selectCube = GameObject.Find("SelectCube");
    selectCube.transform.position = searchObjects[iSelect, jSelect].transform.position;
    selectCube.transform.rotation = searchObjects[iSelect, jSelect].transform.rotation;
  }
  void Update()
  {
    if (Input.GetKey(KeyCode.R)) ResetPlace(); // Rキーを押した時にプレハブを配置

    // Viveコントローラの入力を取得
    posRight = inputControllerScript.posRight;
    isPressedRight = inputControllerScript.isPressedRight;
    squeezeRight = inputControllerScript.squeezeRight;

    // 入力に応じて選択オブジェクトを変更
    if(isPressedRight && !pressFlag)
    {
      if(posRight.y / posRight.x > 1 || posRight.y / posRight.x < -1)
      {
        if(posRight.y > 0)
        {
          Debug.Log("上");
          if(iSelect == searchObjects.GetLength(0) - 1) iSelect = searchObjects.GetLength(0) - 1;
          else iSelect += 1;
        }
        else
        {
          Debug.Log("下");
          if(iSelect == 0) iSelect = 0;
          else iSelect -= 1;
        }
      }
      else
      {
        if(posRight.x > 0)
        {
          Debug.Log("右");
          if(jSelect == 0) jSelect = 0;
          else jSelect -= 1;
        }
        else
        {
          Debug.Log("左");
          if(jSelect == searchObjects.GetLength(1) - 1) jSelect = searchObjects.GetLength(0) - 1;
          else jSelect += 1;
        }
      }
      pressFlag = true;
    }
    else if(!isPressedRight) pressFlag =false;

    selectCube.transform.position = searchObjects[iSelect, jSelect].transform.position;
    selectCube.transform.rotation = searchObjects[iSelect, jSelect].transform.rotation;

    // to do 正解オブジェクトを選択した状態でトリガーを引いた時にタスクを終了
    if(squeezeRight > 0.8)
    {
      if(iSelect == answerI && jSelect == answerJ) Debug.Log("END");
    }
  }
  void ResetPlace()
  { // オブジェクトを現在のカメラ位置に合わせて再配置
    this.transform.position = camTransform.position; // SearchObjectsの座標をCameraの座標に
    this.transform.rotation = camTransform.rotation; // SearchObjectsの回転をCameraの回転に
    for (int i = 0; i < searchObjects.GetLength(0); i++)
    {
      for (int j = 0; j < searchObjects.GetLength(1); j++)
      {
        angle = (angleDiff * j); // このプレハブの回転角度（Degree）
        angleRad = angle * Mathf.Deg2Rad; // このプレハブの回転角度（Radian）
        positionY = distanceY * i - rangeY / 2; // このプレハブのy座標
        searchObjects[i, j].transform.localPosition = new Vector3(radius * Mathf.Cos(angleRad), positionY, radius * Mathf.Sin(angleRad)); // プレハブの配置
        searchObjects[i, j].transform.rotation = camTransform.rotation; // プレハブの回転をCameraに合わせる
        searchObjects[i, j].transform.Rotate(0.0f, -angle, 0.0f); // プレハブがCameraに対して正面を向くように回転
        if (searchObjects[i, j].name.Contains("Cone")) searchObjects[i, j].transform.Rotate(-90f, 0.0f, 0.0f); //Coneの向き調整
      }
    }
  }
  void ShuffleObjects(List<GameObject> list)
  { // リストに格納したオブジェクトをシャッフル
    GameObject temp;
    int randomIndex;

    for(int i = 0 ; i < list.Count ; i++)
    {
      temp = list[i];
      randomIndex = Random.Range(0, list.Count);
      list[i] = list[randomIndex];
      list[randomIndex] = temp;
    }
  }
}
