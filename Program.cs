using Npgsql;
using ClosedXML.Excel;
using StatisticSelect;

class Program
{
  static void Main()
  {
    Console.WriteLine("Скрипт запущен");
    var workbook = new XLWorkbook();
    var sheet = workbook.Worksheets.Add("Statistic");
    sheet.Columns().Width = 30;

    var dbList = GetDbList();
    foreach (var db in dbList)
    {
      Console.WriteLine(db);
      GetStatsticToFile(db, workbook);
    }
    workbook.SaveAs("./statistic.xlsx");
  }

  private static void GetStatsticToFile(string dbName, XLWorkbook book)
  {
    var statisticSheet = book.Worksheets.Where(x => x.Name == "Statistic").First();
    var connectionString = GetConnectionString(dbName);
    using (var conn = new NpgsqlConnection(connectionString))
    {
      conn.Open();
      using (var cmd = new NpgsqlCommand(Variables.SqlStatistic, conn))
      {
        try
        {
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              if (statisticSheet.LastRowUsed() == null)
              {
                FillColumnNames(reader, statisticSheet);
              }

              var row = statisticSheet.LastRowUsed()!.RowNumber() + 1;
              for (int col = 0; col < reader.FieldCount; col++)
              {
                statisticSheet.Cell(row, col + 1).Value = reader[col].ToString();
              }
            }
          }
        }
        catch (PostgresException ex)
        {
          Console.WriteLine($"Error on DB \"{dbName}\": {ex.Message}");
          var errorSheet = book.Worksheets.Where(x => x.Name == "Error db names").FirstOrDefault();
          var row = 1;
          if (errorSheet == null)
          {
            errorSheet = book.Worksheets.Add("Error db names");
            errorSheet.ColumnWidth = 30;
          }
          else
            row = errorSheet.LastRowUsed()!.RowNumber() + 1;

          errorSheet.Cell(row, 1).Value = dbName;
          errorSheet.Cell(row, 2).Value = ex.Message;
        }
      }
    }
  }

  private static void FillColumnNames(NpgsqlDataReader reader, IXLWorksheet sheet)
  {
    for (int i = 0; i < reader.FieldCount; i++)
    {
      string columnName = reader.GetName(i);
      sheet.Cell(1, i + 1).Value = columnName;
    }
  }

  private static IEnumerable<string> GetDbList()
  {
    string sqlDbList = @"SELECT datname FROM pg_database WHERE datistemplate = false;";
    var connectionString = GetConnectionString(Variables.SystemDBName);
    using (var conn = new NpgsqlConnection(connectionString))
    {
      conn.Open();
      using (var cmd = new NpgsqlCommand(sqlDbList, conn))
      {
        using (var reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            var rowValues = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
              rowValues.Add(reader[i].ToString());
            }
            yield return string.Join("; ", rowValues);
          }
        }
      }
    }
  }

  private static string GetConnectionString(string dbName)
  {
    var sb = new NpgsqlConnectionStringBuilder();
    sb.Host = "teleport.directum24.ru";
    sb.Port = 443;
    sb.Username = "adviser";
    sb.Password = "YUTebh7263ukg%$^yjthgff";
    sb.SslMode = SslMode.VerifyFull;
    sb.RootCertificate = Variables.SSLRootCert;
    sb.SslKey = Variables.SSLKey;
    sb.SslCertificate = Variables.SSLCert;
    sb.Database = dbName;
    return sb.ConnectionString;
  }
}
