using System.Collections.Generic;
using Planetbase;

namespace StorageGuru {

    internal static class StorageMapping {

        internal static bool isEditable(ModuleType type) {
            if (type is ModuleTypeAirlock) {
                return false;
            }
            if (type is ModuleTypeAntiMeteorLaser) {
                return false;
            }
            if (type is ModuleTypeBar) {
                return true;
            }
            if (type is ModuleTypeBasePad) {
                return false;
            }
            if (type is ModuleTypeBioDome) {
                return true;
            }
            if (type is ModuleTypeCabin) {
                return true;
            }
            if (type is ModuleTypeCanteen) {
                return true;
            }
            if (type is ModuleTypeControlCenter) {
                return true;
            }
            if (type is ModuleTypeDorm) {
                return true;
            }
            if (type is ModuleTypeFactory) {
                return true;
            }
            if (type is ModuleTypeLab) {
                return true;
            }
            if (type is ModuleTypeLandingPad) {
                return false;
            }
            if (type is ModuleTypeLightningRod) {
                return false;
            }
            if (type is ModuleTypeMine) {
                return false;
            }
            if (type is ModuleTypeMonolith) {
                return false;
            }
            if (type is ModuleTypeMultiDome) {
                return true;
            }
            if (type is ModuleTypeOxygenGenerator) {
                return false;
            }
            if (type is ModuleTypePowerCollector) {
                return false;
            }
            if (type is ModuleTypeProcessingPlant) {
                return true;
            }
            if (type is ModuleTypePyramid) {
                return false;
            }
            if (type is ModuleTypeRadioAntenna) {
                return false;
            }
            if (type is ModuleTypeRoboticsFacility) {
                return true;
            }
            if (type is ModuleTypeSickBay) {
                return true;
            }
            if (type is ModuleTypeSignpost) {
                return false;
            }
            if (type is ModuleTypeSolarPanel) {
                return false;
            }
            if (type is ModuleTypeStarport) {
                return false;
            }
            if (type is ModuleTypeStorage) {
                return true;
            }
            if (type is ModuleTypeTelescope) {
                return false;
            }
            if (type is ModuleTypeWaterExtractor) {
                return false;
            }
            if (type is ModuleTypeWaterTank) {
                return false;
            }
            bool flag = type is ModuleTypeWindTurbine;
            return false;
        }

        internal static Dictionary<ResourceType, GuiDefinitions.Callback> GetAllResources() {
            return new Dictionary<ResourceType, GuiDefinitions.Callback>
			{
				{
					new AlcoholicDrink(),
					ButtonCallbacks.CBAlcoholicDrink
				},
				{
					new Bioplastic(),
					ButtonCallbacks.CBBioplastic
				},
				{
					new Gun(),
					ButtonCallbacks.CBGun
				},
				{
					new Meal(),
					ButtonCallbacks.CBMeal
				},
				{
					new MedicalSupplies(),
					ButtonCallbacks.CBMedicalSupplies
				},
				{
					new MedicinalPlants(),
					ButtonCallbacks.CBMedicinalPlants
				},
				{
					new Metal(),
					ButtonCallbacks.CBMetal
				},
				{
					new Ore(),
					ButtonCallbacks.CBOre
				},
				{
					new Semiconductors(),
					ButtonCallbacks.CBSemiconductors
				},
				{
					new Spares(),
					ButtonCallbacks.CBSpares
				},
				{
					new Starch(),
					ButtonCallbacks.CBStarch
				},
				{
					new Vegetables(),
					ButtonCallbacks.CBVegetables
				},
				{
					new Vitromeat(),
					ButtonCallbacks.CBVitromeat
				}
			};
        }

    }
}
