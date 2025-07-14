using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mfram.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]
        public  string      startScene = string.Empty;
        
        private CanvasGroup fadeCanvasGroup;
        private bool        isfade;

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        
        private IEnumerator Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return LoadSceneSetAtive(startScene);
            EventHandler.CallAfterTransitionEvent();
        }

        private void OnTransitionEvent(string sceneToGo , Vector3 positionToGo)
        {
            if(!isfade)
                StartCoroutine(Transition(sceneToGo , positionToGo));
                
            
        }
        /// <summary>
        /// 場景切換
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName , Vector3 targetPos)
        {
            EventHandler.CallBeforeTransitionEvent();
            yield return Fade(1);
            
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetAtive(sceneName);
            //移動人物座標
            EventHandler.CallMoveToPositionEvent(targetPos);
            
            EventHandler.CallAfterTransitionEvent();
            yield return Fade(0);
            
        }

        private IEnumerator Fade(float targetAlpha)
        {
            isfade                         = true;
            fadeCanvasGroup.blocksRaycasts = true;
            float speed=Mathf.Abs(fadeCanvasGroup.alpha-targetAlpha)/Settings.FadeDuration;
            while (!Mathf.Approximately(fadeCanvasGroup.alpha , targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed*Time.deltaTime);
                yield return null;
            }
            fadeCanvasGroup.blocksRaycasts = false;
            isfade                         = false;
        }

        /// <summary>
        /// 加載場景並啟動
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetAtive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName , LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
        }
    }

}
