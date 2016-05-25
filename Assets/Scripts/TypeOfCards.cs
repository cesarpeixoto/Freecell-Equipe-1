/* ==============================================================================================================
Laboratório de Algorítimos e Estrutura de Dados
Curso: Tecnologia em Jogos Digitais - 2o Semestre - 2016
Professora: Daniele Junqueira Frosoni
//---------------------------------------------------------------------------------------------------------------

Classe que contem o padrão de cartas que será utilizado para criação de instâncias dos GameObjects que irão
representar as cartas no jogo.

Observações:
Optamos pela utilização de Enum para termos uma representação verbalizada nos naipes no código, e ainda, tirar 
priveito das operações matemáticas para identificar a cor da carta pelo naite: (pretas - pares, verlhas ímpares).

Referências e Documentação:
https://msdn.microsoft.com/pt-br/library/sbbt4032.aspx
https://en.wikipedia.org/wiki/Suit_(cards)

//---------------------------------------------------------------------------------------------------------------

$Creator: Cesar Peixoto $
$Notice: (C) Copyright 2016 by Cesar Peixoto. All Rights Reserved. $     Finalizado em 17/05/2016
=============================================================================================================== */

using UnityEngine;
using System.Collections;

[System.Serializable]
public class TypeOfCards
{
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    // Atributos da Classe

    public enum CardSuit { Diamonds = 1, Spades, Hearts, Clubs}                    // Enumeração dos naipes das cartas.
    public CardSuit suit;                                                           // Naipe da carta.
    public int value;                                                               // Valor da carta.
    public Sprite image;                                                            // Imagem que a carta exibirá.

    //-------------------------------------------------------------------------------------------------------------------------------------------------
}
