using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSpot : MonoBehaviour
{
    public GameObject charge;

    [SerializeField] private List<Material> _chargesParticles;
    [SerializeField] private List<Color> _colorChargesParticles;
    [SerializeField] private bool _isOnlySpot;
    
    private int _chargeValue;
    //private bool _insideSpot;

    private Collider2D _collider;

    private string _tagPlayerName = "Player";

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        _chargeValue = Random.Range(0, _chargesParticles.Count);

        charge.GetComponent<ParticleSystemRenderer>().material = _chargesParticles[_chargeValue];

        ParticleSystem.MainModule settings = charge.GetComponent<ParticleSystem>().main;
        settings.startColor = _colorChargesParticles[_chargeValue];
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag(_tagPlayerName)) {
            if (_isOnlySpot) {
                KalawasaController.SetChargeValue(_chargeValue);
            } else {
                if (_chargeValue == -1) {
                    // sube barra de oxigeno
                    KalawasaController.SetChargeValue(_chargeValue);
                } else {
                    int currentChargeValue = KalawasaController.GetChargeValue();
                    if (_chargeValue == currentChargeValue) {
                        // repele
                    } else if (_chargeValue + currentChargeValue == 1) {
                        // atrae y cuando collisione con roca cambia estado de spot y carga de kalawasa
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_tagPlayerName)) {
            if (!_isOnlySpot) {
                if (_chargeValue == -1) {
                    // deja de subir barra de oxigeno
                }
            }
        }
    }

    public void ChargeToNeutro()
    {
        _chargeValue = -1;
        // rock mas oscura y ya no se muestra particulas
    }
}
