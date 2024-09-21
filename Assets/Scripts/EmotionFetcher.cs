using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class EmotionFetcher : MonoBehaviour
{
    [SerializeField] private float RefreshInterval = 0.1f; // Set the interval for invoking the coroutine
    private string emotionDataUrl = "http://localhost:5000/emotion";

    // Store emotions as public fields
    public float happiness;
    public float sadness;
    public float neutral;
    public float angry;
    public float surprise;

    void Start()
    {
        // Start invoking the coroutine repeatedly every RefreshInterval seconds
        InvokeRepeating("StartEmotionCoroutine", 0f, RefreshInterval);
    }

    void StartEmotionCoroutine()
    {
        StartCoroutine(GetEmotionData());
    }

    private void Update() 
    {
        // Debug the emotions
        /*
        Debug.Log("Happiness: " + happiness);
        Debug.Log("Sadness: " + sadness);
        Debug.Log("Neutral: " + neutral);
        Debug.Log("Angry: " + angry);
        Debug.Log("Surprise: " + surprise);
        */
    }

    IEnumerator GetEmotionData()
    {
        UnityWebRequest request = UnityWebRequest.Get(emotionDataUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("Emotion data: " + request.downloadHandler.text);

            // Parse JSON data
            JObject json = JObject.Parse(request.downloadHandler.text);

            // Emotions as float values
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
}
