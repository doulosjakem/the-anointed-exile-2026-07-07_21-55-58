using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexTileGenerator : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private float hexSize = 0.5f;
    [SerializeField] private float borderHeight = 0.05f;
    [SerializeField] private Color baseColor = new Color(0.85f, 0.75f, 0.55f); // warm sand
    [SerializeField] private Color borderColor = new Color(0.55f, 0.45f, 0.30f); // darker sand

    [Header("Variation")]
    [SerializeField] [Range(0f, 0.15f)] private float colorVariation = 0.05f;

    private void Start()
    {
        GenerateHexTile();
    }

    public void GenerateHexTile()
    {
        // Generate the flat hex mesh
        Mesh hexMesh = CreateHexMesh();
        GetComponent<MeshFilter>().mesh = hexMesh;

        // Apply material with color variation
        Color variedColor = ApplyColorVariation(baseColor);
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = variedColor;
        mat.SetFloat("_Metallic", 0.1f);
        mat.SetFloat("_Smoothness", 0.3f);
        GetComponent<MeshRenderer>().material = mat;

        // Add border ring
        CreateBorder();
    }

    private Mesh CreateHexMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "HexTile";

        // Vertices for a flat-top hexagon (6 triangles, center vertex + 6 outer vertices)
        Vector3[] vertices = new Vector3[7];
        int[] triangles = new int[18];

        // Center vertex
        vertices[0] = Vector3.zero;

        // Outer vertices
        for (int i = 0; i < 6; i++)
        {
            float angle = 60f * i + 30f; // flat-top starts at 30 degrees
            float rad = angle * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(
                Mathf.Cos(rad) * hexSize,
                0f,
                Mathf.Sin(rad) * hexSize
            );

            // Triangles (center to each edge)
            int ti = i * 3;
            triangles[ti] = 0;
            triangles[ti + 1] = (i + 1) % 6 + 1;
            triangles[ti + 2] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    private void CreateBorder()
    {
        // Create a slightly raised ring border using a cylinder or separate mesh
        // Simple approach: add a hexagonal ring as a child with raised position
        GameObject border = new GameObject("Border");
        border.transform.SetParent(transform, false);
        border.transform.localPosition = new Vector3(0f, borderHeight, 0f);

        MeshFilter borderFilter = border.AddComponent<MeshFilter>();
        borderFilter.mesh = CreateHexRing();
        
        MeshRenderer borderRenderer = border.AddComponent<MeshRenderer>();
        Material borderMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        borderMat.color = borderColor;
        borderMat.SetFloat("_Metallic", 0.2f);
        borderMat.SetFloat("_Smoothness", 0.4f);
        borderRenderer.material = borderMat;
    }

    private Mesh CreateHexRing()
    {
        Mesh mesh = new Mesh();
        mesh.name = "HexRing";

        // Create a ring by making two hexagons (outer and inner) and connecting them
        float innerSize = hexSize * 0.85f;
        float ringHeight = 0.03f;

        Vector3[] vertices = new Vector3[24]; // 12 top, 12 bottom
        int[] triangles = new int[36];

        // Generate vertices
        for (int i = 0; i < 6; i++)
        {
            float angle = 60f * i + 30f;
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            // Outer top
            vertices[i] = new Vector3(cos * hexSize, ringHeight, sin * hexSize);
            // Inner top
            vertices[i + 6] = new Vector3(cos * innerSize, ringHeight, sin * innerSize);
            // Outer bottom
            vertices[i + 12] = new Vector3(cos * hexSize, 0f, sin * hexSize);
            // Inner bottom
            vertices[i + 18] = new Vector3(cos * innerSize, 0f, sin * innerSize);
        }

        // Top face triangles (connecting outer and inner rings)
        for (int i = 0; i < 6; i++)
        {
            int next = (i + 1) % 6;
            int ti = i * 6;

            // Triangle 1
            triangles[ti] = i;
            triangles[ti + 1] = next;
            triangles[ti + 2] = i + 6;

            // Triangle 2
            triangles[ti + 3] = next;
            triangles[ti + 4] = next + 6;
            triangles[ti + 5] = i + 6;
        }

        // Note: For simplicity we only do the top face. A full ring would need side faces too,
        // but the top face + the base hex creates a nice enough visual border.

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    private Color ApplyColorVariation(Color baseColor)
    {
        float r = baseColor.r + Random.Range(-colorVariation, colorVariation);
        float g = baseColor.g + Random.Range(-colorVariation, colorVariation);
        float b = baseColor.b + Random.Range(-colorVariation, colorVariation);
        return new Color(
            Mathf.Clamp01(r),
            Mathf.Clamp01(g),
            Mathf.Clamp01(b)
        );
    }

    /// <summary>
    /// Configure the tile from an external source (e.g., HexGrid)
    /// </summary>
    public void Configure(float hexSize, Color baseColor, Color borderColor, float variation)
    {
        this.hexSize = hexSize;
        this.baseColor = baseColor;
        this.borderColor = borderColor;
        this.colorVariation = variation;
        GenerateHexTile();
    }
}