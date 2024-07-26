using UnityEngine;


public class Inimigo02Controller : InimigoPai
{

    //Pegando transform da posicao do tiro
    [SerializeField] private Transform posTiro;

    [SerializeField] private float yMax;

    private bool possoMover = true;

    // Start is called before the first frame update
    void Start()
    {
        meuRB = GetComponent<Rigidbody2D>();
        Descer();

        //Tempo para o tiro sair aleatoriamente 
        timer = Random.Range(1f, 1.7f);
    }

    // Update is called once per frame
    void Update()
    {
        Atirar();
        Movimento();
    }

    private void Movimento()
    {
        
        if (transform.position.y < yMax && possoMover)
        {
            //Checando de que lado estou
            //Esquerda
            if (transform.position.x < 0)
            {

                //Indo para a direita
                //meuRB.velocity = (Vector2.up + Vector2.left) * velocidade;
                //ou
                meuRB.velocity = new Vector2(velocidade * -1, velocidade);
    
                //Falando que eu não posso mais me mover
                possoMover = false;
            }
            //Direita
            else
            {
                //Indo para a esquerda
                //meuRB.velocity = (Vector2.up + Vector2.right) * velocidade;
                //ou
                meuRB.velocity = new Vector2(velocidade, velocidade);

                //Falando que eu não posso mais me mover
                possoMover = false;

            }
        }
    }

    public void Descer()
    {
        meuRB.velocity = Vector2.up * velocidade;
       
    }

    public void Atirar()
    {
        //Pegando informação dos meus filho
        bool visivel = GetComponentInChildren<SpriteRenderer>().isVisible;

        if (visivel)
        {
            //Encontrando o player na cena
            var player = FindObjectOfType<PlayerController>();

            //Só fazer qualquer coisa se o player existir
            //Criando timer para atirar
            if (player)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {

                    //Instanciando meu tiro        
                    GameObject tiros = Instantiate(tiro, posTiro.position, transform.rotation);

                    //Encontrando o valor da direção
                    Vector2 direcao = player.transform.position - tiros.transform.position;
                    //Normalizando a velocidade dele
                    direcao.Normalize();
                    //Dando a direcao e velocidade do meu tiro
                    tiros.GetComponent<Rigidbody2D>().velocity = direcao * velocidadeTiros;

                    //Dando o angulo que o tiro tem que estar, Atan2 acha um angulo em um raio com base em um vetor, Math.Rad2Ded converte em graus(angulo
                    //90,180 de um circulo e assim vai
                    float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
                    //Passando o angulo
                    tiros.transform.rotation = Quaternion.Euler(0f, 0f, angulo + 90f);

                    //Reiniciando o tempo para o tiro sair aleatoriamente 
                    timer = Random.Range(2f, 3.5f);

                    //Tocando o som do tiro
                    TocaTiro();
                }
            }
        }
    }
}
