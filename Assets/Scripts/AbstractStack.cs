/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe abstrata, base para criar as instâncias dos diferentes tipos de pilha do jogo.
Ela provê funcionalidades genéricas obrigatórias para suas subclasses (método Push, isBlack, atributos tipo de 
pilha e pilha de cartas), bem como torna obrigatória a implementação do método de validação.

Observações:
Todas as classes das pilhas do jogo, deverão herdar desta classe.
Por conta da interface visual, a validação das cartas ocorre antes, portanto o método Push não se preocupa com
a validação, pois, na construção da interface visual, ele só será invocado (no fim da animação) após a validação 
das cartas.

Referências e Documentação:
https://msdn.microsoft.com/pt-br/library/ms173149.aspx
https://msdn.microsoft.com/pt-br/library/sf985hc5.aspx
https://msdn.microsoft.com/pt-br/library/ms173105.aspx
https://msdn.microsoft.com/pt-br/library/sf985hc5.aspx

//---------------------------------------------------------------------------------------------------------------

$Creator: Cesar Peixoto $
$Notice: (C) Copyright 2016 by Cesar Peixoto. All Rights Reserved. $     Finalizado em 16/05/2016
=============================================================================================================== */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractStack 
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Enumerações da Classe

    public enum TypeOfStack { Intermediate, definitive, SingleSlot }                    // Enumeração dos tipos de pilhas.

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Atributos da Classe

    public TypeOfStack type;                                                            // Tipo de pilha.
    public Stack<GameObject> stack = new Stack<GameObject>();                           // Pilha de cartas.

    //---------------------------------------------------------------------------------------------------------------
    // Método responsável por inserir uma carta na pilha (Sobre validação, consta os motivos nas observações).

    public void Push(ref Stack<GameObject> externalStack)                               // Método abstrato para adicionar cartas na pilha.
    {
        stack.Push(externalStack.Pop());                                                // Adiciona a carta na pilha.
        //GameRulesManager.GetInstance().Organize();                                    // Atualiza as informações nas listas
    }

    //---------------------------------------------------------------------------------------------------------------
    // Método responsável pela identificação da cor da carta pelo naipe, retornado verdadeiro se a carta for preta.

    protected bool isBlack(TypeOfCards.CardSuit suit)
    {
        return ((int)suit % 2 == 0);                                                    // Checa se for par (preto no enum de naipes) retorna verdadeiro.

	}

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método abstrato.

    public abstract bool Validate(Stack<GameObject> externalStack);                     // Método abstrato para validação das regras.

    //-------------------------------------------------------------------------------------------------------------------------------------------------
}
