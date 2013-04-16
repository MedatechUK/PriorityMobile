 
<!-- this file included below is used to create the SHA1 digital signature -->
<!--#include file="sha1.asp"-->

<%


'Pay and Shop Limited (Realex Payments) - Licence Agreement.
'© Copyright and zero Warranty Notice.
'
'
'Merchants and their internet, call centre, and wireless application
'developers (either in-house or externally appointed partners and
'commercial organisations) may access Realex Payments technical
'references, application programming interfaces (APIs) and other sample
'code and software ("Programs") either free of charge from
'www.realexpayments.com or by emailing info@realexpayments.com. 
'
'Realex Payments provides the programs "as is" without any warranty of
'any kind, either expressed or implied, including, but not limited to,
'the implied warranties of merchantability and fitness for a particular
'purpose. The entire risk as to the quality and performance of the
'programs is with the merchant and/or the application development
'company involved. Should the programs prove defective, the merchant
'and/or the application development company assumes the cost of all
'necessary servicing, repair or correction.
'
'Copyright remains with Realex Payments, and as such any copyright
'notices in the code are not to be removed. The software is provided as
'sample code to assist internet, wireless and call center application
'development companies integrate with the Realex Payments service.
'
'Any Programs licensed by Realex Payments to merchants or developers are
'licensed on a non-exclusive basis solely for the purpose of availing
'of the Realex Payments service in accordance with the
'written instructions of an authorised representative of Pay and Shop
'Limited. Any other use is strictly prohibited.
'

' Note:>
' The below code is used to grab the fields Realex Payments POSTs back 
' to this script after a card has been authorised. Realex Payments need
' to know the full URL of this script in order to POST the data back to this
' script. Please inform Realex Payments of this URL if they do not have it 
' already.

' Look at the Realex Documentation to view all hidden fields Realex POSTs back
' for a card transaction.

timestamp = Request.Form("TIMESTAMP")
result = Request.Form("RESULT")
orderid = Request.Form("ORDER_ID")
message = Request.Form("MESSAGE")
authcode = Request.Form("AUTHCODE")
pasref = Request.Form("PASREF")
sha1hash = Request.Form("SHA1HASH")



' -------------------------------------------------------------
' Replace these with the values you receive from Realex Payments. If you do not have
' these values please contact Realex Payments.

merchantid = "yourMerchantId"
secret = "your shared secret"



' Below is the code for creating the digital signature using the SHA1 
' algorithm. The calcSHA1 function is in the file sha1.asp included
' at the top of this file. 
' This digital siganture should correspond to the one Realex Payments POSTs back to
' this script and can therefore be used to verify the message Realex sends back.

temp = timestamp & "." & merchantid & "." & orderid & "." & _
	result & "." & message & "." & pasref & "." & authcode

temp1 = calcSHA1(temp)
temp2 = temp1 & "." & secret
hash = calcSHA1(temp2)

' Check to see if hashes match
if (hash <> sha1hash) Then 
	Response.write "The response did not authenticate."
End if




' The next part is important to understand. The result field sent back to this
' response script will indicate whether the card transaction was successful or not.
' The result 00 indicates it was while anything else indicates it failed. 
' Refer to the Realex Payments documentation to get a full list to response codes.


' IMPORTANT: Whatever this response script prints is grabbed by Realex Payments
' and placed in the template again. It is placed wherever the comment <!--E-PAGE TABLE HERE-->
' is in the template you provide.
'
' This is the case so that from a customer's perspective, they are not suddenly removed from 
' a secure site to an unsecure site. This means that although we call this response script the 
' customer is still on Realex PAyemnt's site and therefore it is recommended that a HTML link is
' printed in order to redirect the customrer back to the merchants site.




' The card authorised
if (result = "00") Then

	Response.Write "Thank You"
%>

<br /><br />
To continue browsing please <a href="http://yourdomain.com"><b><u>click here</u></b></a>
<br /><br />


<%
' The transaction failed
Else
%>


<br /><br />
There was an error processing your subscription.  

<br /><br />
To try again please <a href="http://yourdomain.com"><b><u>click here</u></b></a><br><BR>

NOTE: This link should bring the customer back to a place where an new orderid is<br>
created so that they can try to use another card. It is important that a new orderid<BR>
is created because if the same orderid is sent in a second time Realex Payments will
reject it as a duplicate order even if the first transaction was declined.<BR>
<br /><br />
Please contact our customer care department at <a href="mailto:custcare@yourdomain.com">
<b><u>custcare@yourdomain.com</u></b></a>or if you would prefer to subscribe by phone, 
call on 01 2839428349

<%
End If
%>
