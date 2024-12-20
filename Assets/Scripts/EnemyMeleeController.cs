using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // VAriavel que indica se o inimigo esta vivo
    public bool isDead;

    // Variaveis para controlar o lado que o inimigo esta virado
    private bool facingRight;
    public bool previousDirectionRight;

    // Variavel para armazenar posi��o do Player
    // target - alvo
    private Transform target;

    // Variaveis para movimenta��o do inimigo
    private float enemySpeed = 0.3f;
    private float currentSpeed;
    private bool isWalking;
    private float horizontalForce;
    private float verticalForce;

    // Variavel que vamos usar para controlar o intervalo de tempo 
    private float walkTimer;

    // Variaveis para mec�nica de ataque
    private float attackRate = 1f;
    private float nextAttack;

    // Variaveis para mec�nica de dano
    public int maxHealth;
    public int currentHealth;
    public Sprite enemyImage;

    public float staggerTime = 0.5f;
    private float damageTimer;
    private bool isTakingDamage;



    void Start()
    {
        // Inicializa os componentes do Rigibody e Animator
        rb = GetComponent<Rigidbody2D>();   
        animator = GetComponent<Animator>();

        // Buscar o Player e armazenar sua posi��o
        target = FindAnyObjectByType<PlayerController>().transform;

        // Inicializar a velocidade do inimigo
        currentSpeed = enemySpeed;

        // Inicializar a vida do inimigo
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Verificar se o Player esta para a Direita ou para a Esquerda
        // E determinar o lado que o inimigo ficara virado
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true; 
        }

        // Se facingRight for TRUE, vamos virar o inimigo em 180 graus no eixo Y,
        // Sen�o vamos virar o inimigo para a esquerda
        // Se o Player est� � direita e a posi��o anterior N�O era direita (estava olhando para a esquerda)
        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        // Se o player n�o est� a direita e a posi��o anterior ERA direita
        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        // Iniciar o timer do caminhar do inimigo, retorna o tempo atual
        walkTimer += Time.deltaTime;

        // Gerenciar  a anima��o do inimigo
        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            
            isWalking = true;
            
        }

        // Gerenciar o tempo de stagger 
        if (isTakingDamage && !isDead)
        {
            damageTimer += Time.deltaTime;

            zeroSpeed();

            if (damageTimer >= staggerTime)
            {
                isTakingDamage = false;
                damageTimer = 0;
                ResetSpeed();
            }
        }

        // Atualiza o animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            // MOVIMENTA��O

            // Variavel para armazenar a dist�ncia entre o Inimigo e o Player
            Vector3 targetDistance = target.position - this.transform.position;

            // Determina se a for�a horizontal deve ser negativa ou positiva
            // 5 / 5 = 1
            // -5 / 5 = -1
            horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            // Entre 1 e 2 segundos, ser� feita uma defini��o vetical
            if (walkTimer >= Random.Range(1f, 2f))
            {
                verticalForce = Random.Range(-1, 2);

                // Zera o timr de movimenta��o para andar verticalmente novamente daqui a +- 1 segundo
                walkTimer = 0;
            }

            // Caso estaeja perto do Player, parar  a movimenta��o
            if (Mathf.Abs(targetDistance.x) < 0.2f)
            {
                horizontalForce = 0;
            }

            // Aplica a velocidade no inimigo fazendo o movimentar
            rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);

            // ATAQUE
            // Se estiver perto do Player e o timer do jogo for maior que o valor de nextAttack 
            if (Mathf.Abs(targetDistance.x) < 0.2f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
            {
                // Esse comando executa a naima��o de ataque do inimigo
                animator.SetTrigger("Attack");

                // Ap�s executar a a��o, zera a velocidade do inimigo, portanto, zeroSpeed
                zeroSpeed();

                // Pega o tempo atual e soma o attackRate, para definir a partir de quando o inimigo poder� atacar novamente
                nextAttack = Time.time + attackRate;
            }
        }   
    }

    void UpdateAnimator()
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            isTakingDamage = true;

            currentHealth -= damage;

            animator.SetTrigger("hitDamage");

            // Atualiza a UI do inimigo
            FindFirstObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

            if (currentHealth <= 0)
            {
                isDead = true;

                zeroSpeed();

                animator.SetTrigger("Dead");
            }
        }
    }

    void zeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = enemySpeed;
    }

    public void DisableEnemy()
    {
        this.gameObject.SetActive(false);   
    }
}
