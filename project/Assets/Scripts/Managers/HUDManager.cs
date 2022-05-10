using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

namespace Assets.Scripts
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager instance = null;

        [HideInInspector]
        public Canvas canvas;
        public GameObject lifeObj;
        public GameObject scoreObj;
        public float zPosition=430;

        private TMPro.TextMeshProUGUI tmpro;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                canvas = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetComponent<Canvas>();
                if(canvas == null) Debug.LogError("GRESKA U HUD MANGERU");
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        void Start(){
            tmpro = scoreObj.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
            RenderScore(0);
        }

        public void RenderHp(ref List<GameObject> lifeObjects, int hp)
        {
            //Canvas canvas = null;
            foreach (var l in lifeObjects)
            {
                Destroy(l);
            }
            lifeObjects.Clear();
            for (int i = 0; i < hp; i++)
            {
                GameObject lifeInstance = Instantiate(lifeObj, lifeObj.transform.position + new Vector3(20 * i  -63, -15, zPosition), Quaternion.identity);
                lifeObjects.Add(lifeInstance);
                lifeInstance.transform.SetParent(canvas.transform, worldPositionStays: false);
            }
        }

        private GameObject lastScoreInstance;
        public void RenderScore(int score){
            Destroy(lastScoreInstance);
            tmpro.text = score.ToString();
            GameObject instance = Instantiate(scoreObj, scoreObj.transform.position + new Vector3(-100 , -30, zPosition), Quaternion.identity);
            lastScoreInstance = instance;
            instance.transform.SetParent(canvas.transform, worldPositionStays: false);
        }
    }
}
