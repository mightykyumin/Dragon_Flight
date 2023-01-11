using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public enum SceneState
{
    None = -2,
    CI,
    Title,
    Lobby,
    Game,

}
public class LoadSceneManager : DontDestroy<LoadSceneManager>
{
    
    
    AsyncOperation m_loadingInfo;

    [SerializeField]
    UIPanel m_loadingPanel;
    [SerializeField]
    UIProgressBar m_progressBar;
    [SerializeField]
    UILabel m_progressLabel;
    SceneState m_state = SceneState.CI;
    SceneState m_loadState = SceneState.None;
    public  SceneState GetSceneState { get { return m_state; } }

    public void SetState(SceneState state)
    {
        m_state = state;
    }
    public void LoadSceneAsync(SceneState scene)
    {
        if (m_loadState != SceneState.None) return;
        m_loadState = scene;
        Show();
        m_loadingInfo = SceneManager.LoadSceneAsync((int)scene);
    }
    protected override void OnAwake()
    {
        
    }
    // Start is called before the first frame update
    void Hide()
    {
        m_loadingPanel.gameObject.SetActive(false);
    }
    void Show()
    {
        m_loadingPanel.gameObject.SetActive(true);
    }
    protected override void OnStart()
    {
        Hide(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PopupManager.Instance.IsOpen)
            {
                PopupManager.Instance.ClosePopup();
            }
            else
            {   
                switch(m_state)
                {
                    case SceneState.Title:
                        PopupManager.Instance.OpenPopup_OkCancel("안내", "정말로 게임을 종료하시겠습니까?", () =>
                        {
#if UNITY_EDITOR            
                            EditorApplication.isPlaying = false;
                            
#else
                            Application.Quit();
#endif
                        }, null, "yes", "No");
                        break;
                    case SceneState.Lobby:
                        PopupManager.Instance.OpenPopup_OkCancel("안내", "타이틀화면으로로 이동하시겠습니까?", () =>
                        {
                            LoadSceneAsync(SceneState.Title);
                            PopupManager.Instance.ClosePopup();
                        }, null, "yes", "No");
                        break;
                    case SceneState.Game:
                        PopupManager.Instance.OpenPopup_OkCancel("안내", "타이틀화면으로로 이동하시겠습니까?", () =>
                        {
                            LoadSceneAsync(SceneState.Lobby);
                            PopupManager.Instance.ClosePopup();
                        }, null, "yes", "No");
                        break;
                         

                }
                //PopupManager.Instance.OpenPopup_OkCancel("안내", "")
            }
        }
        if(m_loadingInfo !=null && m_loadState != SceneState.None)
        {
            if(m_loadingInfo.isDone)
            {
                Invoke("Hide", 1f);
                
                m_loadingInfo = null;
                m_progressLabel.text = "100%";
                m_progressBar.value = 1f;
                m_state = m_loadState;
                m_loadState = SceneState.None;
            }
            else
            {
                m_progressLabel.text = Mathf.CeilToInt(m_loadingInfo.progress * 100) + "%";
                m_progressBar.value = m_loadingInfo.progress;
            }
        }
    }
}
