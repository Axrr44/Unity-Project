using BestHTTP;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AvatarCreator : MonoBehaviour
{
    public TMP_InputField userImageURLInputField;
    public Button createAvatar;
    public SkinnedMeshRenderer avatarSkinnedMeshRenderer;

    [Header("Lights")]
    public Light _light1;
    public Light _light2;
    public Light _light3;
    public Light _light4;
    public Light _light5;
    public TMP_InputField _lightIntensityInputField1;
    public TMP_InputField _lightIntensityInputField2;
    public TMP_InputField _lightIntensityInputField3;
    public TMP_InputField _lightIntensityInputField4;
    public TMP_InputField _lightIntensityInputField5;
    public TMP_InputField _lightColorInputField1;
    public TMP_InputField _lightColorInputField2;
    public TMP_InputField _lightColorInputField3;
    public TMP_InputField _lightColorInputField4;
    public TMP_InputField _lightColorInputField5;
    public Button _setupLights;

    [Header("Skin Tones")]
    public Material _mouthMaterial;
    public Material _lipsMaterial;
    public Material _legsMaterial;
    public Material _headMaterial;
    public Material _bodyMaterial;
    public Material _armsMaterial;

    public Texture _mouthTexture;
    public Texture _lipsTexture;
    public Texture _legsTexture;
    public Texture _headTexture;
    public Texture _bodyTexture;
    public Texture _armsTexture;

    public TMP_InputField _mouthMaterialInputField;
    public TMP_InputField _lipsMaterialInputField;
    public TMP_InputField _legsMaterialInputField;
    public TMP_InputField _headMaterialInputField;
    public TMP_InputField _bodyMaterialInputField;
    public TMP_InputField _armsMaterialInputField;


    void Start()
    {
        // Allow insecure connections (not recommended for production)
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    }
    public void CreateAvatar()
    {
        var userImageURL = userImageURLInputField.text;
        if (string.IsNullOrEmpty(userImageURL))
            return;

        CreateAvatarRequestRoot request = new CreateAvatarRequestRoot();
        request.Image = userImageURL;

        string jsonRequestBody = JsonConvert.SerializeObject(request);
        ResetAvatar();
        CallCreateAvatarAPI(jsonRequestBody);
    }
    public void SetupLights()
    {
        ///////////////////////////////////////////
        if (_lightIntensityInputField1.text != "")
        {
            if(float.TryParse(_lightIntensityInputField1.text, out float value))
            {
                _light1.intensity = value;
            }
        }
        if(_lightColorInputField1.text != "")
        {
            if (ColorUtility.TryParseHtmlString(_lightColorInputField1.text, out Color color))
            {
                _light1.color = color;
            }
        }
        ///////////////////////////////////////////
        if (_lightIntensityInputField2.text != "")
        {
            if (float.TryParse(_lightIntensityInputField2.text, out float value))
            {
                _light2.intensity = value;
            }
        }
        if (_lightColorInputField2.text != "")
        {
            if (ColorUtility.TryParseHtmlString(_lightColorInputField2.text, out Color color))
            {
                _light2.color = color;
            }
        }
        ///////////////////////////////////////////
        if (_lightIntensityInputField3.text != "")
        {
            if (float.TryParse(_lightIntensityInputField3.text, out float value))
            {
                _light3.intensity = value;
            }
        }
        if (_lightColorInputField3.text != "")
        {
            if (ColorUtility.TryParseHtmlString(_lightColorInputField3.text, out Color color))
            {
                _light3.color = color;
            }
        }
        ///////////////////////////////////////////
        if (_lightIntensityInputField4.text != "")
        {
            if (float.TryParse(_lightIntensityInputField4.text, out float value))
            {
                _light4.intensity = value;
            }
        }
        if (_lightColorInputField4.text != "")
        {
            if (ColorUtility.TryParseHtmlString(_lightColorInputField4.text, out Color color))
            {
                _light4.color = color;
            }
        }
        ///////////////////////////////////////////
        if (_lightIntensityInputField5.text != "")
        {
            if (float.TryParse(_lightIntensityInputField5.text, out float value))
            {
                _light5.intensity = value;
            }
        }
        if (_lightColorInputField5.text != "")
        {
            if (ColorUtility.TryParseHtmlString(_lightColorInputField5.text, out Color color))
            {
                _light5.color = color;
            }
        }
        ///////////////////////////////////////////
    }

    public void ResetTextures()
    {
        _mouthMaterial.mainTexture = _mouthTexture;
        _lipsMaterial.mainTexture = _lipsTexture;
        _legsMaterial.mainTexture = _legsTexture;
        _headMaterial.mainTexture = _headTexture;
        _bodyMaterial.mainTexture = _bodyTexture;
        _armsMaterial.mainTexture = _armsTexture;
    }
    public void DownloadSkinTones()
    {
        if(_mouthMaterialInputField.text != "")
        {
            StartCoroutine(LoadSkinToneTexture(_mouthMaterialInputField.text, _mouthMaterial));
        }
        if (_lipsMaterialInputField.text != "")
        {
            StartCoroutine(LoadSkinToneTexture(_lipsMaterialInputField.text, _lipsMaterial));
        }
        if (_legsMaterialInputField.text != "")
        {
            StartCoroutine(LoadSkinToneTexture(_legsMaterialInputField.text, _legsMaterial));
        }
        if (_headMaterialInputField.text != "")
        {
            StartCoroutine(LoadSkinToneTexture(_headMaterialInputField.text, _headMaterial));
        }
        if (_bodyMaterialInputField.text != "")
        {
            StartCoroutine(LoadSkinToneTexture(_bodyMaterialInputField.text, _bodyMaterial));
        }
        if (_armsMaterialInputField.text != "")
        {
            StartCoroutine(LoadSkinToneTexture(_armsMaterialInputField.text, _armsMaterial));
        }
    }
    private IEnumerator LoadSkinToneTexture(string url, Material material)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("www.error:" + www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            material.mainTexture = myTexture;
        }
    }
    private void ResetAvatar()
    {
        var faceMaterial = avatarSkinnedMeshRenderer.sharedMaterials.FirstOrDefault(mat => mat.name == "Face");
        faceMaterial.mainTexture = null;

        if (avatarSkinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is not assigned!");
            return;
        }

        for (int i = 0; i < avatarSkinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
        {
            avatarSkinnedMeshRenderer.SetBlendShapeWeight(i, 0f);
        }
    }

    private void CallCreateAvatarAPI(string jsonRequestBody)
    {
        var url = "https://api.28-app.com/api/Face/AvatarCreate";
        var request = new HTTPRequest(new Uri(url), HTTPMethods.Post, OnDownloadGenerated3DHeadResponse);
        request.SetHeader("accept", "*/*");
        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        request.RawData = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        request.Send();
    }

    private void OnDownloadGenerated3DHeadResponse(HTTPRequest originalRequest, HTTPResponse response)
    {
        Debug.Log("Request sent successfully");
        Debug.Log("Response: " + response.DataAsText);

        var _response = JsonConvert.DeserializeObject<Root>(response.DataAsText);
        SetBlendShapesFromRepresentation(_response);
        SetAvatarFaceMaterial(_response);
    }

    private void SetAvatarFaceMaterial(Root response)
    {
        var faceTexture = response.Data.Representation.Textures.HeadTexture;
        var faceMaterial = avatarSkinnedMeshRenderer.sharedMaterials.FirstOrDefault(mat => mat.name == "Face");

        StartCoroutine(LoadFaceTexture(faceTexture, faceMaterial));
    }

    private IEnumerator LoadFaceTexture(string url, Material material)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("www.error:" + www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            material.mainTexture = myTexture;
        }
    }

    public void SetBlendShapesFromRepresentation(Root response)
    {
        if (avatarSkinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is not assigned!");
            return;
        }

        for (int i = 0; i < response.Data.Representation.Morphs.Labels.Count; i++)
        {
            string blendShapeName = response.Data.Representation.Morphs.Labels[i];
            float blendShapeValue = (float)response.Data.Representation.Morphs.Values[i];

            // Find blend shape index by name
            int blendShapeIndex = avatarSkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeName);
            if (blendShapeIndex != -1)
            {
                // Set blend shape value
                avatarSkinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeValue);
            }
            else
            {
                Debug.LogWarning("Blend shape not found: " + blendShapeName);
            }
        }
    }
    public static bool MyRemoteCertificateValidationCallback(System.Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
    {
        // Return true to allow insecure connections
        return true;
    }
}
