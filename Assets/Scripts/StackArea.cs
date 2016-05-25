/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe que representa as pilhas de cartas do jogo.
Contem as informações necessárias para identificar uma pilha de carta do jogo, como tipo de pilha, referência 
para uma pilha de cartas (do GameRulesManager).

OBSERVAÇÃO:
As observações abaixo não puderam ser implementadas, pois, o EventSystem, utilizando Physics2D Raycaster, não 
sincroniza direito os eventos OnDrop e OnEndDrag, hora funcionando OnEndDrag primeiro, hora não, desta forma,
não foi possivel ajustar as Layermask para manipular o comportamento de Raycast de acordo com as necessidades 
da classe, ocasionando bugs. Por este motivos, desistimos da implementação nesta classe, (ficando apenas com 
a função de guardar uma referência da pilha), e implementamos todas as validações na classe Card, enveto OnEndDrag.
Estas interfaces funcionam perfeitamente no Canvas.

//---------------------------------------------------------------------------------------------------------------
                                Era para ser mas não foi (Justificativa acima)
Concentra informações cruciais para controle regras do jogo, como pilha de origem e pilha de destino;
Provê métodos para animação do posicionamento da carta.
Toma proveito de algumas inferfaces oferecidas pelo Unity, para implementar o comportamento de arrastar objetos.

Observações:
A documentação do Unity desaconselha a implementação de construtores para classes herdadas de MonoBehaviour,
portanto, os membros privados são inicializados por intermédio do método OnInitialise().

Referências e Documentação:
http://docs.unity3d.com/Manual/EventSystem.html
http://docs.unity3d.com/ScriptReference/EventSystems.IDropHandler.html
http://docs.unity3d.com/ScriptReference/EventSystems.IPointerEnterHandler.html
http://docs.unity3d.com/ScriptReference/EventSystems.IPointerExitHandler.html
http://docs.unity3d.com/460/Documentation/ScriptReference/EventSystems.PointerEventData-pointerDrag.html


//---------------------------------------------------------------------------------------------------------------

$Creator: Cesar Peixoto $
$Notice: (C) Copyright 2016 by Cesar Peixoto. All Rights Reserved. $     Finalizado em 18/05/2016
=============================================================================================================== */


using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

public class StackArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler 
{
    public AbstractStack stackCards = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //GameRulesManager.GetInstance().Organize("Default");  

        /*
        // Armazena a pilha destino, seta para ativar o raycast das cartas, para saber a posição das carta que está soltando
        if(eventData.pointerDrag != null)                                                                           // Não preciso testar outras coisas, porque só carta é arrastável
        {
            GameRulesManager.GetInstance().Organize("Default");                                                     // Reorganiza as cartas, atribuindo a LayerMask para Default.
        }
        /*
        Card card = eventData.pointerDrag.GetComponent<Card>();                                                     // Recebe referência de Card.
        if (card != null)                                                                                           // Validação de segurança, só fará se estiver arrastando carta.
        {
            card.destinyStack = stackCards;                                                                         // Atribui esta pilha para pilha destino da carta.
            GameRulesManager.GetInstance().Organize("Default");                                                     // Reorganiza as cartas, atribuindo a LayerMask para Default.
        }
        */
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*
        GameRulesManager.GetInstance().Organize("Ignore Raycast");    

        if(eventData.pointerDrag != null)                                                                           // Não preciso testar outras coisas, porque só carta é arrastável
        {
            GameRulesManager.GetInstance().Organize("Ignore Raycast");                                              // Reorganiza as cartas, atribuindo a LayerMask para ignorar o Raycast.
        }


        /*
        // Seta pilha destino para null, seta para destaivaativar o raycast, para poder identificar a zona que vai entrar
        Card card = eventData.pointerDrag.GetComponent<Card>();                                                     // Recebe referência de Card.                                        
        if (card != null)                                                                                           // Validação de segurança, só fará se estiver arrastando carta.
        {
            card.destinyStack = null;                                                                               // Atribui nulo na pilha destino da carta.
            GameRulesManager.GetInstance().Organize("Ignore Raycast");                                              // Reorganiza as cartas, atribuindo a LayerMask para ignorar o Raycast.
        }
        */
    }

    public void OnDrop(PointerEventData eventData)
    {

        //Debug.Log("Ok, tivemos um drop");

        /*
        Debug.Log("Ok, tivemos um drop");
        Card card = eventData.pointerDrag.gameObject.GetComponent<Card>();                                          // Recebe referência da carta solta na pilha

        if(stackCards.type == AbstractStack.TypeOfStack.Intermediate && card.orderToFit == 0)                       // Se for pilha intermediária e for a carta do topo da pilha (lembrando que a pilha é apresentada de forma invertida).
        {    
            Debug.Log("Validação para intermediária: " + stackCards.Validate(GameRulesManager.GetInstance().handStack.stack));
            if(stackCards.Validate(GameRulesManager.GetInstance().handStack.stack))                                 // Faz a validação das cartas que estão na mão do jogador
            {                
                Vector3 position = card.transform.position;                                                         // Recebe a posição da carta do topo da fila (já validado assim, acima).
                position.y -= GameRulesManager.GetInstance().stackOffsetPosition;                                   // Aplica o offset.
                card.MoveTo(ref stackCards, position);
                return;
            }
        }
        else if(!(stackCards.type == AbstractStack.TypeOfStack.Intermediate))                                        // Caso a pilha não seja intermediária
        {
            if (stackCards.Validate(GameRulesManager.GetInstance().handStack.stack))                                 // Faz a validação das cartas que estão na mão do jogador
            {
                return;
            }
        }
            
        // Se chegar até aqui, é porque as validações fracassaram e a carta voltará para pilha de origem
        card.ReturnToOrigin();                                                                              // faz animação e devolve as cartas para sua pilha de origem.

        // sobre esta questão do ray desativado no momento do drop, pode passar referencia daqui para a carta, e tratar as coisas na carta???

        // Validação pode ser feita aqui, ai volta se for inválido???

        // a função da animação pode ficar aqui também, assim eliminamos a necessidade de destinyStak na carta!!!
        // pega o tipo da pilha, se for intermadiária, usa o offset no eixo y, se não for, o offset é zero.


            // checar o tipo de pilha, para saber como empilhar as cartas!!!!!!!!!!!!!!
            */

    }


	
	// Update is called once per frame
	void Update () {
	
	}
}
