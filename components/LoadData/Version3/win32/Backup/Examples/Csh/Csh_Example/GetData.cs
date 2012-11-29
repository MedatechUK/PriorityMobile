using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Loading;
//---------------------------------------------------------------------
//  Copyright (C) eMerge-IT.  All rights reserved.
// 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------

static class GetData
{

    public static void Main()
    {
        string Environment = "demo";
        Csh_Example.PriWebSVC.Service ws = new Csh_Example.PriWebSVC.Service();
        Loading.SerialData sd = new Loading.SerialData();
        string SQL = "SELECT W1URL, W1USER, W1PASS, W1STKEY, " + "ACCOUNTS.ACCNAME AS ACCNAME" + "FROM COMPDATA, ACCOUNTS " + "WHERE COMP <> 0" + "AND COMPDATA.W1ACCOUNT = ACCOUNTS.ACCOUNT";
        Loading.ColumnDef cd = new Loading.ColumnDef(SQL);

        sd.FromStr(ws.GetData(SQL, Environment));
        /*
        if (sd.GetDataError == null)
        {
            if ((sd.Data != null))
            {
                for (int i = 0; i <= sd.RowCount - 1; i++)
                {
                    Console.Write(sd.Data(cd.Column("W1URL"), i), sd.Data(cd.Column("W1USER"), i), sd.Data(cd.Column("W1PASS"), i), sd.Data(cd.Column("W1STKEY"), i), sd.Data(cd.Column("ACCNAME"), i));
                }
            }
            else
            {
                throw new Exception("No data.");
            }
        }
        else
        {
            throw new Exception(sd.GetDataError.Message);
        }
         */
    }

}