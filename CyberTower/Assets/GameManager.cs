using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int level;
    public int souls;
    public int minSouls;
    public int wave;
    public bool isWave;
    [SerializeField] private TMP_Text _soulsText, _waveText;
    [SerializeField] private GameObject _unitPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private List<GameObject> _towers;
    public List<GameObject> waveUnits;

    private void Start()
    {
        level = PlayerPrefs.GetInt("Level");
        _towers[level].SetActive(true);
        _soulsText.text = souls.ToString();
        _waveText.text = $"Волна: {wave+1}";
    }

    public void Finish()
    {
        PlayerPrefs.SetInt("Level", level+1);
        foreach (var unit in waveUnits)
        {
            Destroy(unit);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isWave == false)
        {
            isWave = true;
            _unitPanel.SetActive(false);
        }

        if (waveUnits.Count == 0 && isWave)
        {
            if (minSouls < souls)
            {
                isWave = false;
                _unitPanel.SetActive(true);
                wave++;
                _waveText.text = $"Волна: {wave + 1}";
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            foreach (var hit2D in hit)
            {
                if (hit2D.collider != null)
                {
                    if (hit2D.transform.TryGetComponent(out Unit unit))
                    {
                        if (unit.isDead)
                        {
                            waveUnits.Remove(hit2D.collider.gameObject);
                            AddSouls(unit.farm);
                            Destroy(unit.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void SetSouls(int souls)
    {
        this.souls -= souls;
        _soulsText.text = this.souls.ToString();
    }
    
    public void AddSouls(int souls)
    {
        this.souls += souls;
        _soulsText.text = this.souls.ToString();
    }
    
}
