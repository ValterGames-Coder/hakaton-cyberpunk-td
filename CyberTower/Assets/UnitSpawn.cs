using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawn : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;
    [SerializeField] private Button _unitButton;
    [SerializeField] private Transform _zoneSpawn;
    private List<KeyCode> _codes = new() {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3};
    private RectTransform _panel;
    private bool _spawn;
    private int _selectUnit;
    private Camera _camera;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _camera = Camera.main;
        _panel = GetComponent<RectTransform>();
        for (int i = 0; i < PlayerPrefs.GetInt("Level")+2; i++)
        {
            print(i);
            Button button = Instantiate(_unitButton, _panel);
            button.transform.GetChild(0).GetComponent<TMP_Text>().text = _units[i].name;
            button.transform.GetChild(1).GetComponent<TMP_Text>().text = (i+1).ToString();
            button.onClick.AddListener(() => Spawn(i));
        }
    }

    private void Update()
    {
        foreach (KeyCode keyCode in _codes)
        {
            if(Input.GetKeyDown(keyCode) && _codes.IndexOf(keyCode) < PlayerPrefs.GetInt("Level")+2)
                Spawn(_codes.IndexOf(keyCode));
        }
        if (_spawn)
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _zoneSpawn.position = new Vector2(mousePosition.x, _zoneSpawn.position.y);
            if (Input.GetMouseButtonDown(0))
            {
                _zoneSpawn.gameObject.SetActive(false);
                
                if (_gameManager.souls >= _units[_selectUnit].cost)
                {
                    _gameManager.SetSouls(_units[_selectUnit].cost);
                    Vector2 scale = new Vector2();
                    scale = _zoneSpawn.position.x > 0 ? new Vector2(1, 1) : new Vector2(-1, 1);
                    Unit unit = Instantiate(_units[_selectUnit],
                        new Vector2(_zoneSpawn.position.x, _zoneSpawn.position.y + .5f), Quaternion.identity);
                    unit.transform.localScale = scale;
                    _gameManager.waveUnits.Add(unit.gameObject);
                }

                _spawn = false;
            }
        }

        if (_gameManager.isWave)
        {
            _zoneSpawn.gameObject.SetActive(false);
        }
    }

    private void Spawn(int unitId)
    {
        _spawn = true;
        _selectUnit = unitId;
        _zoneSpawn.gameObject.SetActive(true);
    }
}