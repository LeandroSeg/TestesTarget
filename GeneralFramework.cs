using System.Text.Json;
using TestesTarget;
public class GeneralFramework
{

    static List<(int id, string ProductName, decimal estoque)> ProductList = new List<(int id, string ProductName, decimal estoque)>();
    static List<(int idProduct, int amount, string typeMov)> EstoqueMovimentList = new List<(int idProduct, int amount, string typeMov)>();

    public decimal CalculateFine(decimal presentValue, decimal percentage, DateTime dueDate)
    {
        
        DateTime currentDate = DateTime.Now;
        int daysLate = (currentDate - dueDate).Days;
        if (daysLate <= 0)
        {
            return presentValue; // No fine if not late
        }
        decimal dailyRate = percentage / 100m;
        decimal futureValue = presentValue * (decimal)Math.Pow((double)(1 + dailyRate), daysLate);
        return Math.Round(futureValue - presentValue,2);
    }

    public void ConvertEstoqueToJsonElement(string json, out JsonElement vendas)
    {
        JsonDocument doc;
        doc = JsonDocument.Parse(json);
        vendas = doc.RootElement.GetProperty("estoque");
    }

    public void ConvertVendasToJsonElement(string json, out JsonElement vendas)
    {
        JsonDocument doc;
        doc = JsonDocument.Parse(json);
        vendas = doc.RootElement.GetProperty("vendas");
    }

    public decimal GetInventory(int idProduct)
    {
        InitializeInventory();
        int index = ProductList.FindIndex(s => s.id == idProduct);

        if (index != -1)
        {
            var product = ProductList[index];
            return product.estoque;
        }
        else
        {
            throw new Exception($"Product with ID {idProduct} not found in inventory.");
        }

    }

    public void InitializeInventory()
    {

        if (ProductList.Count == 0)
        {
            var d = new Data();
            string json = d.GetDataJsonTest2();
            JsonElement Estoque;
            ConvertEstoqueToJsonElement(json, out Estoque);

            foreach (JsonElement Product in Estoque.EnumerateArray())
            {
                int codigoProduto = Product.GetProperty("codigoProduto").GetInt32();
                string ProductName = Product.GetProperty("descricaoProduto").GetString();
                double amount = Product.GetProperty("estoque").GetDouble();

                if (!string.IsNullOrEmpty(ProductName))
                    ProductList.Add((codigoProduto, ProductName, (decimal)amount));
            }


        }
    }

    /// <summary>
    /// Incrementa o estoque do item. Adotado modelo de campo de estoque direto no cadastro de produto atualizado diretamente pelo movimento de itens.
    /// </summary>
    /// <param name="idProduct"></param>
    /// <param name="amount"></param>
    /// <exception cref="Exception"></exception>
    public void InsertMovimentToInventory(int idProduct, int amount)
    {
        InitializeInventory();
        int index = ProductList.FindIndex(s => s.id == idProduct);

        if (index != -1)
        {
            var product = ProductList[index];
            product.estoque += amount;
            ProductList[index] = product;
            EstoqueMovimentList.Add((idProduct, amount, "E"));
        }
        else
        {
            throw new Exception($"Produto ID {idProduct} não encontrado no estoque");
        }

    }

    /// <summary>
    /// Decrementa o estoque do item. Adotado modelo de campo de estoque direto no cadastro de produto atualizado diretamente pelo movimento de itens.
    /// </summary>
    /// <param name="idProduct"></param>
    /// <param name="amount"></param>
    /// <exception cref="Exception"></exception>
    public void RemoveMovimentFromInventory(int idProduct, int amount)
    {
        InitializeInventory();
        int index = ProductList.FindIndex(s => s.id == idProduct);

        if (index != -1)
        {
            var product = ProductList[index];
            product.estoque -= amount;
            ProductList[index] = product;
            EstoqueMovimentList.Add((idProduct, amount , "S" ));
        }
        else
        {
            throw new Exception($"Produto ID {idProduct} não encontrado no estoque");
        }
    }

    public List<(string SellerName, decimal CommissionAmount)> SummarySalesBySeller(JsonElement sales)
    {
        var salesList = new List<(string SellerName, decimal CommissionAmount)>();

        foreach (JsonElement venda in sales.EnumerateArray())
        {
            string sellerName = venda.GetProperty("vendedor").GetString();
            double totalSale = venda.GetProperty("valor").GetDouble();

            decimal commissionAmount = (decimal)(totalSale * 0.05);

            if (totalSale < 100)
            {
                commissionAmount = 0;
            }
            else if (totalSale >= 100 && totalSale < 500)
            {
                commissionAmount = (decimal)(totalSale * 0.01);
            }

            int index = salesList.FindIndex(s => s.SellerName.ToLower() == sellerName.ToLower());

            if (index == -1)
            {
                if (!string.IsNullOrEmpty(sellerName))
                    salesList.Add((sellerName, (decimal)commissionAmount));
            }
            else
            {
                var element = salesList[index];
                element.CommissionAmount += (decimal)commissionAmount;
                salesList[index] = element;
            }
        }

        return salesList;
    }
}