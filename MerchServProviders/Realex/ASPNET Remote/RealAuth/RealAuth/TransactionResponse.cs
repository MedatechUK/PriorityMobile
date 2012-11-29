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
using System.Text;

using System.Xml;


namespace RealexPayments {

	namespace RealAuth {

		public class TransactionResponse {

			private int m_resultCode;
			private String m_resultMessage;
			private String m_resultAuthCode;
			private String m_resultPASRef;
			private String m_resultOrderID;
			private int m_resultSuitabilityScore;
			private Dictionary<int, int> m_resultSuitabilityScoreCheck;

            //TODO: if you have sent Realex additional variables and would like to retrieve them:
            //private String m_resultMyInterestingVariableName;

			public int ResultCode {
				get {
					return (m_resultCode);
				}
			}

			public String ResultMessage {
				get {
					return (m_resultMessage);
				}
			}

			public String ResultAuthCode {
				get {
					return (m_resultAuthCode);
				}
			}

			public String ResultPASRef {
				get {
					return (m_resultPASRef);
				}
			}

			public String ResultOrderID {
				get {
					return (m_resultOrderID);
				}
			}

			public int ResultSuitabilityScore {
				get {
					return (m_resultSuitabilityScore);
				}
			}

			public int ResultSuitabilityScoreCheck(int checkID) {
				return(m_resultSuitabilityScoreCheck[checkID]);
			}

            //TODO: if you have sent Realex additional variables and would like to retrieve them:
            /*
            public String ResultMyInterestingVariableName {
                get {
                    return (m_resultMyInterestingVariableName);
                }
                set {
                    m_resultMyInterestingVariableName = value;
                }
            }
            */

			public TransactionResponse(String responseXML) {

				m_resultSuitabilityScoreCheck = new Dictionary<int,int>();

				XmlDocument xml = new XmlDocument();
				xml.LoadXml(responseXML);

				try {

					// these *must* exist
					m_resultCode = Convert.ToInt32(xml.GetElementsByTagName("result")[0].InnerText);
					m_resultMessage = xml.GetElementsByTagName("message")[0].InnerText ;

					// these should exist, but don't throw exceptions if they don't.
					XmlNode el;
					el = xml.GetElementsByTagName("pasref")[0];
					m_resultPASRef = (el != null) ? el.InnerText : "";

					el = xml.GetElementsByTagName("authcode")[0];
					m_resultAuthCode = (el != null) ? el.InnerText : "";

					el = xml.GetElementsByTagName("orderid")[0];
					m_resultOrderID = (el != null) ? el.InnerText : "";

					el = xml.GetElementsByTagName("tss")[0];
					if (el != null) {
						foreach (XmlNode node in el.ChildNodes) {
							switch (node.Name) {
								case ("result"):
									m_resultSuitabilityScore = Convert.ToInt32(node.InnerText);
									break;
								case ("check"):
									foreach (XmlAttribute attr in node.Attributes) {
										if (attr.Name == "id") {
											m_resultSuitabilityScoreCheck.Add(Convert.ToInt32(attr.InnerText), Convert.ToInt32(node.InnerText));
										}
									}
									break;
							}
						}
					}

                    //TODO: if you have sent Realex additional variables and would like to retrieve them:
                    /*
                    el = xml.GetElementsByTagName("MyInterestingVariable")[0];
                    if (el != null) {
                        m_resultMyInterestingVariableName = el.InnerText;
                    }
                    */
				} catch (NullReferenceException e) {
					throw new TransactionFailedException("Error parsing XML response: mandatory fields not present. " + e.Message);
				}
			}

		}
	}
}
