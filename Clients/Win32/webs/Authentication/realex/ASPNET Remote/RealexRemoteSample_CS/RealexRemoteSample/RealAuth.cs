using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using System.IO;

using RealexPayments.RealAuth;


namespace RealexPayments {

	namespace RealexRemoteSample {

		public class RealexSample {

			private static void sampleQuickAuth() {

				try {
					// four lines are all it takes to send in a transaction:
					CreditCard cReq = new CreditCard("visa", "4242424242424242", "0609", "Andrew Harcourt", "123", CreditCard.CVN_PRESENT);
					String orderID = DateTime.Now.ToString("yyyyMMddHHmmss");
					TransactionRequest tReq = new TransactionRequest("realexsample", "secret", "secret", "secret");
					TransactionResponse tResp = tReq.Authorize("internet", orderID, "EUR", 4133, cReq);

					if (tResp.ResultCode == 0) {	// success

						//TODO: Your code goes here.

					} else {	// failure
						//Check the Realex Developer Documentation for transaction result codes.

						//TODO: Your code goes here.

					}
				} catch (DataValidationException e) {
					// transaction not submitted

					//TODO: Your exception-handling code goes here.
					Console.WriteLine("Transaction not submitted: " + e.Message);

				} catch (TransactionFailedException e) {
					// transaction failed

					//TODO: Your exception-handling code goes here.
					Console.WriteLine("Transaction failed: " + e.Message);

				} catch (Exception e) {
					// something else bad happened

					//TODO: Your exception-handling code goes here.
					Console.WriteLine("Unhandled exception: " + e.Message);

				}

			}


			private static void sampleAuthAndRebate() {

				try {
					// four lines are all it takes to send in a transaction:
                    CreditCard cReq = new CreditCard("visa", "4242424242424242", "0609", "Andrew Harcourt", "123", CreditCard.CVN_PRESENT);
					String orderID = DateTime.Now.ToString("yyyyMMddHHmmss");
                    TransactionRequest tReq = new TransactionRequest("realexsample", "secret", "secret", "secret");

					// set up a few more transaction variables

					tReq.TransVariableReference = "Your variable reference here.";
					tReq.TransCustomerNumber = "Your customer number here.";
					tReq.TransBillingAddressCode = "Your billing address code here.";
					tReq.TransBillingAddressCountry = "ie";
					tReq.TransShippingAddressCode = "Your shipping address code here.";
					tReq.TransShippingAddressCountry = "au";
					tReq.TransComments.Add("Testing");
					tReq.TransAutoSettle = 1;

					tReq.TransAmount = 3183;
					tReq.TransAccountName = "internet";
					tReq.TransOrderID = orderID;
					tReq.TransCurrency = "EUR";
					tReq.TransCard = cReq;

					tReq.TransType = "auth";
					TransactionResponse tResp = tReq.SubmitTransaction();
					tReq.ContinueTransaction(tResp);	// load the results from that transaction straight back in to use again in the next one

					// err.. here is where you decide that you didn't really want to process that transaction, and give the cash back.

					tReq.TransType = "rebate";
                    tReq.TransAmount = 3183;
                    tReq.TransCurrency = "EUR";
					tResp = tReq.SubmitTransaction();

					Console.WriteLine("Got response:");
					Console.WriteLine("result: " + tResp.ResultCode);
					Console.WriteLine("authcode: " + tResp.ResultAuthCode);
					Console.WriteLine("pasref: " + tResp.ResultPASRef);
					Console.WriteLine("message: " + tResp.ResultMessage);

					if (tResp.ResultCode == 0) {	// success

						//TODO: Your code goes here.

					} else {	// failure
						//Check the Realex Developer Documentation for transaction result codes.

						//TODO: Your code goes here.

					}
				} catch (DataValidationException e) {
					// transaction not submitted

					//TODO: Your exception-handling code goes here.
					Console.WriteLine("Transaction not submitted: " + e.Message);

				} catch (TransactionFailedException e) {
					// transaction failed

					//TODO: Your exception-handling code goes here.
					Console.WriteLine("Transaction failed: " + e.Message);

				} catch (Exception e) {
					// something else bad happened

					//TODO: Your exception-handling code goes here.
					Console.WriteLine("Unhandled exception: " + e.Message);

				}

			}

            private static void sampleReallyQuickCredit() {

                try {
                    TransactionRequest tReq = new TransactionRequest("realexsample", "secret", "secret", "secret");

                    CreditCard cReq = new CreditCard("visa", "4242424242424242", "0609", "Andrew Harcourt", "123", CreditCard.CVN_PRESENT);
                    String orderID = DateTime.Now.ToString("yyyyMMddHHmmss");
                    TransactionResponse tResp = tReq.Credit("internet", orderID, "EUR", 4242, cReq);

                    if (tResp.ResultCode != 0) {
                        throw new ApplicationException("Transaction submitted but failed.");
                    }
                } catch (Exception e) {
                    Console.WriteLine("An exception occurred: " + e.Message);
                }
            }

            public static void Main()
            {

				// this is all you need to do for 99% of your transactions
				sampleQuickAuth();

				// if you want to get more involved, use this approach instead.
				sampleAuthAndRebate();

                // for a bare-bones example:
                sampleReallyQuickCredit();

			}
		}
	}
}
