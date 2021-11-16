REM Notepad++ Regex Replace:	ROBOCOPY %___dev_dir___%\\\1\\bin\\Debug %___output_dir___%\\\1 \1\.dll Info\.json /E
REM Notepad++ Regex Replace:	"C:\\Program Files\\7-Zip\\7z\.exe" a -tzip %___output_dir___%\\\1\.zip %___output_dir___%\\\1
REM Notepad++ Regex Replace:	RMDIR %___output_dir___%\\\1 /S /Q

SET ___dev_dir___=%~dp0
SET ___output_dir___=%userprofile%\Downloads\PlanetBase_mods_umm

ROBOCOPY %___dev_dir___%\AutoAlerts\bin\Debug %___output_dir___%\AutoAlerts AutoAlerts.dll Info.json /E
ROBOCOPY %___dev_dir___%\AutoRotateBuildings\bin\Debug %___output_dir___%\AutoRotateBuildings AutoRotateBuildings.dll Info.json /E
ROBOCOPY %___dev_dir___%\AutoConnections\bin\Debug %___output_dir___%\AutoConnections AutoConnections.dll Info.json /E
ROBOCOPY %___dev_dir___%\BetterHours\bin\Debug %___output_dir___%\BetterHours BetterHours.dll Info.json /E
ROBOCOPY %___dev_dir___%\BuildingAligner\bin\Debug %___output_dir___%\BuildingAligner BuildingAligner.dll Info.json /E
ROBOCOPY %___dev_dir___%\CameraOverhaul\bin\Debug %___output_dir___%\CameraOverhaul CameraOverhaul.dll Info.json /E
ROBOCOPY %___dev_dir___%\CharacterCam\bin\Debug %___output_dir___%\CharacterCam CharacterCam.dll Info.json /E
ROBOCOPY %___dev_dir___%\DebugManagerOn\bin\Debug %___output_dir___%\DebugManagerOn DebugManagerOn.dll Info.json /E
ROBOCOPY %___dev_dir___%\EternalBot\bin\Debug %___output_dir___%\EternalBot EternalBot.dll Info.json /E
ROBOCOPY %___dev_dir___%\FastAirlock\bin\Debug %___output_dir___%\FastAirlock FastAirlock.dll Info.json config.txt readme.txt /E
ROBOCOPY %___dev_dir___%\FreeBuilding\bin\Debug %___output_dir___%\FreeBuilding FreeBuilding.dll Info.json /E
ROBOCOPY %___dev_dir___%\FreeFurnishing\bin\Debug %___output_dir___%\FreeFurnishing FreeFurnishing.dll Info.json /E
ROBOCOPY %___dev_dir___%\LandingControl\bin\Debug %___output_dir___%\LandingControl LandingControl.dll Info.json /E
ROBOCOPY %___dev_dir___%\MoreSpeed\bin\Debug %___output_dir___%\MoreSpeed MoreSpeed.dll Info.json /E
ROBOCOPY %___dev_dir___%\NoIntruders\bin\Debug %___output_dir___%\NoIntruders NoIntruders.dll Info.json /E
ROBOCOPY %___dev_dir___%\NoSpares\bin\Debug %___output_dir___%\NoSpares NoSpares.dll Info.json /E
ROBOCOPY %___dev_dir___%\Pause\bin\Debug %___output_dir___%\Pause Pause.dll Info.json /E
ROBOCOPY %___dev_dir___%\PowerSaver\bin\Debug %___output_dir___%\PowerSaver PowerSaver.dll Info.json GridManagementConsoleIcon.png PowerSaver.xml /E
ROBOCOPY %___dev_dir___%\Simplify\bin\Debug %___output_dir___%\Simplify Simplify.dll Info.json Simplify.csv /E
ROBOCOPY %___dev_dir___%\SkipIntro\bin\Debug %___output_dir___%\SkipIntro SkipIntro.dll Info.json /E
ROBOCOPY %___dev_dir___%\StorageGuru\bin\Debug %___output_dir___%\StorageGuru StorageGuru.dll Info.json StorageDisable.png StorageEnable.png storage_manifest.txt /E

"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\AutoAlerts.zip %___output_dir___%\AutoAlerts
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\AutoRotateBuildings.zip %___output_dir___%\AutoRotateBuildings
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\AutoConnections.zip %___output_dir___%\AutoConnections
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\BetterHours.zip %___output_dir___%\BetterHours
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\BuildingAligner.zip %___output_dir___%\BuildingAligner
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\CameraOverhaul.zip %___output_dir___%\CameraOverhaul
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\CharacterCam.zip %___output_dir___%\CharacterCam
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\DebugManagerOn.zip %___output_dir___%\DebugManagerOn
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\EternalBot.zip %___output_dir___%\EternalBot
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\FastAirlock.zip %___output_dir___%\FastAirlock
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\FreeBuilding.zip %___output_dir___%\FreeBuilding
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\FreeFurnishing.zip %___output_dir___%\FreeFurnishing
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\LandingControl.zip %___output_dir___%\LandingControl
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\MoreSpeed.zip %___output_dir___%\MoreSpeed
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\NoIntruders.zip %___output_dir___%\NoIntruders
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\NoSpares.zip %___output_dir___%\NoSpares
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\Pause.zip %___output_dir___%\Pause
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\PowerSaver.zip %___output_dir___%\PowerSaver
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\Simplify.zip %___output_dir___%\Simplify
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\SkipIntro.zip %___output_dir___%\SkipIntro
"C:\Program Files\7-Zip\7z.exe" a -tzip %___output_dir___%\StorageGuru.zip %___output_dir___%\StorageGuru

RMDIR %___output_dir___%\AutoAlerts /S /Q
RMDIR %___output_dir___%\AutoRotateBuildings /S /Q
RMDIR %___output_dir___%\AutoConnections /S /Q
RMDIR %___output_dir___%\BetterHours /S /Q
RMDIR %___output_dir___%\BuildingAligner /S /Q
RMDIR %___output_dir___%\CameraOverhaul /S /Q
RMDIR %___output_dir___%\CharacterCam /S /Q
RMDIR %___output_dir___%\DebugManagerOn /S /Q
RMDIR %___output_dir___%\EternalBot /S /Q
RMDIR %___output_dir___%\FastAirlock /S /Q
RMDIR %___output_dir___%\FreeBuilding /S /Q
RMDIR %___output_dir___%\FreeFurnishing /S /Q
RMDIR %___output_dir___%\LandingControl /S /Q
RMDIR %___output_dir___%\MoreSpeed /S /Q
RMDIR %___output_dir___%\NoIntruders /S /Q
RMDIR %___output_dir___%\NoSpares /S /Q
RMDIR %___output_dir___%\Pause /S /Q
RMDIR %___output_dir___%\PowerSaver /S /Q
RMDIR %___output_dir___%\Simplify /S /Q
RMDIR %___output_dir___%\SkipIntro /S /Q
RMDIR %___output_dir___%\StorageGuru /S /Q




