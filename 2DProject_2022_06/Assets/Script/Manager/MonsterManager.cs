using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MonsterType
{
    White,
    Yellow,
    Pink,
    Bomb,
    max
}
public class MonsterManager : SingletonMonobehaviour<MonsterManager>
{
    
    GameObject[] m_monPrefabs;
    Dictionary <MonsterType,  GameObjectPool<MonsterController>> m_monsterPool= new Dictionary<MonsterType, GameObjectPool<MonsterController>> ();
    List<MonsterController> m_monsterList = new List<MonsterController>();
    Vector2 m_spawnPos = new Vector2(-2.68f, 6f);
    float m_PosGap = 1.35f;
    float interval = 4f;
    int m_line;
    float m_scale = 1f;
    public float Scale { get { return m_scale; } set { m_scale = value; } }
    #region Public Methods
    public void StopCreateMonster()
    {
        CancelInvoke("CreateMonsters");
    }
    public void ResetCreateMonsters(float scale)
    {
        Scale = scale;
        CancelInvoke("CreateMonsters");
        InvokeRepeating("CreateMonsters", Scale == 1 ?0f :interval / Scale, interval/ Scale);
    }
    public void RemoveMonster(MonsterController mon, bool isDie = false)
    {
        mon.gameObject.SetActive(false);
        if (m_monsterList.Remove(mon))
        {
            if(isDie)
                mon.SetDie();
            m_monsterPool[mon.Type].Set(mon);
        }
            

    }
    public void RemoveMonsters(int line)
    {
        for(int i= 0; i<m_monsterList.Count; i++)
        {
            if(m_monsterList[i].Line ==line)
            {
                m_monsterList[i].SetDie();
                m_monsterList[i].gameObject.SetActive(false);
                m_monsterPool[m_monsterList[i].Type].Set(m_monsterList[i]);
            }
        }
        m_monsterList.RemoveAll(mon => mon.gameObject.activeSelf == false);
    }
    #endregion
    void CreateMonsters()
    {
        bool isBomb = false;
        bool isTry = false;
        for(int i =0; i<5; i++)     //5개 한줄에 만듬
        {
            
            MonsterType type;
            do
            {          // 폭탄몬스터가 한줄에 하나이상 나오지 않게
                isTry = false;
                type = (MonsterType)UnityEngine.Random.Range((int)MonsterType.White, (int)MonsterType.max);
                if(type == MonsterType.Bomb && !isBomb)       // 타입은 폭탄이고 아직 폭탄이 없을때
                {
                    isBomb = true;      // isbomb을 true로 바꿔줘서 그 해당 줄에 있는거 알려줌
                }
                else if(type == MonsterType.Bomb && isBomb)
                {
                    isTry = true;   
                }
            } while (isTry);
            
            
            var mon = m_monsterPool[type].Get();
            mon.transform.position = m_spawnPos + Vector2.right * (i * m_PosGap);
            mon.SetMonster(m_line);
            mon.gameObject.SetActive(true);
            m_monsterList.Add(mon);
        }
        m_line++;
    }
    

    protected override void OnStart()
    {
        m_monPrefabs = Resources.LoadAll<GameObject>("Prefab/Monster/");
        for(int i=0; i<m_monPrefabs.Length;i++)
        {
            var index = Convert.ToInt32(m_monPrefabs[i].name.Split('.')[0]) - 1; // prefab이름으로 index 만들기
            GameObjectPool<MonsterController> pool = new GameObjectPool<MonsterController>(2,()=>
            {
                var obj = Instantiate(m_monPrefabs[index]);         //i 를 람다안에 넣으면 안댐
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                var mon = obj.GetComponent<MonsterController>();
                mon.InitMonster((MonsterType)index);
                return mon; 
            });
            m_monsterPool.Add((MonsterType)index,pool);
        }
        /*m_monsterPool = new GameObjectPool<MonsterController>(10, () =>
        {
            var obj = Instantiate(m_monPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            var mon = obj.GetComponent<MonsterController>();
            return mon;
        });*/
        InvokeRepeating("CreateMonsters", 3f, interval);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i< m_monsterList.Count;i++)
        {
            m_monsterList[i].Move();
        }
    }
}
