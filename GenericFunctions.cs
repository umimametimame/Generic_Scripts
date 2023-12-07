using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Assertions;
using System.Collections;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AddClass
{

    public class GenericFunctions : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                DontDestroyOnLoad(gameObject); // �ǉ�
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
                        Debug.LogError(t + " ���A�^�b�`���Ă���GameObject�͂���܂���");
                    }
                }

                return instance;
            }
        }

        virtual protected void Awake()
        {
            // ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă��邩���ׂ�
            // �A�^�b�`����Ă���ꍇ�͔j������B
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

        /// <summary>
        /// Vector2���p�x(360�x)�ɕύX
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

        /// <summary>
        /// Vector3�Ŋe�p�x��Ԃ�
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 Vector3AngleSet(Vector3 v1, Vector3 v2)
        {
            Vector3 angle;

            Vector2 start;
            Vector2 end;

            // x�����߂�
            start.x = v1.y;
            start.y = v2.z;

            end.x = v1.y;
            end.y = v2.z;

            angle.x = Vector3.Angle(start, end);

            // y�����߂�
            start.x = v1.x;
            start.y = v2.z;

            end.x = v1.x;
            end.y = v2.z;

            angle.y = Vector3.Angle(start, end);

            // z�����߂�
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

            // x�����߂�
            v1 = t1.up - t2.up;
            v2 = t1.forward - t2.forward;

            angle.x = Vector3.Angle(v1, v2);

            // y�����߂�
            v1 = t1.right - t2.right;
            v2 = t1.forward - t2.forward;

            angle.y = Vector3.Angle(v1, v2);

            // z�����߂�
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

            Debug.Log("Index���Ⴂ�܂�");
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

            Debug.Log("Index���Ⴂ�܂�");
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

            Debug.Log("Index���Ⴂ�܂�");
            return default;
        }
        public static Vector3 CameraToMouse()
        {
            return new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0.0f);
        }

        /// <summary>
        /// ����x�A���sy��Vector2��Ԃ�
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
        /// �^�O����v�f�ii�Ȃǁj�ɂ��ĕԂ�
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
        /// �v�f�ii�Ȃǁj���^�O���ɂ��ĕԂ�
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
        /// Rect�̗�
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Neighbor(Rect rect)
        {
            return rect.x + rect.width;
        }
        public static float Neighbor(HorizontalRect rect)
        {
            return rect.x + rect.width;
        }

        /// <summary>
        /// Animation�̒�����Ԃ�
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
        /// Canvas��Render Mode �� Scene Space - Overlay �̏ꍇ�ɁA���[���h���W���X�N���[�����W�ɕϊ�����
        /// </summary>
        /// <returns>�ϊ����ꂽ�X�N���[�����W</returns>
        /// <param name="position">�Ώۂ̃��[���h���W</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceOverlay(this Vector3 position)
        {
            return position.ToScreenPositionCaseScreenSpaceOverlay(Camera.main);
        }

        /// <summary>
        /// Canvas��Render Mode �� Scene Space - Overlay �̏ꍇ�ɁA���[���h���W���X�N���[�����W�ɕϊ�����
        /// </summary>
        /// <returns>�ϊ����ꂽ�X�N���[�����W</returns>
        /// <param name="position">�Ώۂ̃��[���h���W</param>
        /// <param name="camera">���C���J����</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceOverlay(this Vector3 position, Camera camera)
        {
            return RectTransformUtility.WorldToScreenPoint(camera, position);
        }

        /// <summary>
        /// Canvas��Render Mode �� Scene Space - Camera �̏ꍇ�ɁA���[���h���W���X�N���[�����W�ɕϊ�����
        /// </summary>
        /// <returns>�ϊ����ꂽ�X�N���[�����W</returns>
        /// <param name="position">�Ώۂ̃��[���h���W</param>
        /// <param name="canvas">UI��Canvas</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas)
        {
            return position.ToScreenPositionCaseScreenSpaceCamera(canvas, Camera.main);
        }

        /// <summary>
        /// Canvas��Render Mode �� Scene Space - Camera �̏ꍇ�ɁA���[���h���W���X�N���[�����W�ɕϊ�����
        /// </summary>
        /// <returns>�ϊ����ꂽ�X�N���[�����W</returns>
        /// <param name="position">�Ώۂ̃��[���h���W</param>
        /// <param name="canvas">UI��Canvas</param>
        /// <param name="uiCamera">UI���ʂ��J�����iCanvas��RenderCamera�ɐݒ肳��Ă���J�����j</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas, Camera uiCamera)
        {
            return position.ToScreenPositionCaseScreenSpaceCamera(canvas, uiCamera, Camera.main);
        }

        /// <summary>
        /// Canvas��Render Mode �� Scene Space - Camera �̏ꍇ�ɁA���[���h���W���X�N���[�����W�ɕϊ�����
        /// </summary>
        /// <returns>�ϊ����ꂽ�X�N���[�����W</returns>
        /// <param name="position">�Ώۂ̃��[���h���W</param>
        /// <param name="canvas">UI��Canvas</param>
        /// <param name="uiCamera">UI���ʂ��J�����iCanvas��RenderCamera�ɐݒ肳��Ă���J�����j</param>
        /// <param name="worldCamera">���C���J����</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas, Camera uiCamera, Camera worldCamera)
        {
            Assert.IsTrue(
                canvas.renderMode == RenderMode.ScreenSpaceCamera,
                "Canvas�̃����_�[���[�h���uScene Space - Camera�v�ɂȂ��Ă��܂���"
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

            // ���̃X�N���v�g�̃A�Z���u�����擾
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // ���̃X�N���v�g�̃A�Z���u������^���擾
            Type[] types = assembly.GetTypes();

            // �W�F�l���b�N�^�ƈ�v����^��T��
            foreach (Type type in types)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(T))
                {
                    // �W�F�l���b�N�^�̖��O�����X�g�ɒǉ�
                    typeNames.Add(type.FullName);
                }
            }

            return typeNames;
        }
    }

    /// <summary>
    /// �w�肵���^���������AList�ɂ��ĕԂ��֐�
    /// </summary>
    public class TypeFinder : MonoBehaviour
    {
        [field: SerializeField] public FieldInfo[] fields { get; private set; }
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

    [Serializable] public class TransformOffset
    {
        [SerializeField] private Vector3 vec3Offset;
        [SerializeField] private Vector2 vec2Offset;

        public void Update(Transform transform)
        {
            transform.position += vec3Offset; 
            transform.position += new Vector3(vec2Offset.x, vec2Offset.y, 0.0f);

        }
        public void Update(GameObject obj)
        {
            obj.transform.position += vec3Offset;
            obj.transform.position += new Vector3(vec2Offset.x, vec2Offset.y, 0.0f);
        }
    }

    public enum ExistState
    {
        Disable,
        Start,
        Enable,
        Ending,
    }

    [Serializable]
    public class Exist
    {
        [field: SerializeField, NonEditable] public ExistState state { get; private set; } = ExistState.Disable;
        [field: SerializeField] public Action initialize { get; set; }
        [field: SerializeField] public Action disable { get; set; }
        [field: SerializeField] public Action start { get; set; }
        [field: SerializeField] public Action enable { get; set; }
        [field: SerializeField] public Action toEnd { get; set; }
        [field: SerializeField] public Action ending { get; set; }

        [field: SerializeField, NonEditable] public bool oneShot { get; private set; }

        public void Initialize(bool started = false)
        {
            initialize?.Invoke();
            this.oneShot = started;
            state = ExistState.Disable;
        }

        public void Reset()
        {
            state = ExistState.Disable;
        }

        public void Update()
        {
            switch (state)
            {
                case ExistState.Disable:
                    disable?.Invoke();
                    break;

                case ExistState.Start:
                    state = ExistState.Enable;
                    start?.Invoke();
                    break;

                case ExistState.Enable:
                    enable?.Invoke();
                    break;
                case ExistState.Ending:

                    break;
            }
        }

        public void Stop()
        {
            state = ExistState.Disable;
        }

        public void Start()
        {
            state = ExistState.Start;
        }

        /// <summary>
        /// ��x�̂�
        /// </summary>
        public void StartOneShot()
        {
            if (oneShot == false)
            {
                state = ExistState.Start;
                oneShot = true;
            }
        }
        public void OneShotReset()
        {
            oneShot = false;
        }
        public void Finish()
        {
            state = ExistState.Ending;
            toEnd?.Invoke();
        }
    }


    /// <summary>
    /// �ړ��͈͂��~�`�ɐ���
    /// </summary>
    [Serializable]
    public class CircleClamp
    {

        [SerializeField] private GameObject center;
        [field: SerializeField] public GameObject moveObject { get; set; }
        [field: SerializeField] public float radius { get; private set; }
        [field: SerializeField, NonEditable] public float currentDistance { get; private set; }
        public void Initialize(GameObject center, GameObject moveObject)
        {
            this.center = center;
            this.moveObject = moveObject;
        }
        public void AdjustByCenter()
        {
            moveObject.transform.position = center.transform.position;
        }
        public void Limit()
        {
            currentDistance = Vector2.Distance(moveObject.transform.position, center.transform.position);
            
            if (currentDistance > radius)
            {
                Debug.Log("Limitting");

                Vector3 nor = moveObject.transform.position - center.transform.position;
                moveObject.transform.position = center.transform.position + nor.normalized * radius; 
                currentDistance = Vector2.Distance(moveObject.transform.position, center.transform.position);

            }

        }
    }

    [Serializable] public class MoveCircleSurface
    {
        [field: SerializeField] public Transform centerPos { get; set; }
        [field: SerializeField] public Transform moveObject { get; set; }
        [field: SerializeField] bool lookAtCenter { get; set; } // center��������
        [field: SerializeField] public Vector3 axis { get; set; }   // transform.right�Ȃǂő������

        [SerializeField, NonEditable] private Vector3 norAxis;
        [SerializeField, NonEditable] private Quaternion angleAxis;
        [SerializeField] private float speed;
        [field: SerializeField, NonEditable] public Vector3 angleFromCenter { get; private set; }
        public void Initialize(GameObject moveObject)
        {
            this.moveObject = moveObject.transform;
        }
        public void Initialize(Transform moveObject)
        {
            this.moveObject = moveObject;
        }

        /// <summary>
        /// �����͉�]�X�s�[�h
        /// </summary>
        /// <param name="speed"></param>
        public void Update(float speed )
        {
            if (speed == 0.0f) { return; }
            this.speed = speed;

            norAxis = axis.normalized;
            angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

            moveObject.position -= centerPos.position;
            moveObject.position = angleAxis * moveObject.position;
            moveObject.position += centerPos.position;

            if(lookAtCenter == true)
            {
                moveObject.rotation = moveObject.rotation * angleAxis;
            }

            angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);

        }
        public Vector3 NewPosUpdate(float speed)
        {
            if (speed == 0.0f) { return Vector3.zero; }
            this.speed = speed;

            norAxis = axis.normalized;
            angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

            Vector3 newPos;

            newPos = moveObject.position - centerPos.position;
            newPos = angleAxis * newPos;
            newPos += centerPos.position;

            if (lookAtCenter == true)
            {
                moveObject.rotation = moveObject.rotation * angleAxis;
            }

            angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);

            return newPos;
        }
        public void Update()
        {
            if (this.speed == 0.0f) { return; }

            norAxis = axis.normalized;
            angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

            moveObject.position -= centerPos.position;
            moveObject.position = angleAxis * moveObject.position;
            moveObject.position += centerPos.position;

            if (lookAtCenter == true)
            {
                moveObject.rotation = moveObject.rotation * angleAxis;
            }

            angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);
        }


    }

    #region �N���X�̎��s�^�C�v
    /// <summary>
    /// bool�Ŕ��f���Atrue�̏ꍇ�Ƀ��C���̏������s��
    /// </summary>
    [Serializable] public class Traffic
    {
        [field: SerializeField] public bool active { get; set; }
        public Action activeAction { get; set; }
        public Action nonActiveAction { get; set; }
        public void Initialize()
        {
            active = false;
            activeAction = null;
            nonActiveAction = null;
        }
        public void Update()
        {
            if(active == true)
            {
                activeAction?.Invoke();
            }
            else
            {
                nonActiveAction?.Invoke();
            }
        }
    }

    /// <summary>
    /// �Ԋu����N���X
    /// </summary>
    [Serializable]
    public class Interval
    {
        public enum IncreseType
        {
            DeltaTime,
            Frame,
            Manual,
        }
        [field: SerializeField, NonEditable] public bool active { get; private set; }
        [field: SerializeField] public float interval { get; private set; }
        [field: SerializeField, NonEditable] public VariedTime time { get; private set; }
        private bool autoReset;
        private bool reached;
        public Action reachAction { get; set; }
        public Action activeAction { get; set; }
        public Action lowAction { get; set; }

        /// <summary>
        /// ����:<br/>
        /// �E�ŏ�����active�ɂ���(interval�l��value�𓯂��ɂ���)��<br/>
        /// �Evalue��interval�l�ɓ��B������0�ɖ߂邩<br/>
        /// �E�ŏ���interval�l
        /// </summary>
        /// <param name="start"></param>
        public void Initialize(bool start, bool autoReset = true, float interval = 0.0f)
        {
            if (interval != 0.0f) { this.interval = interval; }
            this.autoReset = autoReset;
            if (start == true)
            {
                time.Initialize(interval);
            }
            else
            {
                time.Initialize();
            }

            active = (time.value >= interval) ? true : false;
            reached = false;
        }

        public void Update(float manualValue = 0.0f)
        {
            time.Update(manualValue);

            if (time.value >= interval)
            {
                if (reached == false)
                {
                    reached = true;
                    reachAction?.Invoke();
                }

                active = true;
                activeAction?.Invoke();
                if (autoReset == true) { Reset(); }
            }
            else
            {
                active = false;
                lowAction?.Invoke();
            }
        }

        /// <summary>
        /// time��0�ɖ߂�
        /// </summary>
        public void Reset()
        {
            reached = false;
            time.Initialize();
        }
    }

    /// <summary>
    /// �͈͖���Action�����s����
    /// </summary>
    [Serializable]
    public class ThresholdRatio
    {
        [field: SerializeField, NonEditable] public bool reaching { get; private set; }
        [field: SerializeField, NonEditable] public bool beforeBool { get; private set; }

        [SerializeField] private Vector2 thresholdRange;
        [SerializeField, NonEditable] private float currentValue;
        [SerializeField, NonEditable] private Vector2 beforeRange;
        public MomentAction withinRangeAction { get; set; } = new MomentAction();   // �͈͓��ɓ��鎞�Ɉ�x�s����
        public Action inRangeAction { get; set; }                                   // �͈͓��ɓ����Ă���Ԃɍs����
        public MomentAction exitRangeAction { get; set; } = new MomentAction();     // �͈͊O�ɏo�鎞�Ɉ�x�s����
        public Action outOfRangeAction { get; set; }                                // �͈͊O�ɏo�Ă���Ԃɍs����

        public void Initialize(float min, float max)
        {
            thresholdRange = new Vector2(min, max);
            Reset();
        }
        public void Initialize(Vector2 range = default)
        {
            if (range != default) { thresholdRange = range; }
            Reset();
        }

        public void Reset()
        {
            beforeBool = false;
            beforeRange = Vector2.zero;
            withinRangeAction.Initialize();
            exitRangeAction.Initialize();
        }

        /// <summary>
        /// ����:���݂̊���
        /// </summary>
        /// <param name="value"></param>
        public void Update(float value)
        {
            currentValue = value;

            // �͈͓��Ȃ�
            if (thresholdRange.x <= currentValue && currentValue <= thresholdRange.y) { reaching = true; }
            else { reaching = false; }


            if (reaching == true)        // �͈͓���
            {
                if (beforeBool == false)    // �������u�ԂȂ�
                {
                    withinRangeAction.Enable();
                }

                inRangeAction?.Invoke();

            }

            if (beforeBool == true)      // �O��͈͓���
            {
                if (reaching == false)  // �o��u�ԂȂ�
                {
                    exitRangeAction.Enable();
                }
            }

            if (reaching == false)   // �͈͊O�Ȃ�
            {
                outOfRangeAction?.Invoke();
            }

            beforeBool = reaching;
            beforeRange = thresholdRange;
        }

    }
    #endregion

    [Serializable]
    public class SmoothRotate
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject targetObj;
        public void Initialize(GameObject targetObj)
        {
            this.targetObj = targetObj;
        }
        public void Update(Vector3 direction)
        {
            if(direction == Vector3.zero) { return; }
            Quaternion me = targetObj.transform.rotation;
            Quaternion you = Quaternion.LookRotation(direction);
            targetObj.transform.rotation = Quaternion.RotateTowards(me, you, speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Animator��Easing�Ő���ł���
    /// </summary>
    [Serializable]
    public class Easing
    {
        [SerializeField] private Traffic traffic;
        [field: SerializeField, NonEditable] public float nowTime { get; private set; }
        [field: SerializeField, NonEditable] public float evaluteValue { get; private set; }
        [field: SerializeField] public AnimationCurve curve { get; set; }

        public void Initialize()
        {
            Reset();

            traffic.Initialize();
            traffic.activeAction += Evalute;
            traffic.nonActiveAction += Reset;
        }
        public void Update()
        {
            traffic.Update();
        }

        public void Reset()
        {
            nowTime = 0.0f;
            evaluteValue *= curve.Evaluate(nowTime);
        }
        public void Evalute()
        {
            evaluteValue = curve.Evaluate(nowTime);
            nowTime += Time.deltaTime;
        }

        public bool active
        {
            get { return traffic.active; }
            set { traffic.active = value; }
        }
    }

    /// <summary>
    /// Animator��Easing�����Ő���ł���
    /// </summary>
    [Serializable]
    public class EasingAnimator
    {
        [SerializeField] private Traffic traffic;
        [field: SerializeField, NonEditable] public float nowRatio { get; private set; }
        [field: SerializeField, NonEditable] public float maxTime { get; private set; }
        [SerializeField] private AnimationCurve curve;
        public Animator animator { get; set; }

        public void Initialize(float maxTime, Animator animator = null)
        {
            if (animator != null) { this.animator = animator; }
            this.maxTime = maxTime;
            nowRatio = 0.0f;


            traffic.Initialize();
            traffic.activeAction += Evalute;
            traffic.nonActiveAction += Reset;
        }
        public void Update()
        {
            traffic.Update();
        }

        public void Reset()
        {
            nowRatio = 0.0f;
        }
        public void Evalute()
        {
            animator.speed *= curve.Evaluate(nowRatio);
            nowRatio += 1 / maxTime * Time.deltaTime;
        }

        public bool active
        {
            get { return traffic.active; }
            set { traffic.active = value; }
        }
    }

    

    /// <summary>
    /// Update���ł���x�������s�ł���
    /// </summary>
    [Serializable]
    public class MomentAction
    {
        public Action action { get; set; }
        [SerializeField, NonEditable] private bool activated;

        public void Initialize()
        {
            activated = false;
        }

        public void Enable()
        {
            if (activated == false)
            {
                action?.Invoke();
                activated = true;
            }
        }
    }

    [Serializable]
    public class Shake
    {
        [field: SerializeField] public Interval interval { get; set; }
        [field: SerializeField] public GameObject targetObj { get; set; }

        public void Initialize()
        {
            interval.Initialize(true);
        }

        public void Update()
        {
            if (interval.active == true)
            {
            }
            interval.Update();

        }

    }

    [Serializable]
    public class EntityAndPlan<T>
    {
        [field: SerializeField, NonEditable] public T entity { get; set; }
        [field: SerializeField, NonEditable] public T plan { get; set; }

        /// <summary>
        /// plan = entity
        /// </summary>
        public void Assign()
        {
            plan = entity;
        }

        public void Default()
        {
            plan = default;
            entity = default;
        }
        public void Reset(T t1)
        {
            plan = t1;
            entity = t1;
        }
    }

    
    [Serializable]
    public class HorizontalRect
    {
        [field: SerializeField] public float x { get; private set; }
        [field: SerializeField] public float y { get; private set; }
        [field: SerializeField] public float width { get; private set; }
        [field: SerializeField] public float height { get; private set; }
        public Rect entity { get; private set; }

        public HorizontalRect(Rect rect)
        {
            x = rect.x;
            y = rect.y;
            width = rect.width;
            height = rect.height;

            entity = rect;
        }

        public void Initialize(Rect rect)
        {
            x = rect.x;
            y = rect.y;
            width = rect.width;
            height = rect.height;

            entity = rect;
        }

        public void Set(float x, float width)
        {
            this.x = x;
            this.width = width;

            entity = new Rect(this.x, y, this.width, height);
        }

        public float X
        {
            set
            {
                x = value;
                entity = new Rect(this.x, y, this.width, height);
            }
        }

        public float Width
        {
            set
            {
                width = value;
                entity = new Rect(this.x, y, this.width, height);
            }
        }

    }

    [Serializable] public class VariedTime
    {
        public enum IncreseType
        {
            DeltaTime,
            Frame,
            Manual,
        }

        [field: SerializeField, NonEditable] public float value { get; private set; }
        [SerializeField] private IncreseType increseType;
        [SerializeField] private bool reversalIncrese;
        public void Initialize(float startTime = 0.0f)
        {
            value = startTime;
        }
        public void Update(float value = 0.0f)
        {
            switch(increseType)
            {
                case IncreseType.DeltaTime:
                    if (reversalIncrese == true) { this.value -= Time.deltaTime; }
                    else { this.value += Time.deltaTime; }
                    break;

                case IncreseType.Frame:
                    if(reversalIncrese == true) { this.value++; }
                    else { this.value--; }
                    break;

                case IncreseType.Manual:
                    this.value = value;
                    break;
            }

        }

    }

    [Serializable] public class Vector3T<T> : IEnumerable<T>
    {
        [field: SerializeField] public T x { get; set; }
        [field: SerializeField] public T y { get; set; }
        [field: SerializeField] public T z { get; set; }

        private T[] elements;

        public Vector3T(T x, T y, T z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            elements = new T[3];

            Assign();

        }

        public void Assign()
        {
            elements[0] = x;
            elements[1] = y;
            elements[2] = z;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Vector3TEnumerator(elements);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Vector3TEnumerator : IEnumerator<T>
        {
            private T[] elements;
            private int currentIndex = -1;
            public Vector3TEnumerator(T[] elements)
            {
                this.elements = elements;
            }

            public bool MoveNext()
            {
                currentIndex++;
                return currentIndex < elements.Length;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public T Current
            {
                get
                {
                    try
                    {
                        return elements[currentIndex];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            object IEnumerator.Current => Current;

            public void Dispose()
            {

            }
        }
    }

    public class ValueChecker<T> where T : struct
    {
        [SerializeField] private T value;
        [SerializeField] private T beforeValue;
        [SerializeField] private bool changed;
        public Action changedAction { get; set; }

        public void Initialize(T value)
        {
            Reset(value);
            changedAction = null;
        }

        public void Reset(T value)
        {
            this.value = value;
            beforeValue = value;
            changed = false;
        }

        public void Update(T value)
        {
            this.value = value;
            changed = !value.Equals(beforeValue);   // �ύX����Ă�����

            if (changed == true)
            {
                changedAction?.Invoke();
            }
        }
    }
    /// <summary>
    /// SpriteRenderer,Image,TMPro�̂��ׂĂ��擾����
    /// </summary>
    [Serializable]
    public class SpriteOrImage
    {
        [field: SerializeField] public SpriteRenderer[] sprites { get; set; }
        [field: SerializeField] public Image[] images { get; set; }
        [field: SerializeField] public TextMeshProUGUI[] texts { get; set; }

        /// <summary>
        /// ������SpriteRenderer�܂���Image���A�^�b�`���ꂽGameObject
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Initialize(GameObject obj)
        {
            sprites = obj.GetComponentsInChildren<SpriteRenderer>();
            images = obj.GetComponentsInChildren<Image>();
            texts = obj.GetComponentsInChildren<TextMeshProUGUI>();
            if (sprites.Length == 0 && images.Length == 0 && texts.Length == 0)
            {
                Debug.LogError("��������A�^�b�`����Ă��܂���");
            }
        }

        /// <summary>
        /// SpriteRenderer�܂���Image��Color��Ԃ�<br/>
        /// �������݂��Ȃ��ꍇ�̓G���[���b�Z�[�W���o����Color.white��Ԃ�
        /// </summary>
        public Color color
        {
            get
            {
                if (sprites.Length != 0)
                {

                    return sprites[0].color;
                }
                else if (images.Length != 0)
                {
                    return images[0].color;
                }

                Debug.LogError("SpriteRenderer�܂���Image���A�^�b�`���Ă�������");
                return Color.white;
            }
            set
            {
                if (sprites.Length != 0)
                {
                    foreach (SpriteRenderer sprite in sprites)
                    {
                        sprite.color = value;

                    }
                }
                else if (images.Length != 0)
                {
                    foreach (Image image in images)
                    {

                        image.color = value;
                    }
                }
                else if (texts.Length != 0)
                {
                    foreach (TextMeshProUGUI text in texts)
                    {

                        text.color = value;
                    }
                }
                else
                {
                    Debug.LogError("SpriteRenderer�܂���Image���A�^�b�`���Ă�������");
                }
            }
        }

        public float Alpha
        {
            get
            {
                if (sprites.Length != 0)
                {

                    return sprites[0].color.a;
                }
                else if (images.Length != 0)
                {
                    return images[0].color.a;
                }

                Debug.LogError("SpriteRenderer�܂���Image���A�^�b�`���Ă�������");
                return 0.0f;
            }
            set
            {
                if (sprites.Length != 0)
                {
                    foreach (SpriteRenderer sprite in sprites)
                    {
                        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);

                    }
                }
                else if (images.Length != 0)
                {
                    foreach (Image image in images)
                    {

                        image.color = new Color(image.color.r, image.color.g, image.color.b, value);
                    }
                }
                else if (texts.Length != 0)
                {
                    foreach (TextMeshProUGUI text in texts)
                    {

                        text.color = new Color(text.color.r, text.color.g, text.color.b, value);
                    }
                }
                else
                {
                    Debug.LogError("SpriteRenderer�܂���Image���A�^�b�`���Ă�������");
                }
            }
        }

    }

    [Serializable] public class VecT<T>
    {
        public T x;
        public T y;
        public T z;

        private List<T> list = new List<T>();

        public VecT()
        {
            GetList();
        }

        private List<T> GetList()
        {
            list.Clear();
            list = new List<T>() { x, y, z };

            return list;
        }

        public void Assign()
        {
            x = list[0];
            y = list[1];
            z = list[2];
        }
        public void Assign(int index)
        {
            switch (index)
            {
                case 0:
                    x = list[0];
                    break;
                case 1:
                    y = list[1];
                    break;
                case 2:
                    z = list[2];
                    break;
            }
        }

        public void Assign(int index, T t)
        {
            switch (index)
            {
                case 0:
                    x = t;
                    break;
                case 1:
                    y = t;
                    break;
                case 2:
                    z = t;
                    break;
            }
        }

        public T IndexToEntity(int index)
        {
            switch (index)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
            }

            Debug.Log("Index���Ⴂ�܂�");
            return default;
        }

        public static int count
        {
            get {  return 3; }
        }

        public List<T> List
        {
            get { return GetList(); }
        }
    }
    [Serializable] public class Vec3Bool
    {
        public bool x;
        public bool y;
        public bool z;

        public VecT<bool> ConvertToVecT()
        {
            VecT<bool> returnVecT = new VecT<bool>();
            returnVecT.x = x;
            returnVecT.y = y;
            returnVecT.z = z;

            return returnVecT;
        }
    }
    [Serializable] public class Range
    {
        public enum ThanType
        {
            ThanOrEqual,
            Than,
        }
        public ThanType minThan;
        public float min;
        public ThanType maxThan;
        public float max;
        public bool minExcess;
        public bool maxExcess;

        public bool JudgeRange(float value)
        {
            switch (minThan)
            {
                case ThanType.ThanOrEqual:
                    if(min > value) {
                        minExcess = true;
                        return false; 
                    }
                    break;
                case ThanType.Than:
                    if(min >= value)
                    {
                        minExcess = true;
                        return false; 
                    }
                    break;
            }
            minExcess = false;


            switch (maxThan) 
            { 
                case ThanType.ThanOrEqual:
                    if(value > max) 
                    { 
                        maxExcess = true;
                        return false; 
                    }
                    break;
                case ThanType.Than:
                    if(value >= max) 
                    { 
                        maxExcess = true;
                        return false; 
                    }
                    break;
            }
            maxExcess = false;

            return true;
        }
    }

    /// <summary>
    /// Range�Ƃ����]������l��������l���܂�
    /// </summary>
    [Serializable] public class ValueInRange : Range
    {
        public bool inRange;
        public float currentValue;
        public void Update(float value = 0.0f)
        {
            if (value != 0.0f) { currentValue = value; }

            inRange = JudgeRange(currentValue);
        }
    }

    
    [Serializable] public class PosRange
    {
        [field: SerializeField] public Vec3Bool enableAxis { get; set; }
        [field: SerializeField] public VecT<ValueInRange> valueInRange { get; private set; } = new VecT<ValueInRange>();
        
        /// <summary>
        /// �C���X�y�N�^��center�͎Q�Ƃ��܂���
        /// </summary>
        /// <param name="centerPos"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public Vector3 Update(Vector3 centerPos, Vector3 targetPos)
        {
            VecT<float> vecTCenter = new VecT<float>();
            VecT<float> vecTTarget = new VecT<float>();

            AddFunction.VecTFloatConvert(vecTCenter, centerPos);
            AddFunction.VecTFloatConvert(vecTTarget, targetPos);

            for (int i = 0; i < VecT<float>.count; ++i)
            {
                if (enableAxis.ConvertToVecT().List[i] == true)
                {
                    valueInRange.List[i].Update(AddFunction.IndexToVec3(i, vecTTarget));
                    vecTTarget.Assign(i, Mathf.Clamp(vecTTarget.IndexToEntity(i), valueInRange.List[i].min + vecTCenter.List[i], valueInRange.List[i].max + vecTCenter.List[i]));
                }
            }

            Vector3 returnPos = AddFunction.VecTFloatConvert(vecTTarget);

            return returnPos;
        }
        public Vector3 Update(Transform centerTra, Transform targetTra)
        {
            VecT<float> vecTCenter = new VecT<float>();
            VecT<float> vecTTarget = new VecT<float>();

            AddFunction.VecTFloatConvert(vecTCenter, centerTra.position);
            AddFunction.VecTFloatConvert(vecTTarget, targetTra.position);

            for (int i = 0; i < VecT<float>.count; ++i)
            {
                if (enableAxis.ConvertToVecT().List[i] == true)
                {

                    valueInRange.List[i].Update(AddFunction.IndexToVec3(i, vecTTarget));
                    vecTTarget.Assign(i, Mathf.Clamp(vecTTarget.IndexToEntity(i), valueInRange.List[i].min + vecTCenter.List[i], valueInRange.List[i].max + vecTCenter.List[i]));
                }
            }

            Vector3 returnPos = AddFunction.VecTFloatConvert(vecTTarget);

            return returnPos;
        }

    }


    [Serializable] public class Vec3Curve
    {
        public AnimationCurve xCurve;
        public AnimationCurve yCurve;
        public AnimationCurve zCurve;
        private List<AnimationCurve> curves = new List<AnimationCurve>();
        
        public void Initialize()
        {
            Reset();
        }

        public void Reset()
        {

            curves.Clear();
            curves.Add(xCurve);
            curves.Add(yCurve);
            curves.Add(zCurve);

        }

        public void ZeroFill()
        {
            for(int i = 0; i < curves.Count; ++i)
            {
                bool artificial = false;
                if (curves[i].length != 0 && curves[i].length != 1)
                {
                    artificial = true;
                }

                if(artificial == false)
                {
                    for (int j = 0; j < curves[i].length; ++j)
                    {

                        curves[i].RemoveKey(j);
                    }

                    curves[i].AddKey(0.0f, 0.0f);
                    curves[i].AddKey(1.0f, 0.0f);

                    for (int j = 0; j < curves[i].length; ++j)
                    {
                        curves[i].MoveKey(j, new Keyframe(j, 0));
                    }

                    curves[i].postWrapMode = WrapMode.Loop;
                    curves[i].preWrapMode = WrapMode.Loop;
                }
            }
        }
        public void Clear()
        {
            for (int i = 0; i < curves.Count; ++i)
            {
                for (int k = 0; k < 3; ++k)
                {
                    for (int j = 0; j < curves[i].length; ++j)
                    {
                        curves[i].MoveKey(j, new Keyframe(0, 0));
                        curves[i].RemoveKey(0);
                    }

                }
            }
        }

        /// <summary>
        /// ���ݎ���
        /// </summary>
        public Vector3 Eva(float time)
        {
            Vector3 newEva;
            newEva.x = xCurve.Evaluate(time);
            newEva.y = yCurve.Evaluate(time);
            newEva.z = zCurve.Evaluate(time);

            return newEva;
        }

    }

    /// <summary>
    /// �}�`��localScale.x�܂���y���Q�ƒl�ɍ��킹�Ċg�k������
    /// </summary>
    [Serializable]
    public class BarByParam
    {

        [SerializeField] private GameObject bar;
        [SerializeField] private float entity;
        [SerializeField] private float max;
        [SerializeField] private float ratio;
        [SerializeField] private bool warp;

        public void Update(float entity, float max)
        {
            this.entity = entity;
            this.max = max;
            ratio = entity / max;
            if (warp == true)
            {
                bar.transform.localScale = new Vector3(bar.transform.localScale.x, ratio);

            }
            else
            {
                bar.transform.localScale = new Vector3(ratio, bar.transform.localScale.y);


            }
        }
    }




#if UNITY_EDITOR
    /// <summary>
    /// serializedObjectUpdate�Ɋ֐���ǉ�����
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
    /// �\������ϐ�����string��p�ӂ���<br/>
    /// Update���㏑������
    /// </summary>
    public class MyPropertyDrawer : PropertyDrawer
    {
        public Rect pos;
        public SerializedProperty prop;
        public float boolWidth = 15;
        public float uniformedLabelWidth;
        public float uniformedFieldWidth;
        public float labelWidthAve;
        public float distance = 4.1f;       // PropertyDrawer�̒萔
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                pos = position; 
                pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

                prop = property;

                // �q�̃t�B�[���h���C���f���g���Ȃ� 
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                Update(position, property, label);


                // �C���f���g�����ʂ�ɖ߂��܂�
                EditorGUI.indentLevel = indent;
            }
            EditorGUI.EndProperty();


        }

        public float RightEnd(Rect pos)
        {
            return pos.width - 90;
        }
        protected virtual void Update(Rect pos, SerializedProperty prop, GUIContent label)
        { }

        protected virtual void ReadOnly(Rect pos, SerializedProperty property, GUIContent label)
        { }

        /// <summary>
        /// Update�̍ŏ��ɍs��<br/>
        /// �����͕\������v�f��<br/>
        /// Label�̑傫��
        /// </summary>
        /// <param name="horizontalElements"></param>
        public void Uniform(int horizontalElements, float labelWidth = 30)
        {
            uniformedLabelWidth = labelWidth;
            uniformedFieldWidth = pos.width / horizontalElements - uniformedLabelWidth - distance * 2;

        }


        public float UniformedRatio(float horizontalElements, float ratio)
        {
            return (pos.width / horizontalElements - labelWidthAve - distance * 2) * (ratio * 2);
        }

        public void UniformedDraw(List<LabelAndproperty> lavProps)
        {

            for (int i = 0; i < lavProps.Count; i++)
            {
                lavProps[i].Set(pos, prop);
                if (i == 0)
                {
                    lavProps[i].InitialPosSet(pos.x, uniformedLabelWidth, uniformedFieldWidth);
                    
                }
                else
                {
                    LabelAndproperty neighbor = lavProps[i - 1];
                    lavProps[i].Uniform(neighbor);
                }
                lavProps[i].Draw();
            }
        }

        public void UniformedDraw(List<LabelAndproperty> lavProps, float labelWidth, float propWidth)
        {

            for (int i = 0; i < lavProps.Count; i++)
            {
                lavProps[i].Set(pos, prop);
                if (i == 0)
                {
                    lavProps[i].InitialPosSet(pos.x, labelWidth, propWidth);

                }
                else
                {
                    LabelAndproperty neighbor = lavProps[i - 1];
                    lavProps[i].Uniform(neighbor);
                }
                lavProps[i].Draw();
            }
        }
        public class LabelAndproperty
        {
            public HorizontalRect labelRect;
            public string label;
            public HorizontalRect propertyRect;
            public SerializedProperty property;
            public string propName;
            public LabelAndproperty neighbor;
            public EditType edit;

            /// <summary>
            /// �R���X�g���N�^
            /// </summary>
            /// <param name="rect"></param>
            /// <param name="property"></param>
            
            public LabelAndproperty(string propertyName)
            {
                this.propName = propertyName;
            }
            public void Set(Rect rect, SerializedProperty property)
            {
                labelRect = new HorizontalRect(rect);
                propertyRect = new HorizontalRect(rect);
                this.property = property.FindPropertyRelative(propName);

                char[] array = this.property.displayName.ToCharArray();
                array[0] = char.ToUpper(array[0]);  // �퓬��啶���ɂ���

                label = new string(array);
            }


            public void InitialPosSet(float x, float labelWidth, float fieldWidth)
            {
                labelRect.Set(x, labelWidth);
                propertyRect.Set(AddFunction.Neighbor(labelRect) + 5, fieldWidth);
            }
            public void NeighborPosSet(float labelWidth, float fieldWidth)
            {
                labelRect.Set(AddFunction.Neighbor(neighbor.propertyRect) + 5, labelWidth);
                propertyRect.Set(AddFunction.Neighbor(labelRect) + 5, fieldWidth);
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
            public void DrawPropertyField()
            {
                EditorGUI.LabelField(labelRect.entity, label);
                EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
            }

            public void Uniform(LabelAndproperty target)
            {
                labelRect.Set(AddFunction.Neighbor(target.propertyRect) + 5, target.labelRect.width);
                propertyRect.Set(AddFunction.Neighbor(labelRect) + 5, target.propertyRect.width);
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

    #region �C���X�y�N�^�[�v���p�e�B
    /// <summary>
    /// ���l�̒��g�ƍő�l���܂�<br/>
    /// �C���X�^���X���s�v
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


        public bool overZero    // entity��0�ȉ��Ȃ�
        {
            get
            {
                if (entity <= 0.0f) { return true; }
                return false;
            }
        }

        /// <summary>
        /// �g�p�\�Ȃ�
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

            entity.edit = EditType.NonEditable;
            List<LabelAndproperty> list = new List<LabelAndproperty>() { entity, max, autoRecoverValue };
                      
            
            Uniform(list.Count, 40);
            UniformedDraw(list);
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
            
            List<LabelAndproperty> lavProps = new List<LabelAndproperty>() { x, y, z };
            Uniform(lavProps.Count, 10);
            foreach(LabelAndproperty l in lavProps)
            {
                l.edit = EditType.NonEditableInGame;
            }
            UniformedDraw(lavProps, uniformedLabelWidth, boolWidth);
        }
    }

    //[CustomPropertyDrawer(typeof(Vec3Curve))]
    //public class Vec3CurveDrawer : MyPropertyDrawer
    //{
    //    LabelAndproperty xCurve = new LabelAndproperty(nameof(xCurve));
    //    LabelAndproperty yCurve = new LabelAndproperty(nameof(yCurve));
    //    LabelAndproperty zCurve = new LabelAndproperty(nameof(zCurve));
    //    protected override void Update(Rect pos, SerializedProperty prop, GUIContent label)
    //    {
    //        List<LabelAndproperty> lavProps = new List<LabelAndproperty>() { xCurve, yCurve, zCurve };
    //        Uniform(lavProps.Count, 10);
    //        foreach(LabelAndproperty l in lavProps)
    //        {
    //            l.edit = EditType.NonEditableInGame;
    //        }
    //        UniformedDraw(lavProps);
    //    }
    //}
#endif

    #endregion
}
