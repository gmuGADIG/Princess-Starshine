using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public static LevelUpUI instance;

    public GameObject iconPrefab;

    LevelUpUI()
    {
        instance = this;
    }

    /**
     * Opens the level-up menu, pausing the game until the player selects one of the four generated upgrades.
     */
    public void Open()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0;
        
        var options = EquipmentManager.instance.GetUpgradeOptions();
        

        var iconHolder = transform.Find("EquipmentSelect");
        foreach (Transform icon in iconHolder.transform) Destroy(icon.gameObject); // destroy left-over icons
        foreach (var option in options)
        {
            var obj = Instantiate(iconPrefab, iconHolder);
            var name = obj.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var image = obj.transform.Find("Icon").GetComponent<RawImage>();
            var description = obj.transform.Find("Description").GetComponent<TextMeshProUGUI>();

            var icon = EquipmentManager.instance.GetIcon(option.equipment);
            name.text = icon.name;
            image.texture = icon.icon;
            
            if (option.isLevelUp)
            {
                description.text = "Item Level Up!";
                if (option.equipment is Weapon)
                {
                    description.text += "\n" + option.levelUps[0] + "\n" + option.levelUps[1];
                }
                else if (option.equipment is Passive passive)
                {
                    description.text += "\n" + passive.GetLevelUpDescription();
                }
            }
            else
            {
                description.text = icon.description;
            }

            obj.GetComponent<Button>().onClick.AddListener(() => SelectOption(option));
        }
    }

    private void Close()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }
    
    /**
     * Applies the option and closes the menu.
     * Called when an option is clicked by the player.
     */
    void SelectOption(UpgradeOption option)
    {
        print($"Applying {option.equipment.type}");
        EquipmentManager.instance.ApplyUpgradeOption(option);
        this.Close();
    }
}
