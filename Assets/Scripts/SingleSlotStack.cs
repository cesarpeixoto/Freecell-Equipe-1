/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe responsável pelas regras da pilhas de slots individuais (celulas livres).

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
public class SingleSlotStack : AbstractStack 
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Construtor da classe.

    public SingleSlotStack(TypeOfStack type)
    {
        this.type = TypeOfStack.SingleSlot;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Método virtual sobrescrito, que faz a validação conforme as regras da pilha de slots individuais

    public override bool Validate(Stack<GameObject> externalStack)
    {
        if (externalStack.Count > 1)                            // Espaço só recebe uma carta
            return false;

        if (stack.Count > 0)                                    // Espaço já contem uma carta
            return false;

        return true;                                            // Se chegar até aqui, é verdadeiro
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
}
