using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePositionCommponent : MonoBehaviour
{
    public Tile StartTile;

    public Tile EndTile;

    public CorridorType CorridorType;

    public Vector3 PastPosition;

    public GameObject LShapedCorridor;

    public bool IsItMoving = true;

    public bool MovedBecouseOfCorridor = false;

    private double amountOfTriesToChangeLocation = 0;
    private double ammountOfStaticFrames = 0;

    private int corridorSize = 3;

    void Update ()
    {
        if (transform.hasChanged && MovedBecouseOfCorridor)
        {
            transform.position = PastPosition;
            MovedBecouseOfCorridor = false;

            ammountOfStaticFrames += 0.25;
        }

        if (transform.hasChanged)
        {
            transform.hasChanged = false;
        }
        else if (ammountOfStaticFrames < 100)
        {
            ammountOfStaticFrames++;
        }

        if (ammountOfStaticFrames >= 100)
        {
            IsItMoving = false;
            GetComponent<SpriteRenderer> ().color = Color.green;

            if (transform.parent != null)
            {
                transform.parent.GetChild (0).GetComponent<Rigidbody2D> ().isKinematic = true;
                transform.parent.GetChild (1).GetComponent<Rigidbody2D> ().isKinematic = true;
            }
        }
    }

    void OnCollisionStay2D (Collision2D collision)
    {
        float corridorX, corridorXLeft, corridorXRight, maxStartX, minEndX;

        float corridorY, corridorYUp, corridorYDown, maxStartY, minEndY;

        int rX, rY, randomMinus;
        Transform parent;

        float distance;
        Vector3 center;

        if (collision.gameObject.tag == "Tile" && collision.gameObject != StartTile.ObjectInGame && collision.gameObject != EndTile.ObjectInGame)
        {
            if (amountOfTriesToChangeLocation < 100)
            {
                ammountOfStaticFrames = 0;

                maxStartX = Mathf.Max (StartTile.X, EndTile.X);
                minEndX = Mathf.Min (StartTile.X + StartTile.Width, EndTile.X + EndTile.Width);

                minEndY = Mathf.Max (StartTile.Y - StartTile.Hight, EndTile.Y - EndTile.Hight);
                maxStartY = Mathf.Min (StartTile.Y, EndTile.Y);

                switch (CorridorType)
                {
                    case CorridorType.Horizontal:

                        amountOfTriesToChangeLocation++;

                        corridorX = Random.Range (maxStartX, minEndX);

                        if (StartTile.Y > EndTile.Y)
                        {
                            corridorYUp = StartTile.Y - StartTile.Hight + 3;
                            corridorYDown = EndTile.Y - 3;
                        }
                        else
                        {
                            corridorYUp = EndTile.Y - EndTile.Hight + 3;
                            corridorYDown = StartTile.Y - 3;
                        }

                        distance = Vector3.Distance (new Vector3 (corridorX, corridorYUp), new Vector3 (corridorX, corridorYDown));
                        center = (new Vector3 (corridorX, corridorYUp) + new Vector3 (corridorX, corridorYDown)) / 2f;

                        transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                        transform.position = center;

                        PastPosition = transform.position;
                        MovedBecouseOfCorridor = false;

                        break;

                    case CorridorType.Vertical:

                        amountOfTriesToChangeLocation++;

                        corridorY = Random.Range (minEndY, maxStartY);

                        if (StartTile.X < EndTile.X)
                        {
                            corridorXLeft = StartTile.X + StartTile.Width - 3;
                            corridorXRight = EndTile.X + 3;
                        }
                        else
                        {
                            corridorXLeft = EndTile.X + EndTile.Width - 3;
                            corridorXRight = StartTile.X + 3;
                        }

                        distance = Vector3.Distance (new Vector3 (corridorXLeft, corridorY), new Vector3 (corridorXRight, corridorY));
                        center = (new Vector3 (corridorXLeft, corridorY) + new Vector3 (corridorXRight, corridorY)) / 2f;

                        transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                        transform.position = center;

                        PastPosition = transform.position;
                        MovedBecouseOfCorridor = false;

                        break;

                    case CorridorType.Lshape_Up_Left:

                        amountOfTriesToChangeLocation += 0.5;
                        randomMinus = Random.Range (0, 2) * 2 - 1;
                        parent = transform.parent.transform;

                        if (randomMinus > 0)
                        {
                            rX = (int)Random.Range (StartTile.X + 3, StartTile.X + StartTile.Width - 3);
                            rY = (int)Random.Range (EndTile.Y - EndTile.Hight + 3, EndTile.Y - 3);
                            //Debug.Log ("Punkt ³¹czenia dla + (" + rX.ToString () + ", " + rY.ToString () + ")");

                            distance = Vector3.Distance (new Vector3 (rX, (int)StartTile.Y - 3), new Vector3 (rX, rY + 1.5f));
                            center = (new Vector3 (rX, (int)StartTile.Y - 3) + new Vector3 (rX, rY + 1.5f)) / 2f;

                            parent.GetChild (0).transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                            parent.GetChild (0).transform.position = center;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (0).transform.position;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;


                            distance = Vector3.Distance (new Vector3 ((int)EndTile.X + EndTile.Width - 3, rY), new Vector3 (rX, rY));
                            center = (new Vector3 ((int)EndTile.X + EndTile.Width - 3, rY) + new Vector3 (rX, rY)) / 2f;

                            parent.GetChild (1).transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                            parent.GetChild (1).transform.position = center;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (1).transform.position;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;
                        }
                        else
                        {
                            rX = (int)Random.Range (EndTile.X + 3, EndTile.X + EndTile.Width - 3);
                            rY = (int)Random.Range (StartTile.Y - StartTile.Hight + 3, StartTile.Y - 3);
                            //Debug.Log ("Punkt ³¹czenia dla - (" + rX.ToString () + ", " + rY.ToString () + ")");

                            distance = Vector3.Distance (new Vector3 (rX, (int)EndTile.Y - EndTile.Hight + 3), new Vector3 (rX, rY - 1.5f));
                            center = (new Vector3 (rX, (int)EndTile.Y - EndTile.Hight + 3) + new Vector3 (rX, rY - 1.5f)) / 2f;

                            parent.GetChild (0).transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                            parent.GetChild (0).transform.position = center;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (0).transform.position;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;

                            distance = Vector3.Distance (new Vector3 ((int)StartTile.X + 3, rY), new Vector3 (rX, rY));
                            center = (new Vector3 ((int)StartTile.X + 3, rY) + new Vector3 (rX, rY)) / 2f;

                            parent.GetChild (1).transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                            parent.GetChild (1).transform.position = center;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (1).transform.position;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;
                        }

                        break;

                    case CorridorType.Lshape_Up_Right:

                        amountOfTriesToChangeLocation += 0.5;
                        randomMinus = Random.Range (0, 2) * 2 - 1;
                        parent = transform.parent.transform;

                        if (randomMinus > 0)
                        {
                            rX = (int)Random.Range (StartTile.X + 3, StartTile.X + StartTile.Width - 3);
                            rY = (int)Random.Range (EndTile.Y - EndTile.Hight + 3, EndTile.Y - 3);
                            //Debug.Log ("Punkt ³¹czenia dla + (" + rX.ToString () + ", " + rY.ToString () + ")");

                            distance = Vector3.Distance (new Vector3 (rX, (int)StartTile.Y - 3), new Vector3 (rX, rY + 1.5f));
                            center = (new Vector3 (rX, (int)StartTile.Y - 3) + new Vector3 (rX, rY + 1.5f)) / 2f;

                            parent.GetChild (0).transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                            parent.GetChild (0).transform.position = center;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (0).transform.position;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;


                            distance = Vector3.Distance (new Vector3 ((int)EndTile.X + 3, rY), new Vector3 (rX, rY));
                            center = (new Vector3 ((int)EndTile.X + 3, rY) + new Vector3 (rX, rY)) / 2f;

                            parent.GetChild (1).transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                            parent.GetChild (1).transform.position = center;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (1).transform.position;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;
                        }
                        else
                        {
                            rX = (int)Random.Range (EndTile.X + 3, EndTile.X + EndTile.Width - 3);
                            rY = (int)Random.Range (StartTile.Y - StartTile.Hight + 3, StartTile.Y - 3);
                            //Debug.Log ("Punkt ³¹czenia dla - (" + rX.ToString () + ", " + rY.ToString () + ")");

                            distance = Vector3.Distance (new Vector3 (rX, (int)EndTile.Y - EndTile.Hight + 3), new Vector3 (rX, rY - 1.5f));
                            center = (new Vector3 (rX, (int)EndTile.Y - EndTile.Hight + 3) + new Vector3 (rX, rY - 1.5f)) / 2f;

                            parent.GetChild (0).transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                            parent.GetChild (0).transform.position = center;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (0).transform.position;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;

                            distance = Vector3.Distance (new Vector3 ((int)StartTile.X + StartTile.Width - 3, rY), new Vector3 (rX, rY));
                            center = (new Vector3 ((int)StartTile.X + StartTile.Width - 3, rY) + new Vector3 (rX, rY)) / 2f;

                            parent.GetChild (1).transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                            parent.GetChild (1).transform.position = center;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (1).transform.position;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;
                        }

                        break;

                    case CorridorType.Lshape_Down_Left:

                        amountOfTriesToChangeLocation += 0.5;
                        randomMinus = Random.Range (0, 2) * 2 - 1;
                        parent = transform.parent.transform;

                        if (randomMinus > 0)
                        {
                            rX = (int)Random.Range (StartTile.X + 3, StartTile.X + StartTile.Width - 3);
                            rY = (int)Random.Range (EndTile.Y - EndTile.Hight + 3, EndTile.Y - 3);
                            //Debug.Log ("Punkt ³¹czenia dla + (" + rX.ToString () + ", " + rY.ToString () + ")");

                            distance = Vector3.Distance (new Vector3 (rX, (int)StartTile.Y - StartTile.Hight + 3), new Vector3 (rX, rY - 1.5f));
                            center = (new Vector3 (rX, (int)StartTile.Y - StartTile.Hight + 3) + new Vector3 (rX, rY - 1.5f)) / 2f;

                            parent.GetChild (0).transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                            parent.GetChild (0).transform.position = center;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (0).transform.position;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;


                            distance = Vector3.Distance (new Vector3 ((int)EndTile.X + EndTile.Width - 3, rY), new Vector3 (rX, rY));
                            center = (new Vector3 ((int)EndTile.X + EndTile.Width - 3, rY) + new Vector3 (rX, rY)) / 2f;

                            parent.GetChild (1).transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                            parent.GetChild (1).transform.position = center;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (1).transform.position;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;
                        }
                        else
                        {
                            rX = (int)Random.Range (EndTile.X + 3, EndTile.X + EndTile.Width - 3);
                            rY = (int)Random.Range (StartTile.Y - StartTile.Hight + 3, StartTile.Y - 3);
                            //Debug.Log ("Punkt ³¹czenia dla - (" + rX.ToString () + ", " + rY.ToString () + ")");

                            distance = Vector3.Distance (new Vector3 (rX, (int)EndTile.Y - 3), new Vector3 (rX, rY + 1.5f));
                            center = (new Vector3 (rX, (int)EndTile.Y - 3) + new Vector3 (rX, rY + 1.5f)) / 2f;

                            parent.GetChild (0).transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                            parent.GetChild (0).transform.position = center;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (0).transform.position;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;

                            distance = Vector3.Distance (new Vector3 ((int)StartTile.X + 3, rY), new Vector3 (rX, rY));
                            center = (new Vector3 ((int)StartTile.X + 3, rY) + new Vector3 (rX, rY)) / 2f;

                            parent.GetChild (1).transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                            parent.GetChild (1).transform.position = center;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (1).transform.position;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;
                        }

                        break;

                    case CorridorType.Lshape_Down_Right:

                        amountOfTriesToChangeLocation += 0.5;
                        randomMinus = Random.Range (0, 2) * 2 - 1;
                        parent = transform.parent.transform;

                        if (randomMinus > 0)
                        {
                            rX = (int)Random.Range (StartTile.X + 3, StartTile.X + StartTile.Width - 3);
                            rY = (int)Random.Range (EndTile.Y - EndTile.Hight + 3, EndTile.Y - 3);
                            //Debug.Log ("Punkt ³¹czenia dla + (" + rX.ToString () + ", " + rY.ToString () + ")");

                            distance = Vector3.Distance (new Vector3 (rX, (int)StartTile.Y - StartTile.Hight + 3), new Vector3 (rX, rY - 1.5f));
                            center = (new Vector3 (rX, (int)StartTile.Y - StartTile.Hight + 3) + new Vector3 (rX, rY - 1.5f)) / 2f;

                            parent.GetChild (0).transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                            parent.GetChild (0).transform.position = center;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (0).transform.position;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;


                            distance = Vector3.Distance (new Vector3 ((int)EndTile.X + 3, rY), new Vector3 (rX, rY));
                            center = (new Vector3 ((int)EndTile.X + 3, rY) + new Vector3 (rX, rY)) / 2f;

                            parent.GetChild (1).transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                            parent.GetChild (1).transform.position = center;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (1).transform.position;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;
                        }
                        else
                        {
                            rX = (int)Random.Range (EndTile.X + 3, EndTile.X + EndTile.Width - 3);
                            rY = (int)Random.Range (StartTile.Y - StartTile.Hight + 3, StartTile.Y - 3);
                            //Debug.Log ("Punkt ³¹czenia dla - (" + rX.ToString () + ", " + rY.ToString () + ")");

                            distance = Vector3.Distance (new Vector3 (rX, (int)EndTile.Y - 3), new Vector3 (rX, rY + 1.5f));
                            center = (new Vector3 (rX, (int)EndTile.Y - 3) + new Vector3 (rX, rY + 1.5f)) / 2f;

                            parent.GetChild (0).transform.localScale = new Vector3 (corridorSize, (int)distance, 0);
                            parent.GetChild (0).transform.position = center;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (0).transform.position;
                            parent.GetChild (0).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;

                            distance = Vector3.Distance (new Vector3 ((int)StartTile.X + StartTile.Width - 3, rY), new Vector3 (rX, rY));
                            center = (new Vector3 ((int)StartTile.X + StartTile.Width - 3, rY) + new Vector3 (rX, rY)) / 2f;

                            parent.GetChild (1).transform.localScale = new Vector3 ((int)distance, corridorSize, 0);
                            parent.GetChild (1).transform.position = center;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().PastPosition = parent.GetChild (1).transform.position;
                            parent.GetChild (1).GetComponent<ChangePositionCommponent> ().MovedBecouseOfCorridor = false;
                        }

                        break;
                }
            }
            else
            {
                if (gameObject != null && transform.parent == null)
                {
                    Destroy (gameObject);
                    RectangleGenerator rectangleGenerator = RectangleGenerator.RectangleGeneratorInstance;
                    rectangleGenerator.CreateLShapedCorridor (StartTile, EndTile);
                }
                else if (gameObject != null) 
                {
                    Destroy (transform.parent.transform.GetChild (0).gameObject);
                    Destroy (transform.parent.transform.GetChild (1).gameObject);
                }
            }
        }

        if (collision.gameObject.tag == "Corridor")
        {
            if (transform.parent == null)
            {
                MovedBecouseOfCorridor = true;
            } 
            else
            {
                if (collision.gameObject != transform.parent.transform.GetChild (0) && collision.gameObject != transform.parent.transform.GetChild (1))
                    MovedBecouseOfCorridor = true;
            }

            if (collision.gameObject.transform.parent == null && ammountOfStaticFrames >= -1)
                ammountOfStaticFrames -= 0.5;

        }
    }
}
