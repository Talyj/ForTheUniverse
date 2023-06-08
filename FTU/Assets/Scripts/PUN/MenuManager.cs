using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance;
    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine("RoomOpen");
    }

    IEnumerator RoomOpen()
    {
        yield return new WaitForSeconds(.5f);
        OpenMenu("room");
    }
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open(); 
    }
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
