using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

public class EmotionFetcher : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float RefreshInterval = 0.1f; // Refresh interval for emotions
    [SerializeField] private float FrameRefreshAmount = 0.5f; // Refresh interval for webcam frames

    [Header("URLs")]
    private string emotionDataUrl = "http://localhost:5000/emotion"; // Emotion data URL
    private string frameDataUrl = "http://localhost:5000/frame"; // Webcam frame data URL

    [Header("Emotion Data")]
    public float happiness;
    public float sadness;
    public float neutral;
    public float angry;
    public float surprise;

    [Header("Webcam Frame")]
    public Renderer targetRenderer; // Renderer to apply webcam texture to

    void Start()
    {
        // Start invoking the coroutines for emotion and webcam frame fetching
        InvokeRepeating("StartEmotionCoroutine", 0f, RefreshInterval);
        StartCoroutine(GetWebcamFrame());
    }

    // Start the emotion data coroutine
    void StartEmotionCoroutine()
    {
        StartCoroutine(GetEmotionData());
    }

    // Coroutine to fetch emotion data
    IEnumerator GetEmotionData()
    {
        UnityWebRequest request = UnityWebRequest.Get(emotionDataUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the emotion data from JSON
            JObject json = JObject.Parse(request.downloadHandler.text);
            happiness = (float)json["happy"];
            sadness = (float)json["sad"];
            neutral = (float)json["neutral"];
            angry = (float)json["angry"];
            surprise = (float)json["surprise"];
        }
        else
        {
            Debug.LogError("Error fetching emotion data: " + request.error);
        }
    }

    // Coroutine to fetch webcam frame
    IEnumerator GetWebcamFrame()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Get(frameDataUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Decode the base64 frame into a byte array
                string frameBase64 = JsonUtility.FromJson<WebcamFrame>(request.downloadHandler.text).frame;
                byte[] imageData = Convert.FromBase64String(frameBase64);

                // Convert byte[] to Texture2D
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);

                // Apply the texture to the target material
                if (targetRenderer != null)
                {
                    targetRenderer.material.mainTexture = texture;
                }
            }
            else
            {
                Debug.LogError("Error fetching frame data: " + request.error);
            }

            yield return new WaitForSeconds(FrameRefreshAmount);
        }
    }

    [Serializable]
    public class WebcamFrame
    {
        public string frame;
    }
}
