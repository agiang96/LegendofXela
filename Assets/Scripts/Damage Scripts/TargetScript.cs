using UnityEngine;

public class TargetScript : MonoBehaviour {

    public float health = 100f;
    public RectTransform healthBar;

    public void TakeDamage(float dmg)
    {
        if((health-=dmg) <=0f)
        {
            //this line gives a NullReferenceException: Obj ref not set
            //to an instance of an object....
            //GetComponent<KillCounter>().UpdateKills();

            ScoreScript.sm._currentscore++;
            Destroy(gameObject);
        }

        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
        gameObject.GetComponent<Flock>().setStunned(true);

    }
}
