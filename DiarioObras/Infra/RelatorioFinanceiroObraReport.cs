using DiarioObras.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Net.Http;

namespace DiarioObras.Infra
{
    public class RelatorioFinanceiroObraReport : IDocument
    {
        private readonly List<CustoObra> _custos;
        private readonly Dictionary<CategoriaCusto, string> _coresCategoria;

        public RelatorioFinanceiroObraReport(List<CustoObra> custos)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _custos = custos.OrderBy(c => c.Data).ToList();

            // Definição de cores para cada categoria
            _coresCategoria = new Dictionary<CategoriaCusto, string>
            {
                { CategoriaCusto.PagamentoFuncionario, Colors.Red.Lighten2 },
                { CategoriaCusto.Alimentacao, Colors.Green.Lighten2 },
                { CategoriaCusto.CompraMateriais, Colors.Orange.Lighten2 },
                { CategoriaCusto.AluguelEquipamentos, Colors.Purple.Lighten2 },
                { CategoriaCusto.Transporte, Colors.Blue.Lighten2 },
                { CategoriaCusto.DespesasGerais, Colors.Grey.Lighten1 },
                { CategoriaCusto.ServicosTerceirizados, Colors.Teal.Lighten2 },
                { CategoriaCusto.Imprevistos, Colors.Pink.Lighten2 }
            };
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Helvetica")); // Reduzi o tamanho da fonte base

                page.Header().Column(header =>
                {
                    header.Item().Text("RELATÓRIO FINANCEIRO DA OBRA")
                        .FontSize(16).Bold().AlignCenter();

                    header.Item().Text($"Período: {_custos.Min(c => c.Data):dd/MM/yyyy} a {_custos.Max(c => c.Data):dd/MM/yyyy}")
                        .FontSize(12).AlignCenter();

                    header.Item().PaddingBottom(5).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                });

                page.Content().Column(col =>
                {
                    col.Spacing(10); // Reduzi o espaçamento entre seções

                    // 1. RESUMO FINANCEIRO
                    AddSectionHeader(col, "RESUMO FINANCEIRO", Colors.Blue.Darken2);

                    // Tabela de custos detalhados com espaçamentos reduzidos
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1.5f); // Data
                            columns.RelativeColumn(3);     // Descrição
                            columns.RelativeColumn(2);     // Categoria
                            columns.RelativeColumn(1.5f);  // Valor
                        });

                        // Cabeçalho
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Data").Bold();
                            header.Cell().Element(CellStyle).Text("Descrição").Bold();
                            header.Cell().Element(CellStyle).Text("Categoria").Bold();
                            header.Cell().Element(CellStyle).Text("Valor (R$)").Bold();
                        });

                        // Linhas
                        foreach (var custo in _custos)
                        {
                            table.Cell().Element(CellStyle).Text(custo.Data.ToString("dd/MM/yyyy"));
                            table.Cell().Element(CellStyle).Text(custo.Descricao);
                            table.Cell().Background(_coresCategoria[custo.Categoria]).Element(CellStyle).Text(custo.Categoria.ToString());
                            table.Cell().Element(CellStyle).Text(custo.Valor.ToString("N2"));
                        }

                        // Totalizador
                        table.Cell().ColumnSpan(3).Element(CellStyle).Text("TOTAL").Bold();
                        table.Cell().Element(CellStyle).Text(_custos.Sum(c => c.Valor).ToString("N2")).Bold();
                    });

                    // 2. VISUALIZAÇÃO POR CATEGORIA
                    AddSectionHeader(col, "DISTRIBUIÇÃO POR CATEGORIA", Colors.Blue.Darken2);

                    // Gráfico de Pizza
                    var pizzaChart = GerarGraficoBase64(_custos, "pizza");
                    if (!string.IsNullOrEmpty(pizzaChart))
                    {
                        try
                        {
                            var bytes = Convert.FromBase64String(pizzaChart.Split(',')[1]);
                            col.Item().Image(bytes, ImageScaling.FitWidth);
                        }
                        catch
                        {
                            col.Item().Text("Não foi possível carregar o gráfico de pizza").Italic();
                        }
                    }

                    // 3. RESUMO POR CATEGORIA
                    AddSectionHeader(col, "RESUMO POR CATEGORIA", Colors.Blue.Darken2);

                    col.Item().Grid(grid =>
                    {
                        grid.Columns(4);
                        grid.Spacing(5); // Reduzi o espaçamento entre os itens

                        var resumo = _custos
                            .GroupBy(c => c.Categoria)
                            .Select(g => new {
                                Categoria = g.Key,
                                Total = g.Sum(c => c.Valor),
                                Percentual = (g.Sum(c => c.Valor) / _custos.Sum(c => c.Valor)) * 100
                            })
                            .OrderByDescending(x => x.Total);

                        foreach (var item in resumo)
                        {
                            grid.Item().Background(_coresCategoria[item.Categoria])
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(5) // Reduzi o padding interno
                                .AlignCenter()
                                .Text(text =>
                                {
                                    text.Span(item.Categoria.ToString().ToUpper()).Bold().FontSize(10);
                                    text.EmptyLine();
                                    text.Span(item.Total.ToString("C")).FontSize(11);
                                    text.EmptyLine();
                                    text.Span($"{item.Percentual:F1}% do total").FontSize(8);
                                });
                        }
                    });

                    // 4. ASSINATURAS
                    AddSectionHeader(col, "ASSINATURAS", Colors.Blue.Darken2);

                    col.Item().PaddingTop(5).Table(table => // Reduzi o padding
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().Column(column =>
                        {
                            column.Item().AlignCenter().Text("Responsável Financeiro").Bold();
                            column.Item().PaddingTop(15).AlignCenter().Text("_________________________"); // Reduzi o espaço
                            column.Item().AlignCenter().Text("Nome do Responsável");
                        });

                        table.Cell().Column(column =>
                        {
                            column.Item().AlignCenter().Text("Contratante").Bold();
                            column.Item().PaddingTop(15).AlignCenter().Text("_________________________");
                            column.Item().AlignCenter().Text("Nome do Contratante");
                        });
                    });
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ").FontSize(8);
                    x.CurrentPageNumber().FontSize(8);
                    x.Span(" de ").FontSize(8);
                    x.TotalPages().FontSize(8);
                    x.EmptyLine();
                    x.Span("Gerado em ").FontSize(8).Italic();
                    x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(8).Italic();
                });
            });
        }

        private void AddSectionHeader(ColumnDescriptor column, string title, string color)
        {
            column.Item().Background(color).Padding(5).Text(title).FontColor(Colors.White).Bold(); // Reduzi o padding
        }

        private IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(3) // Reduzi bastante o espaçamento vertical
                .PaddingHorizontal(5)
                .MinHeight(20); // Reduzi a altura mínima
        }

        private string GerarGraficoBase64(List<CustoObra> custos, string tipoGrafico)
        {
            try
            {
                var dataPorCategoria = custos
                    .GroupBy(c => c.Categoria)
                    .OrderBy(g => g.Key)
                    .Select(g => new {
                        Categoria = g.Key,
                        Nome = g.Key.ToString(),
                        Total = g.Sum(x => x.Valor)
                    });

                var labels = string.Join(",", dataPorCategoria.Select(x => $"\"{x.Categoria}\""));
                var valores = string.Join(",", dataPorCategoria.Select(x => x.Total.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)));
                var cores = string.Join(",", dataPorCategoria.Select(x => $"\"{_coresCategoria[x.Categoria]}\""));

                var chartType = tipoGrafico switch
                {
                    "pizza" => "pie",
                    "barra" => "bar",
                    _ => "bar"
                };

                var chartConfig = $@"
        {{
            type: '{chartType}',
            data: {{
                labels: [{labels}],
                datasets: [{{
                    label: 'Valor (R$)',
                    data: [{valores}],
                    backgroundColor: [{cores}]
                }}]
            }}
        }}";

                var url = "https://quickchart.io/chart?width=500&height=250&c=" + Uri.EscapeDataString(chartConfig); // Reduzi o tamanho

                using var http = new HttpClient();
                var imageBytes = http.GetByteArrayAsync(url).Result;
                return "data:image/png;base64," + Convert.ToBase64String(imageBytes);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}