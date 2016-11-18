﻿using UnityEngine;
using System;
using System.Collections;

/****************************************************************************************/
/*
/* FILE NAME: Player_control
/*
/* DESCRIPTION: this script contains all the default player controls as well as the pwr
/* 		up implementation.
/*
/*
/* DATE     BY     		DESCRIPTION
/* ======== ======= 	=============
/* 10/25/16 Jacob		created headr
/* 11/15/16 Jacob/Brandon edited movement/machine gun pwrup
/*
/****************************************************************************************/

public class Player_control : MonoBehaviour {

	float default_speed;
	public int b_speed = 20;
	public Rigidbody2D projectile;
	public Rigidbody2D laZer;
	public Rigidbody2D rocket;
	public float m_speed;
	public Vector3 pos;
	private bool shield;
	private int automaticFire = 5;
	public float invuln_timer = 0;
	public bool hitframes = false;
	public System.Random rand = new System.Random ();


	public Stack held_power_ups = new Stack();
	//array indices: 0=red, 1=blue, 2=yellow
	public int[] poweruparray = {0, 0, 0};

	// Use this for initialization
	void Start () {
		this.gameObject.layer = 9;
		default_speed = m_speed;
		shield = false;
		pos = transform.position;
		GameObject theCamera = GameObject.Find("Camera");
		pause_script pauseScript = theCamera.GetComponent<pause_script>();
	}
	
	// FixedUpdate called at regular intervals
	void FixedUpdate () {

		//movement keys

		var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);
		transform.position += move * m_speed * Time.deltaTime;

		//check for invulnerability
		if (invuln_timer>0){
			invuln_timer -= 1f*Time.deltaTime;
			if(invuln_timer<=0) {
				this.gameObject.layer = 9;
				hitframes = false;
				this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			}
		}


		//ambient movement		
		transform.position += Vector3.right * Ambient_scrolling.ambientScrollSpeed * Time.deltaTime;

	}

	void Update() {

		if (!pause_script.isPause) {
			FireWeapon ();
			if (Input.GetKeyDown (KeyCode.E) || Input.GetButtonDown ("GetPower")) {
				ActivatePowerups ();
			}
			if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetButtonDown ("Special")) {
				UseAbility ();
			}
		}
	}
	

	void FireWeapon(){
		//machine gun
		if(poweruparray[0]==1){
			if (Input.GetKey(KeyCode.Space)||Input.GetButton("Fire1")) {
				if (automaticFire == 20){
					Rigidbody2D clone;
					clone = Instantiate(projectile, transform.position + new Vector3(0.8F,0,0), transform.rotation) as Rigidbody2D;
					clone.velocity = transform.TransformDirection(new Vector3(b_speed, 3*(float)(rand.NextDouble()-0.5),0));
					automaticFire = 0;
				}
				else{
					automaticFire++;
				}
			}
		}
		//rocket
		else if(poweruparray[0] == 2){ 
			if (Input.GetKeyDown(KeyCode.Space)||Input.GetButtonDown("Fire1")) {
				Rigidbody2D clone;
				clone = Instantiate(laZer, transform.position + new Vector3(0.8F,0,0), transform.rotation) as Rigidbody2D;
				clone.velocity = transform.TransformDirection(new Vector3(b_speed, 0,0));
			}	
		}
		//laZer
		else if(poweruparray[0] == 3){ 
			if (Input.GetKeyDown(KeyCode.Space)||Input.GetButtonDown("Fire1")) {
				Rigidbody2D clone;
				clone = Instantiate(laZer, transform.position + new Vector3(0.8F,0,0), transform.rotation) as Rigidbody2D;
				clone.velocity = transform.TransformDirection(new Vector3(b_speed, 0,0));
			}	
		}
		//default
		else{ 
			if (Input.GetKeyDown(KeyCode.Space)||Input.GetButtonDown("Fire1")) {
				Rigidbody2D clone;
				clone = Instantiate(projectile, transform.position + new Vector3(0.8F,0,0), transform.rotation) as Rigidbody2D;
				clone.velocity = transform.TransformDirection(new Vector3(b_speed, 0,0));
			}
		}
	}
	// void ShowPowerUps(){
	// 	held_power_ups.CopyTo(poweruparray, 0);
	// 	for (int i=0; i<3; i++){
	// 		Debug.Log(poweruparray[i]);
	// 	}

	void UseAbility(){
		//Barrel Roll
		if(poweruparray[1] == 2){
			this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
			invuln_timer = 1;
			this.gameObject.layer = 10;
			// this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public void LosePowerUp(){
		// Getting hit with a power-up
		poweruparray = new int[3];
		invuln_timer = 3;
		hitframes = true;
		this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 0.8f);
		this.gameObject.layer = 10;
		m_speed=default_speed;
	}

	void ActivatePowerups() {
		// Cashing in

		//array indices: 0=red, 1=blue, 2=yellow
		//Clear power-up stack, determine activated power-up, activate power-up
		// switch(poweruparray){
		// 	case [0, 0, 0]:
		// 		break;
		// 	case [1, 0, 0]:

		// }

		// held_power_ups.CopyTo(poweruparray, 0);

		poweruparray = new int[3];

		// Debug.Log(held_power_ups.Pop());
		while (held_power_ups.Count>0){
			char code = (char)held_power_ups.Pop();
			Debug.Log(code);
			if (code == 'r'){
				poweruparray[0]++;
			}
			else if(code == 'b'){
				poweruparray[1]++;
			}
			else if(code == 'y'){
				poweruparray[2]++;
			}
		}

		// for (int i=0; i<3; i++){
		// 	Debug.Log(poweruparray[i]);
		// }

		// if (poweruparray[1] != 2){
		// 	this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		// }

		if (poweruparray[2] == 1){
			m_speed=m_speed*1.5f;
		}
		else{
			m_speed=default_speed;
		}

		if (poweruparray[1] == 1){
			shield = true;
		}
		else{
			shield = false;
		}


		// held_power_ups.Clear();

	}

	// float getX() {
	// 	return transform.position.x;
	// }

	// float getY() {
	// 	return transform.position.y;
	// }


}
