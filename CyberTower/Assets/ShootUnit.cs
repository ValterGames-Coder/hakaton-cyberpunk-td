using System.Collections;
using UnityEngine;

public class ShootUnit : Unit
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _missRot;
    public override IEnumerator CoolDownCoroutine(Health hit)
    {
        _canAttack = false;
        Vector3 direction = _tower.position - transform.position;
        var rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.Euler(0, 0, rot + Random.Range(-_missRot, _missRot));
        Bullet bullet = Instantiate(_bulletPrefab, transform.position, rotation);
        bullet.Init(_damage, _bulletSpeed);
        yield return new WaitForSeconds(_cooldown);
        _canAttack = true;
        _gameManager.AddSouls(farm);
    }
}
