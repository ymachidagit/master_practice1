using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlaceObjectsTest : MonoBehaviour
{
  Color color1 = new Color(100f/255f, 30f/255f, 30f/255f, 1.0f);
  Color color2 = new Color(50f/255f, 50f/255f, 255f/255f, 1.0f);
  const int objectsRow = 5;
  const int objectsColumn = 10;
  GameObject[,] searchObjects = new GameObject[objectsRow, objectsColumn]; // 探索対象のオブジェクト
  GameObject camObject;
  Transform camTransform;
  float angleDiff; // オブジェクト間の角度差（y軸周り．横方向）
  float angle; // オブジェクトの配置角度（Degree）
  float angle0; // 右端のオブジェクトの角度（Degree）
  float angleRad; // オブジェクトの配置角度（Radian）
  float rangeAngle = 120f; // オブジェクト配置範囲（角度）
  float radius = 1.0f; // カメラと探索オブジェクトの距離（半径）
  float distanceY; // オブジェクト間の距離（y軸方向．縦方向）
  float positionY; // オブジェクトの配置座標（y）
  float rangeY = 0.8f; // オブジェクトの配置範囲（y座標）
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
  float taskTime; // 経過時間
  bool timeFlag; // 時間計測用フラグ
  [SerializeField] bool isOutline; // outlineを付けるか
  string[] guids_prefab;
  string[] guids_answer_prefab;
  void Start()
  {
    // カメラオブジェクトを取得
    camObject = GameObject.Find("Camera");
    // camObject = GameObject.Find("Main Camera");
    camTransform = camObject.GetComponent<Transform>();

    // オブジェクト同士の角度差と距離を算出
    angle0 = (180 - rangeAngle) / 2;
    angleDiff = rangeAngle / (float)(objectsColumn - 1);
    distanceY = rangeY / (float)(objectsRow - 1);

    // 枠線あり/なしに応じてアタッチするプレハブを変える
    if(isOutline)
    {
      guids_prefab = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/6x10Object"});
      guids_answer_prefab = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/6x1AnswerObject"});
    }
    else
    {
      guids_prefab = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/6x10ObjectNo"});
      guids_answer_prefab = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/6x1AnswerObjectNo"});
    }

    var preloadPrefabs = new List<GameObject>();

    answerNum = Random.Range(0, 6); // 0~5
    string path_answer_prefab = AssetDatabase.GUIDToAssetPath(guids_answer_prefab[answerNum]);
    answerObject = AssetDatabase.LoadAssetAtPath<GameObject>(path_answer_prefab);

    // 正解オブジェクトの生成
    this.transform.position = camObject.transform.position;
    this.transform.rotation = camObject.transform.rotation;
    answerObject = Instantiate(answerObject, new Vector3(0, -5, -5), Quaternion.identity, this.transform);
    if(answerObject.name.Contains("Color1")) answerObject.GetComponent<Renderer>().material.color = color1;
    else answerObject.GetComponent<Renderer>().material.color = color2;
    Debug.Log("正解オブジェクト" + answerObject);

    for(int i = 0 ; i < 6 ; i++)
    {
      if(i != answerNum)
      {
        for(int j = 0 ; j < 10 ; j++)
        {
          string path_prefab = AssetDatabase.GUIDToAssetPath(guids_prefab[i * 10 + j]);
          preloadPrefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(path_prefab));
        }
      }
    }

    // 格納したオブジェクトのシャッフル
    ShuffleObjects(preloadPrefabs);

    // preloadPrefabsのオブジェクトをsearchObjectsに移動
    for(int i = 0 ; i < objectsRow ; i++)
    {
      for(int j = 0 ; j < objectsColumn ; j++)
      {
        searchObjects[i, j] = preloadPrefabs[loopCount];
        loopCount += 1;
      }
    }

    // 正解オブジェクトの行と列
    answerI = Random.Range(0, objectsRow);
    answerJ = Random.Range(0, objectsColumn);

    // 一度プレハブを生成（GetComponentで色変更するため）
    for (int i = 0; i < objectsRow; i++)
    {
      for (int j = 0; j < objectsColumn; j++)
      {
        if(i == answerI && j == answerJ) searchObjects[i, j] = answerObject;
        else searchObjects[i, j] = Instantiate(searchObjects[i, j], new Vector3(0, -5, -5), Quaternion.identity, this.transform);
      }
    }

    // プレハブの色変更
    for (int i = 0; i < objectsRow; i++)
    {
      for (int j = 0; j < objectsColumn; j++)
      {
        if(!(i == answerI && j == answerJ))
        {
          if(searchObjects[i, j].name.Contains("Color1")) searchObjects[i, j].GetComponent<Renderer>().material.color = color1;
          else searchObjects[i, j].GetComponent<Renderer>().material.color = color2;
        }
      }
    }

    // コントローラ関連のスクリプトを取得
    cameraRigObject = GameObject.Find("[CameraRig]");
		inputControllerScript = cameraRigObject.GetComponent<InputController>();

    // 選択オブジェクトの初期位置設定
    iSelect = 3;
    jSelect = 5;
    selectCube = GameObject.Find("SelectCube");
    selectCube.transform.position = searchObjects[iSelect, jSelect].transform.position;
    selectCube.transform.rotation = searchObjects[iSelect, jSelect].transform.rotation;
  }
  void Update()
  {
    if(Input.GetKey(KeyCode.Space)) StartTask();
    if(timeFlag) taskTime += Time.deltaTime; // タスク時間の計測

    if (Input.GetKey(KeyCode.R)) ResetPlace(); // Rキーを押した時にプレハブを配置
    if(Input.GetKey(KeyCode.RightShift)) ShowAnswer(); // Aキーを押したときに正解オブジェクトを提示

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
        { // 上
          if(iSelect == objectsRow - 1) iSelect = objectsRow - 1;
          else iSelect += 1;
        }
        else
        { // 下
          if(iSelect == 0) iSelect = 0;
          else iSelect -= 1;
        }
      }
      else
      {
        if(posRight.x > 0)
        { // 右
          if(jSelect == 0) jSelect = 0;
          else jSelect -= 1;
        }
        else
        { // 左
          if(jSelect == objectsColumn - 1) jSelect = objectsColumn - 1;
          else jSelect += 1;
        }
      }
      pressFlag = true;
    }
    else if(!isPressedRight) pressFlag =false;

    selectCube.transform.position = searchObjects[iSelect, jSelect].transform.position;
    selectCube.transform.rotation = searchObjects[iSelect, jSelect].transform.rotation;

    // 正解オブジェクトを選択した状態でトリガーを引いた時にタスクを終了
    if(squeezeRight > 0.8)
    {
      if(iSelect == answerI && jSelect == answerJ) FinishTask();
    }
  }
  void ResetPlace()
  { // オブジェクトを現在のカメラ位置に合わせて再配置
    this.transform.position = camTransform.position; // SearchObjectsの座標をCameraの座標に
    this.transform.rotation = camTransform.rotation; // SearchObjectsの回転をCameraの回転に
    for (int i = 0; i < objectsRow; i++)
    {
      for (int j = 0; j < objectsColumn; j++)
      {
        angle = (angleDiff * j) + angle0; // このプレハブの回転角度（Degree）
        angleRad = angle * Mathf.Deg2Rad; // このプレハブの回転角度（Radian）
        positionY = distanceY * i - rangeY / 2; // このプレハブのy座標
        searchObjects[i, j].transform.localPosition = new Vector3(radius * Mathf.Cos(angleRad), positionY, radius * Mathf.Sin(angleRad)); // プレハブの配置
        searchObjects[i, j].transform.rotation = camTransform.rotation; // プレハブの回転をCameraに合わせる
        searchObjects[i, j].transform.Rotate(0.0f, -angle, 0.0f); // プレハブがCameraに対して正面を向くように回転
        if (searchObjects[i, j].name.Contains("Cone")) searchObjects[i, j].transform.localPosition -= new Vector3(0.0f, 0.05f, 0.0f); //Coneの高さ
        if (searchObjects[i, j].name.Contains("Cone")) searchObjects[i, j].transform.Rotate(-90f, 0.0f, 0.0f); //Coneの向き調整
      }
    }
  }
  void ShowAnswer()
  {
    this.transform.position = camTransform.position; // SearchObjectsの座標をCameraの座標に
    this.transform.rotation = camTransform.rotation; // SearchObjectsの回転をCameraの回転に
    for (int i = 0; i < objectsRow; i++)
    {
      for (int j = 0; j < objectsColumn; j++)
      {
        if(!(i == answerI && j == answerJ)) searchObjects[i, j].transform.localPosition = new Vector3(radius * Mathf.Cos(270*Mathf.Deg2Rad), 0, radius * Mathf.Sin(270*Mathf.Deg2Rad));
      }
    }
    answerObject.transform.localPosition = new Vector3(radius * Mathf.Cos(90*Mathf.Deg2Rad), positionY, radius * Mathf.Sin(90*Mathf.Deg2Rad)); // プレハブの配置
    answerObject.transform.rotation = camTransform.rotation; // プレハブの回転をCameraに合わせる
    answerObject.transform.Rotate(0.0f, -90, 0.0f); // プレハブがCameraに対して正面を向くように回転
    if (answerObject.name.Contains("Cone")) answerObject.transform.Rotate(-90f, 0.0f, 0.0f); //Coneの向き調整

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
  void StartTask()
  {
    taskTime = 0;
    timeFlag = true;
    ResetPlace();
  }
  void FinishTask()
  {
    timeFlag = false;
    Debug.Log("タスク完了時間：" + taskTime);
    UnityEditor.EditorApplication.isPlaying = false;
  }
}
