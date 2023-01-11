using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleControler : MonoBehaviour
{

    // UI 직접 만들기
    [SerializeField]
    BgController m_bgCtr;
    [SerializeField]
    GameObject titleObj;
    


    // Start is called before the first frame update
    public void GoNextScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(SceneState.Lobby);

    }
    public void SetTitle()
    {
        titleObj.SetActive(true);
        m_bgCtr.gameObject.SetActive(true);
        LoadSceneManager.Instance.SetState(SceneState.Title);
    }
    void Start()
    {
        titleObj.SetActive(false);
        m_bgCtr.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) return;
            if (UICamera.touchCount > 0) return;
            GoNextScene();
        }
    }
}
