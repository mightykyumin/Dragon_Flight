using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu_Clan : MonoBehaviour,ILobbyMenu
{
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
