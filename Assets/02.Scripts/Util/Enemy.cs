using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Renderer _renderer;
    private Color _originalColor;
    private Animator _animator;

    [SerializeField] private ParticleSystem _hitEffect; // ������ �ִ� ��ƼŬ �ý���

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _originalColor = _renderer.material.color;
        _animator = GetComponent<Animator>();   

        // ������ �ִ� HitEffect �ڵ����� ã�Ƶ� �ǰ� �ν����Ϳ��� �����ص� ��
        if (_hitEffect == null)
            _hitEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            Debug.Log("Enemy Hit");
            _animator.SetTrigger("Hurt");
            // ��ƼŬ ���
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
