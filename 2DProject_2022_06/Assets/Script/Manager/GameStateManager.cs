using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None =-1,
    Normal,
    Invincible,
    Result,
    Max
}
public class GameStateManager : SingletonMonobehaviour<GameStateManager>
{
    GameState m_state;
    [SerializeField]
    PlayerControl m_player;
    [SerializeField]
    BgController m_bgCtr;
    [SerializeField]
    Result m_result;
    
    
    public GameState State { get { return m_state; } }
    
    
    public void SetState(GameState state, float duration = 0f)
    {
        if(m_state == state)
        {
           
            return;
        }
        m_state = state;
        switch (m_state)
        {
            case GameState.Normal:
                m_player.SetPlayer(false);
                m_bgCtr.SetSpeed(1f);
                MonsterManager.Instance.ResetCreateMonsters(1f);
                break;
            case GameState.Invincible:
                
                m_player.SetPlayer(true);
                m_bgCtr.SetSpeed(5f);
                MonsterManager.Instance.ResetCreateMonsters(5f);//몬스터 속도 증가
                break;
            case GameState.Result:
                MonsterManager.Instance.StopCreateMonster();
                m_player.SetDie();
                InGameUIManager.Instance.Hide();
                m_result.SetResult();
                break;
        }


    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
    }

   
}
