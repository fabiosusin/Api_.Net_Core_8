# Streamberry Movie Archive Management System

## Contexto
A Streamberry é uma empresa inovadora no ramo do entretenimento que busca revolucionar a forma como os filmes são produzidos. Determinada a criar algo único, a equipe da Streamberry deseja desenvolver um sistema de gestão de acervo de filmes que servirá como uma valiosa base de dados para a produção de novas obras cinematográficas.

## Desafio
Desenvolver uma API de gestão de acervo de filmes para a Streamberry com os seguintes critérios:

- **Disponibilidade em Streamings**: Os filmes podem estar disponíveis em vários streamings.
- **Agrupamento por Gênero**: Os filmes são agrupados por gênero.
- **Avaliação de Usuários**: Os filmes podem ser avaliados pelos usuários, com uma classificação de 1 a 5, podendo receber comentários.
- **Identificação por Títulos**: Os filmes são diferenciados através dos títulos.
- **Data de Lançamento**: Os filmes possuem mês e ano de lançamento.

## Funcionalidades Principais

Para auxiliar na produção de novas obras, a API permite:

1. **Informações sobre a Disponibilidade**:
   - Quantos streamings um filme está disponível?
   
2. **Avaliações**:
   - Qual a média de avaliação de cada filme?
   
3. **Lançamentos por Ano**:
   - Quantos filmes e quais foram lançados em cada ano?
   
4. **Localização de Filmes**:
   - Localizar filmes conforme avaliação e seus respectivos comentários.
   
5. **Avaliações Agrupadas por Gênero**:
   - Quais são as avaliações médias de filmes agrupados por gênero conforme a época de lançamento.

## Critérios Obrigatórios

- **CRUD**:
  - Criar e Atualizar (levando em consideração as validações de entrada)
  - Listar (com paginação)
  - Deletar
  - Pesquisar
  
- **Banco de Dados**:
  - Banco de dados a escolha.

## Diferenciais

- **Teste Unitário**:
  - Implementação de testes unitários (não obrigatório, mas considerado um diferencial).

## Tecnologias Utilizadas

- **Linguagem**: C#
- **Framework**: .NET Core 8
- **Banco de Dados**: MongoDB

## Pré-requisitos

- .NET Core 8 SDK
- MongoDB
- Visual Studio ou outro IDE compatível
