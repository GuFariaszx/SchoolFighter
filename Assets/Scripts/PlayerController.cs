using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;

    public float playerSpeed = 0.6f;
    public float currentSpeed;

    public Vector2 playerDirection;

    private bool isWalking;

    private Animator playerAnimator;

    // Player olhando para a direita
    private bool playerFacingRight = true;

    //Variavel contadora
    private int punchCount;

    //Tempo de ataque
    private float timeCross = 0.75f;

    private bool comboControl;

    // Inidicar se o Player esta morto
    private bool isDead;



    void Start()
    {
        //Obtem e inicializa as propriedades do RigiBody2D
        playerRigidBody = GetComponent<Rigidbody2D>();

        // Obtem e inicializa as propiedades do animator
        playerAnimator = GetComponent<Animator>();

        currentSpeed = playerSpeed;

    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMove();
        UpdateAnimator();

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Iniciar o temporizador
                if (punchCount < 2)
                {
                    PlayerJab();
                    punchCount++;
                    if (!comboControl)
                    {
                        
                        StartCoroutine(CrossController());
                    }
                }
                else if (punchCount >= 2)
                {
                    PlayerCross();
                    punchCount = 0;
                }           
        }

        // Parando o temporizador
        StopCoroutine(CrossController());
        
    }

    // Fixed Update geralmente é utilizada para implementação de física no jogo,
    // por ter uma execução padronizada em diferentes dispositivos
    private void FixedUpdate()
    {
        // Verificar se o Player está em movimento
        if (playerDirection.x != 0 || playerDirection.y != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        playerRigidBody.MovePosition(playerRigidBody.position + currentSpeed * Time.fixedDeltaTime * playerDirection);
    }

    void PlayerMove()
    {
        //Pega a entrada do jogador, cria um Vector2 para usar no playDirection
        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Se o player vai para a ESQUERDA e está olhando a DIREITA
        if (playerDirection.x < 0&&playerFacingRight)
        {
            Flip();
        }

        // Se o player vai para a DIREITA e está olhando para a ESQUERDA
        else if (playerDirection.x > 0 && !playerFacingRight)
        {
            Flip();
        }
    }

    void UpdateAnimator()
    {
        // Definir o valor do parâmetro do animator, igual à propiedade isWalking
        playerAnimator.SetBool("isWalking", isWalking);
    }

    void Flip()
    {
        // Vai girar o sprite do player em 180 graus no eixo Y

        // Inverter o valor da variável playerFacingRight
        playerFacingRight = !playerFacingRight;

        // Girar o sprite em 180 graus no eixo Y
        // X, Y, Z
        transform.Rotate(0, 180, 0);
    }

    //Jab Ataque
    void PlayerJab()
    {
        // Acessa a animação do Jab
        // Ativa o gatilho de ataque Jab 
        playerAnimator.SetTrigger("isJab");
    }

    // Cross Ataque
    void PlayerCross()
    {
        // Acessa a animação do Cross
        // Ativa o gatilho de ataque Cross
        playerAnimator.SetTrigger("isCross");
    }

    IEnumerator CrossController()
    {
        comboControl = true;
        yield return new WaitForSeconds(timeCross);
        punchCount = 0;
        comboControl = false;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = playerSpeed;
    }
}

