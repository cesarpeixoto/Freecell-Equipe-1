/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe responsável pelas regras da pilhas mão do jogador.

Referências e Documentação:
https://msdn.microsoft.com/pt-br/library/sf985hc5.aspx
https://msdn.microsoft.com/pt-br/library/ms173105.aspx


//---------------------------------------------------------------------------------------------------------------

$Creator: Cesar Peixoto $
$Notice: (C) Copyright 2016 by Cesar Peixoto. All Rights Reserved. $     Finalizado em 10/05/2016
=============================================================================================================== */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandStack 
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Atributos da Classe

    public Stack<GameObject> stack = new Stack<GameObject>();                                                           // Pilha de cartas.

    public void Push(ref Stack<GameObject> externalStack, int quantity)
    {
        for (int i = 0; i < quantity; i++)                                                                              // Transfere a quantidade de cartas para mão.
            stack.Push(externalStack.Pop());
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método  que faz a validação conforme as regras da pilha mão do jogador

    public bool Validate(Stack<GameObject> externalStack, int quantity)
    {
        //if (stack.Count > 0)                                                                                           // Se já tiver cartas, não pode pegar outras
        //    return false;
        
        if(quantity == 1)                                                                                                // Para pegar a primeira carta da pilha, não precisa validar
            return true;             

        GameObject[] cards = externalStack.ToArray();                                                                    // Converte para Array, a primeira a sair na pilha é indice 0!

        for (int i = 0; i < quantity -1; i++) //                                                                         // -1 porque já comparamos com a carta de baixo.
        {
            bool condition1 = cards[i].GetComponent<Card>().Value == (cards[i + 1].GetComponent<Card>().Value - 1);      // Carta de baixo deve ter valor uma unidade menor que a de cima.
            bool condition2 = isBlack(cards[i].GetComponent<Card>().Suit) != isBlack(cards[i + 1].GetComponent<Card>().Suit);// Naipes de cores diferentes.
            if (!condition1 || !condition2)                                                                              // Se qualquer condição não for satisfeita, retorna falso.
                return false;             
        }
        return true;
    }

    //---------------------------------------------------------------------------------------------------------------
    // Método responsável pela identificação da cor da carta pelo naipe, retornado verdadeiro se a carta for preta.

    protected bool isBlack(TypeOfCards.CardSuit suit)
    {
        return ((int)suit % 2 == 0);                                                                                    // Checa se for par (preto no enum de naipes) retorna verdadeiro.
    }

}
