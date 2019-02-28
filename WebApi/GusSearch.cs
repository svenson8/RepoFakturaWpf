using FakturaWpf.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FakturaWpf.WebApi
{
    class GusSearch
    {
        private static readonly HttpClient client = new HttpClient();
        private const string sendUri = "https://wyszukiwarkaregontest.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc";
        private const string actionUrl = "http://CIS/BIR/PUBL/2014/07/IUslugaBIRzewnPubl/";
        private const string userKey = "abcde12345abcde12345";
        private string logKey;
        private string fSessionId;
        private string fErrorMessage;
        private XmlDocument fSearchResult;
        private HttpContent fRequest;


        public GusSearch()
        {
            fErrorMessage = "";
            fSearchResult = new XmlDocument();
        }

        private async Task<Boolean> Login() 
        {
            string body;
            body = "<ns:pKluczUzytkownika>" + userKey + "</ns:pKluczUzytkownika>" + Environment.NewLine;
            fSessionId = await GetResponse("Zaloguj", body);
            return (fSessionId != "");
        }

        private async Task Logout()
        {
            string body;
            body = "<ns:pIdentyfikatorSesji>" + fSessionId + "</ns:pIdentyfikatorSesji>" + Environment.NewLine;
            await GetResponse("Wyloguj", body);
        }


        private async Task GetErrorMessage()
        {
            string body;
            body = "<ns:DaneKomunikat/>" + Environment.NewLine;
            fErrorMessage = await GetResponse("DaneKomunikat", body);

        }

        private async Task<Boolean> FullReport(string regon)
        {
            string body;
            body =  "<ns:pRegon>" + regon + "</ns:pRegon>" + Environment.NewLine;
            body += "<ns:pRegon>" + regon + "</ns:pRegon>" + Environment.NewLine;
            body += "<ns:pNazwaRaportu>PublDaneRaportPrawna</ns:pNazwaRaportu>" + Environment.NewLine;

              try{
                  fSearchResult.LoadXml(await GetResponse("DanePobierzPelnyRaport", body)); }
              catch{ }

              if (fSearchResult.ChildNodes.Count == 0)
                  await GetErrorMessage();

              return (fSearchResult.ChildNodes.Count != 0); 
            await GetErrorMessage();
            return false;
        }

        private async Task<Boolean> SearchGustByData(string nip, string regon, string krs)
        {
            string searchFor = "";
            string body = "";

            if (nip.Length > 3)
                searchFor = "  <dat:Nip>" + nip.Trim() + "</dat:Nip>" + Environment.NewLine;

            if (regon.Length > 3)
                searchFor = "  <dat:Regon>" + regon.Trim() + "</dat:Regon>" + Environment.NewLine;

            if (krs.Length > 3) 
              searchFor = "  <dat:Krs>" + krs.Trim() + "</dat:Krs>" + Environment.NewLine;

            body =  "<ns:pParametryWyszukiwania>" + Environment.NewLine;
            body += searchFor;
            body += "</ns:pParametryWyszukiwania>" + Environment.NewLine;

            try {
                fSearchResult.LoadXml(await GetResponse("DaneSzukaj", body)); }
            catch { }

            if (fSearchResult.ChildNodes.Count == 0)
                await GetErrorMessage();

            return (fSearchResult.ChildNodes.Count != 0);
        }

        private async Task<string> GetResponse(string action, string body)
        {
            fErrorMessage = "";
            //set FRequest
            var restr = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:ns=""http://CIS/BIR/PUBL/2014/07"" xmlns:dat=""http://CIS/BIR/PUBL/2014/07/DataContract""> " + Environment.NewLine+
                        @"<soap:Header xmlns:wsa=""http://www.w3.org/2005/08/addressing""> " + Environment.NewLine +
                           "<wsa:To>" + sendUri + "</wsa:To> " + Environment.NewLine +
                           "<wsa:Action>"+ actionUrl + action+"</wsa:Action> " + Environment.NewLine +
                           "</soap:Header> " + Environment.NewLine +
                           "    <soap:Body> " + Environment.NewLine +
                           "      <ns:" + action + ">" + Environment.NewLine +
                           body +
                           "      </ns:" + action + ">" + Environment.NewLine +
                           "    </soap:Body> " + Environment.NewLine +
                           "</soap:Envelope> ";

            fRequest = new StringContent(restr, Encoding.UTF8, "application/soap+xml");


            //set custom headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("action", actionUrl + action);
            if (fSessionId != "")
              client.DefaultRequestHeaders.Add("sid", fSessionId);

            //send Request
            var response = await client.PostAsync(sendUri, fRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            
            if (responseString.IndexOf("<" + action + "Result>") > 0) 
                {
                var strout = Various.StringBeetween(responseString, "<" + action + "Result>", "</" + action + "Result>");
                strout = strout.Replace("&lt;", "<");
                strout = strout.Replace("&gt;", ">");
                strout = strout.Replace("&#xD;", "");
                return strout;
            }
            else
                return "";  

        }

        public async Task<CustomerClass> StartGusSearching(string nip, string regon, string krs)
        {
            CustomerClass ansCus = new CustomerClass();

            try
            {                
                if (await Login())
                {
                    if (await SearchGustByData(nip, regon, krs))
                    {
                        var root = "/root/dane/";
                        ansCus.KLINIP = nip;
                        ansCus.KLINAZ = fSearchResult.DocumentElement.SelectSingleNode(root + "Nazwa").InnerText;
                        ansCus.KLIULICA = fSearchResult.DocumentElement.SelectSingleNode(root + "Ulica").InnerText;
                        ansCus.KLINRDOMU = "";
                        ansCus.KLIKOD = fSearchResult.DocumentElement.SelectSingleNode(root + "KodPocztowy").InnerText;
                        ansCus.KLIMIEJSC = fSearchResult.DocumentElement.SelectSingleNode(root + "Miejscowosc").InnerText;
                        //ansCus.KLIWOJID = GetWojIdFromstring(fSearchResult.DocumentElement.SelectSingleNode(root + "Wojewodztwo").InnerText);
                        ansCus.KLIPOWIAT = fSearchResult.DocumentElement.SelectSingleNode(root + "Powiat").InnerText;
                        ansCus.KLIGMINA = fSearchResult.DocumentElement.SelectSingleNode(root + "Gmina").InnerText;

                        if ((fSearchResult.DocumentElement.SelectSingleNode(root + "Regon") != null) &&
                            (fSearchResult.DocumentElement.SelectSingleNode(root + "Regon").InnerText != ""))
                        {
                            var reg = fSearchResult.DocumentElement.SelectSingleNode(root + "Regon").InnerText;
                            if (await FullReport(reg))
                            {
                                ansCus.KLINIP = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_nip").InnerText;
                                ansCus.KLINAZ = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_nazwa").InnerText;
                                ansCus.KLIULICA = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzUlica_Nazwa").InnerText;
                                ansCus.KLINRDOMU = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzNumerNieruchomosci").InnerText;
                                ansCus.KLINRLOK = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzNumerLokalu").InnerText;
                                ansCus.KLIKOD = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzKodPocztowy").InnerText;
                                ansCus.KLIMIEJSC = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzMiejscowosc_Nazwa").InnerText;
                                // ansCus.KLIKRAJID  := fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzKraj_Nazwa").InnerText;
                                //ansCus.KLIWOJID   := fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzWojewodztwo_Nazwa").InnerText;
                                ansCus.KLIPOWIAT = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzPowiat_Nazwa").InnerText;
                                ansCus.KLIGMINA = fSearchResult.DocumentElement.SelectSingleNode(root + "praw_adSiedzGmina_Nazwa").InnerText;
                            }
                            else
                                ansCus.KLIUWAGI = fErrorMessage;
                        }

                        if (ansCus.KLINIP?.Length < 3) ansCus.KLINIP = nip;
                        if (ansCus.KLIREGON?.Length < 3) ansCus.KLIREGON = regon;

                    }
                    else
                        ansCus.KLIUWAGI = fErrorMessage;

                    await Logout();
                }

             
            } catch (Exception Ex)
            {
                Various.Warning(Ex.Message, "Błąd ...");
            }
            return ansCus;
        }
                
    }
}
