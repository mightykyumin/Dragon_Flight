using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    None =-1,
    Magnet,
    Invincible,
    Max
}
public struct BuffData
{
    public BuffType type;
    public float duration;
    public string onStart;
    public string onFinish;
}
public class BuffInfo
{
    public BuffData data;
    public float time;
    
}

public class BuffController : MonoBehaviour
{
    List<BuffData> m_buffTable = new List<BuffData>()   // list선언
    {
        new BuffData(){type = BuffType.Magnet, duration =5f,onStart = "SetMagnetEffect", onFinish ="EndMagnetEffect" },  //생성자를 이용해 list넣기
        new BuffData(){type= BuffType.Invincible, duration =3f,onStart = "SetInvincibleEffect",onFinish = "EndInvincibleEffect" }
    };
    Dictionary<BuffType, BuffInfo> m_buffList = new Dictionary<BuffType, BuffInfo>();
    IEnumerator Coroutine_BuffProcess(BuffType type)
    {
        gameObject.SendMessage(m_buffList[type].data.onStart);  //gameobject에 메세지를 보내 이 method가 있으면 실행하라고 하는거임(빠르지 않음)
        while(true)
        {
            yield return null;
            m_buffList[type].time += Time.deltaTime / m_buffList[type].data.duration;
            if (m_buffList[type].time > 1f)
            {
                gameObject.SendMessage(m_buffList[type].data.onFinish);
                m_buffList.Remove(type);
                yield break;
            }
        }
    }
    public void SetBuff(BuffType type)
    {
        var buffData = m_buffTable.Find(buff => buff.type == type); // table 에서 가져온 타입 찾기
        if(m_buffList.ContainsKey(type))        // 타입을 찾으면
        {
            var buffInfo = m_buffList[type];        // buffinfo에 넣기
            buffInfo.time = 0f;
        }
        else
        {
            m_buffList.Add(type, new BuffInfo() { data = buffData, time = 0 });
            StartCoroutine(Coroutine_BuffProcess(type));
        }
    }
    public BuffData GetBuffData(BuffType type)
    {
        return m_buffTable.Find(buff => buff.type == type);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
