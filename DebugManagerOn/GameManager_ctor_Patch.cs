using Planetbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace DebugManagerOn {

    [HarmonyPatch(typeof(GameManager), MethodType.Constructor)]
    public class GameManager_ctor_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            Planetbase.ShortcutManager.getInstance().addShortcut(KeyCode.F11, (object parameter) => Planetbase.DebugManager.getInstance().toggle(), true);
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mRenderInteriorNavGraph").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mRenderExteriorNavGraph").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mRenderNavPath").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mRenderAStar").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mDebugAiMessages").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mDebugAiSelectionOnly").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mDebugCamera").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mHideAllTops").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mExtraDescriptionInfo").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mInexpensiveBuildings").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mHideConnections").Value = false;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mDebugGround").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mShowAchievements").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("m100PercentInterceptionChance").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mRenderGraphColors").Value = true;
            Traverse.Create(Planetbase.DebugManager.getInstance()).Field<bool>("mRenderAirlockDistances").Value = true;

        }

    }

}
