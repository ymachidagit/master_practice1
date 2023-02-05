using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uWindowCapture;

public class ChangeOutlineTest : MonoBehaviour
{
    Color color = new Color(255f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f);
    //float count = 0.0f;
    Outline outline;
    //Material material;

    //Renderer WindowRenderer;

    [SerializeField] UwcWindowTexture uwcTexture;

    //int pixelCount = 0;

    int pixelX = 0;
    int pixelY = 0;


    

    // Start is called before the first frame update
    void Start()
    {
        //Color color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        //var outlineColor = outline.OutlineColor;
        //outlineColor = color;
        outline.OutlineColor = color;
        outline.OutlineWidth = 10f;

        //WindowRenderer = GameObject.Find("Window").GetComponent<Renderer>();
        //material = GetComponent<Renderer>().material;
        Debug.Log("start");

        //outline.OutlineColor = window.GetPixel(pixelCount, pixelCount);
    }

    // Update is called once per frame
    void Update()
    {
        //Transform myTransform = this.transform;
        //Vector3 pos = myTransform.position;

        //Vector3 posInScreen = new Vector3(pos.x * 2, pos.y * 2, pos.z * 2);

        //Texture2D tex = WindowRenderer.material.mainTexture as Texture2D;

        //count += 1.0f;
        //color = new Color(255f / 255f, (0.0f + count) / 255f, 0.0f / 255f, 255f/ 255f);
        //outline.OutlineColor = color;

        //pixelCount += 1;

        pixelX += 1;
        pixelY += 1;

        var window = uwcTexture.window;

        //if (window == null)
        //{
        //    return;
        //}
        //else
        //{
        //    outline.OutlineColor = window.GetPixel(pixelCount, pixelCount);
        //    //var cursorPos = Lib.GetCursorPosition();
        //    //var x = cursorPos.x - window.x;
        //    //var y = cursorPos.y - window.y;
        //    //material.color = window.GetPixel(x, y);
        //    //Debug.Log(x);

        //pixelX = (int)posInScreen.x;
        //pixelY = (int)posInScreen.y;

        //pixelX *= tex.width;
        //pixelY *= tex.height;

        if (window == null)
        {
            return;
        }
        else
        {
            outline.OutlineColor = window.GetPixel(pixelX, pixelY);
            //var cursorPos = Lib.GetCursorPosition();
            //var x = cursorPos.x - window.x;
            //var y = cursorPos.y - window.y;
            //material.color = window.GetPixel(x, y);
            //Debug.Log(x);
        }

        //if (UwcManager.cursorWindow == window)
        //{
        //    //material.color = window.GetPixel(1 + pixelCount, 1 + pixelCount);

        //    var cursorPos = Lib.GetCursorPosition();
        //    var x = cursorPos.x - window.x;
        //    var y = cursorPos.y - window.y;
        //    material.color = window.GetPixel(x, y);
        //    Debug.Log(x);
        //    Debug.Log(window.GetPixel(x, y));

        //}
    }
}
