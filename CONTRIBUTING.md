# Development setup

## Pre-requisites
To get started with RA3-Tweaks development you will need the following installed:
* [Robot Arena 3](http://store.steampowered.com/app/363530/)
* [Unity 5 Personal Edition](http://unity3d.com/get-unity) (or greater)
* [Visual Studio 2015 Community Edition](https://www.visualstudio.com/) (or greater)
* (Optional) [VSCode](https://code.visualstudio.com/)


## Quick steps
* Clone this repository 
* In VS, open `src\vs.sln` -or- In VSCode open the cloned directory
* In VS, build the solution -or- In VSCode run the build task (with CTRL + SHIFT + B)
* In Unity, open `src\unity` then open the main scene `src\unity\Assets\Workspace.unity`
* In Unity, from the `Assets` menu, select `Build RA3-Tweaks AssetBundles...`
* Run the `ra3-tweaker.exe` application from `out\` (Make sure Robot Arena 3 is not running)
* Once it has run successfully, run `Robot Arena 3`


## Components
The project is split into 3 components that have different uses. 

### Installer
The code lives under `src\installer`
This builds the `ra3-tweakr.exe` console application that is used to inject the C# IL into the Robot Arena 3, Assembly-CSharp.dll.
* Open this project in VS using `src\vs.sln` -or- in VSCode by running `code .` in the root project directory
* Build the solution in VS -or- Run the build task in VSCode

### Tweaks
The code lives under `src\tweaks`
This builds the `ra3-tweaks.dll` assembly that contains the modifications to the main game. It uses C# attributes to specify where code should be injected into the RA3 game code.
* Open this project in VS using `src\vs.sln` -or- in VSCode by running `code .` in the root project directory
* Build the solution in VS -or- Run the build task in VSCode

### Assets
The code lives under `src\unity`
This is a unity project that builds the `ra3-tweaks.asset` asset bundle. This is used to import new Unity components into the RA3 game. Right now that just means a few menu buttons as GameObjects.
* Open this project in Unity using the `src\unity` folder, and then the `Workspace.unity` Unity scene
* Build the asset bundle using the provided editor script by selecting `Build RA3-Tweaks AssetBundles...` from the `Assets` menu in Unity

## Contributing
ToDo

