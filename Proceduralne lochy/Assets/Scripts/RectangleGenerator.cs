using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectangleGenerator : MonoBehaviour
{
    [SerializeField]
    protected int maxX = 5;

    [SerializeField]
    protected int maxY = 5;

    [SerializeField]
    protected int minX = 1;

    [SerializeField]
    protected int minY = 1;

    [SerializeField]
    protected int ammount = 10;

    [SerializeField]
    private int radious = 5;

    [SerializeField]
    private TilePainter painter;

    [SerializeField]
    public GameObject Player;

    [SerializeField]
    public GameObject Statistisc;

    [SerializeField]
    public GameObject LoadingScreen;

    [SerializeField]
    public Slider LoadingBar;

    protected List<Tile> listOfTiles = new();
    protected List<Corridor> listoOFCorridors = new List<Corridor> ();
    protected List<GameObject> helpGameObject = new List<GameObject> ();
    protected Vector3 positionOfStart;
    protected float progres = 0;

    protected TileGraph tileGraph;
    public static RectangleGenerator RectangleGeneratorInstance
    {
        get; 
        private set;
    }

    protected bool roomsFixed = false;
    protected bool corridorsFixed = false;

    public void SetThisInstanceAsCurrent ()
    {
        RectangleGeneratorInstance = this;
    }

    public void GenerateTiles ()
    {
        GameObject.Find ("CanvasControler").GetComponent<CanvaControler> ().defeatCanvas.SetActive (false);
        LoadingScreen.SetActive (true);
        LoadingBar.value = 0;
        progres = 0;
        helpGameObject.Clear ();

        StartCoroutine (resetGameObjects ());

        if (listOfTiles.Count > 0)
        {
            foreach (Tile tile in listOfTiles)
                Destroy (tile.ObjectInGame);
        }

        if (listoOFCorridors.Count > 0)
        {
            foreach (Corridor corr in listoOFCorridors)
                Destroy (corr.ObjectInGame);
        }

        if (helpGameObject.Count > 0)
        {
            foreach (GameObject gm in helpGameObject)
            {
                if(gm.transform.childCount > 0)
                    Destroy (gm.transform.GetChild (0));

                if (gm.transform.childCount > 0)
                    Destroy (gm.transform.GetChild (1));

                Destroy (gm);
            }
        }

        Player.SetActive (false);
        Player.GetComponent<SpriteRenderer> ().enabled = false;
        Player.GetComponent<PlayerInfo> ().HealthBar.SetActive (false);
        Player.GetComponent<PlayerInfo> ().HealthBar.GetComponent<Slider> ().value = 100;
        Statistisc.SetActive (false);

        listOfTiles.Clear ();
        listoOFCorridors.Clear ();

        painter.Clear ();

        roomsFixed = false;
        corridorsFixed = false;

        for (int i = 0; i < ammount; i++)
        {
            int temX = Random.Range (-radious, radious);
            int randomMinus = Random.Range (0, 2) * 2 - 1;
            int temY = randomMinus * (int)Mathf.Sqrt ((radious * radious) - Mathf.Pow (temX, 2));

            listOfTiles.Add (new Tile (new Vector3 (temX, temY), new Vector3 ((int)Random.Range (minX, maxX), (int)Random.Range (minY, maxY))));
        }

        listOfTiles.Sort ((tile1, tile2) => tile1.AreaOfGameObject ().CompareTo (tile2.AreaOfGameObject ()));

        refreshLoadingBar (10);
    }

    private void Update ()
    {
        if (listOfTiles.Count > 0)
        {
            if (!roomsFixed)
            {
                bool dilateRigedbody = true;

                foreach (Tile tile in listOfTiles)
                {
                    if (!tile.ObjectInGame.GetComponent<Rigidbody2D> ().IsSleeping ())
                        dilateRigedbody = false;
                }

                if (dilateRigedbody)
                {
                    foreach (Tile tile in listOfTiles)
                    {
                        tile.ObjectInGame.GetComponent<Rigidbody2D> ().isKinematic = true;
                        tile.Center = new Vector2 (tile.ObjectInGame.transform.position.x, tile.ObjectInGame.transform.position.y);
                    }

                    roomsFixed = true;

                    decideTileType ();
                    refreshLoadingBar (15);

                    generateGraph (listOfTiles);
                    changeTilesBoxColliderToTheSizeOfObject ();
                    refreshLoadingBar (20);

                    generateCorridors (tileGraph);
                    SetThisInstanceAsCurrent ();
                    refreshLoadingBar (50);
                }
            }
            else if (!corridorsFixed)
            {
                bool generateFloor = true;

                foreach (Corridor corr in listoOFCorridors)
                {
                    if (corr.ObjectInGame != null)
                        if (corr.ObjectInGame.GetComponent<ChangePositionCommponent> ().IsItMoving)
                            generateFloor = false;

                    if (corr.LShapedCorridor != null)
                        if (corr.LShapedCorridor.GetComponent<ChangePositionCommponent> ().IsItMoving)
                            generateFloor = false;
                }

                refreshLoadingBar (55);

                if (generateFloor)
                {
                    foreach (Corridor corr in listoOFCorridors)
                    {
                        if (corr.ObjectInGame != null)
                        {
                            corr.ObjectInGame.GetComponent<Rigidbody2D> ().isKinematic = true;
                            Destroy (corr.ObjectInGame.GetComponent<ChangePositionCommponent> ());
                        }

                        if (corr.LShapedCorridor != null)
                        {
                            corr.LShapedCorridor.GetComponent<Rigidbody2D> ().isKinematic = true;
                            Destroy (corr.LShapedCorridor.GetComponent<ChangePositionCommponent> ());
                        }
                    }

                    refreshLoadingBar (65);

                    corridorsFixed = true;
                    Player.SetActive (true);

                    painter.PaintRooms (listOfTiles);
                    refreshLoadingBar (70);

                    painter.PaintCorridors (listoOFCorridors);
                    refreshLoadingBar (80);

                    painter.PaintWalls ();
                    refreshLoadingBar (90);

                    Player.transform.position = positionOfStart;
                    Player.GetComponent<SpriteRenderer> ().enabled = true;
                    Player.GetComponent<PlayerInfo> ().HealthBar.SetActive (true);
                    Statistisc.SetActive (true);

                    if (helpGameObject.Count > 0)
                    {
                        foreach (GameObject gm in helpGameObject)
                        {
                            if (gm.transform.childCount > 0)
                                Destroy (gm.transform.GetChild (0));

                            if (gm.transform.childCount > 0)
                                Destroy (gm.transform.GetChild (1));

                            Destroy (gm);
                        }
                    }

                    refreshLoadingBar (100);
                    LoadingScreen.SetActive (false);
                }
            }
        }
    }

    IEnumerator resetGameObjects ()
    {
        while (GameObject.Find ("Counture") != null) 
        {
            Destroy (GameObject.Find ("Counture"));
            yield return new WaitForEndOfFrame ();
        }

        while (GameObject.Find ("Enemy") != null)
        {
            Destroy (GameObject.Find ("Enemy"));
            yield return new WaitForEndOfFrame ();
        }

        while (GameObject.Find ("Boss") != null)
        {
            Destroy (GameObject.Find ("Boss"));
            yield return new WaitForEndOfFrame ();
        }

        while (GameObject.Find ("Cheast") != null)
        {
            Destroy (GameObject.Find ("Cheast"));
            yield return new WaitForEndOfFrame ();
        }

        while (GameObject.Find ("Healing Flask") != null)
        {
            Destroy (GameObject.Find ("Healing Flask"));
            yield return new WaitForEndOfFrame ();
        }

        while (GameObject.Find ("Skull") != null)
        {
            Destroy (GameObject.Find ("Skull"));
            yield return new WaitForEndOfFrame ();
        }

        while (GameObject.Find ("Column") != null)
        {
            Destroy (GameObject.Find ("Column"));
            yield return new WaitForEndOfFrame ();
        }

        while (GameObject.Find ("Trap Door") != null)
        {
            Destroy (GameObject.Find ("Trap Door"));
            yield return new WaitForEndOfFrame ();
        }

    }

    protected void refreshLoadingBar (float p)
    {
        progres = p;
        LoadingBar.value = progres;
    }

    private void changeTilesBoxColliderToTheSizeOfObject ()
    {
        foreach (Tile tile in listOfTiles)
        {
            tile.ObjectInGame.GetComponent<BoxCollider2D> ().size = new Vector2 (1f,1f);
            tile.ObjectInGame.SetActive (true);
        }
    }

    private void decideTileType ()
    {
        listOfTiles[listOfTiles.Count - 1].SetTileType (TileType.Boss);

        int tenPercent = listOfTiles.Count / 10;
        float biggestDistance = 0;
        int index = 0;

        for (int i = listOfTiles.Count -  1; i >= tenPercent; i--)
        {
            if (0 >= biggestDistance.CompareTo(Vector3.Distance (listOfTiles[listOfTiles.Count - 1].Center, listOfTiles[i].Center)))
            {
                index = i;
                biggestDistance = Vector3.Distance (listOfTiles[listOfTiles.Count - 1].Center, listOfTiles[i].Center);
            }
        }

        listOfTiles[index].SetTileType (TileType.Start);
        positionOfStart = listOfTiles[index].ObjectInGame.transform.position;

        List<int> lastFields = new List<int>(tenPercent * 2);

        for (int i = tenPercent; i < tenPercent * 3; i++)
            lastFields.Add (i);

        int random;

        if (lastFields.Count >= 3)
        {
            random = (int) Random.Range (0, lastFields.Count);

            while (listOfTiles[lastFields[random]].TileType != TileType.Corridor)
                random = (int)Random.Range (0, lastFields.Count);

            listOfTiles[lastFields[random]].SetTileType (TileType.Cheast);
            lastFields.Remove (lastFields[random]);

            random = (int)Random.Range (0, lastFields.Count);

            while (random <= 5 && listOfTiles[lastFields[random]].TileType != TileType.Corridor)
                random = (int)Random.Range (0, lastFields.Count);

            listOfTiles[lastFields[random]].SetTileType (TileType.Cheast);
            lastFields.Remove (lastFields[random]);

            random = (int)Random.Range (0, lastFields.Count);

            while (listOfTiles[lastFields[random]].TileType != TileType.Corridor)
                random = (int)Random.Range (0, lastFields.Count);

            listOfTiles[lastFields[random]].SetTileType (TileType.Heal);
        }
        else if (tenPercent == 2)
        {
            random = (int)Random.Range (0, lastFields.Count);

            while (listOfTiles[lastFields[random]].TileType != TileType.Corridor)
                random = (int)Random.Range (0, lastFields.Count);

            listOfTiles[lastFields[random]].SetTileType (TileType.Cheast);
            lastFields.Remove (lastFields[random]);

            listOfTiles[0].SetTileType (TileType.Heal);
        }
        else
        {
            listOfTiles[0].SetTileType (TileType.Heal);
        }
    }

    private void generateGraph(List<Tile> AllTiles)
    {
        List<Tile> mainRooms = new List<Tile> ();
        int ammountOfMainRooms = 4 * (AllTiles.Count / 10);
        int id = 0;

        for (int i = AllTiles.Count - 1; i > AllTiles.Count - ammountOfMainRooms - 1; i--)
        {
            AllTiles[i].ID = id;

            if(AllTiles[i].TileType == TileType.Corridor)
                AllTiles[i].SetTileType (TileType.Normal);

            mainRooms.Add (AllTiles[i]);
            id++;
        }

        foreach (Tile tile in AllTiles)
        {
            if ((tile.TileType == TileType.Cheast || tile.TileType == TileType.Heal || tile.TileType == TileType.Start) && tile.ID == -1)
            {
                tile.ID = id;
                mainRooms.Add (tile);
                id++;
            }
            else if (tile.TileType == TileType.Corridor)
            {
                Destroy (tile.ObjectInGame);
            }
        }

        tileGraph = new TileGraph (mainRooms);
    }

    private void generateCorridors (TileGraph tileGraph)
    {
        Tile startTile, endTile;

        float corridorX;
        float corridorXLeft;
        float corridorXRight;
        float maxStartX;
        float minEndX;

        float corridorY;
        float corridorYUp;
        float corridorYDown;
        float maxStartY;
        float minEndY;
        float distance;
        Vector3 center;

        GameObject corridorGameObject;
        listoOFCorridors = new List<Corridor>();

        foreach (Edge edge in tileGraph.Edges)
        {
            startTile = tileGraph.Tiles[edge.Start];
            endTile = tileGraph.Tiles[edge.End];

            maxStartX = Mathf.Max (startTile.X, endTile.X);
            minEndX = Mathf.Min (startTile.X + startTile.Width, endTile.X + endTile.Width);

            minEndY = Mathf.Max (startTile.Y - startTile.Hight, endTile.Y - endTile.Hight);
            maxStartY = Mathf.Min (startTile.Y, endTile.Y);

            if (maxStartX < minEndX && startTile.ID != endTile.ID)
            {
                corridorX = Random.Range (maxStartX, minEndX);

                if (startTile.Y > endTile.Y)
                {
                    corridorYUp = startTile.Y - startTile.Hight + 3;
                    corridorYDown = endTile.Y - 3;
                }
                else
                {
                    corridorYUp = endTile.Y - endTile.Hight + 3;
                    corridorYDown = startTile.Y - 3;
                }

                distance = Vector3.Distance (new Vector3 (corridorX, corridorYUp), new Vector3 (corridorX, corridorYDown));
                center = (new Vector3 (corridorX, corridorYUp) + new Vector3 (corridorX, corridorYDown)) / 2f;

                corridorGameObject = Corridor.crateStraightCorridorObject (true, center, distance);

                listoOFCorridors.Add (new Corridor (corridorGameObject, startTile, endTile, CorridorType.Horizontal, listoOFCorridors.Count));
            }
            else if (maxStartY > minEndY && startTile.ID != endTile.ID)
            {
                corridorY = Random.Range (minEndY, maxStartY);

                if (startTile.X < endTile.X)
                {
                    corridorXLeft = startTile.X + startTile.Width - 3;
                    corridorXRight = endTile.X + 3;
                }
                else
                {
                    corridorXLeft = endTile.X + endTile.Width - 3;
                    corridorXRight = startTile.X + 3;
                }

                distance = Vector3.Distance (new Vector3 (corridorXLeft, corridorY), new Vector3 (corridorXRight, corridorY));
                center = (new Vector3 (corridorXLeft, corridorY) + new Vector3 (corridorXRight, corridorY)) / 2f;

                corridorGameObject = Corridor.crateStraightCorridorObject (false, center, distance);

                listoOFCorridors.Add (new Corridor (corridorGameObject, startTile, endTile, CorridorType.Vertical, listoOFCorridors.Count));
            }
            else
            {
                CreateLShapedCorridor (startTile, endTile);
            }
        }
    }

    public void CreateLShapedCorridor (Tile startTile, Tile endTile)
    {
        float distance;
        Vector3 center;

        int randomMinus;
        int rX, rY;
        GameObject verticalPart, horizontalPart, parent;

        if (startTile.X > endTile.X && startTile.Y < endTile.Y)         // Left up
        {
            randomMinus = Random.Range (0, 2) * 2 - 1;
            parent = new GameObject ();

            if (randomMinus > 0)
            {
                rX = (int)Random.Range (startTile.X + 3, startTile.X + startTile.Width - 3);
                rY = (int)Random.Range (endTile.Y - endTile.Hight + 3, endTile.Y - 3);
                //Debug.Log ("Punkt ³¹czenia dla + (" + rX.ToString () + ", " + rY.ToString () + ")");

                distance = Vector3.Distance (new Vector3 (rX, (int)startTile.Y - 3), new Vector3 (rX, rY + 1.5f));
                center = (new Vector3 (rX, (int)startTile.Y - 3) + new Vector3 (rX, rY + 1.5f)) / 2f;

                horizontalPart = Corridor.crateStraightCorridorObject (true, center, distance);
                horizontalPart.transform.SetParent (parent.transform);

                distance = Vector3.Distance (new Vector3 ((int)endTile.X + endTile.Width - 3, rY), new Vector3 (rX, rY));
                center = (new Vector3 ((int)endTile.X + endTile.Width - 3, rY) + new Vector3 (rX, rY)) / 2f;

                verticalPart = Corridor.crateStraightCorridorObject (false, center, distance);
                verticalPart.transform.SetParent (parent.transform);

                helpGameObject.Add (parent);

                listoOFCorridors.Add (new Corridor (verticalPart, horizontalPart, startTile, endTile, CorridorType.Lshape_Up_Left, listoOFCorridors.Count));
                listoOFCorridors.Add (new Corridor (horizontalPart, listoOFCorridors.Count));
            }
            else
            {
                rX = (int)Random.Range (endTile.X + 3, endTile.X + endTile.Width - 3);
                rY = (int)Random.Range (startTile.Y - startTile.Hight + 3, startTile.Y - 3);
                //Debug.Log ("Punkt ³¹czenia dla - (" + rX.ToString () + ", " + rY.ToString () + ")");

                distance = Vector3.Distance (new Vector3 (rX, (int)endTile.Y - endTile.Hight + 3), new Vector3 (rX, rY - 1.5f));
                center = (new Vector3 (rX, (int)endTile.Y - endTile.Hight + 3) + new Vector3 (rX, rY - 1.5f)) / 2f;

                horizontalPart = Corridor.crateStraightCorridorObject (true, center, distance);
                horizontalPart.transform.SetParent (parent.transform);

                distance = Vector3.Distance (new Vector3 ((int)startTile.X + 3, rY), new Vector3 (rX, rY));
                center = (new Vector3 ((int)startTile.X + 3, rY) + new Vector3 (rX, rY)) / 2f;

                verticalPart = Corridor.crateStraightCorridorObject (false, center, distance);
                verticalPart.transform.SetParent (parent.transform);

                helpGameObject.Add (parent);

                listoOFCorridors.Add (new Corridor (verticalPart, horizontalPart, startTile, endTile, CorridorType.Lshape_Up_Left, listoOFCorridors.Count));
                listoOFCorridors.Add (new Corridor (horizontalPart, listoOFCorridors.Count));
            }
        }
        else if (startTile.X > endTile.X && startTile.Y > endTile.Y)    // Left down
        {
            randomMinus = Random.Range (0, 2) * 2 - 1;
            parent = new GameObject ();

            if (randomMinus > 0)
            {
                rX = (int)Random.Range (startTile.X + 3, startTile.X + startTile.Width - 3);
                rY = (int)Random.Range (endTile.Y - endTile.Hight + 3, endTile.Y - 3);
                //Debug.Log ("Punkt ³¹czenia dla + (" + rX.ToString () + ", " + rY.ToString () + ")");

                distance = Vector3.Distance (new Vector3 (rX, (int)startTile.Y - startTile.Hight + 3), new Vector3 (rX, rY - 1.5f));
                center = (new Vector3 (rX, (int)startTile.Y - startTile.Hight + 3) + new Vector3 (rX, rY - 1.5f)) / 2f;

                horizontalPart = Corridor.crateStraightCorridorObject (true, center, distance);
                horizontalPart.transform.SetParent (parent.transform);

                distance = Vector3.Distance (new Vector3 ((int)endTile.X + endTile.Width - 3, rY), new Vector3 (rX, rY));
                center = (new Vector3 ((int)endTile.X + endTile.Width - 3, rY) + new Vector3 (rX, rY)) / 2f;

                verticalPart = Corridor.crateStraightCorridorObject (false, center, distance);
                verticalPart.transform.SetParent (parent.transform);

                helpGameObject.Add (parent);

                listoOFCorridors.Add (new Corridor (verticalPart, horizontalPart, startTile, endTile, CorridorType.Lshape_Down_Left, listoOFCorridors.Count));
                listoOFCorridors.Add (new Corridor (horizontalPart, listoOFCorridors.Count));
            }
            else
            {
                rX = (int)Random.Range (endTile.X + 3, endTile.X + endTile.Width - 3);
                rY = (int)Random.Range (startTile.Y - startTile.Hight + 3, startTile.Y - 3);
                //Debug.Log ("Punkt ³¹czenia dla - (" + rX.ToString () + ", " + rY.ToString () + ")");

                distance = Vector3.Distance (new Vector3 (rX, (int)endTile.Y - 3), new Vector3 (rX, rY + 1.5f));
                center = (new Vector3 (rX, (int)endTile.Y - 3) + new Vector3 (rX, rY + 1.5f)) / 2f;

                horizontalPart = Corridor.crateStraightCorridorObject (true, center, distance);
                horizontalPart.transform.SetParent (parent.transform);

                distance = Vector3.Distance (new Vector3 ((int)startTile.X + 3, rY), new Vector3 (rX, rY));
                center = (new Vector3 ((int)startTile.X + 3, rY) + new Vector3 (rX, rY)) / 2f;

                verticalPart = Corridor.crateStraightCorridorObject (false, center, distance);
                verticalPart.transform.SetParent (parent.transform);

                helpGameObject.Add (parent);

                listoOFCorridors.Add (new Corridor (verticalPart, horizontalPart, startTile, endTile, CorridorType.Lshape_Down_Left, listoOFCorridors.Count));
                listoOFCorridors.Add (new Corridor (horizontalPart, listoOFCorridors.Count));
            }
        }
        else if (startTile.X < endTile.X && startTile.Y < endTile.Y)    // Right up
        {
            randomMinus = Random.Range (0, 2) * 2 - 1;
            parent = new GameObject ();

            if (randomMinus > 0)
            {
                rX = (int)Random.Range (startTile.X + 3, startTile.X + startTile.Width - 3);
                rY = (int)Random.Range (endTile.Y - endTile.Hight + 3, endTile.Y - 3);
                //Debug.Log ("Punkt ³¹czenia dla + (" + rX.ToString () + ", " + rY.ToString () + ")");

                distance = Vector3.Distance (new Vector3 (rX, (int)startTile.Y - 3), new Vector3 (rX, rY + 1.5f));
                center = (new Vector3 (rX, (int)startTile.Y - 3) + new Vector3 (rX, rY + 1.5f)) / 2f;

                horizontalPart = Corridor.crateStraightCorridorObject (true, center, distance);
                horizontalPart.transform.SetParent (parent.transform);

                distance = Vector3.Distance (new Vector3 ((int)endTile.X + 3, rY), new Vector3 (rX, rY));
                center = (new Vector3 ((int)endTile.X + 3, rY) + new Vector3 (rX, rY)) / 2f;

                verticalPart = Corridor.crateStraightCorridorObject (false, center, distance);
                verticalPart.transform.SetParent (parent.transform);

                helpGameObject.Add (parent);

                listoOFCorridors.Add (new Corridor (verticalPart, horizontalPart, startTile, endTile, CorridorType.Lshape_Up_Right, listoOFCorridors.Count));
                listoOFCorridors.Add (new Corridor (horizontalPart, listoOFCorridors.Count));
            }
            else
            {
                rX = (int)Random.Range (endTile.X + 3, endTile.X + endTile.Width - 3);
                rY = (int)Random.Range (startTile.Y - startTile.Hight + 3, startTile.Y - 3);
                //Debug.Log ("Punkt ³¹czenia dla - (" + rX.ToString () + ", " + rY.ToString () + ")");

                distance = Vector3.Distance (new Vector3 (rX, (int)endTile.Y - endTile.Hight + 3), new Vector3 (rX, rY - 1.5f));
                center = (new Vector3 (rX, (int)endTile.Y - endTile.Hight + 3) + new Vector3 (rX, rY - 1.5f)) / 2f;

                horizontalPart = Corridor.crateStraightCorridorObject (true, center, distance);
                horizontalPart.transform.SetParent (parent.transform);

                distance = Vector3.Distance (new Vector3 ((int)startTile.X + startTile.Width - 3, rY), new Vector3 (rX, rY));
                center = (new Vector3 ((int)startTile.X + startTile.Width - 3, rY) + new Vector3 (rX, rY)) / 2f;

                verticalPart = Corridor.crateStraightCorridorObject (false, center, distance);
                verticalPart.transform.SetParent (parent.transform);

                helpGameObject.Add (parent);

                listoOFCorridors.Add (new Corridor (verticalPart, horizontalPart, startTile, endTile, CorridorType.Lshape_Up_Right, listoOFCorridors.Count));
                listoOFCorridors.Add (new Corridor (horizontalPart, listoOFCorridors.Count));
            }
        }
        else if (startTile.X < endTile.X && startTile.Y > endTile.Y)    // Right down
        {
            randomMinus = Random.Range (0, 2) * 2 - 1;
            parent = new GameObject ();

            if (randomMinus > 0)
            {
                rX = (int)Random.Range (startTile.X + 3, startTile.X + startTile.Width - 3);
                rY = (int)Random.Range (endTile.Y - endTile.Hight + 3, endTile.Y - 3);
                //Debug.Log ("Punkt ³¹czenia dla + (" + rX.ToString () + ", " + rY.ToString () + ")");

                distance = Vector3.Distance (new Vector3 (rX, (int)startTile.Y - startTile.Hight + 3), new Vector3 (rX, rY - 1.5f));
                center = (new Vector3 (rX, (int)startTile.Y - startTile.Hight + 3) + new Vector3 (rX, rY - 1.5f)) / 2f;

                horizontalPart = Corridor.crateStraightCorridorObject (true, center, distance);
                horizontalPart.transform.SetParent (parent.transform);

                distance = Vector3.Distance (new Vector3 ((int)endTile.X + 3, rY), new Vector3 (rX, rY));
                center = (new Vector3 ((int)endTile.X + 3, rY) + new Vector3 (rX, rY)) / 2f;

                verticalPart = Corridor.crateStraightCorridorObject (false, center, distance);
                verticalPart.transform.SetParent (parent.transform);

                helpGameObject.Add (parent);

                listoOFCorridors.Add (new Corridor (verticalPart, horizontalPart, startTile, endTile, CorridorType.Lshape_Down_Right, listoOFCorridors.Count));
                listoOFCorridors.Add (new Corridor (horizontalPart, listoOFCorridors.Count));
            }
            else
            {
                rX = (int)Random.Range (endTile.X + 3, endTile.X + endTile.Width - 3);
                rY = (int)Random.Range (startTile.Y - startTile.Hight + 3, startTile.Y - 3);
                //Debug.Log ("Punkt ³¹czenia dla - (" + rX.ToString () + ", " + rY.ToString () + ")");

                distance = Vector3.Distance (new Vector3 (rX, (int)endTile.Y - 3), new Vector3 (rX, rY + 1.5f));
                center = (new Vector3 (rX, (int)endTile.Y - 3) + new Vector3 (rX, rY + 1.5f)) / 2f;

                horizontalPart = Corridor.crateStraightCorridorObject (true, center, distance);
                horizontalPart.transform.SetParent (parent.transform);

                distance = Vector3.Distance (new Vector3 ((int)startTile.X + startTile.Width - 3, rY), new Vector3 (rX, rY));
                center = (new Vector3 ((int)startTile.X + startTile.Width - 3, rY) + new Vector3 (rX, rY)) / 2f;

                verticalPart = Corridor.crateStraightCorridorObject (false, center, distance);
                verticalPart.transform.SetParent (parent.transform);

                helpGameObject.Add (parent);

                listoOFCorridors.Add (new Corridor (verticalPart, horizontalPart, startTile, endTile, CorridorType.Lshape_Down_Right, listoOFCorridors.Count));
                listoOFCorridors.Add (new Corridor (horizontalPart, listoOFCorridors.Count));
            }
        }
    }
}
