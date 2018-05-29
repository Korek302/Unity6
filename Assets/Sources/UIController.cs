using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameObject _finishPanel;
    private int _score;
    private Text _scoreText;

    private void Start()
    {
        _score = 0;
        _finishPanel = this.GetComponentsInChildren<RectTransform>()[2].gameObject;
        _finishPanel.SetActive(false);
        _scoreText = this.GetComponentInChildren<Text>();
        Messenger.AddListener(GameEvent.PICK_UP, OnPickUp);
    }

    private void OnPickUp()
    {
        _score++;
        if (_scoreText != null)
        {
            _scoreText.text = "Score: " + _score.ToString();
        }
    }

    private void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Find") == null)
        {
            _finishPanel.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Example");
    }
}
