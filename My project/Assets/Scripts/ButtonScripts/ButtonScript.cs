using UnityEngine;
using UnityEngine.SceneManagement;

namespace ButtonScripts
{
    public class ButtonScript : MonoBehaviour
    {
        public void Exit()
        {
            Application.Quit();
        }
        public void PlayAgain()
        {
            SceneManager.LoadScene("Level");
        }
    }
}
