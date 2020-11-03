using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Units")]
    public List<Unit> units = new List<Unit>();


    public bool IsMyUnit(Unit unit)
    {
        return units.Contains(unit);
    }

}
