using System.Collections.Generic;

namespace EPiServer.Reference.Commerce.Site.Features.AddressBook.Helpers
{
    public static class AddressHelper
    {
        public static Dictionary<string, string> StatesWithCode = new Dictionary<string, string>
        {
            { "Alabama", "AL"},
            {"Alaska", "AK"},
           { "Arizona", "AZ"},
           { "Arkansas", "AR"},
           { "California", "CA"},
           { "Colorado", "CO"},
           { "Connecticut", "CT"},
           { "Delaware", "DE"},
           { "District of Columbia", "DC"},
           { "Florida", "FL"},
           { "Georgia", "GA"},
           { "Hawaii", "HI"},
           { "Idaho", "ID"},
           { "Illinois", "IL"},
           { "Indiana", "IN"},
           { "Iowa", "IA"},
           { "Kansas", "KS"},
           { "Kentucky", "KY"},
           { "Louisiana", "LA"},
           { "Maine", "ME"},
           { "Maryland", "MD"},
           { "Massachusetts", "MA"},
           { "Michigan", "MI"},
           { "Minnesota", "MN"},
           { "Mississippi", "MS"},
           { "Missouri", "MO"},
           { "Montana", "MT"},
           { "Nebraska", "NE"},
           { "Nevada", "NV"},
           { "New Hampshire", "NH"},
           { "New Jersey", "NJ"},
           { "New Brunswick", "NB"},
           { "New Mexico", "NM"},
           { "New York", "NY"},
           { "North Carolina", "NC"},
           {"North Dakota", "ND"},
           { "Ohio", "OH"},
           { "Oklahoma", "OK"},
           { "Oregon", "OR"},
           { "Pennsylvania", "PA"},
           { "Puerto Rico", "PR"},
           { "Rhode Island", "RI"},
           { "South Carolina", "SC"},
           { "South Dakota", "SD"},
           { "Tennessee", "TN"},
           { "Texas", "TX"},
           { "Utah", "UT"},
           { "Vermont", "VT"},
           { "Virgin Islands", "VI"},
           { "Virginia", "VA"},
           { "Washington", "WA"},
           { "West Virginia", "WV"},
           { "Wisconsin", "WI"},
           { "Wyoming", "WY"},
            //CA provinces
           { "Alberta", "AB"},
           { "British Columbia", "BC"},
           { "Manitoba", "MB"},
           { "Newfoundland and Labrador", "NL"},
           { "Northwest Territories", "NT"},
           { "Nova Scotia", "NS"},
           { "Nunavut", "NU"},
           { "Ontario", "ON"},
           { "Prince Edward Island", "PE"},
           { "Quebec", "QC"},
           { "Saskatchewan", "SK"},
           { "Yukon", "YT"}
        };

        public static Dictionary<string, string> CountryCodes = new Dictionary<string, string>
        {
            { "AFG", "AF"}, // Afghanistan
             { "ALA", "AX"}, // Aland Islands
             { "ALB", "AL"}, // Albania
             { "DZA", "DZ"}, // Algeria
             { "ASM", "AS"}, // American Samoa
             { "AND", "AD"}, // Andorra
             { "AGO", "AO"}, // Angola
             { "AIA", "AI"}, // Anguilla
             { "ATA", "AQ"}, // Antarctica
             { "ATG", "AG"}, // Antigua and Barbuda
             { "ARG", "AR"}, // Argentina
             { "ARM", "AM"}, // Armenia
             { "ABW", "AW"}, // Aruba
             { "AUS", "AU"}, // Australia
             { "AUT", "AT"}, // Austria
             { "AZE", "AZ"}, // Azerbaijan
             { "BHS", "BS"}, // Bahamas
             { "BHR", "BH"}, // Bahrain
             { "BGD", "BD"}, // Bangladesh
             { "BRB", "BB"}, // Barbados
             { "BLR", "BY"}, // Belarus
             { "BEL", "BE"}, // Belgium
             { "BLZ", "BZ"}, // Belize
             { "BEN", "BJ"}, // Benin
             { "BMU", "BM"}, // Bermuda
             { "BTN", "BT"}, // Bhutan
             { "BOL", "BO"}, // Bolivia
             { "BIH", "BA"}, // Bosnia and Herzegovina
             { "BWA", "BW"}, // Botswana
             { "BVT", "BV"}, // Bouvet Island
             { "BRA", "BR"}, // Brazil
             { "VGB", "VG"}, // British Virgin Islands
             { "IOT", "IO"}, // British Indian Ocean Territory
             { "BRN", "BN"}, // Brunei Darussalam
             { "BGR", "BG"}, // Bulgaria
             { "BFA", "BF"}, // Burkina Faso
             { "BDI", "BI"}, // Burundi
             { "KHM", "KH"}, // Cambodia
             { "CMR", "CM"}, // Cameroon
             { "CAN", "CA"}, // Canada
             { "CPV", "CV"}, // Cape Verde
             { "CYM", "KY"}, // Cayman Islands 
             { "CAF", "CF"}, // Central African Republic
             { "TCD", "TD"}, // Chad
             { "CHL", "CL"}, // Chile
             { "CHN", "CN"}, // China
             { "HKG", "HK"}, // Hong Kong, SAR China
             { "MAC", "MO"}, // Macao, SAR China
             { "CXR", "CX"}, // Christmas Island
             { "CCK", "CC"}, // Cocos (Keeling) Islands
             { "COL", "CO"}, // Colombia
             { "COM", "KM"}, // Comoros
             { "COG", "CG"}, // Congo (Brazzaville)
             { "COD", "CD"}, // Congo, (Kinshasa)
             { "COK", "CK"}, // Cook Islands 
             { "CRI", "CR"}, // Costa Rica
             { "CIV", "CI"}, // Côte d'Ivoire
             { "HRV", "HR"}, // Croatia
             { "CUB", "CU"}, // Cuba
             { "CYP", "CY"}, // Cyprus
             { "CZE", "CZ"}, // Czech Republic
             { "DNK", "DK"}, // Denmark
             { "DJI", "DJ"}, // Djibouti
             { "DMA", "DM"}, // Dominica
             { "DOM", "DO"}, // Dominican Republic
             { "ECU", "EC"}, // Ecuador
             { "EGY", "EG"}, // Egypt
             { "SLV", "SV"}, // El Salvador
             { "GNQ", "GQ"}, // Equatorial Guinea
             { "ERI", "ER"}, // Eritrea
             { "EST", "EE"}, // Estonia
             { "ETH", "ET"}, // Ethiopia
             { "FLK", "FK"}, // Falkland Islands (Malvinas) 
             { "FRO", "FO"}, // Faroe Islands
             { "FJI", "FJ"}, // Fiji
             { "FIN", "FI"}, // Finland
             { "FRA", "FR"}, // France
             { "GUF", "GF"}, // French Guiana
             { "PYF", "PF"}, // French Polynesia
             { "ATF", "TF"}, // French Southern Territories
             { "GAB", "GA"}, // Gabon
             { "GMB", "GM"}, // Gambia
             { "GEO", "GE"}, // Georgia
             { "DEU", "DE"}, // Germany
             { "GHA", "GH"}, // Ghana
             { "GIB", "GI"}, // Gibraltar 
             { "GRC", "GR"}, // Greece
             { "GRL", "GL"}, // Greenland
             { "GRD", "GD"}, // Grenada
             { "GLP", "GP"}, // Guadeloupe
             { "GUM", "GU"}, // Guam
             { "GTM", "GT"}, // Guatemala
             { "GGY", "GG"}, // Guernsey
             { "GIN", "GN"}, // Guinea
             { "GNB", "GW"}, // Guinea-Bissau
             { "GUY", "GY"}, // Guyana
             { "HTI", "HT"}, // Haiti
             { "HMD", "HM"}, // Heard and Mcdonald Islands
             { "VAT", "VA"}, // Holy See (Vatican City State)
             { "HND", "HN"}, // Honduras
             { "HUN", "HU"}, // Hungary
             { "ISL", "IS"}, // Iceland
             { "IND", "IN"}, // India
             { "IDN", "ID"}, // Indonesia
             { "IRN", "IR"}, // Iran, Islamic Republic of
             { "IRQ", "IQ"}, // Iraq
             { "IRL", "IE"}, // Ireland
             { "IMN", "IM"}, // Isle of Man 
             { "ISR", "IL"}, // Israel
             { "ITA", "IT"}, // Italy
             { "JAM", "JM"}, // Jamaica
             { "JPN", "JP"}, // Japan
             { "JEY", "JE"}, // Jersey
             { "JOR", "JO"}, // Jordan
             { "KAZ", "KZ"}, // Kazakhstan
             { "KEN", "KE"}, // Kenya
             { "KIR", "KI"}, // Kiribati
             { "PRK", "KP"}, // Korea (North)
             { "KOR", "KR"}, // Korea (South)
             { "KWT", "KW"}, // Kuwait
             { "KGZ", "KG"}, // Kyrgyzstan
             { "LAO", "LA"}, // Lao PDR
             { "LVA", "LV"}, // Latvia
             { "LBN", "LB"}, // Lebanon
             { "LSO", "LS"}, // Lesotho
             { "LBR", "LR"}, // Liberia
             { "LBY", "LY"}, // Libya
             { "LIE", "LI"}, // Liechtenstein
             { "LTU", "LT"}, // Lithuania
             { "LUX", "LU"}, // Luxembourg
             { "MKD", "MK"}, // Macedonia, Republic of
             { "MDG", "MG"}, // Madagascar
             { "MWI", "MW"}, // Malawi
             { "MYS", "MY"}, // Malaysia
             { "MDV", "MV"}, // Maldives
             { "MLI", "ML"}, // Mali
             { "MLT", "MT"}, // Malta
             { "MHL", "MH"}, // Marshall Islands
             { "MTQ", "MQ"}, // Martinique
             { "MRT", "MR"}, // Mauritania
             { "MUS", "MU"}, // Mauritius
             { "MYT", "YT"}, // Mayotte
             { "MEX", "MX"}, // Mexico
             { "FSM", "FM"}, // Micronesia, Federated States of
             { "MDA", "MD"}, // Moldova
             { "MCO", "MC"}, // Monaco
             { "MNG", "MN"}, // Mongolia
             { "MNE", "ME"}, // Montenegro
             { "MSR", "MS"}, // Montserrat
             { "MAR", "MA"}, // Morocco
             { "MOZ", "MZ"}, // Mozambique
             { "MMR", "MM"}, // Myanmar
             { "NAM", "NA"}, // Namibia
             { "NRU", "NR"}, // Nauru
             { "NPL", "NP"}, // Nepal
             { "NLD", "NL"}, // Netherlands
             { "ANT", "AN"}, // Netherlands Antilles
             { "NCL", "NC"}, // New Caledonia
             { "NZL", "NZ"}, // New Zealand
             { "NIC", "NI"}, // Nicaragua
             { "NER", "NE"}, // Niger
             { "NGA", "NG"}, // Nigeria
             { "NIU", "NU"}, // Niue 
             { "NFK", "NF"}, // Norfolk Island
             { "MNP", "MP"}, // Northern Mariana Islands
             { "NOR", "NO"}, // Norway
             { "OMN", "OM"}, // Oman
             { "PAK", "PK"}, // Pakistan
             { "PLW", "PW"}, // Palau
             { "PSE", "PS"}, // Palestinian Territory
             { "PAN", "PA"}, // Panama
             { "PNG", "PG"}, // Papua New Guinea
             { "PRY", "PY"}, // Paraguay
             { "PER", "PE"}, // Peru
             { "PHL", "PH"}, // Philippines
             { "PCN", "PN"}, // Pitcairn
             { "POL", "PL"}, // Poland
             { "PRT", "PT"}, // Portugal
             { "PRI", "PR"}, // Puerto Rico
             { "QAT", "QA"}, // Qatar
             { "REU", "RE"}, // Réunion
             { "ROM", "RO"}, // Romania
             { "RUS", "RU"}, // Russian Federation
             { "RWA", "RW"}, // Rwanda
             { "BLM", "BL"}, // Saint-Barthélemy
             { "SHN", "SH"}, // Saint Helena
             { "KNA", "KN"}, // Saint Kitts and Nevis
             { "LCA", "LC"}, // Saint Lucia
             { "MAF", "MF"}, // Saint-Martin (French part)
             { "SPM", "PM"}, // Saint Pierre and Miquelon 
             { "VCT", "VC"}, // Saint Vincent and Grenadines
             { "WSM", "WS"}, // Samoa
             { "SMR", "SM"}, // San Marino
             { "STP", "ST"}, // Sao Tome and Principe
             { "SAU", "SA"}, // Saudi Arabia
             { "SEN", "SN"}, // Senegal
             { "SRB", "RS"}, // Serbia
             { "SYC", "SC"}, // Seychelles
             { "SLE", "SL"}, // Sierra Leone
             { "SGP", "SG"}, // Singapore
             { "SVK", "SK"}, // Slovakia
             { "SVN", "SI"}, // Slovenia
             { "SLB", "SB"}, // Solomon Islands
             { "SOM", "SO"}, // Somalia
             { "ZAF", "ZA"}, // South Africa
             { "SGS", "GS"}, // South Georgia and the South Sandwich Islands
             { "SSD", "SS"}, // South Sudan
             { "ESP", "ES"}, // Spain
             { "LKA", "LK"}, // Sri Lanka
             { "SDN", "SD"}, // Sudan
             { "SUR", "SR"}, // Suriname
             { "SJM", "SJ"}, // Svalbard and Jan Mayen Islands 
             { "SWZ", "SZ"}, // Swaziland
             { "SWE", "SE"}, // Sweden
             { "CHE", "CH"}, // Switzerland
             { "SYR", "SY"}, // Syrian Arab Republic (Syria)
             { "TWN", "TW"}, // Taiwan, Republic of China
             { "TJK", "TJ"}, // Tajikistan
             { "TZA", "TZ"}, // Tanzania, United Republic of
             { "THA", "TH"}, // Thailand
             { "TLS", "TL"}, // Timor-Leste
             { "TGO", "TG"}, // Togo
             { "TKL", "TK"}, // Tokelau 
             { "TON", "TO"}, // Tonga
             { "TTO", "TT"}, // Trinidad and Tobago
             { "TUN", "TN"}, // Tunisia
             { "TUR", "TR"}, // Turkey
             { "TKM", "TM"}, // Turkmenistan
             { "TCA", "TC"}, // Turks and Caicos Islands 
             { "TUV", "TV"}, // Tuvalu
             { "UGA", "UG"}, // Uganda
             { "UKR", "UA"}, // Ukraine
             { "ARE", "AE"}, // United Arab Emirates
             { "GBR", "GB"}, // United Kingdom
             { "USA", "US"}, // United States of America
             { "UMI", "UM"}, // US Minor Outlying Islands
             { "URY", "UY"}, // Uruguay
             { "UZB", "UZ"}, // Uzbekistan
             { "VUT", "VU"}, // Vanuatu
             { "VEN", "VE"}, // Venezuela (Bolivarian Republic)
             { "VNM", "VN"}, // Viet Nam
             { "VIR", "VI"}, // Virgin Islands, US
             { "WLF", "WF"}, // Wallis and Futuna Islands 
             { "ESH", "EH"}, // Western Sahara
             { "YEM", "YE"}, // Yemen
             { "ZMB", "ZM"}, // Zambia
             { "ZWE", "ZW"} // Zimbabwe
        };
    }
}
