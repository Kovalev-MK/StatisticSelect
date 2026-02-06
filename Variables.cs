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
          select 
    ent.id,
    ent.accessrights,
    ent.recipient,
    ent.accessrightstype,
    sca.*,
    sse.typeguid,
    sse.typename,
    scr.sid as role_sid,
    scr.name as recipient_name
from public.sungero_core_accessrightent ent
inner join public.sungero_core_accessrights sca on ent.accessrights = sca.id
inner join sungero_system_entitytype sse on sse.typeguid = sca.entitytypeguid
inner join public.sungero_core_recipient scr on ent.recipient = scr.id
where scr.sid in (
    '9cc6ea59-cd05-4c8e-b041-abefe9432e20', -- Administrators
    '80b8c40f-3200-4eef-bd38-3bd6058da6f1', -- Auditors
    'fd3e67bf-cf3f-431d-9863-7dbc2518237e', -- ConfigurationManagers
    '5025795b-1042-4841-8ed7-bb0460a3374c', -- ServiceUsers
    '94CD8E7F-D754-4FE3-9901-BA02588C5597', -- SoloUsers
    '47067570-0c13-4756-9036-f4a78ff3d932', -- IntegrationServiceUsers
    '776145b3-77a8-40f8-b384-a17db39fa94d', -- UsersAuthorizedToBulkExport
    '9434CED8-E760-4F17-AD85-560F88EB5499', -- ReservedLicenseUsers
    'e473e9ec-df3f-4ee7-bdc4-2b8a7782faab'  -- NonInteractiveUsers
);";

    public const string SystemDBName = "DirRX";

    public const string SSLRootCert = @"C:\Users\kovalev_mk\.tsh\keys\teleport.directum24.ru\cas\tele.example.com.pem";
    public const string SSLCert = @"C:\Users\kovalev_mk\.tsh\keys\teleport.directum24.ru\kovalev_mk-db\tele.example.com\mt02-ub-x509.pem";
    public const string SSLKey = @"C:\Users\kovalev_mk\.tsh\keys\teleport.directum24.ru\kovalev_mk";

    public const int Port = 61243;
    public const string Password = "";
  }
}
