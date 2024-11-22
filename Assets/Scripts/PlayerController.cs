using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    public float playerSpeed = 1f;

    public Vector2 playerDirection;

    private bool isWalking;

    private Animator playerAnimator;

    // Player olhando para a direita
    private bool playerFacingRight = true;

    void Start()
    {
        //Obtem e inicializa as propriedades do RigiBody2D
        playerRigidBody = GetComponent<Rigidbody2D>();

        // Obtem e inicializa as propiedades do animator
        playerAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMove();
        UpdateAnimator();
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

        playerRigidBody.MovePosition(playerRigidBody.position + playerSpeed * Time.fixedDeltaTime * playerDirection);
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
}

