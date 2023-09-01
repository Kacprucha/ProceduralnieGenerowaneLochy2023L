using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelControler : MonoBehaviour
{
    [SerializeField]
    PlayerInfo playerInfo;

    [SerializeField]
    Image head;
    Image headShadow = null;
    IteamInfo headInfo = null;

    [SerializeField]
    Image torso;
    Image torsoShadow = null;
    IteamInfo torsoInfo = null;

    [SerializeField]
    Image leftArm;
    [SerializeField]
    Image leftShadow;
    [SerializeField]
    IteamInfo leftInfo;

    [SerializeField]
    Image rightArm;
    Image rightShadow = null;
    IteamInfo rightInfo = null;

    [SerializeField]
    Image legs;
    Image legsShadow = null;
    IteamInfo legsInfo = null;

    [SerializeField]
    Text atackLabel;

    [SerializeField]
    Text defenceLabel;

    int actualDef = 0;
    int actualAtack = 5;

    public void EquipNewIteam (IteamInfo iteamInfo, Sprite image, Image shadow)
    {
        switch (iteamInfo.Placement)
        {
            case EquipmentPlacement.Helmet:

                if (headShadow == null)
                {
                    headShadow = shadow;
                    headShadow.enabled = true;
                    headInfo = iteamInfo;
                }
                else
                {
                    if (headInfo != null)
                    {
                        actualAtack -= headInfo.DMG;
                        actualDef -= headInfo.DEF;
                    }

                    headShadow.enabled = false;

                    headShadow = shadow;
                    headInfo = iteamInfo;

                    headShadow.enabled = true;
                }

                head.sprite = image;
                head.enabled = true;
                break;

            case EquipmentPlacement.Armor:

                if (torsoShadow == null)
                {
                    torsoShadow = shadow;
                    torsoShadow.enabled = true;
                    torsoInfo = iteamInfo;
                }
                else
                {
                    if (torsoInfo != null)
                    {
                        actualAtack -= torsoInfo.DMG;
                        actualDef -= torsoInfo.DEF;
                    }

                    torsoShadow.enabled = false;

                    torsoShadow = shadow;
                    torsoInfo = iteamInfo;

                    torsoShadow.enabled = true;
                }

                torso.sprite = image;
                torso.enabled = true;
                break;

            case EquipmentPlacement.Main_Wepon:

                if (leftShadow == null)
                {
                    leftShadow = shadow;
                    leftShadow.enabled = true;
                    leftInfo = iteamInfo;
                }
                else
                {
                    if (leftInfo != null)
                    {
                        actualAtack -= leftInfo.DMG;
                        actualDef -= leftInfo.DEF;
                    }

                    leftShadow.enabled = false;

                    leftShadow = shadow;
                    leftInfo = iteamInfo;

                    leftShadow.enabled = true;
                }

                leftArm.sprite = image;
                leftArm.enabled = true;
                break;

            case EquipmentPlacement.Side:

                if (rightShadow == null)
                {
                    rightShadow = shadow;
                    rightShadow.enabled = true;
                    rightInfo = iteamInfo;
                }
                else
                {
                    if (rightInfo != null)
                    {
                        actualAtack -= rightInfo.DMG;
                        actualDef -= rightInfo.DEF;
                    }

                    rightShadow.enabled = false;

                    rightShadow = shadow;
                    rightInfo = iteamInfo;

                    rightShadow.enabled = true;
                }

                rightArm.sprite = image;
                rightArm.enabled = true;
                break;

            case EquipmentPlacement.Boots:

                if (legsShadow == null)
                {
                    legsShadow = shadow;
                    legsShadow.enabled = true;
                    legsInfo = iteamInfo;
                }
                else
                {
                    if (legsInfo != null)
                    {
                        actualAtack -= legsInfo.DMG;
                        actualDef -= legsInfo.DEF;
                    }

                    legsShadow.enabled = false;

                    legsShadow = shadow;
                    legsInfo = iteamInfo;

                    legsShadow.enabled = true;
                }

                legs.sprite = image;
                legs.enabled = true;
                break;
        }

        actualAtack += iteamInfo.DMG;
        actualDef += iteamInfo.DEF;

        atackLabel.text = actualAtack.ToString();
        defenceLabel.text = actualDef.ToString();

        if (playerInfo != null)
        {
            playerInfo.Damage = actualAtack;
            playerInfo.Defence = actualDef;
        }
    }

    public bool HealByEating (IteamInfo iteamInfo)
    {
        bool result = false;

        if (playerInfo.AcctualHealth () != 100)
        {
            playerInfo.CheangeHealth (iteamInfo.HP);
            result = true;
        }

        return result;
    }
}
