using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private InputActionReference menuInputActionReference;
        public GameObject pauseMenu;
        private bool paused = false;

        private void Start()
        {
            menuInputActionReference.action.Enable();
        }

        private void OnEnable()
        {
            // pauseMenu.SetActive(true);  // works
            menuInputActionReference.action.performed += MenuPressed;  // does not work
        }

        private void OnDisable()
        {
            menuInputActionReference.action.performed -= MenuPressed;
        }

        private void MenuPressed(InputAction.CallbackContext context)
        {
            pauseMenu.SetActive(true);  // pause game
            // if(!paused)  // game not paused
            // {
            //     pauseMenu.SetActive(true);  // pause game
            //     paused = true;
            //     // if antagonist GameObject active, deactive
            // }
            // else
            // {
            //     pauseMenu.SetActive(false);  // unpause game
            //     paused = false;
            //     // if antagonist GameObject not active, active
            // }
        }
    }
}
