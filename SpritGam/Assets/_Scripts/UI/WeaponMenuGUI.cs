﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class WeaponMenuGUI : AbstractButtonMap {

    private WeaponStatConfig wsc;
    private BulletModule bulletModule;
    private PlayerMovement player_movement;

    [SerializeField] Text w_weapon_name;
    [SerializeField] Image w_weapon_image;
    [SerializeField] Text w_damage_num;
    [SerializeField] Text w_fireRate_num;
    [SerializeField] Text w_fireType_num;
    [SerializeField] Text w_accuracy_num;
    [SerializeField] Text w_critChance_num;
    [SerializeField] Text w_critDamage_num;

    [SerializeField] Image[] chipMod_images;
    [SerializeField] Text[] chipModOne_effects;
    [SerializeField] Text[] chipModTwo_effects;
    [SerializeField] Text[] chipModThree_effects;

    [SerializeField] Image b_bullet_image;
    [SerializeField] Text b_bullet_name;
    [SerializeField] Text b_manaCost_num;
    [SerializeField] Text b_speed_num;
    [SerializeField] Text b_effect_description;

    [SerializeField] Text[] bonus_stats;

    [SerializeField] Text dmg_increase_text;
    [SerializeField] Text fire_rate_increase_text;
    [SerializeField] Text accuracy_increase_text;
    [SerializeField] Text crit_chance_increase_text;
    [SerializeField] Text crit_multi_increase_text;

    //[SerializeField] GameObject 

    private float dmg_increase;
    private float fire_rate_increase;
    private float accuracy_increase;
    private float crit_chance_increase;
    private float crit_multi_increase;

    [SerializeField] GameObject mainInfoPanel;
    [SerializeField] GameObject bulletInventoryPanel;
    [SerializeField] GameObject chipModInventoryPanel;

    // CHIP MOD INVENTORY
    [SerializeField] Button chip_one_button;
    [SerializeField] Button chip_two_button;
    [SerializeField] Button chip_three_button;
    [SerializeField] Button bullet_button;
    [SerializeField] GameObject chipMod_inventory;
    [SerializeField] GameObject bullet_inventory;

    
    public GameObject current_selected_chip;
    [SerializeField] Image current_chip_sprite;

    [HideInInspector] public ChipMod chip_one;
    [HideInInspector] public ChipMod chip_two;
    [HideInInspector] public ChipMod chip_three;

    // Use this for initialization
    void OnEnable () {

        player_movement = GameObject.Find("config: player").GetComponent<PlayerMovement>();
        wsc = GameObject.Find("config: weapon").GetComponent<WeaponStatConfig>();
        bulletModule = wsc.GetComponent<BulletModule>();

        mainInfoPanel.SetActive(true);
        bulletInventoryPanel.SetActive(false);
        chipModInventoryPanel.SetActive(false);
        

        chip_one = wsc.chip_mods[0].GetComponent<ChipMod>();
        chip_two = wsc.chip_mods[1].GetComponent<ChipMod>();
        chip_three = wsc.chip_mods[2].GetComponent<ChipMod>();

        
        ResetStrings();

        w_weapon_image.sprite = GameObject.Find("Weapon").GetComponent<SpriteRenderer>().sprite;
        SetChipModDisplay();
        AddButtonListeners();
        
        SetCurrentBulletDisplay();
        CalculateStatIncreases();
        SetWeaponStatDisplay();
        
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons[0].Select();

    }

    void AddButtonListeners()
    {
        chip_one_button.onClick.AddListener(OpenChipModInventory);
        chip_two_button.onClick.AddListener(OpenChipModInventory);
        chip_three_button.onClick.AddListener(OpenChipModInventory);

        bullet_button.onClick.AddListener(OpenBulletInventory);
        
    }

    void OpenChipModInventory()
    {
        current_selected_chip = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;

        if (current_selected_chip.name == "ChipMod_1")
        {
            current_selected_chip = wsc.chip_mods[0];
        } else if (current_selected_chip.name == "ChipMod_2")
        {
            current_selected_chip = wsc.chip_mods[1];
        } else if (current_selected_chip.name == "ChipMod_3")
        {
            current_selected_chip = wsc.chip_mods[2];
        } else
        {
            Debug.Log("CHIP GUI NAME READ ERROR");
        }
        

        current_chip_sprite.sprite = current_selected_chip.transform.parent.GetComponentInChildren<SpriteRenderer>().sprite;

        //SetCurrentChipDisplay();

        mainInfoPanel.SetActive(false);
        chipMod_inventory.SetActive(true);
        
        
    }

    void SetCurrentChipDisplay()
    {
        //current_chip_sprite.sprite = current_selected_chip.GetComponentInChildren<Image>().sprite;
    }

    void OpenBulletInventory()
    {
        mainInfoPanel.SetActive(false);
        bullet_inventory.SetActive(true);
        Button[] buttons = bullet_inventory.GetComponentsInChildren<Button>();
        buttons[0].Select();

        
    }

    public override void OnPress_A()
    {
        Debug.Log("Pressed A");
    }

    public override void OnPress_B()
    {
        if (chipMod_inventory.activeInHierarchy || bullet_inventory.activeInHierarchy)
        {
            chipMod_inventory.SetActive(false);
            bullet_inventory.SetActive(false);
            mainInfoPanel.SetActive(true);
        }
        //chipMod_inventory.SetActive(!chipMod_inventory.activeInHierarchy);
    }



    private void ResetStrings()
    {
        for (int i = 0; i < chipModOne_effects.Length; i++)
        {
            chipModOne_effects[i].text = "";
        }

        for (int i = 0; i < chipModTwo_effects.Length; i++)
        {
            chipModTwo_effects[i].text = "";
        }

        for (int i = 0; i < chipModThree_effects.Length; i++)
        {
            chipModThree_effects[i].text = "";
        }

        for (int i = 0; i < bonus_stats.Length; i++)
        {
            bonus_stats[i].text = "";
        }

        dmg_increase_text.text = "";
        fire_rate_increase_text.text = "";
        accuracy_increase_text.text = "";
        crit_chance_increase_text.text = "";
        crit_multi_increase_text.text = "";
    }

    void SetChipModDisplay()
    {
        
        chipMod_images[0].sprite = wsc.chip_mods[0].GetComponent<SpriteRenderer>().sprite;
        chipMod_images[1].sprite = wsc.chip_mods[1].GetComponent<SpriteRenderer>().sprite;
        chipMod_images[2].sprite = wsc.chip_mods[2].GetComponent<SpriteRenderer>().sprite;

        for (int i = 0; i < chip_one.chipModStats.Length; i++)
        {
            chipModOne_effects[i].text = chip_one.chipModStats[i].type.ToString().Replace("_", " ").Replace("Chance", "%").Replace("Multiplier", "Dmg") + "  +" + chip_one.chipModStats[i].increase_amount.ToString().Replace("0.", ".");
        }

        for (int i = 0; i < chip_two.chipModStats.Length; i++)
        {
            chipModTwo_effects[i].text = chip_two.chipModStats[i].type.ToString().Replace("_", " ").Replace("Chance", "%").Replace("Multiplier", "Dmg") + "  +" + chip_two.chipModStats[i].increase_amount.ToString().Replace("0.", ".");
        }

        for (int i = 0; i < chip_three.chipModStats.Length; i++)
        {
            chipModThree_effects[i].text = chip_three.chipModStats[i].type.ToString().Replace("_", " ").Replace("Chance", "%").Replace("Multiplier", "Dmg") + "  +" + chip_three.chipModStats[i].increase_amount.ToString().Replace("0.", ".");
        }
        
        
    }

    public void SetWeaponStatDisplay()
    {
        w_weapon_name.text = wsc.weapon_name.ToString();
        
        w_damage_num.text = wsc.damage.ToString();
        w_fireRate_num.text = wsc.fire_rate.ToString();
        w_fireType_num.text = wsc.fire_type;
        w_accuracy_num.text = wsc.accuracy.ToString();
        w_critChance_num.text = wsc.crit_chance.ToString();
        w_critDamage_num.text = wsc.crit_multiplier.ToString();

        CalculateStatIncreases();

        if (dmg_increase > 0)
        {
            dmg_increase_text.text = "+" + dmg_increase.ToString();
        } else
        {
            dmg_increase_text.text = "";
        }
        if (fire_rate_increase > 0)
        {
            fire_rate_increase_text.text = "+" + fire_rate_increase.ToString();
        } else
        {
            fire_rate_increase_text.text = "";
        }
        if (accuracy_increase > 0)
        {
            accuracy_increase_text.text = "+" + accuracy_increase.ToString();
        } else
        {
            accuracy_increase_text.text = "";
        }
        if (crit_chance_increase > 0)
        {
            crit_chance_increase_text.text = "+" + crit_chance_increase.ToString();
        } else
        {
            crit_chance_increase_text.text = "";
        }
        if (crit_multi_increase > 0)
        {
            crit_multi_increase_text.text = "+" + crit_multi_increase.ToString();
        } else
        {
            crit_multi_increase_text.text = "";
        }
        
        
    }

    public void CalculateStatIncreases()
    {
        dmg_increase = wsc.damage - wsc.base_damage;
        fire_rate_increase = wsc.fire_rate - wsc.base_fire_rate;
        accuracy_increase = wsc.accuracy - wsc.base_accuracy;
        crit_chance_increase = wsc.crit_chance - wsc.base_crit_chance;
        crit_multi_increase = wsc.crit_multiplier - wsc.base_crit_multi;
    }

    void SetNameDisplay()
    {
        w_weapon_name.text = wsc.weapon_name.ToString();
        b_bullet_name.text = bulletModule.bullet.bullet_name;
    }

    void SetCurrentBulletDisplay()
    {
        b_bullet_image.sprite = bulletModule.bullet.bullet_sprite;
        b_bullet_image.color = bulletModule.bullet.bullet_color;
        b_bullet_name.text = bulletModule.bullet.bullet_name;
        b_manaCost_num.text = bulletModule.bullet.mana_cost_per_shot.ToString();
        b_speed_num.text = bulletModule.bullet.bullet_speed.ToString();
        b_effect_description.text = bulletModule.bullet.special_effect.effect_description;

        for (int i = 0; i < bulletModule.bullet.bullet_bonus_stats.Length; i++)
        {
            bonus_stats[i].text = bulletModule.bullet.bullet_bonus_stats[i].bonus_stats.ToString() + " +" + bulletModule.bullet.bullet_bonus_stats[i].stat_increase_amount.ToString();
        }
    }



	
	// Update is called once per frame
	void FixedUpdate () {
        SetNameDisplay();
    }
}