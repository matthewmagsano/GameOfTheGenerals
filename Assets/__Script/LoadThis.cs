using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadThis : MonoBehaviour
{
    [SerializeField] string nameScene;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadIt();
        }
    }

    public void LoadIt()
    {
        SceneManager.LoadScene(nameScene);
    }
}
