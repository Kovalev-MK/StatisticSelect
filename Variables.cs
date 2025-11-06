using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSelect
{
  public static class Variables
  {
    public const string SqlStatistic = @"
            WITH
            total_counts AS (
                SELECT
                    SUM(CASE WHEN cp.discriminator = '593e143c-616c-4d95-9457-fd916c4aa7f8' THEN 1 ELSE 0 END) AS total_organizations,
                    SUM(CASE WHEN cp.discriminator = '80c4e311-e95f-449b-984d-1fd540b8f0af' THEN 1 ELSE 0 END) AS total_banks
                FROM sungero_parties_counterparty cp
            ),
            reliability_counts AS (
                SELECT
                    SUM(CASE WHEN discriminator = '593e143c-616c-4d95-9457-fd916c4aa7f8' AND reliability = 'High' THEN 1 ELSE 0 END) AS org_high,
                    SUM(CASE WHEN discriminator = '593e143c-616c-4d95-9457-fd916c4aa7f8' AND reliability = 'Medium' THEN 1 ELSE 0 END) AS org_medium,
                    SUM(CASE WHEN discriminator = '593e143c-616c-4d95-9457-fd916c4aa7f8' AND reliability = 'Low' THEN 1 ELSE 0 END) AS org_low,
                    SUM(CASE WHEN discriminator = '80c4e311-e95f-449b-984d-1fd540b8f0af' AND reliability = 'High' THEN 1 ELSE 0 END) AS bank_high,
                    SUM(CASE WHEN discriminator = '80c4e311-e95f-449b-984d-1fd540b8f0af' AND reliability = 'Medium' THEN 1 ELSE 0 END) AS bank_medium,
                    SUM(CASE WHEN discriminator = '80c4e311-e95f-449b-984d-1fd540b8f0af' AND reliability = 'Low' THEN 1 ELSE 0 END) AS bank_low
                FROM sungero_parties_counterparty
            )
            SELECT
                tc.total_organizations,
                tc.total_banks,
                rc.org_high,
                rc.org_medium,
                rc.org_low,
                rc.bank_high,
                rc.bank_medium,
                rc.bank_low,
                ROUND((rc.org_high::numeric / NULLIF(tc.total_organizations, 0)) * 100, 2) AS org_high_percent,
                ROUND((rc.org_medium::numeric / NULLIF(tc.total_organizations, 0)) * 100, 2) AS org_medium_percent,
                ROUND((rc.org_low::numeric / NULLIF(tc.total_organizations, 0)) * 100, 2) AS org_low_percent,
                ROUND((rc.bank_high::numeric / NULLIF(tc.total_banks, 0)) * 100, 2) AS bank_high_percent,
                ROUND((rc.bank_medium::numeric / NULLIF(tc.total_banks, 0)) * 100, 2) AS bank_medium_percent,
                ROUND((rc.bank_low::numeric / NULLIF(tc.total_banks, 0)) * 100, 2) AS bank_low_percent
            FROM total_counts tc, reliability_counts rc;";

    public const string SystemDBName = "DirRX";

    public const string SSLRootCert = @"C:\Users\kovalev_mk\.tsh\keys\teleport.directum24.ru\cas\tele.example.com.pem";
    public const string SSLCert = @"C:\Users\kovalev_mk\.tsh\keys\teleport.directum24.ru\kovalev_mk-db\tele.example.com\mt02-ub-x509.pem";
    public const string SSLKey = @"C:\Users\kovalev_mk\.tsh\keys\teleport.directum24.ru\kovalev_mk";
  }
}
