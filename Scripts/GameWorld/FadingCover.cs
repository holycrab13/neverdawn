using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FadingCover : MonoBehaviour
{

    private BoxCollider[] colliders;

    private float targetAlpha;
    private bool prevContainsCharacter;
    private float currentAlpha;
    private float lerp;

    [SerializeField]
    private float fadeSpeed = 3.0f;

    [SerializeField]
    private float minAlpha;

    private List<Material> materials;

    private List<MeshRenderer> shadowRenderer;
    private List<MeshRenderer> solidRenderer;

    void Start()
    {
        colliders = GetComponents<BoxCollider>();
        materials = new List<Material>();
        shadowRenderer = new List<MeshRenderer>();
        solidRenderer = new List<MeshRenderer>();

        solidRenderer.AddRange(GetComponentsInChildren<MeshRenderer>());
        solidRenderer.ForEach(r => r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off);

        foreach (MeshRenderer renderer in solidRenderer)
        {
            Transform rendererTransform = renderer.transform;

            GameObject go = new GameObject("Shadow_" + renderer.gameObject.name);
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = renderer.GetComponent<MeshFilter>().sharedMesh;

            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterials = renderer.sharedMaterials;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            shadowRenderer.Add(meshRenderer);

            go.transform.SetParent(transform);
            go.transform.localPosition = rendererTransform.localPosition;
            go.transform.localEulerAngles = rendererTransform.localEulerAngles;
            go.transform.localScale = rendererTransform.localScale;

            
            //foreach (Material material in renderer.materials)
            //{
            //    material.SetFloat("_Mode", 2);
            //    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            //    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            //    material.SetInt("_ZWrite", 1);
            //    material.DisableKeyword("_ALPHATEST_ON");
            //    material.EnableKeyword("_ALPHABLEND_ON");
            //    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            //    material.renderQueue = 3000;

            //    materials.Add(material);
            //}
        }


        lerp = 0.0f;
        currentAlpha = 1.0f;
        targetAlpha = 1.0f;

    }

    void Update()
    {
        
            bool containsCharacter = false;


            foreach (Character character in GameController.instance.party.Where(c => c.controller))
            {
                foreach (BoxCollider collider in colliders)
                {
                    if (NeverdawnUtility.PointInOABB(character.position, collider))
                    {
                        containsCharacter = true;
                    }
                }
            }

            if (prevContainsCharacter != containsCharacter)
            {
                solidRenderer.ForEach(r => r.enabled = !containsCharacter);

                //targetAlpha = containsCharacter ? minAlpha : 1.0f;
                //currentAlpha = materials[0].color.a;

                //foreach (Material material in materials)
                //{
                //    if (containsCharacter)
                //    {
                //        material.renderQueue = 3001;
                //    }
                //    else
                //    {

                //        material.renderQueue = 3000;
                //    }

                //}
                

                lerp = 0.0f;
            }

            //if (lerp <= 1.0f)
            //{
            //    lerp += Time.deltaTime * fadeSpeed;

            //    foreach (Material material in materials)
            //    {
            //        Color color = material.color;
            //        color.a = Mathf.Lerp(currentAlpha, targetAlpha, lerp);

            //        if(color.a >= 0.99f)
            //        {
            //            shadowRenderer.ForEach(r => r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On);
            //            solidRenderer.ForEach(r => r.enabled = false);
            //        }
            //        else
            //        {
            //            shadowRenderer.ForEach(r => r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly);
            //            solidRenderer.ForEach(r => r.enabled = true);
            //        }

            //        //material.SetInt("_ZWrite", color.a < 0.5f ? 0 : 1);
                 
            //        material.color = color;
            //    }

            //}

            prevContainsCharacter = containsCharacter;
        
    }


}
