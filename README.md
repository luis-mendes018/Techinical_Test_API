<h1>Visão geral do Projeto 👩🏿‍💻🚀</h1>

<p>Esse projeto coniste numa API que consome um outro serviço que pertence a OpenWeatherMap, que possui uma API que retorna dados meteorológicos de um determinado lugar, 
retornando informações relevantes como temperatura mínima, temperatura máxima, descrição do clima e etc...</p> 

<h4>Fluxo geral resumido do funcionamento do sistema:</h4>
<ol>
  <li> O usuário insere o nome do lugar que pretende obter as informações climáticas;</li>
  <br>
  <li> A API dispara uma requisição para a API da OpenWeatherMap retornando os dados climáticos do lugar; </li>
  <br>
  <li>O sistema processa as informações retornadas da API externa e as salva automáticamente no banco de dados; </li>
</ol>


Veja as imagens abaixo 👇🏿

Ilustração do passo 1: Inserindo o nome do lugar

<img width="1844" height="739" alt="image" src="https://github.com/user-attachments/assets/bb398d19-b905-4b54-a1bd-eb7d34cdf443" />
<br>
<br>
<br>

Ilustração do passo 2: Retornando os dados do lugar

<img width="1813" height="605" alt="image" src="https://github.com/user-attachments/assets/0b91c8c5-8f78-4dd8-8563-50a0bb6a9630" />
Obs: Note que ao retornar os dados, as propriedades id e city retornam null, o que é normal, pois elas serão usadas em outro cenário
na hora de consultar as informações no banco de dados.

<br>
<br>
<br>

Ilustração do passo 3: O usuário obtém as informações climáticas do lugar no banco de dados:

<img width="931" height="751" alt="image" src="https://github.com/user-attachments/assets/43ac5831-9f93-4e7d-81c1-5980f0806c98" />
<br>
Obs: Agora sim, é possível visualizar as propriedades City e Id preenchidas
<br>
<br>
Aqui, é usado o endpoint que consulta um determinado dado por id, mas a API também possui o endpoint que retorna todos os dados paginados:
<br>
<br>

<img width="1827" height="686" alt="image" src="https://github.com/user-attachments/assets/88789734-f48a-4631-8b4f-4cf304c61a9a" />

<br>
<img width="1814" height="858" alt="image" src="https://github.com/user-attachments/assets/18aee635-9bc2-4a7b-b98e-a91aead0e86f" />
<img width="1662" height="631" alt="image" src="https://github.com/user-attachments/assets/67258b3d-5676-4717-8bb1-e4f3f85357cb" />
<br>
<br>
<br>

<h2>Outras funcionalidades</h2>
<br>

<p>
  A API também conta com um sistema de autenticação e autorização usando jwt. 
  Para realizar todo o fluxo apresentado anteriormente, o usuário precisa estar logado.
  Sendo assim, caso o mesmo tente enviar uma requisição sem um token válido, receberá um erro 401(Unauthorized) conforme mostra
  a imagem abaixo:
</p>
<br>

<img width="1697" height="849" alt="image" src="https://github.com/user-attachments/assets/012e8378-b285-4b21-8c21-01fb71be6d44" />

<h3>Como realizar o login?</h3>

<p>
  A API é documentada com swagger que assim como o postman, oferece também suporte para autenticação. Veja as imagens abaixo com os endpoints de autenticação:
</p>

<br>
<img width="1894" height="571" alt="image" src="https://github.com/user-attachments/assets/ad9f3ff3-34c5-45b2-b5d4-be5621f2111b" />

<br>
<br>
<p>Basta fazer login, que será gerado um token. Ao gerar esse token, será necessário copia-lo e inseri-lo no cabeçalho do request via swagger</p>



<img width="1917" height="905" alt="image" src="https://github.com/user-attachments/assets/bea8d479-8e8a-4bfd-bad9-64d7dffb463a" />


<p>Essa é a visão geral do projeto.</p>
<br>
<br>
<br>
<br>
<h1>Instruções de configuração 👩🏿‍💻⚙️</h1>

<p>
  Para você que irá testar o projeto, é necessário alguns procedimentos para que ele rode corretamente sem bugs, sem erros e sem problemas. 
  Segue os procedimentos abaixo:
</p>
<br>
<br>
<h1>1. Ferramentas 🛠️</h1>
<br>
<br>
<p>
  Certifique-se de ter as 
  ferramentas Visual Studio IDE(ou VSCode caso já tenha o .NET 8 configurado nele ) e o SQL Server com a ferramenta SQL Server Management Studio
  instaladas na sua máquina. Caso não as tenha, segue os links abaixo, caso já as tenha, pode seguir direto para o próximo passo.

  - Visual Studio: https://visualstudio.microsoft.com/downloads/
    <br>
  - SQL Server: https://www.microsoft.com/pt-br/sql-server/sql-server-downloads
    <br>
  - SQL Server Managment Studio: https://learn.microsoft.com/en-us/ssms/install/install  
</p>
<br>
<br>
<h1>2. Clonagem do repositório e execução do projeto</h1>
<br>
<br>
<p>
  - Clone o repositório usando o comando git clone https://github.com/luis-mendes018/Techinical_Test_API.git
  
  - Abra o projeto com o visual studio
  - Vá até o arquivo appsettings.json e insira sua string de conexão conforma a estrutura do código abaixo 👇🏿

   "ConnectionStrings": {
  "DefaultConnection": "Server={nome_do_servidor};Database=WeatherDatabase;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;" // Substitua pela sua string de conexão.
},

- Execute o projeto e ao fazer isso, será gerado o banco de dados automaticamente com as suas respectivas tabelas, já que o projeto usa o dapper e eu o configurarei para isso via código.
</p>



