using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// Editor��ł��C�ӂ̃��\�b�h�����s�ł���
/// </summary>
[ExecuteAlways]
public class EditorUpdate : MonoBehaviour
{
    static double waitTime = 0;
    bool updateOnSelect = true;     // �Ώۂ̃Q�[���I�u�W�F�N�g��I�𒆂̂�Update����
    Action editorUpdateAction;


    /// <summary>
    /// OnEnable�ɋL�q
    /// </summary>
    /// <param name="editorUpdateAction"></param>
    public void EnableAction(Action editorUpdateAction)
    {
        this.editorUpdateAction += editorUpdateAction;
        EditorApplication.update += ExeEditorUpdate;
    }

    /// <summary>
    /// OnDisable�ɋL�q
    /// </summary>
    public void DisableAction()
    {
        this.editorUpdateAction -= editorUpdateAction;
        EditorApplication.update -= ExeEditorUpdate;
    }
    private void ExeEditorUpdate()
    {
        //Debug.Log("Update");
        if (EditorApplication.isPlaying == true) { return; }    // Play���͏d�����鋰�ꂪ���邽��Update���Ȃ�

        if (updateOnSelect == true)
        {
            foreach (var go in Selection.gameObjects)
            {
                // �I�𒆂̃I�u�W�F�N�g�̂ݍX�V
                if (go == this.gameObject)
                {
                    //�P�^�U�O�b�ɂP��X�V
                    if ((EditorApplication.timeSinceStartup - waitTime) >= 0.01666f)
                    {
                        EditorUpdateAction();
                        Debug.Log("Update");
                        SceneView.RepaintAll(); // �V�[���r���[�X�V
                        waitTime = EditorApplication.timeSinceStartup;
                        break;
                    }
                }
            }
        }
        else
        {
            //�P�^�U�O�b�ɂP��X�V
            if ((EditorApplication.timeSinceStartup - waitTime) >= 0.01666f)
            {
                EditorUpdateAction();
                Debug.Log("Update");
                SceneView.RepaintAll(); // �V�[���r���[�X�V
                waitTime = EditorApplication.timeSinceStartup;
            }

        }
    }

    public virtual void EditorUpdateAction()
    {
        editorUpdateAction?.Invoke();
    }
}
#endif