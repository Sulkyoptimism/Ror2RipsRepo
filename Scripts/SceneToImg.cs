using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneToImg : MonoBehaviour
{
 

    private RenderTexture renderTexture;
    private Camera renderCamera;

    private int resolution = 4096;

    void Start()
    {
        Debug.Log("Initializing camera and stuff...");

        gameObject.GetComponent(typeof(Camera));

        renderCamera = GameObject.Find("Cam").GetComponent<Camera>();

        renderCamera.enabled = true;
        renderCamera.cameraType = CameraType.Game;
        renderCamera.forceIntoRenderTexture = true;
        renderCamera.aspect = 1.0f;
        renderCamera.targetDisplay = 2;

        renderTexture = new RenderTexture(resolution, resolution, 24);

        renderCamera.targetTexture = renderTexture;


        Debug.Log("Initialized successfully!");
        Debug.Log("Computing level boundaries...");

      
        

        Debug.Log("Computing target image resolution and final setup...");

        Texture2D virtualPhoto = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;

        Debug.Log("Success! Everything seems ready to render!");

        Vector3[] frustumCorners = new Vector3[4];
        renderCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), renderCamera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        int firstX = (int)frustumCorners[0][0];
        int firstY = (int)frustumCorners[0][1];
        int secondX = (int)frustumCorners[2][0];
        int secondY = (int)frustumCorners[2][1];
    
        int diffX = secondX - firstX;
        int diffY = secondY - firstY;

        renderCamera.Render();

        virtualPhoto.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);

   
        Debug.Log("All chunks rendered! Some final adjustments and picture should be saved!");

        RenderTexture.active = null;
        renderCamera.targetTexture = null;

        byte[] bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(@"C:\Users\Mark\Desktop\MapWork\roostMidRender.png", bytes);

        Debug.Log("All done! Always happy to help you :)");
    }
}