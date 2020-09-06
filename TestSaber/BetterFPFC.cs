using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestSaber
{
    class BetterFPFC: MonoBehaviour
    {
        internal static BetterFPFC instance { get; private set; }
        private Camera camera;
        private MouseLook mouseLook = new MouseLook();

        private static Quaternion cameraLocalRotation;
        private static Quaternion characterLocalRotation;
        private static Vector3 characterPosition;

        private const string FLY_UP_BTTN = "space";
        private const string FLY_DOWN_BTTN = "control";
        private const string FAST_BTTN = "shift";
        private const string FORWARD_BTTN = "w";
        private const string REVERSE_BTTN = "s";
        private const string LEFT_BTTN = "a";
        private const string RIGHT_BTTN = "d";
        private const float NORMAL_SENS = 0.001f;
        private const float FAST_SENS = 0.01f;

        internal static void Load()
        {
            new GameObject("BetterFPFC").AddComponent<BetterFPFC>();
            Camera oldMainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            oldMainCamera.enabled = false;
            instance.camera = new GameObject().AddComponent<Camera>();
            instance.camera.tag = "MainCamera";
            instance.camera.transform.parent = instance.transform;
            RestoreCameraState();
            instance.mouseLook.Init(instance.transform, instance.camera.transform);
        }

        public virtual void OnEnable()
        {
            instance.mouseLook.SetCursorLock(true);
        }

        private void Update()
        {
            instance.mouseLook.LookRotation(instance.transform, camera.transform);

            Vector3 position = instance.transform.position;
            float sens = NORMAL_SENS;
            Vector3 a = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                a = camera.transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                a = -camera.transform.forward;
            }
            Vector3 b = Vector3.zero;
            if (Input.GetKey(KeyCode.D))
            {
                b = camera.transform.right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                b = -camera.transform.right;
            }
            Vector3 c = Vector3.zero;
            if (Input.GetKey(KeyCode.Space))
            {
                c = camera.transform.up;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                c = -camera.transform.up;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                sens = FAST_SENS;
            }
            position += (a + b + c) * sens;
            instance.transform.position = position;
        }

        private static void SaveCameraState()
        {
            characterLocalRotation = instance.transform.localRotation;
            cameraLocalRotation = instance.camera.transform.localRotation;
            characterPosition = instance.transform.position;
        }

        private static void RestoreCameraState()
        {
            instance.transform.localRotation = characterLocalRotation;
            instance.camera.transform.localRotation = cameraLocalRotation;
            instance.transform.position = characterPosition;
        }

        private void Awake()
        {
            if (instance != null)
            {
                Logger.log?.Warn($"Instance of {this.GetType().Name} already exists, destroying.");
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this);
            instance = this;
            Logger.log?.Debug($"{name}: Awake()");
        }

        private void OnDestroy()
        {
            SaveCameraState();
            GameObject.Destroy(camera);
            Camera oldMainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            oldMainCamera.enabled = true;
            Logger.log?.Debug($"{name}: OnDestroy()");
            instance = null;
        }
    }
}
