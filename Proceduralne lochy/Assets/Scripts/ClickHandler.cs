using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    IteamInfo iteamInfo;

    [SerializeField]
    Image icon;

    [SerializeField]
    Image shadow;

    [SerializeField]
    PlayerPanelControler playerPanelControler;

    public void OnPointerClick (PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (iteamInfo.Placement != EquipmentPlacement.Non)
            {
                playerPanelControler.EquipNewIteam (iteamInfo, icon.sprite, shadow);
            }
            else
            {
                if (playerPanelControler.HealByEating (iteamInfo))
                {
                    iteamInfo.Resetart ();
                    icon.sprite = null;
                    icon.gameObject.SetActive (false);
                }
            }
        }
    }
}
