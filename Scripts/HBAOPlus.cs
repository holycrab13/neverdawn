using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class HBAOPlus : MonoBehaviour
{
    public enum BlurRadiusMode
    {
        BLUR_RADIUS_2,
        BLUR_RADIUS_4,
        BLUR_RADIUS_8,
    }

    [SerializeField] private Shader fetchDepthShader; // Use a dummy shader to explicitly binds the _CameraDepthTexture SRV to a register.
    [SerializeField] private Shader renderAoShader;

    private bool useNormalTexture = false; //Not working.

    public float radius = 0.2f;
    public float bias;
    public float powerExponent = 1.0f;
    public bool enableBlur = true;
    public float blurSharpness = 1.0f;
    public BlurRadiusMode blurRadiusMode;

    private static Material fetchDepthMaterial;
    private static Material renderAoMaterial;

    private RenderTexture output;
    private RenderTexture normals;

	// For lack of a better way to do this... 
#if UNITY_5_0
	private const string PluginName = "HBAO_Plugin.x64";
#else
    private const string PluginName = "HBAO_Plugin.x64";
#endif

	private Camera _camera;
	private Camera Camera 
	{
		get 
		{
			if (_camera == null) 
			{
				_camera = GetComponent<Camera> ();
			}
			return _camera;
		}
	}

    // The block of code below is a neat trick to allow for calling into the debug console from C++
	[DllImport(PluginName)]
    private static extern void LinkDebug([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr debugCal);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void DebugLog(string log);

    private static readonly DebugLog debugLog = DebugWrapper;
    private static readonly IntPtr functionPointer = Marshal.GetFunctionPointerForDelegate(debugLog);

    private static void DebugWrapper(string log) { Debug.Log(log); }

	[DllImport(PluginName, CallingConvention = CallingConvention.StdCall)]
    private static extern int GetEventID();

	[DllImport(PluginName, CallingConvention = CallingConvention.StdCall)]
	private static extern void SetAoParameters( float Radius,
	                                            float Bias,
	                                            float PowerExponent,
	                                            bool EnableBlur,
	                                            int BlurRadiusMode,
	                                            float BlurSharpness,
	                                            int BlendMode );

	[DllImport(PluginName, CallingConvention = CallingConvention.StdCall)]
	private static extern void SetInputData(    float MetersToViewSpaceUnits,
                                                float[] pProjectionMatrix,
												float[] pWorldToViewMatrix, 
	                                            float height, 
	                                            float width,
	                                            float topLeftX,
	                                            float topLeftY,
	                                            float minDepth,
	                                            float maxDepth,
												bool useNormals );

	[DllImport(PluginName, CallingConvention = CallingConvention.StdCall)]
	private static extern void SetNormalsData(IntPtr pNormalsTexture);

	[DllImport(PluginName, CallingConvention = CallingConvention.StdCall)]
    private static extern void SetOutputData(IntPtr pOutputTexture);

    private void Start()
    {
        LinkDebug(functionPointer); // Hook our c++ plugin into Unitys console log.
    }

    private void OnEnable() 
    {
        if (renderAoMaterial == null)   renderAoMaterial    = new Material(renderAoShader);
		if (fetchDepthMaterial == null) fetchDepthMaterial = new Material(fetchDepthShader);
	}

    private void OnDisable()
    {
        if (renderAoMaterial != null)   DestroyImmediate(renderAoMaterial);
        if (fetchDepthMaterial != null) DestroyImmediate(fetchDepthMaterial);

		if (output != null)	 DestroyImmediate(output);
		if (normals != null) DestroyImmediate(normals);
    }

    private void Update()
    {
		Camera.depthTextureMode |= DepthTextureMode.Depth;

		if (useNormalTexture)
		{
			Camera.depthTextureMode |= DepthTextureMode.DepthNormals;  
		}
    }

    // Perform AO immediately after opaque rendering.
    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
        if (output == null || output.width != Screen.width || output.height != Screen.height)
        {
            output = new RenderTexture(Screen.width, Screen.height, 0);
            output.Create(); // Must call create or ptr will be null.
            SetOutputData(output.GetNativeTexturePtr());
        } 

		if (useNormalTexture)
		{
			if (normals == null || normals.width != Screen.width || normals.height != Screen.height)
			{
				normals = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
				normals.Create(); // Must call create or ptr will be null.
				SetNormalsData(normals.GetNativeTexturePtr());
			}

			fetchDepthMaterial.SetMatrix("_Camera2World", Camera.cameraToWorldMatrix);
			Graphics.Blit(null, normals, fetchDepthMaterial, 0);
		}

        SetAoParameters(radius, bias, powerExponent, enableBlur, (int)blurRadiusMode, blurSharpness, 0);

		// Convert matrices to float arrays.
		float[] projMatrixArr = new float[16];
		float[] viewMatrixArr = new float[16];
		{
			Matrix4x4 projMatrix = GL.GetGPUProjectionMatrix(Camera.projectionMatrix, false);
			Matrix4x4 viewMatrix = Camera.cameraToWorldMatrix;// transform.worldToLocalMatrix;
			for (int i = 0; i < 16; i++)
			{
				projMatrixArr[i] = projMatrix[i];
				viewMatrixArr[i] = viewMatrix[i];
			}
		}

		SetInputData(1.0f, projMatrixArr, viewMatrixArr, (float)Screen.height, (float)Screen.width, 0, 0, 0.0f, 1.0f, useNormalTexture);
        
		fetchDepthMaterial.SetPass(1); // Here Unity will bind the _CameraDepthTexture SRV used in this shader to the explicit register t99, which we can fetch in our plugin using ID3D11DeviceContext::PSGetShaderResources(...)
        
        // Call our render method from the AO plugin.
        GL.IssuePluginEvent(GetEventID());

        renderAoMaterial.SetTexture("_AoResult", output);
        renderAoMaterial.SetTexture("_MainTex", source);
		Graphics.Blit(source, destination, renderAoMaterial);
	}
}
