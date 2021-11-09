using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Planetbase;
using UnityEngine;
using System.IO;
using HarmonyLib;

namespace PowerSaver {

    public class GridManagementConsole : ComponentType {
        public static string NAME = "Grid Management Console";
        public static string DESCRIPTION = @"An Engineer can use this console to control the resource distribution in case of shortage. This will prevent your most vital modules from shutting down while there are non-vital modules still operating.";

        public GridManagementConsole() {
            this.mConstructionCosts = new ResourceAmounts();
            this.mConstructionCosts.add(ResourceTypeList.MetalInstance, 1);
            this.mConstructionCosts.add(ResourceTypeList.BioplasticInstance, 1);
            base.addUsageAnimation(CharacterAnimationType.WorkSeated, CharacterProp.Count, CharacterProp.Count);
            this.mOperatorSpecialization = TypeList<Specialization, SpecializationList>.find<Engineer>();
            this.mFlags = 264;
            this.mSurveyedConstructionCount = 20;
            this.mPrefabName = "PrefabRadioConsole";

            string language = Profile.getInstance().getLanguage();
            Dictionary<String, String> StringList_mStrings = Traverse.Create(typeof(StringList)).Field<Dictionary<String, String>>("mStrings").Value;
            if (language == "en") {
                StringList_mStrings.Add("component_" + Util.camelCaseToLowercase(this.GetType().Name), NAME);
                StringList_mStrings.Add("tooltip_" + Util.camelCaseToLowercase(this.GetType().Name), DESCRIPTION);
            }
            // this is needed because the game doesn't use the fallback strings for tooltips
            else if (!StringList.exists("tooltip_" + Util.camelCaseToLowercase(this.GetType().Name))) {
                StringList_mStrings.Add("tooltip_" + Util.camelCaseToLowercase(this.GetType().Name), DESCRIPTION);
            }
            Dictionary<String, String> StringList_mFallbackStrings = Traverse.Create(typeof(StringList)).Field<Dictionary<String, String>>("mFallbackStrings").Value;
            StringList_mFallbackStrings.Add("component_" + Util.camelCaseToLowercase(this.GetType().Name), NAME);
            StringList_mFallbackStrings.Add("tooltip_" + Util.camelCaseToLowercase(this.GetType().Name), DESCRIPTION);

            this.initStrings();

            string iconPath = Path.Combine(Util.getFilesFolder(), PowerSaver.CONSOLE_ICON_PATH);
            if (File.Exists(iconPath)) {
                byte[] iconBytes = File.ReadAllBytes(iconPath);
                Texture2D tex = new Texture2D(0, 0);
                tex.LoadImage(iconBytes);
                this.mIcon = Util.applyColor(tex);
            }
        }
    }

}
