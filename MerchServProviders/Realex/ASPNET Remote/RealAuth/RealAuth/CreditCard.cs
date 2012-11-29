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

		public class CreditCard {

			public const int CVN_PRESENT = 1;
			public const int CVN_ILLEGIBLE = 2;
			public const int CVN_NOT_REQUESTED_BY_MERCHANT = 3;
			public const int CVN_NOT_ON_CARD = 4;

			public String CardNumber {
				get {
					return (m_number);
				}

				set {
					if ((value.Length >= 12) && (value.Length <= 19)) {
						m_number = value;
					} else {
						throw new DataValidationException("Card number fails Luhn check.");
					}
				}
			}

			public String CardType {
				get {
					return (m_cctype);
				}

				set {
					if (value.Equals("")) {
						throw new DataValidationException("Card type must not be blank.");
					}

					value = value.ToUpper();

					switch (value) {
						case ("VISA"):
						case ("MC"):
						case ("AMEX"):
						case ("LASER"):
						case ("DINERS"):
						case ("SWITCH"):
						case ("SOLO"):
						case ("JCB"):
							m_cctype = value;
							break;
						default:
							throw new DataValidationException("Invalid credit card type specified.");
					}
				}
			}

			public int IssueNumber {
				get {
					return (m_issueNumber);
				}

				set {
					if (!m_cctype.Equals("SWITCH")) {
						throw new DataValidationException("Issue numbers are only used by Switch cards.");
					}

					m_issueNumber = value;
				}
			}

			public String ExpiryDate {
				get {
					return (m_expiryDate);
				}

				set {
					if (value.Length != 4) {
						throw new DataValidationException("Card expiry length is incorrect (must be exactly 4 digits).");
					}

					String sMonth = value.Substring(0, 2);
					String sYear = value.Substring(2, 2);
					int iMonth = Int16.Parse(sMonth);
					int iYear = Int16.Parse(sYear);
					int currentYear = new DateTime().Year % 100;

					if (!((iMonth >= 1) && (iMonth <= 12))) {
						throw new DataValidationException("Card expiry month must be between 01 and 12 inclusive.");
					}

					if (iYear < (currentYear - 1)) {	// refunds can be made to expired cards
						throw new DataValidationException("Card expiry year is too far into the past.");
					}

					if (iYear > (currentYear + 20)) {
						throw new DataValidationException("Card expiry year is too far into the future.");
					}

					m_expiryDate = value;
				}
			}

			public String CardholderName {
				get {
					return (m_cardholderName);
				}

				set {
					if (value.Equals("")) {
						throw new DataValidationException("Cardholder name must not be empty.");
					}

					m_cardholderName = value;
				}
			}

			public String CVN {
				get {
					return (m_cvn);
				}

				set {
					if ((value.Length != 3) && (value.Length != 4)) {
						throw new DataValidationException("CVN must be 3 or 4 digits in length.");
					}

					m_cvn = value;
				}
			}

			public int CVNPresent {
				get {
					return (m_cvnPresent);
				}

				set {
					if (!(value >= 1) && (value <= 4)) {
						throw new DataValidationException("Invalid CVN status. Please use the defined constants.");
					}

					m_cvnPresent = value;
				}
			}

			private String m_cctype;
			private String m_number;
			private int m_issueNumber;	// Switch cards only
			private String m_expiryDate;
			private String m_cardholderName;
			private String m_cvn;
			private int m_cvnPresent;


			public CreditCard(String cctype, String number, String expiryDate, String cardholderName, String cvn, int cvnPresent) {
				CardType = cctype;
				CardNumber = number;
				ExpiryDate = expiryDate;
				CardholderName = cardholderName;
				CVN = cvn;
				CVNPresent = cvnPresent;
			}

			public CreditCard(String cctype, String number, String expiryDate, String cardholderName, String cvn, int cvnPresent, int issueNumber) {
				CardType = cctype;
				CardNumber = number;
				ExpiryDate = expiryDate;
				CardholderName = cardholderName;
				CVN = cvn;
				CVNPresent = cvnPresent;
				IssueNumber = issueNumber;
			}

			public void WriteXML(XmlWriter xml) {

				xml.WriteStartElement("card");
				{
					xml.WriteElementString("number", m_number);
					xml.WriteElementString("expdate", m_expiryDate);
					xml.WriteElementString("type", m_cctype);
					xml.WriteElementString("chname", m_cardholderName);
					if (m_cctype.Equals("SWITCH")) {
						xml.WriteElementString("issueno", m_issueNumber.ToString());
					}
					xml.WriteStartElement("cvn");
					{
						xml.WriteElementString("number", m_cvn);
						xml.WriteElementString("presind", m_cvnPresent.ToString());
					}
					xml.WriteEndElement();
				}
				xml.WriteEndElement();
			}

		}

	}

}
