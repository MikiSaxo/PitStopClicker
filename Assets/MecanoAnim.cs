using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using DG.Tweening;

public class MecanoAnim : MonoBehaviour
{
    [field:SerializeField] public UpgradeType MyType { get; private set; }
    
    [SerializeField] private Transform _transformA;
    [SerializeField] private Transform _transformB;
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private bool _isOneTimePurchased;
   

    private Collider _collider;
    private MeshRenderer _meshRenderer;
    private Vector3 _startPos;
    private Quaternion _startRot;
    private float _startRotY;
    private bool _canRepair;
    
    private float _speed => UpgradeManager.Instance.CurrentMecanoSpeed[(int)MyType];
    private float _power => UpgradeManager.Instance.CurrentMecanoPower[(int)MyType];

    private void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
        _startRotY = transform.rotation.eulerAngles.y;
        
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void UpdateMeshRenderer(Mesh mesh, Material material)
    {
        if(_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
        
        GetComponent<MeshFilter>().sharedMesh = mesh;
        _meshRenderer.material = material;
    }

    private IEnumerator AnimateMovement()
    {
        _collider.enabled = false;
        
        yield return DOTween.Sequence()
            .Append(transform.DOMove(_transformA.position, _moveDuration))
            .Join(transform.DORotate(_transformA.rotation.eulerAngles, _moveDuration))
            .WaitForCompletion();

        while (_canRepair)
        {
            yield return DOTween.Sequence()
                .Append(transform.DOMove(_transformB.position, _speed))
                .Join(transform.DORotate(_transformB.rotation.eulerAngles, _speed))
                .Append(transform.DOMove(_transformA.position, _speed))
                .Join(transform.DORotate(_transformA.rotation.eulerAngles, _speed))
                .WaitForCompletion();
        }
    }

    public void StopAnim()
    {
        transform.DOKill();
        _canRepair = false;
        StopCoroutine(AnimateMovement());
        transform.DOKill();
        
        transform.DOMove(_startPos, _moveDuration).SetEase(Ease.InOutQuad).OnComplete(() => _collider.enabled = true);
        transform.DORotate(_startRot.eulerAngles, _moveDuration).SetEase(Ease.InOutQuad);
    }
    public void LaunchAnim()
    {
        if(ClickCarJack.Instance.IsSet == false) return;
        
        if (_isOneTimePurchased)
        {
            if(MyType == UpgradeType.AutoGas && GasCan.Instance.IsSet
               || MyType == UpgradeType.AutoCarJack)
                RotateAnim();
        }
        else
        {
            foreach (var ClickObj in CarSpawner.Instance.ClickObjectsList)
            {
                if (ClickObj.MyType == MyType)
                {
                    _canRepair = true;
                    StartCoroutine(AnimateMovement());
                    return;
                }
            }
            StopAnim();
        }
    }
    
    private void RotateAnim()
    {
        transform.DORotate(new Vector3(0, _startRotY + 360, 0), _moveDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad).OnComplete(() => transform.DORotate(new Vector3(0, _startRotY, 0), 0.1f));
    }
}