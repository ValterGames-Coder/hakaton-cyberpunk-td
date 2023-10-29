using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage;
    private float _speed;
    public bool IsTower;
    

    void Update()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime);
    }

    public void Init(float damage, float speed)
    {
        _damage = damage;
        _speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsTower && collider.CompareTag("Unit"))
        {
            collider.gameObject.GetComponent<Health>().TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if (IsTower == false && collider.CompareTag("Tower"))
        {
            collider.gameObject.GetComponent<Health>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
