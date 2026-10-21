using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public Queue<Canvas> menus = new Queue<Canvas>();
    public List<Canvas> menusInScene = new List<Canvas>();
    void Start()
    {
        foreach (Canvas canvas in menusInScene)
        {
            menus.Enqueue(canvas);
            Debug.Log("there's " + menus.Count + "menus on the scene");
        }
    }
    void Update()
    {
        
    }
    public void ChangeMenu()
    {

    }
}
