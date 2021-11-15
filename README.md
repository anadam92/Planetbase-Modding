# README #

This is a fork of this BitBucket repository of João Farias:
https://bitbucket.org/joaofarias/planetbase-modding.git
I completely changed each project to be a [UMM (Unity Mod Manager)](https://github.com/newman55/unity-mod-manager) mod.
I also added other Planetbase mods which can be found [here](https://www.nexusmods.com/planetbase/mods/).
## Usage
To use these mods:
1. [Install UMM](https://www.nexusmods.com/site/mods/21)
2. Download a release from this repository. [1.0.0](https://github.com/anadam92/Planetbase-Modding/releases/download/1.0.0/PlanetBase_mods_umm.7z)
3. Go to the installation folder of UMM and find the UnityModManagerConfig.xml file
4. Add these lines in the end of the file before the "</Config>" line.
`<!-- 0.24.0 -->
	<GameInfo Name="Planetbase">
		<Folder>Planetbase</Folder>
		<ModsDirectory>Mods</ModsDirectory>
		<ModInfo>Info.json</ModInfo>
		<GameExe>Planetbase.exe</GameExe>
		<EntryPoint>[Assembly-CSharp.dll]Planetbase.GameStateTitle.init:After</EntryPoint>
		<StartingPoint>[Assembly-CSharp.dll]Planetbase.GameStateTitle.init:After</StartingPoint>
		<MinimalManagerVersion>0.24.0</MinimalManagerVersion>
	</GameInfo>`
  5. Run UMM.
  6. Now you can select Planetbase as the game.
  7. Drag & Drop the mods (zip files) you want in the "Mods" tab of UMM.

While playing press Ctrl+F10 to toggle on/off any mod.

------------------------------------------------------

Credits:

[PiMaker](https://github.com/PiMaker) for the original version of the Mod Patcher that can be found [here](https://github.com/PiMaker/PlanetbasePatcher).

[SSchoener](https://github.com/sschoener) for the detour mechanism used in the Redirector - it can be found [here](https://github.com/sschoener/cities-skylines-detour).

[newman55](https://github.com/newman55) for the Unity Mod Manager which can be found [here](https://github.com/newman55/unity-mod-manager)

[João Farias](https://bitbucket.org/joaofarias/planetbase-modding.git)

[anadam92](https://github.com/anadam92)
