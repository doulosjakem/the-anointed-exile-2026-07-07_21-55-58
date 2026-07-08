using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitVisual : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Color playerColor = new Color(0.25f, 0.50f, 0.75f); // warm blue
    [SerializeField] private Color enemyColor = new Color(0.75f, 0.25f, 0.20f); // dusty red
    [SerializeField] private float baseRadius = 0.3f;
    [SerializeField] private float baseHeight = 0.08f;

    private Unit unit;
    private GameObject baseObject;
    private GameObject bodyObject;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        CreateUnitVisual();
    }

    private void CreateUnitVisual()
    {
        Color teamColor = unit.IsEnemy ? enemyColor : playerColor;

        // Create the circular base (like a board game token)
        baseObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        baseObject.name = "Base";
        baseObject.transform.SetParent(transform, false);
        baseObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        baseObject.transform.localScale = new Vector3(baseRadius * 2f, baseHeight, baseRadius * 2f);

        // Darker base
        Renderer baseRenderer = baseObject.GetComponent<Renderer>();
        Material baseMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        baseMat.color = teamColor * 0.6f; // darker shade
        baseMat.SetFloat("_Metallic", 0.5f);
        baseMat.SetFloat("_Smoothness", 0.6f);
        baseRenderer.material = baseMat;

        // Create the body based on unit type
        CreateBody(teamColor);

        // Add a highlight ring for selection (disabled by default)
        CreateSelectionRing();
    }

    private void CreateBody(Color teamColor)
    {
        if (unit.IsCommander)
        {
            // Commander: taller body with "shoulders" (sphere on cylinder)
            bodyObject = new GameObject("CommanderBody");
            bodyObject.transform.SetParent(transform, false);
            bodyObject.transform.localPosition = new Vector3(0f, 0.2f, 0f);

            // Main body
            GameObject torso = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            torso.name = "Torso";
            torso.transform.SetParent(bodyObject.transform, false);
            torso.transform.localPosition = new Vector3(0f, 0.12f, 0f);
            torso.transform.localScale = new Vector3(0.2f, 0.18f, 0.2f);
            ApplyMaterial(torso, teamColor, 0.3f, 0.4f);

            // Shoulders (horizontal cylinder or sphere)
            GameObject shoulders = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            shoulders.name = "Shoulders";
            shoulders.transform.SetParent(bodyObject.transform, false);
            shoulders.transform.localPosition = new Vector3(0f, 0.25f, 0f);
            shoulders.transform.localScale = new Vector3(0.3f, 0.06f, 0.15f);
            ApplyMaterial(shoulders, teamColor * 0.8f, 0.3f, 0.4f);

            // Head
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head";
            head.transform.SetParent(bodyObject.transform, false);
            head.transform.localPosition = new Vector3(0f, 0.35f, 0f);
            head.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ApplyMaterial(head, teamColor * 0.9f, 0.2f, 0.3f);

            // Small "cape" indicator - a flat piece behind
            GameObject cape = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cape.name = "Cape";
            cape.transform.SetParent(bodyObject.transform, false);
            cape.transform.localPosition = new Vector3(0f, 0.15f, -0.15f);
            cape.transform.localScale = new Vector3(0.2f, 0.15f, 0.02f);
            ApplyMaterial(cape, teamColor * 0.7f, 0.1f, 0.2f);
        }
        else if (unit.UnitName.ToLower().Contains("slinger") || unit.UnitName.ToLower().Contains("archer"))
        {
            // Ranged: smaller, crouched shape
            bodyObject = new GameObject("RangedBody");
            bodyObject.transform.SetParent(transform, false);
            bodyObject.transform.localPosition = new Vector3(0f, 0.12f, 0f);

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "Body";
            body.transform.SetParent(bodyObject.transform, false);
            body.transform.localPosition = new Vector3(0f, 0.06f, 0f);
            body.transform.localScale = new Vector3(0.14f, 0.1f, 0.14f);
            ApplyMaterial(body, teamColor, 0.2f, 0.3f);

            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head";
            head.transform.SetParent(bodyObject.transform, false);
            head.transform.localPosition = new Vector3(0f, 0.16f, 0.05f);
            head.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            ApplyMaterial(head, teamColor * 0.9f, 0.2f, 0.3f);
        }
        else if (unit.UnitName.ToLower().Contains("scout"))
        {
            // Scout: slim, small
            bodyObject = new GameObject("ScoutBody");
            bodyObject.transform.SetParent(transform, false);
            bodyObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "Body";
            body.transform.SetParent(bodyObject.transform, false);
            body.transform.localPosition = new Vector3(0f, 0.08f, 0f);
            body.transform.localScale = new Vector3(0.1f, 0.12f, 0.1f);
            ApplyMaterial(body, teamColor, 0.2f, 0.3f);

            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head";
            head.transform.SetParent(bodyObject.transform, false);
            head.transform.localPosition = new Vector3(0f, 0.18f, 0f);
            head.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
            ApplyMaterial(head, teamColor * 0.9f, 0.2f, 0.3f);
        }
        else
        {
            // Swordsman/Spearman/Raider: standard humanoid shape
            bodyObject = new GameObject("StandardBody");
            bodyObject.transform.SetParent(transform, false);
            bodyObject.transform.localPosition = new Vector3(0f, 0.15f, 0f);

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "Body";
            body.transform.SetParent(bodyObject.transform, false);
            body.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            body.transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
            ApplyMaterial(body, teamColor, 0.3f, 0.4f);

            // Shield arm (small cube on one side)
            GameObject shield = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shield.name = "Shield";
            shield.transform.SetParent(bodyObject.transform, false);
            shield.transform.localPosition = new Vector3(-0.12f, 0.1f, 0f);
            shield.transform.localScale = new Vector3(0.04f, 0.1f, 0.08f);
            ApplyMaterial(shield, teamColor * 0.8f, 0.4f, 0.5f);

            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head";
            head.transform.SetParent(bodyObject.transform, false);
            head.transform.localPosition = new Vector3(0f, 0.22f, 0f);
            head.transform.localScale = new Vector3(0.09f, 0.09f, 0.09f);
            ApplyMaterial(head, teamColor * 0.9f, 0.2f, 0.3f);

            // Weapon indicator (small stick on the right)
            if (unit.UnitName.ToLower().Contains("spear"))
            {
                GameObject spear = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                spear.name = "Spear";
                spear.transform.SetParent(bodyObject.transform, false);
                spear.transform.localPosition = new Vector3(0.12f, 0.2f, 0f);
                spear.transform.localScale = new Vector3(0.02f, 0.3f, 0.02f);
                ApplyMaterial(spear, Color.gray * 0.7f, 0.5f, 0.3f);
            }
            else
            {
                GameObject sword = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sword.name = "Sword";
                sword.transform.SetParent(bodyObject.transform, false);
                sword.transform.localPosition = new Vector3(0.12f, 0.18f, 0f);
                sword.transform.localScale = new Vector3(0.02f, 0.15f, 0.04f);
                ApplyMaterial(sword, Color.gray * 0.6f, 0.6f, 0.3f);
            }
        }
    }

    private void CreateSelectionRing()
    {
        // A flat ring that appears when selected
        GameObject ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring.name = "SelectionRing";
        ring.transform.SetParent(transform, false);
        ring.transform.localPosition = new Vector3(0f, 0.01f, 0f);
        ring.transform.localScale = new Vector3(baseRadius * 1.8f, 0.02f, baseRadius * 1.8f);

        Renderer ringRenderer = ring.GetComponent<Renderer>();
        Material ringMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        Color ringColor = unit.IsEnemy ? new Color(1f, 0.3f, 0.2f, 0.5f) : new Color(0.2f, 0.8f, 0.3f, 0.5f);
        ringMat.color = ringColor;
        ringMat.SetFloat("_Metallic", 0.1f);
        ringMat.SetFloat("_Smoothness", 0.2f);
        ringRenderer.material = ringMat;

        // Disabled by default
        ring.SetActive(false);
    }

    /// <summary>
    /// Show or hide the selection ring
    /// </summary>
    public void SetSelected(bool selected)
    {
        Transform ring = transform.Find("SelectionRing");
        if (ring != null)
        {
            ring.gameObject.SetActive(selected);
        }
    }

    private void ApplyMaterial(GameObject obj, Color color, float metallic, float smoothness)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = color;
        mat.SetFloat("_Metallic", metallic);
        mat.SetFloat("_Smoothness", smoothness);
        renderer.material = mat;
    }
}