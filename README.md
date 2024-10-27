# Revit API - Parameter Scanner Add-In

# Overview

This add-in allows you to either select or isolate in the active view all model elements that match the specified Parameter Name and Value

# Prerequisites

Revit version 2020 or 2021
.Net Framework 4.8

# Installation

1. Clone the repository
2. Build the solution in Visual Studio to compile the add-in
3. Copy the resulting .dll file to your Revit add-ins folder (e.g. C:\ProgramData\Autodesk\Revit\Addins\2020\)
4. Copy the manifest.addin file in the same directory

# Usage

1. Open Revit 
2. A warnig window will appear for unsigned add-in, select 'Always Load'
3. A new tab called 'Parameters' should appear, which contains a Parameter Scanner button
4. When clicked, a new window will appear for you to enter the parameter name and value that you need to find on the project
5. Next, you can click Isolate in View or Select button to filter those elements in the view (the addin only supports Floor Plans, Ceiling Plans and 3D views)