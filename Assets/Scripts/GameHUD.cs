using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;


public class GameHUD : MonoBehaviour
{

    public Slider EnergyMeter;
    private GameManagerService _linkGMService;


    // Start is called before the first frame update
    void Start()
    {
        _linkGMService = ServiceLocator.Instance.Get<GameManagerService>();

        EnergyMeter.maxValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        EnergyMeter.value = _linkGMService.GetPlayerEnergy();

        if (EnergyMeter.maxValue < 5)
        {
            EnergyMeter.maxValue = _linkGMService.GetMaxEnergy();
        }
    }
}
