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
    public Transform iconHolder;
    public bool openOnStart = true;

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
    }

    public void Start()
    {
        if (!openOnStart)
        {
            this.gameObject.SetActive(false);
            return;
        }
        
        this.gameObject.SetActive(true);
        Time.timeScale = 0;
        
        ShowOptions(EquipmentManager.instance.GetUpgradeOptions(true));
    }
    
    /**
     * Opens the level-up menu, pausing the game until the player selects one of the four generated upgrades.
     */
    public void Open()
    {
        this.gameObject.SetActive(true);
        this.menuParent.SetActive(true);
        Time.timeScale = 0;
        
        ShowOptions(EquipmentManager.instance.GetUpgradeOptions());
    }

    private void ShowOptions(List<UpgradeOption> upgradeOptions)
    {
        if (iconHolder == null) throw new Exception("LevelUpUI failed to find icon holder!");
        foreach (Transform icon in iconHolder.transform) Destroy(icon.gameObject); // destroy left-over icons
        foreach (var option in upgradeOptions)
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
                    InGameUI.UpdateItems();
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
