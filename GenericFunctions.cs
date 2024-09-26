using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Assertions;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AddClass
{


    public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                DontDestroyOnLoad(gameObject); // 追加
            }
            else
                Destroy(gameObject);
        }
    }
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    if (instance == null)
                    {
                        Debug.LogError(t + " をアタッチしているGameObjectはありません");
                    }
                }

                return instance;
            }
        }

        virtual protected void Awake()
        {
            // 他のゲームオブジェクトにアタッチされているか調べる
            // アタッチされている場合は破棄する。
            CheckInstance();
        }

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = this as T;
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }
            Destroy(this);
            return false;
        }
    }

    public static class AddFunction
    {
        public static void AddTag(string tagname)
        {
            UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    if (tags.GetArrayElementAtIndex(i).stringValue == tagname)
                    {
                        return;
                    }
                }

                int index = tags.arraySize;
                tags.InsertArrayElementAtIndex(index);
                tags.GetArrayElementAtIndex(index).stringValue = tagname;
                so.ApplyModifiedProperties();
                so.Update();
            }
        }
        public static void ChangeDoubleNumber(ref float v1, ref float v2)
        {
            float tmp = v1;
            v1 = v2;
            v2 = tmp;
        }
        public static List<float> SortInDescending(List<float> list)
        {
            for(int i = 0; i < list.Count; ++i)
            {
                for(int j = i + 1; j < list.Count; ++j)
                {
                    if (list[i] < list[j])
                    {
                        (list[i], list[j]) = (list[j], list[i]);    // Turpleで値を入れ替える
                    }
                }
            }

            return list;
        }




        /// <summary>
        /// angleを指定した角度に丸める
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetNormalizedAngle(float angle, float min, float max)
        {
            return Mathf.Repeat(angle - min, max - min) + min;
        }

        public static Vector3 GetNormalizedAngles(Vector3 angles, float min, float max)
        {
            Vector3 retVec3;
            retVec3.x = GetNormalizedAngle(angles.x, min, max);
            retVec3.y = GetNormalizedAngle(angles.y, min, max);
            retVec3.z = GetNormalizedAngle(angles.z, min, max);

            return retVec3;
        }

        /// <summary>
        /// Vector2を角度(360度)に変更
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Vec2ToAngle(Vector2 v)
        {
            return Mathf.Repeat(Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg, 360);
        }

        public static Vector2 DegToVec(float deg)
        {
            Vector2 vec;
            vec.x = MathF.Cos(deg);
            vec.y = MathF.Sin(deg);
            return vec;
        }

        public static float GetAngleByVec3(Vector3 start, Vector3 target)
        {
            float angle;
            Vector3 dt = start - target;
            angle = Mathf.Atan2(dt.y, dt.x) * Mathf.Rad2Deg;

            return angle;
        }
        public static float GetAngleByVec2(Vector2 start, Vector2 target)
        {
            float angle;
            Vector3 dt = start - target;
            angle = Mathf.Atan2(dt.y, dt.x) * Mathf.Rad2Deg;

            return angle;
        }

        public static float GetSumVector(Vector3 vec3)
        {
            return (Mathf.Abs(vec3.x) + Mathf.Abs(vec3.y) + Mathf.Abs(vec3.z));
        }

        /// <summary>
        /// Vector3で各角度を返す
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 Vector3AngleSet(Vector3 v1, Vector3 v2)
        {
            Vector3 angle;

            Vector2 start;
            Vector2 end;

            // xを求める
            start.x = v1.y;
            start.y = v2.z;

            end.x = v1.y;
            end.y = v2.z;

            angle.x = Vector3.Angle(start, end);

            // yを求める
            start.x = v1.x;
            start.y = v2.z;

            end.x = v1.x;
            end.y = v2.z;

            angle.y = Vector3.Angle(start, end);

            // zを求める
            start = v1;
            end = v2;

            angle.z = Vector3.Angle(start, end);

            return angle;
        }
        public static Vector3 Vector3AngleSet(Transform t1, Transform t2)
        {
            Vector3 angle;


            Vector3 v1;
            Vector3 v2;

            // xを求める
            v1 = t1.up - t2.up;
            v2 = t1.forward - t2.forward;

            angle.x = Vector3.Angle(v1, v2);

            // yを求める
            v1 = t1.right - t2.right;
            v2 = t1.forward - t2.forward;

            angle.y = Vector3.Angle(v1, v2);

            // zを求める
            v1 = t1.up - t2.up;
            v2 = t1.right - t2.right;

            angle.z = Vector3.Angle(v1, v2);

            return angle;
        }


        public static Vector3 VecTFloatConvert(VecT<float> vec)
        {
            Vector3 newVec;
            newVec.x = vec.x;
            newVec.y = vec.y;
            newVec.z = vec.z;

            return newVec;
        }
        public static VecT<float> VecTFloatConvert(VecT<float> vec, Vector3 vec3)
        {
            vec.x = vec3.x;
            vec.y = vec3.y;
            vec.z = vec3.z;

            return vec;
        }

        public static Vector3 IndexToDirrection(int index, Transform tra)
        {
            switch (index % 3)
            {
                case 0: return tra.right;
                case 1: return tra.up;
                case 2: return tra.forward;
            }

            Debug.Log("Indexが違います");
            return Vector3.zero;
        }

        public static float IndexToVec3(int index, Vector3 vec3)
        {
            switch(index % 3)
            {
                case 0: return vec3.x;
                case 1: return vec3.y;
                case 2: return vec3.z;
            }

            Debug.Log("Indexが違います");
            return 0.0f;
        }


        public static T IndexToVec3<T>(int index, VecT<T> vec3)
        {
            switch (index)
            {
                case 0: return vec3.x;
                case 1: return vec3.y;
                case 2: return vec3.z;
            }

            Debug.Log("Indexが違います");
            return default;
        }

        public static Vector3 CameraToMouse()
        {
            return new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0.0f);
        }

        /// <summary>
        /// 横軸x、奥行yのVector2を返す
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 GetFPSMoveVec2(Camera cam, Vector2 input)
        {
            Vector3 cameraForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
            Vector3 movePos = cameraForward * input.y + cam.transform.right * input.x;

            return new Vector2(movePos.x, movePos.z);
        }
        public static bool IsEven(int value)
        {
            if (value / 2 == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// タグ名を要素（iなど）にして返す
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static int TagToArray(string tag)
        {
            switch (tag)
            {
                case "Player01":
                    return 0;
                case "Player02":
                    return 1;
            }
            return -1;
        }

        /// <summary>
        /// 要素（iなど）をタグ名にして返す
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ArrayToTag(int array)
        {
            switch (array)
            {
                case 0:
                    return "Player01";
                case 1:
                    return "Player02";
            }
            return "0";
        }

        /// <summary>
        /// Rectの隣
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Neighbor(Rect rect)
        {
            return rect.x + rect.width;
        }
        public static float HorizontalityNeighbor(RectNeo rect)
        {
            return rect.x + rect.width;
        }

        public static float VerticalityNeighbor(RectNeo rect)
        {
            return rect.y + rect.height;
        }

        /// <summary>
        /// Animationの長さを返す
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <returns></returns>
        public static float GetAnimationClipLength(Animator animator, string clipName)
        {
            return Get(animator.runtimeAnimatorController.animationClips, clipName);

            float Get(IEnumerable<AnimationClip> animationClips, string clipName)
            {
                return (from animationClip in animationClips
                        where animationClip.name == clipName
                        select animationClip.length).FirstOrDefault();
            }
        }
        public static void TimeStop()
        {
            if (Time.timeScale == 1.0f || Time.timeScale == 0.0f)
            {
                Time.timeScale = 0;

            }
            else
            {
                Debug.Log("TimeScaleが変更されています。");
            }
        }

        /// <summary>
        /// 現在のAnimationを最初から再生する
        /// </summary>
        /// <param name="animator"></param>
        public static void ResetAnimation(Animator animator)
        {
            AnimatorStateInfo stateName = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(stateName.shortNameHash, 0, 0);
        }

        public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
        {
            source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

            switch (allign)
            {
                case (AnchorPresets.TopLeft):
                    {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.TopCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 1);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.TopRight):
                    {
                        source.anchorMin = new Vector2(1, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.MiddleLeft):
                    {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(0, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0.5f);
                        source.anchorMax = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleRight):
                    {
                        source.anchorMin = new Vector2(1, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }

                case (AnchorPresets.BottomLeft):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 0);
                        break;
                    }
                case (AnchorPresets.BottonCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 0);
                        break;
                    }
                case (AnchorPresets.BottomRight):
                    {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.HorStretchTop):
                    {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
                case (AnchorPresets.HorStretchMiddle):
                    {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }
                case (AnchorPresets.HorStretchBottom):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.VertStretchLeft):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchRight):
                    {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.StretchAll):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
            }
        }

        public static void SetPivot(this RectTransform source, PivotPresets preset)
        {

            switch (preset)
            {
                case (PivotPresets.TopLeft):
                    {
                        source.pivot = new Vector2(0, 1);
                        break;
                    }
                case (PivotPresets.TopCenter):
                    {
                        source.pivot = new Vector2(0.5f, 1);
                        break;
                    }
                case (PivotPresets.TopRight):
                    {
                        source.pivot = new Vector2(1, 1);
                        break;
                    }

                case (PivotPresets.MiddleLeft):
                    {
                        source.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleCenter):
                    {
                        source.pivot = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleRight):
                    {
                        source.pivot = new Vector2(1, 0.5f);
                        break;
                    }

                case (PivotPresets.BottomLeft):
                    {
                        source.pivot = new Vector2(0, 0);
                        break;
                    }
                case (PivotPresets.BottomCenter):
                    {
                        source.pivot = new Vector2(0.5f, 0);
                        break;
                    }
                case (PivotPresets.BottomRight):
                    {
                        source.pivot = new Vector2(1, 0);
                        break;
                    }
            }
        }
        /// <summary>
        /// CanvasのRender Mode が Scene Space - Overlay の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceOverlay(this Vector3 position)
        {
            return position.ToScreenPositionCaseScreenSpaceOverlay(Camera.main);
        }

        /// <summary>
        /// CanvasのRender Mode が Scene Space - Overlay の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        /// <param name="camera">メインカメラ</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceOverlay(this Vector3 position, Camera camera)
        {
            return RectTransformUtility.WorldToScreenPoint(camera, position);
        }

        /// <summary>
        /// CanvasのRender Mode が Scene Space - Camera の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        /// <param name="canvas">UIのCanvas</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas)
        {
            return position.ToScreenPositionCaseScreenSpaceCamera(canvas, Camera.main);
        }

        /// <summary>
        /// CanvasのRender Mode が Scene Space - Camera の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        /// <param name="canvas">UIのCanvas</param>
        /// <param name="uiCamera">UIを写すカメラ（CanvasのRenderCameraに設定されているカメラ）</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas, Camera uiCamera)
        {
            return position.ToScreenPositionCaseScreenSpaceCamera(canvas, uiCamera, Camera.main);
        }

        /// <summary>
        /// CanvasのRender Mode が Scene Space - Camera の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        /// <param name="canvas">UIのCanvas</param>
        /// <param name="uiCamera">UIを写すカメラ（CanvasのRenderCameraに設定されているカメラ）</param>
        /// <param name="worldCamera">メインカメラ</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas, Camera uiCamera, Camera worldCamera)
        {
            Assert.IsTrue(
                canvas.renderMode == RenderMode.ScreenSpaceCamera,
                "Canvasのレンダーモードが「Scene Space - Camera」になっていません"
            );

            var p = RectTransformUtility.WorldToScreenPoint(worldCamera, position);
            var retPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                p,
                uiCamera,
                out retPosition
            );
            return retPosition;
        }
        public static List<string> SearchTypes<T>()
        {
            List<string> typeNames = new List<string>();

            // このスクリプトのアセンブリを取得
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // このスクリプトのアセンブリから型を取得
            Type[] types = assembly.GetTypes();

            // ジェネリック型と一致する型を探す
            foreach (Type type in types)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(T))
                {
                    // ジェネリック型の名前をリストに追加
                    typeNames.Add(type.FullName);
                }
            }

            return typeNames;
        }
    }

    /// <summary>
    /// 指定した型を検索し、Listにして返す関数
    /// </summary>
    public class TypeFinder : MonoBehaviour
    {
        [field: SerializeField] public FieldInfo[] fields { get; private set; }

        /// <summary>
        /// 引数に欲しい型を入れた後、GetType()を実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<T> GetAndInList<T>(Type type)
        {
            fields = type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            List<T> variables = new List<T>();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(T))
                {
                    T variable = (T)field.GetValue(GetComponent(type));
                    variables.Add(variable);

                }
            }

            return variables;
        }
    }
    public static class ConvertEnums<T1, T2> where T1 : Enum where T2 : class
    {
        public static Dictionary<T1, T2> GetDic()
        {
            Dictionary<T1, T2> newDic = new Dictionary<T1, T2>();
            T2 newValue = default;
            foreach (T1 s in Enum.GetValues(typeof(T1)))
            {
                newDic.Add(s, newValue);
            }

            return newDic;
        }
        public static List<T1> GetEnumList()
        {
            List<T1> list = new List<T1>();
            foreach (T1 s in Enum.GetValues(typeof(T1)))
            {
                list.Add(s);
            }

            return list;
        }
    }







#if UNITY_EDITOR
    /// <summary>
    /// serializedObjectUpdateに関数を追加する
    /// </summary>
    /// <typeparam name="CustomEditorType"></typeparam>
    public class MyEditor<CustomEditorType> : Editor where CustomEditorType : UnityEngine.Object
    {
        protected Action serializedObjectUpdate;
        protected CustomEditorType tg;
        protected void Initialize()
        {
            tg = (CustomEditorType)target;
        }


        public override void OnInspectorGUI()
        {

            SerializedObjectUpdate();
            EndUpdate();

        }

        protected void EndUpdate()
        {

            if (GUI.changed)
            {
                EditorUtility.SetDirty(tg);
            }

        }

        void SerializedObjectUpdate()
        {
            serializedObject.Update();
            {
                serializedObjectUpdate?.Invoke();
            }
            serializedObject.ApplyModifiedProperties();
        }

    }


    /// <summary>
    /// 表示する変数名のstringを用意する<br/>
    /// Updateを上書きする
    /// </summary>
    public class MyPropertyDrawer : PropertyDrawer
    {
        public Rect pos;
        public SerializedProperty prop;
        public float boolWidth = 15;
        public float uniformedLabelWidth;
        public float uniformedFieldWidth;
        public float labelWidthAve;
        public float distance = 4.1f;       // PropertyDrawerの定数
        public Vector2 elements;
        public List<List<LabelAndproperty>> verticalProps;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                pos = position; 
                pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

                prop = property;

                // 子のフィールドをインデントしない 
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                Update(position, property, label);


                // インデントを元通りに戻します
                EditorGUI.indentLevel = indent;
            }
            EditorGUI.EndProperty();


        }

        /// <summary>
        /// verticalPropsを全て描画する
        /// </summary>

        public void DrawPropsList()
        {
            for (int i = 0; i < verticalProps.Count; ++i)
            {
                SetElements(i);
                Uniform(verticalProps[i]);
                for(int j = 0; j < verticalProps[i].Count; ++j)
                {
                    //if(verticalProps[i][j].useRatioInRemainder == 0.0f)
                    //{
                    //    verticalProps[i][j].propertyRect.Width = UseHorizontalRatio(1.0f);
                    //}
                    //else
                    //{
                    //    verticalProps[i][j].propertyRect.Width = UseHorizontalRatio(verticalProps[i][j].useRatioInRemainder);

                    //}
                    if(i != 0)
                    {
                        verticalProps[i][j].verticalNeighbor = verticalProps[i - 1];
                        verticalProps[i][j].NewLinePosSet();
                    }
                }
                DrawPropsOnHorizontal(verticalProps[i]);
                
            }
        }

        public Vector2 SetElements(int verticalIndex = 0)
        {
            Vector2 newEle;
            newEle.x = verticalProps[verticalIndex].Count;
            newEle.y = verticalProps.Count;
            elements = newEle;

            return elements;
        }

        public void SetPropsList(List<List<LabelAndproperty>> props)
        {
            for(int i = 0; i < props.Count; ++i)
            {
                for(int j = 0; j < props[i].Count; ++j)
                {
                    props[i][j].Set(pos, prop);
                }
            }
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 各変数の高さを計算
            return EditorGUIUtility.singleLineHeight * elements.y + 1.82f;
        }
        protected virtual void Update(Rect pos, SerializedProperty prop, GUIContent label)
        { }


        /// <summary>
        /// Updateの最初に行う<br/>
        /// 引数はLabelAndPropertyList<br/>
        /// Labelの大きさ
        /// </summary>
        /// <param name="horizontalElements"></param>
        public void Uniform(List<LabelAndproperty> horizontalProps, float aveLabelWidth)
        {
            uniformedLabelWidth = aveLabelWidth;
            foreach (var prop in horizontalProps)
            {
                prop.labelRect.Width = aveLabelWidth;
            }
            uniformedFieldWidth = pos.width / horizontalProps.Count - uniformedLabelWidth - distance * 2;
        }


        public void Uniform(List<LabelAndproperty> horizontalProps)
        {
            uniformedLabelWidth = 0.0f;
            foreach (var prop in horizontalProps)
            {
                uniformedLabelWidth += prop.labelRect.width;
            }
            labelWidthAve = uniformedLabelWidth / horizontalProps.Count;
            uniformedLabelWidth = labelWidthAve;
            uniformedFieldWidth = pos.width / horizontalProps.Count - uniformedLabelWidth - distance * 2;

        }

        /// <summary>
        /// 横幅の使用する割合を指定する
        /// </summary>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public float UseHorizontalRatio(float ratio)
        {
            return (pos.width / elements.x - labelWidthAve - distance * 2) * (ratio * elements.x);
        }

        public void DrawPropsOnHorizontal(List<LabelAndproperty> lavProps)
        {

            for (int i = 0; i < lavProps.Count; i++)
            {
                float ratio;
                if (lavProps[i].useRatioInRemainder == 0.0f)
                {
                    ratio = UseHorizontalRatio(1.0f);
                }
                else
                {
                    ratio = UseHorizontalRatio(lavProps[i].useRatioInRemainder);
                }


                if (i == 0)
                {
                    lavProps[i].InitialPosSet(pos.x, lavProps[i].labelRect.width, ratio);

                }
                else
                {
                    lavProps[i].horizontalNeighbor = lavProps[i - 1];
                    lavProps[i].PosSetByHorizontalNeighbor();
                    lavProps[i].propertyRect.Width = ratio;
                }
                lavProps[i].Draw();
            }
        }
        public void UniformedDraw(List<LabelAndproperty> lavProps, float avePropWidth)
        {

            for (int i = 0; i < lavProps.Count; i++)
            {
                if (i == 0)
                {
                    lavProps[i].InitialPosSet(pos.x, lavProps[i].labelRect.width, avePropWidth);

                }
                else
                {
                    lavProps[i].horizontalNeighbor = lavProps[i - 1];
                    lavProps[i].PosSetByHorizontalNeighbor();
                    lavProps[i].propertyRect.Width = avePropWidth;
                }
                lavProps[i].Draw();
            }
        }

        public class LabelAndproperty
        {
            public RectNeo labelRect;
            public string label;
            public RectNeo propertyRect;
            public SerializedProperty property;
            public string propName;
            public float useRatioInRemainder;
            public LabelAndproperty horizontalNeighbor;
            public List<LabelAndproperty> verticalNeighbor;
            public EditType edit;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="rect"></param>
            /// <param name="property"></param>
            
            public LabelAndproperty(string propertyName)
            {
                this.propName = propertyName;
            }
            public void Set(Rect rect, SerializedProperty property)
            {
                labelRect = new RectNeo(rect);
                propertyRect = new RectNeo(rect);
                labelRect.Height = EditorGUIUtility.singleLineHeight;
                propertyRect.Height = EditorGUIUtility.singleLineHeight;
                this.property = property.FindPropertyRelative(propName);
                useRatioInRemainder = 0.0f;

                char[] array = this.property.displayName.ToCharArray();
                array[0] = char.ToUpper(array[0]);  // 戦闘を大文字にする

                label = new string(array);
            }

            public void InitialPosSet(float x, float labelWidth, float fieldRatio = 0.0f)
            {
                if(fieldRatio != 0.0f)
                {
                    labelRect.SetHorizontal(x, labelWidth);
                    propertyRect.SetHorizontal(AddFunction.HorizontalityNeighbor(labelRect) + 5, fieldRatio);

                }
                else
                {
                    labelRect.SetHorizontal(x, labelWidth);
                    propertyRect.SetHorizontal(AddFunction.HorizontalityNeighbor(labelRect) + 5, useRatioInRemainder);
                }
            }

            public void HorizontalityNeighborPosSet(float labelWidth, float fieldWidth = 0.0f)
            {
                if(fieldWidth != 0.0f)
                {
                    labelRect.SetHorizontal(AddFunction.HorizontalityNeighbor(horizontalNeighbor.propertyRect) + 5, labelWidth);
                    propertyRect.SetHorizontal(AddFunction.HorizontalityNeighbor(labelRect) + 5, fieldWidth);
                }
                else
                {
                    labelRect.SetHorizontal(AddFunction.HorizontalityNeighbor(horizontalNeighbor.propertyRect) + 5, labelWidth);
                    propertyRect.SetHorizontal(AddFunction.HorizontalityNeighbor(labelRect) + 5, useRatioInRemainder);

                }
            }

            public void NewLinePosSet()
            {
                float distanceBeforeLine = 1.82f;
                labelRect.Y = AddFunction.VerticalityNeighbor(verticalNeighbor[0].propertyRect) + distanceBeforeLine;
                propertyRect.Y = labelRect.y;

            }
            public void Draw()
            {
                switch (edit)
                {

                    case EditType.None:
                        DrawPropertyField();
                        break;

                    case EditType.NonEditable:
                        EditorGUI.LabelField(labelRect.entity, label);
                        
                        EditorGUI.BeginDisabledGroup(true);
                        {
                            EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
                        }
                        EditorGUI.EndDisabledGroup();
                        break;

                    case EditType.NonEditableInGame:
                        EditorGUI.LabelField(labelRect.entity, label);

                        if (EditorApplication.isPlaying)
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        else
                        {
                            EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);

                        }

                        break;
                }
            }

            

            /// <summary>
            /// verticalList内をすべて描画する
            /// </summary>
            public void DrawProps()
            {
                switch (edit)
                {

                    case EditType.None:
                        DrawPropertyField();
                        break;

                    case EditType.NonEditable:
                        EditorGUI.LabelField(labelRect.entity, label);

                        EditorGUI.BeginDisabledGroup(true);
                        {
                            EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
                        }
                        EditorGUI.EndDisabledGroup();
                        break;

                    case EditType.NonEditableInGame:
                        EditorGUI.LabelField(labelRect.entity, label);

                        if (EditorApplication.isPlaying)
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        else
                        {
                            EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);

                        }

                        break;
                }
            }
            public void DrawPropertyField()
            {
                EditorGUI.LabelField(labelRect.entity, label);
                EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
            }

            public void PosSetByHorizontalNeighbor()
            {
                labelRect.X = AddFunction.HorizontalityNeighbor(horizontalNeighbor.propertyRect) + 5;
                propertyRect.X = AddFunction.HorizontalityNeighbor(labelRect) + 5;
            }
        }
        public enum EditType
        {
            None,
            NonEditable,
            NonEditableInGame,
        }
    }

#endif

    #region インスペクタープロパティ
    /// <summary>
    /// 数値の中身と最大値を含む<br/>
    /// インスタンス化不要
    /// </summary>
    [Serializable]
    public class Parameter
    {
        public float entity;
        public float max;
        public float autoRecoverValue;
        public void Initialize()
        {
            entity = max;
        }

        public void Update()
        {
            entity += autoRecoverValue;
            ReturnRange();
        }

        public void Update(float changeEntity)
        {
            entity += changeEntity;
            ReturnRange();
        }

        public void ReturnRange()
        {

            if (entity > max) { entity = max; }
            else if (entity < 0.0f) { entity = 0.0f; }
        }

        public bool inRange
        {
            get
            {
                if (entity <= max)
                {
                    if (entity >= 0.0f)
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        public bool overZero    // entityが0以下なら
        {
            get
            {
                if (entity <= 0.0f) { return true; }
                return false;
            }
        }

        /// <summary>
        /// 使用可能なら
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool CostJudge(float cost)
        {
            if (entity - cost > 0.0f)
            {
                return true;
            }

            return false;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Parameter))]
    public class ParameterDrawer : MyPropertyDrawer
    {

        LabelAndproperty entity = new LabelAndproperty(nameof(entity));
        LabelAndproperty max = new LabelAndproperty(nameof(max));
        LabelAndproperty autoRecoverValue = new LabelAndproperty(nameof(autoRecoverValue));
        protected override void Update(Rect _pos, SerializedProperty property, GUIContent label)
        {
            autoRecoverValue.label = "Auto";

            List<LabelAndproperty> list = new List<LabelAndproperty>() { entity, max, autoRecoverValue };
            verticalProps = new List<List<LabelAndproperty>>() { list };
            SetElements();
            SetPropsList(verticalProps);

            entity.useRatioInRemainder = 0.4f;
            max.useRatioInRemainder = 0.3f;
            autoRecoverValue.useRatioInRemainder = 0.3f;

            entity.edit = EditType.NonEditable;

            entity.labelRect.Width = 40;
            max.labelRect.Width = 25;
            autoRecoverValue.labelRect.Width = 30;

            DrawPropsList();
        }
    }

    [CustomPropertyDrawer(typeof(Vec3Bool))]
    public class Vec3BoolDrawer : MyPropertyDrawer 
    {
        LabelAndproperty x = new LabelAndproperty("x");
        LabelAndproperty y = new LabelAndproperty("y");
        LabelAndproperty z = new LabelAndproperty("z");
        protected override void Update(Rect _pos, SerializedProperty _prop, GUIContent _label)
        {
            
            List<LabelAndproperty> list = new List<LabelAndproperty>() { x, y, z };
            verticalProps = new List<List<LabelAndproperty>>() { list };
            SetElements();
            SetPropsList(verticalProps);

            foreach (LabelAndproperty l in list)
            {
                l.edit = EditType.NonEditableInGame;
            }


            Uniform(list, 10);
            UniformedDraw(list, boolWidth);
        }
    }

    [CustomPropertyDrawer(typeof(MinMax))]
    public class RangeDrawer : MyPropertyDrawer
    {
        LabelAndproperty min = new LabelAndproperty(nameof(min));
        LabelAndproperty max = new LabelAndproperty(nameof(max));

        protected override void Update(Rect pos, SerializedProperty prop, GUIContent label)
        {
            List<LabelAndproperty> list = new List<LabelAndproperty>() { min, max };
            verticalProps = new List<List<LabelAndproperty>>() { list };
            SetElements();
            SetPropsList(verticalProps);

            foreach (LabelAndproperty l in list)
            {
                l.edit = EditType.None;
            }

            min.labelRect.Width = max.labelRect.Width = 25.0f;

            min.useRatioInRemainder = max.useRatioInRemainder = 0.5f;

            DrawPropsList();
        }
    }

    [CustomPropertyDrawer(typeof(Traffic))]
    public class TrafficDrawer : MyPropertyDrawer
    {
        LabelAndproperty active = new LabelAndproperty(nameof(active));
        protected override void Update(Rect pos, SerializedProperty prop, GUIContent label)
        {
            List<LabelAndproperty> list = new List<LabelAndproperty> { active };
            verticalProps = new List<List<LabelAndproperty>> { list };
            SetElements();
            SetPropsList(verticalProps);

            active.edit = EditType.NonEditable;
            active.labelRect.Width = 40;


            DrawPropsList();
        }
    }
#endif

    #endregion
}

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll
}

public enum PivotPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottomCenter,
    BottomRight,
}
public enum IncreseType
{
    DeltaTime,
    Frame,
    Manual,
}
