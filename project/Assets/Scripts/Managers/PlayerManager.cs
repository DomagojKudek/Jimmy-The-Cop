using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerManager : MonoBehaviour
    {
        public static GameObject instance = null;

        void Awake()
        {
            if (instance == null)
            {
                instance = GameObject.FindWithTag("Player");
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
