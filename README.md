# PBL-GN3

Este repositório contém toda a aplicação relacionada ao projeto de PBL (Projeto
Baseado em Problemas) do 5º período do primeiro semestre de 2024 da Faculdade 
Engenheiro Salvador Arena.

O objetivo do projeto é obter os dados de um kit de temperatura por meio da 
tensão de saída, que é lida pela porta analógica do ESP32. O ESP32 envia as 
informações via Wi-Fi para o Fiware, que se comunica com uma interface ASP.NET 
MVC, onde é possível gerir os dispositivos e acompanhar as informações via 
dashboard.

## Estrutura do Repositório

- **Docker/**: Configurações e scripts para configurar o ambiente Docker.
- **Postman/**: Coleções e ambientes para testes com Postman.
- **sketch/**: Códigos e exemplos de sketch para o projeto.

## Como Usar

1. **Clone o Repositório:**

   ```sh
   git clone https://github.com/FESA-EC5-S1-2024/PBL-Testes.git
   cd PBL-Testes
   ```

   &nbsp;

2. **Configurando o Docker:**

   Edite o arquivo .env com as seguintes configurações:

   - **MONGO_USER:** Usuário do MongoDB que será criado.
   - **MONGO_PWD:** Senha do usuário do MongoDB.
   - **MSSQL_PWD:** Senha do SQL Server (o usuário padrão é sa).
   - Portas externas de cada componente do Fiware podem ser alteradas conforme 
   necessário.

   &nbsp;

3. **Subir os Contêineres:**

   ```sh
   docker-compose up -d
   ```

   &nbsp;

4. **Configurar Postman:**

   Na pasta Postman, você encontrará a _collection_ e os _environment_. O 
   _environment_ têm as portas padrão definidas no arquivo .env do Docker.
   Se alterar essas portas, também precisará atualizá-las aqui.

   - A propriedade **host** é onde o Fiware está rodando (servidor) e deve ser 
   definida, pois vem zerada.

   &nbsp;

5. **Provisionando os recursos do Fiware:**

   Após definir as variávis no _environment_, devemos provisionar alguns 
   recursos para que ESP32 se comunique e interaja com o Fiware.

   - **Provisioning a Service Group for MQTT:** tópico 2 na parte do 
   "IOT AGENT MQTT".
   - **Provisioning a Temperature Sensor:** tópico 3, também no 
   "IOT AGENT MQTT".
   - **Subscribe Temperature:** tópico 2 na parte do "STH-COMET".

   &nbsp;
   
   Os outros itens são exemplos de requests que podemos fazer para os serviços 
   do Fiware.

## Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull 
requests.

## Licença

Este projeto é licenciado sob a BSD 3-Clause [License](./LICENSE).
