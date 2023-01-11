using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    enum EyeType
    {
        Normal,
        Angry
    }
    
    [SerializeField]
    float m_speed = 2f;
    int m_hp;
    int m_maxHp;
    int m_line;

    MonsterType m_type;
    Animator m_animator;
    public int Line { get { return m_line; } }
    [SerializeField]
    SpriteRenderer[] m_eyesprites;
    public MonsterType Type { get { return m_type; } }
    Coroutine m_eyeCoroutine;
    IEnumerator Coroutine_ChageEyes(EyeType type, float duration= 0f)
    {
        switch(type)
        {
            case EyeType.Normal:
                m_eyesprites[0].enabled = m_eyesprites[1].enabled = true;
                m_eyesprites[2].enabled = m_eyesprites[3].enabled = false;
                break;
            case EyeType.Angry:
                m_eyesprites[0].enabled = m_eyesprites[1].enabled = false;
                m_eyesprites[2].enabled = m_eyesprites[3].enabled = true;
                break;
        }
        
        yield return new WaitForSeconds(duration);
        if (type ==EyeType.Angry)
        {
            m_eyesprites[0].enabled = m_eyesprites[1].enabled = true;
            m_eyesprites[2].enabled = m_eyesprites[3].enabled = false;
        }
    }

    public void InitMonster( MonsterType type)
    {
        m_type = type;
        m_hp = m_maxHp = ((int)type+1);
    }
    public void SetMonster(int line)
    {
        m_hp = m_maxHp;
        m_line = line;
    }
    public void SetDie()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SfxClip.Mon_Die);
        InGameUIManager.Instance.SetHuntScore((uint)m_type + 1 *64);
        ItemManager.Instance.CreateItem(transform.position);    //죽을때 아이템 생성
        EffectPool.Instance.CreateEffect(transform.position);
    }
    public void SetDamage(int Power)
    {
        if (!gameObject.activeSelf) return;
        m_animator.Play("Hit", 0, 0f);
        StopCoroutine(m_eyeCoroutine);
        m_eyeCoroutine = StartCoroutine(Coroutine_ChageEyes(EyeType.Angry, 0.6f));
        if (Power < 0)
        {
            m_hp = 0;
        }
        else
        {
            m_hp -= Power;
        }
        if (m_hp <= 0)
        {
            if(Type == MonsterType.Bomb)
            {
                MonsterManager.Instance.RemoveMonsters(Line);
            }
            else
            {
                SetDie();
                MonsterManager.Instance.RemoveMonster(this,true);
            }
            
        }
        
        
        
    }
    public void Move()
    {
        transform.position += Vector3.down * m_speed *MonsterManager.Instance.Scale* Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Collider_Bottom"))
        {
            MonsterManager.Instance.RemoveMonster(this, false);
        }
        else if(collision.CompareTag("MyBullet"))
        {
            var bullet = collision.GetComponent<BulletController>();
            SetDamage(bullet.Power);
            bullet.RemoveBullet();
            //MonsterManager.Instance.RemoveMonster(this);
        }
        else if(collision.CompareTag("Invinsible"))
        {
            SetDamage(-1);
        }
    }
    void OnEnable()
    {
        m_eyeCoroutine = StartCoroutine(Coroutine_ChageEyes(EyeType.Normal));
        
    }

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

}
