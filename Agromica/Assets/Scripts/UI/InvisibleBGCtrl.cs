using UnityEngine;
using UnityEngine.EventSystems;

// written by user ben-rasooli https://forum.unity.com/threads/how-to-close-a-ui-panel-when-clicking-outside.322565/

public class InvisibleBGCtrl : MonoBehaviour, IPointerClickHandler
{
    MenuVisibilityCtrl _parentCtrl;

    public void setParentCtrl(MenuVisibilityCtrl ctrl)
    {
        _parentCtrl = ctrl;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _parentCtrl.hide();
    }
}
