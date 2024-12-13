using Assets.Scripts;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyArray;

    public int numberOfEnemies;
    private int currentEnemies;

    public float spawnTime;

    public string nextSection;


    void Update()
    {
        // Caso atinja o n�mero m�ximo de inimigos spawnados
        if (currentEnemies >= numberOfEnemies)
        {
            // Contar a quantidade de inimigos ativos na cena
            int enemies = FindObjectsByType<EnemyMeleeController>(FindObjectsSortMode.None).Length;

            if (enemies <= 0)
            {
                // Avan�a de se��o
                LevelManager.ChangeSection(nextSection);

                // Desabilitar o spawner 
                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemy()
    {
        // Posi��o de Spawn do inimigo
        Vector2 spawnPosition;

        // Limites Y
        //-0,36 e -1
        spawnPosition.y = Random.Range(-0.95f, -0.36f);

        // Posi��o X m�ximo (direita) do confiner da c�mera + 1 de dist�ncia
        // Pegar RightBound (limite direito) da Section (Confiner) como base 
        float rightSectionBound = LevelManager.currentConfiner.BoundingShape2D.bounds.max.x;

        // Define o x do spawnPosition, igual ao ponto da Direita do confiner
        spawnPosition.x = rightSectionBound;

        // Instacia (Spawna) os inimigos
        // Pega um inimigo aleat�rio da lista de inimigos
        // Spawna na posi��o spawnPosition
        // Quaternion � uma classe utilizada para trabalhar com rota��es
        Instantiate(enemyArray[Random.Range(0, enemyArray.Length)], spawnPosition, Quaternion.identity).SetActive(true);

        // Incrementa o contador de inimigos do Spawner
        currentEnemies++;

        // Se o n�mero de inimigos atualmente na cena for menor que o numero maximo de inimigos,
        // Invoca novamente a fun��o de spawn
        if (currentEnemies < numberOfEnemies)
        {
            // Spawna os inimigos a cada spawnTime
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        // Desativa o colisor para iniciar o Spawning apenas uma vez
        // ATEN��O: Desabilita o collider, mas o objeto Spawner continua ativo
        if (player)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;

            // Invoca pela primeira vez a fun��o SpawnEnemy
            SpawnEnemy();
        }
    }

}
