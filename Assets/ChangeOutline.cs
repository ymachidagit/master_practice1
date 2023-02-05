using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uWindowCapture;

public class ChangeOutline : MonoBehaviour
{
    Color color = new Color(255f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f);
    Outline outline;

    Color pixelColor = new Color(255f / 255f, 0.0f / 255f, 0.0f / 255f, 255f / 255f);

    //Renderer WindowRenderer;

    [SerializeField] UwcWindowTexture uwcTexture;

    int pixelX = 0;
    int pixelY = 0;

    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = color;
        outline.OutlineWidth = 10f;

        //WindowRenderer = GameObject.Find("Window").GetComponent<Renderer>();

        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        Transform myTransform = this.transform;
        Vector3 pos = myTransform.position;

        Vector3 posInScreen = new Vector3(pos.x * 2, pos.y * 2, pos.z * 2);

        //Texture2D tex = WindowRenderer.material.mainTexture as Texture2D;

        //count += 1.0f;
        //color = new Color(255f / 255f, (0.0f + count) / 255f, 0.0f / 255f, 255f/ 255f);
        //outline.OutlineColor = color;

        //pixelCount += 1;

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

        pixelX = (int)posInScreen.x + 50;
        pixelY = (int)posInScreen.y + 50;

        //pixelX *= 2;
        //pixelY *= 2;

        if (window == null)
        {
            return;
        }
        else
        {
            pixelColor = window.GetPixel(pixelX, pixelY);
            pixelColor.r = 1.0f;
            //pixelColor.g = pixelColor.b;
            //pixelColor.b = 1.0f - pixelColor.b;
            outline.OutlineColor = pixelColor;
            //outline.OutlineColor = window.GetPixel(pixelX, pixelY);
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
