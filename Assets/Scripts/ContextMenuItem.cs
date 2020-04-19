using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ItemType
    {
        mainItem,
        subItem
    }

    private Image _itemBackground;
    private ContextMenuItem _itemHit = null;
    private GraphicRaycaster _graphicRaycaster;
    private List<RaycastResult> _raycastResults = new List<RaycastResult>();

    [SerializeField]
    private GameObject _subMenu;
    [SerializeField]
    private GameObject _mainMenuItem;
    [SerializeField]
    private ItemType _itemType;

    void Start()
    {
        _graphicRaycaster = this.GetComponentInParent<GraphicRaycaster>();
        _itemBackground = this.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    { 
        //when entering any item we set the selectcolor
        _itemBackground.color = new Color(1f, 0.5f, 0.5f, 0.6f);
        //if the mousepointer enters a menuItem that has a subMenu attached to it, open it
        if (_subMenu != null)
        {
            _subMenu.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //let's raycast to see if we went from item to another item.
        _graphicRaycaster.Raycast(eventData, _raycastResults);
        foreach (RaycastResult raycastResult in _raycastResults)
        {
            _itemHit = raycastResult.gameObject.GetComponent<ContextMenuItem>();
        }
        _raycastResults.Clear();
        //If the mousepointer is outside a menu
        if (_itemHit == null)
        {
            //always set the color to deselectcolor
            _itemBackground.color = new Color(1f, 1f, 1f, 1f);
            //If we exit from the mainmenu while a submenu was open, close it
            if (_subMenu != null)
            {
                _subMenu.SetActive(false);
            }
            //if we exit from a submenu, close it and set the associated MainMenuItem deselectcolor
            if (_itemType == ItemType.subItem)
            {
                if(_mainMenuItem.GetComponent<ContextMenuItem>() != null)
                {
                    _mainMenuItem.GetComponent<ContextMenuItem>()._itemBackground.color = new Color(1f, 1f, 1f, 1f);
                    gameObject.transform.parent.gameObject.SetActive(false);
                }        
            }
        }
        //The mouspointer exited to another item
        else
        {
            //if the next item that will be entered is a mainMenuItem or the current item we exit is a subMenuItem
            if (_itemHit._itemType == ItemType.mainItem || _itemType == ItemType.subItem)
            {
                //set deselectcolor of the item we're exiting
                _itemBackground.color = new Color(1f, 1f, 1f, 1f);
                //if the mousepointer exited from a mainItem with a subMenu, close the submenu
                if(_subMenu != null)
                {
                    _subMenu.SetActive(false);
                }
            }
            //if the mousepointer exits from a subMenuItem and enters a mainItem, close the subMenu
            if(_itemType == ItemType.subItem && _itemHit._itemType == ItemType.mainItem)
            {
                _itemBackground.color = new Color(1f, 1f, 1f, 1f);
                if(_mainMenuItem.GetComponent<ContextMenuItem>() != null)
                {
                _mainMenuItem.GetComponent<ContextMenuItem>()._itemBackground.color = new Color(1f, 1f, 1f, 1f);
                gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
            _itemHit = null;
        }
    }
}
