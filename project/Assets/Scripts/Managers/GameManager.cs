using System.Collections;
using UnityEngine;
using System.IO;
using Respawn;
using Assets.Scripts.Enemy;

namespace Assets.Scripts
{
    public enum GameState
    {
        Menu,
        Game
    }
    public class GameManager : MonoBehaviour
    {
        public Animator anim;
        public static GameManager instance = null;

        public GameState gameState;

        public Cinemachine.CinemachineVirtualCamera cinemachineCamera;

        public Transform[] spawnPoints;
        public int currentLevel;
        public float spawnDelay = 2;

        public bool joystick = false;

        [HideInInspector]
        public int score;

        [HideInInspector]
        public int goldenDoughnutCount = 0;
        private AudioManager audioManager;

        [HideInInspector]
        public Vector3 pointerDirection;

        private Camera mainCamera;

        private float radius;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                mainCamera = Camera.main;
                radius = 8;//PROMIJENI!!!! - radius od push
                if (GameObject.FindWithTag("IntroParent"))
                { //GameObject.FindGameObjectWithTag("Intro")
                    gameState = GameState.Menu;
                }
                else
                {
                    gameState = GameState.Game;
                }

            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            audioManager = AudioManager.instance;
        }

        public IEnumerator RespawnPlayer()
        {
            audioManager.Play("PlayerDeath");
            anim.SetBool("Dying", true);
            GameObject player = PlayerManager.instance;
            player.GetComponent<JimmyController1>().enabled = false;
            player.GetComponent<GravityField>().enabled = false;
            player.GetComponent<Push_Pull>().enabled = false;
            player.GetComponent<PlayerHealth>().enabled = false;
            yield return new WaitForSeconds(spawnDelay);

            player.GetComponent<JimmyController1>().enabled = true;
            player.GetComponent<GravityField>().enabled = true;
            player.GetComponent<Push_Pull>().enabled = true;
            player.GetComponent<PlayerHealth>().enabled = true;
            player.transform.position = spawnPoints[0].position;
            player.transform.rotation = spawnPoints[0].rotation;
            player.GetComponent<JimmyController1>().ClearCollisionList();
            player.GetComponent<PlayerHealth>().HealToFull();
            //Instantiate(playerPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
        }

        public IEnumerator RespawnPlayerNovi()
        {
			anim.SetBool("Dying", true);
            audioManager.Play("PlayerDeath");
            GameObject player = PlayerManager.instance;
            player.GetComponent<JimmyController1>().enabled = false;//.enabled = false;
            player.GetComponent<GravityField>().enabled = false;
            player.GetComponent<Push_Pull>().enabled = false;
            player.GetComponent<PlayerHealth>().enabled = false;

            cinemachineCamera.Follow = null;

            yield return new WaitForSeconds(spawnDelay);
			anim.SetBool("Dying", false);
			
			player.GetComponent<JimmyController1>().enabled = true;
            player.GetComponent<GravityField>().enabled = true;
            player.GetComponent<Push_Pull>().enabled = true;
            player.GetComponent<PlayerHealth>().enabled = true;

            cinemachineCamera.Follow = PlayerManager.instance.transform;

            //reloaad active area ?
            Area area = AreaManager.instance.GetActiveArea();
            if(area != null)
                area.ReloadArea();
            else{
                //TODO boss respawn
                GameObject boss = GameObject.FindWithTag("Boss");
                if(boss != null){
                    boss.GetComponent<Boss>().Restart();
                    boss.SetActive(false);
                }
            }

            Transform t = CheckpointSystem.GetInstance().GetActiveCheckpointTransform();
            player.transform.position = new Vector3(t.position.x, t.position.y, 0);
            player.transform.rotation = t.rotation;
            
            player.GetComponent<JimmyController1>().ClearCollisionList();
            player.GetComponent<PlayerHealth>().HealToFull();
        }

        public static void KillPlayer()
        {
            if (GameObject.FindWithTag("RespawnParent") != null)
                instance.StartCoroutine(instance.RespawnPlayerNovi());
        }

        public void ChangeControllerScheme()
        {
            joystick = !joystick;
        }


        //MousePointer
        public Vector3 GetVecToMouse()
        {
            Vector3 VScreen = new Vector3();
            Vector3 VWold = new Vector3();

            VScreen.x = Input.mousePosition.x;
            VScreen.y = Input.mousePosition.y;
            if (mainCamera == null) mainCamera = Camera.main;
            VScreen.z = mainCamera.transform.position.z * -1;//Nemam pojma zasto
            VWold = mainCamera.ScreenToWorldPoint(VScreen);
            VWold.z = 0;
            pointerDirection = VWold - PlayerManager.instance.transform.position;
            return pointerDirection;
        }

        public Vector3 GetVecFromControler()
        {
            pointerDirection = new Vector3(Input.GetAxis("HorizontalAim") * radius, Input.GetAxis("VerticalAim") * radius, 0);
            return pointerDirection;
        }
    }
}