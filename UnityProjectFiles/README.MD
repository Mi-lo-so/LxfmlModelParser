# Unity Project

Very simple and naive project that attempts to plot simple GameObject cubes 
based on the transform coordinates of the Bone from an LXFML file.

# Instructions
- Follow the instruction in LxfmlModelParser web appp project, and run it
- Copy the Unity files in this project into a (new) Unity project
- Open the scene SampleScene
- Check that the ModelLoader object has:
  - Api Url: http://localhost:5253/api/lxfml/upload (or whatever your url turned into when running LxfmlModelParser)
  - Model Folder: Models
  - Part Prefab: Cube (if not set, it is a Cube object in the Models folder)
- Run the Unity SampleScene project
- The ModelLoader object should start the scene by trying to upload the everyoneisawesome.lxfml file 
and using the returned data to create and position gameobjects based on the Bone's transform string.