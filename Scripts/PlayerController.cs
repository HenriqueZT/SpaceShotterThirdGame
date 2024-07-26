using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float vel = 5f;
    private Rigidbody2D meuRB;

    //meu tiro
    [SerializeField] private GameObject tiro;
    [SerializeField] private GameObject tiro2;

    private float timer;

    //Pegando transform da posicao do tiro
    [SerializeField] private Transform posTiro;

    [SerializeField] private int vida = 5;
    [SerializeField] private int maxVida = 5;

    //Velocidade do tiro
    [SerializeField] private float velocidadeTiro = 10f;

    //Pegando gameobject da explosao
    [SerializeField] private GameObject explosao;

    private Vector3 pos;
    private float limiteY = 4.44f;
    private float limiteX = 8.3f;

    [SerializeField] private int levelTiro = 1;

    //Variavel do escudo
    [SerializeField] private GameObject shield;
    private GameObject actualShield;
    private float timerShield;
    [SerializeField] private int qtdEscudo;

    //Criando canva para mostrar a vida e o escudo
    [SerializeField] private Text vidaTexto;
    [SerializeField] private Text vidaEscudo;

    //Criando som do tiro
    [SerializeField] private AudioClip somTiro;
    [SerializeField] private AudioClip somMorte;
    [SerializeField] private AudioClip somEscudo;
    [SerializeField] private AudioClip somEscudoTchau;

    // Start is called before the first frame update
    void Start()
    {
        meuRB = GetComponent<Rigidbody2D>();

        //Informando o quanto de vida eu tenho
        vidaTexto.text = vida.ToString();
        vidaEscudo.text = qtdEscudo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Movimento();

        Atirar();

        CriaEscudo();

    }

    private void Movimento()
    {
        //Pegando o imput horizontal(telcas "A" e "D")
        var horizontal = Input.GetAxis("Horizontal");

        //Pegando o imput vertical (teclas "W" e "S")
        var vertical = Input.GetAxis("Vertical");

        // Criando um vetor de velocidade combinando horizontal e vertical
        Vector2 minhaVelo = new Vector2(horizontal, vertical).normalized * vel;//Uso o normalized para normalizar as velocidades em todas as direcoes   
        //Normalized poderia ser minhaVelo.Normalize();

        //Passando a minha velocidade para o meu RB
        meuRB.velocity = minhaVelo;

        //Limitando a posição dele
        //Clamp
        //Checando a minha posição x
        float meuX = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        //Checando a minha posição y
        float meuY = Mathf.Clamp(transform.position.y, -limiteY, limiteY);

        //Aplicando o meuY e meuX a minha posição
        transform.position = new Vector3(meuX, meuY, transform.position.z);
    }

    //Criando o método para atirar
    public void Atirar()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            //Pegando a tecla space que esta no fire1 em Edit, project settings e imput manager, para poder atirar
            if (Input.GetButton("Fire1"))
            {
                //Tocando o som do tiro
                AudioSource.PlayClipAtPoint(somTiro, Vector3.zero);

                switch (levelTiro)
                {
                    case 1:
                        CriaTiro(tiro, posTiro.position);

                        //Reiniciando o tempo
                        timer = 0.25f;
                        break;

                    case 2:

                        //Criando tiro da esquerda e direita
                        //esquerda
                        Vector3 posicao = new Vector3(transform.position.x - 0.45f, transform.position.y + 0.1f, 0f);
                        CriaTiro(tiro2, posicao);

                        //direita
                        posicao = new Vector3(transform.position.x + 0.45f, transform.position.y + 0.1f, 0f);
                        CriaTiro(tiro2, posicao);

                        //Reiniciando o tempo
                        timer = 0.25f;
                        break;

                    case 3:

                        // Criando tiro da esquerda, direita e meio
                        //esquerda
                        posicao = new Vector3(transform.position.x - 0.45f, transform.position.y + 0.1f, 0f);
                        CriaTiro(tiro2, posicao);

                        //meio
                        CriaTiro(tiro, posTiro.position);

                        //direita
                        posicao = new Vector3(transform.position.x + 0.45f, transform.position.y + 0.1f, 0f);
                        CriaTiro(tiro2, posicao);

                        //Reiniciando o tempo
                        timer = 0.25f;
                        break;

                }
            }
        }
    }

    private void CriaTiro(GameObject tiroCriado, Vector3 Posicao)
    {
        //Instanciando meu tiro
        GameObject tiros = Instantiate(tiroCriado, Posicao, transform.rotation);
        //Dar a direção e velocidade para o rb do tiro
        tiros.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, velocidadeTiro);
    }


    public void PerdeVida(int dano)
    {
        vida -= dano;

        //Informando o quanto de vida eu tenho
        vidaTexto.text = vida.ToString();

        if (vida <= 0)
        {
            //Destruindo player se vida = 0
            Destroy(gameObject);

            //Criando explosao quando morrer
            Instantiate(explosao, transform.position, Quaternion.identity);

            //Tocando o som de morte
            AudioSource.PlayClipAtPoint(somMorte,Vector3.zero);

            //Carregando a cena inicial do jogo
            //Achando o game manager
            var gameManager = FindObjectOfType<GameManager>();
            //Rodando o método de iniciar o jogo
            if (gameManager)
            {
                gameManager.Inicio();
            }
        }
    }

    //Evento de colisao com powerUp
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            //Só vai aumentar o nivel do tiro até o 3
            if (levelTiro < 3)
            {
                levelTiro++;
            }
            else
            {
                // Se o nível do tiro já está no máximo, aumenta a vida
                if (vida < maxVida) // Opcional: verifica se a vida está abaixo do máximo
                {
                    vida++;
                    vidaTexto.text = vida.ToString();
                }
            }

            Destroy(other.gameObject);
        }
    }

    private void CriaEscudo()
    {
        vidaEscudo.text = qtdEscudo.ToString();
        if (Input.GetButtonDown("Shield") && qtdEscudo > 0)
        {
            //Instanciar o escudo SE eu não tenho escudo atual
            if (!actualShield)
            {
                actualShield = Instantiate(shield, transform.position, transform.rotation);

                //Diminuindo a quantidade de escudo
                qtdEscudo--;

                //Tocando o som do escudo
                AudioSource.PlayClipAtPoint(somEscudo, Vector3.zero);
            }
        }

        //Fazendo o escudo atual seguir o player SE o escudo atual existe
        if (actualShield)
        {
            actualShield.transform.position = transform.position;

            //Se eu tenho um escudo eu começo a contar o tempo
            timerShield += Time.deltaTime;

            //Se já se passaram 6 segundos, então eu destruo o escudo
            if (timerShield > 6.2f)
            {
                //Destruindo o escudo atual
                Destroy(actualShield);

                //Tocando o som do escudo
                AudioSource.PlayClipAtPoint(somEscudoTchau, Vector3.zero);

                //Zerar o escudo timer
                timerShield = 0f;
            }
        }
    }
}