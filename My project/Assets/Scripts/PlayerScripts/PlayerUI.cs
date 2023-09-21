using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerScripts
{
    public class PlayerUI : MonoBehaviour
    {
        public bool pause;
        public GameObject UI;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !pause)
            {
                pause = true;
                UI.SetActive(true);
            }

            else if (Input.GetKeyDown(KeyCode.Escape) && pause)
            {
                pause = false;
                UI.SetActive(false);
            }
        }

        public void Exit()
        {
            Application.Quit();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                SceneManager.LoadScene("End");
            }
        }
    }
}