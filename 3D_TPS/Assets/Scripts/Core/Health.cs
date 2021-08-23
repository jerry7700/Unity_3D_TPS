using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    #region 欄位
    [Header("最大血量")]
    [SerializeField] private float maxHealth = 10f;
    [Header("當前血量")]
    [SerializeField] private float currentHealth;

    //當受到攻擊時觸發的委派事件
    public event Action onDamage;
    //當受到治癒效果時，觸發的委派事件，並且回傳float
    public event Action<float> onHealed;
    //當人物死亡時觸發的委派事件
    public event Action onDie;

    private bool isDead = false;
    #endregion

    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 取得目前血量
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// 取得最大血量
    /// </summary>
    /// <returns></returns>
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// 取得血量百分比
    /// </summary>
    /// <returns></returns>
    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }

    /// <summary>
    /// 是否死亡
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if(gameObject.tag == "Player")
        {
            print("玩家目前多少血量: " + currentHealth);
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if(currentHealth > 0)
        {
            onDamage?.Invoke();
        }

        if(currentHealth >= 0)
        {
            HandleDeath();
        }
    }

    /// <summary>
    /// 死亡動作
    /// </summary>
    private void HandleDeath()
    {
        if (isDead) return;

        if(currentHealth <= 0)
        {
            isDead = true;

            onDie?.Invoke();
        }
    }
}
