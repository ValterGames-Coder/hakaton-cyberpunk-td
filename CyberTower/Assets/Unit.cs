using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _defaultSpeed;
    [SerializeField] protected int _damage;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _radius;
    [SerializeField] protected float _cooldown;
    public int farm;
    public int cost;
    
    private Rigidbody2D _rb;
    private Health _health;
    protected GameManager _gameManager;
    protected Transform _tower;
    private Coroutine _attackCoroutine;
    protected bool _canAttack = true;
    private bool _nearTower;
    public bool isDead;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
        _health.OnDied += Died;
        _defaultSpeed = _speed;
        _tower = GameObject.FindWithTag("Tower").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Move();
        Attack();
    }

    private void Died() => StartCoroutine(Timer());
    

    private IEnumerator Timer()
    {
        _speed = 0;
        isDead = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(2f);
        _gameManager.AddSouls(farm / 2);
        _rb.gravityScale = 1;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        _gameManager.waveUnits.Remove(gameObject);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public virtual void Move()
    {
        //rb.velocity = new Vector2(tower.position.x + speed * Time.fixedDeltaTime, rb.velocity.y);
        //rb.MovePosition(tower.position.x + speed * Time.fixedDeltaTime);
        if(_gameManager.isWave)
            transform.position = Vector2.MoveTowards(transform.position,
            _tower.position, _speed * Time.deltaTime);
    }

    public virtual void Attack()
    {
        Collider2D tower = Physics2D.OverlapCircle(transform.position, _radius, _layer);
        if (tower != null)
        {
            _nearTower = true;
            _speed = 0;
            if (_canAttack && _nearTower)
                _attackCoroutine = StartCoroutine(CoolDownCoroutine(tower.GetComponent<Health>()));
        }
    }

    public virtual IEnumerator CoolDownCoroutine(Health hit)
    {
        _canAttack = false;
        hit.TakeDamage(_damage);
        _gameManager.AddSouls(farm);
        yield return new WaitForSeconds(_cooldown);
        _canAttack = true;
        StopCoroutine(_attackCoroutine);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
