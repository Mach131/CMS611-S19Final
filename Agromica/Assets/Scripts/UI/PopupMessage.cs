using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    void Awake()
    {
        this.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void OnDisable()
    {
        Destroy(this.gameObject, 1.0f);
    }
}
