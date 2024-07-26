using UnityEngine;
using UnityEngine.UI;

public class BossController : InimigoPai
{
    [Header("Informações basicas")]
    private string estado = "estado1";
    private bool direita = true;

    [Header("Informações dos tiros")]
    [SerializeField] private Transform posicaoTiro1Esquerda;
    [SerializeField] private Transform posicaoTiro1Direita;
    [SerializeField] private Transform posicaoTiro2;
    [SerializeField] private GameObject tiro1;
    [SerializeField] private GameObject tiro2;
    private float delayTiro = 1f;
    private float timer2 = 1f;
    [SerializeField] private string[] estados;
    [SerializeField] private float timerEstado = 10f;

    //Criando variavel para canva da vida do boss
    [SerializeField] private Image vidaImagem;
    [SerializeField] private int vidaMax = 100;

    // Start is called before the first frame update
    void Start()
    {
        meuRB = GetComponent<Rigidbody2D>();

        //Dando a vida inicial maxima
        vida = vidaMax;

        //Avisando para o meu canvas que a camera dele é a camera atual do jogo
        //Pegando o canvas do boss - Avisando qual é a camera
        //Passando a camera do jogo
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        TrocaEstado();

        switch (estado)
        {

            case "estado1":
                Estado1();
                break;

            case "estado2":
                Estado2();
                break;

            case "estado3":
                Estado3();
                break;

        }

        //Atualizando a vida do boss
        //Garantir que a divisão vai retornar um float
        vidaImagem.fillAmount = ((float)vida / vidaMax);
        //Converter o valor do fillAmount para alguma coisa entre 0 e 255

        //Convertendo o valor do fill amount para alguma coisa entre 0 e 255, e garantindo que o valor é do tipo byte
        vidaImagem.color = new Color32(190,(byte) (vidaImagem.fillAmount * 255), 54, 255);
    }

    private void AumentaDificuldade()
    {
        //Checando se minha vida é menor ou igual a metade
        if (vida <= vidaMax / 2)
        {
            delayTiro = 0.5f;
        }
    }
    private void Estado1()
    {
        AumentaDificuldade();
        //Atirando
        //Criando a espera do tiro
        if (timer <= 0f) 
        { 
            Tiro1();
            timer = delayTiro;
        }
        else
        {
            //Diminuindo a espera do tiro
            timer -= Time.deltaTime;
        }

        //Indo para a direita e para a esquerda
        if (direita)
        {
            if (transform.position.x <= 6.12f)
            {
                meuRB.velocity = new Vector2(velocidade, 0f);
            }
            else
            {
                direita = false;
            }
        }
        else
        {
            if (transform.position.x >= -6.12)
            {
                meuRB.velocity = new Vector2(-velocidade, 0f);
            }
            else
            {
                direita = true;
            }
        }
    }

    private void Estado2()
    {
        AumentaDificuldade();
        //Deixando ele parado
        meuRB.velocity = Vector2.zero;

        //Criando a espera do tiro
        if (timer <= 0f)
        {
            Tiro2();
            timer = delayTiro/2;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void Estado3()
    {
        AumentaDificuldade();
        //Deixando ele parado
        meuRB.velocity = Vector2.zero;

        //Criando a espera do tiro
        if(timer <= 0f)
        {
            Tiro1();
            timer = delayTiro;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        if(timer2 <= 0)

        {
            Tiro2();
            timer2 = delayTiro / 2;
        }
        else
        {
            timer2 -= Time.deltaTime;
        }
    }

    private void Tiro1()
    {
        //Tocando o som do tiro
        TocaTiro();

        //Criando o tiro da esquerda
        GameObject tiro = Instantiate(tiro1, posicaoTiro1Esquerda.position, transform.rotation);
        tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -velocidadeTiros);
        //Criando o tiro da direita
        tiro = Instantiate(tiro1, posicaoTiro1Direita.position, transform.rotation);
        tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -velocidadeTiros);
    }
    private void Tiro2()
    {
        //Tocando o som do tiro
        TocaTiro();

        //Encontrando o player na cena
        var player = FindObjectOfType<PlayerController>();

        //Só fazer qualquer coisa se o player existir
        //Criando timer para atirar
        if (player)
        {
            //Instanciando meu tiro        
            GameObject tiros = Instantiate(tiro2, posicaoTiro2.position, transform.rotation);

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
        }
    }

    private void TrocaEstado()
    {
        if (timerEstado <= 0f)
        {
            //Escolhendo meu novo estado
            //Escolher um valor aleatório entre 0 e a quantidade de estados
            int Indice = Random.Range(0, estados.Length);

            estado = estados[Indice];
            timerEstado = 8f;
        }
        else
        {
            timerEstado -= Time.deltaTime;
        }
    }
}
