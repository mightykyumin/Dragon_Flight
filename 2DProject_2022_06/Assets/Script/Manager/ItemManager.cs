using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonobehaviour<ItemManager>
{
    public enum ItemType
    {
        Coin,
        Gem_Red,
        Gem_Green,
        Gem_Blue,
        Invincible,
        Magnet,
        Max


    }
    [SerializeField]
    Sprite[] m_iconSprites;
    [SerializeField]
    PlayerControl m_Player;
    [SerializeField]
    GameObject m_itemPrefab;
    GameObjectPool<ItemController> m_itemPool;
    float m_maxDuration = 2f;
    float m_maxDistance = 13.3f;
    public float m_endPosY = -7.7f;
    float m_maxMoveX = 1f;
    int[] m_itemTable = { 60, 3, 2, 1, 31, 3 };  // 아이템 확률
    public void CreateItem(Vector3 pos)
    {
        ItemType type;
        bool isTry;
        do
        {
            isTry = false;

            type = (ItemType)Util.GetPriority(m_itemTable);  //item 랜덤으로 가져오기

            if (GameStateManager.Instance.State == GameState.Invincible)
            {
                if (type == ItemType.Invincible)
                    isTry = true;
            }
        } while (isTry);






        var item = m_itemPool.Get();
        /*Vector3 from = pos;
        var dir = m_Player.transform.position - pos;
        dir.y = 0f;
        var value = dir.magnitude;
        if (value > 1f)
            value = m_maxMoveX;
        Vector3 to = new Vector3(pos.x, m_endPosY) + dir.normalized * value;*/
        Vector3 from, to;
        float duration;
        ResetItemPos(pos, out from, out to, out duration);
        item.SetItem(from, to, duration, type);
        //item.gameObject.SetActive(true);
        //item.StartMove(from, to,m_maxDuration* ((to-from).magnitude / m_maxDistance));


    }
    public void ResetItemPos(Vector3 target, out Vector3 from, out Vector3 to, out float duration)
    {
        from = target;
        var dir = m_Player.transform.position - target;
        dir.y = 0f;
        var value = dir.magnitude;
        if (value > 1f)
            value = m_maxMoveX;
        to = new Vector3(target.x, m_endPosY) + dir.normalized * value;
        duration = m_maxDuration * ((to - from).magnitude / m_maxDistance);
    }
    public void RemoveItem(ItemController item)
    {
        item.gameObject.SetActive(false);
        m_itemPool.Set(item);
    }
    public Sprite GetIconSprite(ItemType type)
    {
        return m_iconSprites[(int)type];
    }
    protected override void OnStart()
    {
        m_itemPool = new GameObjectPool<ItemController>(10, () =>       // item 10개 풀로 넣기
        {
            var obj = Instantiate(m_itemPrefab);        // item prefab 받아오기
            obj.SetActive(false);                       // item 처음에 꺼놓기
            obj.transform.SetParent(transform);         // 부모 지정?
            obj.transform.localPosition = Vector3.zero; // 위치 초기화
            obj.transform.localScale = Vector3.one;     // 크기 초기화
            var item = obj.GetComponent<ItemController>();  //item controller 가져오기
            item.InitItem(m_Player);
            return item;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
