using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamGetter : MonoBehaviour
{
    WebCamTexture textureOut;
    public Material targetMaterial;

private IEnumerator InitializeCameraWithDelay()
{
    WebCamDevice[] devices = WebCamTexture.devices;
    if (devices.Length > 0)
    {
        WebCamTexture webcamTexture = new WebCamTexture(devices[0].name);
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = webcamTexture;
        }
        yield return new WaitForSeconds(1f);  // Add a delay of 1 second
        webcamTexture.Play();
        textureOut = webcamTexture;

        if (webcamTexture.isPlaying)
        {
            Debug.Log("Webcam feed started successfully.");
        }
        else
        {
            Debug.LogError("Failed to start the webcam feed.");
        }
    }
}

void Start()
{
    StartCoroutine(InitializeCameraWithDelay());
}

    void Update()
    {
        // Update the target material with the webcam texture if it exists
        if (textureOut != null)
        {
            Debug.Log("Updating target material with webcam texture.");
            if (targetMaterial != null)
            {
                targetMaterial.SetTexture("_MainTex", textureOut);
            }
            else
            {
                Debug.LogError("Target material is not assigned. Cannot update texture.");
            }
        }
    }
}
