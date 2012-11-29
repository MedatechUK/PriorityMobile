/*
Pay and Shop Limited (payandshop.com) - Licence Agreement.
© Copyright and zero Warranty Notice.


Merchants and their internet, call centre, and wireless application
developers (either in-house or externally appointed partners and
commercial organisations) may access payandshop.com technical
references, application programming interfaces (APIs) and other sample
code and software ("Programs") either free of charge from
www.payandshop.com or by emailing info@payandshop.com. 

payandshop.com provides the programs "as is" without any warranty of
any kind, either expressed or implied, including, but not limited to,
the implied warranties of merchantability and fitness for a particular
purpose. The entire risk as to the quality and performance of the
programs is with the merchant and/or the application development
company involved. Should the programs prove defective, the merchant
and/or the application development company assumes the cost of all
necessary servicing, repair or correction.

Copyright remains with payandshop.com, and as such any copyright
notices in the code are not to be removed. The software is provided as
sample code to assist internet, wireless and call center application
development companies integrate with the payandshop.com service.

Any Programs licensed by Pay and Shop to merchants or developers are
licensed on a non-exclusive basis solely for the purpose of availing
of the Pay and Shop payment solution service in accordance with the
written instructions of an authorised representative of Pay and Shop
Limited. Any other use is strictly prohibited.
*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using System.Security.Cryptography;
using System.Xml;
using System.Net;
using System.IO;

namespace RealexPayments {

	namespace RealAuth {

		public class TransactionRequest {

			/*
			 * <request timestamp="yyyymmddhhmmss" type="auth">
			 *		<merchantid>realexsample</merchantid>
			 *		<account>internet</account>
			 *		<orderid>12345</orderid>
			 * 
			 *		<amount currency="EUR">4133</amount>
			 *		<card>
			 *			<number>4242424242424242</number>
			 *			<expdate>0610</expdate>
			 *			<type>VISA</type>
			 *			<chname>Andrew Harcourt</chname>
			 *			<issueno>19</issueno>
			 *			<cvn>
			 *				<number>123</number>
			 *				<presind>1</presind>
			 *			</cvn>
			 *		</card>
			 *		<autosettle flag="1"/>
			 * 
			 *		<sha1hash>blahblahblah</sha1hash>
			 *		<comments>
			 *			<comment id="1"></comment>
			 *			<comment id="2"></comment>
			 *		</comments>
			 *		<tssinfo>
			 *			<address type="billing">
			 *				<code></code>
			 *				<country></country>
			 *			</address>
			 *			<address type="shipping">
			 *				<code></code>
			 *				<country></country>
			 *			</address>
			 *			<custnum></custnum>
			 *			<varref></varref>
			 *			<prodid></prodid>
			 *		</tssinfo>
			 * </request>
			 * 
			 */


			// used in every transaction
			private String m_normalPassword;
			private String m_rebatePassword;
			private String m_refundPassword;

			// used by all transaction types
			private String m_transType;
			private String m_transTimestamp;
			private String m_transMerchantName;
			private String m_transAccountName;
			private String m_transOrderID;
			
			private String m_transSHA1Hash;
			private ArrayList m_transComments;
			private String m_transBillingAddressCode;
			private String m_transBillingAddressCountry;
			private String m_transShippingAddressCode;
			private String m_transShippingAddressCountry;
			private String m_transCustomerNumber;
			private String m_transVariableReference;
			private String m_transProductID;

			// used by *some* transaction types
			private uint m_transAmount;
			private String m_transCurrency;
			private CreditCard m_transCard;
			private int m_transAutoSettle;
			private String m_transPASRef;
			private String m_transAuthCode;

            //TODO: add any additional instance variables you would like to send to Realex here
            //private String m_transMyInterestingVariableName;

			// public properties reprenting the protected vars above
			public String TransType {
				get {
					return (m_transType);
				}

				set {
					value = value.ToLower();
					switch (value) {

						case("auth"):
						case ("void"):
						case ("settle"):
						case ("credit"):
						case ("rebate"):
						case ("tss"):
						case ("offline"):
							m_transType = value;
							break;

						case ("magicnewtransactiontype"):
							throw new DataValidationException("Transaction type " + value + "not yet implemented.");

						default:
							throw new DataValidationException("Unknown transaction type requested.");
					}
				}
			}

			public String TransTimestamp {
				get {
					return (m_transTimestamp);
				}

				set {
					throw new ReadOnlyException("This property is read-only.");
				}
			}

			public String TransMerchantName {
				get {
					return (m_transMerchantName);
				}

				set {
					m_transMerchantName = value;
				}
			}

			public String TransAccountName {
				get {
					return (m_transAccountName);
				}

				set {
					m_transAccountName = value;
				}
			}

			public String TransOrderID {
				get {
					return (m_transOrderID);
				}

				set {
					m_transOrderID = value;
				}
			}

			public ArrayList TransComments {
				get {
					return (m_transComments);
				}

				set {
					//FIXME: how should this be done?
				}
			}

			public String TransBillingAddressCode {
				get {
					return (m_transBillingAddressCode);
				}

				set {
					m_transBillingAddressCode = value;
				}
			}

			public String TransBillingAddressCountry {
				get {
					return (m_transBillingAddressCountry);
				}

				set {
					value = value.ToUpper();
					Validator.assertAlphaStrict("Billing Country", value);
					Validator.assertLength("Billing Country", value, 2);
					m_transBillingAddressCountry = value;
				}
			}

			public String TransShippingAddressCode {
				get {
					return (m_transShippingAddressCode);
				}

				set {
					m_transShippingAddressCode = value;
				}
			}

			public String TransShippingAddressCountry {
				get {
					return (m_transShippingAddressCountry);
				}

				set {
					value = value.ToUpper();
					Validator.assertAlphaStrict("Shipping Country", value);
					Validator.assertLength("Shipping Country", value, 2);
					m_transShippingAddressCountry = value;
				}
			}

			public String TransCustomerNumber {
				get {
					return (m_transCustomerNumber);
				}

				set {
					Validator.assertAlphaNumericLoose("Customer Number", value);
					m_transCustomerNumber = value;
				}
			}

			public String TransVariableReference {
				get {
					return (m_transVariableReference);
				}

				set {
					Validator.assertAlphaNumericLoose("Variable Reference", value);
					m_transVariableReference = value;
				}
			}

			public String TransProductID {
				get {
					return (m_transProductID);
				}

				set {
					Validator.assertAlphaNumericLoose("Product ID", value);
					m_transProductID = value;
				}
			}


			// used by *some* transaction types
			public uint TransAmount {
				get {
					return (m_transAmount);
				}

				set {
					m_transAmount = value;
				}
			}

			public String TransCurrency {
				get {
					return (m_transCurrency);
				}

				set {
					m_transCurrency = value;
				}
			}

			public CreditCard TransCard {
				get {
					return (m_transCard);
				}

				set {
					m_transCard = value;
				}
			}

			public int TransAutoSettle {
				get {
					return (m_transAutoSettle);
				}

				set {
					m_transAutoSettle = value;
				}
			}

			public String TransPASRef {
				get {
					return (m_transPASRef);
				}

				set {
					m_transPASRef = value;
				}
			}

			public String TransAuthCode {
				get {
					return (m_transAuthCode);
				}

				set {
					m_transAuthCode = value;
				}
			}

            //TODO: Add your property handler(s) for your own variables here
            /*
            public String TransMyInterestingVariableName {
                get {
                    return (m_transMyInterestingVariableName);
                }
                set {
                    m_transMyInterestingVariableName = value;
                }
            }
            */


            // Constructor(s)
			public TransactionRequest(string merchantName, String normalPassword, String rebatePassword, String refundPassword) {

				m_transComments = new ArrayList();

				m_transMerchantName = merchantName;
				m_normalPassword = normalPassword;
				m_rebatePassword = rebatePassword;
				m_refundPassword = refundPassword;
                m_transAutoSettle = 1;
			}


            // Methods and other gack
			public void ContinueTransaction(TransactionResponse transactionResponse) {

				// nuke values that definitely won't be in the next transaction
				m_transTimestamp = null;
				m_transType = null;
				m_transSHA1Hash = null;

                TransPASRef = transactionResponse.ResultPASRef;
				TransAuthCode = transactionResponse.ResultAuthCode;
				TransOrderID = transactionResponse.ResultOrderID;
			}

			private  void generateTimestamp() {
				m_transTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
			}

			private String hexEncode(byte[] data) {

				String result = "";
				foreach (byte b in data) {
					result += b.ToString("X2");
				}
				result = result.ToLower();

				return (result);
			}

			private void generateSHA1Hash() {

				SHA1 sha = new SHA1Managed();

				String hashInput =
					m_transTimestamp + "." +
					m_transMerchantName + "." +
					m_transOrderID + "." +
					m_transAmount + "." +
					m_transCurrency + "." +
					m_transCard.CardNumber;

				String hashStage1 =
					hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashInput)))  + "." + 
					m_normalPassword;

				String hashStage2 =
					hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashStage1)));

				m_transSHA1Hash = hashStage2;
			}

			public TransactionResponse Authorize(String transAccount, String transOrderID, String transCurrency, uint transAmount, CreditCard transCard) {

				TransType = "auth";
				TransAccountName = transAccount;
				TransOrderID = transOrderID;
				TransCurrency = transCurrency;
				TransAmount = transAmount;
				TransCard = transCard;
				TransAutoSettle = 1;

				return (SubmitTransaction());
			}

			public TransactionResponse Rebate(String transAccount, String transOrderID, String transCurrency, uint transAmount, CreditCard transCard) {

				TransType = "rebate";
				TransAccountName = transAccount;
				TransOrderID = transOrderID;
				TransCurrency = transCurrency;
				TransAmount = transAmount;
				TransCard = transCard;
				TransAutoSettle = 1;

				return (SubmitTransaction());
			}

			public TransactionResponse Credit(String transAccount, String transOrderID, String transCurrency, uint transAmount, CreditCard transCard) {

				TransType = "credit";
				TransAccountName = transAccount;
				TransOrderID = transOrderID;
				TransCurrency = transCurrency;
				TransAmount = transAmount;
				TransCard = transCard;
				TransAutoSettle = 1;

				return(SubmitTransaction());
			}

			public TransactionResponse Void(String transAccount, String transOrderID, String transCurrency, uint transAmount, CreditCard transCard) {

				TransType = "void";
				TransAccountName = transAccount;
				TransOrderID = transOrderID;
				TransCurrency = transCurrency;
				TransAmount = transAmount;
				TransCard = transCard;

				return (SubmitTransaction());
			}
			
			public TransactionResponse TSS(String transAccount, String transOrderID, String transCurrency, uint transAmount, CreditCard transCard) {

				TransType = "tss";
				TransAccountName = transAccount;
				TransOrderID = transOrderID;
				TransCurrency = transCurrency;
				TransAmount = transAmount;
				TransCard = transCard;

				return (SubmitTransaction());
			}

			public TransactionResponse Offline(String transAccount, String transOrderID, String transCurrency, uint transAmount, CreditCard transCard) {

				TransType = "offline";
				TransAccountName = transAccount;
				TransOrderID = transOrderID;
				TransCurrency = transCurrency;
				TransAmount = transAmount;
				TransCard = transCard;
				TransAutoSettle = 0;

				return (SubmitTransaction());
			}

			public TransactionResponse SubmitTransaction() {

				String requestXML = this.ToXML();

				HttpWebRequest wReq = (HttpWebRequest) WebRequest.Create("https://epage.payandshop.com/epage-remote.cgi");
				wReq.ContentType = "text/xml";
				wReq.UserAgent = "Realex Payments C# Sample Code.";
				wReq.Timeout = 45 * 1000;	// milliseconds
				wReq.AllowAutoRedirect = false;
				wReq.ContentLength = requestXML.Length;
				wReq.Method = "POST";

				try {
					StreamWriter sReq = new StreamWriter(wReq.GetRequestStream());
					sReq.Write(requestXML);
					sReq.Flush();
					sReq.Close();

					// dump i/o to files for debugging purposes
					//TODO: if you have trouble with your requests, uncomment the line below to save a copy.
					// PLEASE remember to remove the line again before you go live; otherwise you will be keeping
					// your customers' credit card data in cleartext on your server.
					//File.WriteAllText("request.xml", requestXML);

					HttpWebResponse wResp = (HttpWebResponse)wReq.GetResponse();
					StreamReader sResp = new StreamReader(wResp.GetResponseStream());

					String responseXML = sResp.ReadToEnd();
					sResp.Close();

					// dump i/o to files for debugging purposes
					//TODO: if you have trouble with your requests, uncomment the line below to save a copy.
					// PLEASE remember to remove the line again before you go live; otherwise you will be keeping
					// your customers' credit card data in cleartext on your server.
					//File.WriteAllText("response.xml", responseXML);

					return (new TransactionResponse(responseXML));
				} catch (WebException e) {
					throw new TransactionFailedException("Web request failed or timed out: " + e.Message);
				}
			}

			protected String ToXML() {

				generateTimestamp();	// timestamp the request as it's generated
				generateSHA1Hash();	// ... and ensure that we have a correct hash

				// NOTE: element variable names are named in the XML case, not in camel case, so as to
				// avoid confusion in mapping between the two.

				XmlWriterSettings xmlSettings = new XmlWriterSettings();
				xmlSettings.Indent = true;
				xmlSettings.NewLineOnAttributes = false;
				xmlSettings.NewLineChars = "\r\n";
				xmlSettings.CloseOutput = true;

				StringBuilder strBuilder = new StringBuilder();

				XmlWriter xml = XmlWriter.Create(strBuilder, xmlSettings);

				xml.WriteStartDocument();

				xml.WriteStartElement("request");
				{
					xml.WriteAttributeString("timestamp", m_transTimestamp);
					xml.WriteAttributeString("type", m_transType);

					xml.WriteElementString("merchantid", m_transMerchantName);
					xml.WriteElementString("account", m_transAccountName);
					xml.WriteElementString("orderid", m_transOrderID);

					switch(m_transType) {
						case("auth"):
						case("credit"):
                        case("offline"):
                        case("rebate"):
						case("tss"):

							xml.WriteStartElement("amount");
							xml.WriteAttributeString("currency", m_transCurrency);
							xml.WriteString(m_transAmount.ToString());
							xml.WriteEndElement();

							m_transCard.WriteXML(xml);

							xml.WriteStartElement("autosettle");
							xml.WriteAttributeString("flag", m_transAutoSettle.ToString());
							xml.WriteEndElement();
							break;
					}

					switch(m_transType) {
                        case("credit"):
                        case("rebate"):
                        case("settle"):
                        case("void"):
                            xml.WriteElementString("pasref", m_transPASRef);
							break;
					}

					switch(m_transType) {
                        case("credit"):
                        case("offline"):
                        case("rebate"):
                        case("settle"):
                        case("void"):
                            xml.WriteElementString("authcode", m_transAuthCode);
							break;
					}

					xml.WriteElementString("sha1hash", m_transSHA1Hash);

					// if this is a transaction requiring an additional hash, include it here
					SHA1 sha = new SHA1Managed();
					switch (m_transType) {
                        case("credit"):
							String refundHash = hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(m_refundPassword)));
							xml.WriteElementString("refundhash", refundHash);
							break;
						case("rebate"):
							String rebateHash = hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(m_rebatePassword)));
							xml.WriteElementString("refundhash", rebateHash);   // this is still sent as "refundhash", not "rebatehash"
							break;
					}

					xml.WriteStartElement("comments");
					{
						int iComment = 1;	// this must start from 1, not 0.
						foreach (String comment in m_transComments) {
							xml.WriteStartElement("comment");
							xml.WriteAttributeString("id", iComment.ToString());
							xml.WriteString(comment);
							xml.WriteEndElement();

							iComment++;
						}
					}
					xml.WriteEndElement();

					xml.WriteStartElement("tssinfo");
					{
						{
							xml.WriteStartElement("address");
							xml.WriteAttributeString("type", "billing");
							xml.WriteElementString("code", m_transBillingAddressCode);
							xml.WriteElementString("country", m_transBillingAddressCountry);
							xml.WriteEndElement();
						}

						{
							xml.WriteStartElement("address");
							xml.WriteAttributeString("type", "shipping");
							xml.WriteElementString("code", m_transShippingAddressCode);
							xml.WriteElementString("country", m_transShippingAddressCountry);
							xml.WriteEndElement();
						}

						{
							xml.WriteElementString("custnum", m_transCustomerNumber);
							xml.WriteElementString("varref", m_transVariableReference);
							xml.WriteElementString("prodid", m_transProductID);
						}
					}
					xml.WriteEndElement();
				}

                //TODO: if you wish to send Realex any additional variables, include them here
                //xml.WriteElementString("MyInterestingVariable", m_myInterestingVariableName);

				xml.WriteEndElement();

				xml.Flush();
				xml.Close();

				return(strBuilder.ToString());
			}

		}

	}

}

