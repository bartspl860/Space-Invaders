using System;
using System.Collections;
using Controllers;
using Effects;
using TMPro;
using UnityEngine;

namespace Enemies
{
    public class BossTurret : MonoBehaviour
    {
        private BulletController _bulletController;

        [SerializeField] 
        private float _shootingDelay;

        [SerializeField] 
        private Transform _shootingPoint;
        
        private GameObject _playerGameObject;

        [SerializeField] 
        private int _health;

        [SerializeField] 
        private Hit _hitEffect;

        [SerializeField] 
        private TextMeshPro _hpText;
        
        private Coroutine _shootingCoroutine;

        private void Start()
        {
            _shootingCoroutine = StartCoroutine(Shoot());
            _bulletController = FindFirstObjectByType<BulletController>();
            _playerGameObject = GameObject.FindGameObjectWithTag("Player");
            _hpText.text = $"HP \n {_health}";
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                 TakeDamage();
            }
        }

        private void TakeDamage()
        {
            _health--;
            if (_health <= 0)
            {
                StopCoroutine(_shootingCoroutine);
                _hitEffect.MarkAsDestroyed();
                return;
            }
            _hitEffect.StartEffect();
            _hpText.text = $"HP \n {_health}";
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                yield return new WaitForSeconds(_shootingDelay);
                var rotationEuler = transform.rotation.eulerAngles;
                rotationEuler.z += 180f;
                _bulletController.GenerateEnemyBullet(_shootingPoint.position, Quaternion.Euler(rotationEuler));
            }
        }
        
        private void Rotate()
        {
            var target = _playerGameObject.transform.position;
            Vector3 turretPos = transform.position;
            Vector3 distance = Vector2.zero;
            distance.x = target.x - turretPos.x;
            distance.y = target.y - turretPos.y;

            float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void Update()
        {
            if(_health > 0)
                Rotate();
        }
    }
}