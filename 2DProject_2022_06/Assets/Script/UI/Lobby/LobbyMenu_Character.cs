using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu_Character : MonoBehaviour, ILobbyMenu
{
    [SerializeField]
    LobbyController m_lobby;
    [SerializeField]
    UI2DSprite m_heroSprite;
    [SerializeField]
    TweenPosition m_heroTween;
    [SerializeField]
    UIButton m_selectBtn;
    [SerializeField]
    UIButton m_buyBtn;
    int m_selectIndex;
    public Sprite LoadHeroSprite()
    {
        return  Resources.Load<Sprite>(string.Format("Character/character_{0:00}", m_selectIndex + 1));    //Resource에서 character sprite받아오기
    }
    public void SetHero()
    {
        m_heroSprite.sprite2D = LoadHeroSprite();       //sprite받아오기
        m_heroSprite.MakePixelPerfect();
        m_heroSprite.transform.localPosition = TableHero.Instance.m_heroDatas[m_selectIndex].Position;  //포지션 Tablehero에서 저장해논 값으로 변경
        m_heroTween.from = m_heroSprite.transform.localPosition; //tweenposition 도 변경해주기
        m_heroTween.to = m_heroTween.from + Vector3.down * 20f;

        ResetInfo();
    }
    public void SelectHero()
    {
        PlayerDataManager.Instance.Heroindex = (byte)m_selectIndex;
        PlayerDataManager.Instance.Save();
        m_lobby.gameObject.SetActive(true);
        m_lobby.SetLobbyInfo(m_heroSprite.sprite2D);
        Close();
    }

    public void BuyHero()
    {
        
        var data = TableHero.Instance.m_heroDatas[m_selectIndex];
        
        PopupManager.Instance.OpenPopup_OkCancel("구입 안내", string.Format("[000000][ffffff]{0}[-][00ff00]Gem[-]을 소모하여 [0000ff]{1} [00ffff] 캐릭터[-]를\r\n 구매하시겠습니까?[-]", data.price, data.name), ()=>
         {
             if (PlayerDataManager.Instance.DecreaseGem((uint)data.price))
             {
                 PlayerDataManager.Instance.SetPlayableHero(m_selectIndex);
                 PopupManager.Instance.ClosePopup();
                 ResetInfo();
                 PlayerDataManager.Instance.Save();
             }
             else
             {
                 PopupManager.Instance.OpenPopup_Ok("NOTICE", "소지한 Gem이 부족합니다.");
             }
         },null,"구입","취소");
        
        //PopupManager.Instance.OpenPopup_OkCancel("구입 안내", string.Format("[000000][ffffff]{0}[-][00ff00]Gem[-]을 소모하여 [0000ff]{1} [00ffff] 캐릭터[-]를\r\n 구매하시겠습니까?[-]", data.price, data.name), null, null, "OK", "Cancel");
    }
    public void OnPressLeft()
    {
        m_selectIndex--;
        if (m_selectIndex < 0)
            m_selectIndex = TableHero.Instance.m_heroDatas.Length - 1;
        SetHero();
    }
    public void OnPressRight()
    {
        m_selectIndex++;
        if (m_selectIndex > TableHero.Instance.m_heroDatas.Length - 1)
            m_selectIndex = 0;
        SetHero();
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }    
    void ResetInfo()
    {
        if(PlayerDataManager.Instance.IsPlayable(m_selectIndex))    //구입한 항목이면
        {
            m_heroSprite.depth = 1;     //depth를 1로
            m_selectBtn.gameObject.SetActive(true); // select버튼 켜주기
            m_buyBtn.gameObject.SetActive(false);
        }
        else
        {
            m_heroSprite.depth = -1;    //depth를 -1로 해서 blurry하게
            m_selectBtn.gameObject.SetActive(false);
            m_buyBtn.gameObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_selectIndex = PlayerDataManager.Instance.Heroindex;
        SetHero();
        m_lobby.SetLobbyInfo(m_heroSprite.sprite2D);
        Close();
    }
}
