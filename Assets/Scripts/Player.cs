using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Units")]
    public List<Unit> units = new List<Unit>();

    [Header("Resources")]
    public int food;

    [Header("Components")]
    public GameObject unitPrefab;
    public Transform unitSpawnPos;

    public readonly int unitCost = 50;

    //events
    [System.Serializable]
    public class UnityCreatedEvent : UnityEvent<Unit> { }
    public UnityCreatedEvent onUnitCreated;

    void Start()
    {
        GameUi.instance.UpdateUnitCountText(units.Count);
        GameUi.instance.UpdateFoodText(food);
    }


    public bool IsMyUnit(Unit unit)
    {
        return units.Contains(unit);
    }

    public void GainResource(ResourceType resourceType, int amount)
    {
        switch (resourceType)
        {
            case ResourceType.Food:
                {
                    food += amount;
                    GameUi.instance.UpdateFoodText(food);
                    break;
                }
        }
    }

    public void CreateNewUnit()
    {
        if (food - unitCost < 0)
            return;
        GameObject unitObj = Instantiate(unitPrefab, unitSpawnPos.position, Quaternion.identity, transform);
        Unit unit = unitObj.GetComponent<Unit>();

        units.Add(unit);
        unit.player = this;
        food -= unitCost;

        if (onUnitCreated != null)
            onUnitCreated.Invoke(unit);

        GameUi.instance.UpdateUnitCountText(units.Count);
        GameUi.instance.UpdateFoodText(food);
    }
}
