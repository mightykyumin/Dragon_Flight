using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : SingletonMonobehaviour<EffectPool>
{
    [SerializeField]
    GameObject m_fxExplosionPrefab;
    GameObjectPool<FXController> m_effectPool;
    public void CreateEffect(Vector3 pos)
    {
        var effect = m_effectPool.Get();
        effect.transform.position = pos;
        effect.gameObject.SetActive(true);
    }
    public void RemoveEffect(FXController effect)
    {
        if (m_effectPool == null) return;
        m_effectPool.Set(effect);   //풀에다 다시 넣기
    }
    void Start()
    {
        m_effectPool = new GameObjectPool<FXController>(10, () =>
       {
           var obj = Instantiate(m_fxExplosionPrefab);
           obj.transform.SetParent(transform);
           obj.transform.localPosition = Vector3.zero;
           obj.transform.localScale = Vector3.one;
           obj.SetActive(false);
           var effect = obj.GetComponent<FXController>();
           return effect;
       });
    }

    
}
