using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	// Array svih zvukova koje ubacis preko inspektora
	public Sound[] sounds;

	public static AudioManager instance;
	// Use this for initialization
	void Awake () {

		if(instance == null){
			instance = this;
		}
		else{
			Destroy(gameObject);
			return;
		}		

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			SoundToSource(s, s.source);
		}
	}

	//Pusti zvuk ok AudioManager objekta tj. globalni zvuk
	// name je ime zvuka kojeg dodijelis u inspektoru
    public void Play(string name, bool unMute = false)
    {
        Sound sound;
        if (!checkSoundExists(name, out sound)) return;
        if (unMute) sound.source.mute = false;
        if (sound.source.loop)
        {
            PlayOnLoop(sound);
        }
        if (sound.source.isPlaying)
        {
            return;
        }
        else
        {
            sound.source.Play();
        }
    }

	//Pusti zvuk sa "source" AudioSourcea, dobijes 3d zvuk
	// Ako nije 3d, pogledaj na "source" u inspektoru jel parametar "Spatial Blend" blizu 1
	//overide true => forsira override na "source"
	public void Play(string name, AudioSource source, bool overide = false){
		if(source.isPlaying && !overide) return;
        Sound sound;
        if (!checkSoundExists(name, out sound)) return;
		SoundToSource(sound, source);
		source.Play();
	}

	private void PlayOnLoop(Sound sound){
		if(sound.source.isPlaying){
			return;
		}
        sound.source.Play();
	}

	//Zaustavi zvuk
	public void Stop(string name){
		Sound sound;
        if(!checkSoundExists(name, out sound)) return;
        sound.source.Stop();
	}
	//Zaustavi zvuk na "source"
	public void Stop(string name, AudioSource source){
        Sound sound;
        if (!checkSoundExists(name, out sound)) return;
        sound.source.Stop();
	}

	private bool checkSoundExists(string name, out Sound sound){
        sound = sounds.FirstOrDefault(s => s.name.Equals(name));
        if (sound == null)
        {
            Debug.LogError("Sound " + name + " not found! Add it to the AudioManager");
            return false;
        }
		return true;
	}

	private void SoundToSource(Sound sound, AudioSource source){
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
		source.loop = sound.loop;
	}
}
