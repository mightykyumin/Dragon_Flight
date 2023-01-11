using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResolution : MonoBehaviour
{

    public enum ClearMode
    {
        None,
        Clear
    }
    [SerializeField]
    ClearMode m_mode;
    Camera m_camera;
    

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();
        Rect viewRect = new Rect(0f, 0f, 1f, 1f);
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)2 / 3);
        float scaleWidth = 1f / scaleHeight;

        if(scaleWidth >1f)
        {
            viewRect.height = scaleHeight;
            viewRect.y= (1f - scaleHeight) / 2f;
        }
        else
        {
            viewRect.width = scaleWidth;
            viewRect.x = (1f - scaleWidth) / 2f;
        }
        m_camera.rect = viewRect;
          
    }
    private void OnPreCull()
    {
        if (m_mode == ClearMode.None) return;

        GL.Clear(true, true, Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
