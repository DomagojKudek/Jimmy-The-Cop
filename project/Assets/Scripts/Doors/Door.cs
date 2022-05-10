using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Doors
{
    public abstract class Door : MonoBehaviour
    {
        public bool open = false;
        public bool disableChange = false;
        public abstract void Open();
        public abstract void Close();
    }
}
