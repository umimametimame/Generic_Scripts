using AddUnityClass;
using Fusion;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdulationTarget : MonoBehaviour
{
    private EditorUpdate editorUpdate;
    private void OnEnable()
    {
        editorUpdate = gameObject.AddComponent<EditorUpdate>();
        
        editorUpdate.EnableAction(PosAdulation);
    }
    private void OnDisable()
    {
        editorUpdate.DisableAction();
    }
    enum AdulationType
    {
        World,
        Screen,
    }
    [SerializeField] private AdulationType adulationType;
    [field: SerializeField] public GameObject target { get; set; }
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Vec3Bool rotation;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Vector3 adjustPos;
    [SerializeField, NonEditable] private Vector3 adulation;
    [SerializeField] private float adulationRatio;
    [SerializeField] private Vector3 wToS;
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private Vector2 screenPos;
    private Rigidbody rigidBody;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (target == null) { return; }

        thisRect = GetComponent<RectTransform>();
        rigidBody = GetComponent<Rigidbody>();

        switch (adulationType)
        {
            case AdulationType.World:

                gameObject.transform.position = target.transform.position + adjustPos;                
                break;

            case AdulationType.Screen:
                wToS = targetCamera.WorldToScreenPoint(target.transform.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, wToS, targetCamera, out Vector2 outPos);
                thisRect.anchoredPosition = outPos;
                break;
        }
        
    }

    public void FixedUpdate()
    {
        if(adulationRatio <= 0)
        {
            Debug.Log("’Ç]Š„‡‚ª0ˆÈ‰º‚Å‚·!");
        }
        PosAdulation();
    }


    void PosAdulation()
    {
        if (target == null) { return; }


        switch (adulationType)
        {
            case AdulationType.World:
                {
                    if (rigidBody != null)
                    {
                        adulation = rigidBody.position + (target.transform.position + adjustPos - gameObject.transform.position) * adulationRatio;
                        rigidBody.position = adulation;
                    }
                    else
                    {
                        adulation = gameObject.transform.position + (target.transform.position + adjustPos - gameObject.transform.position) * adulationRatio;
                        gameObject.transform.position = adulation;
                    }


                    Quaternion newRotation = gameObject.transform.rotation;
                    if (rotation.x == true)
                    {
                        newRotation.x = target.transform.rotation.x;
                    }
                    if (rotation.y == true)
                    {
                        newRotation.y = target.transform.rotation.y;
                    }
                    if (rotation.z == true)
                    {
                        newRotation.z = target.transform.rotation.z;
                    }

                    gameObject.transform.rotation = newRotation;
                }
                break;


            case AdulationType.Screen:

                wToS = targetCamera.WorldToScreenPoint(target.transform.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, wToS, targetCamera, out Vector2 outPos);
                screenPos  =  outPos;
                adulation = thisRect.anchoredPosition + (screenPos + (new Vector2 (adjustPos.x, adjustPos.y)) - thisRect.anchoredPosition) * adulationRatio;
                gameObject.transform.localPosition = outPos;
                //thisRect.anchoredPosition = adulation;
                break;
        }

        SceneView.RepaintAll();
        
    }
}
