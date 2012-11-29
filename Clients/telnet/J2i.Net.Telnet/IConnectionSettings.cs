using System;
using System.Collections.Generic;
using System.Text;

namespace J2i.Net.Telnet
{
    public interface IConnectionSettings
    {
        string  ConnectionName      { get;set;}
        string  ConnectionAddress   { get;set;}
        int     Port                { get;set;}
        bool    BufferedInput       { get;set;}
        bool    LocalEcho           { get; set;}
        string  NewLineSequence     { get;set; }
        TerminalType TerminalType   { get; set;}


    }
}
