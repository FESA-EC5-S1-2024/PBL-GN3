# PBL-Grupo6

Este repositório contém toda a aplicação relacionada ao projeto de PBL (Projeto
Baseado em Problemas) do 5º período do primeiro semestre de 2024 da Faculdade
Engenheiro Salvador Arena.

## Índice

1. [Visão Geral do Projeto e Objetivos](#visão-geral-do-projeto-e-objetivos)
2. [Funcionalidades da interface web](#funcionalidades-da-interface-web)
3. [Estrutura do Repositório](#estrutura-do-repositório)
4. [Como Usar](#como-usar)
   - [Requisitos](#requisitos)
   1. [Clonando o Repositório](#1-clonando-o-repositório)
   2. [Configurando o Docker](#2-configurando-o-docker)
   3. [Subindo os Contêineres](#3-subindo-os-contêineres)
   4. [Configurando o Postman](#4-configurando-o-postman)
   5. [Provisionando os Recursos do Fiware](#5-provisionando-os-recursos-do-fiware)
   6. [Configurando o Sketch e a Arduino IDE](#6-configurando-o-sketch-e-a-arduino-ide)
   7. [Compilando Para o ESP32](#7-compilando-para-o-esp32)
5. [Contribuições](#contribuições)
6. [Licença](#licença)

## Visão Geral do Projeto e Objetivos

Este é um projeto integrado que utiliza um sistema de controle e monitoramento
de IoT para estufas de secagem de motores elétricos, a fim de assegurar os
valores de temperatura e realizar um monitoramento em tempo real, otimizando o
processo de fabricação e resultando em alta qualidade dos produtos. Por meio
deste programa, é possível realizar a consulta e apuração dos dados de
temperatura captados pelos sistemas de controle IoT apresentados em gráficos.

O objetivo do projeto é obter os dados de um kit de temperatura por meio da
tensão de saída, que é lida pela porta analógica do ESP32. O ESP32 envia as
informações via Wi-Fi para o Fiware, que se comunica com uma interface ASP.NET
MVC, onde é possível gerir os dispositivos e acompanhar as informações via
dashboard.

## Funcionalidades da Interface Web

O programa desktop possui em sua tela inicial um sistema de exibição que
apresenta um gráfico com os últimos 100 valores captados pelo sistema de
controle IoT. Ao efetuar login no sistema, é possível registrar novos aparelhos
conectados ao programa. Também conta com um sistema de gerenciamento de usuários
e aparelhos, acessível e controlado apenas por usuários administradores.

## Estrutura do Repositório

- [**Docker/**](./Docker/README.md): Configurações e scripts para configurar o
  ambiente Docker.
- [**Postman/**](./Postman/README.md): Coleções e ambientes para testes com
  Postman.
- [**Sketch/**](./Sketch/README.md): Códigos e exemplos de sketch para o
  projeto.
- [**MVC/**](./MVC/README.md): Códigos da interface web.

## Como Usar

### Requisitos

Para executar o sistema, você precisará de:

- Docker e Docker Compose instalados em uma máquina ARM64/V8.
- Postman para realizar testes de API.
- Arduino IDE para programar o ESP32.
- Bibliotecas Arduino: `PubSubClient` e `WiFiManager`.

#### 1 Clonando o Repositório

```sh
git clone https://github.com/FESA-EC5-S1-2024/PBL-Testes.git
```

&nbsp;

#### 2 Configurando o Docker

Edite o arquivo .env com as seguintes configurações:

- `MONGO_USER`: Usuário do MongoDB que será criado.
- `MONGO_PWD`: Senha do usuário do MongoDB.
- `MSSQL_PWD`: Senha do SQL Server (o usuário padrão é sa).
- Portas externas de cada componente do Fiware podem ser alteradas conforme
  necessário.

&nbsp;

#### 3 Subindo os Contêineres

```sh
docker-compose up -d
```

&nbsp;

#### 4 Configurando o Postman

Na pasta Postman, você encontrará a _collection_ e os _environment_. O
_environment_ têm as portas padrão definidas no arquivo .env do Docker.
Se alterar essas portas, também precisará atualizá-las aqui.

- A propriedade `host` é onde o Fiware está rodando (servidor) e deve ser
  definida, pois vem zerada.

&nbsp;

#### 5 Provisionando os Recursos do Fiware

Após definir as variávis no _environment_, devemos provisionar alguns
recursos para que ESP32 se comunique e interaja com o Fiware.

- `Provisioning a Service Group for MQTT` (tópico 2 na parte do
  "IOT AGENT MQTT").
- `Provisioning a Temperature Sensor` (tópico 3, também no
  "IOT AGENT MQTT").
- `Subscribe Temperature` (tópico 2 na parte do "STH-COMET").

&nbsp;

Os outros itens são exemplos de requests que podemos fazer para os serviços
do Fiware.

&nbsp;

#### 6 Configurando o Sketch e a Arduino IDE

Baixe e instale as seguintes bibliotecas na Arduino IDE:

- `PubSubClient` (versão testada v2.8)
- `WiFiManager` (versão testada v2.0.17)

&nbsp;

Navegue até sketch/include e edite o arquivo config.hpp.

- Altere a definição de `BROKER_MQTT` para o endereço do seu broker MQTT.

&nbsp;

#### 7 Compilando Para o ESP32

Para compilar o código para a placa ESP32 estamos uando as seguintes
configurações:

- `Board`: DOIT ESP32 DEVKIT V1.
- `Upload Speed`: 115200.
- `Flash Frequency`: 80MHz.
- `Programador`: Esptool.

&nbsp;

## Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull
requests.

## Licença

Este projeto é licenciado sob a BSD 3-Clause [License](./LICENSE).
