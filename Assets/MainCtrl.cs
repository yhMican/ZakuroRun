 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtrl : MonoBehaviour
{

	enum GAMEMODE
	{
		TITLE,
		PLAY,
		END,
		DEMO
	};

	GAMEMODE nowMode;

	public Transform titleInfoGroup;
	public Player player;
	public Transform goalMarker;
	public Transform endInfoGroup;
	public Transform esaGroup;

	// Use this for initialization
	void Start ()
	{
		nowMode = GAMEMODE.TITLE;
		titleInfoGroup.gameObject.SetActive (true);
		endInfoGroup.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (nowMode) {
		case GAMEMODE.TITLE:
			if (Input.GetButtonDown ("Jump")) { 
				nowMode = GAMEMODE.PLAY;
				titleInfoGroup.gameObject.SetActive (false);
				player.IsStop = false;
			}
			break;

		case GAMEMODE.PLAY:
			if (player.transform.position.x > goalMarker.position.x) {
				player.IsStop = true;
				if (player.transform.position.y < goalMarker.position.y) {
					nowMode = GAMEMODE.DEMO;
					endInfoGroup.gameObject.SetActive (true);
					player.DemoStart ();
				}
			}
			break;

		case GAMEMODE.DEMO:
			nowMode = GAMEMODE.END;
			break;

		case GAMEMODE.END:
			if (Input.GetButton ("Jump")) {
				nowMode = GAMEMODE.TITLE;
				endInfoGroup.gameObject.SetActive (false);
				titleInfoGroup.gameObject.SetActive (true);

				for (int i = 0; i < esaGroup.childCount; ++i) {
					esaGroup.GetChild (i).gameObject.SetActive (true);
				}

				player.Reset ();
			}
			break;
		}
	}
}
