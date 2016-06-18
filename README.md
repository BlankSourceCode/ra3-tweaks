# RA3 Tweaks

RA3-Tweaks is a small application that can be used to apply code modifications to [Robot Arena 3](http://store.steampowered.com/app/363530/).


## Usage
To apply the tweaks:
* Run `ra3-tweaks.exe`


Optional parameters:
* -?                 Display this help message
* -i <path>          Full path to the RA3 install directory
* -t <path>          Full path to tweak dll
* -a <path>          Full path to the asset bundles
* -r                 Restore the modified file back to the original RA3 state


## How it works
RA3-Tweaks uses [Mono.Cecil](https://github.com/jbevain/cecil) to re-write some of the C# IL code that is used by the game.
The re-written code is inserted to make calls out to another C# dll that can add new functionality to the game.
It also uses [Unity](http://unity3d.com/) [asset bundles](http://docs.unity3d.com/Manual/AssetBundlesIntro.html) to add new UI and Unity assets.


## Current Tweaks
The current code changes that are applied are:
* Added RA3-Tweaks Menu button
* Ability to write out the components.json file to disk
* Ability to export the models as *.obj files
* Note: Exported files are outputted to `<Robot Arena 3 Directory>\RobotArena3_Data\Managed\ra3-tweaks\exported`


## Future improvements
The system could probably be expanded to import new components and arenas into the game
