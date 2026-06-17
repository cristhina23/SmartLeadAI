using Microsoft.Data.Sqlite;

var source = args[0];
var output = args[1];

if (!Path.GetFullPath(source).Equals(Path.GetFullPath(output), StringComparison.OrdinalIgnoreCase))
{
    File.Copy(source, output, overwrite: true);
}

await using var connection = new SqliteConnection($"Data Source={output}");
await connection.OpenAsync();

var inserts = new[]
{
    new
    {
        CustomerId = 1,
        UserId = 3,
        Type = "Email",
        Notes = "The rent of a tiny house.",
        InteractionDate = "2026-06-15 22:29:40.8636192",
        NextFollowUp = "2026-06-27 00:00:00"
    },
    new
    {
        CustomerId = 1,
        UserId = 2,
        Type = "Meeting",
        Notes = "The price of the Apartment of Heldon Street.",
        InteractionDate = "2026-06-15 22:46:16.3689209",
        NextFollowUp = "2026-06-30 00:00:00"
    }
};

foreach (var item in inserts)
{
    await using var exists = connection.CreateCommand();
    exists.CommandText = """
        select count(*)
        from Interactions
        where CustomerId = $customerId
          and UserId = $userId
          and Type = $type
          and Notes = $notes
          and InteractionDate = $interactionDate
          and NextFollowUp = $nextFollowUp
        """;
    exists.Parameters.AddWithValue("$customerId", item.CustomerId);
    exists.Parameters.AddWithValue("$userId", item.UserId);
    exists.Parameters.AddWithValue("$type", item.Type);
    exists.Parameters.AddWithValue("$notes", item.Notes);
    exists.Parameters.AddWithValue("$interactionDate", item.InteractionDate);
    exists.Parameters.AddWithValue("$nextFollowUp", item.NextFollowUp);

    if ((long)(await exists.ExecuteScalarAsync() ?? 0) > 0)
    {
        continue;
    }

    await using var insert = connection.CreateCommand();
    insert.CommandText = """
        insert into Interactions (CustomerId, UserId, Type, Notes, InteractionDate, NextFollowUp)
        values ($customerId, $userId, $type, $notes, $interactionDate, $nextFollowUp)
        """;
    insert.Parameters.AddWithValue("$customerId", item.CustomerId);
    insert.Parameters.AddWithValue("$userId", item.UserId);
    insert.Parameters.AddWithValue("$type", item.Type);
    insert.Parameters.AddWithValue("$notes", item.Notes);
    insert.Parameters.AddWithValue("$interactionDate", item.InteractionDate);
    insert.Parameters.AddWithValue("$nextFollowUp", item.NextFollowUp);
    await insert.ExecuteNonQueryAsync();
}

foreach (var table in new[] { "Companies", "Users", "Customers", "Interactions" })
{
    await using var count = connection.CreateCommand();
    count.CommandText = $"select count(*) from [{table}]";
    Console.WriteLine($"{table}: {await count.ExecuteScalarAsync()} rows");
}
