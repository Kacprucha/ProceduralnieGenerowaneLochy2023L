using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    public GameObject HealthBar;

    [SerializeField]
    GameObject panelEq;

    [SerializeField]
    GameObject panelPlayer;

    [SerializeField]
    Text atackLabel;

    [SerializeField]
    Text defenceLabel;

    int startHealth = 100;
    public int Damage = 5;
    public int Defence = 0;

    public void CheangeHealth (int emount)
    {
        HealthBar.GetComponent<Slider> ().value += emount;

        if (HealthBar.GetComponent<Slider> ().value <= 0)
        {
            GameObject.Find ("CanvasControler").GetComponent<CanvaControler> ().defeatCanvas.SetActive (true);
        }
    }

    public bool PutNewItemToEquipment (Sprite sprite, IteamInfo iteamInfo)
    {
        bool result = false;

        for (int i = 0; i < panelEq.gameObject.transform.childCount; i++)
        {
            if (panelEq.gameObject.transform.GetChild (i).gameObject.transform.GetChild (1).GetComponent<Image> ().sprite == null)
            {
                panelEq.gameObject.transform.GetChild (i).gameObject.transform.GetChild (1).gameObject.SetActive (true);
                panelEq.gameObject.transform.GetChild (i).gameObject.transform.GetChild (1).GetComponent<Image> ().sprite = sprite;
                panelEq.gameObject.transform.GetChild (i).gameObject.transform.GetChild (1).GetComponent<Image> ().enabled = true;
                panelEq.gameObject.transform.GetChild (i).gameObject.transform.GetChild (1).GetComponent<IteamInfo> ().DEF = iteamInfo.DEF;
                panelEq.gameObject.transform.GetChild (i).gameObject.transform.GetChild (1).GetComponent<IteamInfo> ().DMG = iteamInfo.DMG;
                panelEq.gameObject.transform.GetChild (i).gameObject.transform.GetChild (1).GetComponent<IteamInfo> ().HP = iteamInfo.HP;
                panelEq.gameObject.transform.GetChild (i).gameObject.transform.GetChild (1).GetComponent<IteamInfo> ().Placement = iteamInfo.Placement;

                result = true;
                break;
            }
        }

        return result;
    }

    public float AcctualHealth ()
    {
        return HealthBar.GetComponent<Slider> ().value;
    }

    void OnCollisionStay2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Contour")
        {
            collision.gameObject.GetComponent<TileInfo> ().AnableEmemies ();
        }
    }
}
