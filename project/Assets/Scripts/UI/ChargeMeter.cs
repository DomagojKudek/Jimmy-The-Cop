using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
namespace Assets.Scripts
{
    public class ChargeMeter : MonoBehaviour
    {
        public float alpha = 0.3f;
        private float value = 0f;

        [HideInInspector]
        public float min = 0.12f; //magic number

        [HideInInspector]
        public float max = 1.54f; //magic number

        private SpriteRenderer spriteRenderer;

        private GameObject spriteMaskGo;

        private float scaledMid;

        private float scaledMax;

        private void Start(){
            spriteRenderer = this.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            spriteMaskGo = this.transform.GetChild(1).gameObject;
        }

        public void Initialize(float mid, float max){
            scaledMid = mid;
            scaledMax = max;
        }
        public void SetColor(Color color){
            color.a = alpha;
            //spriteRenderer.color = color;
            spriteRenderer.material.color = color;
            //print(spriteRenderer.material.color);
        }

        //ulaz vrijednost od 0 do 1
        public Color SetValue(float v){
            Color color = Color.red;
            //print(v);
            /*
            if (v > 1 || v < 0)
            {
                this.SetColor(color);
                return color;
            }
            */

            value = v*(max - min) + min + 0.01f;
            spriteMaskGo.transform.localScale = new Vector3(value, value, 0);
            float scaledValue = this.GetValue();

            if (scaledValue < scaledMid)
            {
                //green
                color = Color.green;
                this.SetColor(color);
            }

            if (scaledValue > scaledMid && scaledValue < scaledMax)
            {
                //yellow
                color = Color.yellow;
                this.SetColor(color);
            }
            if (scaledValue >= scaledMax)
            {
                //red
                color = Color.red;
                this.SetColor(color);
            }
            return color;
        }

        public static Gradient colorGradient(){
            Gradient grad = new Gradient();
            grad.mode = GradientMode.Blend;
            /*
            grad.SetKeys(new GradientColorKey[]{ new GradientColorKey(Color.green, 0.0f)
                                                ,new GradientColorKey(Color.yellow, 0.5f)
                                                ,new GradientColorKey(Color.red, 1.0f)}
                        ,new GradientAlphaKey[]{ new GradientAlphaKey(0.3f, 0.0f)
                                                ,new GradientAlphaKey(0.3f, 0.5f)
                                                ,new GradientAlphaKey(0.3f, 1.0f) });

            */         
            
            grad.SetKeys(new GradientColorKey[]{ new GradientColorKey(new Color(0, 255, 0), 0.0f)
                                                ,new GradientColorKey(new Color(255, 255, 0), 0.5f)
                                                //,new GradientColorKey(new Color(6, 13, 59), 1.0f)}
                                                ,new GradientColorKey(new Color(255, 0, 0), 1.0f)}
                        ,new GradientAlphaKey[]{ new GradientAlphaKey(1f, 0.0f)
                                                ,new GradientAlphaKey(1f, 0.5f)
                                                ,new GradientAlphaKey(1f, 1.0f) });
            
            return grad;
        }

        //vraca vrijednost skaliranu na 0 - 1
        public float GetValue(){
            return (value - min)/(max - min);
        }
        /*
        private void OnDisable(){
            //TODO stavi u funkciju koju zoves na pozivu Push i Pull
            spriteMaskGo.transform.localScale = Vector3.zero;
        }
        */
        public void ResetMask(){
            spriteMaskGo.transform.localScale = Vector3.zero;
        }
    }
}