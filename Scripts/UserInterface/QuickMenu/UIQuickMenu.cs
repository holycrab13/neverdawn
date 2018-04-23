using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum QuickMenuBehaviourType
{
    None,
    Close,
    NavigateUp,
    Refresh,
    OpenMenu,
}

public class UIQuickMenu : UIBehaviour {

    
    [SerializeField]
    private AudioClip soundNavigateUp;
    
    /// <summary>
    /// The navigations stack
    /// </summary>
    private Stack<UIQuickMenuPage> navigationStack;

    /// <summary>
    /// The current page of the quick menu
    /// </summary>
    private UIQuickMenuPage _currentPage; 

    public UIQuickMenuPage currentPage
    {
        get { return _currentPage; }
    }
    
    /// <summary>
    /// The ui audio source
    /// </summary>
    private AudioSource audioSource;


    private AvatarController _avatarController;

    [SerializeField]
    private Image _mainIcon;

    [SerializeField]
    private Text _mainLabel;

    public Sprite icon
    {
        set { _mainIcon.sprite = value; }
    }

    [SerializeField]
    private Transform pageParent;

    private RectTransform rectTransform;

    [SerializeField]
    private RectTransform header;

    [SerializeField]
    private float switchSpeed;
    private bool _isOpen;

    public Character character;

    /// <summary>
    /// The avatar controller controlling this quick menu
    /// </summary>
    public AvatarController avatarController
    {
        get { return _avatarController; }
        set
        {
            _avatarController = value;
            GetComponent<UIInputController>().SetInputModules(_avatarController ? _avatarController.inputModule : null);
        }
    }

    void Start()
    {

        rectTransform = GetComponent<RectTransform>();
        navigationStack = new Stack<UIQuickMenuPage>();
        audioSource = GetComponentInParent<AudioSource>();
    }


    /// <summary>
    /// Use the input module to navigate up
    /// </summary>
    void Update()
    {
        if (!character)
            Destroy(gameObject);

        if (avatarController != character.controller)
        {
            avatarController = character.controller as AvatarController;
        }

        float delta = switchSpeed * Time.deltaTime;
        float currentX = rectTransform.anchoredPosition.x;
        float currentY = rectTransform.anchoredPosition.y;
        float sizeY = rectTransform.sizeDelta.y;
        float headerSizeY = header.sizeDelta.y;

        if (navigationStack.Count == 0)
        {
            rectTransform.anchoredPosition = new Vector3(currentX, Mathf.MoveTowards(currentY, headerSizeY, delta * 2.0f));
            return;
        }

        if (navigationStack.Peek() != _currentPage)
        {
            UIQuickMenuPage targetPage = navigationStack.Peek();
            targetPage.gameObject.SetActive(true);

            float targetY = (targetPage == null || targetPage.includeHeader) ? headerSizeY : 0.0f;

            rectTransform.anchoredPosition = new Vector3(currentX, Mathf.MoveTowards(currentY, targetY, delta * 2.0f));

            if (currentY == targetY)
            {
                if (_currentPage != null)
                {
                    if (!navigationStack.Contains(_currentPage))
                    {
                        Destroy(_currentPage.gameObject);
                    }
                    else
                    {
                        _currentPage.gameObject.SetActive(false);
                    }
                }

                header.gameObject.SetActive(targetPage == null ? true : targetPage.includeHeader);
                _currentPage = targetPage;

                if (_currentPage != null)
                {
                    _currentPage.gameObject.SetActive(true);
                    StartCoroutine(_currentPage.enablePage());
                }
            }

        }
        else
        {
            rectTransform.anchoredPosition = new Vector3(currentX, Mathf.MoveTowards(currentY, sizeY, delta));
        }

    }

    /// <summary>
    /// Set a specific quick menu page
    /// </summary>
    /// <param name="page"></param>
    public void Open(UIQuickMenuPage page)
    {
        clear();
        addPage(page);
    }

    /// <summary>
    /// Navigate into a page. The current page will be pushed to the navigation stack.
    /// </summary>
    /// <param name="quickMenuPage"></param>
    public void NavigateInto(UIQuickMenuPage quickMenuPage)
    {
        addPage(quickMenuPage);
    }

    /// <summary>
    /// Plays a sound a moves up in the navigation stack.
    /// </summary>
    public void GoBack()
    {
        audioSource.PlayOneShot(soundNavigateUp);
        navigateUp();
    }

    /// <summary>
    /// Clears the navigation stack and closes the menu
    /// </summary>
    public void Close()
    {
        clear();
        _isOpen = false;
        // StartCoroutine(deactivate());
    }

    /// <summary>
    /// Sets the current page. Marks the page as a child, activates the game object and calls onPageEnabled
    /// </summary>
    /// <param name="page"></param>
    private void addPage(UIQuickMenuPage page)
    {
        if(page != null)
        {
            navigationStack.Push(page);
      
            _isOpen = true;
            page.menu = this;
            page.transform.SetParent(pageParent, false);
            page.gameObject.SetActive(false);
            page.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
   
    /// <summary>
    /// Navigate up in the navigation stack
    /// </summary>
    private void navigateUp()
    {
        if (navigationStack.Count > 0)
        {
            UIQuickMenuPage page = navigationStack.Pop();
            Destroy(page.gameObject);
        }

        if (navigationStack.Count == 0)
        {
            Close();
        }
    }

    /// <summary>
    /// Destroys all children and clears the navigation stack.
    /// </summary>
    private void clear()
    {
        //foreach (Transform child in pageParent)
        //{
        //    Destroy(child.gameObject);
        //}
        foreach (UIQuickMenuPage page in navigationStack)
        {
            Destroy(page.gameObject);
        }

        navigationStack.Clear();
    }


    public bool isOpen
    {
        get { return _isOpen; }
    }
}
