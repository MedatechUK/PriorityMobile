using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using priority;

/*
 * The Following example generates the XML shown below and posts it to the service.

<?xml version="1.0" encoding="UTF-16"?>
<PriorityLoading PROCEDURE="ZSFDC_TEST" TABLE="ZSFDC_TABLE" ENVIRONMENT="company"> 
    <ROW CQUANT="0" STATUS="''" PART="''" CURDATE="13060097" BIN="'0'" WARHS="'Main'" USERNAME="'user'" RECORDTYPE="1"/> 
    <ROW CQUANT="1000" STATUS="'Goods'" PART="'PART123'" CURDATE="0" BIN="''" WARHS="''" USERNAME="''" RECORDTYPE="2"/> 
    <ROW CQUANT="1000" STATUS="'Goods'" PART="'PART321'" CURDATE="0" BIN="''" WARHS="''" USERNAME="''" RECORDTYPE="2"/> 
</PriorityLoading>

 *  
*/

static class example
{
    public static void Main()
    {
        string ServiceURL = "http://localhost:8080";
        using (Loading xl = new Loading())
        {            
            try
            {
                xl.Table = "ZSFDC_TABLE";
                xl.Procedure = "ZSFDC_TEST";
                xl.Environment = "company";

                xl.set_AddColumn(1, new LoadColumn("USERNAME", tColumnType.typeCHAR));
                xl.set_AddColumn(1, new LoadColumn("WARHS", tColumnType.typeCHAR));
                xl.set_AddColumn(1, new LoadColumn("BIN", tColumnType.typeCHAR));
                xl.set_AddColumn(1, new LoadColumn("CURDATE", tColumnType.typeDATE));
                xl.set_AddColumn(2, new LoadColumn("PART", tColumnType.typeCHAR));
                xl.set_AddColumn(2, new LoadColumn("STATUS", tColumnType.typeCHAR));
                xl.set_AddColumn(2, new LoadColumn("CQUANT", tColumnType.typeINT));

                xl.set_AddRecordType(1, new LoadRow("user", "Main", "0", DateTime.Now.ToString()));
                xl.set_AddRecordType(2, new LoadRow("PART123", "Goods", "1"));
                xl.set_AddRecordType(2, new LoadRow("PART321", "Goods", "1"));

                Exception exp = new Exception();
                if (!xl.Post(ServiceURL, ref exp)) throw exp;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
