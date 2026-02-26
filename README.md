SqlClinicaUser

SqlClinicaUser é uma aplicação desktop desenvolvida em C# (Windows Forms) para o gerenciamento de usuários de clínicas médicas, implementando operações CRUD completas e persistência de dados em banco de dados MySQL.

O projeto utiliza separação entre a camada de interface (UI) e a camada de acesso a dados (Data Access), com comunicação ao banco realizada via ADO.NET, facilitando a manutenção e organização do código.

Funcionalidades principais

Cadastro de usuários (nome, CPF, telefone, e-mail, endereço e função/permissão).

Edição e exclusão de registros.

Pesquisa e listagem de usuários com filtros básicos.

Persistência de dados utilizando MySQL.

Interface desktop baseada em Windows Forms, voltada para uso local em clínicas de pequeno porte.

Tecnologias utilizadas

Linguagem: C#

Plataforma: .NET (Windows Forms)

Banco de dados: MySQL

Acesso a dados: ADO.NET

Pacote principal: MySql.Data

Pré-requisitos

Sistema operacional Windows.

Visual Studio com suporte a projetos Windows Forms.

.NET Framework compatível com a versão do projeto.

MySQL Server ou MariaDB instalado.

Configuração do banco de dados

A aplicação utiliza uma string de conexão definida no arquivo App.config.
Exemplo de configuração:

<connectionStrings>
  <add name="ConexaoMySql"
       connectionString="Server=localhost;Database=clinica;Uid=usuario;Pwd=senha;" />
</connectionStrings>

É necessário criar previamente o banco de dados e as tabelas utilizadas pela aplicação antes da execução.

Execução do projeto

Abra a solução no Visual Studio.

Restaure os pacotes NuGet, se necessário.

Defina o projeto como inicialização.

Execute em modo Debug ou gere o build em modo Release para distribuição.

Uso

A aplicação disponibiliza formulários para cadastro e gerenciamento de usuários.

Os campos possuem validações conforme as regras implementadas no sistema (como CPF e e-mail).

A funcionalidade de pesquisa permite localizar registros de forma rápida.

Estrutura do projeto

SqlClinicaUser.sln — arquivo de solução do Visual Studio.

SqlClinicaUser/ — projeto principal contendo código-fonte, formulários e configurações.

packages/ — pacotes NuGet utilizados.

bin/ e obj/ — diretórios gerados durante a compilação.

Observações

As credenciais de acesso ao banco devem ser mantidas fora de repositórios públicos em ambientes reais.

Recomenda-se manter rotinas de backup do banco de dados.

O projeto não possui testes automatizados implementados.

Licença

Projeto disponibilizado para fins educacionais e de estudo.
