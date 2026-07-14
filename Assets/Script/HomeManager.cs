using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class HomeManager : MonoBehaviour
{
    void Update()
    {
        
        if (Keyboard.current == null || Mouse.current == null) 
        return;

        
        if (Keyboard.current.xKey.wasPressedThisFrame || Keyboard.current.cKey.wasPressedThisFrame ||Mouse.current.leftButton.wasPressedThisFrame) 
        {
            SceneManager.LoadScene("Run");
        }

    }
}