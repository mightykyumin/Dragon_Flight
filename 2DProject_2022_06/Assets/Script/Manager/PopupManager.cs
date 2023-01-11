using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void ButtonDelegate();

public class PopupManager : DontDestroy<PopupManager>
{
    [SerializeField]
    GameObject m_popupOkCancelPrefab;
    [SerializeField]
    GameObject m_popupOkPrefab;


    List<GameObject> M_popupList = new List<GameObject>();
    public bool IsOpen { get { return M_popupList.Count > 0; } }
    int m_popupDepth = 1000;
    int m_popupDepthGap = 10;
    public void OpenPopup_OkCancel(string title, string body, ButtonDelegate okBtnDel= null, ButtonDelegate cancelBtnDel =null, string okBtnText = "OK", string cancelbtn = "Cancel")
    {
        
        var obj = Instantiate(m_popupOkCancelPrefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        var popup = obj.GetComponent<Popup_OkCancel>();
        popup.SetUI(title, body, okBtnDel, cancelBtnDel, okBtnText, cancelbtn);
        var panels = obj.GetComponentsInChildren<UIPanel>();

        for(int i=0; i< panels.Length; i++)
        {
            panels[i].depth = m_popupDepth + M_popupList.Count * m_popupDepthGap + i;
            
        }
        M_popupList.Add(obj);
    }
    public void OpenPopup_Ok(string title, string body, ButtonDelegate okBtnDel = null, string okBtnText = "OK")
    {
        var obj = Instantiate(m_popupOkPrefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        var popup = obj.GetComponent<Popup_Ok>();
        popup.SetUI(title, body, okBtnDel,  okBtnText);
        var panels = obj.GetComponentsInChildren<UIPanel>();

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = m_popupDepth + M_popupList.Count * m_popupDepthGap + i;
            Debug.Log(M_popupList.Count);
        }
        M_popupList.Add(obj);
    }
    public void ClosePopup()
    {
        
        if (M_popupList.Count>0)
        {
            Destroy(M_popupList[M_popupList.Count - 1].gameObject);
            
            M_popupList.RemoveAt(M_popupList.Count - 1);

        }
    }
        

    
    protected override void OnStart()
    {
        m_popupOkCancelPrefab = Resources.Load<GameObject>("Prefab/Ok_Cancel");
        m_popupOkPrefab = Resources.Load<GameObject>("Prefab/Popup_OK");

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OpenPopup_OkCancel("NOTICE", "팝업테스트",null, null ,"확인", "종료");
        }
    }
}
