using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public static class ConfigurationManager{
	
	public static void saveConfiguration(string controlScheme){
		BinaryFormatter formatter=new BinaryFormatter();
		FileStream saveStream=new FileStream(Application.persistentDataPath+"/configuration.config",FileMode.Create);
		Configuration configuration=new Configuration(controlScheme);
		formatter.Serialize(saveStream,configuration);
		saveStream.Close();
	}

	public static bool loadConfiguration(){
		if(File.Exists(Application.persistentDataPath+"/configuration.config")){
			BinaryFormatter formatter=new BinaryFormatter();
			FileStream loadStream=new FileStream(Application.persistentDataPath+"/configuration.config",FileMode.Open);
			Configuration configuration=formatter.Deserialize(loadStream) as Configuration;
			loadStream.Close();
			Debug.Log(configuration.controlScheme);
			if(configuration.controlScheme=="Joystick"){
				return true;
			}
			else{
				return false;
			}
		}
		else return true;
	}
}
