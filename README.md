# Autômato com pilha

Implementação de um autômato com pilha em C# (.NET Core). Para executar este projeto, é necessário ter instalado o [.NET Core Runtime](https://dotnet.microsoft.com/download) (versão mínima 3.1).

## Instruções

1. Clone o repositório;
2. _(Opcional)_ Execute os testes unitários:
```
$ dotnet test
```
3. Navegue até o projeto do atuômato e execute o programa passando o arquivo do autômato como parâmetro, e.g.:
```
$ cd src/PushdownAutomata
$ dotnet run ../../examples/AnBn.json
```

## Arquivo de entrada

O arquivo de entrada para essa implementação é um arquivo JSON, contendo o autômato com pilha e as cadeias a serem testadas.

### Observações
Exemplos de arquivos de entrada estão contidos na pasta `examples`.

Para facilitar a inserção dos dados, foi criado um _[schema](docs/schema.json)_ para o arquivo, se for editar ou criar algum autômato personalizado, adicione a linha abaixo no objeto raiz, assim saberá se algum valor estiver errado ou faltando:

```
"$schema": "https://raw.githubusercontent.com/victor-borges/pushdown-automation/master/docs/schema.json"
```

### Estrutura do arquivo

O arquivo é composto por um objeto raiz, que por sua vez é dividido em dois objetos, `pushdown_automata` e `inputs`.

#### `pushdown_automata`

Composto por:

Nome | Tipo | Descrição
---- | ---- | ---------
`name` | `string` | Nome amigável do autômato.
`description` | `string` | Descrição do autômato.
`input_alphabet` | `string[]` | Alfabeto da linguagem. O valor `null` deve ser explicitamente adicionado para que o autômato reconheça a palavra vazia (`λ`).
`stack_alphabet` | `string[]` | Alfabeto da pilha.
`states` | `int[]` | Conjunto de possíveis estados do autômato.
`initial_state` | `int` | Estado inicial do autômato. Deve estar contido no conjunto de de estados.
`initial_stack_symbol` | `string` | Símbolo inicial da pilha. Deve estar contido no alfabeto da pilha.
`transition_rules` | `object[]` | Conjunto de regras de transição.

##### `transition_rule`

Composto por:

Nome | Tipo | Descrição |
---- | ---- | --------- |
`from_state` | `int` | Estado em que essa regra se aplica.
`to_state` | `int` | Estado para o qual o autômato transitará quando a regra for aplicada.
`input` | `string` | Símbolo que deve ser lido da cadeia de entrada.
`stack` | `string` | Símbolo que deve ser lido do topo da pilha.
`replace` | `string` | Simbolos que substituirão o topo da pilha quando a regra for aplicada.

### `inputs`

_Array_ de _strings_ que serão testadas pelo autômato.
