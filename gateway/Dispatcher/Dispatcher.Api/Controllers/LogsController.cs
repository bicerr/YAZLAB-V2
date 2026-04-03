using Dispatcher.Application.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Dispatcher.Api.Controllers;

[ApiController]
[Route("admin")]
public class LogsController : ControllerBase
{
    private readonly ILogRepository _logRepository;

    public LogsController(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs()
    {
        var logs = await _logRepository.GetRecentAsync(100);
        var sb = new StringBuilder();

        sb.Append("<!DOCTYPE html><html><head><title>Dispatcher Log Tablosu</title><meta charset='UTF-8'>");
        sb.Append("<style>");
        sb.Append("body{font-family:Arial,sans-serif;margin:20px;background:#1e1e2e;color:#cdd6f4}");
        sb.Append("h1{color:#89b4fa}");
        sb.Append("table{width:100%;border-collapse:collapse;margin-top:20px}");
        sb.Append("th{background:#313244;color:#89b4fa;padding:12px;text-align:left}");
        sb.Append("td{padding:10px;border-bottom:1px solid #313244}");
        sb.Append("tr:hover{background:#313244}");
        sb.Append(".s200,.s201{color:#a6e3a1;font-weight:bold}");
        sb.Append(".s401,.s403,.s404{color:#fab387;font-weight:bold}");
        sb.Append(".s500{color:#f38ba8;font-weight:bold}");
        sb.Append(".badge{padding:3px 8px;border-radius:4px;font-size:12px;color:white}");
        sb.Append(".get{background:#1e66f5}.post{background:#40a02b}.put{background:#df8e1d}.delete{background:#d20f39}");
        sb.Append(".summary{display:flex;gap:20px;margin:20px 0}");
        sb.Append(".card{background:#313244;padding:15px 25px;border-radius:8px}");
        sb.Append(".card h3{margin:0;color:#89b4fa}.card p{margin:5px 0 0;font-size:24px;font-weight:bold}");
        sb.Append("</style></head><body>");
        sb.Append("<h1>Dispatcher Log Tablosu</h1>");

        sb.Append("<div class='summary'>");
        sb.Append($"<div class='card'><h3>Toplam İstek</h3><p>{logs.Count}</p></div>");
        sb.Append($"<div class='card'><h3>Başarılı</h3><p style='color:#a6e3a1'>{logs.Count(l => l.StatusCode < 400)}</p></div>");
        sb.Append($"<div class='card'><h3>Hatalı</h3><p style='color:#f38ba8'>{logs.Count(l => l.StatusCode >= 400)}</p></div>");
        sb.Append($"<div class='card'><h3>Son Güncelleme</h3><p style='font-size:14px'>{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p></div>");
        sb.Append("</div>");

        sb.Append("<table><thead><tr><th>#</th><th>Zaman</th><th>Method</th><th>Path</th><th>Status</th></tr></thead><tbody>");

        int i = 1;
        foreach (var l in logs)
        {
            var method = l.Method.ToLower();
            var statusClass = $"s{l.StatusCode}";
            sb.Append("<tr>");
            sb.Append($"<td>{i++}</td>");
            sb.Append($"<td>{l.Timestamp:yyyy-MM-dd HH:mm:ss}</td>");
            sb.Append($"<td><span class='badge {method}'>{l.Method}</span></td>");
            sb.Append($"<td>{l.Path}</td>");
            sb.Append($"<td class='{statusClass}'>{l.StatusCode}</td>");
            sb.Append("</tr>");
        }

        sb.Append("</tbody></table></body></html>");

        return Content(sb.ToString(), "text/html");
    }
}