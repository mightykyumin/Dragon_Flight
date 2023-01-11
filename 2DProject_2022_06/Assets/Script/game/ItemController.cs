using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer m_iconRenderer;  //
    [SerializeField]
    AnimationCurve m_curveY = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    AnimationCurve m_curveX = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    
    float m_duration = 1f;
    [SerializeField]
    Vector3 m_from;
    [SerializeField]
    Vector3 m_to;
    [SerializeField]
    TweenRotation m_tweenRot;
    Coroutine m_coroutineMove;
    ItemManager.ItemType m_type;
    PlayerControl m_player;
    bool m_isMagnet;
    bool m_isResume;
    public ItemManager.ItemType Type { get { return m_type; } }
    IEnumerator Coroutine_MovePosition()
    {
        float time = 0;
        while(true)
        {
            
            if (!m_isMagnet)    //마그넷이 꺼져있을떄만 코루틴 작동
            {
                time += Time.deltaTime / m_duration;
                var yValue = Vector3.up * m_curveY.Evaluate(time) * 4f;
                var xValue = m_curveX.Evaluate(time);
                transform.position = m_from * (1f - xValue) + (xValue * m_to) + yValue;
                if (time > 1f)
                {

                    ItemManager.Instance.RemoveItem(this);
                    yield break;
                }
            }
            else if (m_isResume)
            {
                m_from = transform.position;
                m_to = new Vector3(transform.position.x, ItemManager.Instance.m_endPosY);
                m_isResume = false;
            }

            yield return null;
        }
    }
    public void SetItem(Vector3 from, Vector3 to, float duration, ItemManager.ItemType type)
    {
        m_isMagnet = false;     //처음엔 마그넷 끄기
        m_isResume = false;
        gameObject.SetActive(true);
        m_type = type;
        StartMove(from, to, duration);
        transform.rotation = Quaternion.identity; //0으로 로테이션 초기화(다시 만들어질떄 초기화 해야함)
        if (type >= ItemManager.ItemType.Gem_Red && type <= ItemManager.ItemType.Gem_Blue)  // 보석은 rotation
        {
            m_tweenRot.enabled = true;  //재생
            m_tweenRot.ResetToBeginning();
            m_tweenRot.PlayForward();
        }
        else
        {
            m_tweenRot.enabled = false;  //끄기
            
        }
        m_iconRenderer.sprite = ItemManager.Instance.GetIconSprite(type);   //
    }

    public void StartMove(Vector3 from, Vector3 to, float duration)
    {
        m_from = from;
        m_to = to;
        m_duration = duration;
        StartMove();
    }
    public void InitItem(PlayerControl player)
    {
        m_player = player;
    }
    void StartMove()
    {
        if (m_coroutineMove != null)
            StopCoroutine(m_coroutineMove);
        m_coroutineMove = StartCoroutine("Coroutine_MovePosition");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ItemManager.Instance.RemoveItem(this);
            switch(m_type)
            {
                case ItemManager.ItemType.Coin:
                    SoundManager.Instance.PlaySFX(SoundManager.SfxClip.Get_Coin);
                    InGameUIManager.Instance.SetCoinOwnedCount(1);  // 코인 먹을떄마다 1씩 증가
                    break;
                case ItemManager.ItemType.Gem_Red:
                case ItemManager.ItemType.Gem_Green:
                case ItemManager.ItemType.Gem_Blue:
                    SoundManager.Instance.PlaySFX(SoundManager.SfxClip.Get_Gem);
                    InGameUIManager.Instance.SetCoinOwnedCount((uint)(m_type)*10);
                    break;
                case ItemManager.ItemType.Invincible:
                    SoundManager.Instance.PlaySFX(SoundManager.SfxClip.Get_invincible);
                    m_player.SetBuff(BuffType.Invincible);
                    break;
                case ItemManager.ItemType.Magnet:
                    SoundManager.Instance.PlaySFX(SoundManager.SfxClip.Get_Item);
                    m_player.SetBuff(BuffType.Magnet);
                    break;
            }
        }
        else if(collision.CompareTag("Magnet"))// Magnet collider에 부딫이면
        {
            m_isMagnet = true;
        }

    }
    void OnTriggerExit2D(Collider2D collision)  // Magnet collider에서 나가면
    {
        m_isMagnet = false;
        m_isResume = true;
        
    }
    void Awake()
    {
        m_tweenRot = GetComponent<TweenRotation>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
            m_from = transform.position;
            m_to = transform.position + Vector3.right * 2.5f + Vector3.down * 4.5f;
            StartMove();
        }
        if(m_isMagnet)
        {
            var dir =  m_player.transform.position - transform.position; //magnet이면 플레이어 포지션 가져오기 player 위치 - 내위치
            transform.position += dir.normalized * 16f * Time.deltaTime; // 적당한 속도로 빨려들어가기
        }
        
    }
}
