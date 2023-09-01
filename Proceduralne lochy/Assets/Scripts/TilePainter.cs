using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    [SerializeField]
    private TileBase floorTile, trapDoorTilemap;

    [SerializeField]
    private TileBase wallTop, wallSideRight, wallSiderLeft, wallBottom, wallFull, wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    [SerializeField]
    private Sprite healingSprite, cheastSprite, columnSprite, skullSprite;

    private HashSet<Vector3Int> positionOfFloorTiles = new HashSet<Vector3Int>();
    private List<GameObject> elementsOnMap = new List<GameObject> ();

    public void PaintRooms (List<Tile> rooms)
    {
        foreach (Tile t in rooms)
        {
            if (t.ObjectInGame != null)
            {
                paintSpecificRoom (t.X, t.Y, t.Hight, t.Width, t.TileType);
                Destroy (t.ObjectInGame);
            }
        }
    }

    public void PaintCorridors (List<Corridor> corridor)
    {
        float x, y, hight, width;

        foreach (Corridor c in corridor)
        {
            if (c.ObjectInGame != null)
            {
                width = c.ObjectInGame.GetComponent<SpriteRenderer> ().bounds.size.x;
                hight = c.ObjectInGame.GetComponent<SpriteRenderer> ().bounds.size.y;
                x = c.ObjectInGame.transform.position.x - (width / 2);
                y = c.ObjectInGame.transform.position.y + (hight / 2);

                paintSpecificRoom (x, y, hight, width, TileType.Corridor);

                if (c.LShapedCorridor != null)
                {
                    width = c.LShapedCorridor.GetComponent<SpriteRenderer> ().bounds.size.x;
                    hight = c.LShapedCorridor.GetComponent<SpriteRenderer> ().bounds.size.y;
                    x = c.LShapedCorridor.transform.position.x - (width / 2);
                    y = c.LShapedCorridor.transform.position.y + (hight / 2);

                    paintSpecificRoom (x, y, hight, width, TileType.Corridor);
                    Destroy (c.LShapedCorridor);
                }

                Destroy (c.ObjectInGame);
            }
        }
    }

    public void PaintWalls()
    {
        paintBasicWalls (findPositionsOfWallsInDirections (positionOfFloorTiles, Direction2D.cardinalDirectionsList), positionOfFloorTiles);
        paintCornerWalls (findPositionsOfWallsInDirections (positionOfFloorTiles, Direction2D.diagonalDirectionsList), positionOfFloorTiles);
    }

    public void Clear ()
    {
        floorTilemap.ClearAllTiles ();
        wallTilemap.ClearAllTiles ();
        positionOfFloorTiles.Clear ();

        if (elementsOnMap != null)
        {
            foreach (GameObject item in elementsOnMap)
            {
                Destroy (item);
            }
        }
    }

    private void paintSpecificRoom (float sX, float sY, float hight, float wight, TileType type)
    {
        float wightInTies = 0, hightInTiles = 0;

        for (float y = sY; y > sY - hight ; y--)
        {
            for (float x = sX; x < sX + wight; x++)
            {
                var tilePosition = floorTilemap.WorldToCell (new Vector3 (x, y));
                positionOfFloorTiles.Add (tilePosition);
                floorTilemap.SetTile (tilePosition, floorTile);

                if (y == sY)
                    wightInTies++;
            }

            hightInTiles++;
        }

        if (type != TileType.Corridor && type != TileType.Start)
        {
            GameObject contour = new GameObject ();
            contour.name = "Counture";
            contour.tag = "Contour";
            contour.layer = 8;
            contour.AddComponent<RectTransform> ();
            contour.GetComponent<RectTransform> ().sizeDelta = new Vector2 (wightInTies, hightInTiles);
            contour.GetComponent<RectTransform> ().position = new Vector3 (sX + (wightInTies / 2), sY - (hightInTiles / 2));
            contour.AddComponent<BoxCollider2D> ();
            contour.GetComponent<BoxCollider2D> ().size = contour.GetComponent<RectTransform> ().sizeDelta;
            contour.AddComponent<TileInfo> ();
            contour.GetComponent<TileInfo> ().Setup ((int)wightInTies, (int)hightInTiles, type);
        }


        int spriteWight, spriteHight;

        switch (type)
        {
            case TileType.Heal:

                GameObject healingFlask = new GameObject ();
                healingFlask.name = "Healing Flask";
                healingFlask.AddComponent<SpriteRenderer> ();
                healingFlask.GetComponent<SpriteRenderer> ().sprite = healingSprite;
                healingFlask.GetComponent<SpriteRenderer> ().sortingOrder = 1;
                healingFlask.AddComponent<Rigidbody2D> ();
                healingFlask.GetComponent<Rigidbody2D> ().gravityScale = 0;
                healingFlask.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
                healingFlask.AddComponent<BoxCollider2D> ();
                healingFlask.AddComponent<CollisionCommponent> ();
                healingFlask.GetComponent<CollisionCommponent> ().ObjectType = ObjectType.Potion;

                spriteWight = (int)healingFlask.GetComponent<SpriteRenderer> ().bounds.size.x;
                spriteHight = (int)healingFlask.GetComponent<SpriteRenderer> ().bounds.size.y;

                healingFlask.transform.position = new Vector2 ((int)sX + (wight / 2) - (spriteWight / 2), (int)sY - (hight / 2) - (spriteHight / 2));

                elementsOnMap.Add (healingFlask);

                break;

            case TileType.Cheast:

                GameObject cheast = new GameObject ();
                cheast.name = "Cheast";
                cheast.AddComponent<SpriteRenderer> ();
                cheast.GetComponent<SpriteRenderer> ().sprite = cheastSprite;
                cheast.GetComponent<SpriteRenderer> ().sortingOrder = 1;
                cheast.AddComponent<Rigidbody2D> ();
                cheast.GetComponent<Rigidbody2D> ().gravityScale = 0;
                cheast.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
                cheast.AddComponent<BoxCollider2D> ();
                cheast.AddComponent<CollisionCommponent> ();
                cheast.GetComponent<CollisionCommponent> ().ObjectType = ObjectType.Cheast;

                cheast.transform.position = new Vector2 ((int)sX + (wight / 2), (int)sY - (hight / 2));

                elementsOnMap.Add (cheast);

                break;

            case TileType.Boss:

                var tilePosition = floorTilemap.WorldToCell (new Vector3 (sX + (wight / 2), sY - (hight / 2)));
                floorTilemap.SetTile (tilePosition, trapDoorTilemap);
                GameObject trapDoor = new GameObject ();
                trapDoor.name = "Trap Door";
                trapDoor.layer = 8;
                trapDoor.transform.position = tilePosition;
                trapDoor.AddComponent<Rigidbody2D> ();
                trapDoor.GetComponent<Rigidbody2D> ().gravityScale = 0;
                trapDoor.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
                trapDoor.AddComponent<BoxCollider2D> ();
                trapDoor.AddComponent<CollisionCommponent> ();
                trapDoor.GetComponent<CollisionCommponent> ().ObjectType = ObjectType.TrapDoor;

                for (int i = 0; i < 5; i++)
                {
                    GameObject skull = new GameObject ();
                    skull.name = "Skull";
                    skull.AddComponent<SpriteRenderer> ();
                    skull.GetComponent<SpriteRenderer> ().sprite = skullSprite;
                    skull.GetComponent<SpriteRenderer> ().sortingOrder = 1;

                    float rX = Random.Range (sX + 1, sX + wight - 1);
                    float rY = Random.Range (sY - hight + 1, sY - 1);

                    skull.transform.position = new Vector2 ((int)rX, (int)rY );

                    elementsOnMap.Add (skull);
                }

                break;

            case TileType.Start:

                GameObject column = new GameObject ();
                column.name = "Column";
                column.AddComponent<SpriteRenderer> ();
                column.GetComponent<SpriteRenderer> ().sprite = columnSprite;
                column.GetComponent<SpriteRenderer> ().sortingOrder = 1;
                column.AddComponent<Rigidbody2D> ();
                column.GetComponent<Rigidbody2D> ().gravityScale = 0;
                column.GetComponent<Rigidbody2D> ().freezeRotation = true;
                column.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
                column.AddComponent<BoxCollider2D> ();

                column.transform.localScale = new Vector3 (2, 2, 2);
                column.transform.position = new Vector2 ((int)sX + (wight / 2) - 2, (int)sY - (hight / 2) + 2); //Left up

                elementsOnMap.Add (column);

                column = new GameObject ();
                column.name = "Column";
                column.AddComponent<SpriteRenderer> ();
                column.GetComponent<SpriteRenderer> ().sprite = columnSprite;
                column.GetComponent<SpriteRenderer> ().sortingOrder = 1;
                column.AddComponent<Rigidbody2D> ();
                column.GetComponent<Rigidbody2D> ().gravityScale = 0;
                column.GetComponent<Rigidbody2D> ().freezeRotation = true;
                column.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
                column.AddComponent<BoxCollider2D> ();

                column.transform.localScale = new Vector3 (2, 2, 2);
                column.transform.position = new Vector2 ((int)sX + (wight / 2) - 2, (int)sY - (hight / 2) - 2); //Left down

                elementsOnMap.Add (column);

                column = new GameObject ();
                column.name = "Column";
                column.AddComponent<SpriteRenderer> ();
                column.GetComponent<SpriteRenderer> ().sprite = columnSprite;
                column.GetComponent<SpriteRenderer> ().sortingOrder = 1;
                column.AddComponent<Rigidbody2D> ();
                column.GetComponent<Rigidbody2D> ().gravityScale = 0;
                column.GetComponent<Rigidbody2D> ().freezeRotation = true;
                column.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
                column.AddComponent<BoxCollider2D> ();

                column.transform.localScale = new Vector3 (2, 2, 2);
                column.transform.position = new Vector2 ((int)sX + (wight / 2) + 2, (int)sY - (hight / 2) + 2); //Right up

                elementsOnMap.Add (column);

                column = new GameObject ();
                column.name = "Column";
                column.AddComponent<SpriteRenderer> ();
                column.GetComponent<SpriteRenderer> ().sprite = columnSprite;
                column.GetComponent<SpriteRenderer> ().sortingOrder = 1;
                column.AddComponent<Rigidbody2D> ();
                column.GetComponent<Rigidbody2D> ().gravityScale = 0;
                column.GetComponent<Rigidbody2D> ().freezeRotation = true;
                column.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
                column.AddComponent<BoxCollider2D> ();

                column.transform.localScale = new Vector3 (2, 2, 2);
                column.transform.position = new Vector2 ((int)sX + (wight / 2) + 2, (int)sY - (hight / 2) - 2); //Right down

                elementsOnMap.Add (column);

                break;
        }
    }

    private void paintBasicWalls (HashSet<Vector3Int> wallPosition, HashSet<Vector3Int> floorPositions)
    {
        foreach (var position in wallPosition)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPosition = position + (Vector3Int)direction;
                if (floorPositions.Contains (neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            paintBasicWall ((Vector2Int)position, neighboursBinaryType);
        }
    }

    private void paintCornerWalls (HashSet<Vector3Int> cornerWallPositions, HashSet<Vector3Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + (Vector3Int)direction;
                if (floorPositions.Contains (neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            paintCornerWall ((Vector2Int)position, neighboursBinaryType);
        }
    }

    internal void paintBasicWall (Vector2Int position, string binaryType)
    {
        int typeAsInt = System.Convert.ToInt32 (binaryType, 2);
        TileBase tile = null;
        if (WallHelper.wallTop.Contains (typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallHelper.wallSideRight.Contains (typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallHelper.wallSideLeft.Contains (typeAsInt))
        {
            tile = wallSiderLeft;
        }
        else if (WallHelper.wallBottm.Contains (typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallHelper.wallFull.Contains (typeAsInt))
        {
            tile = wallFull;
        }

        if (tile != null)
        {
            var tilePosition = floorTilemap.WorldToCell ((Vector3Int)position);
            wallTilemap.SetTile (tilePosition, tile);
        }
    }

    internal void paintCornerWall (Vector2Int position, string binaryType)
    {
        int typeASInt = System.Convert.ToInt32 (binaryType, 2);
        TileBase tile = null;

        if (WallHelper.wallInnerCornerDownLeft.Contains (typeASInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallHelper.wallInnerCornerDownRight.Contains (typeASInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallHelper.wallDiagonalCornerDownLeft.Contains (typeASInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallHelper.wallDiagonalCornerDownRight.Contains (typeASInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallHelper.wallDiagonalCornerUpRight.Contains (typeASInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallHelper.wallDiagonalCornerUpLeft.Contains (typeASInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallHelper.wallFullEightDirections.Contains (typeASInt))
        {
            tile = wallFull;
        }
        else if (WallHelper.wallBottmEightDirections.Contains (typeASInt))
        {
            tile = wallBottom;
        }

        if (tile != null)
        {
            var tilePosition = floorTilemap.WorldToCell ((Vector3Int)position);
            wallTilemap.SetTile (tilePosition, tile);
        }
    }

    internal HashSet<Vector3Int> findPositionsOfWallsInDirections (HashSet<Vector3Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int> ();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + (Vector3Int)direction;
                if (floorPositions.Contains (neighbourPosition) == false)
                    wallPositions.Add (neighbourPosition);
            }
        }
        return wallPositions;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, 0) //LEFT
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 1) //LEFT-UP
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 0), //LEFT
        new Vector2Int(-1, 1) //LEFT-UP

    };
}
