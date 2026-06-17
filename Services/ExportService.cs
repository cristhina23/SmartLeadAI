using System.Text;
using SmartLeadAI.Models;

namespace SmartLeadAI.Services;

public class ExportService
{
    public string GenerateCustomerCsv(List<Customer> customers)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Name,Email,Phone,InterestType,Status");
        
        foreach (var c in customers)
        {
            sb.AppendLine($"{c.FullName},{c.Email},{c.Phone},{c.InterestType},{c.Status}");
        }
        
        return sb.ToString();
    }

    public string GenerateInteractionsCsv(List<Interaction> interactions)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Date,Customer,Type,Summary");
        foreach (var i in interactions)
        {
            sb.AppendLine($"{i.InteractionDate:yyyy-MM-dd},{i.Customer?.FullName},{i.Type},{i.Notes?.Replace(",", " ")}");
        }
        return sb.ToString();
    }

    private string EscapeCsv(string? field)
    {
        if (string.IsNullOrEmpty(field)) return "";
        
        // If field contains a comma, wrap in double quotes
        if (field.Contains(","))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}

