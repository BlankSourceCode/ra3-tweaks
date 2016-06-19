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
* Run the `ra3-tweaker.exe` application from `out\` (Make sure Robot Arena 3 is not running) (You may need to specify full paths using the command line options, see -?)
* Once it has installed successfully, run `Robot Arena 3`


## Components
The project is split into 4 components that have different uses. 

### Installer
The code lives under `src\installer`

This builds the `ra3-tweaker.exe` console application that is used to inject the C# IL into the Robot Arena 3, Assembly-CSharp.dll.
* Open this project in VS using `src\vs.sln` -or- in VSCode by running `code .` in the root project directory
* Build the solution in VS -or- Run the build task in VSCode

### Tweaks
The code lives under `src\tweaks`

This builds the `ra3-tweaks.dll` assembly that contains the modifications to the main game. It uses C# attributes to specify where code should be injected into the RA3 game code.
* Open this project in VS using `src\vs.sln` -or- in VSCode by running `code .` in the root project directory
* Build the solution in VS -or- Run the build task in VSCode
* You may need to update the `<RA3InstallDir>` property inside `tweaks.csproj` to point to the correct Robot Arena 3 directory

### Assets
The code lives under `src\unity`

This is a unity project that builds the `ra3-tweaks.asset` asset bundle. It is used to import new Unity components into the RA3 game. Right now that just means a few menu buttons as GameObjects and an example component.
* Open this project in Unity using the `src\unity` folder, and then the `Workspace.unity` Unity scene
* Build the asset bundle using the provided editor script by selecting `Build RA3-Tweaks AssetBundles...` from the `Assets` menu in Unity

### Debugger
The code lives under `src\debugger`

This contains two projects to enable debugging your injected C# code (unfortunately you cannot debug RA3 itself but you can see variables in watches).
This is a version of [dynity](https://github.com/HearthSim/dynity) modified to work with RA3. See their github page for more instructions on how to use it.

But a quick start guide is:
* Build the solution to produce debug.exe and dynity.dll
* Set the environment variable UNITY_GIVE_CHANCE_TO_ATTACH_DEBUGGER to 1 (restart RA3 if it was running)
* Run `RobotArena3.exe` (you should see a message box asking you to attach a debugger)
* Run `debug.exe` (you should see a new console window show that the mono debugger has started)
* Click OK on the message box
* In Visual Studio, select `Debug` -> `Attach Unity Debugger`
* Click the `Input IP` button (the info should be auto populated correctly - if not edit it)
* Click OK
* Now you should be able to add breakpoints in your RA3-Tweaks C# code

## Contributing
ToDo

