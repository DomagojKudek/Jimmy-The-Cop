using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LightweightPipeline;

public class Daytime_v2 : MonoBehaviour
{

	public Light sun;
    public int frames=0;
    public float secondsInFullDay = 120f;
    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;
    public int fremefrequency=100;
    public float deltaTime=2;

    float sunInitialIntensity;
    public float lastTime=0;
    public LightweightPipelineAsset asset;


    void Start()
    {
        //asset.shadowDistance=1000;
        sunInitialIntensity = sun.intensity;
        lastTime=Time.time;
            UpdateSun();
            currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

            if (currentTimeOfDay >= 1)
            {
                currentTimeOfDay = 0;
            }
    }

    void Update()
    {
        frames++;
        if(Time.time+fremefrequency*Time.deltaTime>lastTime+deltaTime){
        //if (frames % (int)(Time.deltaTime*fremefrequency) == 0) {
            lastTime=Time.time;
            UpdateSun();
            currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

            if (currentTimeOfDay >= 1)
            {
                currentTimeOfDay = 0;
            }
        }
       /*  currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }*/
    }

    void UpdateSun()
    {
        sun.transform.rotation = Quaternion.Euler(((-currentTimeOfDay) * 176) +168, (-currentTimeOfDay*0.59f* 105)-30, 0);
        //sun.transform.rotation = Quaternion.Euler(((-currentTimeOfDay) * 176) +168, (-currentTimeOfDay*100)+240, 0);

        float intensityMultiplier = 1;
        //if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        //{
        //    intensityMultiplier = 0.7f;
        //}
        //else if (currentTimeOfDay <= 0.25f)
        //{
        //    intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        //}
        //else if (currentTimeOfDay >= 0.73f)
        //{
        //    intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        //}
       // sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}
