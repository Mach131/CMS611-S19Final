using UnityEngine;
using UnityEngine.UI;

// written by user ben-rasooli https://forum.unity.com/threads/how-to-close-a-ui-panel-when-clicking-outside.322565/

public class MenuVisibilityCtrl : MonoBehaviour
{
    [SerializeField] bool shouldStartVisible;

    GameObject _myGameObj;
    GameObject _invisibleBG;

    void Awake()
    {
        setupInvisibleBG();
        _myGameObj = gameObject;
        if (!shouldStartVisible)
            hide();
    }

    void setupInvisibleBG()
    {
        // Instantiate new object.
        _invisibleBG = new GameObject("InvisibleBG");

        InvisibleBGCtrl tempInvisibleBGCtrl = _invisibleBG.AddComponent<InvisibleBGCtrl>();
        tempInvisibleBGCtrl.setParentCtrl(this);

        Image tempImage = _invisibleBG.AddComponent<Image>(); // need an image to register clicks
        tempImage.color = new Color(1f, 1f, 1f, 0f); // alpha is zero, so it's transparent

        // Make it fill the screen and then set it as the child to the attached object.
        RectTransform tempTransform = _invisibleBG.GetComponent<RectTransform>();
        tempTransform.anchorMin = new Vector2(0f, 0f);
        tempTransform.anchorMax = new Vector2(1f, 1f);
        tempTransform.offsetMin = new Vector2(0f, 0f);
        tempTransform.offsetMax = new Vector2(0f, 0f);
        tempTransform.SetParent(GetComponentsInParent<Transform>()[1], false); // worldpositionstays false maintains fullscreen size
        tempTransform.SetSiblingIndex(transform.GetSiblingIndex()); // put it immediately beind this panel in the hierarchy
    }

    void OnEnable()
    {
        _invisibleBG.SetActive(true); // Activate whenever parent is active.
    }

    // Called when this object is clicked.
    public void hide()
    {
        _myGameObj.SetActive(false);
        _invisibleBG.SetActive(false);
    }
}
