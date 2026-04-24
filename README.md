# Stock Quote Alert

Monitor contínuo de cotações da B3 com alertas por e-mail quando o preço de um ativo ultrapassa os limites configurados:

- Envia um e-mail aconselhando a **VENDA** quando o preço sobe acima do limite superior
- Envia um e-mail aconselhando a **COMPRA** quando o preço cai abaixo do limite inferior

Alertas duplicados são suprimidos — um novo e-mail só é enviado quando o estado muda (ex: de venda para compra, ou vice-versa).

## API de cotações

O projeto utiliza a **[brapi.dev](https://brapi.dev)** — API brasileira gratuita com cobertura de todos os ativos da B3 e criptoativos.

## Arquitetura

```
StockQuoteAlert/
├── Arguments.cs                  # Parsing e validação dos argumentos CLI
├── Program.cs                    # Entry point, configuração e loop principal
├── Models/
│   ├── BrApiResponseModel.cs     # Modelo de cotação retornado pela brapi
│   └── AlertType.cs              # Enum: Buy | Sell
├── Services/
│   ├── BrApiService.cs           # Integração com brapi.dev
│   └── EmailService.cs           # Envio de e-mail via SMTP (MailKit)
├── Settings/
│   └── ApplicationSettings.cs    # Modelagem das configurações do appsettings.json
└── Templates/
    └── EmailTemplate.cs          # Template HTML do e-mail (gerado com IA)
```

## Instalação

```bash
git clone https://github.com/MaioranoB/StockQuoteAlert.git
cd StockQuoteAlert
dotnet restore
```

### Configuração


Preencha o arquivo [appsettings.json](./appsettings.json) na raiz do repositório com suas configurações de e-mail e SMTP.

No projeto `StockQuoteAlert`, inicialize o arquivo de [User Secrets do .NET](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=linux#enable-secret-storage)
e adicione a ele a seguinte estrutura:

```json
{
  "SMTP:Password": "",
  "BRAPI:Key": ""
}
```

## Uso

```bash
dotnet run --project StockQuoteAlert -- <TICKER> <PREÇO_VENDA> <PREÇO_COMPRA>
```

### Exemplos

```bash
# Monitorar PETR4
dotnet run --project StockQuoteAlert -- PETR4 22.67 22.59

# Monitorar Bitcoin
dotnet run --project StockQuoteAlert -- BTC-USD 77500 77000
```

## Transparência sobre uso de IA

Este `README` e o arquivo [EmailTemplate.cs](./StockQuoteAlert/Templates/EmailTemplate.cs) foram gerados com assistência direta do Claude (Anthropic). 
