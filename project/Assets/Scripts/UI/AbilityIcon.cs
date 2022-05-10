using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    private Slider slider;
    public Sprite image;

    public float speed;
    public int numOfCharges;

    private float updateRate = 1f;

    private Coroutine updateCorutine;

    private void Start(){
        slider = this.gameObject.GetComponent<Slider>();
        slider.maxValue = numOfCharges;
        slider.value = numOfCharges;
        if(image != null){
            slider.transform.GetChild(0).GetComponent<Image>().sprite = image;
        }
        updateCorutine = StartCoroutine(UpdateSliderCorutine(updateRate));
    }

    private IEnumerator UpdateSliderCorutine(float rate)
    {
        while(true){
            UpdateSlider();
            yield return new WaitForSeconds(rate);
        }
    }
    
    public void PauseUpdateSlider(){
        StopCoroutine(updateCorutine);
    }

    public void ContinueSlider(){
        updateCorutine = StartCoroutine(UpdateSliderCorutine(updateRate));
    }

    private void UpdateSlider(){
        slider.value += speed;
    }

    public void UseCharges(int n){
        slider.value -= n;
    }

    public bool CheckIfUsable(int n){
        return n <= GetCharges();
    }

    public int GetCharges(){
        return (int)slider.value;
    }

    public void ContinousUseCharges(int n){
        slider.value -= n ;
    }

    public bool CheckIfUsableContinous(float n){
        return slider.value > n;
    }
  
    void OnEnable(){
        this.Start();
    }
    
}