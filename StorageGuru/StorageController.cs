using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Planetbase;
using UnityEngine;
using HarmonyLib;

namespace StorageGuru {

    public class StorageController {

        private Dictionary<Module, HashSet<Type>> storeManifest;
        private SerializeHelper serializer;

        public StorageController() {
            serializer = new SerializeHelper();
            storeManifest = new Dictionary<Module, HashSet<Type>>();
        }

        public void ConsolidateDefinitions() {
            List<Module> allStorage = GetAllStorage();
            if (storeManifest != null && allStorage != null && allStorage.Count != storeManifest.Count) {
                UpdateManifest(allStorage);
            }
        }

        private void UpdateManifest(List<Module> allStorage) {
            List<Module> list = storeManifest.Keys.ToList();
            if (allStorage.Count > storeManifest.Count) {
                foreach (Module item in allStorage.Except(list).ToList()) {
                    AddDefinitions(item, StorageGuru.allResourceTypes);
                }
            }
            else {
                if (allStorage.Count >= storeManifest.Count) {
                    return;
                }
                foreach (Module item2 in list.Except(allStorage).ToList()) {
                    RemoveDefinition(item2, null);
                }
            }
        }

        public void SaveManifest(string filepath) {
            string contents = serializer.SerializeManifest(storeManifest);
            File.WriteAllText(filepath, contents);
        }

        public void LoadManifest(string filePath) {
            Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
            if (File.Exists(filePath)) {
                string[] contents = File.ReadAllLines(filePath);
                dictionary = serializer.DeserializeManifest(contents);
            }
            else {
                Debug.Log("[MOD] StorageGuru could not find manifest file");
            }
            foreach (KeyValuePair<int, List<string>> item in dictionary) {
                Module moduleById = GetModuleById(item.Key);
                if (moduleById == null) {
                    continue;
                }
                AddDefinition(moduleById, null);
                if (item.Value == null || item.Value.Count <= 0) {
                    continue;
                }
                foreach (string item2 in item.Value) {
                    if (!string.IsNullOrEmpty(item2)) {
                        Type type = Type.GetType(item2);
                        if (type != null) {
                            AddDefinition(moduleById, type);
                        }
                    }
                }
            }
        }

        private void AddDefinition(Module module, Type resource) {
            if (storeManifest.ContainsKey(module)) {
                if (resource != null) {
                    storeManifest[module].Add(resource);
                }
            }
            else if (resource == null) {
                storeManifest.Add(module, new HashSet<Type>());
            }
            else {
                storeManifest.Add(module, new HashSet<Type> { resource });
            }
        }

        private void RemoveDefinition(Module module, Type resource) {
            if (storeManifest.ContainsKey(module)) {
                if (resource == null) {
                    storeManifest.Remove(module);
                }
                else {
                    storeManifest[module].Remove(resource);
                }
            }
        }

        public void ToggleDefinitions(Module module, Type[] resources) {
            foreach (Type resource in resources) {
                if (HasDefinition(module, resource)) {
                    RemoveDefinition(module, resource);
                }
                else {
                    AddDefinition(module, resource);
                }
            }
            DefinitionChanged(module);
        }

        public void AddDefinitions(Module module, Type[] resources) {
            foreach (Type resource in resources) {
                AddDefinition(module, resource);
            }
            DefinitionChanged(module);
        }

        public void RemoveDefinitions(Module module, Type[] resources) {
            foreach (Type resource in resources) {
                RemoveDefinition(module, resource);
            }
            DefinitionChanged(module);
        }

        private void DefinitionChanged(Module module) {
            if (module != null) {
                StorageGuru.RefreshStorage(module, GetDefinition(module));
                SaveManifest(StorageGuru.STORAGE_MANIFEST_PATH);
            }
        }

        public HashSet<Type> GetDefinition(Module module) {
            if (storeManifest.ContainsKey(module)) {
                return storeManifest[module];
            }
            return new HashSet<Type>();
        }

        public bool HasDefinition(Module module, Type resource) {
            if (module != null && resource != null && storeManifest.ContainsKey(module) && GetDefinition(module).Count > 0) {
                return GetDefinition(module).Contains(resource);
            }
            return false;
        }

        public List<Module> GetValidModules(Type resource) {
            List<Module> list = (from x in storeManifest
                                 where
                                    (
                                        x.Value == null ||
                                        x.Value.Contains(resource)
                                    ) &&
                                    x.Key.isEnabled() &&
                                    Traverse.Create(x.Key).Field("mResourceStorageIndicator").Field<float>("mValue").Value < 0.95f
                                 select
                                    x.Key
                                ).ToList();
            if (list.Count == 0) {
                list = storeManifest.Keys.ToList();
            }
            return list;
        }

        public int GetAllModuleCount() {
            return storeManifest.Count;
        }

        public int GetValidModuleCount(Type resource) {
            return GetValidModules(resource).Count();
        }

        public bool IsStorageAvailable(Type resource) {
            return storeManifest.Where((KeyValuePair<Module, HashSet<Type>> x) => x.Value.Contains(resource)).ToList().Count > 0;
        }

        public Module FindNearestModule(Vector3 position, List<Module> validModules) {
            List<Module> list = null;
            if (validModules != null && validModules.Count > 0) {
                list = validModules.OrderBy((Module x) => x.distanceTo(position)).ToList();
                if (list.Count <= 0) {
                    return null;
                }
                return list[0];
            }
            return null;
        }

        public Module GetModuleById(int id) {
            List<Module> allStorage = GetAllStorage();
            if (allStorage != null) {
                return allStorage.FirstOrDefault((Module x) => x.getId() == id);
            }
            return null;
        }

        public List<Module> GetAllStorage() {
            List<Module> categoryModules = Module.getCategoryModules(Module.Category.Storage);
            if (categoryModules != null) {
                for (int i = 0; i < categoryModules.Count; i++) {
                    if (categoryModules[i] == null) {
                        categoryModules.RemoveAt(i);
                    }
                }
            }
            return categoryModules;
        }

    }

}
