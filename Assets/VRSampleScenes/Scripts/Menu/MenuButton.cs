using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Menu
{
    // This script is for loading scenes from the main menu.
    // Each 'button' will be a rendering showing the scene
    // that will be loaded and use the SelectionRadial.
    public class MenuButton : MonoBehaviour
    {
        public event Action<MenuButton> OnButtonSelected;                   // This event is triggered when the selection of the button has finished.


        [SerializeField] private int m_LevelToLoad;                      // The name of the scene to load.
        [SerializeField] private VRCameraFade m_CameraFade;                 // This fades the scene out when a new scene is about to be loaded.
        [SerializeField] private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
        [SerializeField] private VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.


        private bool m_GazeOver;                                            // Whether the user is looking at the VRInteractiveItem currently.


        private void OnEnable ()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
        }


        private void OnDisable ()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
        }
        

        private void HandleOver()
        {
            // When the user looks at the rendering of the scene, show the radial.
            m_SelectionRadial.Show();

            m_GazeOver = true;
        }


        private void HandleOut()
        {
            // When the user looks away from the rendering of the scene, hide the radial.
            m_SelectionRadial.Hide();

            m_GazeOver = false;
        }


        private void HandleSelectionComplete()
        {
            // If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
            if(m_GazeOver)
                StartCoroutine (ActivateButton());
        }


        private IEnumerator ActivateButton()
        {
            // If the camera is already fading, ignore.
            if (m_CameraFade.IsFading)
                yield break;

            // If anything is subscribed to the OnButtonSelected event, call it.
            if (OnButtonSelected != null)
                OnButtonSelected(this);

            // Wait for the camera to fade out.
            yield return StartCoroutine(m_CameraFade.BeginFadeOut(true));


			//Loading level
			string m_SceneToLoad = "";
			ApplicationManager.currentLevel = m_LevelToLoad;

			if (m_LevelToLoad.Equals (0))				
				m_SceneToLoad = "1.Labirinto_fase1";
			
			if (m_LevelToLoad.Equals (1))				
				m_SceneToLoad = "1.Labirinto_fase1";
			
			if (m_LevelToLoad.Equals(2))		
				m_SceneToLoad = "2.Labirinto_fase2";

			if (m_LevelToLoad.Equals(3))		
				m_SceneToLoad = "3.HC_fase1";

			if (m_LevelToLoad.Equals(4))		
				m_SceneToLoad = "3.HC_fase1";

			if (m_LevelToLoad.Equals(5))		
				m_SceneToLoad = "3.HC_fase1";

			if (m_LevelToLoad.Equals(6))		
				m_SceneToLoad = "3.HC_fase1";


            // Load the level.
            SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);
        }
    }
}