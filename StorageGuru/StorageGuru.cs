using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Planetbase;
using UnityEngine;
using HarmonyLib;

namespace StorageGuru {

    public class StorageGuru {

        public static string STORAGE_MANIFEST_PATH = "Mods\\Settings\\storage_manifest.txt";
        private static StorageGuru _instance;
        private static GameState gameState;
        private static GameStateGame gameStateGame;
        private static Module activeModule;
        Traverse t_mMenuSystem;
        private static GuiMenu activeMenu;
        private bool initialising = true;
        private bool menuSetUp;
        private bool enableAll = true;
        private Dictionary<Character, Resource> carriedResources;
        private Dictionary<Character, Resource> newCarriedResources;
        private List<Type> uniqueCarriedResources;
        private Dictionary<Type, List<Module>> resourceTargets;
        private Dictionary<Character, Module> newCharacterTargets;
        private Dictionary<Character, Module> characterTargets = new Dictionary<Character, Module>();
        public static StorageController Controller { get; private set; }
        public static Dictionary<ResourceType, GuiDefinitions.Callback> allResources { get; private set; }
        public static Type[] allResourceTypes { get; private set; }
        public static Dictionary<ResourceType, IconPair> allIcons { get; private set; }

        public static StorageGuru GetInstance() {
            return _instance;
        }

        public void Init() {
            _instance = this;
            STORAGE_MANIFEST_PATH = Path.Combine(Util.getFilesFolder(), STORAGE_MANIFEST_PATH);
            Controller = new StorageController();
            allResources = StorageMapping.GetAllResources();
            allResourceTypes = allResources.Select((KeyValuePair<ResourceType, GuiDefinitions.Callback> x) => x.Key.GetType()).ToArray();
            allIcons = ContentManager.LoadContent(allResources.Keys.ToArray());
        }

        public void Update() {
            gameState = GameManager.getInstance().getGameState();
            if (gameState is GameStateTitle) {
                if (!initialising) {
                    Controller.SaveManifest(STORAGE_MANIFEST_PATH);
                    FirstUpdate();
                }
            }
            else if (gameState is GameStateGame) {
                if (initialising) {
                    FirstUpdate();
                    initialising = false;
                }
                GameUpdate();
                Controller.ConsolidateDefinitions();
            }
        }

        private void GameUpdate() {
            gameStateGame = (GameStateGame)gameState;
            Traverse t_gameStateGame = Traverse.Create(gameStateGame);
            Traverse t_mActiveModule= t_gameStateGame.Field("mActiveModule");
            Traverse t_mMenuSystem= t_gameStateGame.Field("mMenuSystem");
            activeModule = t_mActiveModule.GetValue<Module>();
            activeMenu = t_mMenuSystem.GetValue<GuiMenuSystem>().getCurrentMenu();
            if (activeModule != null && t_mActiveModule.Method("getCategory").GetValue<Planetbase.Module.Category>() == Module.Category.Storage) {
                if (activeMenu == t_mMenuSystem.Field<GuiMenu>("mMenuEdit").Value && !menuSetUp) {
                    SetupEditMenu(activeModule);
                    menuSetUp = true;
                }
            }
            else {
                menuSetUp = false;
            }
            RedirectCharacters();
        }

        public void FirstUpdate() {
            Controller = new StorageController();
            Controller.LoadManifest(STORAGE_MANIFEST_PATH);
            initialising = false;
        }

        private void SetupEditMenu(Module module) {
            GuiMenu mMenuEdit = t_mMenuSystem.Field<GuiMenu>("mMenuEdit").Value;
            HashSet<Type> definition = Controller.GetDefinition(module);
            GuiMenuItem backItem = mMenuEdit.getBackItem();
            mMenuEdit.clear();
            foreach (KeyValuePair<ResourceType, GuiDefinitions.Callback> allResource in allResources) {
                string text = allResource.Key.getName().Trim();
                Texture2D icon;
                if (definition.Count == 0 || !definition.Contains(allResource.Key.GetType())) {
                    icon = allIcons[allResource.Key].disabledIcon;
                    text += " - OFF";
                }
                else {
                    icon = allIcons[allResource.Key].enabledIcon;
                    text += " - ON";
                }
                mMenuEdit.addItem(new GuiMenuItem(icon, text, allResource.Value));
            }
            enableAll = definition.Count != allResources.Count && (definition.Count == 0 || enableAll);
            if (enableAll) {
                mMenuEdit.addItem(new GuiMenuItem(ContentManager.StorageEnableIcon, "Enable All", EnableAllCallback));
            }
            else {
                mMenuEdit.addItem(new GuiMenuItem(ContentManager.StorageDisableIcon, "Disable All", DisableAllCallback));
            }
            mMenuEdit.addItem(backItem);
            menuSetUp = true;
        }

        public void StorageCallback(Type resource) {
            if (resource != null) {
                Controller.ToggleDefinitions(activeModule, new Type[1] { resource });
                SetupEditMenu(activeModule);
            }
        }

        public void DisableAllCallback(object parameter) {
            Controller.RemoveDefinitions(activeModule, allResourceTypes);
            SetupEditMenu(activeModule);
        }

        public void EnableAllCallback(object parameter) {
            Controller.AddDefinitions(activeModule, allResourceTypes);
            SetupEditMenu(activeModule);
        }

        private void RedirectCharacters() {
            carriedResources = Traverse.Create(typeof(Character)).Field<List<Character>>("mCharacters").Value
                .Where((Character x) => x.getLoadedResource() != null)
                .ToDictionary(
                    (Character y) => y,
                    (Character x) => x.getLoadedResource()
                );
            newCarriedResources = carriedResources.Where((KeyValuePair<Character, Resource> x) => !characterTargets.ContainsKey(x.Key)).ToDictionary((KeyValuePair<Character, Resource> y) => y.Key, (KeyValuePair<Character, Resource> x) => x.Value);
            if (newCarriedResources.Count == 0) {
                return;
            }
            newCarriedResources = newCarriedResources
                .Where(
                    (KeyValuePair<Character, Resource> x) => 
                        x.Key.getTarget() != null &&
                        x.Key.getTarget().getSelectable() != null &&
                        x.Key.getTarget().getSelectable() is Module &&
                        ((Module)x.Key.getTarget().getSelectable()).isBuilt() &&
                        Traverse.Create(((Module)x.Key.getTarget().getSelectable())).Method("getCategory").GetValue<Module.Category>() == Module.Category.Storage
                )
                .ToDictionary(
                    (KeyValuePair<Character, Resource> x) => x.Key,
                    (KeyValuePair<Character, Resource> y) => y.Value
                );
            uniqueCarriedResources = newCarriedResources.Select((KeyValuePair<Character, Resource> x) => x.Value.getResourceType().GetType()).Distinct().ToList();
            resourceTargets = uniqueCarriedResources.ToDictionary((Type x) => x, (Type y) => Controller.GetValidModules(y));
            newCharacterTargets = newCarriedResources.ToDictionary((KeyValuePair<Character, Resource> x) => x.Key, (KeyValuePair<Character, Resource> y) => Controller.FindNearestModule(y.Key.getPosition(), resourceTargets[y.Value.getResourceType().GetType()]));
            characterTargets = characterTargets.Where((KeyValuePair<Character, Module> x) => newCarriedResources.ContainsKey(x.Key)).ToDictionary((KeyValuePair<Character, Module> x) => x.Key, (KeyValuePair<Character, Module> y) => y.Value);
            foreach (KeyValuePair<Character, Module> newCharacterTarget in newCharacterTargets) {
                if (newCharacterTarget.Value != null) {
                    Traverse.Create(newCharacterTarget.Key).Method("setTarget").GetValue(
                        new object[] { new Target(newCharacterTarget.Value, newCharacterTarget.Value.getRadius() / 1.8f) }
                    );
                }
                characterTargets.Add(newCharacterTarget.Key, newCharacterTarget.Value);
            }
        }

        public static void RefreshStorage(Module module, HashSet<Type> allowedResources) {
            Traverse t_module_mResourceStorage = Traverse.Create(module).Field("mResourceStorage");
            Traverse t_module_mResourceStorage_mSlots = t_module_mResourceStorage.Field("mSlots");
            if (module == null || t_module_mResourceStorage.GetValue() == null || t_module_mResourceStorage_mSlots.GetValue() == null) {
                return;
            }
            foreach (StorageSlot mSlot in t_module_mResourceStorage_mSlots.GetValue<List<StorageSlot>>()) {
                Traverse<List<Resource>> mSlot_mResources = Traverse.Create(mSlot).Field<List<Resource>>("mResources");
                if (mSlot == null || mSlot_mResources.Value == null) {
                    continue;
                }
                foreach (Resource mResource in mSlot_mResources.Value) {
                    if (allowedResources.Contains(mResource.getResourceType().GetType())) {
                        mResource.setState(Resource.State.Stored);
                    }
                    else if (Controller.IsStorageAvailable(mResource.getResourceType().GetType())) {
                        mResource.setState(Resource.State.Idle);
                    }
                }
            }
        }

    }

}
