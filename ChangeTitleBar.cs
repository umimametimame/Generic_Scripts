using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//
// �^�C�g���o�[�̕������ύX����(Windows�̂�)
// https://fall-and-fall.hatenablog.com/entry/unity/script/change-window-title-for-standalone-pc
//
public class ChangeTitleBar : MonoBehaviour
{
    public string titleText;

    // �����t���R���p�C���iWindows�X�^���h�A�����̂ݗL���j
#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
    static extern System.IntPtr FindWindow(string className, string windowName);
    [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Unicode)]
    static extern bool SetWindowText(System.IntPtr hwnd, string txt);
    void Start()
    {
        System.IntPtr hwnd = FindWindow(null, Application.productName);
        SetWindowText(hwnd, titleText);
    }
#endif
}
