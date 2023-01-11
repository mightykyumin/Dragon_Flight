using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteToPng : MonoBehaviour
{
    [SerializeField]
    UITexture m_texture;
    Sprite[] m_sprites;

    
    // Start is called before the first frame update
    void Start()
    {
        m_sprites = Resources.LoadAll<Sprite>("Fonts/text_01");
#if UNITY_EDITOR
        Debug.Log(Application.dataPath);
#else
    Debug.Log(Application.persistentDataPath);
#endif

        for(int i =0; i<m_sprites.Length; i++)
        {
            var spr = m_sprites[i];
            Texture2D texture = new Texture2D((int)spr.rect.width, (int)spr.rect.height, TextureFormat.ARGB32, false);
            m_texture.mainTexture = texture;
            for(int y= 0; y<texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, spr.texture.GetPixel((int)spr.rect.x+x, (int)spr.rect.y+y));
                }
            }
            texture.Apply();
            var  imageBytes=  texture.EncodeToPNG();
            File.WriteAllBytes(string.Format("{0}/Images/ImageFont/imageFont_{1:00}.png", Application.dataPath, i + 1),imageBytes);
            
            m_texture.MakePixelPerfect();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
