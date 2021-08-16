using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton單例模式
//可以確保生成對象只有一個實例對象存在
//開發遊戲會希望某一個類別只有一個實例化物件就可以使用
public class GameManagerSingleton 
{
    private GameObject gameObject;

    //單例
    private static GameManagerSingleton m_Instance;
    //接口
    public static GameManagerSingleton Instance
    {
        get
        {
            //如果有就返回Instance
            //如果沒有就創立新的返回
            if (m_Instance == null)
            {
                m_Instance = new GameManagerSingleton();
                m_Instance.gameObject = new GameObject("GameManager");
                m_Instance.gameObject.AddComponent<InputController>();
            }
            return m_Instance;
        }      
    }

    //登記InputController(一個遊戲只會有一個)
    private InputController m_InputController;
    public InputController InputController
    {
        get
        {
            if (m_InputController == null)
            {
                m_InputController = gameObject.GetComponent<InputController>();
            }
            return m_InputController;
        }
    }
}
