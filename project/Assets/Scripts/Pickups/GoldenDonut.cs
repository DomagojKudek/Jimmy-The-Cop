using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Pickups
{
    class GoldenDonut : Pickup
    {
        public int scorePoints = 100;
        private HUDManager hudManager;

        public override void OnPickup()
        {
            hudManager = HUDManager.instance;
            GameManager.instance.score += scorePoints;
            GameManager.instance.goldenDoughnutCount += 1;
            print("GoldenDounouts = " + GameManager.instance.goldenDoughnutCount.ToString());
            hudManager.RenderScore(GameManager.instance.score);

        }
    }
}