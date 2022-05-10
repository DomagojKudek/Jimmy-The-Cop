using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Configuration {

	public string controlScheme;
	
	public Configuration(string selectedControlScheme){
		controlScheme=selectedControlScheme;
	}
	
}
