using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ao colidir, salva na variavel enemy, o inimigo que foi colidido
        EnemyMeleeController enemy = collision.GetComponent<EnemyMeleeController>();

        // Ao colidir, salva na ariavel player, o player que foi atingido
        PlayerController player = collision.GetComponent<PlayerController>();

        // Se a colisão foi com um inimigo
        if (enemy != null)
        {
            // Inimigo recebe dano
            enemy.TakeDamage(damage);
        }

        // Se a colisão foi com o player
        if (player != null)
        {
            // Player recebe dano
            player.TakeDamage(damage);
        }
    }
}
