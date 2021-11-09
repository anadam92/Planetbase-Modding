using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Planetbase;
using HarmonyLib;

namespace DebugManagerOn {

    public class DebugManagerOn_MonoBehaviour : MonoBehaviour {

        public void OnGUI() {
            Traverse.Create(DebugManager.getInstance()).Field<bool>("mEnabled").Value = true;
            DebugManager.getInstance().onGui();
        }

    }

}