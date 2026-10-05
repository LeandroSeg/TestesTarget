using System;
using System.Text.Json;

namespace TestesTarget
{
    public class Program
    {
        static void Main()
        {
            int opcao = 0;


            do
            {
                Console.Clear();
                Console.WriteLine("MENU TESTES");
                Console.WriteLine("1-Teste sobre comissões");
                Console.WriteLine("2-Teste sobre movimentação de estoque");
                Console.WriteLine("3-Teste sobre cálculo de juros");
                Console.WriteLine("4-Sair");
                Console.Write("Escolha uma opção: ");
                string entrada = Console.ReadLine();

                if (int.TryParse(entrada, out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            Console.WriteLine("Opção 1 escolhida");
                            ExecTest1();
                            // Aqui você pode chamar o código que lê o JSON
                            break;

                        case 2:
                            Console.WriteLine("Opção 2 escolhida");
                            ExecTest2();
                            // Aqui você pode implementar cadastro
                            break;

                        case 3:
                            Console.WriteLine("Opção 3 escolhida.");
                            ExecTest3();
                            break;

                        case 4:
                            Console.WriteLine("Saindo do programa...");
                            break;

                        default:
                            Console.WriteLine("Opção inválida!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida, digite um número!");
                }

                if (opcao != 4)
                {
                    Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();

                }

            } while (opcao != 4);


        }

        private static void ExecTest3()
        {
            Console.WriteLine("\nCalculo de juros diário");
            Console.WriteLine("Digite o valor:");

            string Amount = Console.ReadLine();

            Console.WriteLine("Digite a taxa de juros diária (padrão = 2,5%): ");

            string Percentage = Console.ReadLine();

            Percentage = Percentage.Replace("%", "").Replace(".", ",").Trim();

            if (string.IsNullOrWhiteSpace(Percentage))
            {
                Percentage = "2,5";
            }

            Console.WriteLine("Digite a data de vencimento (dd/mm/aaaa):");

            string DueDate = Console.ReadLine();


            var f = new GeneralFramework();
            decimal futureValue = f.CalculateFine( System.Convert.ToDecimal( Amount ), System.Convert.ToDecimal(Percentage), System.Convert.ToDateTime(DueDate));

            Console.WriteLine($"Valor dos juros até a data de HOJE: {futureValue}");
        }

        public static void ExecTest1()
        {
            var d = new Data();
            var f = new GeneralFramework();
            string json = d.GetDataJsonTest1();
            JsonElement sales;
            f.ConvertVendasToJsonElement(json, out sales);
            var salesSummarizedBySeller = f.SummarySalesBySeller(sales);

            Console.WriteLine($"Total de vendedores: {salesSummarizedBySeller.Count()}");

            for( int i = 0; i < salesSummarizedBySeller.Count(); i++)
            {
                Console.WriteLine($"Vendedor: {salesSummarizedBySeller[i].SellerName}, Comissão: {salesSummarizedBySeller[i].CommissionAmount}");
            }
          
        }



        private static void ExecTest2()
        {
            int opcao = 0;
            var f = new GeneralFramework();

            do
            {
                Console.Clear();
                Console.WriteLine("MOVIMENTAÇÃO DE PRODUTOS");
                Console.WriteLine("1-Entrada no Estoque");
                Console.WriteLine("2-Saida do Estoque");
                Console.WriteLine("3-Sair");
                Console.WriteLine("Estoque atual");
                Console.WriteLine($"Produto 101 tem {f.GetInventory(101)}");
                Console.WriteLine($"Produto 102 tem {f.GetInventory(102)}");
                Console.WriteLine($"Produto 103 tem {f.GetInventory(103)}");
                Console.WriteLine($"Produto 104 tem {f.GetInventory(104)}");
                Console.WriteLine($"Produto 105 tem {f.GetInventory(105)}");
                Console.WriteLine("");
                Console.Write("Escolha uma opção: ");
                Console.WriteLine("");
                Console.WriteLine("");




                string entrada = Console.ReadLine();

                try
                {
                    if (int.TryParse(entrada, out opcao))
                    {
                        switch (opcao)
                        {
                            case 1:
                                Console.WriteLine("Entrada de movimento no estoque");
                                ExecProductIntake();
                                // Aqui você pode chamar o código que lê o JSON
                                break;

                            case 2:
                                Console.WriteLine("Opção 2 escolhida");
                                ExecProductOutFlow();
                                // Aqui você pode implementar cadastro
                                break;

                            case 3:
                                Console.WriteLine("Retornando...");
                                break;

                            default:
                                throw new Exception("Não consta esta opção");
                                break;
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception erro)
                {

                    Console.WriteLine( erro.Message );
                    break;
                }

  

            } while (opcao != 3);


        }


        private static void ExecProductIntake()
        {
            Console.WriteLine("Digite o ID do produto para entrada no estoque:");

            string IdProduct = Console.ReadLine();

            Console.WriteLine("Digite a quantidade do produto para entrada no estoque:");

            string Qtde = Console.ReadLine();

            if (int.TryParse(IdProduct, out int idProduct) && int.TryParse(Qtde, out int amount))
            {
                var f = new GeneralFramework();
                f.InsertMovimentToInventory(idProduct, amount);
                Console.WriteLine($"Entrada de {amount} unidades do produto {idProduct} realizada com sucesso.");
            }
            else
            {
                Console.WriteLine("ID do produto ou quantidade inválidos. Por favor, insira valores numéricos válidos.");
            }


        }

        private static void ExecProductOutFlow()
        {
            Console.WriteLine("Digite o ID do produto para saída no estoque:");

            string IdProduct = Console.ReadLine();

            Console.WriteLine("Digite a quantidade do produto para saída no estoque:");

            string Qtde = Console.ReadLine();

            if (int.TryParse(IdProduct, out int idProduct) && int.TryParse(Qtde, out int amount))
            {
                var f = new GeneralFramework();
                f.RemoveMovimentFromInventory(idProduct, amount);
                Console.WriteLine($"Saída de {amount} unidades do produto {idProduct} realizada com sucesso.");
            }
            else
            {
                Console.WriteLine("ID do produto ou quantidade inválidos. Por favor, insira valores numéricos válidos.");
            }

        }
    }

}