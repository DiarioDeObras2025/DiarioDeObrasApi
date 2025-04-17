using DiarioObras.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace DiarioObras.Infra
{
    public class RegistroDiarioDertaisReport : IDocument
    {
        public RegistroDiario Model { get; set; }

        public RegistroDiarioDertaisReport(RegistroDiario relatorioDiario)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Model = relatorioDiario;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(15);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Helvetica"));

                // Cabeçalho
                page.Header().Column(header =>
                {
                    header.Item().AlignCenter().Text(text =>
                    {
                        text.Span("RELATÓRIO DIÁRIO DE OBRA").Bold().FontSize(12);
                        text.EmptyLine();
                        text.Span($"Obra: {Model.Obra?.Nome ?? "N/A"}").FontSize(9);
                        text.EmptyLine();
                        text.Span($"Data: {Model.Data:dd/MM/yyyy}").FontSize(9);
                    });

                    header.Item().PaddingBottom(5).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                });

                // Conteúdo
                page.Content().Column(col =>
                {
                    col.Spacing(8);

                    // SEÇÃO 1: DETALHES DA OBRA
                    AddSectionHeader(col, "1. DETALHES DA OBRA");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120);
                            columns.RelativeColumn();
                            columns.ConstantColumn(120);
                            columns.RelativeColumn();
                        });

                        AddTableRow(table, "Nome da Obra", Model.Obra?.Nome);
                        AddTableRow(table, "Endereço", Model.Obra?.Endereco);
                        AddTableRow(table, "Cliente", Model.Obra?.Cliente);
                        AddTableRow(table, "Contrato", Model.Obra?.NumeroContrato);
                        AddTableRow(table, "Eng. Responsável", Model.Obra?.EngenheiroResponsavel);
                        AddTableRow(table, "Status", Model.Obra?.Status.ToString());
                        AddTableRow(table, "Data Início", Model.Obra?.DataInicio.ToString("dd/MM/yyyy"));
                        AddTableRow(table, "Previsão Término", Model.Obra?.DataTerminoPrevista?.ToString("dd/MM/yyyy") ?? "N/A");
                    });

                    // SEÇÃO 2: INFORMAÇÕES DO DIA
                    AddSectionHeader(col, "2. INFORMAÇÕES DO DIA");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120);
                            columns.RelativeColumn();
                            columns.ConstantColumn(120);
                            columns.RelativeColumn();
                        });

                        AddTableRow(table, "Título", Model.Titulo);
                        AddTableRow(table, "Resumo", Model.Resumo);
                        AddTableRow(table, "Etapa Atual", GetEtapaDisplayName(Model.Etapa));
                        AddTableRow(table, "% Concluído", $"{Model.PercentualConcluido}%");
                        AddTableRow(table, "Área Executada", $"{Model.AreaExecutada} m²");
                        AddTableRow(table, "Cond. Climáticas", Model.CondicoesClimaticas.ToString());
                        AddTableRow(table, "Temperatura", Model.Temperatura.HasValue ? $"{Model.Temperatura}°C" : "N/A");
                        AddTableRow(table, "Precipitação", Model.Precipitacao.HasValue ? $"{Model.Precipitacao} mm" : "N/A");
                    });

                    // SEÇÃO 3: EQUIPE
                    AddSectionHeader(col, "3. EQUIPE");

                    // Tabela de resumo
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Total Funcionários").Bold();
                            header.Cell().Element(CellStyle).Text("Total Terceirizados").Bold();
                        });

                        table.Cell().Element(CellStyle).Text(Model.Equipe?.Count(m => !m.Terceirizado) ?? 0);
                        table.Cell().Element(CellStyle).Text(Model.Equipe?.Count(m => m.Terceirizado) ?? 0);
                    });

                    // Tabela detalhada da equipe
                    if (Model.Equipe.Any())
                    {
                        col.Spacing(10);
                        col.Item().Text("Detalhamento da Equipe");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Nome
                                columns.RelativeColumn(2); // Cargo
                                columns.RelativeColumn();  // Tipo
                                columns.RelativeColumn(3); // Observações
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Nome").Bold();
                                header.Cell().Element(CellStyle).Text("Cargo").Bold();
                                header.Cell().Element(CellStyle).Text("Tipo").Bold();
                                header.Cell().Element(CellStyle).Text("Observações").Bold();
                            });

                            foreach (var membro in Model.Equipe)
                            {
                                table.Cell().Element(CellStyle).Text(membro.Nome);
                                table.Cell().Element(CellStyle).Text(membro.Cargo);
                                table.Cell().Element(CellStyle).Text(membro.Terceirizado ? "Terceirizado" : "Próprio");
                                table.Cell().Element(CellStyle).Text(membro.Observacao ?? "-");
                            }
                        });
                    }
                    else
                    {
                        col.Item().Text("Nenhum membro da equipe registrado");
                    }

                    // SEÇÃO 4: MATERIAIS E EQUIPAMENTOS
                    AddSectionHeader(col, "4. MATERIAIS E EQUIPAMENTOS");

                    // Equipamentos
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns => columns.RelativeColumn());
                        table.Cell().Element(CellStyle).Text("Equipamentos Utilizados:").Bold();
                        table.Cell().Element(CellStyle).Text(Model.Equipamentos ?? "Nenhum equipamento registrado");
                    });

                    // Consumo de Cimento
                    if (Model.ConsumoCimento > 0)
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => columns.RelativeColumn());
                            table.Cell().Element(CellStyle).Text("Consumo de Cimento:").Bold();
                            table.Cell().Element(CellStyle).Text($"{Model.ConsumoCimento} sacos");
                        });
                    }

                    // Materiais Utilizados
                    if (Model.Materiais.Any())
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => columns.RelativeColumn());
                            table.Cell().Element(CellStyle).Text("Materiais Utilizados:").Bold();
                        });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Material").Bold();
                                header.Cell().Element(CellStyle).Text("Quantidade").Bold();
                                header.Cell().Element(CellStyle).Text("Unidade").Bold();
                            });

                            foreach (var m in Model.Materiais)
                            {
                                table.Cell().Element(CellStyle).Text(m.Nome ?? "N/A");
                                table.Cell().Element(CellStyle).Text(m.Quantidade.ToString());
                                table.Cell().Element(CellStyle).Text(m.Unidade ?? "un");
                            }
                        });
                    }

                    // SEÇÃO 5: OCORRÊNCIAS
                    if (!string.IsNullOrWhiteSpace(Model.Ocorrencias))
                    {
                        AddSectionHeader(col, "5. OCORRÊNCIAS E OBSERVAÇÕES");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => columns.RelativeColumn());
                            table.Cell().Element(CellStyle).Text(Model.Ocorrencias);
                        });
                    }

                    // SEÇÃO 6: REGISTRO FOTOGRÁFICO - VERSÃO ATUALIZADA
                    AddSectionHeader(col, "6. REGISTRO FOTOGRÁFICO");

                    if (Model.Fotos != null && Model.Fotos.Any())
                    {
                        // Calcular quantas linhas serão necessárias (3 fotos por linha)
                        int rowsNeeded = (int)Math.Ceiling(Model.Fotos.Count / 3.0);

                        col.Item().Table(table =>
                        {
                            // Definir 3 colunas de largura igual
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            for (int row = 0; row < rowsNeeded; row++)
                            {
                                // Adicionar uma linha para cada conjunto de 3 fotos
                                table.Cell().Row((uint)row + 1).Column(1).Element(c => AddFotoElement(c, Model.Fotos.ElementAtOrDefault(row * 3)));
                                table.Cell().Row((uint)row + 1).Column(2).Element(c => AddFotoElement(c, Model.Fotos.ElementAtOrDefault(row * 3 + 1)));
                                table.Cell().Row((uint)row + 1).Column(3).Element(c => AddFotoElement(c, Model.Fotos.ElementAtOrDefault(row * 3 + 2)));
                            }
                        });
                    }
                    else
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => columns.RelativeColumn());
                            table.Cell().Element(CellStyle).Text("Nenhuma foto registrada.").Italic();
                        });
                    }

                    // SEÇÃO 7: CONTROLE E ASSINATURAS
                    AddSectionHeader(col, "7. CONTROLE");

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        AddTableRow(table, "Data do Registro", Model.DataCriacao.ToString("dd/MM/yyyy HH:mm"));
                        AddTableRow(table, "Última Atualização", Model.Obra?.DataAtualizacao?.ToString("dd/MM/yyyy HH:mm") ?? "N/A");
                    });

                    if (!string.IsNullOrWhiteSpace(Model.AssinaturaResponsavel))
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns => columns.RelativeColumn());

                            table.Cell().Element(CellStyle).PaddingTop(10).Text("Responsável:").Bold();
                            table.Cell().Element(CellStyle).PaddingTop(15).AlignCenter().Text(Model.AssinaturaResponsavel);
                            table.Cell().Element(CellStyle).AlignCenter().PaddingBottom(5).BorderBottom(1).BorderColor(Colors.Black);
                            table.Cell().Element(CellStyle).AlignCenter().Text("Assinatura do Engenheiro Responsável");
                            table.Cell().Element(CellStyle).AlignCenter().Text(Model.DataAssinatura?.ToString("dd/MM/yyyy") ?? "");
                        });
                    }

                    // ... (código anterior permanece igual)

                    // SEÇÃO 8: ASSINATURAS
                    AddSectionHeader(col, "8. ASSINATURAS");

                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        // Assinatura do Engenheiro/Responsável
                        table.Cell().Column(column =>
                        {
                            column.Item().AlignCenter().Text("Responsável").Bold();
                            column.Item().PaddingTop(25).AlignCenter().Text("_________________________");
                            column.Item().AlignCenter().Text(Model.Obra?.EngenheiroResponsavel ?? "Nome do Engenheiro");
                            column.Item().AlignCenter().Text("CREA: _______________");
                        });

                        // Assinatura do Contratante
                        table.Cell().Column(column =>
                        {
                            column.Item().AlignCenter().Text("Contratante").Bold();
                            column.Item().PaddingTop(25).AlignCenter().Text("_________________________");
                            column.Item().AlignCenter().Text(Model.Obra?.Cliente ?? "Nome do Contratante");
                            column.Item().AlignCenter().Text("CPF/CNPJ: _______________");
                        });
                    });

                    // ... (rodapé permanece o mesmo)

                });



                // Rodapé
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ").FontSize(7);
                    x.CurrentPageNumber().FontSize(7);
                    x.Span(" de ").FontSize(7);
                    x.TotalPages().FontSize(7);
                    x.EmptyLine();
                    x.Span("Gerado em ").FontSize(7).Italic();
                    x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(7).Italic();
                });
            });
        }

        private void AddSectionHeader(ColumnDescriptor column, string title)
        {
            column.Item().Background(Colors.Grey.Lighten3).Padding(3).Text(title).Bold();
        }

        private void AddTableRow(TableDescriptor table, string label, string? value)
        {
            table.Cell().Element(CellStyle).Text(label).SemiBold();
            table.Cell().Element(CellStyle).Text(value ?? "N/A");
        }

        private IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(3)
                .PaddingHorizontal(5);
        }

        private string GetEtapaDisplayName(EtapaObraEnum etapa)
        {
            var fieldInfo = etapa.GetType().GetField(etapa.ToString());
            var attribute = fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)
                           .FirstOrDefault() as DisplayAttribute;

            return attribute?.Name ?? etapa.ToString();
        }

        private byte[] LoadImage(string imagePath)
        {
            // Se o caminho for uma URL ou caminho absoluto/relativo do sistema de arquivos
            if (File.Exists(imagePath))
            {
                return File.ReadAllBytes(imagePath);
            }

            // Se o caminho for uma URI (http/https)
            if (Uri.TryCreate(imagePath, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                using var httpClient = new HttpClient();
                return httpClient.GetByteArrayAsync(uri).Result;
            }

            // Se o caminho for um base64 string (data:image)
            if (imagePath.StartsWith("data:image"))
            {
                var base64Data = imagePath.Split(',')[1];
                return Convert.FromBase64String(base64Data);
            }

            throw new FileNotFoundException("Imagem não encontrada ou formato inválido");
        }

        private void AddFotoElement(IContainer container, FotoRegistro? foto)
        {
            if (foto == null)
            {
                return; // Não faz nada se não houver foto para esta posição
            }

            container.Padding(5).MinHeight(180).Column(column =>
            {
                try
                {
                    column.Item()
                        .Background(Colors.Grey.Lighten4)
                        .Padding(2)
                        .Image(LoadImage(foto.CaminhoArquivo), ImageScaling.FitArea);
                }
                catch
                {
                    column.Item().Background(Colors.Grey.Lighten3).Padding(5)
                        .AlignCenter().Text("Imagem não disponível").Italic();
                }

                // Legenda
                if (!string.IsNullOrWhiteSpace(foto.Descricao))
                {
                    column.Item().PaddingTop(5).Text(foto.Descricao).FontSize(7);
                }

                if (!string.IsNullOrWhiteSpace(foto.Categoria))
                {
                    column.Item().Text($"Categoria: {foto.Categoria}").FontSize(6).Italic();
                }
            });
        }
    }
}