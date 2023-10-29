using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] private float _bulletDamage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Area _area;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private List<Transform> _turrets;
    [SerializeField] private float _missRot;
    
    private Coroutine _attackCoroutine;
    private GameManager _gameManager;
    private bool _canAttack = true;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        Cast();
    }

    private void Shoot(Transform unit)
    {
        foreach (Transform turret in _turrets)
        {
            Vector3 direction = unit.position - turret.position;
            var rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.Euler(0, 0, rot + Random.Range(-_missRot, _missRot));
            turret.rotation = rotation;
            Bullet bullet = Instantiate(_bulletPrefab, turret.position, turret.rotation);
            bullet.Init(_bulletDamage, _bulletSpeed);
        }
}

    private void Cast()
    {
        if (_area.objects.Count > 0 && _canAttack && _gameManager.isWave)
            _attackCoroutine = StartCoroutine(CoolDownCoroutine(_area.objects));
    }

    IEnumerator CoolDownCoroutine(List<Transform> area)
    {
        _canAttack = false;
        Shoot(FindClosestEnemy(area));
        yield return new WaitForSeconds(_cooldown);
        _canAttack = true;
        StopCoroutine(_attackCoroutine);
    }

    private Transform FindClosestEnemy(List<Transform> list)
    {
        Transform closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (Transform go in list)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
