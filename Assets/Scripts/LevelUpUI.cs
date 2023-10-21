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
    public GameObject menuParent;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        Close();
    }

    /**
     * Opens the level-up menu, pausing the game until the player selects one of the four generated upgrades.
     */
    public void Open()
    {
        menuParent.SetActive(true);
        Time.timeScale = 0;
        
        var options = EquipmentManager.instance.GetUpgradeOptions();
        
        var iconHolder = transform.Find("EquipmentSelect");
        if (iconHolder != null)
        {
            foreach (Transform icon in iconHolder.transform)
            {
                Destroy(icon.gameObject); // destroy left-over icons
            }
        }
        foreach (var option in options)
        {
            var obj = Instantiate(iconPrefab, iconHolder);
            var name = obj.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var image = obj.transform.Find("Icon").GetComponent<RawImage>();
            var description = obj.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            
            name.text = option.name;
            image.texture = option.icon;
            description.text = option.description;

            obj.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    option.onSelect();
                    this.Close();
                }
            );
        }
    }

    private void Close()
    {
        Time.timeScale = 1;
        menuParent.SetActive(false);
    }
}
