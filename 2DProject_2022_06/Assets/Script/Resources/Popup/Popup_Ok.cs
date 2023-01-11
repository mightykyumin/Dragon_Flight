using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Ok : MonoBehaviour
{
    [SerializeField]
    UILabel m_title;
    [SerializeField]
    UILabel m_body;
    [SerializeField]
    UILabel m_okBtnText;
    ButtonDelegate m_okBtnDel;
    
    public void SetUI(string title, string body, ButtonDelegate okBtnDel,  string okBtnText = "Ok")
    {
        m_title.text = title;
        m_body.text = body;
        m_okBtnText.text = okBtnText;
        
        m_okBtnDel = okBtnDel;
        
    }
    public void OnPressOk()
    {
        if (m_okBtnDel != null)
        {
            m_okBtnDel();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }
    


    void Start()
    {

    }
}
