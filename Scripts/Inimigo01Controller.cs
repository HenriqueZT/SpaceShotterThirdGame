using UnityEngine;

public class Inimigo01Controller : InimigoPai
{

    //Pegando transform da posicao do tiro
    [SerializeField] private Transform posTiro;

    // Start is called before the first frame update
    void Start()
    {
        meuRB = GetComponent<Rigidbody2D>();
        Descer();

        //Tempo para o tiro sair aleatoriamente 
        timer = Random.Range(1f, 2f);

    }

    // Update is called once per frame
    void Update()
    {

        Atirar();
        //Vou checar se meu sprite renderer esta visivel

    }

    public void Descer()
    {
        meuRB.velocity = new Vector2(0f, velocidade);


    }

    public void Atirar()
    {
        //Pegando informação dos meus filho
        bool visivel = GetComponentInChildren<SpriteRenderer>().isVisible;

        if (visivel)
        {
            //Criando timer para atirar
            timer -= Time.deltaTime;
            if (timer <= 0)
            {

                //Instanciando meu tiro        
                var tiros = Instantiate(tiro, posTiro.position, Quaternion.identity);
                tiros.GetComponent<Rigidbody2D>().velocity = Vector2.down * velocidadeTiros;

                //Reiniciando o tempo
                timer = Random.Range(1.5f, 2f);

                //Tocando o som do tiro
                TocaTiro();
            }
        }
    }
}
