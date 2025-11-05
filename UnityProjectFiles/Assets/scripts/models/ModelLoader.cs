using System;
using System.Collections;
using System.IO;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class UploadModels : MonoBehaviour
{
     [SerializeField] private string apiUrl = "http://localhost:5253/api/lxfml/upload";
    [SerializeField] private string modelFolder = "Models"; // relative to Application.dataPath
    [SerializeField] private GameObject partPrefab; // prefab to spawn


    private void Start()
    {
        StartCoroutine(UploadAllLxfmlFiles());
    }

    private IEnumerator UploadAllLxfmlFiles()
    {
        string folderPath = Path.Combine(Application.dataPath, modelFolder);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"Model folder not found: {folderPath}");
            yield break;
        }

        string[] lxfmlFiles = Directory.GetFiles(folderPath, "*.lxfml", SearchOption.AllDirectories);
        if (lxfmlFiles.Length == 0)
        {
            Debug.LogWarning($"No .lxfml files found in {folderPath}");
            yield break;
        }

        foreach (string filePath in lxfmlFiles)
        {
            yield return UploadLxfmlFile(filePath);
        }
    }

    private IEnumerator UploadLxfmlFile(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        string fileName = Path.GetFileName(filePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, fileName, "application/octet-stream");

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, form))
        {
            Debug.Log($"📤 Uploading: {fileName}");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"❌ Upload failed for {fileName}: {request.error}");
                yield break;
            }

            string jsonResponse = request.downloadHandler.text;
            Debug.Log($"📥 Response: {jsonResponse}");
            // "modelId":"6bc9f5ef-df24-46d2-b6fe-80023a480cb6","name":"everyoneisawesome.lxfml","description":"","totalBricks":346,"totalParts":412

            try
            {
                Debug.Log("Trying to parse...");
                ModelInfo modelInfo = JsonUtility.FromJson<ModelInfo>(jsonResponse);
                Debug.Log("Going to spawn...");
                SpawnModelParts(modelInfo);
                if (modelInfo != null)
                {
                    Debug.Log($"✅ Model parsed: {modelInfo.name} | Bricks: {modelInfo.totalBricks} | Parts: {modelInfo.totalParts}");
                }
                else
                {
                    Debug.LogWarning($"⚠️ Could not parse response into ModelInfo for {fileName}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"⚠️ JSON parse error for {fileName}: {ex.Message}");
            }
        }
    }
    
    private void SpawnModelParts(ModelInfo model)
    {
        Debug.Log("the bricks:"+ model.bricks.Count);
        foreach (BrickInfo brick in model.bricks)
        {
            Debug.Log("the parts:"+ brick.parts.Count);
            foreach (PartInfo part in brick.parts)
            {
                Debug.Log("the bones:"+ part.bones);
                if (part.bones != null && partPrefab != null)
                {
                    foreach (BoneInfo bone in part.bones)
                    {
                        Debug.Log("the bone:"+ bone.transformation);
                        //example: "-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,4.4,0.02119982,-1
                        GameObject instance = Instantiate(partPrefab);
                        bone.ApplyTransformation(instance, bone.transformation);
                        instance.name = $"{brick.designId}_{part.id}";
                    
                    }
                }
            }
        }
    }

    
}

