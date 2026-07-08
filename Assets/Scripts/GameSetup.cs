using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HexGrid grid;
    [SerializeField] private TurnManager turnManager;

    [Header("Unit Prefabs")]
    [SerializeField] private GameObject unitPrefab; // Base prefab with Unit.cs + UnitVisual.cs

    [Header("Starting Units")]
    [SerializeField] private List<UnitTemplate> playerUnits = new List<UnitTemplate>();
    [SerializeField] private List<UnitTemplate> enemyUnits = new List<UnitTemplate>();

    private void Start()
    {
        if (grid == null)
        {
            grid = FindObjectOfType<HexGrid>();
        }
        if (turnManager == null)
        {
            turnManager = FindObjectOfType<TurnManager>();
        }

        SetupBattle();
    }

    private void SetupBattle()
    {
        if (unitPrefab == null)
        {
            Debug.LogError("Unit prefab not assigned in GameSetup!");
            return;
        }

        // If no templates configured, use default setup
        if (playerUnits.Count == 0 && enemyUnits.Count == 0)
        {
            SetupDefaultBattle();
            return;
        }

        // Spawn player units on the left side
        for (int i = 0; i < playerUnits.Count; i++)
        {
            HexCoord coord = new HexCoord(1, 1 + i * 2);
            if (grid.IsValidCoord(coord))
            {
                SpawnUnit(playerUnits[i], coord, false);
            }
        }

        // Spawn enemy units on the right side
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            HexCoord coord = new HexCoord(grid.Width - 2, 1 + i * 2);
            if (grid.IsValidCoord(coord))
            {
                SpawnUnit(enemyUnits[i], coord, true);
            }
        }
    }

    private void SetupDefaultBattle()
    {
        // Player: David + 2 Scouts
        SpawnUnit(new UnitTemplate { unitName = "David", armorTier = ArmorTier.Bronze, isCommander = true },
            new HexCoord(1, 3), false);
        SpawnUnit(new UnitTemplate { unitName = "Scout", armorTier = ArmorTier.Leather, isCommander = false },
            new HexCoord(1, 1), false);
        SpawnUnit(new UnitTemplate { unitName = "Scout", armorTier = ArmorTier.Leather, isCommander = false },
            new HexCoord(1, 5), false);

        // Enemy: Chieftain + Raider + Slinger + Scout
        SpawnUnit(new UnitTemplate { unitName = "Amalekite Chieftain", armorTier = ArmorTier.Bronze, isCommander = true },
            new HexCoord(grid.Width - 2, 3), true);
        SpawnUnit(new UnitTemplate { unitName = "Amalekite Raider", armorTier = ArmorTier.Bronze, isCommander = false },
            new HexCoord(grid.Width - 2, 1), true);
        SpawnUnit(new UnitTemplate { unitName = "Amalekite Slinger", armorTier = ArmorTier.Leather, isCommander = false },
            new HexCoord(grid.Width - 2, 5), true);
        SpawnUnit(new UnitTemplate { unitName = "Amalekite Scout", armorTier = ArmorTier.Leather, isCommander = false },
            new HexCoord(grid.Width - 3, 4), true);
    }

    private GameObject SpawnUnit(UnitTemplate template, HexCoord coord, bool isEnemy)
    {
        GameObject unitObj = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);
        unitObj.name = template.unitName;

        // Configure Unit component
        Unit unit = unitObj.GetComponent<Unit>();
        if (unit != null)
        {
            unit.SetName(template.unitName);
            unit.SetEnemy(isEnemy);
            unit.SetCommander(template.isCommander);
            unit.SetArmorTier(template.armorTier);
        }

        // Place on grid
        grid.PlaceUnit(unit, coord);

        // Register with TurnManager
        if (turnManager != null)
        {
            turnManager.RegisterUnit(unit);
        }

        return unitObj;
    }
}

// A serializable template for configuring units in the Inspector
[System.Serializable]
public class UnitTemplate
{
    public string unitName = "Unit";
    public ArmorTier armorTier = ArmorTier.Leather;
    public bool isCommander = false;
}