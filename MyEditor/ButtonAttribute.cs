using System.Linq;
using System;
using System.Reflection;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public sealed class ButtonAttribute : PropertyAttribute
{
    public string Function { get; private set; }   // �֐���
    public string Name { get; private set; }   // �{�^���ɕ\������e�L�X�g
    public object[] Parameters { get; private set; }   // �֐��ɓn���������Ǘ�����z��

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="function">�֐���</param>
    /// <param name="name">�{�^���ɕ\������e�L�X�g</param>
    /// <param name="parameters">�֐��ɓn���������Ǘ�����z��</param>
    public ButtonAttribute(string function, string name, params object[] parameters)
    {
        Function = function;
        Name = name;
        Parameters = parameters;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ButtonAttribute))]
public sealed class ButtonDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var buttonAttribute = attribute as ButtonAttribute;

        if (GUI.Button(position, buttonAttribute.Name))
        {
            var objectReferenceValue = property.serializedObject.targetObject;
            var type = objectReferenceValue.GetType();
            var bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var method = type.GetMethod(buttonAttribute.Function, bindingAttr);

            try
            {
                method.Invoke(objectReferenceValue, buttonAttribute.Parameters);
            }
            catch (AmbiguousMatchException)
            {
                var format = @"{0}.{1} �֐����I�[�o�[���[�h����Ă��邽�ߊ֐������ł��܂���B{0}.{1} �֐��̃I�[�o�[���[�h���폜���Ă�������";
                var message = string.Format(format, type.Name, buttonAttribute.Function);

                Debug.LogError(message, objectReferenceValue);
            }
            catch (ArgumentException)
            {
                var parameters = string.Join(", ", buttonAttribute.Parameters.Select(c => c.ToString()).ToArray());
                var format = @"{0}.{1} �֐��Ɉ��� {2} ��n�����Ƃ��ł��܂���B{0}.{1} �֐��̈����̌^�����������ǂ������m�F���Ă�������";
                var message = string.Format(format, type.Name, buttonAttribute.Function, parameters);

                Debug.LogError(message, objectReferenceValue);
            }
            catch (NullReferenceException)
            {
                var format = @"{0}.{1} �֐��͒�`����Ă��܂���B{0}.{1} �֐�����`����Ă��邩�ǂ������m�F���Ă�������";
                var message = string.Format(format, type.Name, buttonAttribute.Function);

                Debug.LogError(message, objectReferenceValue);
            }
            catch (TargetParameterCountException)
            {
                var parameters = string.Join(", ", buttonAttribute.Parameters.Select(c => c.ToString()).ToArray());
                var format = @"{0}.{1} �֐��Ɉ��� {2} ��n�����Ƃ��ł��܂���B{0}.{1} �֐��̈����̐������������ǂ������m�F���Ă�������";
                var message = string.Format(format, type.Name, buttonAttribute.Function, parameters);

                Debug.LogError(message, objectReferenceValue);
            }
        }
    }
}
#endif