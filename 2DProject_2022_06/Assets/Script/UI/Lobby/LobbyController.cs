using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    [SerializeField]
    UI2DSprite m_heroSprite;
    [SerializeField]
    TweenPosition m_heroTween;
    [SerializeField]
    UIGrid m_buttonGrid;
    [SerializeField]
    UIButton[] m_buttons;
    [SerializeField]
    GameObject m_menuObj;
    [SerializeField]
    ILobbyMenu[] m_lobbyMenu;
    // Start is called before the first frame update
    public void OnButtonClick(UIButton button)  //클릭됬을때 실행할것
    {
        gameObject.SetActive(false);    //클릭됬을때 꺼짐
        var results = button.name.Split('.');   //.을 기준으로 쪼개기;
        var index = System.Convert.ToInt32(results[0]);
        m_lobbyMenu[index - 1].Open(); //해당 인덱스 gameobject열기
    }
    public void GoNextScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(SceneState.Game);//start 버튼 누르면 gamescene으로
    }
    public void SetLobbyInfo(Sprite herospr)
    { 
        m_heroSprite.sprite2D = herospr;  //로비 이미지를 받아온 sprite로 변경
        m_heroSprite.MakePixelPerfect();
        m_heroSprite.transform.localPosition = TableHero.Instance.m_heroDatas[PlayerDataManager.Instance.Heroindex].Position + Vector2.up * 20f;  //포지션 Tablehero에서 저장해논 값으로 변경
        m_heroTween.from = m_heroSprite.transform.localPosition +Vector3.up* 20f; //tweenposition 도 변경해주기
        m_heroTween.to = m_heroTween.from + Vector3.down * 20f;
    }
    void Start()
    {
        m_buttons = m_buttonGrid.GetComponentsInChildren<UIButton>();   //grid 자식에 있는buttons들 가져옴
        m_lobbyMenu = m_menuObj.GetComponentsInChildren<ILobbyMenu>(true); // true적으면 active가 꺼져도 찾을수 있음
        for(int i=0; i<m_buttons.Length;i++)
        {
            EventDelegate eventDel = new EventDelegate(this, "OnButtonClick");  
            eventDel.parameters[0] = Util.MakeParam(m_buttons[i], typeof(UIButton)); //eventDelegate에 paramter을 보내준다(매개변수)
            m_buttons[i].onClick.Add(eventDel); // 각 버튼에 있는 on-click에 eventDel을 연결해준다.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
