using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class InGameUIManager : SingletonMonobehaviour<InGameUIManager>
{
    StringBuilder m_sb = new StringBuilder();   //string builder
    [SerializeField]
    UILabel m_distScoreLable;
    [SerializeField]
    UILabel m_huntScoreLabel;
    [SerializeField]
    UILabel m_coinOwnedLabel;
    uint m_distScore;
    uint m_huntScore;
    uint m_coinOwned;
    // Start is called before the first frame update

    public uint DistScore { get { return m_distScore; } } 
    public uint HuntScore { get { return m_huntScore; } }
    public uint GoldCount { get { return m_coinOwned; } }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetDistScore(float dist)
    {
        m_distScore = (uint)Mathf.CeilToInt(dist * 100f);
        //String 보다는String Builder 사용하기
        m_sb.AppendFormat("{0:n0}", m_distScore);
        m_distScoreLable.text = m_sb.ToString();
        m_sb.Clear();   //다시 지워주기
    }
    public void SetHuntScore(uint score)
    {
        m_huntScore += score;
        m_sb.AppendFormat("{0:n0}", m_huntScore);
        m_huntScoreLabel.text = m_sb.ToString();
        m_sb.Clear();   //다시 지워주기
    }
    public void SetCoinOwnedCount(uint coin)
    {
        m_coinOwned += coin;
        m_sb.AppendFormat("{0:n0}", m_coinOwned);
        m_coinOwnedLabel.text = m_sb.ToString();
        m_sb.Clear();   //다시 지워주기
    }
    protected override void OnStart()
    {
        m_distScore = 0;
        SetDistScore(0f);
        m_huntScore = 0;
        SetHuntScore(0);
        m_coinOwned = 0;
        SetCoinOwnedCount(0);
    }

    
}
