using Services;
using UnityEngine;

/// <summary>
/// Sets up GameManagerService & acts as a go-between for FMOD audiosource.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- Private Variable Declarations --- //
    [SerializeField] private int peakBulletCount;

    GameManagerService _linkGMService;
    void Awake()
    {
        ServiceLocator.Instance.Register(new  GameManagerService());
        _linkGMService = ServiceLocator.Instance.Get<GameManagerService>();
        _linkGMService.Init();
    }

    private void Update()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Bullets", _linkGMService.getBulletDensity());

        // To be removed once we have a peak bullet count for level
        // then hardcode this value into the GMService
        peakBulletCount = _linkGMService.GetPeakBulletCount();
    }
}
