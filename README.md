# RA3 Tweaks

RA3-Tweaks is a small application that can be used to apply code modifications to [Robot Arena 3](http://store.steampowered.com/app/363530/).


## Usage
To apply the tweaks:
* Run `ra3-tweaker.exe`


Optional parameters:
* -?                 Display this help message
* -i <path>          Full path to the RA3 install directory
* -t <path>          Full path to tweak dll
* -a <path>          Full path to the asset bundles
* -r                 Restore the modified file back to the original RA3 state


## How it works
RA3-Tweaks uses [Mono.Cecil](https://github.com/jbevain/cecil) to re-write some of the C# IL code that is used by the game.
The re-written code is inserted to make calls out to another C# dll that can add new functionality to the game or change existing methods.
It also uses [Unity](http://unity3d.com/) [asset bundles](http://docs.unity3d.com/Manual/AssetBundlesIntro.html) to add new UI and Unity assets.
This work was inspired by this [Q&A Post](http://steamcommunity.com/games/363530/announcements/detail/853808393799416587) from the RA3 developers:
> I'd also like to ask if any modding tools will be supplied by Octopus Tree 
> - Not at initial release. Between us, Unity games are HIGHLY MODDABLE without any support required on the Dev side. Clever folks can actually hook right into the C# mono code.


## Current Tweaks
The current code changes that are applied are:
* Added RA3-Tweaks Menu button
* An example new component - the epic dagger
* Ability to write out the components.json file to disk
* Ability to export the models as *.obj files
* Note: Exported files are output to `<Robot Arena 3 Directory>\RobotArena3_Data\Managed\ra3-tweaks\exported`


## Future improvements
Lots to do, feel free to contribute!
