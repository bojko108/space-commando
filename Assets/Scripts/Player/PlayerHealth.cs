using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Player health when the game starts")]
    public int StartingHealth = 200;
    [Tooltip("Current health")]
    public int CurrentHealth;
    [Tooltip("Player oxygen when the game starts")]
    public int StartingOxygen = 100;
    [Tooltip("Current oxygen")]
    public int CurrentOxygen;
    [Tooltip("Reference to health slider - will display current health level")]
    public Slider HealthSlider;
    [Tooltip("Reference to oxygen slider - will display current oxygen level")]
    public Slider OxygenSlider;
    public Image DamageImage;
    [HideInInspector]
    public bool HaveOxygen = false;
    
    private Color flashColor = new Color(1f, 0f, 0f, 0.6f);
    private float flashSpeed = 3f;

    // is the player damaged
    private bool isDamaged = false;
    // is the player dead
    private bool isDead = false;

    // reference to the FPS controller
    private FirstPersonController fpsController;

    private void Awake()
    {
        // turn on the mouse smooth
        this.fpsController = GetComponent<FirstPersonController>();
        this.fpsController.MouseLook.smooth = true;

        this.SetLevels(this.StartingHealth, this.StartingOxygen);
    }

    private void Start()
    {
        if (this.HaveOxygen == false)
        {
            // update oxygen level every second
            StartCoroutine(this.AbjustOxygen());
        }
    }

    private void Update()
    {
        // if the player is damaged
        if (this.isDamaged)
        {
            this.DamageImage.color = this.flashColor;
        }
        else
        {
            this.DamageImage.color = Color.Lerp(this.DamageImage.color, Color.clear, this.flashSpeed * Time.deltaTime);
        }

        this.isDamaged = false;
    }

    /// <summary>
    /// used when loading saved game
    /// </summary>
    /// <param name="health"></param>
    /// <param name="oxygen"></param>
    public void SetLevels(int health, int oxygen)
    {
        this.CurrentHealth = health;
        this.CurrentOxygen = oxygen;

        this.HealthSlider.maxValue = this.StartingHealth;
        this.HealthSlider.minValue = 0;
        this.HealthSlider.value = this.CurrentHealth;

        this.OxygenSlider.maxValue = this.StartingOxygen;
        this.OxygenSlider.minValue = 0;
        this.OxygenSlider.value = this.CurrentOxygen;
    }

    /// <summary>
    /// Called when the player has oxygen
    /// </summary>
    public void PlayerHaveOxygen()
    {
        // turn off mouse smooth
        this.fpsController.MouseLook.smooth = false;

        this.CurrentOxygen = this.StartingOxygen;
        this.OxygenSlider.value = this.CurrentOxygen;

        this.HaveOxygen = true;

        StartCoroutine(this.AnimateHealPlayer(this.OxygenSlider));
    }

    /// <summary>
    /// Updates player's oxygen level. 
    /// 
    /// The oxygen level will drop every second until the player has control over the main control room.
    /// </summary>
    private IEnumerator AbjustOxygen()
    {
        while (this.HaveOxygen == false)
        {
            if (this.CurrentOxygen > 1)
            {
                this.CurrentOxygen -= 1;
                this.OxygenSlider.value = this.CurrentOxygen;
            }
            // if the player has no oxygen then apply damage
            else
            {
                this.TakeDamage(5);
            }

            float smoothTime = this.CurrentOxygen / 10;
            this.fpsController.MouseLook.smoothTime = smoothTime >= 1f ? smoothTime : 1f;

            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Heals the player. Health level is restored when the player collide with a medpack
    /// </summary>
    public void HealPlayer()
    {
        this.CurrentHealth = this.StartingHealth;
        this.HealthSlider.value = this.CurrentHealth;

        StartCoroutine(this.AnimateHealPlayer(this.HealthSlider));
    }

    private IEnumerator AnimateHealPlayer(Slider slider)
    {        
        slider.transform.localScale *= 1.5f;
        yield return new WaitForSeconds(0.7f);
        slider.transform.localScale /= 1.5f;
    }

    /// <summary>
    /// Take damage
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        this.isDamaged = true;
        this.CurrentHealth -= amount;
        this.HealthSlider.value = this.CurrentHealth;

        if (this.CurrentHealth <= 0 && this.isDead == false)
        {
            this.Dead();
        }
    }

    /// <summary>
    /// Will emit EventManager.emit("PlayerDead")
    /// </summary>
    private void Dead()
    {
        this.isDead = true;

        EventManager.Emit(Resources.Events.PlayerDead);

        Destroy(this.gameObject, 2f);
    }
}
