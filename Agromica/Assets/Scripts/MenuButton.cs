using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    GameObject _myMenu;

    void Start() {}

    void Update() {}

    void Awake()
    {
        _myMenu = gameObject;
    }

    public void Toggle()
    {
        bool action = !_myMenu.activeSelf; // _myMenu.activeInHierarchy also exists
        _myMenu.SetActive(action);
    }
}
