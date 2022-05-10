using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class IntroText : MonoBehaviour {

	private TMPro.TextMeshPro textMeshPro;
	public float speed = 0.14f;
	public float fadeAfter = 16f;

	private bool isMoving = false;

	// Use this for initialization
	void Awake () {
		textMeshPro = this.GetComponent<TMPro.TextMeshPro>();
		textMeshPro.gameObject.SetActive(false);

    }

	public void StartText(){
        textMeshPro.gameObject.SetActive(true);
		/*
		if(!isMoving){
			StartCoroutine(Move());
			StartCoroutine(FadeDelay(fadeAfter));
		}
		*/
	}

	public IEnumerator Move(){
		isMoving = true;
		int i = 0;
		float time = 0;
		while(i < 450){
			time += Time.deltaTime;
			this.gameObject.transform.localPosition += new Vector3(0, 0.05f * speed, 0.025f * speed);
			i++;
			yield return null;
        }
	}

	private IEnumerator FadeDelay(float fadeTime){
		yield return new WaitForSecondsRealtime(fadeTime);
		yield return Fade();
	}

	public IEnumerator Fade(){
		Color32 c = this.textMeshPro.color;
        byte alpha = 0;
		while(c.a > 0){
            alpha = (byte)(c.a - 1);
            this.textMeshPro.color = new Color32(c.r, c.g, c.b, alpha);
            c = this.textMeshPro.color;
			yield return null;
        }
		this.gameObject.SetActive(false);
		
	}
}
