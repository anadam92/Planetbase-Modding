using Planetbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;

namespace LandingControl {

    public class GuiLandingPermissionsCustom : GuiLandingPermissions {

        private List<GuiAmountSelector> new_mSpecializationAmountSelectors;

        public GuiLandingPermissionsCustom()
            : base() {

            this.new_mSpecializationAmountSelectors = Traverse.Create(this).Field<List<GuiAmountSelector>>("mSpecializationAmountSelectors").Value;
            foreach (GuiAmountSelector selector in new_mSpecializationAmountSelectors) {
                Traverse.Create(selector).Field<int>("mStep").Value = 1;
                Traverse.Create(selector).Field<GuiDefinitions.Callback>("mChangeCallback").Value = null;
                Traverse.Create(selector).Field<int>("mFlags").Value = 0;
                Traverse.Create(selector).Field<string>("mTooltip").Value = null;
                Traverse.Create(this).Field("mResetItem").Field<GuiDefinitions.Callback>("mCallback").Value = new GuiDefinitions.Callback(onReset_custom);
            }
        }

        private void onReset_custom(object parameter) {
            LandingPermissions landingPermissions = LandingShipManager.getInstance().getLandingPermissions();
            foreach (Specialization specialization in SpecializationList.getColonistSpecializations()) {
                landingPermissions.getSpecializationPercentage(specialization).set(0);
            }
        }

    }
}
