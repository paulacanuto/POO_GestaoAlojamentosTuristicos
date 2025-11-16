# 🏨 Gestão de Alojamentos Turísticos

Projeto em desenvolvimento, realizado pela a aluna **Ana Paula Canuto da Silva**,  em linguagem **C#** para a disciplina de **Programação Orientada a Objetos (POO)** da Licenciatura de Sistemas Informáticos no **Instituto Politécnico do Cávado e Ave - IPCA**, com o objetivo de gerenciar informações de clientes, alojamentos e suas classificações.  
O projeto foi estruturado para permitir fácil expansão futura, incluindo reservas, faturamento e interface gráfica (Windows Forms).

---

## Funcionalidades Atuais

### **Classe Cliente**
- Armazena informações básicas do cliente: **Id**, **Nome**, **Email**  
- Método **`ValidarEmail()`** para verificação simples do formato do email.

### **Classe Alojamento (base)**
- Contém dados gerais do alojamento: **Id**, **Endereço**, **PreçoPorNoite**
- Método **`CalcularTaxaServico()`** calcula 10% de taxa sobre o valor da estadia.
- Getters públicos: `GetId()`, `GetEndereco()`, `GetPrecoPorNoite()`
- Construtor protegido permitindo herança.

### **Classe Hotel (herda de Alojamento)**
- Atributo extra: **NumeroEstrelas**
- Método **`ClassificarHotel()`** que retorna: *Luxo*, *Conforto* ou *Standard*
- Getter público **GetNumeroEstrelas()**

---

## Arquitetura do Projeto

O projeto segue os pilares da Programação Orientada a Objetos:

- **Encapsulamento:** atributos privados com acesso controlado.
- **Herança:** *Hotel* deriva de *Alojamento*.
- **Coesão:** cada classe tem função específica.
- **Extensível:** fácil criar novas subclasses (Hostel, Apartamento, etc.)

### Diagrama UML (resumo das 3 classes criadas)


Alojamento
|
└── Hotel

Cliente

## Requisitos

- .NET Framework ou **.NET 6/7**
- **Visual Studio** ou **Visual Studio Code**
- Noções básicas de **C#** e **POO**

---

## Como Executar

### 1. Clone o repositório:
``bash
git clone <URL_DO_REPOSITORIO>

2. Abra no Visual Studio ou VS Code
3. Compile e execute a aplicação
4. Teste no Program.cs ou integre com Forms

