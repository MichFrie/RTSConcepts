using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{

    public LayerMask unitLayerMask;
    private List<Unit> selectedUnits = new List<Unit>();

    private Camera cam;
    private Player player;

    public RectTransform selectionBox;

    private Vector2 startPos;

    

    private void Awake()
    {
        cam = Camera.main;
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            ToggleSelectionVisual(false);
            selectedUnits = new List<Unit>();
            TrySelect(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }
    }


    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach(Unit unit in player.units)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
            if(screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
    }

    void ToggleSelectionVisual(bool selected)
    {
        foreach(Unit unit in selectedUnits)
        {
            unit.ToggleSelectionVisual(selected);
        }
    }

    void TrySelect(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100, unitLayerMask))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (player.IsMyUnit(unit))
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }

    }

    public bool HasUnitsSelected()
    {
        return selectedUnits.Count > 0 ? true : false;
    }

    public Unit[] GetSelectedUnits()
    {
        return selectedUnits.ToArray();
    }
}
