# Desafio Hyperativa

## Ponderações pertinentes 
* DESAFIO-HYPERATIVA possui um padrão que não pode ser aceito no sistema
* PadraoCerto é o padrão aceito pelo sistema.
* O desafio envolve uma série de número de cartões onde precisa receber um id único.
* O txt possui itens duplicados.
* O txt possui itens que não correspondem a cartão com numeração de 16 caracteres.
##
## Este repositório contém uma API desenvolvida em .NET Core 7 que possibilita o cadastro e consulta de números de cartão. A API é segura e eficiente, permitindo a inserção de dados de cartões individualmente ou por meio de arquivos TXT.
## Executando o Projeto

1. Clone o repositório para a sua máquina.
2. Certifique-se de ter o .NET Core SDK instalado (versão 7).
3. Configure a conexão com o banco de dados Postgres no arquivo `appsettings.json`.
4. Abra um terminal na pasta do projeto e execute: Update-DataBase

# Endpoints
Autenticação
Os endpoints da API requerem autenticação via token JWT. Para obter o token, faça uma requisição POST para/Token/Authorize.

# Inserção de Dados
* POST /Cartao/InserirCartao - Insere um cartão individual.
* POST /Cartao/InserirCriptografadoo - Insere um cartão individual criptografado.

# Consulta de Dados
* GET /cartao/{numeroCartao} - Consulta a existência de um número de cartão na base de dados.
* GET /cartao/criptografado/{numeroCartao} - Consulta a existência de um número de cartão criptografado na base de dados.

# Inserção de dados TXT
* POST /InserirArquivo/ProcessarArquivo - Insere dados de cartões a partir de um arquivo TXT.
* GET /InserirArquivo/VerificarStatus/{guid} - Consulta o status do processamento de um arquivo utilizando o GUID fornecido.


# Requisitos Obrigatórios
* A API registra todas as requisições e seus retornos em logs usando a biblioteca Serilog.
* A autenticação do usuário é feita com JWT.
* Os dados dos cartões são armazenados de forma segura no banco de dados.

# Requisitos Opcionais
* Foram implementados testes unitários para garantir a qualidade do código.
* A criptografia de ponta a ponta é usada para o tráfego de informações sensíveis.


# Detalhes Relevantes
* A API é construída em um ambiente scoped, o que evita vazamentos de recursos.
* O método de inserção através de arquivo TXT é executado de forma assíncrona.

# Banco de Dados
* Foi utilizado o banco de dados Postgres para armazenar os dados.
* O número do cartão é um valor único no banco de dados.


# Logs
* A biblioteca Serilog foi usada para salvar os logs.
* Os logs são salvos em arquivos TXT na pasta Logs na raiz do projeto.

# Considerações Finais
Este projeto atende ao Desafio Hyperativa, proporcionando uma API segura e escalável para o cadastro e consulta de números de cartão. Sua eficiência e medidas de segurança garantem a integridade dos dados e a satisfação do usuário.
