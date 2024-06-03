# Configuração do Sistema MVC de Controle de Temperatura

Para que o funcionamento do sistema MVC de controle de temperatura funcione é necessário que seja feito:

Uma adição no repositório base do projeto de um arquivo `config.json` com o seguinte formato:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=<IP de onde está localizado o banco de dados>; Database=<Nome da database a ser utilizada, já existente no banco>; user id=<Usuário a ser logado na autenticação SQL Server>; password=<Senha para autenticação do Usuário>"
    },
    "Host" : "<host onde está localizado o FIWARE>"
}
```

## Exemplo de `config.json`

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=192.168.1.100; Database=ControleTemperatura; user id=admin; password=senha123"
    },
    "Host" : "192.168.1.100"
}
```

Para configuração de host do FIWARE e string de conexão do banco de dados.
