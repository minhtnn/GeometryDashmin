using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Assets.Scripts
{
    public class MenuScript : MonoBehaviour
    {
        [SerializeField] private string MenuScene;
        [SerializeField] private string choosingLevelScreen;

        public AudioManager audioManager;
        public VideoPlayerScript videoManager;

        public GameObject settingMenu;
        public static bool isPause = false;
        public static bool isWin= false;
        public GameObject winningMenu;

        public Transform player;
        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            if (GameObject.FindGameObjectWithTag("Video").GetComponent<VideoPlayerScript>() != null)
            {
                videoManager = GameObject.FindGameObjectWithTag("Video").GetComponent<VideoPlayerScript>();
            }
        }
        private void Start()
        {
            settingMenu.SetActive(false);
            winningMenu.SetActive(false);
        }
        private void Update()
        {
            // Kiểm tra nếu người dùng nhấn phím Esc
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPause)
                {
                    // Đóng menu cài đặt nếu đang mở
                    CloseOption();
                }
                else
                {
                    // Mở menu cài đặt nếu chưa mở
                    OpenOption();
                }
            }
            Wining();
            Debug.Log("Pause: " + isPause);

        }
        public void StartGame()
        {
            Time.timeScale = 1.0f;
            if (!MenuScript.isPause)
            {
                StartCoroutine(DelayedAction(audioManager.buttonSelection));
            }
        }
        IEnumerator DelayedAction(AudioClip audio)
        {
            ButtonSelection(audio);
            yield return new WaitForSeconds(audio.length);
            SceneManager.LoadScene(choosingLevelScreen);
        }
        public void ButtonSelection(AudioClip clip)
        {
            audioManager.StopBackgroundMusic();
            if (clip)
            {
                audioManager.PlaySFX(clip, audioManager.SFXVolume);
            }
        }
        public void Quit()
        {
            Time.timeScale = 1.0f;
            //Application.Quit();
            if (!MenuScript.isPause)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
                
        }
        public void OpenOption()
        {
            if (!MenuScript.isPause)
            {
                isPause = true;
                settingMenu.SetActive(true);
                audioManager.PauseBackgroundMusic();
                Time.timeScale = 0f;
            }
        }
        public void CloseOption()
        {
            if (MenuScript.isPause)
            {
                isPause = false;
                settingMenu.SetActive(false);
                audioManager.UnpauseBackgroundMusic();
                Time.timeScale = 1.0f;
            }
        }

        public void BackToMainMenu()
        {
            if (MenuScript.isPause)
            {
                audioManager.StopBackgroundMusic();
                SceneManager.LoadScene(MenuScene);
                audioManager.PlayBackgroundMusic();
                if (videoManager)
                {
                    videoManager.PlayVideo();
                }
                
                Time.timeScale = 1.0f;
            }
            MenuScript.isPause = false;
        }
        public void ReplayStage()
        {
            if (MenuScript.isPause)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1.0f;
                
            }
            isPause = false;
        }
        public void Wining()
        {
            if (player.position.x > 137f)
            {
                isWin = true;
                OpenWinOption();
                settingMenu.SetActive(false);
            }
            else
            {
                isWin = false;
                CloseOptionWin();
                if (isPause) { settingMenu.SetActive(true); }
            }
        }
        public void OpenWinOption()
        {
            if (MenuScript.isWin)
            {
                winningMenu.SetActive(true);
                audioManager.PauseBackgroundMusic();
            }
        }
        public void CloseOptionWin()
        {
            if (!MenuScript.isWin)
            {
                winningMenu.SetActive(false);
                audioManager.UnpauseBackgroundMusic();
            }
        }
        public void BackToMainMenuWin()
        {
            if (MenuScript.isWin)
            {
                audioManager.StopBackgroundMusic();
                SceneManager.LoadScene(MenuScene);
                audioManager.PlayBackgroundMusic();
                if (videoManager)
                {
                    videoManager.PlayVideo();
                }

                Time.timeScale = 1.0f;
            }
            MenuScript.isWin = false;
        }
        public void ReplayStageWin()
        {
            if (MenuScript.isWin)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1.0f;

            }
            isWin = false;
        }
        public void PlayNextGame()
        {
            if (isWin)
            {
                StartCoroutine(DelayedAction(audioManager.buttonSelection));
                isWin = false;
            }
        }
    }
}
