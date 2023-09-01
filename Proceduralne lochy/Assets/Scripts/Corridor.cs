using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CorridorType
{
    Invalid = 0,
    Horizontal,
    Vertical,
    Lshape_Up_Left,
    Lshape_Up_Right,
    Lshape_Down_Left,
    Lshape_Down_Right
}

public class Corridor : MonoBehaviour
{
    public GameObject ObjectInGame;

    public GameObject LShapedCorridor;

    public Tile StartTile;

    public Tile EndTile;

    public CorridorType CorridorType;

    public int ID = -1;

    static int corridorSize = 3;

    public Corridor()
    {

    }

    public Corridor (GameObject gm, int id)
    {
        ObjectInGame = gm;
        ID = id;
    }

    public Corridor (GameObject gm, Tile start, Tile end, CorridorType type, int id)
    {
        ObjectInGame = gm;
        StartTile = start;
        EndTile = end;
        CorridorType = type;
        ID = id;

        ObjectInGame.GetComponent<ChangePositionCommponent> ().StartTile = start;
        ObjectInGame.GetComponent<ChangePositionCommponent> ().EndTile = end;
        ObjectInGame.GetComponent<ChangePositionCommponent> ().CorridorType = type;
        ObjectInGame.GetComponent<ChangePositionCommponent> ().PastPosition = ObjectInGame.transform.position;
    }

    public Corridor (GameObject gm, GameObject lgm, Tile start, Tile end, CorridorType type, int id)
    {
        ObjectInGame = gm;
        LShapedCorridor = lgm;
        StartTile = start;
        EndTile = end;
        CorridorType = type;
        ID = id;

        ObjectInGame.GetComponent<ChangePositionCommponent> ().StartTile = start;
        ObjectInGame.GetComponent<ChangePositionCommponent> ().EndTile = end;
        ObjectInGame.GetComponent<ChangePositionCommponent> ().CorridorType = type;
        ObjectInGame.GetComponent<ChangePositionCommponent> ().PastPosition = ObjectInGame.transform.position;
        //ObjectInGame.GetComponent<SpriteRenderer> ().color = Color.green;

        LShapedCorridor.GetComponent<ChangePositionCommponent> ().StartTile = start;
        LShapedCorridor.GetComponent<ChangePositionCommponent> ().EndTile = end;
        LShapedCorridor.GetComponent<ChangePositionCommponent> ().CorridorType = type;
        LShapedCorridor.GetComponent<ChangePositionCommponent> ().PastPosition = LShapedCorridor.transform.position;
        //LShapedCorridor.GetComponent<SpriteRenderer> ().color = Color.green;

        //LShapedCorridor.transform.SetParent (ObjectInGame.transform);

        //Destroy (ObjectInGame.GetComponent<ChangePositionCommponent> ());
        //Destroy (ObjectInGame.GetComponent<Rigidbody2D> ());

        //Destroy (LShapedCorridor.GetComponent<ChangePositionCommponent> ());
        //Destroy (LShapedCorridor.GetComponent<Rigidbody2D> ());
    }

    public static GameObject crateStraightCorridorObject (bool horizontal, Vector3 center, float distance)
    {
        GameObject result = new GameObject ();

        if (horizontal)
        {
            result.transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
        }
        else
        {
            result.transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
        }

        result.transform.position = center;
        result.tag = "Corridor";

        result.AddComponent<SpriteRenderer> ();
        result.GetComponent<SpriteRenderer> ().color = Color.cyan;
        result.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Square");

        result.AddComponent<BoxCollider2D> ();
        result.GetComponent<BoxCollider2D> ().size = new Vector2 (0.99f, 0.99f);

        result.AddComponent<Rigidbody2D> ();
        result.GetComponent<Rigidbody2D> ().gravityScale = 0;
        result.GetComponent<Rigidbody2D> ().freezeRotation = true;
        result.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
        //result.GetComponent<Rigidbody2D> ().isKinematic = true;

        result.AddComponent<ChangePositionCommponent> ();

        result.SetActive (true);

        return result;
    }
}
