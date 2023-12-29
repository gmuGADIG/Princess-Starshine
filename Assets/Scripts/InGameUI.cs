using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private static InGameUI instance;

    [SerializeField] Image hpBarMask;
    [SerializeField] Image xpBarMask;
    [SerializeField] TextMeshProUGUI xpBarText;
    [SerializeField] Transform weapons;
    [SerializeField] Transform passives;
    
    [Header("Twirls")]
    [SerializeField] Transform twirlParent;
    [SerializeField] Sprite twirlFilledImage;
    [SerializeField] Sprite twirlEmptyImage;
    
    void Awake()
    {
        instance = this;
    }

    /**
     * Updates the xp bar based on the given values.
     * fractionalLevelProgress should be in the range 0 to 1.
     */
    public static void SetXp(int currentLevel, float fractionalLevelProgress)
    {
        instance.xpBarMask.fillAmount = fractionalLevelProgress;
        instance.xpBarText.text = "lvl " + currentLevel;
    }

    public static void UpdateItems()
    {
        SetItemList(instance.passives, EquipmentManager.instance.EquippedPassiveIcons());
        SetItemList(instance.weapons, EquipmentManager.instance.EquippedWeaponIcons());
    }

    public static void UpdateTwirls(int count)
    {
        // TODO: make this method cope with the count being higher than the number of children

        var twirlsDrawn = 0;
        foreach (Transform child in instance.twirlParent)
        {
            var image = child.GetComponent<Image>();
            image.sprite =
                twirlsDrawn < count
                ? instance.twirlFilledImage
                : instance.twirlEmptyImage;
            twirlsDrawn += 1;
        }
    }
    
    /**
     * Updates the hp bar based on the given values.
     * fractionalHp should be in the range 0 to 1.
     */
    public static void SetHp(float fractionalHp)
    {
        instance.hpBarMask.fillAmount = fractionalHp;
    }

    private static void SetItemList(Transform group, List<Texture> icons)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            var image = group.GetChild(i).GetComponent<RawImage>();
            image.texture = icons[i];
        }
    }
}
