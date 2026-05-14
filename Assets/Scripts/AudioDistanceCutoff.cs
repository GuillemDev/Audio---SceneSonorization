using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDistanceCutoff : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private Transform _listener;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _listener.position);
        _source.mute = distance > _source.maxDistance;
    }
}