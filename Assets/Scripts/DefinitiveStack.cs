/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe responsável pelas regras da pilhas definitivas.

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
public class DefinitiveStack : AbstractStack 
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Construtor da classe.

    public DefinitiveStack(TypeOfStack type)
    {
        this.type = TypeOfStack.definitive;                                             // Inicializa informação a respeito do tipo de pilha
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método virtual sobrescrito, que faz a validação conforme as regras da pilha definitiva.

    public override bool Validate(Stack<GameObject> externalStack)
    {
        if (externalStack.Count > 1)                                                    // Esta pilha só recebe uma carta por vez, portanto a mão do jogador deve conter apenas uma carta.
            return false;

        Card externalCard = externalStack.Peek().GetComponent<Card>();                  // Recebe referência da carta externa (carta que se pretente empilhar nesta pilha).
        // se tiver vazio qualquer az entra
        if (stack.Count == 0 && externalCard.Value == 1)                                // Verifica, se a pilha estiver vazia, a primeira carta posta deve ser um AS.
            return true;

        if(stack.Count != 0)                                                            // Caso exista outras cartas na pilha, verifica.
        {
            Card internalCard = stack.Peek().GetComponent<Card>();                      // Recebe referência da carta do topo da pilha.             
            bool condition1 = internalCard.Suit == externalCard.Suit;                   // Primeira Condição: Compara se os naipes são iguais.
            bool condition2 = (internalCard.Value == (externalCard.Value - 1));         // Segunda Condição: Compara se a carta que se pretende empilhar é uma unidade mais baixo que a do topo da pilha.

            if (condition1 && condition2)                                               // Caso as duas contições estejam satisfeitas, retorna verdadeiro.
                return true;
            else                                                                        // Caso contrário, retorna falso
                return false;
        }

        return false;                                                                    // Caso não satisfeita nenhuma das condições acima, retorna falso. 

    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------    
}
