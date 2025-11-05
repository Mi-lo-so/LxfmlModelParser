using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class BoneInfo
{
    public string Id  = "";

    /// <summary>
    ///     For example of type: "-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,4.4,0.02119982,-11.2"
    /// </summary>
    public string transformation = "";
    
    /// <summary>
    /// Loose and fast, very naive attempt at applying position, rotation and tranformation
    /// to a gameObject based on a bone's transformation property
    /// </summary>
    /// <param name="standInBrickObject">object to apply transformation to</param>
    /// <param name="transformation">the string of transformation, separated by ','</param>
    public void ApplyTransformation(GameObject standInBrickObject, string transformation)
    {
        /* assuming
        | Rxx  Rxy  Rxz  Tx |   ← Rotation on the x-axis, then x-position last
        | Ryx  Ryy  Ryz  Ty |   ← Rotation on the y-axis, then y-position last
        | Rzx  Rzy  Rzz  Tz |   ← Rotation on the z-axis, then y-position last
         */
        
        // Try to split the tranformation into floats
        var values = transformation.Split(',');
        if (values.Length != 12)
        {
            Debug.LogError("Invalid transformation matrix: " + transformation);
            return;
        }

        var f = new float[12];
        for (var i = 0; i < 12; i++)
            f[i] = float.Parse(values[i], System.Globalization.CultureInfo.InvariantCulture);

        // Build transformation Matrix3x4 from the transformation string
        // Assuming the last "row" is the actual position on the axis (positions 9,10,11)
        // and (0,1,2), (3,4,5), (6,7,8) are the rotations on the x,y,z axis
        var transformationMatrix = new Matrix4x4();
        transformationMatrix.m00 = f[0]; transformationMatrix.m01 = f[1]; transformationMatrix.m02 = f[2];  transformationMatrix.m03 = f[9];
        transformationMatrix.m10 = f[3]; transformationMatrix.m11 = f[4]; transformationMatrix.m12 = f[5];  transformationMatrix.m13 = f[10];
        transformationMatrix.m20 = f[6]; transformationMatrix.m21 = f[7]; transformationMatrix.m22 = f[8];  transformationMatrix.m23 = f[11];
        
        // Assume position and apply to object
        standInBrickObject.transform.position = transformationMatrix.GetColumn(3);

        // Extract rotation and scale (magnitude of the entire row on 0, 1, and 2)
        // e.g. 0 is sqrt( f[0]² + f[3]² + f[6]² )
        var scale = new Vector3(
            transformationMatrix.GetColumn(0).magnitude,
            transformationMatrix.GetColumn(1).magnitude,
            transformationMatrix.GetColumn(2).magnitude
        );

        // Assume first two columns represent the rotation direction
        var rotation = Quaternion.LookRotation(
            transformationMatrix.GetColumn(2).normalized, // forward - z axis
            transformationMatrix.GetColumn(1).normalized // up - y axiz
        );

        standInBrickObject.transform.rotation = rotation;
        standInBrickObject.transform.localScale = scale;
    }
    
}

[Serializable]
public class BrickInfo
{
    public string id = "";
    public string designId = "";
    public List<PartInfo> parts = new();
}

[Serializable]
public class PartInfo
{
    public string id = "";
    public string designId  = "";
    public List<MaterialInfo> materials = new();
    public List<BoneInfo> bones= new();
}

[Serializable]
public class MaterialInfo
{
    public string id;
    public string layer;
}

[Serializable]
public class ModelInfo
{
    public string modelId;
    public string name;
    public string description;
    public int totalBricks;
    public int totalParts;
    public List<BrickInfo> bricks; 
    public List<MaterialInfo> materials; 
}
