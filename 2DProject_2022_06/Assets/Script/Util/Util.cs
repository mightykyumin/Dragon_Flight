using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util 
{
    public static EventDelegate.Parameter MakeParam(Object obj, System.Type type)
    {
        EventDelegate.Parameter param = new EventDelegate.Parameter();
        param.obj = obj;
        param.expectedType = type;   //어떤 타입인지 저장
        return param;

    }
   public static int GetPriority(int[] table)       // 아이템 확률 control
    {
        if (table == null || table.Length == 0) return -1;
        int sum = 0;
        int num = 0;
        for(int i =0; i<table.Length; i++)
        {
            sum += table[i];
        }
        
        num = Random.Range(0, 100) + 1;
        sum = 0;
        for(int i=0; i<table.Length; i++)   //
        {
            if (num > sum && num <= sum + table[i])     // 이전확률 전부 더한거 < Random 숫자 < 이전확률 전부 더한것 + 현재 인덱스의 확률 이면 현재 인덱스 값 return
                return i;
            sum += table[i];
        }
        return -1;

    }
}
