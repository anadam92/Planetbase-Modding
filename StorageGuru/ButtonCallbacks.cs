using Planetbase;

namespace StorageGuru {

    public static class ButtonCallbacks {

        public static void CBBioplastic(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Bioplastic().GetType());
        }

        public static void CBGun(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Gun().GetType());
        }

        public static void CBMeal(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Meal().GetType());
        }

        public static void CBMedicalSupplies(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new MedicalSupplies().GetType());
        }

        public static void CBMetal(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Metal().GetType());
        }

        public static void CBOre(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Ore().GetType());
        }

        public static void CBSpares(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Spares().GetType());
        }

        public static void CBStarch(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Starch().GetType());
        }

        public static void CBVegetables(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Vegetables().GetType());
        }

        public static void CBVitromeat(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Vitromeat().GetType());
        }

        public static void CBMedicinalPlants(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new MedicinalPlants().GetType());
        }

        public static void CBSemiconductors(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new Semiconductors().GetType());
        }

        public static void CBAlcoholicDrink(object parameter) {
            StorageGuru.GetInstance().StorageCallback(new AlcoholicDrink().GetType());
        }

    }

}
