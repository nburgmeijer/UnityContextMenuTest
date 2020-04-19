using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum ItemType
    {
        mainItem,
        subItem
    }

    private ContextMenuItem _mainMenuContextMenuItem;
    private Image _itemBackground;
    private ColorBlock _colorBlock;
    private ContextMenuItem _itemHit = null;
    private GraphicRaycaster _graphicRaycaster;
    private List<RaycastResult> _raycastResults = new List<RaycastResult>();

    [SerializeField]
    private GameObject _subMenu;
    [SerializeField]
    private GameObject _mainMenuItem;
    [SerializeField]
    private ItemType _itemType;

    public static event EventHandler OnClickItem;

    void Start()
    {
        _colorBlock = this.GetComponent<Button>().colors;
        if(_mainMenuItem != null)
        {
            _mainMenuContextMenuItem = _mainMenuItem.GetComponent<ContextMenuItem>();
        }
        
        _graphicRaycaster = this.GetComponentInParent<GraphicRaycaster>();
        _itemBackground = this.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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
            //If we exit from the mainmenu while a submenu was open, close it
            if (_subMenu != null)
            {
                _subMenu.SetActive(false);
            }
            //if we exit from a submenu, close it
            if (_itemType == ItemType.subItem)
            {
                    transform.parent.gameObject.SetActive(false);
                    _mainMenuItem.GetComponent<Button>().interactable = true;
            }
        }

        //The mouspointer exited to another item
        else
        {
            //Pointer went to a mainItem
            if(_itemHit._itemType == ItemType.mainItem)
            {
                //From mainItem to another mainItem
                if(_itemType == ItemType.mainItem)
                {
                    if(_subMenu != null) 
                    { 
                        _subMenu.SetActive(false); 
                    }
                }
                //from subItem to mainItem
                if(_itemType == ItemType.subItem)
                {
                    transform.parent.gameObject.SetActive(false);
                    _mainMenuItem.GetComponent<Button>().interactable = true;
                }
                               
            }
            if(_itemHit._itemType == ItemType.subItem && _subMenu != null)
            {
                GetComponent<Button>().interactable = false;
            }
            _itemHit = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (OnClickItem != null)
        {
            OnClickItem(gameObject, EventArgs.Empty);
        }
        
    }
    void Update()
    {

      
    }
}
