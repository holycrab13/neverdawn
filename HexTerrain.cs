using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexTerrain : MonoBehaviour
{
    [SerializeField]
    private HexTile hexTilePrefab;

    [SerializeField]
    private Sprite spriteHexTileDashed;

    [SerializeField]
    private Sprite spriteHexTileSolid;

    [SerializeField]
    private int hexTextureWidth;

    [SerializeField]
    private float border = 1.0f;

    [SerializeField]
    [HideInInspector]
    private int width;

    [SerializeField]
    [HideInInspector]
    private int height;

    private float offsetZ = 1.04f;

    private float offsetX = 1.2f;

    [SerializeField]
    [HideInInspector]
    private HexTile[] tiles;

    [SerializeField]
    private Sprite hexBorder;

    [SerializeField]
    private Material borderMaterial;

    private List<HexTile> desertTiles;

    private List<HexTile> activeTiles;

    private List<HexTile>[] placementTiles;

    private HexTile targetedTile;

    private HexTile markedTile;

    [SerializeField]
    private float _tileSize = 1.0f;

    [SerializeField]
    private float occlusionSampleModifier;

    [SerializeField]
    private int occlusionSampleCount;

    [SerializeField]
    private float maxTerrainSlope;

    [SerializeField]
    private float sampleHeight;

    [SerializeField]
    [HideInInspector]
    private float tileWidth;
    [SerializeField]
    [HideInInspector]
    private float tileHeight;
    [SerializeField]
    [HideInInspector]
    private float tileHalfWidth;
    [SerializeField]
    [HideInInspector]
    private float tileC;
    [SerializeField]
    [HideInInspector]
    private float tileM;
    [SerializeField]
    [HideInInspector]
    private List<HexOutline> borders;

    /// <summary>
    /// Creates a border with a specified color. Returns a handle to the border
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static int CreateBorder(List<HexTile> tiles, Color color)
    {
        if(instance.borders == null)
            instance.borders = new List<HexOutline>();

        int handle = createHandle();

        GameObject parentObject = new GameObject("outline_" + handle);
        parentObject.transform.SetParent(instance.transform, true);

        HexOutline outline = parentObject.AddComponent<HexOutline>();
        outline.handle = handle;
        outline.tiles = tiles;
        outline.thickness = instance.lineThickness * 2f;
        outline.color = color;
        outline.material = instance.borderMaterial;

        instance.borders.Add(outline);
        return handle;
    }

    public static GameObject CreateTileSelector(Color color, float thickness = 1.5f, float size = 0.9f)
    {
        GameObject selectorObject = new GameObject("selector");
        selectorObject.layer = 14;

        MeshRenderer renderer = selectorObject.AddComponent<MeshRenderer>();
        renderer.material = Instantiate(instance.borderMaterial);
        renderer.material.color = color;

        selectorObject.AddComponent<MeshFilter>();

        LineHexagon hexagon = selectorObject.AddComponent<LineHexagon>();
        hexagon.thickness = instance.lineThickness * thickness;
        hexagon.scale = instance._tileSize * size;
        hexagon.CreateMesh();

        return selectorObject;
    }

    private static int createHandle()
    {
        return handleCounter++;
    }

    /// <summary>
    /// Destroys a border by border handle
    /// </summary>
    /// <param name="borderHandle"></param>
    public static void DestroyBorder(int borderHandle)
    {
        HexOutline outline = instance.borders.FirstOrDefault(b => b.handle == borderHandle);

        if (outline != null)
        {
            Destroy(outline.gameObject);
            instance.borders.Remove(outline);
        }
        
    }

    public static float tileSize
    {
        get
        {
            if (instance == null)
            {
                return 1.0f;
            }

            return instance._tileSize;
        }
    }

    private List<Frame> maskedFrames = new List<Frame>();

    void Awake()
    {
        exists = true;
    }

    public void CreateTerrain()
    {
        Clear();

        Terrain terrain = GetComponent<Terrain>();

        width = (int)((terrain.terrainData.size.x - 2 * border) / _tileSize);
        height = (int)((terrain.terrainData.size.z - 2 * border) / (0.866025404f * _tileSize));
        
        tiles = new HexTile[width * height];

        tileWidth = instance._tileSize;
        tileHeight = 0.75f * instance._tileSize / 0.866025404f;
        tileHalfWidth = instance._tileSize / 2.0f;
        tileC = tileHeight / 4.0f;
        tileM = tileC / tileHalfWidth;

        float scale = _tileSize / (hexTextureWidth / 100.0f);

        int indexCounter = 0;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width - (z % 2); x++)
            {
                HexTile hexTile = Instantiate(hexTilePrefab);
                Vector3 tilePosition = new Vector3(border + 0.5f * _tileSize +
                    _tileSize * x + 0.5f * _tileSize * (z % 2), 
                    0.0f,
                    z * _tileSize * 0.866025404f + border + 0.5f * _tileSize * (1.0f / 0.866025404f));

                hexTile.x = x;
                hexTile.y = z;
                hexTile.index = indexCounter++;
                hexTile.transform.SetParent(transform);
                hexTile.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                hexTile.transform.localPosition = tilePosition;
                hexTile.gameObject.isStatic = true;

                hexTile.GetComponent<LineHexagon>().thickness = lineThickness;
                hexTile.GetComponent<LineHexagon>().scale = _tileSize;
                hexTile.GetComponent<LineHexagon>().CreateMesh();
                // hexTile.gameObject.hideFlags = HideFlags.HideInHierarchy;

                Ray tileRay = new Ray(hexTile.transform.position + sampleHeight * Vector3.up, Vector3.down);
                int layer_mask = LayerMask.GetMask("Terrain");

                RaycastHit info;

                if (Physics.Raycast(tileRay, out info, 1000.0f, layer_mask))
                {
                    hexTile.transform.position = new Vector3(hexTile.transform.position.x,
                        terrain.transform.position.y + sampleHeight - info.distance, hexTile.transform.position.z);
                }
                else
                {
                    hexTile.isOccluded = true;
                }

                // hexTile.GetComponent<SpriteRenderer>().sprite = spriteHexTileSolid;

                tiles[z * width + x] = hexTile;
            }
        }

        findNeighbors();

        DisableOccluded();
    }

    private static HexTerrain _cachedInstance;

    private static HexTerrain instance
    {
        get
        {
            if (!_cachedInstance)
            {
                _cachedInstance = FindObjectOfType<HexTerrain>();
            }

            return _cachedInstance;
        }
    }

    private static HexCamera _cachedCamera;
    private static int handleCounter;

    [SerializeField]
    private float lineThickness;

    private static HexCamera camera
    {
        get
        {
            if (!_cachedCamera)
            {
                _cachedCamera = FindObjectOfType<HexCamera>();
            }

            return _cachedCamera;
        }
    }


    private void findNeighbors()
    {
        for (int y = 0; y < height; y++)
        {
            int maxWidth = width - (y % 2);
            int maxWidthAdj = width - ((y + 1) % 2);

            for (int x = 0; x < maxWidth; x++)
            {
                HexTile tile = GetTile(x, y);

                if (tile.isOccluded || tile.type == HexTileType.Disabled)
                    continue;

                if (x > 0)
                {
                    tile.AddPath(GetTile(x - 1, y));
                }

                if (x < maxWidth - 1)
                {
                    tile.AddPath(GetTile(x + 1, y));
                }

                if (y > 0 && x < maxWidthAdj)
                {
                    tile.AddPath(GetTile(x, y - 1));
                }

                if (y < height - 1 && x < maxWidthAdj)
                {
                    tile.AddPath(GetTile(x, y + 1));
                }

                if (y % 2 == 0)
                {
                    if (y > 0 && x > 0)
                    {
                        tile.AddPath(GetTile(x - 1, y - 1));
                    }

                    if (y < height - 1 && x > 0)
                    {
                        tile.AddPath(GetTile(x - 1, y + 1));
                    }
                }
                else
                {
                    if (y > 0 && x < maxWidthAdj - 1)
                    {
                        tile.AddPath(GetTile(x + 1, y - 1));
                    }

                    if (y < height - 1 && x < maxWidthAdj - 1)
                    {
                        tile.AddPath(GetTile(x + 1, y + 1));
                    }
                }
            }
        }
    }

   
    public HexTile GetTile(int x, int y)
    {
        int rowWidth = (y % 2 == 0) ? width : width - 1;

        if (x < 0 || y < 0 || y > height || x > rowWidth)
            return null;


        return tiles[y * width + x];
    }

    internal static List<HexTile> GetTilesInRange(int x, int y, int range, bool includeSelf = true)
    {
        List<HexTile> result = new List<HexTile>();

        bool evenRow = y % 2 == 0;
        int mod = evenRow ? 0 : 1;

        for (int i = -range; i <= range; i++)
        {
            int dist = Mathf.Abs(i);
            
            bool evenDist = dist % 2 == 0;
            
            int start = -range + ((dist / 2) + mod * (dist % 2));
            int end = range - ((dist / 2) + (1 - mod) * (dist % 2));

            for(int j = start; j <= end; j++)
            {
                HexTile tileInRange = instance.GetTile(x + j, y + i);

                if (tileInRange && (i != 0 || j != 0 || includeSelf))
                    result.Add(tileInRange);
            }

        }

       

        return result;
    }

    internal static List<HexTile> GetTilesInRange(HexTile tile, int range, bool includeSelf = true)
    {
        return GetTilesInRange(tile.x, tile.y, range, includeSelf);
    }

    public static HexTile GetClosestTile(Vector3 vector3, bool checkIfEmpty = false)
    {
        if (instance == null)
            return null;

        if (instance.tiles == null)
            return null;

        Vector3 pos = vector3 - (instance.transform.position + new Vector3(instance.border, 0.0f, instance.border));
        HexTile selected = getSelectedHexagon(pos.x, pos.z);

        if (selected != null & (!checkIfEmpty || selected.currentFrame == null))
        {
            return selected;
        }

        HexTile closest = null;
        float dist = float.MaxValue;

        foreach (HexTile tile in instance.tiles)
        {
            if (tile != null && (!checkIfEmpty || tile.currentFrame == null))
            {
                float newDist = Vector3.Distance(tile.transform.position, vector3);

                if (newDist < dist)
                {
                    dist = newDist;
                    closest = tile;
                }
            }
        }

        return closest;
    }

    private static HexTile getSelectedHexagon(float x, float y)
    {

        // Find the row and column of the box that the point falls in.
        int row = (int)(y / instance.tileHeight);
        int column;

        bool rowIsOdd = row % 2 == 1;

        // Is the row an odd number?
        if (rowIsOdd)// Yes: Offset x to match the indent of the row
            column = (int)((x - instance.tileHalfWidth) / instance.tileWidth);
        else// No: Calculate normally
            column = (int)(x / instance.tileWidth);
        // Work out the position of the point relative to the box it is in
        double relY = y - (row * instance.tileHeight);
        double relX;

        if (rowIsOdd)
            relX = (x - (column * instance.tileWidth)) - instance.tileHalfWidth;
        else
            relX = x - (column * instance.tileWidth);

        // Work out if the point is above either of the hexagon's top edges
        if (relY < (-instance.tileM * relX) + instance.tileC) // LEFT edge
        {
            row--;
            if (!rowIsOdd)
                column--;
        }
        else if (relY < (instance.tileM * relX) - instance.tileC) // RIGHT edge
        {
            row--;
            if (rowIsOdd)
                column++;
        }

        return instance.GetTile(column, row);
    }


    public void Clear()
    {
        foreach (HexTile tile in tiles)
        {
            if (tile != null)
            {
                DestroyImmediate(tile.gameObject);
            }
        }
    }

    public void DisableOccluded()
    {
        float sampleRadius = (_tileSize / 2.0f) * occlusionSampleModifier;

        float step = 2 * sampleRadius / (float)occlusionSampleCount;

        foreach (HexTile tile in tiles)
        {
            bool occludeTile = false;

            if (tile != null)
            {
                int layer_mask = LayerMask.GetMask("Environment", "Terrain");

                float minDepth = float.MaxValue;
                float maxDepth = float.MinValue;

                for(int i = 0; i < 10; i++)
                {
                    for(int j = 0; j < 10; j++)
                    {
                        Vector3 position = tile.transform.position + new Vector3(-sampleRadius + i * step, 0, -sampleRadius + j * step);

                        if(Vector3.Distance(position, tile.transform.position) < sampleRadius)
                        {
                            Ray tileRay = new Ray(position + sampleHeight * Vector3.up, Vector3.down);

                            RaycastHit info;

                            if(Physics.Raycast(tileRay, out info, 1000.0f, layer_mask))
                            {
                                if(info.collider.gameObject.layer != 9)
                                {
                                    occludeTile = true;
                                    break;
                                }
                                else
                                {
                                    if (info.distance < minDepth)
                                    {
                                        minDepth = info.distance;
                                    }

                                    if (info.distance > maxDepth)
                                    {
                                        maxDepth = info.distance;
                                    }

                                    if ((maxDepth - minDepth) > maxTerrainSlope)
                                    {
                                        occludeTile = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                occludeTile = true;
                                break;
                            }
                        }
                    }

                    if (occludeTile)
                        break;
                }

                tile.isOccluded = occludeTile;
            }
        }
    }

    public static bool exists { get; private set; }
}
