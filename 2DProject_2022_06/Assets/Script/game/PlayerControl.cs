using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] m_bodyParts;
    [SerializeField]
    GameObject m_fxMagnetObj;
    [SerializeField]
    float m_speed = 4f;
    Vector3 m_dir;
    [SerializeField]
    GameObject m_bulletprefab;
    [SerializeField]
    Transform m_FirePos;
    [SerializeField]
    GameObject m_fxInvincibleObj;
    [SerializeField]
    BuffController m_buffctr;
    GameObjectPool<BulletController> m_bulletPool;
    Animator m_animator;
    CameraShake m_camShake;
    Vector3 m_startPos;     //마우스 드래그 시작위치
    //마우스 눌렀을때 땟을때 확인 변수 드래그중
    bool m_isDrag;
    
    

    float m_moveVal; //이동값
    public void SetBuff(BuffType type)
    {
        
        m_buffctr.SetBuff(type);
    }
    public void SetMagnetEffect()
    {
        m_fxMagnetObj.SetActive(true);
    }
    public void EndMagnetEffect()
    {
        m_fxMagnetObj.SetActive(false);
    }
    public void SetPlayer(bool isInvincible)
    {
        if(isInvincible)
        {

            CancelInvoke("CreateBullet");
            m_animator.Play("Invinsible", 0, 0f);     //animator 실행
            
            m_fxInvincibleObj.SetActive(true);
            m_camShake.Shake(15f, m_buffctr.GetBuffData(BuffType.Invincible).duration);
        }
        else
        {
            if (!IsInvoking("CreateBullet"))
                InvokeRepeating("CreateBullet", 0.1f, 0.15f);
            m_animator.Play("Idle2", 0, 0f);
            m_fxInvincibleObj.SetActive(false);
        }
        
    }
    public void SetInvincibleEffect()
    {
        GameStateManager.Instance.SetState(GameState.Invincible);
    }
    public void EndInvincibleEffect()
    {
        GameStateManager.Instance.SetState(GameState.Normal);
    }
    public void RemoveBullet(BulletController bullet)
    {
        bullet.gameObject.SetActive(false);
        m_bulletPool.Set(bullet);
    }
    public void SetDie()
    {
        gameObject.SetActive(false);
        CancelInvoke("CreateBullet");
        
    }
    void LoadHeroSprites()
    {
        var index = PlayerDataManager.Instance.Heroindex;       //인덱스 받아오기
        var parts= Resources.LoadAll<Sprite>(string.Format("Heroes/sunny_{0:00}",index +1));
        m_bodyParts[0].sprite = parts[0];
        m_bodyParts[1].sprite = m_bodyParts[2].sprite = parts[1];
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Monster"))
        {
            GameStateManager.Instance.SetState(GameState.Result);
        }
        
    }
    public void CreateBullet()
    {
        var bullet = m_bulletPool.Get();
        bullet.gameObject.SetActive(true);
        bullet.transform.position = m_FirePos.position;
        
    }
   

    // Start is called before the first frame update
    void Start()
    {
        m_camShake = Camera.main.GetComponent<CameraShake>();
        m_animator = GetComponent<Animator>();
        m_fxInvincibleObj.SetActive(false);
        m_fxMagnetObj.SetActive(false);
        m_bulletPool = new GameObjectPool<BulletController>(10, () =>
        {
            var obj = Instantiate(m_bulletprefab);
            obj.SetActive(false);
            var bullet = obj.GetComponent<BulletController>();
            bullet.initBullet(this);
            return bullet;
        });
        LoadHeroSprites();
        InvokeRepeating("CreateBullet", 2f,0.12f);     //예약 걸어두기(매개변수 없을때만 가능), 있으면 코루틴으로 해야함
    }

    // Update is called once per frame
    void Update()
    {
        m_moveVal = m_speed * Time.deltaTime;
        m_dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //마우스 input받기
        if (Input.GetMouseButtonDown(0))
        {
            m_isDrag = true;
            m_startPos =Camera.main.ScreenToWorldPoint(Input.mousePosition);   // 마우스의 스크린좌표를 월드 좌표로
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_isDrag = false;
        }
        if (m_isDrag)
        {
            var endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("Start Pos : " + m_startPos + " EndPos :" + endPos);
            var dir =  endPos - m_startPos;
            dir.y = 0f;
            m_dir = dir.normalized;
            m_moveVal = Mathf.Abs(dir.x);
            var hit = Physics2D.Raycast(transform.position, m_dir, m_moveVal, 1 << LayerMask.NameToLayer("Background"));
            if(hit.collider != null)
            {
                if (m_dir != Vector3.right && hit.collider.CompareTag("Collider_Left") || m_dir != Vector3.left && hit.collider.CompareTag("Collider_Right"))
                {
                    m_moveVal = hit.distance;
                }
                    
            }
            
            //transform.position += dir;
            m_startPos = endPos;
        }
        transform.position += m_dir * m_moveVal;



    }
}
