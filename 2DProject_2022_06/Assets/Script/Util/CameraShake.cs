using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    float m_duration = 1f;
    [SerializeField]
    float m_power = 0.5f;
    Vector3 m_orgPos;
    bool m_isStart;
    float m_time;
    public void Shake(float power, float duration)
    {
        m_power = power;
        m_duration = duration;
        m_isStart = true;
    }
    void Start()
    {
        m_orgPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isStart)
        {
            m_time += Time.deltaTime / m_duration;
            Vector3 dir = Random.insideUnitCircle;             //반지름이 1 인 원을만들어 랜덤한 위치 선택
            
            transform.localPosition = dir * m_power * Time.deltaTime + Vector3.forward * m_orgPos.z;
            if(m_time > 1f)
            {
                m_isStart = false;
                m_time = 0f;
                transform.position = m_orgPos;
            }
        }
    }
}
