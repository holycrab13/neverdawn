using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    Exploration,
    Combat,
    Event,
    Cinematic,
    Loading
}

public class GameController : MonoBehaviour {

    /// <summary>
    /// Global instance for access from anywhere!
    /// </summary>
    public static GameController instance { get; private set; }

    private static string playerFrameId = "player_frame";

    public static GameState state
    {
        get { return instance.gameState; }
    }

    public GameState gameState
    {
        get { return _gameState; }
    }

    public CombatController combatController
    {
        get { return _combatController; }
    }

    [SerializeField]
    private GameState _gameState;

    [SerializeField]
    private bool _isPaused;

    [SerializeField]
    private ExplorationController _explorationController;

    [SerializeField]
    private CombatController _combatController;

    [SerializeField]
    private EventController _eventController;

    [SerializeField]
    private UIPage _generalGuiPage;

    [SerializeField]
    private UIPage _pauseGuiPage;

    [SerializeField]
    private UIPage _eventUIPage;

    [SerializeField]
    private UIPage _mainMenuUIPage;

    [SerializeField]
    private UILoadingScreen _loadingScreen;

    private bool _sceneChangeRequested;

    private SerializableGame _currentGame;

    private PlayerTime _time;

    private Frame _playerFrame;

    private PlayerInventory _inventory;

    private PlayerParty _party;

    public static Vector3 playerSpawnPosition { get; set; }
   

	// Use this for initialization
	void Awake () {

        // Never allow more than one game controller
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        // Hook into the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void PauseGame()
    {
        _pauseGuiPage.Show();
        _isPaused = true;
    }

    /// <summary>
    /// Unpauses the game
    /// </summary>
    public void UnPauseGame()
    {
        _generalGuiPage.Show();
        _isPaused = false;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_currentGame != null)
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            Frame.FlushMemory();

            NeverdawnScene neverdawnScene = FindObjectOfType<NeverdawnScene>();

            if (neverdawnScene != null)
            {
                Frame.DeserializeFrames(_currentGame, buildIndex);

                // cache the most important frames
                _playerFrame = Frame.FindFrameById(playerFrameId);
                _inventory = _playerFrame.GetComponent<PlayerInventory>();
                _party = _playerFrame.GetComponent<PlayerParty>();
                _time = _playerFrame.GetComponent<PlayerTime>();

                UICharacterMenus.UpdateView();

                _inventory.Initialize(_currentGame);
                _explorationController.Initialize(_currentGame);
                _eventController.Initialize();

                _time.Run();

                _gameState = GameState.Exploration;
                _isPaused = false;

            }
        }
    }

    public void StartNewGame(int playerCount)
    {
        _loadingScreen.Show(1.0f);

        SerializableGame newGameData = SerializableGame.NewGame(playerCount);

        StartCoroutine(loadGame(newGameData, 1.0f));
    }

    public void LoadGame()
    {
        SerializableGame data;

        if (LoadSaveUtils.Load("Test", out data))
        {
            _loadingScreen.Show(1.0f);
            _loadingScreen.SetProgress(0.0f);

            StartCoroutine(loadGame(data, 1.0f));
        }
    }

    public static void DestroyGame()
    {
        PlayerInventory.Clear();
        NeverdawnCamera.Destroy();
        EventController.ClearEvents();

        foreach(AvatarController controller in activeControllers)
        {
            Destroy(controller.gameObject);
        }

        activeControllers.Clear();
    }

   

	// Update is called once per frame
	void Update () {

        if (!_isPaused)
        {
            switch (_gameState)
            {
                case GameState.Exploration:
                    _explorationController.UpdateExplorationController(Time.deltaTime);
                    break;
                case GameState.Combat:
                    _combatController.UpdateCombat();
                    break;
                case GameState.Event:
                    _eventController.UpdateEvent();
                    break;
            }
        }
	}

    internal void StartCombat(Character attacker)
    {
        UIAnnouncement.Announce("Fight!", 2.0f);
        NeverdawnCamera.ShowGrid(2.0f);

        _gameState = GameState.Combat;
        _combatController.PrepareCombat(_explorationController.controllers, attacker);
    }

    public void ExitCombat()
    {
        NeverdawnCamera.HideGrid(1.0f);

        _combatController.ExitCombat();
        _gameState = GameState.Exploration;
    }

    public void ExitEventMode()
    {
        _generalGuiPage.Show();
        _gameState = GameState.Exploration;
    }

    internal void JoinCombat(Character neverdawnCharacter)
    {
        if(state == GameState.Combat)
        {
            instance._combatController.AddEnemy(neverdawnCharacter);
        }
        else
        {
            StartCombat(neverdawnCharacter);
        }
    }

    internal static Character[] GetEnemies()
    {
        if (state == GameState.Combat)
        {
            return instance._combatController.enemies.ToArray();
        }
        else
        {
            return new Character[0];
        }
    }

    internal static Character[] GetEnemiesInRange(Vector3 vector3, float p)
    {
        return instance._combatController.GetEnemiesInRange(vector3, p);
    }

    internal static Character[] GetEnemiesInRange(HexTile hexTile, int maxRange)
    {
        return instance._combatController.GetEnemiesInRange(hexTile, maxRange);
    }

    public void TriggerEvent(string trigger)
    {
        if (gameState == GameState.Exploration)
        {
            _gameState = GameState.Event;
            _eventUIPage.Show();
        }

        _eventController.TriggerEvent(trigger);
    }

    public static List<AvatarController> activeControllers
    {
        get
        {
            if (instance != null && instance._explorationController != null)
            {
                return instance._explorationController.controllers;
            }

            return new List<AvatarController>();
        }
    }

    public void ChangeScene(int sceneBuildIndex, int spawnIndex)
    {
        if (gameState == GameState.Exploration && !_sceneChangeRequested)
        {
            _sceneChangeRequested = true;
            _loadingScreen.Show(1.0f);
            _loadingScreen.SetProgress(0.0f);

            // clear level specific events
            EventController.ClearEvents();

            // Serialize all frames of the scene..
            serializeFrames();

            

            StartCoroutine(changeScene(sceneBuildIndex, spawnIndex, 1.0f));
        }
    }


    private void serializeFrames()
    {
        Frame[] frames = FindObjectsOfType<Frame>();

        foreach(Frame frame in frames)
        {
            frame.SerializeTo(currentGame);
        }
    }


    internal void Save(string saveName)
    {
        AvatarController controller = _explorationController.controllers.FirstOrDefault(a => a.character != null);

        serializeFrames();
        currentGame.currentPosition = SerializableVector3.FromVector3(controller.character.position);
        currentGame.currentRotation = SerializableVector3.FromVector3(controller.character.eulerAngles);
        currentGame.currentScene = SceneManager.GetActiveScene().buildIndex;

        LoadSaveUtils.Save(currentGame, saveName);

        if (isPaused)
        {
            UnPauseGame();
        }
    }

    internal void GoToMainMenu()
    {
        _pauseGuiPage.Hide();
        _loadingScreen.Show(1.0f);

        StartCoroutine(goToMainMenu(1.0f));
    }

    private IEnumerator goToMainMenu(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        DestroyGame();

        AsyncOperation async = SceneManager.LoadSceneAsync(0);

        while (!async.isDone)
        {
            _loadingScreen.SetProgress(Mathf.Clamp01(async.progress / 0.9f));
            yield return null;
        }

        _loadingScreen.Hide(1.0f);
        _mainMenuUIPage.Show();
    }

    private IEnumerator changeScene(int sceneBuildIndex, int spawnIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        _gameState = GameState.Loading;

        transferPlayerFrames(Frame.FindFrameById("player_frame"), sceneBuildIndex);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneBuildIndex);

        while (!async.isDone)
        {
            _loadingScreen.SetProgress(Mathf.Clamp01(async.progress / 0.9f));
            yield return null;
        }

        _loadingScreen.Hide(1.0f);

        NeverdawnSpawn point = NeverdawnSpawn.FindSpawnByIndex(spawnIndex);
        _party.SetCharacterPositions(point != null ? point.position : Vector3.zero, 1.0f);

        yield return new WaitForSeconds(1.0f);

        _sceneChangeRequested = false;
        _gameState = GameState.Exploration;
    }

    private void transferPlayerFrames(Frame playerFrame, int sceneBuildIndex)
    {
        foreach (Frame frame in playerFrame.GetComponentsInChildren<Frame>())
        {
            SerializedFrame serializedFrame = currentGame.GetFrameById(frame.id);

            if (serializedFrame != null)
            {
                serializedFrame.scene = sceneBuildIndex;
            }
        }
    }

    private IEnumerator loadGame(SerializableGame data, float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);

        _currentGame = data;
        AsyncOperation async = SceneManager.LoadSceneAsync(data.currentScene);

        while (!async.isDone)
        {
            _loadingScreen.SetProgress(Mathf.Clamp01(async.progress / 0.9f));
            yield return null;
        }

        _generalGuiPage.Show();
        _loadingScreen.Hide(1.0f);
        NeverdawnCamera.HideGrid();

        yield return new WaitForSeconds(1.0f);
    }

    public static string dateString
    {
        get { return "Heute"; }
    }

    public SerializableGame currentGame
    {
        get { return _currentGame; }
    }

    public bool isPaused
    {
        get { return _isPaused; }
    }

    public PlayerInventory inventory
    {
        get { return _inventory; }
    } 
    
    public PlayerParty party
    {
        get { return _party; }
    }

    public IEnumerable<Character> enemies { get; set; }
}
