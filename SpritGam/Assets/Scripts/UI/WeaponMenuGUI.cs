﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenuGUI : MonoBehaviour {

    private WeaponStatConfig wsc;

    [SerializeField] Text w_weapon_name;
    [SerializeField] Image w_weapon_image;
    [SerializeField] Text w_damage_num;
    [SerializeField] Text w_fireRate_num;
    [SerializeField] Text w_fireType_num;
    [SerializeField] Text w_accuracy_num;
    [SerializeField] Text w_critChance_num;
    [SerializeField] Text w_critDamage_num;
    [SerializeField] public Image b_bullet_image;

    [SerializeField] Text b_manaCost_num;
    //[SerializeField] Text b_damage_num;
    //[SerializeField] Text b_bulletSpeed_num;
    //[SerializeField] Text b_ricochetCount_num;
    
    
    // Use this for initialization
    void Awake () {
        wsc = GameObject.Find("config: weapon").GetComponent<WeaponStatConfig>();
        w_weapon_image.sprite = GameObject.Find("Weapon").GetComponent<SpriteRenderer>().sprite;
    }

    void SetWeaponStatDisplay()
    {
        w_weapon_name.text = wsc.weapon_name.ToString();
        
        w_damage_num.text = wsc.damage.ToString();
        w_fireRate_num.text = wsc.fire_rate.ToString();
        w_fireType_num.text = wsc.fire_type;
        w_accuracy_num.text = wsc.accuracy.ToString();
        w_critChance_num.text = wsc.crit_chance.ToString();
        w_critDamage_num.text = wsc.crit_multiplier.ToString();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        SetWeaponStatDisplay();
    }
}
