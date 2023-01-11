using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]

public class HeroData
{
    //주인공아 갖는 정보
    public string className;
    public string name;
    public int price;
    public Vector2 Position;
}
[Serializable]
public class HeroInfo
{
    public HeroData data;
    public bool isPlayable;
    public int level;
}
[Serializable]
public class PlayerData
{
    public readonly static uint BasicGold=2000;
    public readonly static uint BasicGem = 100;
    public uint goldOwned;
    public uint gemOwned;
    public uint bestRecord;
    public byte heroIndex;
    public List<HeroInfo> m_herosList = new List<HeroInfo>();
}
