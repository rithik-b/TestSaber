using CustomSaber;
using System;
using System.Collections;
using UnityEngine;

namespace TestSaber
{
    public class TestSaberController : MonoBehaviour
    {
        internal static TestSaberController instance { get; private set; }
        private EventManager[] eventManagers;
        private PauseController pauseController;
        private event Action onPausePressed;
        private event Action onQuitPressed;
        private event Action onRestartPressed;

        private const string ON_LEVEL_START_BTTN = "\n";
        private const string ON_LEVEL_ENDED_BTTN = "\b";
        private const string ON_LEVEL_FAIL_BTTN = "f";
        private const string ON_SLICE_BTTN = "k";
        private const string ON_COMBO_BREAK_BTTN = "1";
        private const string MULTIPLIER_UP_BTTN = "2";
        private const string SABER_START_COLLIDING_BTTN = "3";
        private const string SABER_STOP_COLLIDING_BTTN = "4";
        private const string ON_BLUE_LIGHT_ON_BTTN = "5";
        private const string ON_RED_LIGHT_ON_BTTN = "6";
        private const string ON_COMBO_CHANGED_BTTN = "7";
        private const int COMBO = 100;
        private const string ON_ACCURACY_CHANGED_BTTN = "8";
        private const float ACCURACY = 90.0f;
        private const string PAUSE_BTTN = "escape";
        private const string QUIT_BTTN = "q";
        private const string RESTART_BTTN = "r";

        internal static void Load()
        {
            new GameObject("TestSaberController").AddComponent<TestSaberController>();
            instance.StartCoroutine(instance.LoadCoroutine());
        }
        private IEnumerator LoadCoroutine()
        {
            yield return new WaitUntil(() => GameObject.FindObjectsOfType<EventManager>() != null);
            eventManagers = GameObject.FindObjectsOfType<EventManager>();
            pauseController = GameObject.FindObjectOfType<PauseController>();
            onPausePressed += Pause;
            BetterFPFC.Load();
        }

        private void Update()
        {
            if (Input.GetKeyDown(PAUSE_BTTN))
            {
                onPausePressed.Invoke();
            }
            switch (Input.inputString)
            {
                case ON_LEVEL_START_BTTN:
                    for (int i=0; i<eventManagers.Length; i++)
                        eventManagers[i].OnLevelStart.Invoke();
                    break;
                case ON_LEVEL_ENDED_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].OnLevelEnded.Invoke();
                    break;
                case ON_LEVEL_FAIL_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].OnLevelFail.Invoke();
                    break;
                case ON_SLICE_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].OnSlice.Invoke();
                    break;
                case ON_COMBO_BREAK_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].OnComboBreak.Invoke();
                    break;
                case MULTIPLIER_UP_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].MultiplierUp.Invoke();
                    break;
                case SABER_START_COLLIDING_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].SaberStartColliding.Invoke();
                    break;
                case SABER_STOP_COLLIDING_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].SaberStopColliding.Invoke();
                    break;
                case ON_BLUE_LIGHT_ON_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].OnBlueLightOn.Invoke();
                    break;
                case ON_RED_LIGHT_ON_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].OnRedLightOn.Invoke();
                    break;
                case ON_COMBO_CHANGED_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].OnComboChanged.Invoke(COMBO);
                    break;
                case ON_ACCURACY_CHANGED_BTTN:
                    for (int i = 0; i < eventManagers.Length; i++)
                        eventManagers[i].OnAccuracyChanged.Invoke(ACCURACY);
                    break;
                case RESTART_BTTN:
                    try
                    {
                        onRestartPressed.Invoke();
                    }
                    catch (Exception) { }
                    break;
                case QUIT_BTTN:
                    try
                    {
                        onQuitPressed.Invoke();
                    } catch(Exception) { }
                    break;
            }
        }
        
        private void Pause()
        {
            pauseController.Pause();
            onPausePressed -= Pause;
            onPausePressed += Resume;
            onQuitPressed += Quit;
            onRestartPressed += Restart;
        }
        private void Resume()
        {
            pauseController.HandlePauseMenuManagerDidPressContinueButton();
            onPausePressed -= Resume;
            onPausePressed += Pause;
            onQuitPressed -= Quit;
            onRestartPressed -= Restart;
        }
        private void Quit()
        {
            pauseController.HandlePauseMenuManagerDidPressMenuButton();
            GameObject.Destroy(instance);
        }
        private void Restart()
        {
            pauseController.HandlePauseMenuManagerDidPressRestartButton();
            GameObject.Destroy(instance);
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
            Logger.log?.Debug($"{name}: OnDestroy()");
            GameObject.Destroy(BetterFPFC.instance);
            instance = null;
        }

        internal static void DestroyInstance(StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
        {
            GameObject.Destroy(instance);
        }
    }
}
