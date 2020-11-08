using System.Reflection;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    public static Vector3[] GetUnitGroupDestinations(Vector3 moveToPos, int numUnits, float unitGap)
    {
        // vector3 array for final destinations
        Vector3[] destinations = new Vector3[numUnits];

        // calculate the rows and columns
        int rows = Mathf.RoundToInt(Mathf.Sqrt(numUnits));
        int cols = Mathf.CeilToInt((float)numUnits / (float)rows);
        // we need to know the current row and column we're calculating
        int curRow = 0;
        int curCol = 0;
        float width = ((float)rows - 1) * unitGap;
        float length = ((float)cols - 1) * unitGap;
        for (int x = 0; x < numUnits; x++)
        {
            destinations[x] = moveToPos + (new Vector3(curRow, 0, curCol) * unitGap) - new Vector3(length / 2, 0, width / 2);
            curCol++;
            if (curCol == rows)
            {
                curCol = 0;
                curRow++;
            }
        }
        return destinations;
    }

    public static Vector3[] GetUnitGroupDestionationsAroundResource(Vector3 resourcePos, int unitsNum)
    {
        Vector3[] destinations = new Vector3[unitsNum];
        float unitDistanceGap = 360.0f / (float) unitsNum;

        for (int i = 0; i < unitsNum; i++)
        {
            float angle = unitDistanceGap * i;
            Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            destinations[i] = resourcePos + dir;
        }
        return destinations;
    }

    public static Vector3 GetUnitDestinationAroundResource(Vector3 resourcePos)
    {
        float angle = Random.Range(0, 360);
        Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        return resourcePos;
    }
}
