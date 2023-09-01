using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Cheast = 0,
    Potion,
    Item,
    NoType,
    TrapDoor
}

public class CollisionCommponent : MonoBehaviour
{
    public ObjectType ObjectType = ObjectType.NoType;

    private RectangleGenerator rectangleGenerator;

    private void Start ()
    {
        rectangleGenerator = RectangleGenerator.RectangleGeneratorInstance;
    }

    void OnCollisionStay2D (Collision2D collision)
    {
        switch (ObjectType)
        {
            case ObjectType.Cheast:

                int itemGroup, specificItem;
                string folderPath, itemName = "Apple";

                int dmg , def, hp;
                EquipmentPlacement placement;

                for (int i = 0; i < 3; i++)
                {
                    dmg = 0;
                    def = 0;
                    hp = 0;
                    placement = EquipmentPlacement.Non;

                    itemGroup = (int)Random.Range (1, 100);
                    specificItem = (int)Random.Range (1, 100);

                    if (itemGroup < 51) // food group
                    {
                        folderPath = "_Sprites/Texture/Food/";
                        specificItem /= 10;

                        switch (specificItem)
                        {
                            case 0:

                                itemName = "Apple";
                                hp = 5;
                                break;

                            case 1:

                                itemName = "Beer";
                                hp = 10;
                                break;

                            case 2:

                                itemName = "Bread";
                                hp = 10;
                                break;

                            case 3:

                                itemName = "Cheese";
                                hp = 10;
                                break;

                            case 4:

                                itemName = "Fish Steak";
                                hp = 25;
                                break;

                            case 5:

                                itemName = "Green Apple";
                                hp = 5;
                                break;

                            case 6:

                                itemName = "Ham";
                                hp = 25;
                                break;

                            case 7:

                                itemName = "Meat";
                                hp = 25;
                                break;

                            case 8:

                                itemName = "Mushroom";
                                hp = 5;
                                break;

                            case 9:

                                itemName = "Wine";
                                hp = 20;
                                break;

                            case 10:

                                itemName = "Wine 2";
                                hp = 20;
                                break;
                        }
                    } 
                    else if (51 < itemGroup && itemGroup < 76) // eq group
                    {

                        folderPath = "_Sprites/Texture/Equipment/";

                        if (specificItem < 26)
                        {
                            itemName = "Wooden Armor";
                            def = 2;
                            placement = EquipmentPlacement.Armor;
                        }
                        else if (specificItem >= 26 && specificItem < 35)
                        {
                            itemName = "Leather Helmet";
                            def = 5;
                            placement = EquipmentPlacement.Helmet;
                        }
                        else if (specificItem >= 35 && specificItem < 43)
                        {
                            itemName = "Leather Boot";
                            def = 5;
                            placement = EquipmentPlacement.Boots;
                        }
                        else if (specificItem >= 43 && specificItem < 51)
                        {
                            itemName = "Leather Armor";
                            def = 5;
                            placement = EquipmentPlacement.Armor;
                        }
                        else if (specificItem >= 51 && specificItem < 60)
                        {
                            itemName = "Iron Helmet";
                            def = 10;
                            placement = EquipmentPlacement.Helmet;
                        }
                        else if (specificItem >= 60 && specificItem < 68)
                        {
                            itemName = "Iron Boot";
                            def = 10;
                            placement = EquipmentPlacement.Boots;
                        }
                        else if (specificItem >= 68 && specificItem < 76)
                        {
                            itemName = "Iron Armor";
                            def = 10;
                            placement = EquipmentPlacement.Armor;
                        }
                        else
                        {
                            itemName = "Helm";
                            def = 20;
                            placement = EquipmentPlacement.Helmet;
                        }
                    }
                    else // wepon group
                    {
                        folderPath = "_Sprites/Texture/Weapon & Tool/";

                        if (specificItem < 3)
                        {
                            itemName = "Knife";
                            dmg = 10;
                            placement = EquipmentPlacement.Side;
                        }
                        else if (specificItem >= 3 && specificItem < 7)
                        {
                            itemName = "Shovel";
                            dmg = 10;
                            placement = EquipmentPlacement.Main_Wepon;
                        }
                        else if (specificItem >= 7 && specificItem < 10)
                        {
                            itemName = "Pickaxe";
                            dmg = 10;
                            placement = EquipmentPlacement.Main_Wepon;
                        }
                        else if (specificItem >= 10 && specificItem < 21)
                        {
                            itemName = "Wooden Shield";
                            def = 2;
                            placement = EquipmentPlacement.Side;
                        }
                        else if (specificItem >= 21 && specificItem < 42)
                        {
                            itemName = "Hammer";
                            dmg = 15;
                            placement = EquipmentPlacement.Main_Wepon;
                        }
                        else if (specificItem >= 42 && specificItem < 63)
                        {
                            itemName = "Axe";
                            dmg = 15;
                            placement = EquipmentPlacement.Main_Wepon;
                        }
                        else if (specificItem >= 63 && specificItem < 74)
                        {
                            itemName = "Iron Sword";
                            dmg = 20;
                            placement = EquipmentPlacement.Main_Wepon;
                        }
                        else if (specificItem >= 74 && specificItem < 85)
                        {
                            itemName = "Iron Shield";
                            def = 10;
                            placement = EquipmentPlacement.Side;
                        }
                        else if (specificItem >= 85 && specificItem < 96)
                        {
                            itemName = "Golden Sword";
                            dmg = 25;
                            placement = EquipmentPlacement.Main_Wepon;
                        }
                        else
                        {
                            itemName = "Silver Sword";
                            dmg = 30;
                            placement = EquipmentPlacement.Main_Wepon;
                        }
                    }

                    string path = folderPath + itemName;

                    GameObject iteam = new GameObject ();
                    iteam.AddComponent<SpriteRenderer> ();
                    iteam.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (path);
                    iteam.GetComponent<SpriteRenderer> ().sortingOrder = 1;
                    iteam.AddComponent<Rigidbody2D> ();
                    iteam.GetComponent<Rigidbody2D> ().gravityScale = 0;
                    iteam.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
                    iteam.AddComponent<BoxCollider2D> ();
                    iteam.AddComponent<CollisionCommponent> ();
                    iteam.GetComponent<CollisionCommponent> ().ObjectType = ObjectType.Item;
                    iteam.AddComponent<IteamInfo> ();
                    iteam.GetComponent<IteamInfo> ().Placement = placement;
                    iteam.GetComponent<IteamInfo> ().DMG = dmg;
                    iteam.GetComponent<IteamInfo> ().DEF = def;
                    iteam.GetComponent<IteamInfo> ().HP = hp;

                    iteam.transform.position = this.gameObject.transform.position;
                }

                Destroy (this.gameObject);

                break;

            case ObjectType.Item:

                if (rectangleGenerator.Player.GetComponent<PlayerInfo> ().PutNewItemToEquipment (this.GetComponent<SpriteRenderer> ().sprite, 
                                                                                        this.GetComponent<IteamInfo> ()))
                {
                    ObjectType = ObjectType.NoType;
                    Destroy (this.gameObject);
                }

                break;

            case ObjectType.Potion:

                rectangleGenerator.Player.GetComponent<PlayerInfo> ().CheangeHealth (100);
                Destroy (this.gameObject);

                break;

            case ObjectType.TrapDoor:

                if (GameObject.Find ("Boss") == null)
                {
                    rectangleGenerator.GenerateTiles ();
                    Destroy (this.gameObject);
                }

                break;
        }
    }
}
