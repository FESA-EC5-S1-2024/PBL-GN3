# Postman

Esta pasta contém todos os arquivos e explicações relacionadas a `collection` e 
ao `environment` criados para interagir com o Postman.

## Índice

1. [Importando a Coleção e o Ambiente](#importando-a-coleção-e-o-ambiente)
2. [Configurando as Variáveis de Ambiente](#configurando-as-variáveis-de-ambiente)
3. [Executando Requisições](#executando-requisições)

## Importando a Coleção e o Ambiente

1. Abra o Postman e acesse o menu "Import".
2. Selecione a opção "File" e escolha o arquivo 
`PBL-Grupo6.postman_collection.json` e o `PBL-Grupo6.postman_environment.json`.
3. Clique em "Import" para importar a coleção.

## Configurando as Variáveis de Ambiente

1. No Postman, acesse o menu "Collections" e selecione a coleção "PBL-Grupo6".
2. Clique na guia "Environments".
3. Edite as variáveis de ambiente conforme necessário:
    - `host`: Insira o endereço do servidor Fiware.
    - `IOT-AGENT_PORT`: porta padrão 4041.
    - `ORION_PORT`: porta padrão 1026.
    - `STH-COMET_PORT`: porta padrão 8666.
    - `entity_name`: ID de temperatura padrão `urn:ngsi-ld:Temp:003`.
    - `device_id`: ID do dispositivo padrão `temp003`.

Caso tenha mudado as portas quando subiu o Docker, será necessário mudar as 
portas para interagir com o Fiware usando o Postman.

O valor 003 do `entity_name` e `device_id` tem a ver com a gôndola que 
utilizamos no desenvolvimento do projeto, esses valores podem ser trocados 
também, mas para funcionar perfeitamente com o ESP32, você terá que alterar o 
arquivo `config.hpp` que contém as variáveis `TOPICO_SUBSCRIBE`, 
`TOPICO_PUBLISH` e `topicPrefix` que são baseadas nesses números.

## Executando Requisições

1. Na coleção "PBL-Grupo6", selecione a pasta que contém a requisição que você 
deseja executar (por exemplo, "IOT Agente MQTT", "STH-Comet" ou "Orion").
2. Selecione a requisição desejada.
3. Clique no botão "Send" para executar a requisição.
4. Observe os resultados da requisição na guia "Response".

Os recursos necessários para o funcionamento e a conexão com o dispositivo são 
descritos na documentação principal do projeto no tópico `Como Usar` itens 4 e 5.
