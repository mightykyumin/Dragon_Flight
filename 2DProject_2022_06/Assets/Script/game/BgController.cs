using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgController : MonoBehaviour
{
    SpriteRenderer m_SprRenderer;
    Material m_mat;
    [SerializeField]
    float m_speed = 0.05f;
    float m_scale = 1f;     //속도 조절할떄 이걸 조절
    // Start is called before the first frame update

    public void SetSpeed(float scale)
    {
        m_scale = scale;
    }
    void Start()
    {
        m_SprRenderer = GetComponent<SpriteRenderer>();
        m_mat = m_SprRenderer.material;
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlayBGM(SoundManager.BgmClip.BGM01);
    }

    // Update is called once per frame
    void Update()
    {
        m_mat.mainTextureOffset += Vector2.up * m_speed * m_scale *Time.deltaTime;
        if(InGameUIManager.Instance !=null)
        InGameUIManager.Instance.SetDistScore(m_mat.mainTextureOffset.y);//화면 y값 이동값에 따른 점수 증가 구현
    }
}
