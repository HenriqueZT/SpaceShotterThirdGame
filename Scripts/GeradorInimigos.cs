using UnityEngine;
using UnityEngine.UI;

public class GeradorInimigos : MonoBehaviour
{
    [SerializeField] private GameObject[] inimigos;

    [SerializeField] private int pontos = 0;
    [SerializeField] private int level = 0;
    [SerializeField] private int proximoLevel = 100;
    
    private float esperaInimigos = 0f;
    [SerializeField] private float tempEspera = 5f;
    [SerializeField] private int qtdInimigo = 0;
    [SerializeField] private GameObject bossAnimation;
    private bool animBoss = false;

    //Criando canvas de pontuação e level
    [SerializeField] private Text pontuacao;
    [SerializeField] private Text leveis;

    //Criando musica para o boss
    [SerializeField] private AudioClip musicaBoss;
    [SerializeField] private AudioSource musicaJogo;

    // Start is called before the first frame update
    void Start()
    {
        pontuacao.text = "Pontos: " + pontos.ToString();
        leveis.text = "Level " + level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Gerando inimigos enquanto eu não estou no level 10, cheguei no level 10 eu crio o boss
        //Criando a animação do boss

        if (level < 10)
        {
            GerarInimigos();
        }
        else 
        {
            GeraBoss();
        }
    }

    //Criando o método para gerar o boss
    private void GeraBoss()
    {
        //Se a quantidade de inimigos for 0, entao ele diminui o tempo de espera
        if (qtdInimigo <= 0 && tempEspera > 0)
        {
            tempEspera -= Time.deltaTime; 
        }
            //Instanciando a animação do boss se eu ainda não criei ele e a espera for 0
            if (!animBoss && tempEspera <= 0)              {
                GameObject bA = Instantiate(bossAnimation, transform.position, transform.rotation);
                
                //Avisando que eu ja fiz a animação do boss
                animBoss = true;
                
                //Destruindo a animação depois de faze-la
                Destroy(bA, 6.5f);

                //Parando de tocar a musica do jogo e comecando a tocar a musica do boss
                musicaJogo.clip = musicaBoss;
                //Agora toque essa musica nova
                musicaJogo.Play();
            }
    }

    public void GanhandoPontos(int pontos)
    {
        this.pontos += pontos * level;

        pontuacao.text = "Pontos: " + this.pontos.ToString();

        if (this.pontos >= proximoLevel)
        {
            level++;
            leveis.text = "Level " + level.ToString();
            proximoLevel *= 2;
        }
    }

    //Diminuindo a quantidade de inimigos
    public void DiminuiQtd()
    {
        qtdInimigo--;
    } 

    //Método para checar se a posição está livre
    private bool ChecaPosicao(Vector3 posicao, Vector2 size)
    {
        //Checando se na posição tem alguem
        Collider2D hit = Physics2D.OverlapBox(posicao, size, 0f);

        //Debug.Log(hit);
        //Se o hit for NULL retorna False
        //Se o hit não for NULL retorna True
        if (hit != null)
        {
            return true;
            //Houve colisão
        }
        else
        {
            return false;
            //Não houve colisão
        }

        //Se o hit é NULL = não houve colisão
        //Posso criar o inimigo ali
        //Se o hit nao é NULL = houve colisão
        //Não posso criar inimigo ali

        //Fazer esse método retornar um dado booleano
    }

    private void GerarInimigos()
    {
        //Diminuindo o tempo se ele for maior que 0
        if (esperaInimigos > 0 && qtdInimigo <= 0)
        {
            esperaInimigos -= Time.deltaTime;
        }

        //Checando se a espera zerou
        if (esperaInimigos <= 0f && qtdInimigo <= 0f)
        {
            int quantidade = level * 4;

            int tentativa = 0;
            //Criando varios inimigos de uma só vez
            while (qtdInimigo < quantidade)
            {

                //Fazendo ele sair do laço de repetição SE ele repetir muitas vezes
                //Aumentando a tentativa
                tentativa++;
                // Se ele tentou mais de 200 vezes ele desiste
                if (tentativa > 200)
                {
                    break;
                }

                GameObject inimigoCriado;

                //Decidindo qual inimigo vai ser criado com base no level
                float chance = Random.Range(0f, level);
                if (chance > 2f)
                {
                    inimigoCriado = inimigos[1];
                }
                else
                {
                    inimigoCriado = inimigos[0];
                }

                //Definindo a posição que o inimigo vai ser criado
                Vector3 posicao = new Vector3(Random.Range(-8.1f, 8.1f), Random.Range(17f, 8.5f), 0f);
                
                //Checar se essa posição esta livre, ou seja, não tenha ninguem ainda
                //Fazer o checaPosicao avisar se eu posso ou nao criar o inimigo
                bool colisao = ChecaPosicao(posicao, inimigoCriado.transform.localScale);
                //Criar os inimigos se não houve colisão
                //Se houve colisão vou pular essa repetição
                if (colisao) 
                {
                    //Continue faz o laço de repetição ir paraa próxima repetição
                    continue; 
                }

                //Criando o inimigp
                Instantiate(inimigoCriado, posicao, transform.rotation);
               
                //Aumentando a quantidade de inimigos
                qtdInimigo++;
                
                //Reiniciando a espera
                esperaInimigos = tempEspera;
            }
        }
    }
}
