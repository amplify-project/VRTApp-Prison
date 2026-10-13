# VRTApp-Prison

This repository contains a social VR application showcasing a virtual visit in 1970s female political prison in Francoist Spain. The experience is designed to be visited in pairs, and the visitors are represented using live volumetric videos (which is achieved by using VR2Gather unity package). During the visit the users move through various spaces of the prison, where they can explore, listen to the stories of ex-inmates (represented by volumetric video) and interact with their personal objects from the time of imprisonment - each of them having a symbolic meaning. These objects are represented either by gaussian splats or are AI generated from videos/photos of actual objects. The interaction with the objects is multimodal - the users can pick them up or touch them, which triggers sound effects and voiceovers. 

To be able to run the experience with live volumetric representation of the users, a specific hardware setup is required. You can find the description of hardware requirement and setup here: https://www.dis.cwi.nl/vr2gather/hardware/

More information on VR2Gather itself can be found at its repository, <https://github.com/cwi-dis/VR2Gather>.

The Unity project here should run on desktop computers (Windows, Mac, Linux) either with keyboard/mouse/screen or with an attached tethered HMD (Meta Quest, Vive).

The project should also run natively on the Meta Quest.

## Getting started, desktop

Full instructions are in the <https://github.com/cwi-dis/VR2Gather> repository, please refer to those. But here are the quick steps:

- Ensure you have `git` installed, and you have done `git lfs install`.
- Clone this repository.
- Install  the _cwipc_ point cloud package. Instructions are can be found at <https://github.com/cwi-dis/cwipc>
- Open the project in the Unity Editor.
- Select the `VRTLogin` scene.
- Play.

## Getting started, Meta Quest

- Ensure you have `git` installed, and you have done `git lfs install`.
- Clone this repository.
- Ensure you have Android build support included in your Unity install. Unity Hub can do this for you.
- No need to install _cwipc_, the native support for Android is included.
- Open the project in the Unity Editor.
- In Player Settings, select the Android platform, and select your HMD in the target popup.
- Build.
- Run. 
