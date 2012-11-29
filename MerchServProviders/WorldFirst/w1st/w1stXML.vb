Imports System
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.xml
Imports System.Security.Cryptography
Imports System.Reflection
Imports System.ComponentModel

#Region "ISO Types"

Public Class ISO3166

    Public Const AFGHANISTAN = "AF"
    Public Const ÅLAND_ISLANDS = "AX"
    Public Const ALBANIA = "AL"
    Public Const ALGERIA = "DZ"
    Public Const AMERICAN_SAMOA = "AS"
    Public Const ANDORRA = "AD"
    Public Const ANGOLA = "AO"
    Public Const ANGUILLA = "AI"
    Public Const ANTARCTICA = "AQ"
    Public Const ANTIGUA_AND_BARBUDA = "AG"
    Public Const ARGENTINA = "AR"
    Public Const ARMENIA = "AM"
    Public Const ARUBA = "AW"
    Public Const AUSTRALIA = "AU"
    Public Const AUSTRIA = "AT"
    Public Const AZERBAIJAN = "AZ"
    Public Const BAHAMAS = "BS"
    Public Const BAHRAIN = "BH"
    Public Const BANGLADESH = "BD"
    Public Const BARBADOS = "BB"
    Public Const BELARUS = "BY"
    Public Const BELGIUM = "BE"
    Public Const BELIZE = "BZ"
    Public Const BENIN = "BJ"
    Public Const BERMUDA = "BM"
    Public Const BHUTAN = "BT"
    Public Const BOLIVIA_PLURINATIONAL_STATE_OF = "BO"
    Public Const BOSNIA_AND_HERZEGOVINA = "BA"
    Public Const BOTSWANA = "BW"
    Public Const BOUVET_ISLAND = "BV"
    Public Const BRAZIL = "BR"
    Public Const BRITISH_INDIAN_OCEAN_TERRITORY = "IO"
    Public Const BRUNEI_DARUSSALAM = "BN"
    Public Const BULGARIA = "BG"
    Public Const BURKINA_FASO = "BF"
    Public Const BURUNDI = "BI"
    Public Const CAMBODIA = "KH"
    Public Const CAMEROON = "CM"
    Public Const CANADA = "CA"
    Public Const CAPE_VERDE = "CV"
    Public Const CAYMAN_ISLANDS = "KY"
    Public Const CENTRAL_AFRICAN_REPUBLIC = "CF"
    Public Const CHAD = "TD"
    Public Const CHILE = "CL"
    Public Const CHINA = "CN"
    Public Const CHRISTMAS_ISLAND = "CX"
    Public Const COCOS_KEELING_ISLANDS = "CC"
    Public Const COLOMBIA = "CO"
    Public Const COMOROS = "KM"
    Public Const CONGO = "CG"
    Public Const CONGO_THE_DEMOCRATIC_REPUBLIC_OF_THE = "CD"
    Public Const COOK_ISLANDS = "CK"
    Public Const COSTA_RICA = "CR"
    Public Const CÔTE_DIVOIRE = "CI"
    Public Const CROATIA = "HR"
    Public Const CUBA = "CU"
    Public Const CYPRUS = "CY"
    Public Const CZECH_REPUBLIC = "CZ"
    Public Const DENMARK = "DK"
    Public Const DJIBOUTI = "DJ"
    Public Const DOMINICA = "DM"
    Public Const DOMINICAN_REPUBLIC = "DO"
    Public Const ECUADOR = "EC"
    Public Const EGYPT = "EG"
    Public Const EL_SALVADOR = "SV"
    Public Const EQUATORIAL_GUINEA = "GQ"
    Public Const ERITREA = "ER"
    Public Const ESTONIA = "EE"
    Public Const ETHIOPIA = "ET"
    Public Const FALKLAND_ISLANDS_MALVINAS = "FK"
    Public Const FAROE_ISLANDS = "FO"
    Public Const FIJI = "FJ"
    Public Const FINLAND = "FI"
    Public Const FRANCE = "FR"
    Public Const FRENCH_GUIANA = "GF"
    Public Const FRENCH_POLYNESIA = "PF"
    Public Const FRENCH_SOUTHERN_TERRITORIES = "TF"
    Public Const GABON = "GA"
    Public Const GAMBIA = "GM"
    Public Const GEORGIA = "GE"
    Public Const GERMANY = "DE"
    Public Const GHANA = "GH"
    Public Const GIBRALTAR = "GI"
    Public Const GREECE = "GR"
    Public Const GREENLAND = "GL"
    Public Const GRENADA = "GD"
    Public Const GUADELOUPE = "GP"
    Public Const GUAM = "GU"
    Public Const GUATEMALA = "GT"
    Public Const GUERNSEY = "GG"
    Public Const GUINEA = "GN"
    Public Const GUINEA_BISSAU = "GW"
    Public Const GUYANA = "GY"
    Public Const HAITI = "HT"
    Public Const HEARD_ISLAND_AND_MCDONALD_ISLANDS = "HM"
    Public Const HOLY_SEE_VATICAN_CITY_STATE = "VA"
    Public Const HONDURAS = "HN"
    Public Const HONG_KONG = "HK"
    Public Const HUNGARY = "HU"
    Public Const ICELAND = "IS"
    Public Const INDIA = "IN"
    Public Const INDONESIA = "ID"
    Public Const IRAN_ISLAMIC_REPUBLIC_OF = "IR"
    Public Const IRAQ = "IQ"
    Public Const IRELAND = "IE"
    Public Const ISLE_OF_MAN = "IM"
    Public Const ISRAEL = "IL"
    Public Const ITALY = "IT"
    Public Const JAMAICA = "JM"
    Public Const JAPAN = "JP"
    Public Const JERSEY = "JE"
    Public Const JORDAN = "JO"
    Public Const KAZAKHSTAN = "KZ"
    Public Const KENYA = "KE"
    Public Const KIRIBATI = "KI"
    Public Const KOREA_DEMOCRATIC_PEOPLES_REPUBLIC_OF = "KP"
    Public Const KOREA_REPUBLIC_OF = "KR"
    Public Const KUWAIT = "KW"
    Public Const KYRGYZSTAN = "KG"
    Public Const LAO_PEOPLES_DEMOCRATIC_REPUBLIC = "LA"
    Public Const LATVIA = "LV"
    Public Const LEBANON = "LB"
    Public Const LESOTHO = "LS"
    Public Const LIBERIA = "LR"
    Public Const LIBYAN_ARAB_JAMAHIRIYA = "LY"
    Public Const LIECHTENSTEIN = "LI"
    Public Const LITHUANIA = "LT"
    Public Const LUXEMBOURG = "LU"
    Public Const MACAO = "MO"
    Public Const MACEDONIA_THE_FORMER_YUGOSLAV_REPUBLIC_OF = "MK"
    Public Const MADAGASCAR = "MG"
    Public Const MALAWI = "MW"
    Public Const MALAYSIA = "MY"
    Public Const MALDIVES = "MV"
    Public Const MALI = "ML"
    Public Const MALTA = "MT"
    Public Const MARSHALL_ISLANDS = "MH"
    Public Const MARTINIQUE = "MQ"
    Public Const MAURITANIA = "MR"
    Public Const MAURITIUS = "MU"
    Public Const MAYOTTE = "YT"
    Public Const MEXICO = "MX"
    Public Const MICRONESIA_FEDERATED_STATES_OF = "FM"
    Public Const MOLDOVA_REPUBLIC_OF = "MD"
    Public Const MONACO = "MC"
    Public Const MONGOLIA = "MN"
    Public Const MONTENEGRO = "ME"
    Public Const MONTSERRAT = "MS"
    Public Const MOROCCO = "MA"
    Public Const MOZAMBIQUE = "MZ"
    Public Const MYANMAR = "MM"
    Public Const NAMIBIA = "NA"
    Public Const NAURU = "NR"
    Public Const NEPAL = "NP"
    Public Const NETHERLANDS = "NL"
    Public Const NETHERLANDS_ANTILLES = "AN"
    Public Const NEW_CALEDONIA = "NC"
    Public Const NEW_ZEALAND = "NZ"
    Public Const NICARAGUA = "NI"
    Public Const NIGER = "NE"
    Public Const NIGERIA = "NG"
    Public Const NIUE = "NU"
    Public Const NORFOLK_ISLAND = "NF"
    Public Const NORTHERN_MARIANA_ISLANDS = "MP"
    Public Const NORWAY = "NO"
    Public Const OMAN = "OM"
    Public Const PAKISTAN = "PK"
    Public Const PALAU = "PW"
    Public Const PANAMA = "PA"
    Public Const PAPUA_NEW_GUINEA = "PG"
    Public Const PARAGUAY = "PY"
    Public Const PERU = "PE"
    Public Const PHILIPPINES = "PH"
    Public Const PITCAIRN = "PN"
    Public Const POLAND = "PL"
    Public Const PORTUGAL = "PT"
    Public Const PUERTO_RICO = "PR"
    Public Const QATAR = "QA"
    Public Const REUNION = "RE"
    Public Const ROMANIA = "RO"
    Public Const RUSSIAN_FEDERATION = "RU"
    Public Const RWANDA = "RW"
    Public Const SAINT_BARTHELEMY = "BL"
    Public Const SAINT_HELENA_ASCENSION_AND_TRISTAN_DA_CUNHA = "SH"
    Public Const SAINT_KITTS_AND_NEVIS = "KN"
    Public Const SAINT_LUCIA = "LC"
    Public Const SAINT_MARTIN = "MF"
    Public Const SAINT_PIERRE_AND_MIQUELON = "PM"
    Public Const SAINT_VINCENT_AND_THE_GRENADINES = "VC"
    Public Const SAMOA = "WS"
    Public Const SAN_MARINO = "SM"
    Public Const SAO_TOME_AND_PRINCIPE = "ST"
    Public Const SAUDI_ARABIA = "SA"
    Public Const SENEGAL = "SN"
    Public Const SERBIA = "RS"
    Public Const SEYCHELLES = "SC"
    Public Const SIERRA_LEONE = "SL"
    Public Const SINGAPORE = "SG"
    Public Const SLOVAKIA = "SK"
    Public Const SLOVENIA = "SI"
    Public Const SOLOMON_ISLANDS = "SB"
    Public Const SOMALIA = "SO"
    Public Const SOUTH_AFRICA = "ZA"
    Public Const SOUTH_GEORGIA_AND_THE_SOUTH_SANDWICH_ISLANDS = "GS"
    Public Const SPAIN = "ES"
    Public Const SRI_LANKA = "LK"
    Public Const SUDAN = "SD"
    Public Const SURINAME = "SR"
    Public Const SVALBARD_AND_JAN_MAYEN = "SJ"
    Public Const SWAZILAND = "SZ"
    Public Const SWEDEN = "SE"
    Public Const SWITZERLAND = "CH"
    Public Const SYRIAN_ARAB_REPUBLIC = "SY"
    Public Const TAIWAN_PROVINCE_OF_CHINA = "TW"
    Public Const TAJIKISTAN = "TJ"
    Public Const TANZANIA_UNITED_REPUBLIC_OF = "TZ"
    Public Const THAILAND = "TH"
    Public Const TIMOR_LESTE = "TL"
    Public Const TOGO = "TG"
    Public Const TOKELAU = "TK"
    Public Const TONGA = "TO"
    Public Const TRINIDAD_AND_TOBAGO = "TT"
    Public Const TUNISIA = "TN"
    Public Const TURKEY = "TR"
    Public Const TURKMENISTAN = "TM"
    Public Const TURKS_AND_CAICOS_ISLANDS = "TC"
    Public Const TUVALU = "TV"
    Public Const UGANDA = "UG"
    Public Const UKRAINE = "UA"
    Public Const UNITED_ARAB_EMIRATES = "AE"
    Public Const UNITED_KINGDOM = "GB"
    Public Const UNITED_STATES = "US"
    Public Const UNITED_STATES_MINOR_OUTLYING_ISLANDS = "UM"
    Public Const URUGUAY = "UY"
    Public Const UZBEKISTAN = "UZ"
    Public Const VANUATU = "VU"
    Public Const VENEZUELA_BOLIVARIAN_REPUBLIC_OF = "VE"
    Public Const VIET_NAM = "VN"
    Public Const VIRGIN_ISLANDS_BRITISH = "VG"
    Public Const VIRGIN_ISLANDS_US = "VI"
    Public Const WALLIS_AND_FUTUNA = "WF"
    Public Const WESTERN_SAHARA = "EH"
    Public Const YEMEN = "YE"
    Public Const ZAMBIA = "ZM"
    Public Const ZIMBABWE = "ZW"

End Class

Public Class ISO4217

    Public Const Afghanistan_afghani = "AFA"
    Public Const Albanian_lek = "ALL"
    Public Const Algerian_dinar = "DZD"
    Public Const Angolan_kwanza_reajustado = "AOR"
    Public Const Argentine_peso = "ARS"
    Public Const Armenian_dram = "AMD"
    Public Const Aruban_guilder = "AWG"
    Public Const Australian_dollar = "AUD"
    Public Const Azerbaijanian_new_manat = "AZN"
    Public Const Bahamian_dollar = "BSD"
    Public Const Bahraini_dinar = "BHD"
    Public Const Bangladeshi_taka = "BDT"
    Public Const Barbados_dollar = "BBD"
    Public Const Belarusian_ruble = "BYR"
    Public Const Belize_dollar = "BZD"
    Public Const Bermudian_dollar = "BMD"
    Public Const Bhutan_ngultrum = "BTN"
    Public Const Bolivian_boliviano = "BOB"
    Public Const Botswana_pula = "BWP"
    Public Const Brazilian_real = "BRL"
    Public Const British_pound = "GBP"
    Public Const Brunei_dollar = "BND"
    Public Const Bulgarian_lev = "BGN"
    Public Const Burundi_franc = "BIF"
    Public Const Cambodian_riel = "KHR"
    Public Const Canadian_dollar = "CAD"
    Public Const Cape_Verde_escudo = "CVE"
    Public Const Cayman_Islands_dollar = "KYD"
    Public Const CFA_franc_BCEAO = "XOF"
    Public Const CFA_franc_BEAC = "XAF"
    Public Const CFP_franc = "XPF"
    Public Const Chilean_peso = "CLP"
    Public Const Chinese_yuan_renminbi = "CNY"
    Public Const Colombian_peso = "COP"
    Public Const Comoros_franc = "KMF"
    Public Const Congolese_franc = "CDF"
    Public Const Costa_Rican_colon = "CRC"
    Public Const Croatian_kuna = "HRK"
    Public Const Cuban_peso = "CUP"
    Public Const Czech_koruna = "CZK"
    Public Const Danish_krone = "DKK"
    Public Const Djibouti_franc = "DJF"
    Public Const Dominican_peso = "DOP"
    Public Const East_Caribbean_dollar = "XCD"
    Public Const Egyptian_pound = "EGP"
    Public Const El_Salvador_colon = "SVC"
    Public Const Eritrean_nakfa = "ERN"
    Public Const Estonian_kroon = "EEK"
    Public Const Ethiopian_birr = "ETB"
    Public Const EU_euro = "EUR"
    Public Const Falkland_Islands_pound = "FKP"
    Public Const Fiji_dollar = "FJD"
    Public Const Gambian_dalasi = "GMD"
    Public Const Georgian_lari = "GEL"
    Public Const Ghanaian_new_cedi = "GHS"
    Public Const Gibraltar_pound = "GIP"
    Public Const Gold_ounce = "XAU"
    Public Const Gold_franc = "XFO"
    Public Const Guatemalan_quetzal = "GTQ"
    Public Const Guinean_franc = "GNF"
    Public Const Guyana_dollar = "GYD"
    Public Const Haitian_gourde = "HTG"
    Public Const Honduran_lempira = "HNL"
    Public Const Hong_Kong_SAR_dollar = "HKD"
    Public Const Hungarian_forint = "HUF"
    Public Const Icelandic_krona = "ISK"
    Public Const IMF_special_drawing_right = "XDR"
    Public Const Indian_rupee = "INR"
    Public Const Indonesian_rupiah = "IDR"
    Public Const Iranian_rial = "IRR"
    Public Const Iraqi_dinar = "IQD"
    Public Const Israeli_new_shekel = "ILS"
    Public Const Jamaican_dollar = "JMD"
    Public Const Japanese_yen = "JPY"
    Public Const Jordanian_dinar = "JOD"
    Public Const Kazakh_tenge = "KZT"
    Public Const Kenyan_shilling = "KES"
    Public Const Kuwaiti_dinar = "KWD"
    Public Const Kyrgyz_som = "KGS"
    Public Const Lao_kip = "LAK"
    Public Const Latvian_lats = "LVL"
    Public Const Lebanese_pound = "LBP"
    Public Const Lesotho_loti = "LSL"
    Public Const Liberian_dollar = "LRD"
    Public Const Libyan_dinar = "LYD"
    Public Const Lithuanian_litas = "LTL"
    Public Const Macao_SAR_pataca = "MOP"
    Public Const Macedonian_denar = "MKD"
    Public Const Malagasy_ariary = "MGA"
    Public Const Malawi_kwacha = "MWK"
    Public Const Malaysian_ringgit = "MYR"
    Public Const Maldivian_rufiyaa = "MVR"
    Public Const Mauritanian_ouguiya = "MRO"
    Public Const Mauritius_rupee = "MUR"
    Public Const Mexican_peso = "MXN"
    Public Const Moldovan_leu = "MDL"
    Public Const Mongolian_tugrik = "MNT"
    Public Const Moroccan_dirham = "MAD"
    Public Const Mozambique_new_metical = "MZN"
    Public Const Myanmar_kyat = "MMK"
    Public Const Namibian_dollar = "NAD"
    Public Const Nepalese_rupee = "NPR"
    Public Const Netherlands_Antillian_guilder = "ANG"
    Public Const New_Zealand_dollar = "NZD"
    Public Const Nicaraguan_cordoba_oro = "NIO"
    Public Const Nigerian_naira = "NGN"
    Public Const North_Korean_won = "KPW"
    Public Const Norwegian_krone = "NOK"
    Public Const Omani_rial = "OMR"
    Public Const Pakistani_rupee = "PKR"
    Public Const Palladium_ounce = "XPD"
    Public Const Panamanian_balboa = "PAB"
    Public Const Papua_New_Guinea_kina = "PGK"
    Public Const Paraguayan_guarani = "PYG"
    Public Const Peruvian_nuevo_sol = "PEN"
    Public Const Philippine_peso = "PHP"
    Public Const Platinum_ounce = "XPT"
    Public Const Polish_zloty = "PLN"
    Public Const Qatari_rial = "QAR"
    Public Const Romanian_new_leu = "RON"
    Public Const Russian_ruble = "RUB"
    Public Const Rwandan_franc = "RWF"
    Public Const Saint_Helena_pound = "SHP"
    Public Const Samoan_tala = "WST"
    Public Const Sao_Tome_and_Principe_dobra = "STD"
    Public Const Saudi_riyal = "SAR"
    Public Const Serbian_dinar = "RSD"
    Public Const Seychelles_rupee = "SCR"
    Public Const Sierra_Leone_leone = "SLL"
    Public Const Silver_ounce = "XAG"
    Public Const Singapore_dollar = "SGD"
    Public Const Solomon_Islands_dollar = "SBD"
    Public Const Somali_shilling = "SOS"
    Public Const South_African_rand = "ZAR"
    Public Const South_Korean_won = "KRW"
    Public Const Sri_Lanka_rupee = "LKR"
    Public Const Sudanese_pound = "SDG"
    Public Const Suriname_dollar = "SRD"
    Public Const Swaziland_lilangeni = "SZL"
    Public Const Swedish_krona = "SEK"
    Public Const Swiss_franc = "CHF"
    Public Const Syrian_pound = "SYP"
    Public Const Taiwan_New_dollar = "TWD"
    Public Const Tajik_somoni = "TJS"
    Public Const Tanzanian_shilling = "TZS"
    Public Const Thai_baht = "THB"
    Public Const Tongan_paanga = "TOP"
    Public Const Trinidad_and_Tobago_dollar = "TTD"
    Public Const Tunisian_dinar = "TND"
    Public Const Turkish_lira = "TRY"
    Public Const Turkmen_new_manat = "TMT"
    Public Const UAE_dirham = "AED"
    Public Const Uganda_new_shilling = "UGX"
    Public Const UIC_franc = "XFU"
    Public Const Ukrainian_hryvnia = "UAH"
    Public Const Uruguayan_peso_uruguayo = "UYU"
    Public Const US_dollar = "USD"
    Public Const Uzbekistani_sum = "UZS"
    Public Const Vanuatu_vatu = "VUV"
    Public Const Venezuelan_bolivar_fuerte = "VEF"
    Public Const Vietnamese_dong = "VND"
    Public Const Yemeni_rial = "YER"
    Public Const Zambian_kwacha = "ZMK"
    Public Const Zimbabwe_dollar = "ZWL"

End Class

#End Region

#Region "Currency Ranking"

Public Class cRank
    Private d As Dictionary(Of String, Integer) = Nothing
    Public Sub New()
        d = New Dictionary(Of String, Integer)
        With d
            .Add("EUR", 1)
            .Add("GBP", 2)
            .Add("AUD", 4)
            .Add("NZD", 5)
            .Add("BWP", 6)
            .Add("USD", 7)
            .Add("CAD", 8)
            .Add("CHF", 10)
            .Add("SGD", 11)
            .Add("DKK", 12)
            .Add("ZAR", 15)
            .Add("HKD", 16)
            .Add("NOK", 17)
            .Add("TRY", 18)
            .Add("PLN", 19)
            .Add("AED", 21)
            .Add("SEK", 23)
            .Add("BBD", 26)
            .Add("BHD", 27)
            .Add("CYP", 28)
            .Add("CZK", 29)
            .Add("ILS", 30)
            .Add("INR", 31)
            .Add("ISK", 32)
            .Add("JMD", 33)
            .Add("JOD", 34)
            .Add("KES", 35)
            .Add("KWD", 36)
            .Add("MAD", 38)
            .Add("MTL", 39)
            .Add("MUR", 40)
            .Add("MXN", 41)
            .Add("OMR", 42)
            .Add("QAR", 44)
            .Add("SAR", 45)
            .Add("THB", 47)
            .Add("TND", 48)
            .Add("TZS", 49)
        End With
    End Sub
    Public ReadOnly Property CurrencyRank(ByVal CurrStr As String) As Integer
        Get
            If d.ContainsKey(CurrStr) Then
                Return d(CurrStr)
            Else
                Return -1
            End If
        End Get
    End Property
End Class

#End Region

#Region "World First Types"

#Region "Enumerative types"

Public Enum w1st_enum_Side
    B
    S
End Enum

Public Enum w1st_enum_Reason
    Emigration = 1
    Overseas_Mortgage_Payments = 3
    Sending_Money_Home = 4
    Transfer_to_Own_Account = 5
    Other = 6
    Paying_Overseas_Suppliers = 7
    Repatriating_Overseas_Earnings = 8
    Investing_Abroad = 9
    Holiday_Home_Second_Home_Purchase = 16
    Investment_Property_Purchase = 17
    Overseas_Purchase = 18
    Property_Sale = 19
    Returning_From_Abroad = 20
End Enum

Public Enum w1st_enum_PaymentType
    Std
    StdCov
    BACS
    CHAPS
    SEPA
End Enum

#End Region

#Region "Request types"

Public Class w1st_REQUEST_Beneficiary

#Region "initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New( _
        ByVal accholder As String, _
        ByVal accholderadd1 As String, _
        ByVal curr As String, _
        ByVal bankname As String, _
        ByVal bankcountry As String, _
        Optional ByVal bankcode As String = Nothing, _
        Optional ByVal accno As String = Nothing, _
        Optional ByVal iban As String = Nothing, _
        Optional ByVal bic As String = Nothing, _
        Optional ByVal accholderadd2 As String = Nothing, _
        Optional ByVal accholderadd3 As String = Nothing, _
        Optional ByVal bankadd1 As String = Nothing, _
        Optional ByVal bankadd2 As String = Nothing, _
        Optional ByVal bankadd3 As String = Nothing _
    )

        With Me
            .accholder = accholder
            .accholderadd1 = accholderadd1
            .accholderadd2 = accholderadd2
            .accholderadd3 = accholderadd3
            .curr = curr
            .bankname = bankname
            .bankcode = bankcode
            .accno = accno
            .bankadd1 = bankadd1
            .bankadd2 = bankadd2
            .bankadd3 = bankadd3
            .bankcountry = bankcountry
            .iban = iban
            .bic = bic
        End With
    End Sub
#End Region

#Region "Properties"
    Private _accholder As String = Nothing
    Public Property accholder() As String
        Get
            Return _accholder
        End Get
        Set(ByVal value As String)
            _accholder = value
        End Set
    End Property
    Private _accholderadd1 As String = Nothing
    Public Property accholderadd1() As String
        Get
            Return _accholderadd1
        End Get
        Set(ByVal value As String)
            _accholderadd1 = value
        End Set
    End Property
    Private _accholderadd2 As String = Nothing
    Public Property accholderadd2() As String
        Get
            Return _accholderadd2
        End Get
        Set(ByVal value As String)
            _accholderadd2 = value
        End Set
    End Property
    Private _accholderadd3 As String = Nothing
    Public Property accholderadd3() As String
        Get
            Return _accholderadd3
        End Get
        Set(ByVal value As String)
            _accholderadd3 = value
        End Set
    End Property
    Private _curr As String = Nothing
    Public Property curr() As String
        Get
            Return _curr
        End Get
        Set(ByVal value As String)
            _curr = value
        End Set
    End Property
    Private _bankname As String = Nothing
    Public Property bankname() As String
        Get
            Return _bankname
        End Get
        Set(ByVal value As String)
            _bankname = value
        End Set
    End Property
    Private _bankcode As String = Nothing
    Public Property bankcode() As String
        Get
            Return _bankcode
        End Get
        Set(ByVal value As String)
            _bankcode = value
        End Set
    End Property
    Private _accno As String = Nothing
    Public Property accno() As String
        Get
            Return _accno
        End Get
        Set(ByVal value As String)
            _accno = value
        End Set
    End Property
    Private _bankadd1 As String = Nothing
    Public Property bankadd1() As String
        Get
            Return _bankadd1
        End Get
        Set(ByVal value As String)
            _bankadd1 = value
        End Set
    End Property
    Private _bankadd2 As String = Nothing
    Public Property bankadd2() As String
        Get
            Return _bankadd2
        End Get
        Set(ByVal value As String)
            _bankadd2 = value
        End Set
    End Property
    Private _bankadd3 As String = Nothing
    Public Property bankadd3() As String
        Get
            Return _bankadd3
        End Get
        Set(ByVal value As String)
            _bankadd3 = value
        End Set
    End Property
    Private _bankcountry As String = Nothing
    Public Property bankcountry() As String
        Get
            Return _bankcountry
        End Get
        Set(ByVal value As String)
            _bankcountry = value
        End Set
    End Property
    Private _iban As String = Nothing
    Public Property iban() As String
        Get
            Return _iban
        End Get
        Set(ByVal value As String)
            _iban = value
        End Set
    End Property
    Private _bic As String = Nothing
    Public Property bic() As String
        Get
            Return _bic
        End Get
        Set(ByVal value As String)
            _bic = value
        End Set
    End Property
#End Region

End Class

Public Class w1st_REQUEST_GetQuote

#Region "Initialisation"
    Public Sub New()
        MyBase.new()
    End Sub
    Public Sub New(ByVal SettlementDate As Date, _
        ByVal BuyCurr As String, _
        ByVal SellCurr As String, _
        ByVal Side As w1st_enum_Side, _
        ByVal Amount As Double)
        With Me
            .SettlementDate = SettlementDate
            .BuyCurr = BuyCurr
            .SellCurr = SellCurr
            .Side = Side
            .Amount = Amount
        End With
    End Sub
#End Region

#Region "Properies"
    Private _settlementdate As Date = Now
    Public Property SettlementDate() As Date
        Get
            Return _settlementdate
        End Get
        Set(ByVal value As Date)
            _settlementdate = value
        End Set
    End Property
    Private _buycurr As String = Nothing
    Public Property BuyCurr() As String
        Get
            Return _buycurr
        End Get
        Set(ByVal value As String)
            _buycurr = value
        End Set
    End Property
    Private _sellcurr As String = Nothing
    Public Property SellCurr() As String
        Get
            Return _sellcurr
        End Get
        Set(ByVal value As String)
            _sellcurr = value
        End Set
    End Property
    Private _side As w1st_enum_Side = Nothing
    Public Property Side() As w1st_enum_Side
        Get
            Return _side
        End Get
        Set(ByVal value As w1st_enum_Side)
            _side = value
        End Set
    End Property
    Private _amount As Double = Nothing
    Public Property Amount() As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            _amount = value
        End Set
    End Property
#End Region

End Class

Public Class w1st_REQUEST_Payment

#Region "Initialisation"
    Sub New()
        MyBase.new()
    End Sub
    Sub New(ByVal Beneficiary As w1st_REQUEST_Beneficiary, _
        ByVal paymentid As String, _
        ByVal amount As Double, _
        ByVal paymentdate As Date, _
        ByVal reason As w1st_enum_Reason, _
        Optional ByVal tradeID As String = Nothing, _
        Optional ByVal sellcurrency As String = Nothing, _
        Optional ByVal reason_if_other As String = Nothing, _
        Optional ByVal notes1 As String = Nothing, _
        Optional ByVal notes2 As String = Nothing, _
        Optional ByVal notes3 As String = Nothing)

        With Me
            .PaymentID = paymentid
            .TradeID = tradeID
            .sellcurrency = sellcurrency
            .amount = amount
            .paymentdate = paymentdate
            .notes1 = notes1
            .notes2 = notes2
            .notes3 = notes3
            .reason = reason
            .reason_if_other = reason_if_other
            .beneficiary = Beneficiary
        End With
    End Sub
#End Region

#Region "Properties"
    Private _tradeid As String = Nothing
    Public Property TradeID() As String
        Get
            Return _tradeid
        End Get
        Set(ByVal value As String)
            _tradeid = value
        End Set
    End Property
    Private _paymentid As String = Nothing
    Public Property PaymentID() As String
        Get
            Return _paymentid
        End Get
        Set(ByVal value As String)
            _paymentid = value
        End Set
    End Property
    Private _sellcurrency As String = Nothing
    Public Property sellcurrency() As String
        Get
            Return _sellcurrency
        End Get
        Set(ByVal value As String)
            _sellcurrency = value
        End Set
    End Property
    Private _amount As Double = Nothing
    Public Property amount() As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            _amount = value
        End Set
    End Property
    Private _paymentdate As Date = Nothing
    Public Property paymentdate() As Date
        Get
            Return _paymentdate
        End Get
        Set(ByVal value As Date)
            _paymentdate = value
        End Set
    End Property
    Private _notes1 As String = Nothing
    Public Property notes1() As String
        Get
            Return _notes1
        End Get
        Set(ByVal value As String)
            _notes1 = value
        End Set
    End Property
    Private _notes2 As String = Nothing
    Public Property notes2() As String
        Get
            Return _notes2
        End Get
        Set(ByVal value As String)
            _notes2 = value
        End Set
    End Property
    Private _notes3 As String = Nothing
    Public Property notes3() As String
        Get
            Return _notes3
        End Get
        Set(ByVal value As String)
            _notes3 = value
        End Set
    End Property
    Private _reason As w1st_enum_Reason = Nothing
    Public Property reason() As w1st_enum_Reason
        Get
            Return _reason
        End Get
        Set(ByVal value As w1st_enum_Reason)
            _reason = value
        End Set
    End Property
    Private _reason_if_other As String = Nothing
    Public Property reason_if_other() As String
        Get
            Return _reason_if_other
        End Get
        Set(ByVal value As String)
            _reason_if_other = value
        End Set
    End Property
    Private _beneficiary As New w1st_REQUEST_Beneficiary
    Public Property beneficiary() As w1st_REQUEST_Beneficiary
        Get
            Return _beneficiary
        End Get
        Set(ByVal value As w1st_REQUEST_Beneficiary)
            _beneficiary = value
        End Set
    End Property
#End Region

End Class

Public Class w1st_REQUEST_PastPayment

#Region "Initialisation"
    Sub New()
        MyBase.new()
    End Sub
    Sub New(ByVal PaymentID As String)

        With Me
            .Past_Payment = PaymentID
        End With
    End Sub
#End Region

#Region "Properties"
    Private _Past_Payment As String = Nothing
    Public Property Past_Payment() As String
        Get
            Return _Past_Payment
        End Get
        Set(ByVal value As String)
            _Past_Payment = value
        End Set
    End Property
#End Region

End Class

Public Class w1st_REQUEST

#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal Atomic As Boolean, ByVal Testing As Boolean, _
        Optional ByVal PastPaymentid As String = Nothing)
        With Me
            .Atomic = Atomic
            .Testing = Testing
            'If Not IsNothing(PastPaymentid) Then .PastPaymentid = PastPaymentid
        End With
    End Sub
#End Region

#Region "Properties"
    Private _atomic As Boolean = False
    Public Property Atomic() As Boolean
        Get
            Return _atomic
        End Get
        Set(ByVal value As Boolean)
            _atomic = value
        End Set
    End Property
    Private _testing As Boolean = False
    Public Property Testing() As Boolean
        Get
            Return _testing
        End Get
        Set(ByVal value As Boolean)
            _testing = value
        End Set
    End Property
#End Region

#Region "Object Dictionaries"
    Private _getquote As New Dictionary(Of Integer, w1st_REQUEST_GetQuote)
    Public Property GetQuote() As Dictionary(Of Integer, w1st_REQUEST_GetQuote)
        Get
            Return _getquote
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_REQUEST_GetQuote))
            _getquote = value
        End Set
    End Property
    Private _payment As New Dictionary(Of Integer, w1st_REQUEST_Payment)
    Public Property Payment() As Dictionary(Of Integer, w1st_REQUEST_Payment)
        Get
            Return _payment
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_REQUEST_Payment))
            _payment = value
        End Set
    End Property
    Private _pastpayment As New Dictionary(Of Integer, w1st_REQUEST_PastPayment)
    Public Property PastPayment() As Dictionary(Of Integer, w1st_REQUEST_PastPayment)
        Get
            Return _pastpayment
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_REQUEST_PastPayment))
            _pastpayment = value
        End Set
    End Property
#End Region

End Class

#End Region

#Region "Response Types"

Public Class w1st_RESPONSE_PastPayment

#Region "Properties"
    Private _requested_paymentid As String
    Public Property requested_paymentid() As String
        Get
            Return _requested_paymentid
        End Get
        Set(ByVal value As String)
            _requested_paymentid = value
        End Set
    End Property
    Private _success As Boolean
    Public Property success() As Boolean
        Get
            Return _success
        End Get
        Set(ByVal value As Boolean)
            _success = value
        End Set
    End Property
    Private _created As Date
    Public Property created() As Date
        Get
            Return _created
        End Get
        Set(ByVal value As Date)
            _created = value
        End Set
    End Property
    Private _tradeid As String
    Public Property tradeid() As String
        Get
            Return _tradeid
        End Get
        Set(ByVal value As String)
            _tradeid = value
        End Set
    End Property
    Private _buycurr As String
    Public Property buycurr() As String
        Get
            Return _buycurr
        End Get
        Set(ByVal value As String)
            _buycurr = value
        End Set
    End Property
    Private _sellcurr As String
    Public Property sellcurr() As String
        Get
            Return _sellcurr
        End Get
        Set(ByVal value As String)
            _sellcurr = value
        End Set
    End Property
    Private _amount As String
    Public Property amount() As String
        Get
            Return _amount
        End Get
        Set(ByVal value As String)
            _amount = value
        End Set
    End Property
    Private _rate As String
    Public Property rate() As String
        Get
            Return _rate
        End Get
        Set(ByVal value As String)
            _rate = value
        End Set
    End Property
    Private _paymentdate As Date
    Public Property paymentdate() As Date
        Get
            Return _paymentdate
        End Get
        Set(ByVal value As Date)
            _paymentdate = value
        End Set
    End Property
#End Region

#Region "Dictionaries"
    Private _errors As New Dictionary(Of Integer, w1st_RESPONSE_error)
    Public Property Errors() As Dictionary(Of Integer, w1st_RESPONSE_error)
        Get
            Return _errors
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_RESPONSE_error))
            _errors = value
        End Set
    End Property
#End Region

End Class

Public Class w1st_RESPONSE_payment

#Region "Properties"
    Private rank As New cRank()
    Private _success As Boolean
    Public Property success() As Boolean
        Get
            Return _success
        End Get
        Set(ByVal value As Boolean)
            _success = value
        End Set
    End Property
    Private _tradeid As String
    Public Property tradeid() As String
        Get
            Return _tradeid
        End Get
        Set(ByVal value As String)
            _tradeid = value
        End Set
    End Property
    Private _paymentid As String
    Public Property paymentid() As String
        Get
            Return _paymentid
        End Get
        Set(ByVal value As String)
            _paymentid = value
        End Set
    End Property
    Private _buycurr As String
    Public Property buycurr() As String
        Get
            Return _buycurr
        End Get
        Set(ByVal value As String)
            _buycurr = value
        End Set
    End Property
    Private _sellcurr As String
    Public Property sellcurr() As String
        Get
            Return _sellcurr
        End Get
        Set(ByVal value As String)
            _sellcurr = value
        End Set
    End Property
    Private _amount As Double
    Public Property amount() As Double
        Get
            Return _amount
        End Get
        Set(ByVal value As Double)
            _amount = value
        End Set
    End Property
    Private _rate As Double
    Public Property rate() As String
        Get
            If rank.CurrencyRank(buycurr) > rank.CurrencyRank(sellcurr) Then
                Return _rate.ToString
            Else
                Return Format(1 / CDbl(_rate), "0.00000").ToString
            End If
        End Get
        Set(ByVal value As String)
            _rate = value
        End Set
    End Property
    Private _paymentdate As Date
    Public Property paymentdate() As Date
        Get
            Return _paymentdate
        End Get
        Set(ByVal value As Date)
            _paymentdate = value
        End Set
    End Property
#End Region

#Region "Dictionaries"
    Private _errors As New Dictionary(Of Integer, w1st_RESPONSE_error)
    Public Property Errors() As Dictionary(Of Integer, w1st_RESPONSE_error)
        Get
            Return _errors
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_RESPONSE_error))
            _errors = value
        End Set
    End Property
#End Region

End Class

Public Class w1st_RESPONSE_quote

#Region "Properties"

    Private rank As New cRank()
    Private _success As Boolean
    Public Property success() As Boolean
        Get
            Return _success
        End Get
        Set(ByVal value As Boolean)
            _success = value
        End Set
    End Property
    Private _tradeid As String
    Public Property tradeid() As String
        Get
            Return _tradeid
        End Get
        Set(ByVal value As String)
            _tradeid = value
        End Set
    End Property
    Private _buycurr As String
    Public Property buycurr() As String
        Get
            Return _buycurr
        End Get
        Set(ByVal value As String)
            _buycurr = value
        End Set
    End Property
    Private _sellcurr As String
    Public Property sellcurr() As String
        Get
            Return _sellcurr
        End Get
        Set(ByVal value As String)
            _sellcurr = value
        End Set
    End Property
    Private _buyamt As Double
    Public Property buyamt() As Double
        Get
            Return _buyamt
        End Get
        Set(ByVal value As Double)
            _buyamt = value
        End Set
    End Property
    Private _sellamt As Double
    Public Property sellamt() As Double
        Get
            Return _sellamt
        End Get
        Set(ByVal value As Double)
            _sellamt = value
        End Set
    End Property
    Private _rate As String
    Public Property rate() As String
        Get
            If rank.CurrencyRank(buycurr) > rank.CurrencyRank(sellcurr) Then
                Return _rate.ToString
            Else
                Return Format(1 / CDbl(_rate), "0.00000").ToString
            End If
        End Get
        Set(ByVal value As String)
            _rate = value
        End Set
    End Property
    Private _settlementdate As Date
    Public Property settlementdate() As Date
        Get
            Return _settlementdate
        End Get
        Set(ByVal value As Date)
            _settlementdate = value
        End Set
    End Property
    Private _expiry As String
    Public Property expiry() As String
        Get
            Return _expiry
        End Get
        Set(ByVal value As String)
            _expiry = value
        End Set
    End Property
    Private _side As w1st_enum_Side
    Public Property side() As w1st_enum_Side
        Get
            Return _side
        End Get
        Set(ByVal value As w1st_enum_Side)
            _side = value
        End Set
    End Property
    Private _amount As String
    Public Property amount() As String
        Get
            Return _amount
        End Get
        Set(ByVal value As String)
            _amount = value
        End Set
    End Property
#End Region

#Region "Dictionaries"
    Private _errors As New Dictionary(Of Integer, w1st_RESPONSE_error)
    Public Property Errors() As Dictionary(Of Integer, w1st_RESPONSE_error)
        Get
            Return _errors
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_RESPONSE_error))
            _errors = value
        End Set
    End Property
#End Region

End Class

Public Class w1st_RESPONSE_error

#Region "Initialisation"
    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal Code As String, ByVal Message As String)
        With Me
            .code = Code
            .message = Message
        End With
    End Sub
#End Region

#Region "Properties"

    Private _code As String = Nothing
    Public Property code() As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            _code = value
        End Set
    End Property
    Private _message As String = Nothing
    Public Property message() As String
        Get
            Return _message
        End Get
        Set(ByVal value As String)
            _message = value
        End Set
    End Property
#End Region

End Class

Public Class w1st_RESPONSE

#Region "Initialisation"

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal Atomic As Boolean, ByVal Testing As Boolean)
        With Me
            .Atomic = Atomic
            .Testing = Testing
        End With
    End Sub

#End Region

#Region "Properties"
    Private _atomic As Boolean = False
    Public Property Atomic() As Boolean
        Get
            Return _atomic
        End Get
        Set(ByVal value As Boolean)
            _atomic = value
        End Set
    End Property
    Private _testing As Boolean = False
    Public Property Testing() As Boolean
        Get
            Return _testing
        End Get
        Set(ByVal value As Boolean)
            _testing = value
        End Set
    End Property
#End Region

#Region "Object Dictionaries"
    Private _errors As New Dictionary(Of Integer, w1st_RESPONSE_error)
    Public Property Errors() As Dictionary(Of Integer, w1st_RESPONSE_error)
        Get
            Return _errors
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_RESPONSE_error))
            _errors = value
        End Set
    End Property
    Private _payment As New Dictionary(Of Integer, w1st_RESPONSE_payment)
    Public Property Payment() As Dictionary(Of Integer, w1st_RESPONSE_payment)
        Get
            Return _payment
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_RESPONSE_payment))
            _payment = value
        End Set
    End Property
    Private _quote As New Dictionary(Of Integer, w1st_RESPONSE_quote)
    Public Property Quote() As Dictionary(Of Integer, w1st_RESPONSE_quote)
        Get
            Return _quote
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_RESPONSE_quote))
            _quote = value
        End Set
    End Property
    Private _pastpayment As New Dictionary(Of Integer, w1st_RESPONSE_PastPayment)
    Public Property PastPayment() As Dictionary(Of Integer, w1st_RESPONSE_PastPayment)
        Get
            Return _pastpayment
        End Get
        Set(ByVal value As Dictionary(Of Integer, w1st_RESPONSE_PastPayment))
            _pastpayment = value
        End Set
    End Property
#End Region

End Class

#End Region

#End Region

#Region "World First API"

Public Class w1st_Service

#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal url As String, _
        ByVal user As String, _
        ByVal pass As String, _
        ByVal key As String, _
        Optional ByVal enc As Encoding = Nothing _
    )
        With Me
            .user = user
            .pass = pass
            .key = key
            If Not IsNothing(enc) Then .enc = enc
        End With
    End Sub
#End Region

#Region "Properties"
    Private _url As String = "https://trading.worldfirst.com/api/demo/"
    Public Property url() As String
        Get
            Return _url & "?hash=" & Hash
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property
    Private _user As String = "demo"
    Public Property user() As String
        Get
            Return _user
        End Get
        Set(ByVal value As String)
            _user = value
        End Set
    End Property
    Private _pass As String = "d3m0u5r"
    Public Property pass() As String
        Get
            Return _pass
        End Get
        Set(ByVal value As String)
            _pass = value
        End Set
    End Property
    Private _key As String = "abcdefghihjklmnopqrstuvwxyz0123456789"
    Public Property key() As String
        Get
            Return _key
        End Get
        Set(ByVal value As String)
            _key = value
        End Set
    End Property
    Private _enc As Encoding = Encoding.UTF8
    Public Property enc() As Encoding
        Get
            Return _enc
        End Get
        Set(ByVal value As Encoding)
            _enc = value
        End Set
    End Property
    Private _request As w1st_REQUEST
    Public Property request() As w1st_REQUEST
        Get
            Return _request
        End Get
        Set(ByVal value As w1st_REQUEST)
            _request = value
        End Set
    End Property
    Private _repsonse As w1st_RESPONSE
    Public Property response() As w1st_RESPONSE
        Get
            Return _repsonse
        End Get
        Set(ByVal value As w1st_RESPONSE)
            _repsonse = value
        End Set
    End Property
    Public ReadOnly Property RequestXML() As String
        Get
            Dim xml As String = ""
            With Me.request

                xml += "<request>"
                xml += String.Format("<atomic>{0}</atomic>", LCase(CStr(.Atomic)))
                xml += String.Format("<testing>{0}</testing>", LCase(CStr(.Testing)))

                If .PastPayment.Count > 0 Then
                    For Each pp As w1st_REQUEST_PastPayment In .PastPayment.Values
                        xml += String.Format("<past-paymentid>{0}</past-paymentid>", _
                            pp.Past_Payment)
                    Next
                End If

                If .GetQuote.Count > 0 Then
                    For Each q As w1st_REQUEST_GetQuote In .GetQuote.Values
                        CleanObject(q)
                        xml += "<getquote>"
                        With q
                            xml += String.Format("<settlementdate>{0}-{1}-{2}</settlementdate>", _
                                Year(.SettlementDate), _
                                Right("00" & CStr(Month(.SettlementDate)), 2), _
                                Right("00" & CStr(Day(.SettlementDate)), 2) _
                            )
                            xml += String.Format("<buycurr>{0}</buycurr>", .BuyCurr)
                            xml += String.Format("<sellcurr>{0}</sellcurr>", .SellCurr)
                            Select Case .Side
                                Case w1st_enum_Side.B
                                    xml += String.Format("<side>{0}</side>", "B")
                                Case w1st_enum_Side.S
                                    xml += String.Format("<side>{0}</side>", "S")
                            End Select
                            xml += String.Format("<amount>{0}</amount>", String.Format("{0:f2}", .Amount))
                        End With
                        xml += "</getquote>"
                    Next
                End If

                If .Payment.Count > 0 Then
                    For Each p As w1st_REQUEST_Payment In .Payment.Values
                        CleanObject(p)
                        xml += "<payment>"
                        With p
                            If Not IsNothing(.TradeID) Then _
                                xml += String.Format("<tradeid>{0}</tradeid>", _
                                .TradeID)
                            xml += String.Format("<paymentid>{0}</paymentid>", .PaymentID)
                            If Not IsNothing(.sellcurrency) Then _
                                xml += String.Format("<sellcurrency>{0}</sellcurrency>", _
                                .sellcurrency)
                            xml += String.Format("<amount>{0}</amount>", String.Format("{0:f2}", .amount))
                            xml += String.Format("<paymentdate>{0}-{1}-{2}</paymentdate>", _
                                Year(.paymentdate), _
                                Right("00" & CStr(Month(.paymentdate)), 2), _
                                Right("00" & CStr(Day(.paymentdate)), 2) _
                            )
                            Dim pt As String = ""
                            If Not IsNothing(.notes1) Then _
                                xml += String.Format("<notes1>{0}</notes1>", _
                                .notes1)
                            If Not IsNothing(.notes2) Then _
                                xml += String.Format("<notes2>{0}</notes2>", _
                                .notes2)
                            If Not IsNothing(.notes3) Then _
                                xml += String.Format("<notes3>{0}</notes3>", _
                                .notes3)
                            xml += String.Format("<reason>{0}</reason>", CStr(CInt(.reason)))
                            If Not IsNothing(.reason_if_other) Then _
                                xml += String.Format("<reason_if_other>{0}</reason_if_other>", _
                                .reason_if_other)

                            xml += "<beneficiary>"
                            CleanObject(.beneficiary)
                            With .beneficiary
                                If Not IsNothing(.accholderadd1) Then _
                                xml += String.Format("<accholder>{0}</accholder>", .accholder)
                                If Not IsNothing(.accholderadd2) Then _
                                xml += String.Format("<accholderadd1>{0}</accholderadd1>", .accholderadd1)
                                If Not IsNothing(.accholderadd2) Then _
                                xml += String.Format("<accholderadd2>{0}</accholderadd2>", _
                                .accholderadd2)
                                If Not IsNothing(.accholderadd3) Then _
                                xml += String.Format("<accholderadd3>{0}</accholderadd3>", _
                                .accholderadd3)
                                xml += String.Format("<curr>{0}</curr>", .curr)
                                xml += String.Format("<bankname>{0}</bankname>", .bankname)
                                If Not IsNothing(.bankcode) Then _
                                    xml += String.Format("<bankcode>{0}</bankcode>", _
                                    .bankcode)
                                If Not IsNothing(.accno) Then _
                                    xml += String.Format("<accno>{0}</accno>", _
                                    .accno)
                                If Not IsNothing(.bankadd1) Then _
                                    xml += String.Format("<bankadd1>{0}</bankadd1>", _
                                    .bankadd1)
                                If Not IsNothing(.bankadd2) Then _
                                    xml += String.Format("<bankadd2>{0}</bankadd2>", _
                                    .bankadd2)
                                If Not IsNothing(.bankadd3) Then _
                                    xml += String.Format("<bankadd3>{0}</bankadd3>", _
                                    .bankadd3)
                                xml += String.Format("<bankcountry>{0}</bankcountry>", _
                                .bankcountry)
                                If Not IsNothing(.iban) Then _
                                    xml += String.Format("<iban>{0}</iban>", _
                                    .iban)
                                If Not IsNothing(.bic) Then _
                                    xml += String.Format("<bic>{0}</bic>", _
                                    .bic)
                            End With
                            xml += "</beneficiary>"
                        End With
                        xml += "</payment>"
                    Next
                End If

                xml += "</request>"
            End With
            Return xml
        End Get
    End Property
    Private Function CleanObject(ByRef o As Object)
        Dim pi As System.Reflection.PropertyInfo
        Dim objectProps As PropertyDescriptorCollection = TypeDescriptor.GetProperties(o.GetType)
        For Each prop As PropertyDescriptor In objectProps
            pi = o.GetType.GetProperty(prop.Name)
            If Not IsNothing(pi.GetValue(o, Nothing)) Then
                Select Case pi.GetValue(o, Nothing).GetType.ToString
                    Case "System.double", "System.Int32"
                        If pi.GetValue(o, Nothing) = 0 Then
                            pi.SetValue(o, Nothing, Nothing)
                        End If
                    Case "System.String"
                        If pi.GetValue(o, Nothing) = "" Then
                            pi.SetValue(o, Nothing, Nothing)
                        End If
                    Case Else

                End Select
            End If

        Next
        Return 0
    End Function
    Public ReadOnly Property Hash() As String
        Get
            Dim bKey() As Byte = _enc.GetBytes(key)
            Dim bMessage() As Byte = _enc.GetBytes(RequestXML)
            Dim hmacmd5 As HMACMD5 = New HMACMD5(bKey)
            Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
            Dim md5Hash() As Byte = parseHash(md5.ComputeHash(bMessage))
            Dim smd5Hash As String = _enc.GetString(md5Hash)
            Dim bMsgHash() As Byte = parseHash(hmacmd5.ComputeHash(md5Hash))
            Return _enc.GetString(bMsgHash)
        End Get
    End Property
#End Region

#Region "public functions"

    Public Sub Send()

        Dim webRequest As WebRequest = Net.WebRequest.Create(url)

        With webRequest
            .Credentials = New NetworkCredential(user, pass)
            .ContentType = "application/x-www-form-urlencoded"
            .Method = "POST"
        End With

        Dim bytes() As Byte = _enc.GetBytes(RequestXML)
        Dim os As Stream = Nothing
        Try
            webRequest.ContentLength = bytes.Length
            os = webRequest.GetRequestStream()
            os.Write(bytes, 0, bytes.Length)
        Catch ex As WebException
            Throw New Exception("HttpPost: Request error - " + ex.Message)
        Finally
            If Not os Is Nothing Then
                os.Close()
            End If
        End Try

        Try
            Dim xd As New XmlDocument
            Dim webResponse As WebResponse = webRequest.GetResponse()
            If Not IsNothing(webResponse) Then
                Using sr As New StreamReader(webResponse.GetResponseStream())
                    xd.LoadXml(sr.ReadToEnd().Trim())
                    parseResponse(xd)
                End Using
            Else
                Throw New Exception("HttpPost: Response error - returned null.")
            End If
        Catch ex As WebException
            Throw New Exception("HttpPost: Response error - " + ex.Message)
        End Try

    End Sub

#End Region

#Region "Private Functions"

    Private Function parseHash(ByVal hash As Byte()) As Byte()

        Dim ret As String = ""
        For Each a As Byte In hash
            ret += a.ToString("x2")
        Next
        Return _enc.GetBytes(ret)

    End Function

    Private Sub parseResponse(ByVal xd As XmlDocument)
        response = Nothing
        response = New w1st_RESPONSE
        With response
            For Each n As XmlNode In xd.ChildNodes
                Select Case LCase(n.Name)
                    Case "response"
                        For Each no As XmlNode In n.ChildNodes
                            Select Case LCase(no.Name)
                                Case "atomic"
                                    .Atomic = CBool(no.InnerText)
                                Case "testing"
                                    .Testing = CBool(no.InnerText)
                                Case "error"
                                    With .Errors
                                        .Add(.Count + 1, parseError(no))
                                    End With
                                Case "quote"
                                    With .Quote
                                        .Add(.Count + 1, parseQuote(no))
                                    End With
                                Case "payment"
                                    With .Payment
                                        .Add(.Count + 1, parsePayment(no))
                                    End With
                                Case "past-payment"
                                    With .PastPayment
                                        .Add(.Count + 1, parsePastPayment(no))
                                    End With
                            End Select
                        Next
                End Select
            Next
        End With
    End Sub

    Private Function parseError(ByVal node As XmlNode) As w1st_RESPONSE_error
        Dim ret As New w1st_RESPONSE_error
        With ret
            For Each no As XmlNode In node.ChildNodes
                Select Case LCase(no.Name)
                    Case "code"
                        .code = no.InnerText
                    Case "message"
                        .message = no.InnerText
                End Select
            Next
        End With
        Return ret
    End Function

    Private Function parseQuote(ByVal node As XmlNode) As w1st_RESPONSE_quote
        Dim ret As New w1st_RESPONSE_quote
        With ret
            For Each no As XmlNode In node.ChildNodes
                Select Case LCase(no.Name)
                    Case "success"
                        .success = CBool(no.InnerText)
                    Case "tradeid"
                        .tradeid = no.InnerText
                    Case "buycurr"
                        .buycurr = no.InnerText
                    Case "sellcurr"
                        .sellcurr = no.InnerText
                    Case "buyamt"
                        .buyamt = CDbl(no.InnerText)
                    Case "sellamt"
                        .sellamt = CDbl(no.InnerText)
                    Case "rate"
                        .rate = no.InnerText
                    Case "settlementdate"
                        .settlementdate = DateTime.ParseExact(no.InnerText, _
                            "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat)
                    Case "expiry"
                        .expiry = no.InnerText
                    Case "side"
                        Select Case UCase(no.InnerText)
                            Case "B"
                                .side = w1st_enum_Side.B
                            Case "S"
                                .side = w1st_enum_Side.S
                        End Select
                    Case "amount"
                        .amount = no.InnerText
                    Case "error"
                        With .Errors
                            .Add(.Count + 1, parseError(no))
                        End With
                End Select
            Next
        End With
        Return ret
    End Function

    Private Function parsePayment(ByVal node As XmlNode) As w1st_RESPONSE_payment
        Dim ret As New w1st_RESPONSE_payment
        With ret
            For Each no As XmlNode In node.ChildNodes
                Select Case LCase(no.Name)
                    Case "success"
                        .success = CBool(no.InnerText)
                    Case "tradeid"
                        .tradeid = no.InnerText
                    Case "paymentid"
                        .paymentid = no.InnerText
                    Case "buycurr"
                        .buycurr = no.InnerText
                    Case "sellcurr"
                        .sellcurr = no.InnerText
                    Case "amount"
                        .amount = CDbl(no.InnerText)
                    Case "rate"
                        .rate = no.InnerText
                    Case "paymentdate"
                        .paymentdate = DateTime.ParseExact(no.InnerText, _
                            "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat)
                    Case "error"
                        With .Errors
                            .Add(.Count + 1, parseError(no))
                        End With
                End Select
            Next
        End With
        Return ret
    End Function

    Private Function parsePastPayment(ByVal node As XmlNode) As w1st_RESPONSE_PastPayment
        Dim ret As New w1st_RESPONSE_PastPayment
        With ret
            For Each no As XmlNode In node.ChildNodes
                Select Case LCase(no.Name)
                    Case "requested-paymentid"
                        .requested_paymentid = no.InnerText
                    Case "success"
                        .success = CBool(no.InnerText)
                    Case "created"
                        .created = DateTime.ParseExact(no.InnerText, _
                            "yyyy-MM-ddThh:mm:ssZ", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat)
                    Case "tradeid"
                        .tradeid = no.InnerText
                    Case "buycurr"
                        .buycurr = no.InnerText
                    Case "sellcurr"
                        .sellcurr = no.InnerText
                    Case "amount"
                        .amount = CDbl(no.InnerText)
                    Case "rate"
                        .rate = no.InnerText
                    Case "paymentdate"
                        .paymentdate = DateTime.ParseExact(no.InnerText, _
                            "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat)
                    Case "error"
                        With .Errors
                            .Add(.Count + 1, parseError(no))
                        End With
                End Select
            Next
        End With
        Return ret
    End Function

#End Region

End Class

#End Region