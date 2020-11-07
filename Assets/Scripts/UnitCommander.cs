using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    public GameObject selectionMarkerPrefab;
    public LayerMask layerMask;

    private UnitSelection unitSelection;
    private Camera cam;

    void Awake()
    {
        unitSelection = GetComponent<UnitSelection>();
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && unitSelection.HasUnitsSelected())
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Unit[] selectedUnits = unitSelection.GetSelectedUnits();

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {

                if (hit.collider.CompareTag("Ground"))
                {

                    UnitsMoveToPosition(hit.point, selectedUnits);
                    CreateSelectionMarker(hit.point, false);

                }

                else if (hit.collider.CompareTag("Resource"))
                {
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), selectedUnits);
                    CreateSelectionMarker(hit.point, true);
                }
            }
        }
    }

    void UnitsMoveToPosition(Vector3 movePos, Unit[] units)
    {
        Vector3[] destinations = UnitMover.GetUnitGroupDestinations(movePos, units.Length, 2);
        for (int i = 0; i < units.Length; i++)
        {
            units[i].MoveToPosition(destinations[i]);
        }
    }

    void CreateSelectionMarker(Vector3 pos, bool large)
    {
        GameObject marker = Instantiate(selectionMarkerPrefab, new Vector3(pos.x, 0.01f, pos.z), Quaternion.identity);
        if (large)
            marker.transform.localScale = Vector3.one * 3;
    }

    void UnitsGatherResource(ResourceSource resource, Unit[] units)
    {

    }
}
