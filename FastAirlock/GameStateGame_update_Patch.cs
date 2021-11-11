using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Planetbase;
using HarmonyLib;

namespace FastAirlock {

    [HarmonyPatch(typeof(GameStateGame), "update", MethodType.Normal)]
    class GameStateGame_update_Patch {

        [HarmonyPostfix]
        public static void Postfix() {
            string path = "./Mods/FastAirlockSettings/config.txt";
            FastAirlockPatch.pathy = Path.Combine(Util.getFilesFolder(), path);
            StreamReader streamReader = new StreamReader(FastAirlockPatch.pathy);
            string text = streamReader.ReadLine();
            text = text.Substring(13);
            FastAirlockPatch.speedmult = int.Parse(text);
        }

    }
}
