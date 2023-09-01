using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    NoType,
    Normal = 0,
    Boss, 
    Start,
    Cheast,
    Heal,
    Corridor
}

public class Tile
{
    // GameObject that represent tile in the game
    public GameObject ObjectInGame;

    // The type of this tile
    public TileType TileType;

    // Center of the tile
    public Vector2 Center;

    // Id of the tile
    public int ID = -1;

    public float X
    {
        get { return ObjectInGame.transform.position.x - (Width / 2); }
    }

    public float Y
    {
        get { return ObjectInGame.transform.position.y + (Hight / 2); }
    }

    public float Width
    {
        get { return ObjectInGame.GetComponent<SpriteRenderer> ().bounds.size.x; }
    }

    public float Hight
    {
        get { return ObjectInGame.GetComponent<SpriteRenderer> ().bounds.size.y; }
    }

    public Tile (Vector3 startPosition, Vector3 scale)
    {
        ObjectInGame = new GameObject ();
        ObjectInGame.transform.localPosition = startPosition;
        ObjectInGame.transform.localScale = scale;
        ObjectInGame.tag = "Tile";

        ObjectInGame.AddComponent<SpriteRenderer> ();
        ObjectInGame.GetComponent<SpriteRenderer> ().color = Color.black;
        ObjectInGame.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Square");

        ObjectInGame.AddComponent<BoxCollider2D> ();
        ObjectInGame.GetComponent<BoxCollider2D> ().size = new Vector2 (1.3f, 1.3f);

        ObjectInGame.AddComponent<Rigidbody2D> ();
        ObjectInGame.GetComponent<Rigidbody2D> ().gravityScale = 0;
        ObjectInGame.GetComponent<Rigidbody2D> ().freezeRotation = true;

        TileType = TileType.Corridor;
    }

    // Set the sprite for this tile
    public void SetSprite (Sprite sprite)
    {
        ObjectInGame.GetComponent<SpriteRenderer> ().sprite = sprite;
    }

    // Set the color for this tile
    public void SetColor (Color color)
    {
        ObjectInGame.GetComponent<SpriteRenderer> ().color = color;
    }

    public void SetTileType (TileType type)
    {
        TileType = type;

        switch(type)
        {
            case TileType.Normal:

                SetColor (Color.gray);

                break;

            case TileType.Boss:

                SetColor (Color.red);

                break;

            case TileType.Start:

                SetColor (Color.green);

                break;

            case TileType.Cheast:

                SetColor (Color.blue);

                break;

            case TileType.Heal:

                SetColor (Color.white);

                break;

            case TileType.Corridor:

                SetColor (Color.black);

                break;
        }
    }

    public float AreaOfGameObject()
    {
        return ObjectInGame.GetComponent<SpriteRenderer> ().bounds.size.x * ObjectInGame.GetComponent<SpriteRenderer> ().bounds.size.y;
    }
}
