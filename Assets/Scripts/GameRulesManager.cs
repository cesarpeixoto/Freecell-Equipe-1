/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe responsável pela gerencia de todas as regras do jogo.
A validação das jogadas é feita pelas próprias classes que representam as pilhas de cartas (PilhaIntermediaria / 
PilhaDefinitiva e CartaSolta).

Observaços:
As cartas da pilha mão do jogador sempre utilizam a layermask "Ignore Raycast", pois o raycast parte do ponteiro
do mouse, portanto, se as cartas arrastadas forem visiveis para o raycast, elas irão obstruir o raio, e nunca
saberemos onde estamos apontando o ponteiro do mouse.

Referências e Documentação:
https://msdn.microsoft.com/en-us/library/system.collections.stack(v=vs.110).aspx
https://msdn.microsoft.com/en-us/library/aa288453(v=vs.71).aspx
http://docs.unity3d.com/ScriptReference/Bounds.html
http://docs.unity3d.com/ScriptReference/Renderer-sortingOrder.html

//---------------------------------------------------------------------------------------------------------------

$Creator: Cesar Peixoto $
$Notice: (C) Copyright 2016 by Cesar Peixoto. All Rights Reserved. $     Finalizado em 17/05/2016
=============================================================================================================== */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameRulesManager : MonoBehaviour 
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Singleton - Design Pattern

    private static GameRulesManager _Instance;
    public static GameRulesManager GetInstance() { return _Instance; }       

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Atributos da Classe

    public List<TypeOfCards> pack;                                                  // Lista de cartas que formam o Baralho
    public HandStack handStack = new HandStack();                                   // Classe mão, que representa as cartas movidas pelo jogador.
    private IntermediateStack[] _intermediateStacks = new IntermediateStack[8];     // Pilhas de cartas intermediárias (Array de PilhaIntermediaria).
    private DefinitiveStack[] _definitiveStacks = new DefinitiveStack[4];           // Pilhas de cartas definitivas (Array de PilhaDefinitiva).
    private SingleSlotStack[] _singleSlotStacks = new SingleSlotStack[4];           // Espaço para cartas (Array de Cartas).

    public float stackOffsetPosition = 0.5f;                                        // Offset vertical entre as cartas na pilha.

    public GameObject[] _intermediateStackArea;                                     // Array contendo as áreas físicas de descarte das pilhas intermediárias.
    public GameObject[] _definitiveStackArea;                                       // Array contendo as áreas físicas de descarte das pilhas definitivas.
    public GameObject[] _singleSlotStackArea;                                       // Array contendo as áreas físicas de descarte dos slots individuais.

    public GameObject cardPrefab = null;                                            // Referência para o prefab utilizado para instância das cartas. (ele é atribuido via inspector).
    public GameObject gameOverMenu = null;                                          // Painel de Game Over.


    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Métodos herdados de MonoBehaviour

    // Use this for initialization
    void Awake()
    {
        //---------------------------------------------------------------------------------------------------------------
        // Singleton - Design Pattern
        if (_Instance != null)
        {
            Destroy(gameObject);
            Debug.LogError("Mais de uma instancia de GameManager!!! Algo muito errado aconteceu!!!");
            return;
        }
        _Instance = this;

        //---------------------------------------------------------------------------------------------------------------
        // Inicialização das pilhas
        for (int i = 0; i < _intermediateStacks.Length; i++)
        {
            _intermediateStacks[i] = new IntermediateStack(AbstractStack.TypeOfStack.Intermediate);                 // Inicialização das pilhas intermediárias.
        }

        for (int i = 0; i < _definitiveStacks.Length; i++)
        {
            _definitiveStacks[i] = new DefinitiveStack(AbstractStack.TypeOfStack.definitive);                       // Inicialização das pilhas definitivas.
        }

        for (int i = 0; i < _singleSlotStacks.Length; i++)
        {
            _singleSlotStacks[i] = new SingleSlotStack(AbstractStack.TypeOfStack.SingleSlot);                       // Inicialização das pilhas de cartas soltas.
        }

        //---------------------------------------------------------------------------------------------------------------
        // Inicialização das areas físicas das pilhas

        for (int index = 0; index < _intermediateStackArea.Length; index++)
        {
            _intermediateStackArea[index].GetComponent<StackArea>().stackCards = _intermediateStacks[index];        // Inicializa referências das areas de pilhas intermediárias.
        }

        for (int index = 0; index < _definitiveStackArea.Length; index++)
        {
            _definitiveStackArea[index].GetComponent<StackArea>().stackCards = _definitiveStacks[index];            // Inicializa referências das areas de pilhas definitivas.
        }

        for (int index = 0; index < _singleSlotStackArea.Length; index++)
        {
            _singleSlotStackArea[index].GetComponent<StackArea>().stackCards = _singleSlotStacks[index];            // Inicializa referências das areas de pilhas slot individual.
        }

    }

    private void Start()
    {
        SortCards();
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método de conveniência, apenas para enxugar o código de partes repetitivas no método abaixo

    private void OrganizeCards(ref GameObject card, Vector2 position, int sortingOrder, AbstractStack stack, string layermask, int stackorder)
    {
        card.transform.position = position;
        card.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + 1;                                // Altera a ordem de renderização para a sequencia da pilha, de forma a carta de traz não ser desenhada na carta da frente.
        card.GetComponent<Card>().originalStack = stack;                                                // Atribui referência da pilha atual na carta.
        card.layer = LayerMask.NameToLayer(layermask);                                                  // Atribui Layer para Raycast.
        card.GetComponent<Card>().orderInStack = stackorder;                                            // Marca a ordem da carta na pinha.
        card.transform.SetParent(null);                                                                 // "Lipa" qualquer relação de parentesco
    }


    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método que organiza as cartas de acordo com suas respectivas pilhas (organiza posição, ordem de renderização, layermak, entre outros)

    public void Organize(string cardLayerMask = "Default", string handLayerMask = "Ignore Raycast")
    {
        // Organiza as cartas localizadas nas pilhas intermediárias
        for (int index = 0; index < _intermediateStackArea.Length; index++)
        {
			GameObject card = cardPrefab;                                                 // Pega uma carta qualquer, que será utilizada para extrair medidas.
            Bounds coordinates = _intermediateStackArea[index].GetComponent<BoxCollider2D>().bounds;                    // Coordenadas das áreas de depósito de cartas.
            Vector2 offset = new Vector2(coordinates.center.x, coordinates.max.y + (card.GetComponent<BoxCollider2D>().bounds.size.y / 2f) - 1.1f); //+ 0f*/)); /// 2)); // Cria um offset para alinar as cartas.
            GameObject[] cardsArray = _intermediateStacks[index].stack.ToArray();                                       // Converte a pilha para Array, para facilitar o loop inverso.

            int order = 0;                                                                                              // Indice para marcar a ordem na pilha.
            for (int i = cardsArray.Length -1; i >= 0; i--)
            {
                OrganizeCards(ref cardsArray[i], offset, order, _intermediateStacks[index], cardLayerMask, i);
                offset.y -= stackOffsetPosition;                                                                        // Aplica o offset no eixo Y, para proxima carta ser posta mais para baixo.
                order++;                                                                                                // Incrementa o indice de marcação da pilha.
            }
        }

        // Organiza pilha definitiva
        for (int index = 0; index < _definitiveStackArea.Length; index++)
        {
            Bounds coordinates = _definitiveStackArea[index].GetComponent<BoxCollider2D>().bounds;                      // Coordenadas das áreas de depósito de cartas.
            GameObject[] cardsArray = _definitiveStacks[index].stack.ToArray();                                         // Converte a pilha para Array, para facilitar o loop inverso.

            int order = 0;                                                                                              // Indice para marcar a ordem na pilha.
            for (int i = cardsArray.Length -1; i >= 0; i--)
            {
                OrganizeCards(ref cardsArray[i], coordinates.center, order, _definitiveStacks[index], cardLayerMask, order);
                order++;                                                                                                // Incrementa o indice de marcação da pilha.
            }
        }
            
        // Organiza pilha definitiva
        for (int index = 0; index < _singleSlotStackArea.Length; index++)
        {
            Bounds coordinates = _singleSlotStackArea[index].GetComponent<BoxCollider2D>().bounds;                      // Coordenadas das áreas de depósito de cartas.
            GameObject[] cardsArray = _singleSlotStacks[index].stack.ToArray();                                         // Converte a pilha para Array, para facilitar o loop inverso.

            int order = 0;                                                                                              // Indice para marcar a ordem na pilha.
            for (int i = cardsArray.Length -1; i >= 0; i--)
            {
                OrganizeCards(ref cardsArray[i], coordinates.center, order, _singleSlotStacks[index], cardLayerMask, order);
                order++;                                                                                                // Incrementa o indice de marcação da pilha.
            }
        }

        // Organiza as cartas localizadas nas pilhas mão do jogador
        GameObject[] handArray = handStack.stack.ToArray();                                                             // Converte a pilha de mão do jogador para um array.
        for (int index = 0; index < handArray.Length; index++)
        {
            handArray[index].GetComponent<SpriteRenderer>().sortingOrder = 30 + index;                                  // Altera a orde de renderização, para que ele apareça por cima das outra cartas do jogo.
            handArray[index].layer = LayerMask.NameToLayer(handLayerMask);                                              // Atribui as cartas empiladas em mão do jogador, a layermask Ignore Raycast.
        }

    }	
	
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método para criar as instancias dos GameObjects cartas, e atribuir suas caracteristicas individuais

    private GameObject createGameObject(TypeOfCards card)
    {
        GameObject result;                                                                                              // Cria uma referência para um GameObject
        result = (GameObject)Instantiate(cardPrefab);                                                                   // Cria uma instância de GameObject a partir do prefab, atribuindo-o para a referência criada acima
        result.GetComponent<SpriteRenderer>().sprite = card.image;                                                      // Atribui a imagem da carta ao GameObject
        result.GetComponent<Card>().OnInitialise(card.suit, card.value);                                                // Atribui informações de valor e naipe ao GameObject
        return result;                                                                                                  // Retorna o GameObject criado
	}

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método para sortear as cartas, distribuindo-as nas pilhas de acordo com as regras do jogo

    private void SortCards()
    {
        //---------------------------------------------------------------------------------------------------------------
        // sorteio das cartas

        for (int i = 0; i < 6; i++)                                                                                     // Distribui 6 cartas.
        {
            for(int j = 0; j < 8; j++)                                                                                  // 8 pilhas recebem 6 cartas.
            {
                int randomCard = Random.Range(0, pack.Count);                                                           // Sorteia uma carta aleatória.
                GameObject card = createGameObject(pack[randomCard]);                                                   // Função cria instancia do GameObject e insere referencia.
                _intermediateStacks[j].stack.Push(card);                                                                // Adiciona a carta sorteada na pilha.
				pack.RemoveAt(randomCard);                                                                              // Remove a carta sorteada do baralho.
            }
        }
        //---------------------------------------------------------------------------------------------------------------
        // Distribui as ultimas 4 cartas nas primeiras 4 pilhas intermediárias
        for(int j = 0; j < 4; j++)                                                                                      // Distribui mais 4 cartas para as 4 primeiras pilhas.
        {
            int randomCard = Random.Range(0, pack.Count);                                                               // Sorteia uma carta aleatória.
            GameObject card = createGameObject(pack[randomCard]);                                                       // Função cria instancia do GameObject e insere referencia.
            _intermediateStacks[j].stack.Push(card);                                                                    // Adiciona a carta sorteada na pilha.
            pack.RemoveAt(randomCard);                                                                                  // Remove a carta sorteada do baralho.
        }
        //---------------------------------------------------------------------------------------------------------------
        // Para fins de debug, se houver cartas no baralho, houve um erro no sorteio das cartas
        if (pack.Count > 0)
            Debug.LogError("Ocorreu um erro no sorteio de cartas. Método Start(), classe GameManager"); // Exibe mensagem de erro

        Organize();
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método que verifica condição de fim da partida

    private void CheckGameOver()
    {
        bool result = true;
        for (int i = 0; i < 4; i++) 
        {
            if (_definitiveStacks[0].stack.Count != 13)                         // Se alguma pilha definitivia não conter 13 cartas, result será falso.
                result = false;
        }            
        gameOverMenu.SetActive(result);                                         // Ativa o painel de Game Over se result for true.
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método que reinicia o jogo

    public void RestartGame()
    {
        SceneManager.LoadScene("Jogo");                                              // Reinicia o jogo.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------

}


/*   Integrantes do time

/////// PROGRAMAÇÃO \\\\\\\

Cesar da Silva Peixoto      RA: 15674591
Heitor Soares Mattosinho    RA: 15671514


////////// ARTES \\\\\\\\\\

Lucas Linarelo              RA: 15677859
Isaac Lara                  RA: 15692155


/////// GAME DESIGN \\\\\\\


 */
