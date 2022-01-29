# Digital Innovation One - Prática .NET

## Abstraindo um Jogo de RPG Usando Orientação a Objetos com C#

Projeto desenvolvido durante o bootcamp GFT Start #3 .NET na plataforma DIO com o intuito de aplicar conceitos de POO e padrões de desenvolvimento .NET.
Consiste em na abstração e implementação básica de um jogo simples de RPG com gráficos baseados em texto, desenvolvidos sem bibliotecas externas.

A idéia do jogo consiste em um dungeon crawler básico, onde o jogador controla um party de até 4 heróis de diferentes classes, batalhando em turnos contra inimigos de dificuldade incremental.

O projeto no momento possui apenas classes que exemplifiquem o conceito do jogo, sem lógica de progresso e com a maioria das features não implementadas. Consiste atualmente de apenas uma fase, com heróis e inimigos pré-definidos.

## Preview
Preview do projeto:
## ![preview_gif](./preview.gif)

## Planos futuros
Pretende-se implementar:
- Novas classes de heróis e inimigos, com atributos específicos e habilidades únicas
- Sistema de progressão baseado em RNG
- Diferentes "seções" da dungeon, com elementos gráficos e bosses
- Itens consumíveis e habilidades com efeitos temporários

Contribuições são bem vindas!

## Rodando o projeto

Para iniciar o projeto:
### `dotnet run`

O projeto roda no console, e a janela deve possuir exatament 120 colunas e no mínimo 50 linhas para a impressão adequada.

Caso receba uma mensagem de erro sobre as configurações do console, tente diminuir a fonte (`Properties > Font`, no CMD).

Caso o seu sistema operacional não seja o Windows, não será possível redimensionar o console automaticamente. As dimensões deverão ser configuradas manualmente:
### `$ resize -s 50 120`


## Créditos da mentoria:

Felipe Aguiar: https://www.linkedin.com/in/felipe-aguiar-047/
