	/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe responsável pelas regras da pilhas intermediárias.

Referências e Documentação:
https://msdn.microsoft.com/pt-br/library/ms173149.aspx
https://msdn.microsoft.com/pt-br/library/sf985hc5.aspx
https://msdn.microsoft.com/pt-br/library/ms173105.aspx
https://msdn.microsoft.com/pt-br/library/sf985hc5.aspx


//---------------------------------------------------------------------------------------------------------------

$Creator: Cesar Peixoto $
$Notice: (C) Copyright 2016 by Cesar Peixoto. All Rights Reserved. $     Finalizado em 17/05/2016
=============================================================================================================== */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]     
public class IntermediateStack : AbstractStack 
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Construtor da classe.

    public IntermediateStack(TypeOfStack type)
    {
        this.type = TypeOfStack.Intermediate;
    }
	
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método virtual sobrescrito, que faz a validação conforme as regras da pilha intermediária

    public override bool Validate(System.Collections.Generic.Stack<GameObject> externalStack)
    {
        // Referencias das carta
        Card externalCard = externalStack.Peek().GetComponent<Card>();// Referência da carta da pilha externa.

        // Caso não haja cartas na pilha, checará:
        if (externalStack.Count == 0 && externalCard.Value == 13)                           // Checa se a pilha está vazia, caso positivo, só aceita reis.
            return true;        
        
        Card internalCard = stack.Peek().GetComponent<Card>();                              // Referência da carta da pilha interna.

        // Caso haja cartas na pilha, checará:
        bool condition1 = (isBlack(internalCard.Suit) != isBlack(externalCard.Suit));       // Compara as cores entre as cartas, e valida se as cores forem opostas
        bool condition2 = (internalCard.Value == (externalCard.Value + 1));                 // Compara se o valor da carta a ser recebida é é uma unidade inferior e valida caso positivo
        bool condition3 = internalCard.Value > 1;                                           // Compara se a carta a ser recebida é maior do que ás, e valida se for positivo

        if (condition1 && condition2 && condition3)                                         // Checa se todas as condições fora atendidas
            return true;
        else
            return false;                                                                   // Caso contrário, retorna falso.
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
}
