using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Animator selectorAnimator;
    public GameObject transition;

    public Button selectedButton;
    private MenuButton _selectedMenuButton;

    public List<Button> buttons;

    private bool _disableSelection = false;
    private int actualIndex = 0;

    void Start()
    {
        SelectAButton(selectedButton);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_disableSelection)
        {
            if (Input.GetButtonDown("Down") && actualIndex < buttons.Count - 1)
            {
                actualIndex++;
                SelectAButton(buttons[actualIndex]);
            }

            if (Input.GetButtonDown("Up") && actualIndex > 0)
            {
                actualIndex--;
                SelectAButton(buttons[actualIndex]);
            }

            if (Input.GetButtonDown("Fire") || Input.GetButtonDown("Submit"))
            {
                selectorAnimator.SetTrigger("Select");
                selectedButton.onClick.Invoke();
                
                _disableSelection = true;
            }
        }
    }

    private void SelectAButton(Button selectedButton)
    {
        this.selectedButton = selectedButton;
        _selectedMenuButton = selectedButton.GetComponent<MenuButton>();
        selectorAnimator.transform.position = _selectedMenuButton.selectPos.transform.position;
    }

    public void PlayGame()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 2));
    }

    public void GoInstructions()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    IEnumerator LoadScene(int index)
    {
        transition.SetActive(true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(index);
    }
}
