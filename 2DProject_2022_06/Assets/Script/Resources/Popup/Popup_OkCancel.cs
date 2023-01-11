using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_OkCancel : MonoBehaviour
{
    [SerializeField]
    UILabel m_title;
    [SerializeField]
    UILabel m_body;
    [SerializeField]
    UILabel m_okBtnText;
    [SerializeField]
    UILabel m_cancelBtnText;
    ButtonDelegate m_okBtnDel;
    ButtonDelegate m_cancelDel;
    public void SetUI(string title, string body,ButtonDelegate okBtnDel, ButtonDelegate cancelBtnDel=null, string okBtnText ="Ok", string cancelBtnTExt = "Cancel")
    {
        m_title.text = title;
        m_body.text = body;
        m_okBtnText.text = okBtnText;
        m_cancelBtnText.text = cancelBtnTExt;
        m_okBtnDel = okBtnDel;
        m_cancelDel = cancelBtnDel;
    }
    public void OnPressOk()
    {
        if(m_okBtnDel != null)
        {
            m_okBtnDel();
        }
        else
        {
            
            PopupManager.Instance.ClosePopup();
        }

        
    }
    public void OnPressCancel()
    {
        
        if (m_cancelDel != null)
        {
            m_cancelDel();
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
