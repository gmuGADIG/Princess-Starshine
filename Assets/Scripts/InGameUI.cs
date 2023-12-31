using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private static InGameUI instance;

    [SerializeField] float secondHealthBarDelay = 1f;
    [SerializeField] float secondHealthBarCorrectionSpeed = 1f;
    [SerializeField] Image hpBarMask;
    [SerializeField] Image secondHPBarMask;
    [SerializeField] Image xpBarMask;
    [SerializeField] TextMeshProUGUI xpBarText;
    [SerializeField] Transform weapons;
    [SerializeField] Transform passives;
    [SerializeField] Texture emptyItem;
    
    [Header("Twirls")]
    [SerializeField] Transform twirlParent;
    [SerializeField] Sprite twirlFilledImage;
    [SerializeField] Sprite twirlEmptyImage;

    float secondHealthBarTarget = 1;
    
    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        instance.secondHPBarMask.fillAmount = Mathf.Lerp(instance.secondHPBarMask.fillAmount, secondHealthBarTarget, secondHealthBarCorrectionSpeed * Time.deltaTime);
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
        if (instance.isActiveAndEnabled)
        {
            instance.hpBarMask.fillAmount = fractionalHp;
            instance.StartCoroutine(SetSecondHP(fractionalHp));
        }
    }

    public static IEnumerator SetSecondHP(float fractionalHp)
    {
        yield return new WaitForSeconds(instance.secondHealthBarDelay);
        instance.secondHealthBarTarget = fractionalHp;
    }

    private static void SetItemList(Transform group, List<Texture> icons)
    {
        for (int idx = 0; idx < group.childCount; idx++) {
            var image = group.GetChild(idx).GetComponent<RawImage>();
            image.texture = idx < icons.Count ? icons[idx] : instance.emptyItem;
        }
    }
}
