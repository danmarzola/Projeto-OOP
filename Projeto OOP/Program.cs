using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Projeto_OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            //MENU
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }
        public static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Escolha uma das Funções do Sistema de Gerenciamente de Restaurante em C# do Danilo Marzola Cunha");
            Console.WriteLine("(1) Abrir um comanda vazia e alocar em uma mesa vazia");
            Console.WriteLine("(2) Lançar um item em uma comanda");
            Console.WriteLine("(3) Excluir um item de uma comanda");
            Console.WriteLine("(4) Fechar uma comanda e gerar a nota fiscal no arquivo de texto");
            Console.WriteLine("(5) Mostrar Cardapio");
            Console.WriteLine("(6) Mostrar Mesas Ocupadas");
            Console.WriteLine("(7) Mostrar Mesas Desocupadas");
            Console.WriteLine("(8) Mostrar Comandas Abertas");
            Console.WriteLine("(9) Mostrar Fornecedores");
            Console.WriteLine("(10) Mostrar Produtos dos Fornecedores");
            Console.WriteLine("(11) Alterar Estoque de um item");
            Console.WriteLine("(12) Limpar Console");
            Console.WriteLine("(13) Encerrar Terminal");
            Console.Write("\r\nSelecione uma das opções usando um numero inteiro:");

            switch (Console.ReadLine())
            {
                case "1":
                    AbrirComanda();
                    return true;
                case "2":
                    AdicionarItem();
                    return true;
                case "3":
                    ExcluirItem();
                    return true;
                case "4":
                    FecharComanda();
                    return true;
                case "5":
                    MostrarCardapio();
                    return true;
                case "6":
                    MostrarMesasOcupada();
                    return true;
                case "7":
                    MostrarMesasDesocupadas();
                    return true;
                case "8":
                    MostrarComandasAbertas();
                    return true;
                case "9":
                    MostrarFornecedores();
                    return true;
                case "10":
                    MostrarProdutosDosFornecedores();
                    return true;
                case "11":
                    AlterarEstoque();
                    return true;            
                case "12":
                    Console.Clear();
                    return true;
                case "13":
                    return false;
                default:
                    return true;
            }
            void AbrirComanda()
            {
                //Inicia Pegando as Lista de Comandas,Mesas Atuais dos Arquivos de TXT
                List<Comanda> ListaComandas = Service.GetComandas();
                List<Mesa> ListaMesas = Service.GetMesas();
                //Para evitar alocar comandas a mesas que estão em uso criamos uma nova lista vazia de inteiro que representam os ID's das mesas ocupadas
                List<int> MesasOcupadas = new List<int>();
                if (ListaComandas == null)
                {
                    ListaComandas = new List<Comanda>();
                }
                foreach(Comanda p in ListaComandas)
                {
                    if(p.Estado == "OPEN")
                    {
                        //Adicionamos todas as mesas com Estado = OPEN na lista de mesas ocupadas
                        MesasOcupadas.Add(p.IDmesa);
                    }
                }
                //Iniciamos uma lista vazia de Produtos já que a comanda acabou de ser aberta
                List<Produto> vazia = new List<Produto>();
                Console.WriteLine("Digite no seguinte formato as informações necessarias para abrir a comanda: ");
                Console.WriteLine("Formato: Atendente, Cliente, ID da Mesa");
                String[] strlist = Console.ReadLine().Split(',');
                //Condição de Proteção
                //Checar se a mesa esta ocupada
                if (MesasOcupadas.Contains(Int32.Parse(strlist[2])))
                {
                    Console.Clear();
                    Console.WriteLine("Mesa já esta sendo utilizada em uma comanda em aberto!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                //Checar se o ID da mesa está dentro da lista
                }else if(ListaMesas.Count < Int32.Parse(strlist[2]))
                {
                    Console.Clear();
                    Console.WriteLine("Mesa não existe!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                //Criamos um novo elemento Comanda na lista
                ListaComandas.Add(new Comanda(ListaComandas.Count+1, strlist[0], strlist[1], Int32.Parse(strlist[2]), DateTime.Now.ToString("dd/MM/yyyy H:mm"), "OPEN", vazia));
                //Chamamos a Função do Service para operar os TXT
                Service.GravarComanda(ListaComandas);
                Console.Clear();
                Console.WriteLine("Comanda Criada com ID: "+(ListaComandas.Count) + " | Atendente: "+ strlist[0] + " | Cliente: " + strlist[1] + " | IDMesa: " + Int32.Parse(strlist[2]) + " | HorarioDeChegada: " + DateTime.Now.ToString("dd / MM / yyyy H: mm") + " | Estado: OPEN");
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();
            }
            void AdicionarItem() {
                List<Comanda> ListaComandas = Service.GetComandas();
                List<Produto> ListaProdutos = Service.GetProdutos();
                if (ListaComandas == null)
                {
                    Console.Clear();
                    Console.WriteLine("Não há comandas em aberto, tente criar uma primeiro!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Informe o ID da Comanda e o ID do Produto que deseja lançar");
                Console.WriteLine("Formato: IDComanda, IDProduto");
                String[] strlist = Console.ReadLine().Split(',');
                //Condição de Proteção
                if(ListaComandas.Count < Int32.Parse(strlist[0]))
                {
                    Console.Clear();
                    Console.WriteLine("ID não existe");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                else if (ListaComandas[Int32.Parse(strlist[0]) - 1].Estado == "CLOSE")
                {
                    Console.Clear();
                    Console.WriteLine("Comanda Fechada, não é possivel lancar novos itens!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                else if (ListaProdutos.Count < Int32.Parse(strlist[1]))
                {
                    Console.Clear();
                    Console.WriteLine("Produto não Existe!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                else if (ListaProdutos[Int32.Parse(strlist[1]) - 1].Estoque < 1)
                {
                    Console.Clear();
                    Console.WriteLine("Produto Esgotado!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                ListaProdutos[Int32.Parse(strlist[1]) - 1].Estoque--;
                List<Produto> temp = ListaComandas[Int32.Parse(strlist[0])-1].ItensConsumidos;
                temp.Add(Produto.GetProdutoByID(Int32.Parse(strlist[1]), ListaProdutos));
                ListaComandas[Int32.Parse(strlist[0])-1].ItensConsumidos = temp;
                Service.GravarComanda(ListaComandas);
                Service.GravarProduto(ListaProdutos);
            }
            void ExcluirItem()
            {
                List<Produto> ListaProdutos = Service.GetProdutos();
                List<Comanda> ListaComandas = Service.GetComandas();
                if (ListaComandas == null)
                {
                    Console.Clear();
                    Console.WriteLine("Não há comandas em aberto, tente criar uma primeiro!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Informe o ID da Comanda e será exibido todos os item lançados na mesma");
                Console.WriteLine("Formato: IDComanda");
                String[] strlist = Console.ReadLine().Split(',');
                //Fazer Condição de Proteção
                if (ListaComandas.Count < Int32.Parse(strlist[0]))
                {
                    Console.Clear();
                    Console.WriteLine("ID não existe");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                List<Produto> temp = ListaComandas[Int32.Parse(strlist[0]) - 1].ItensConsumidos;
                if(temp.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Não itens nessa comanda!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Selecione um dos numeros entre [ ] para remover");
                int n = 0;
                foreach (Produto p in temp)
                {
                    Console.WriteLine("["+n+"] "+ p.Nome);
                    n++;
                }
                String[] strlist2 = Console.ReadLine().Split(',');
                //Fazer Condição de Proteção
                temp.RemoveAt(Int32.Parse(strlist2[0]));
                ListaComandas[Int32.Parse(strlist[0]) - 1].ItensConsumidos = temp;
                Service.GravarComanda(ListaComandas);
            }
            void FecharComanda()
            {
                List<Comanda> ListaComandas = Service.GetComandas();
                if (ListaComandas == null)
                {
                    Console.Clear();
                    Console.WriteLine("Não há comandas em aberto, tente criar uma primeiro!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Informe o ID da Comanda que deseje encerrar e gerar a nota fiscal");
                Console.WriteLine("Formato: IDComanda");
                String[] strlist = Console.ReadLine().Split(',');
                //Fazer Condição de Proteção
                if (ListaComandas.Count < Int32.Parse(strlist[0]))
                {
                    Console.Clear();
                    Console.WriteLine("ID não existe");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }else if (ListaComandas[Int32.Parse(strlist[0]) - 1].Estado == "CLOSE")
                {
                    Console.Clear();
                    Console.WriteLine("Está comanda já foi encerrada!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                ListaComandas[Int32.Parse(strlist[0]) - 1].Estado = "CLOSE";
                Service.GravarComanda(ListaComandas);
                Service.GerarNF(ListaComandas[Int32.Parse(strlist[0]) - 1].Cliente, ListaComandas[Int32.Parse(strlist[0]) - 1].ID, ListaComandas[Int32.Parse(strlist[0]) - 1].ItensConsumidos);
                Console.Clear();
                Console.WriteLine("NF Gerada com Sucesso!");
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();

            }
            void MostrarCardapio()
            {
                List<Produto> ListaProdutos = Service.GetProdutos();
                Console.Clear();
                foreach (Produto p in ListaProdutos) {
                    Console.WriteLine("ID: "+p.ID+ " | Nome: " +p.Nome+  " | Valor: "+p.Valor+ " | Descricao: " +p.Descricao+ " | Estoque: " +p.Estoque);
                }
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();
            }
            void MostrarMesasOcupada()
            {
                List<Comanda> ListaComandas = Service.GetComandas();
                List<Mesa> ListaMesas = Service.GetMesas();
                Console.Clear();
                foreach (Comanda p in ListaComandas)
                {
                    if (p.Estado == "OPEN")
                    {
                        Console.WriteLine("IDMesa: " + p.IDmesa + " | Andar: " + ListaMesas[p.IDmesa-1].Andar + " | Tipo: " + ListaMesas[p.IDmesa - 1].Tipo_De_Salão + " | Lugares: " + ListaMesas[p.IDmesa - 1].Lugares);
                    }
                }
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();
            }
            void MostrarMesasDesocupadas()
            {
                List<Comanda> ListaComandas = Service.GetComandas();
                List<Mesa> ListaMesas = Service.GetMesas();
                Console.Clear();
                foreach (Comanda p in ListaComandas)
                {
                    if (p.Estado == "CLOSE")
                    {
                        Console.WriteLine("IDMesa: " + p.IDmesa + " | Andar: " + ListaMesas[p.IDmesa - 1].Andar + " | Tipo: " + ListaMesas[p.IDmesa - 1].Tipo_De_Salão + " | Lugares: " + ListaMesas[p.IDmesa - 1].Lugares);
                    }
                }
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();
            }
            void MostrarComandasAbertas()
            {
                List<Comanda> ListaComandas = Service.GetComandas();
                Console.Clear();
                foreach (Comanda p in ListaComandas)
                {
                    if (p.Estado == "OPEN")
                    {
                        Console.WriteLine("ID: " + p.ID + " | Atendente: " + p.Atendente + " | Cliente: " + p.Cliente + " | Horario De Chegada: " + p.HorarioDeChegada+ " | IDmesa: " + p.IDmesa);
                    }
                }
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();
            }
            void MostrarFornecedores()
            {
                List<Fornecedor> ListaFornecedores = Service.GetFornecedores();
                Console.Clear();
                foreach (Fornecedor p in ListaFornecedores)
                {
                    Console.WriteLine("ID: " + p.ID + " | Fornecedor: " + p.Nome + " | Localização: " + p.Localizacao);
                }
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();
            }
            void MostrarProdutosDosFornecedores()
            {
                List<Produto> ListaProdutos = Service.GetProdutosFromFornecedores();
                Console.Clear();
                foreach (Produto p in ListaProdutos)
                {
                    Console.WriteLine("ID: " + p.ID + " | Nome: " + p.Nome + " | Valor: " + p.Valor + " | Descricao: " + p.Descricao + " | Estoque: " + p.Estoque);
                }
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();
            }
            void AlterarEstoque()
            {
                List<Produto> ListaProdutos = Service.GetProdutos();
                Console.WriteLine("Informe o ID do Produto e a Quantidade Atual em estoque");
                Console.WriteLine("Formato: IDProduto, Quantidade");
                String[] strlist = Console.ReadLine().Split(',');
                if (ListaProdutos.Count < Int32.Parse(strlist[0]))
                {
                    Console.Clear();
                    Console.WriteLine("Produto não Existe!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                ListaProdutos[Int32.Parse(strlist[0]) - 1].Estoque = Int32.Parse(strlist[1]);
                Service.GravarProduto(ListaProdutos);
                Console.WriteLine("Estoque Alterado");
                Console.WriteLine("Pressione enter para voltar ao menu principal!");
                Console.ReadLine();
            }
        }
    }
    class Fornecedor
    {
        public int ID;
        public string Nome;
        public string Localizacao;
        public List<Produto> ProdutosFornecidos;

        // Construtor vazio para permitir uma instância vazia da classe Produto
        public Fornecedor()
        {
        }

        public Fornecedor(int iD, string nome, string localizacao, List<Produto> produtosFornecidos)
        {
            ID = iD;
            Nome = nome;
            Localizacao = localizacao;
            ProdutosFornecidos = produtosFornecidos;
        }
    }
    class Produto
    {
        public int ID;
        public string Nome;
        public string Descricao;
        public float Valor;
        public int Estoque;

        // Construtor vazio para permitir uma instância vazia da classe Produto
        public Produto()

        {
        }

        public Produto(int iD, string nome, string descricao, float valor, int estoque)
        {
            ID = iD;
            Nome = nome;
            Descricao = descricao;
            Valor = valor;
            Estoque = estoque;
        }
        //Para facilitar as funções de insert Produto, fizemos uma função para pegar Produto de uma lista dado um certo ID
        public static Produto GetProdutoByID(int ID, List<Produto> input)
        {
            Produto saida = new Produto();
            foreach (Produto p in input)
            {
                if (p.ID == ID)
                {
                    saida = p;
                }
            }
            return saida;
        }
    }
    class Mesa
    {
        public int ID;
        public int Andar;
        public string Tipo_De_Salão;
        public int Lugares;

        public Mesa(int iD, int andar, string tipo_De_Salão, int lugares)
        {
            ID = iD;
            Andar = andar;
            Tipo_De_Salão = tipo_De_Salão;
            Lugares = lugares;
        }
        // Construtor vazio para permitir uma instância vazia da classe Mesa
        public Mesa()
        {
        }
    }
    class Comanda
    {
        public int ID;
        public string Atendente;
        public string Cliente;
        public int IDmesa;
        public string HorarioDeChegada;
        public string Estado;
        public List<Produto> ItensConsumidos;

        // Construtor vazio para permitir uma instância vazia da classe Comanda
        public Comanda()
        {
        }

        public Comanda(int iD, string atendente, string cliente, int iDmesa, string horarioDeChegada, string estado, List<Produto> itensConsumidos)
        {
            ID = iD;
            Atendente = atendente;
            Cliente = cliente;
            IDmesa = iDmesa;
            HorarioDeChegada = horarioDeChegada;
            Estado = estado;
            ItensConsumidos = itensConsumidos;
        }
    }
    class Service
    {
        public static List<Produto> GetProdutos()
        {
            string path = @"C:\Users\danma\Documents\teste\produtos.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            // read file into a string and deserialize JSON to a type
            List<Produto> lista = JsonConvert.DeserializeObject<List<Produto>>(File.ReadAllText(path));
            return lista;
        }
        public static List<Produto> GetProdutosFromFornecedores()
        {
            List<Produto> Extraida1 = new List<Produto>();
            List<Produto> Extraida2 = new List<Produto>();
            string path = @"C:\Users\danma\Documents\teste\fornecedores.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            // read file into a string and deserialize JSON to a type
            List<Fornecedor> lista = JsonConvert.DeserializeObject<List<Fornecedor>>(File.ReadAllText(path));
            foreach (Fornecedor p in lista)
            {
                Extraida1 = p.ProdutosFornecidos;
                foreach (Produto k in Extraida1) {
                    Extraida2.Add(k);
                }
            }

            return Extraida2;
        }

        public static List<Mesa> GetMesas()
        {
            string path = @"C:\Users\danma\Documents\teste\mesas.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            // read file into a string and deserialize JSON to a type
            List<Mesa> lista = JsonConvert.DeserializeObject<List<Mesa>>(File.ReadAllText(path));
            return lista;
        }

        public static List<Fornecedor> GetFornecedores()
        {
            string path = @"C:\Users\danma\Documents\teste\fornecedores.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            // read file into a string and deserialize JSON to a type
            List<Fornecedor> lista = JsonConvert.DeserializeObject<List<Fornecedor>>(File.ReadAllText(path));
            return lista;
        }

        public static void GravarComanda(List<Comanda> input)
        {
            string path = @"C:\Users\danma\Documents\teste\comandas.txt";
            File.WriteAllText(path, JsonConvert.SerializeObject(input));
        }

        public static void GravarProduto(List<Produto> input)
        {
            string path = @"C:\Users\danma\Documents\teste\produtos.txt";
            File.WriteAllText(path, JsonConvert.SerializeObject(input));
        }

        public static List<Comanda> GetComandas()
        {
            string path = @"C:\Users\danma\Documents\teste\comandas.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            // read file into a string and deserialize JSON to a type
            List<Comanda> lista = JsonConvert.DeserializeObject<List<Comanda>>(File.ReadAllText(path));
            return lista;
        }
        public static void GerarNF(string cliente, int IDComanda, List<Produto> inputProdutos)
        {
            string path = @"C:\Users\danma\Documents\teste\CLIENTE_"+ cliente +"_IDCOMANDA_"+IDComanda+"_HORADECHEGADA_"+DateTime.Now.ToString("dd-MM-yyyy-H-mm") + ".txt";
            string NF = "";
            float ValorTotal = 0;
            List<Produto> ListaDeProdutos = GetProdutos();
            foreach (Produto p in inputProdutos)
            {
                NF += "Produto: " +p.Nome+ "        Valor: "+p.Valor+"\n";
                ValorTotal += p.Valor;
            }
            NF += "Valor Total = " + ValorTotal;
            File.WriteAllText(path, NF);
        }
    }
}

