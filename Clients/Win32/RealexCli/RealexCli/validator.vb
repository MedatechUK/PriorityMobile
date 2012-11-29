'
'Pay and Shop Limited (payandshop.com) - Licence Agreement.
'© Copyright and zero Warranty Notice.
'
'
'Merchants and their internet, call centre, and wireless application
'developers (either in-house or externally appointed partners and
'commercial organisations) may access payandshop.com technical
'references, application programming interfaces (APIs) and other sample
'code and software ("Programs") either free of charge from
'www.payandshop.com or by emailing info@payandshop.com. 
'
'payandshop.com provides the programs "as is" without any warranty of
'any kind, either expressed or implied, including, but not limited to,
'the implied warranties of merchantability and fitness for a particular
'purpose. The entire risk as to the quality and performance of the
'programs is with the merchant and/or the application development
'company involved. Should the programs prove defective, the merchant
'and/or the application development company assumes the cost of all
'necessary servicing, repair or correction.
'
'Copyright remains with payandshop.com, and as such any copyright
'notices in the code are not to be removed. The software is provided as
'sample code to assist internet, wireless and call center application
'development companies integrate with the payandshop.com service.
'
'Any Programs licensed by Pay and Shop to merchants or developers are
'licensed on a non-exclusive basis solely for the purpose of availing
'of the Pay and Shop payment solution service in accordance with the
'written instructions of an authorised representative of Pay and Shop
'Limited. Any other use is strictly prohibited.
'


Imports System.Collections.Generic
Imports System.Text

Imports System.Text.RegularExpressions

Namespace RealexPayments
    Namespace RealAuth

        Class Validator

            Public Shared Sub assertAlphaNumericLoose(ByVal fieldName As [String], ByVal s As [String])
                If Not Regex.IsMatch(s, "^[A-Za-z0-9_\-\.,\+@\ ]*$") Then
                    Throw New DataValidationException(fieldName & " may only contain characters A-Z a-z 0-9 _ - . , + @ and space characters.")
                End If
            End Sub

            Public Shared Sub assertAlphaNumericSpace(ByVal fieldName As [String], ByVal s As [String])
                If Not Regex.IsMatch(s, "^[A-Za-z0-9\ ]*$") Then
                    Throw New DataValidationException(fieldName & " may only contain alpha, numeric and space characters.")
                End If
            End Sub

            Public Shared Sub assertAlphaNumericStrict(ByVal fieldName As [String], ByVal s As [String])
                If Not Regex.IsMatch(s, "^[A-Za-z0-9]*$") Then
                    Throw New DataValidationException(fieldName & " may only contain alphanumeric characters.")
                End If
            End Sub

            Public Shared Sub assertAlphaStrict(ByVal fieldName As [String], ByVal s As [String])
                If Not Regex.IsMatch(s, "^[A-Za-z]*$") Then
                    Throw New DataValidationException(fieldName & " may only contain alpha characters.")
                End If
            End Sub

            Public Shared Sub assertNumeric(ByVal fieldName As [String], ByVal s As [String])
                If Not Regex.IsMatch(s, "^[0-9]*$") Then
                    Throw New DataValidationException(fieldName & " may only contain numeric characters.")
                End If
            End Sub

            Public Shared Sub assertLength(ByVal fieldName As [String], ByVal s As [String], ByVal exactLength As Integer)
                If s.Length <> exactLength Then
                    Throw New DataValidationException(fieldName & " must be exactly " & exactLength & "characters in length.")
                End If
            End Sub

            Public Shared Sub assertLength(ByVal fieldName As [String], ByVal s As [String], ByVal minLength As Integer, ByVal maxLength As Integer)
                If (s.Length < minLength) OrElse (s.Length > maxLength) Then
                    Throw New DataValidationException(fieldName & " must be between " & minLength & " and " & maxLength & " characters (inclusive) in length.")
                End If
            End Sub

        End Class
    End Namespace
End Namespace
