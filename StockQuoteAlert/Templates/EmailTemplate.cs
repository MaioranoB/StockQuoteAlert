using System.Text;
using StockQuoteAlert.Models;

namespace StockQuoteAlert.Templates;

public static class EmailTemplate
{
    public static (string Subject, string Body) Build(
        StockInfo stock,
        AlertType alertType,
        decimal threshold)
    {
        var subject = BuildSubject(stock, alertType);
        var body = BuildBody(stock, alertType, threshold);
        return (subject, body);
    }

    private static string BuildSubject(StockInfo stock, AlertType alertType) =>
        alertType == AlertType.Sell
            ? $"📈 VENDA — {stock.Symbol} atingiu R$ {stock.RegularMarketPrice:F2}"
            : $"📉 COMPRA — {stock.Symbol} caiu para R$ {stock.RegularMarketPrice:F2}";

    private static string BuildBody(StockInfo stock, AlertType alertType, decimal threshold)
    {
        bool isSell = alertType == AlertType.Sell;

        string accentColor = isSell ? "#e05c2a" : "#2a8ae0";
        string badgeBg = isSell ? "#fff4f0" : "#f0f6ff";
        string badgeText = isSell ? "#c44d1e" : "#1e6ec4";
        string arrow = isSell ? "↑" : "↓";
        string actionLabel = isSell ? "SINAL DE VENDA" : "SINAL DE COMPRA";

        string thresholdLabel = isSell
            ? $"Preço ultrapassou o limite de venda de <strong>R$ {threshold:F2}</strong>"
            : $"Preço caiu abaixo do limite de compra de <strong>R$ {threshold:F2}</strong>";

        string recommendation = isSell
            ? "Com base no limite configurado, considere <strong>realizar a venda</strong> desta posição."
            : "Com base no limite configurado, considere <strong>adquirir</strong> este ativo.";

        return $"""
            <!DOCTYPE html>
            <html lang="pt-BR">
            <head>
              <meta charset="UTF-8">
              <meta name="viewport" content="width=device-width, initial-scale=1.0">
              <title>Alerta de Cotação</title>
            </head>
            <body style="margin:0;padding:0;background:#f2f3f5;font-family:'Georgia',serif;">

              <!-- Wrapper -->
              <table width="100%" cellpadding="0" cellspacing="0" style="background:#f2f3f5;padding:40px 16px;">
                <tr><td align="center">

                  <!-- Card -->
                  <table width="560" cellpadding="0" cellspacing="0"
                         style="background:#ffffff;border-radius:12px;overflow:hidden;
                                box-shadow:0 4px 24px rgba(0,0,0,0.08);max-width:560px;width:100%;">

                    <!-- Header bar -->
                    <tr>
                      <td style="background:{accentColor};padding:0;height:5px;"></td>
                    </tr>

                    <!-- Logo + ticker row -->
                    <tr>
                      <td style="padding:32px 40px 24px;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                          <tr>
                            <td style="vertical-align:middle;">
                              <img src="{stock.LogoUrl}" width="52" height="52"
                                   alt="{stock.Symbol}"
                                   style="border-radius:50%;border:2px solid #eee;
                                          display:block;object-fit:contain;background:#fff;" />
                            </td>
                            <td style="vertical-align:middle;padding-left:16px;">
                              <div style="font-size:22px;font-weight:bold;color:#111;
                                          letter-spacing:-0.5px;">{stock.Symbol}</div>
                              <div style="font-size:13px;color:#888;margin-top:2px;">
                                {stock.LongName}
                              </div>
                            </td>
                            <td align="right" style="vertical-align:middle;">
                              <span style="display:inline-block;background:{badgeBg};
                                           color:{badgeText};border:1px solid {accentColor};
                                           border-radius:6px;padding:6px 14px;
                                           font-size:12px;font-weight:bold;
                                           letter-spacing:1px;">
                                {arrow} {actionLabel}
                              </span>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>

                    <!-- Price block -->
                    <tr>
                      <td style="padding:0 40px 28px;">
                        <table width="100%" cellpadding="0" cellspacing="0"
                               style="background:#fafafa;border:1px solid #ebebeb;
                                      border-radius:10px;padding:24px;">
                          <tr>
                            <td>
                              <div style="font-size:12px;color:#999;
                                          text-transform:uppercase;letter-spacing:1px;
                                          margin-bottom:6px;">Preço atual</div>
                              <div style="font-size:42px;font-weight:bold;color:{accentColor};
                                          letter-spacing:-1px;line-height:1;">
                                R$ {stock.RegularMarketPrice:F2}
                              </div>
                            </td>
                            <td align="right" style="vertical-align:bottom;">
                              <div style="font-size:12px;color:#aaa;margin-bottom:4px;">
                                Limite configurado
                              </div>
                              <div style="font-size:22px;font-weight:bold;color:#555;">
                                R$ {threshold:F2}
                              </div>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>

                    <!-- Divider -->
                    <tr>
                      <td style="padding:0 40px;">
                        <div style="height:1px;background:#ebebeb;"></div>
                      </td>
                    </tr>

                    <!-- Message -->
                    <tr>
                      <td style="padding:28px 40px;">
                        <p style="margin:0 0 12px;font-size:15px;color:#333;line-height:1.7;">
                          {thresholdLabel}.
                        </p>
                        <p style="margin:0;font-size:15px;color:#333;line-height:1.7;">
                          {recommendation}
                        </p>
                      </td>
                    </tr>

                    <!-- Details table -->
                    <tr>
                      <td style="padding:0 40px 32px;">
                        <table width="100%" cellpadding="0" cellspacing="0"
                               style="border:1px solid #ebebeb;border-radius:8px;
                                      overflow:hidden;font-size:13px;">
                          {DetailRow("Ativo", stock.Symbol, true)}
                          {DetailRow("Nome completo", stock.LongName, false)}
                          {DetailRow("Moeda", stock.Currency, true)}
                          {DetailRow("Horário do alerta", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), false)}
                        </table>
                      </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                      <td style="background:#fafafa;border-top:1px solid #ebebeb;
                                 padding:20px 40px;border-radius:0 0 12px 12px;">
                        <p style="margin:0;font-size:11px;color:#bbb;line-height:1.6;
                                  text-align:center;">
                          Este é um alerta automático gerado pelo <strong>Stock Quote Alert</strong>.<br>
                          Não constitui recomendação de investimento. Consulte um assessor financeiro.
                        </p>
                      </td>
                    </tr>

                  </table>
                  <!-- /Card -->

                </td></tr>
              </table>
              <!-- /Wrapper -->

            </body>
            </html>
            """;
    }

    private static string DetailRow(string label, string value, bool shaded)
    {
        string bg = shaded ? "background:#fafafa;" : "background:#ffffff;";
        return $"""
            <tr>
              <td style="{bg}padding:10px 16px;color:#888;border-bottom:1px solid #ebebeb;">{label}</td>
              <td style="{bg}padding:10px 16px;color:#333;font-weight:bold;
                         border-bottom:1px solid #ebebeb;text-align:right;">{value}</td>
            </tr>
            """;
    }
}