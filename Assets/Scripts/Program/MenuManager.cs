using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public void StartGame()
    {
         SceneManager.LoadScene("Simulation");      
    }

    // Use this for initialization
    private void Start () {
	}

    // Update is called once per frame
    private void Update () {
		
	}
}
