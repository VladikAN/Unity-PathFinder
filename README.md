## Source installation
1) Go to into your Unity3D installation folder (in my case its c:\Program Files\Unity\Editor\Data\Managed).

2) Locate two dll files: UnityEngine.dll and UnityEditor.dll.

3) Copy them into packages\Unity folder.

This files are not placed into this repository because of difference between my unity version and yours.

## Unity3D-Path-Finder
Unity 3D path found project, assets will look up a valid path through 2d maze. For this moment project includes **Wave** and **Jump Point** search algorithms.

![Sample 1](https://github.com/VladikAN/Unity3D-Path-Finder/blob/master/Pics/sample-1.gif "Sample 1")
![Sample 2](https://github.com/VladikAN/Unity3D-Path-Finder/blob/master/Pics/sample-2.gif "Sample 2")

## Example
### Initial map
Project have a single demo scene. Demo scene contains of: 10x10 maze, 6 walls and single camera (with possibility to switch between existed algorithms).

![Initial Map](https://github.com/VladikAN/Unity3D-Path-Finder/blob/master/Pics/initial.png "Initial Map")

Assets will detect all obstacles and allow you to explore 2d maze in debug view:

![Maze Map](https://github.com/VladikAN/Unity3D-Path-Finder/blob/master/Pics/maze.png "Maze Map")

### Wave
**Wave** work result:

![Wave Result](https://github.com/VladikAN/Unity3D-Path-Finder/blob/master/Pics/wave.png "Wave Result")

* blue circles - start and end points;
* red circles - target control points;
* red line - movement path.

### Jump Point
**Jump Point** work result:

![Jump Point Result](https://github.com/VladikAN/Unity3D-Path-Finder/blob/master/Pics/jump-point.png "Jump Point Result")

* blue circles - start and end points;
* yellow circles - posible control points;
* red circles - target control points;
* red line - movement path.
