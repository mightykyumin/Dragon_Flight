using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float m_speed = 20f;
    float m_lifetime = 1.5f;
    int m_power = 1;
    [SerializeField]
    PlayerControl m_player;
    public int Power { get { return m_power; } }
    public void initBullet(PlayerControl player)
    {
        m_player = player;
    }
    public void RemoveBullet()
    {
        //Destroy(gameObject); // C#은 디스토리 한다고해서 메모리에서 완전히 사라지는게 아님
        m_player.RemoveBullet(this);
    }
    void OnEnable() //활성화 될때마다
    {
        if (IsInvoking("RemoveBullet"))
            CancelInvoke("RemoveBullet");
        Invoke("RemoveBullet", m_lifetime);
    }

   

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * m_speed * Time.deltaTime;
        
    }
}
