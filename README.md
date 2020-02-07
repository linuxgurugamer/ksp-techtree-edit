TED - KSP Tech Tree Editor - v0.3
=================

Description
-----
An external, visual, point-and-click tool for editing and creating custom tech trees for Kerbal Space Program. The tool generates the text files used by KSP 1.04+ and the YongeTech plugins.

Installation
-----
1. Unpack the archive in any location.
2. Launch TED-v[x.x].exe

Troubleshooting
-----
- You may be missing the .NET 4.5 framework. [Download and install from here](http://www.microsoft.com/en-us/download/confirmation.aspx?id=42643).

Usage
-----

1. **Startup**
  1. Start by selecting your KSP root folder, usually that's `C:\Program Files (x86)\Steam\SteamApps\common\Kerbal Space Program` but it may differ on your machine
2. **Load**
  2. *(Option 1)* Load an existing tech tree file (*.cfg*)
     - **Make sure you select the right format!**
  3. *(Option 2)* Create a new tech tree from scratch
     - Click on **New Blank Tree**
  4. *(Option 4)* Create a new tech tree starting from the Stock tech
     - Click on **New Stock Tree**

3. **Edit**
   1. Click on a node to edit it's values on the right (the sidebar).
   2. Move nodes around by draging.
   2. Add or remove parts from nodes at the bottom of the sidebar.
      * The top section indicates the parts assigned to that node.
      * The bottom section is a list of all detected parts in your KSP folder.
   3. Press `DELETE` to delete a node.
   4. Press `SHIFT+CLICK` to add a blank node on an empty space
   5. Select a node and press `CTRL+CLICK` on *another* node to link or unlink parents.
      - The little cirlce indicates the direction of the link.

4. **Save**
   1. Click **Save (Stock)** to save in original ksp format.
   2. Click **Save (YongeTech)** to save in YongeTech format.
   3. You will be prompted to select the filename and where to save.
   4. **NOTE:** It does *not* matter what file the tree was when you loaded, you can save in any format (this can be used to convert a tree from one format to another!)

5. **Load into KSP**
   1. **Follow the corresponding mod instructions!**
   2. This has nothing to do with TED, TED only generates the files.
   3. Again, see the corresponding mods instructions: [YongeTech](http://forum.kerbalspaceprogram.com/threads/125338-1-0-4-YongeTech-Tech-Trees-Plugin-(v1-3))
   
Known Issues
-----

- It looks terrible! UI work has not properly started yet, this is a "skin-less" version of the application.
- Lots and lots of bugs. Yes, this is early alpha, there are LOTS of bugs. Please report and I'll fix as soon as I can.

