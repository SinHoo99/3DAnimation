using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Renderer _renderer;
    private Color _originalColor;
    private Animator _animator;

    [SerializeField] private ParticleSystem _hitEffect; // 하위에 있는 파티클 시스템

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _originalColor = _renderer.material.color;
        _animator = GetComponent<Animator>();   

        // 하위에 있는 HitEffect 자동으로 찾아도 되고 인스펙터에서 연결해도 됨
        if (_hitEffect == null)
            _hitEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            Debug.Log("Enemy Hit");
            _animator.SetTrigger("Hurt");
            // 파티클 재생
            if (_hitEffect != null)
            {
                _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _hitEffect.Play();
            }

            //StartCoroutine(FlashRed());
        }
    }
/*
    private System.Collections.IEnumerator FlashRed()
    {
        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _renderer.material.color = _originalColor;
    }*/
}
