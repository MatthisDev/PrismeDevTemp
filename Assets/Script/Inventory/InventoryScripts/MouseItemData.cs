using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseItemData : MonoBehaviour // item de la souris
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;
    private Transform _playerTransform;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;
        ItemCount.text = "";

        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (_playerTransform == null) Debug.Log("Player not found");
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }
    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemData.Icon;
        ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    private void Update()
    {
        if (AssignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                if(AssignedInventorySlot.ItemData.ItemPrefab != null) Instantiate(AssignedInventorySlot.ItemData.ItemPrefab, _playerTransform.position + _playerTransform.forward * 2f, Quaternion.identity);

                if (AssignedInventorySlot.StackSize >1)
                {
                    AssignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }
                else
                {
                    ClearSlot();
                }
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
        return results.Count > 0;
    }
}