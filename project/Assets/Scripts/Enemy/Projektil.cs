using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Projektil : Enemy{
        private void Start() {
            Destroy(this.gameObject, 10);
            audioManager = AudioManager.instance; //treba biti
            sourceAttack = this.GetComponent<AudioSource>();
			sourceAttack.rolloffMode=AudioRolloffMode.Linear;
			attackSound="ProjectileHit";
        }
    }
}