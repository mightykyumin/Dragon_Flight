using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDataManager : DontDestroy<PlayerDataManager>
{
    PlayerData m_myData;
    public uint BestRecord { get { return m_myData.bestRecord; } set { m_myData.bestRecord = value; } }
    public byte Heroindex { get { return m_myData.heroIndex; } set { m_myData.heroIndex = value; } }
    public uint GetGold()
    {
        return m_myData.goldOwned;
    }
    public uint GetGem()
    {
        return m_myData.gemOwned;
    }
    public void IncreaseGold(uint gold)
    {
        m_myData.goldOwned += gold;
    }
    public bool DecreaseGold(uint gold)
    {
        if ((int)m_myData.goldOwned - gold < 0)
            return false;
        m_myData.goldOwned -= gold;
        return true;
    }
    public void IncreaseGem(uint gem)
    {
        m_myData.gemOwned += gem;
    }
    public bool DecreaseGem(uint gem)
    {
        if ((int)m_myData.gemOwned - gem < 0)
            return false;
        m_myData.gemOwned -= gem;
        return true;
    }
    public bool IsPlayable(int index)
    {
        return m_myData.m_herosList[index].isPlayable;
    }
    public void SetPlayableHero(int index)
    {
        m_myData.m_herosList[index].isPlayable = true;
    }
    public PlayerData Load()
    {

        var jsonData = PlayerPrefs.GetString("PLAYER_DATA", string.Empty);
        
        if (string.IsNullOrEmpty(jsonData))
        {
            
            return null;
        }
        return JsonUtility.FromJson<PlayerData>(jsonData);        //JsonReader.Deserialize<PlayerData>(jsonData);
            
    }
    
    public void Save()
    {
        var jsonData = JsonUtility.ToJson(m_myData); //json 형태로 serialize
        Debug.Log(jsonData);
        PlayerPrefs.SetString("PLAYER_DATA", jsonData);
        PlayerPrefs.Save();
        // PlayerPrefs.set // 레지스트리 영역에 저장
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        m_myData = Load();
        
        if(m_myData ==null)
        {
            
            m_myData = new PlayerData()
            {
                goldOwned = PlayerData.BasicGold,
                gemOwned = PlayerData.BasicGem,
                heroIndex = 7

            };
            for (int i = 0; i < TableHero.Instance.m_heroDatas.Length; i++)
            {
                HeroInfo heroinfo = new HeroInfo();
                heroinfo.data = TableHero.Instance.m_heroDatas[i];
                heroinfo.level = 1;
                heroinfo.isPlayable = false;
                m_myData.m_herosList.Add(heroinfo);
            }
            m_myData.m_herosList[m_myData.heroIndex].isPlayable = true;
        }
        Save();

    }

}
