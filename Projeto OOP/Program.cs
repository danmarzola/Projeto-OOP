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
            Console.WriteLine("(9) Limpar Console");
            Console.WriteLine("(10) Encerrar Terminal");
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
                    Console.Clear();
                    return true;
                case "10":
                    return false;
                default:
                    return true;
            }
            void AbrirComanda()
            {
                List<Comanda> ListaComandas = Service.GetComandas();
                List<Mesa> ListaMesas = Service.GetMesas();
                List<int> MesasOcupadas = new List<int>();
                if (ListaComandas == null)
                {
                    ListaComandas = new List<Comanda>();
                }
                foreach(Comanda p in ListaComandas)
                {
                    if(p.Estado == "OPEN")
                    {
                        MesasOcupadas.Add(p.IDmesa);
                    }
                }
                List<int> vazia = new List<int>();
                Console.WriteLine("Digite no seguinte formato as informações necessarias para abrir a comanda: ");
                Console.WriteLine("Formato: Atendente, Cliente, ID da Mesa");
                String[] strlist = Console.ReadLine().Split(',');
                //Fazer Condição de Proteção
                if (MesasOcupadas.Contains(Int32.Parse(strlist[2])))
                {
                    Console.Clear();
                    Console.WriteLine("Mesa já esta sendo utilizada em uma comanda em aberto!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }else if(ListaMesas.Count < Int32.Parse(strlist[2]))
                {
                    Console.Clear();
                    Console.WriteLine("Mesa não existe!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                ListaComandas.Add(new Comanda(ListaComandas.Count+1, strlist[0], strlist[1], Int32.Parse(strlist[2]), DateTime.Now.ToString("dd/MM/yyyy H:mm"), "OPEN", vazia));
                Service.GravarComanda(ListaComandas);
                Console.WriteLine("ID: "+(ListaComandas.Count) + " | Atendente: "+ strlist[0] + " | Cliente: " + strlist[1] + " | IDMesa: " + Int32.Parse(strlist[2]) + " | HorarioDeChegada: " + DateTime.Now.ToString("dd / MM / yyyy H: mm") + " | Estado: OPEN");
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
                //Fazer Condição de Proteção
                if(ListaComandas.Count < Int32.Parse(strlist[0]))
                {
                    Console.Clear();
                    Console.WriteLine("ID não existe");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                else if(ListaProdutos[Int32.Parse(strlist[1]) - 1].Estoque < 1)
                {
                    Console.Clear();
                    Console.WriteLine("Produto Esgotado!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                ListaProdutos[Int32.Parse(strlist[1]) - 1].Estoque--;
                List<int> temp = ListaComandas[Int32.Parse(strlist[0])-1].Lancamentos;
                temp.Add(Int32.Parse(strlist[1]));
                ListaComandas[Int32.Parse(strlist[0])-1].Lancamentos = temp;
                Service.GravarComanda(ListaComandas);
                Service.GravarProduto(ListaProdutos);
            }
            void ExcluirItem()
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
                List<int> temp = ListaComandas[Int32.Parse(strlist[0]) - 1].Lancamentos;
                if(temp.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Não itens nessa comanda!");
                    Console.WriteLine("Pressione enter para voltar ao menu principal!");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Selecione um dos valores entre [ ] para remover");
                int n = 0;
                foreach (var elemento in temp)
                {
                    Console.WriteLine("["+n+"] "+ elemento);
                    n++;
                }
                String[] strlist2 = Console.ReadLine().Split(',');
                //Fazer Condição de Proteção
                temp.RemoveAt(Int32.Parse(strlist2[0]));
                ListaComandas[Int32.Parse(strlist[0]) - 1].Lancamentos = temp;
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
                }
                ListaComandas[Int32.Parse(strlist[0]) - 1].Estado = "CLOSE";
                Service.GravarComanda(ListaComandas);
                Service.GerarNF(ListaComandas[Int32.Parse(strlist[0]) - 1].Cliente, ListaComandas[Int32.Parse(strlist[0]) - 1].ID, ListaComandas[Int32.Parse(strlist[0]) - 1].Lancamentos);

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
                List<Mesa> ListaMesas = Service.GetMesas();
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
        public List<int> Lancamentos;

        public Comanda(int iD, string atendente, string cliente, int iDmesa, string horarioDeChegada, string estado, List<int> lancamentos)
        {
            ID = iD;
            Atendente = atendente;
            Cliente = cliente;
            IDmesa = iDmesa;
            HorarioDeChegada = horarioDeChegada;
            Estado = estado;
            Lancamentos = lancamentos;
        }

        // Construtor vazio para permitir uma instância vazia da classe Comanda
        public Comanda()
        {
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

        public static void GravarComanda(List<Comanda> input)
        {
            string path = @"C:\Users\danma\Documents\teste\comandas.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            File.WriteAllText(path, string.Empty);
            File.WriteAllText(path, JsonConvert.SerializeObject(input));
        }

        public static void GravarProduto(List<Produto> input)
        {
            string path = @"C:\Users\danma\Documents\teste\produtos.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            File.WriteAllText(path, string.Empty);
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
        public static void GerarNF(string cliente, int IDComanda, List<int> IDprodutos)
        {
            string path = @"C:\Users\danma\Documents\teste\NomeCliente_"+cliente+"_IDcomanda_"+IDComanda+"_"+DateTime.Now.ToString("dd-MM-yyyy-H-mm") + ".txt";
            string NF = "";
            float ValorTotal = 0;
            List<Produto> ListaDeProdutos = GetProdutos();
            foreach (var i in IDprodutos)
            {
                NF += "Produto: " +ListaDeProdutos[i-1].Nome+ "        Valor: "+ListaDeProdutos[i - 1].Valor+"\n";
                ValorTotal += ListaDeProdutos[i - 1].Valor;
            }
            NF += "Valor Total = " + ValorTotal;
            File.WriteAllText(path, NF);
        }
    }
}

