using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfEquipment
{
    Non,
    Five_HP,
    Ten_HP,
    Twenty_HP,
    Tewntyfive_HP,
    Five_DMG,
    Ten_DMG,
    Fifteen_DMG,
    Twenty_DMG,
    Tewntyfive_DMG,
    Thirty_DMG,
    Two_DEF,
    Five_DEF,
    Ten_DEF,
    Twenty_DEF
}

public enum EquipmentPlacement
{
    Non,
    Main_Wepon,
    Side,
    Helmet,
    Boots,
    Armor
}

public class IteamInfo : MonoBehaviour
{
    [SerializeField]
    public bool Equiped = false;

    [SerializeField]
    public EquipmentPlacement Placement = EquipmentPlacement.Non;

    [SerializeField]
    public int DMG = 0;

    public int DEF = 0;
    public int HP = 0;

    public void Resetart ()
    {
        DEF = 0;
        DMG = 0;
        HP = 0;
        Equiped = false;
        Placement = EquipmentPlacement.Non;
    }
}
