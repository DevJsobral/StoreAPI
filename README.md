
# StoreAPI

![StoreAPI Logo](https://via.placeholder.com/150)  
*Projeto simples de API de loja com frontend Angular para demonstração e testes.*

---

## Descrição

StoreAPI é um projeto para gerenciar produtos, categorias e pedidos. A API foi construída com ASP.NET Core 8.0 e o frontend com Angular 19.  
O foco principal é a funcionalidade da API, enquanto o frontend serve para facilitar testes e interação.

---

## Tecnologias utilizadas

- **Backend:** ASP.NET Core 8.0, C#, JWT para autenticação, MySQL (Pomelo)
- **Frontend:** Angular 19, TypeScript, Bootstrap
- **Autenticação:** JWT com usuário admin pré-configurado no `appsettings.json`
- **Upload de imagens:** via serviço de upload (API ou serviços externos)

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js (v18+)](https://nodejs.org/)
- Angular CLI (opcional para rodar frontend localmente):  
  ```bash
  npm install -g @angular/cli
  ```
- MySQL ou MariaDB rodando localmente (ou configure a string de conexão no `appsettings.json`)

---

## Rodando a API (TrainingAPI)

1. Clone o repositório e entre na pasta da API:  
   ```bash
   cd StoreAPI/TrainingAPI
   ```

2. Restaure as dependências e compile:  
   ```bash
   dotnet restore
   dotnet build
   ```

3. Configure sua string de conexão no arquivo `appsettings.json` para seu banco MySQL.

4. Aplique as migrações para criar o banco de dados:  
   ```bash
   dotnet ef database update
   ```

5. Execute a API:  
   ```bash
   dotnet run
   ```

6. A API estará disponível em:  
   - `https://localhost:5001` (HTTPS)  
   - `http://localhost:5000` (HTTP)

---

## Rodando o Frontend (Angular)

1. Acesse a pasta do frontend:  
   ```bash
   cd StoreAPI/frontend
   ```

2. Instale as dependências:  
   ```bash
   npm install
   ```

3. Inicie o servidor de desenvolvimento:  
   ```bash
   ng serve --open
   ```

4. O frontend abrirá automaticamente no navegador em `http://localhost:4200`.

---

## Configurações importantes

- **Usuário Admin**  
  O login e senha do usuário admin estão configurados no `appsettings.json` da API para facilitar testes de rotas protegidas.  
  Use essas credenciais para autenticar e obter o token JWT.

- **Autenticação JWT**  
  Rotas que alteram dados (POST, PUT, PATCH, DELETE) exigem token JWT válido no header `Authorization: Bearer <token>`.

---

## Testando rotas protegidas

- Faça login com o usuário admin no frontend para obter o token JWT.
- O frontend utiliza automaticamente o token para chamadas protegidas.
- Você pode testar as rotas protegidas também via ferramentas como Postman, incluindo o token JWT no header.

---

## Deploy

Para produção, recomendo os seguintes passos:

- Gere o build do frontend:  
  ```bash
  ng build --prod
  ```  
  Isso gera a pasta `dist/` com os arquivos estáticos.

- Hospede o backend em um serviço cloud (Azure, AWS, Heroku, DigitalOcean, etc.)

- Hospede o frontend em serviços para SPAs estáticas, como:  
  - GitHub Pages  
  - Netlify  
  - Vercel

---

## Contribuição

Contribuições são muito bem-vindas!  
Sinta-se à vontade para abrir issues ou pull requests para melhorias, correções ou novas funcionalidades.

---

## Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

Se precisar de ajuda ou quiser mais informações, abra uma issue!

---

*Desenvolvido por João Sobral*
