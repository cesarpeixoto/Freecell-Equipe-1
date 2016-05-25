/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe que representa as cartas do jogo.
Contem as informações necessárias para identificar uma carta no jogo, como naipe e valor.
Concentra informações cruciais para controle regras do jogo, como pilha de origem e pilha de destino;
Provê métodos para animação do posicionamento da carta.
Toma proveito de algumas inferfaces oferecidas pelo Unity, para implementar o comportamento de arrastar objetos.

Observações:
A documentação do Unity desaconselha a implementação de construtores para classes herdadas de MonoBehaviour,
portanto, os membros privados são inicializados por intermédio do método OnInitialise().

Não testamos a Tag Card no raycast, porque as interfaces estão implementadas nas próprias cartas, portanto,
se nãop for carta, não possui as interfaces para arrastar e soltar

Referências e Documentação:
http://docs.unity3d.com/Manual/EventSystem.html
http://docs.unity3d.com/ScriptReference/EventSystems.IBeginDragHandler.html
http://docs.unity3d.com/ScriptReference/EventSystems.IDragHandler.html
http://docs.unity3d.com/ScriptReference/EventSystems.IEndDragHandler.html

http://docs.unity3d.com/Manual/Coroutines.html
http://docs.unity3d.com/ScriptReference/EventSystems.PointerEventData-pointerCurrentRaycast.html

//---------------------------------------------------------------------------------------------------------------

$Creator: Cesar Peixoto $
$Notice: (C) Copyright 2016 by Cesar Peixoto. All Rights Reserved. $     Finalizado em 16/05/2016
=============================================================================================================== */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Atributos da Classe

    public AbstractStack originalStack = null;                                                      // Pilha de origem.
    //public AbstractStack destinyStack = null;                                                     // Pilha de destino.
    public int orderInStack = 0;                                                                    // Ordem da carta na pilha.
    public Vector3 originalPosition = Vector3.zero;                                                 // Posição física na origem.
    public Vector2 _clickOffset = Vector2.zero;                                                     // Conserva o offset do local clicado na carta
    //public int orderToFit = 0;                                                                    // Ordem na pilha da carta que está abaixo.

    public TypeOfCards.CardSuit _suit;                                                              // Naipe da carta.
    public int _value = 0;                                                                          // Valor da carta.
    private bool _catch = false;                                                                    // Flag que atribui se a carta passou pela validação e pode ser pega.


    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Propertys da Classe

    public TypeOfCards.CardSuit Suit { get { return _suit; } }                                      // Naipe da carta, depois de inicializado, não pode ser alterado.
    public int Value { get { return _value; } }                                                     // Valor da carta, depois de inicializado, não pode ser alterado.

    private Vector2 MousePosition { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } } // Retorna a posição do mouse na tela convertida para posição no jogo.

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método para inicializar os membros privados

    public void OnInitialise(TypeOfCards.CardSuit suit, int value)
    {
        this._suit = suit;                                                                           // Inicializa o naipe da carta.
        this._value = value;                                                                         // Inicializa o valor da carta.
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Métodos das interfaces herdadas.

    // Evento invocado no momento em que começa a arrastar a carta
    public void OnBeginDrag(PointerEventData eventData)
    {
        this._clickOffset = (Vector2)this.transform.position - MousePosition;                                           // Calcula o offset do clique do mouse na carta.
        //RaycastResult hit = eventData.pointerCurrentRaycast;                                                            // Recebe o resultado do Raycast.

        // Etapa de validação para saber se pode pegar a carta
        int quantity = orderInStack + 1;                                                                                // Pela ordem da carta na pilha, descobre-se a quantidade que tenta pegar.
        if(GameRulesManager.GetInstance().handStack.Validate(originalStack.stack, quantity))                            // Se validar pelas regras da pilha da mão.
        {
            this.originalPosition = transform.position;                                                                 // Atribui a posição física original.
            GameRulesManager.GetInstance().handStack.Push(ref originalStack.stack, quantity);                           // Adiciona a(s) carta(s) na pilha da mão.

            // Atribui hierarquia de parentesco nas cartas que serão movidas, se houver mais de uma
            if(quantity > 1)                
            {
                GameObject[] cardsArray = GameRulesManager.GetInstance().handStack.stack.ToArray();
                for (int i = quantity -2; i >= 0; i--)                                                                  // Menos 2, porque itera duas cartas por vez, e o indice começa em zero.
                    cardsArray[i + 1].transform.SetParent(cardsArray[i].transform);                                     // Faz a de baixo, filha da de cima.
            }                

            this._catch = true;                                                                                         // Assinala o estado como segurando carta.
            GameRulesManager.GetInstance().Organize("Default");
        }
    }

    // Evento invocado enquanto arrasta a carta
    public void OnDrag(PointerEventData eventData)
    {
        if(this._catch)                                                                                                 // Checa se a carta passou na validação para poder arrastá-la.
            this.transform.position = MousePosition + _clickOffset;                                                     // Atualiza a posição da carta conforme ela é arrastada.
    }

    // Evento quando para de arrastar, liberando o botão do mouse
    public void OnEndDrag(PointerEventData eventData)
    {   
        if(_catch)                                                                                                      // Confirma se está de fato arrastando carta.
        {
            if (eventData.pointerCurrentRaycast.gameObject == null)                                                     // Checa se a carta está posicionada em cima de um objecto válido.
            {
                ReturnToOrigin();                                                                                       // Se não estiver, chama função para devolver as cartas a pilha de origem.
                return;                                                                                                 // Devolvendo as cartas, sai da função.
            }

            if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Card"))                                          // Caso o objeto seja válido (verificado aima), verifica se é uma carta (pelo tag).
            {
                // Estamos tratando nesta parte, das cartas abaixo do cursor do mouse, e NÃO nas cartas arrastadas!
                GameObject card = eventData.pointerCurrentRaycast.gameObject;                                           // Recebe referência da a carta apontada abaixo do cursor do mouse (resultado do raycast).
                int orderToFit = card.GetComponent<Card>().orderInStack;                                                // Grava a posição da carta de baixo, na variavel orderToFit.
                AbstractStack destinyStack = card.GetComponent<Card>().originalStack;                                   // Recebe referência da pilha original que está armazenada nas informações da carta.

                if ((destinyStack.type == AbstractStack.TypeOfStack.Intermediate && orderToFit == 0))                   // Compara se é uma pilha intermediária e se está apontando para a carta do topo da pilha.
                {
                    if (destinyStack.Validate(GameRulesManager.GetInstance().handStack.stack))                          // Faz a validação das cartas que estão na mão do jogador
                    {                
                        Vector3 position = card.transform.position;                                                     // Recebe a posição da carta do topo da pilha (já validado assim, acima).
                        position.y -= GameRulesManager.GetInstance().stackOffsetPosition;                               // Aplica o offset de distancia entre as cartas no eixo Y.
                        MoveTo(destinyStack, position);                                                                 // Move as cartas da mão do jogador para a pilha destino.
                        return;
                    }
                }
                else if(destinyStack.type != AbstractStack.TypeOfStack.Intermediate)                                    // Caso, esteja apontado para uma carta em uma pilha que NÃO é intermediária (regras das  outras pilhas).
                {
                    if (destinyStack.Validate(GameRulesManager.GetInstance().handStack.stack))                          // Faz a validação das cartas que estão na mão do jogador, para as pilhas definitivas ou slots individuais
                    { 
                        Vector3 position = card.transform.position;                                                     // Recebe a posição física que a carta deverá ter na pilha.
                        MoveTo(destinyStack, position);                                                                 // Move as cartas da mão do jogador para a pilha destino.
                        return;
                    }
                }
            }
            else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Area"))                                     // Caso o objeto seja válido (verificado aima), e não seja uma carta, mas sim uma pilha ainda vazia.
            {
                GameObject area = eventData.pointerCurrentRaycast.gameObject;                                           // Recebe a referência desta pilha física.
                AbstractStack destinyStack = area.GetComponent<StackArea>().stackCards;                                 // Recebe referência da pilha contida nesta pilha física.
                if(destinyStack.type != AbstractStack.TypeOfStack.Intermediate)                                         // Caso tratar-se de pilhas definitivas ou slots individuais.
                {                    
                    if (destinyStack.Validate(GameRulesManager.GetInstance().handStack.stack))                          // Faz a validação das cartas que estão na mão do jogador nas regras de pilha definitiva e slots individuais.
                    {                
                        Vector3 position = area.transform.position;                                                     // Recebe a posição física que a carta deverá ter na pilha.
                        MoveTo(destinyStack, position);                                                                 // Move as cartas da mão do jogador para a pilha destino.
                        return;
                    }
                }
                else if(destinyStack.type == AbstractStack.TypeOfStack.Intermediate && destinyStack.stack.Count == 0)   // Caso seja uma pilha definitiva fazia
                {
                    GameObject card = GameRulesManager.GetInstance().handStack.stack.Peek();                            // Pega uma carta da mão do jogador, apenas para ter acesso a medidas, para calcular as posições
                    Bounds coordinates = area.GetComponent<BoxCollider2D>().bounds;                                     // Coordenadas das áreas de depósito de cartas.
                    Vector2 offset = new Vector2(coordinates.center.x, coordinates.max.y - (card.GetComponent<BoxCollider2D>().bounds.size.y / 2)); // Cria um offset para alinar as cartas.
                    MoveTo(destinyStack, offset);                                                                       // Move as cartas da mão do jogador para a pilha destino.
                    return;
                }
            }
            ReturnToOrigin();                                                                                           // Se chegar até aqui, é porque nenhum dos casos anteriores foi validado, portanto, retorna para pilha de origem.                        
                            
        }                                                                                                               // que utilizaremos para saber se a carta de baixo é a ultima carta da pilha
        this._catch = false;                                                                                            // Assinala que não está mais arrastando carta

        //GameRulesManager.GetInstance().Organize("Ignore Raycast");
    }
       

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Métodos auxiliar, para fazer as cartas voltarem para pilha de origem.

    public void ReturnToOrigin()
    {
        StartCoroutine(Move(originalStack, originalPosition));                                                          // faz animação e devolve as cartas para sua pilha de origem.
        GameRulesManager.GetInstance().Organize("Default");                                                             // Organiza as cartas em suas respectivas pilhas, atualizando os status.
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Métodos auxiliar, para fazer as cartas irem para pilha de destino.

    public void MoveTo(AbstractStack destinyStack, Vector3 destinyPosition)
    {
        StartCoroutine(Move(destinyStack, destinyPosition));                                                            // faz animação e envia as cartas para sua pilha de destino
        GameRulesManager.GetInstance().Organize("Default");                                                             // Organiza as cartas em suas respectivas pilhas, atualizando os status.
    }
  
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método responsável pela animação das cartas, e pela transação entre a pilha mão do jogador e as demais pilhas do jogo.

    IEnumerator Move(AbstractStack destinyStack, Vector3 destinyPosition)                                         
    {
        Stack<GameObject> cloneHand = CloneHand();                                                                      // Clona o condetudo da mão e a esvazia.
        GameObject[] handArray = cloneHand.ToArray();                                                                   // Converte este clone para um array.

        while(handArray[0].transform.position != destinyPosition)                                                       // Fica no laço enquando não chegar na posição de destino.
        {
            //Vector2 offset = (Vector2)handArray[0].transform.position;
            handArray[0].transform.position = Vector3.MoveTowards(handArray[0].transform.position,                      // Move a carta da posição atual para posição final, 18pixels por segundo.
                                                                  destinyPosition, Time.deltaTime * 18f);  

            yield return new WaitForEndOfFrame();                                                                       // Aguarda o fim do frame neste Thread.                       
        }

        handArray[0].transform.position = destinyPosition;                                                              // Cria ilusão de exatidão no movimento.
        for (int i = 0; i < handArray.Length; i++)
        {
            destinyStack.Push(ref cloneHand);
        }
        GameRulesManager.GetInstance().Organize();                                                                      // Reorganiza as cartas, atribuindo a LayerMask para ignorar o Raycast.
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método que Clona a pilha mão do jogador para um objeto temporário, e limera o conteudo da pilha mão de jogador
    private Stack<GameObject> CloneHand()
    {
        Stack<GameObject> result = new Stack<GameObject>();                                                             // Cria um objeto temporário to tipo pilha de GameObjects.
        GameObject[] handArray = GameRulesManager.GetInstance().handStack.stack.ToArray();                              // Converte o conteudo para um array.
        for (int i = handArray.Length - 1; i >= 0; i--)                                                                 // Clona o conteudo de mão do jogador para o objeto temporário.
        {
            result.Push(handArray[i]);
        }
        GameRulesManager.GetInstance().handStack.stack.Clear();                                                         // Limpa o conteudo de mão de jogador.
        return result;                                                                                                  // Retorna o objeto temporário.
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
}
