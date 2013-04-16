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
'written instructions of an authorised representative of Realex Payments.
'Any other use is strictly prohibited.
'



    Private Function AndW(ByRef pBytWord1Ary, ByRef pBytWord2Ary)
        Dim lBytWordAry(3)
        Dim lLngIndex
        For lLngIndex = 0 To 3
            lBytWordAry(lLngIndex) = CByte(pBytWord1Ary(lLngIndex) And pBytWord2Ary(lLngIndex))
        Next
        AndW = lBytWordAry
    End Function

	
    Private Function OrW(ByRef pBytWord1Ary, ByRef pBytWord2Ary)
        Dim lBytWordAry(3)
        Dim lLngIndex
        For lLngIndex = 0 To 3
            lBytWordAry(lLngIndex) = CByte(pBytWord1Ary(lLngIndex) Or pBytWord2Ary(lLngIndex))
        Next
        OrW = lBytWordAry
    End Function


    Private Function XorW(ByRef pBytWord1Ary, ByRef pBytWord2Ary)
        Dim lBytWordAry(3)
        Dim lLngIndex
        For lLngIndex = 0 To 3
            lBytWordAry(lLngIndex) = CByte(pBytWord1Ary(lLngIndex) Xor pBytWord2Ary(lLngIndex))
        Next
        XorW = lBytWordAry
    End Function


    Private Function NotW(ByRef pBytWordAry)
        Dim lBytWordAry(3)
        Dim lLngIndex
        For lLngIndex = 0 To 3
            lBytWordAry(lLngIndex) = Not CByte(pBytWordAry(lLngIndex))
        Next
        NotW = lBytWordAry
    End Function


    Private Function AddW(ByRef pBytWord1Ary, ByRef pBytWord2Ary)
        Dim lLngIndex
        Dim lIntTotal
        Dim lBytWordAry(3)
        For lLngIndex = 3 To 0 Step -1
            If lLngIndex = 3 Then
                lIntTotal = CInt(pBytWord1Ary(lLngIndex)) + pBytWord2Ary(lLngIndex)
                lBytWordAry(lLngIndex) = lIntTotal Mod 256
            Else
                lIntTotal = CInt(pBytWord1Ary(lLngIndex)) + pBytWord2Ary(lLngIndex) + (lIntTotal \ 256)
                lBytWordAry(lLngIndex) = lIntTotal Mod 256
            End If
        Next
        AddW = lBytWordAry
    End Function


    Private Function CircShiftLeftW(ByRef pBytWordAry, ByRef pLngShift)
        Dim lDbl1
        Dim lDbl2
        lDbl1 = WordToDouble(pBytWordAry)
        lDbl2 = lDbl1
        lDbl1 = CDbl(lDbl1 * (2 ^ pLngShift))
        lDbl2 = CDbl(lDbl2 / (2 ^ (32 - pLngShift)))
        CircShiftLeftW = OrW(DoubleToWord(lDbl1), DoubleToWord(lDbl2))
    End Function


    Private Function WordToHex(ByRef pBytWordAry)
        Dim lLngIndex
        For lLngIndex = 0 To 3
            WordToHex = WordToHex & Right("0" & Hex(pBytWordAry(lLngIndex)), 2)
        Next
    End Function


    Private Function HexToWord(ByRef pStrHex)
        HexToWord = DoubleToWord(CDbl("&h" & pStrHex)) ' needs "#" at end for VB?
    End Function


    Private Function DoubleToWord(ByRef pDblValue)
        Dim lBytWordAry(3)
        lBytWordAry(0) = Int(DMod(pDblValue, 2 ^ 32) / (2 ^ 24))
        lBytWordAry(1) = Int(DMod(pDblValue, 2 ^ 24) / (2 ^ 16))
        lBytWordAry(2) = Int(DMod(pDblValue, 2 ^ 16) / (2 ^ 8))
        lBytWordAry(3) = Int(DMod(pDblValue, 2 ^ 8))
        DoubleToWord = lBytWordAry
    End Function


    Private Function WordToDouble(ByRef pBytWordAry)
        WordToDouble = CDbl((pBytWordAry(0) * (2 ^ 24)) + (pBytWordAry(1) * (2 ^ 16)) + (pBytWordAry(2) * (2 ^ 8)) + pBytWordAry(3))
    End Function


    Private Function DMod(ByRef pDblValue, ByRef pDblDivisor)
        Dim lDblMod
        lDblMod = CDbl(CDbl(pDblValue) - (Int(CDbl(pDblValue) / CDbl(pDblDivisor)) * CDbl(pDblDivisor)))
        If lDblMod < 0 Then
            lDblMod = CDbl(lDblMod + pDblDivisor)
        End If
        DMod = lDblMod
    End Function


    Private Function F( _
        ByRef lIntT, _
        ByRef pBytWordBAry, _
        ByRef pBytWordCAry, _
        ByRef pBytWordDAry _
        )
        If lIntT <= 19 Then
            F = OrW(AndW(pBytWordBAry, pBytWordCAry), AndW((NotW(pBytWordBAry)), pBytWordDAry))
        ElseIf lIntT <= 39 Then
            F = XorW(XorW(pBytWordBAry, pBytWordCAry), pBytWordDAry)
        ElseIf lIntT <= 59 Then
            F = OrW(OrW(AndW(pBytWordBAry, pBytWordCAry), AndW(pBytWordBAry, pBytWordDAry)), AndW(pBytWordCAry, pBytWordDAry))
        Else
            F = XorW(XorW(pBytWordBAry, pBytWordCAry), pBytWordDAry)
        End If
    End Function



    Public Function calcSHA1(ByVal pStrMessage)
        Dim lLngLen
        Dim lBytLenW
        Dim lStrPadMessage
        Dim lLngNumBlocks
        Dim lVarWordWAry(79)
        Dim lLngTempWordWAry
        Dim lStrBlockText
        Dim lStrWordText
        Dim lLngBlock
        Dim lIntT
        Dim lBytTempAry
        Dim lVarWordKAry(3)
        Dim lBytWordH0Ary
        Dim lBytWordH1Ary
        Dim lBytWordH2Ary
        Dim lBytWordH3Ary
        Dim lBytWordH4Ary
        Dim lBytWordAAry
        Dim lBytWordBAry
        Dim lBytWordCAry
        Dim lBytWordDAry
        Dim lBytWordEAry
        Dim lBytWordFAry
        lLngLen = Len(pStrMessage)
        lBytLenW = DoubleToWord(CDbl(lLngLen) * 8)
        lStrPadMessage = pStrMessage & Chr(128) & String((128 - (lLngLen Mod 64) - 9) Mod 64, Chr(0)) & _
        String(4, Chr(0)) & Chr(lBytLenW(0)) & Chr(lBytLenW(1)) & Chr(lBytLenW(2)) & Chr(lBytLenW(3))
        lLngNumBlocks = Len(lStrPadMessage) / 64
        lVarWordKAry(0) = HexToWord("5A827999")
        lVarWordKAry(1) = HexToWord("6ED9EBA1")
        lVarWordKAry(2) = HexToWord("8F1BBCDC")
        lVarWordKAry(3) = HexToWord("CA62C1D6")
        lBytWordH0Ary = HexToWord("67452301")
        lBytWordH1Ary = HexToWord("EFCDAB89")
        lBytWordH2Ary = HexToWord("98BADCFE")
        lBytWordH3Ary = HexToWord("10325476")
        lBytWordH4Ary = HexToWord("C3D2E1F0")
        For lLngBlock = 0 To lLngNumBlocks - 1
            lStrBlockText = Mid(lStrPadMessage, (lLngBlock * 64) + 1, 64)
            For lIntT = 0 To 15
                lStrWordText = Mid(lStrBlockText, (lIntT * 4) + 1, 4)
                lVarWordWAry(lIntT) = Array(Asc(Mid(lStrWordText, 1, 1)), Asc(Mid(lStrWordText, 2, 1)), Asc(Mid(lStrWordText, 3, 1)), Asc(Mid(lStrWordText, 4, 1)))
            Next
            For lIntT = 16 To 79
                lVarWordWAry(lIntT) = CircShiftLeftW(XorW(XorW(XorW(lVarWordWAry(lIntT - 3), lVarWordWAry(lIntT - 8)), lVarWordWAry(lIntT - 14)), lVarWordWAry(lIntT - 16)), 1)
            Next
            lBytWordAAry = lBytWordH0Ary
            lBytWordBAry = lBytWordH1Ary
            lBytWordCAry = lBytWordH2Ary
            lBytWordDAry = lBytWordH3Ary
            lBytWordEAry = lBytWordH4Ary
            For lIntT = 0 To 79
                lBytWordFAry = F(lIntT, lBytWordBAry, _
                    lBytWordCAry, lBytWordDAry)
                lBytTempAry = AddW(AddW(AddW(AddW(CircShiftLeftW(lBytWordAAry, 5), lBytWordFAry), lBytWordEAry), lVarWordWAry(lIntT)), lVarWordKAry(lIntT \ 20))
                lBytWordEAry = lBytWordDAry
                lBytWordDAry = lBytWordCAry
                lBytWordCAry = CircShiftLeftW(lBytWordBAry, 30)
                lBytWordBAry = lBytWordAAry
                lBytWordAAry = lBytTempAry
            Next
            lBytWordH0Ary = AddW(lBytWordH0Ary, lBytWordAAry)
            lBytWordH1Ary = AddW(lBytWordH1Ary, lBytWordBAry)
            lBytWordH2Ary = AddW(lBytWordH2Ary, lBytWordCAry)
            lBytWordH3Ary = AddW(lBytWordH3Ary, lBytWordDAry)
            lBytWordH4Ary = AddW(lBytWordH4Ary, lBytWordEAry)
        Next
        calcSHA1 = _
            WordToHex(lBytWordH0Ary) & _
            WordToHex(lBytWordH1Ary) & _
            WordToHex(lBytWordH2Ary) & _
            WordToHex(lBytWordH3Ary) & _
            WordToHex(lBytWordH4Ary)
            
        calcSHA1 = LCase(calcSHA1)
    End Function

%>
