using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public RectTransform healthBar;

    public void TakeDamage(float dmg)
    {
        if ((health -= dmg) <= 0f)
        {
            Debug.Log("Dead");
            SceneManager.LoadScene(3);
        }

        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }
}
