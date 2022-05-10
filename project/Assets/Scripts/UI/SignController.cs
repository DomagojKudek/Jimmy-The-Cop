using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts;
public class SignController : MonoBehaviour {
	public List<string> hints;
	public List<string> hintsJoystick;
	public List<TextMeshPro> tmpguis;
	public string hint1joystick="To jump press A.";
	public string hint2joystick="To double jump press A twice in a row.";
	public string hint3joystick="Try to push the Ball while holding LT.";
	public string hint4joystick="To activate Gravity field press X.";
	public string hint5joystick="If you charge Push or Pull to the max you can break through orange walls.";
	public string hint6joystick="To use Pull hold down RT.";

	
	// Use this for initialization
	void Start () {
		hintsJoystick=new List<string>{hint1joystick,hint2joystick,hint3joystick,hint4joystick,hint5joystick,hint6joystick};
		hints=new List<string>();
		tmpguis=new List<TextMeshPro>();
		foreach (Transform child in transform)
		{
			tmpguis.Add(child.GetComponentInChildren<TextMeshPro>());
			hints.Add(child.GetComponentInChildren<TextMeshPro>().text);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.instance.joystick==true){
			for (int i = 0; i < tmpguis.Count; i++){
				tmpguis[i].text = hintsJoystick[i];
			}
		}else{
			for (int i = 0; i < tmpguis.Count; i++){
				tmpguis[i].text = hints[i];
			}
		}
		
	}
}
