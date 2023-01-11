using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
{
    public delegate T CreateFunc();
    Queue<T> m_memoryPool = new Queue<T>();
    int m_count;
    CreateFunc m_createFuncDel;
    public GameObjectPool(int count, CreateFunc delegateFunc)
    {
        m_count = count;
        m_createFuncDel = delegateFunc;
        Allocation();
    }
    void Allocation()
    {
        for(int i=0; i< m_count;i ++)
        {
            m_memoryPool.Enqueue(m_createFuncDel()); 
        }
    }
    public T Get()
    {
        if(m_memoryPool.Count >0)
        {
            return m_memoryPool.Dequeue();
        }
        return m_createFuncDel();
    }
    public void Set(T obj)
    {
        m_memoryPool.Enqueue(obj);
    }
    
}
