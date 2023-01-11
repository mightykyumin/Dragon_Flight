using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField]
    GameObject m_bestObj;
    [SerializeField]
    UI2DSprite m_sdCharacter;
    [SerializeField]
    UILabel m_totalScoreLabel;
    [SerializeField]
    UILabel m_distScoreLabel;
    [SerializeField]
    UILabel m_huntScoreLabel;
    [SerializeField]
    UILabel m_goldOwnedLabel;
    [SerializeField]
    UILabel m_bestScoreLabel;
    // Start is called before the first frame update
    public void GoLobbyScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(SceneState.Lobby);
    }
    public void SetResult()
    {

        bool isBest = false;
        m_bestObj.SetActive(false);
        gameObject.SetActive(true);
        int totalScore = (int)(InGameUIManager.Instance.DistScore + InGameUIManager.Instance.HuntScore);    //InGameUIManager에서 점수 받아오기\
        m_totalScoreLabel.text = string.Format("{0:n0}", totalScore);
        m_distScoreLabel.text = string.Format("{0:n0}",InGameUIManager.Instance.DistScore);
        m_huntScoreLabel.text = string.Format("{0:n0}", InGameUIManager.Instance.HuntScore);
        m_goldOwnedLabel.text = string.Format("{0:n0}", InGameUIManager.Instance.GoldCount);
        if (totalScore > PlayerDataManager.Instance.BestRecord)
            isBest = true;
        
        if (isBest)
            m_bestScoreLabel.text = string.Format("{0:n0}", totalScore);
        else
            m_bestScoreLabel.text = string.Format("{0:n0}", (int)PlayerDataManager.Instance.BestRecord);
        if (isBest)
        {
            PlayerDataManager.Instance.BestRecord = (uint)totalScore;
            m_bestObj.SetActive(true);
        }
        m_sdCharacter.sprite2D= Resources.Load<Sprite>(string.Format("SD/sd_{0:00}{1}", PlayerDataManager.Instance.Heroindex + 1, isBest ? "_highscore" : string.Empty));
        PlayerDataManager.Instance.IncreaseGold(InGameUIManager.Instance.GoldCount);
        PlayerDataManager.Instance.Save();
       
        //m_bestScoreLabel.text = string.Format("{0:n0}", InGameUIManager.Instance.GoldCount);


    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    
}
