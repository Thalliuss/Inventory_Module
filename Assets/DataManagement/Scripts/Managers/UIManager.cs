using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

/// <copyright file="UIManager.cs">
/// Copyright (c) 2019 All Rights Reserved
/// </copyright>
/// <author>Kevin Hummel</author>
/// <date>18/03/2019 21:41 PM </date>
/// <summary>
/// This class handles opening the save and load menu.
/// It also handles initializing the loadingscreen on screen.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }

    [SerializeField] private GameObject _menu = null;
    [SerializeField] private GameObject _loading = null;
    [SerializeField] private Text _loadingText = null;

    [SerializeField] private Slider _loadingBar = null;
    public Slider LoadingBar => _loadingBar;

    private bool _menuOpened = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        DontDestroyOnLoad(this);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UIHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !LoadingscreenManager.Instance.IsLoading && !LoadingscreenManager.Instance.IsSetupScene)
        {
            _menuOpened = !_menuOpened;
            _menu.SetActive(_menuOpened);

            Cursor.visible = _menuOpened;
            if (!_menuOpened)
            {
                FindObjectOfType<FirstPersonController>().mouseLook.lockCursor = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                FindObjectOfType<FirstPersonController>().mouseLook.lockCursor = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void OpenLoading(string p_input)
    {
        _loading.SetActive(true);
        _loadingText.text = p_input;
    }

    public void CloseLoading()
    {
        _loading.SetActive(false);
        _loadingText.text = "";
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void Update()
    {
        UIHandler();
    }
}
