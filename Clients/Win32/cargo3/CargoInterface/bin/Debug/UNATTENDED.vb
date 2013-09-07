Imports system
Imports system.drawing

Namespace unattended
    
    Public Class funcs
        
        Public Function Scan_Glacial_Open_Scaner(ByVal Scanner_Open1 As Boolean, ByVal Scanner_Open2 As Boolean, ByVal Scanner_Open3 As Boolean, ByVal Scanner_Open4 As Boolean, ByVal Scanner_Open5 As Boolean) As Boolean
            Dim Result as System.Boolean = (NOT ( Scanner_Open1 AND Scanner_Open2 AND Scanner_Open3 AND Scanner_Open4 AND Scanner_Open5 ))
            Return Result
        End Function
        
        Public Function Scan_Glacial_Open_Scaner_Scanner_Open1(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2366,12)
            if not((this.r > 164 - 12.5) and (this.r < 164 + 12.5)) then Return false
            if not((this.g > 160 - 12.5) and (this.g < 160 + 12.5)) then Return false
            if not((this.b > 159 - 12.5) and (this.b < 159 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Open_Scaner_Scanner_Open2(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2344,7)
            if not((this.r > 155 - 12.5) and (this.r < 155 + 12.5)) then Return false
            if not((this.g > 150 - 12.5) and (this.g < 150 + 12.5)) then Return false
            if not((this.b > 149 - 12.5) and (this.b < 149 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Open_Scaner_Scanner_Open3(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2359,12)
            if not((this.r > 162 - 12.5) and (this.r < 162 + 12.5)) then Return false
            if not((this.g > 158 - 12.5) and (this.g < 158 + 12.5)) then Return false
            if not((this.b > 157 - 12.5) and (this.b < 157 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Open_Scaner_Scanner_Open4(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2342,9)
            if not((this.r > 162 - 12.5) and (this.r < 162 + 12.5)) then Return false
            if not((this.g > 158 - 12.5) and (this.g < 158 + 12.5)) then Return false
            if not((this.b > 157 - 12.5) and (this.b < 157 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Open_Scaner_Scanner_Open5(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2353,11)
            if not((this.r > 163 - 12.5) and (this.r < 163 + 12.5)) then Return false
            if not((this.g > 159 - 12.5) and (this.g < 159 + 12.5)) then Return false
            if not((this.b > 158 - 12.5) and (this.b < 158 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Ignore(ByVal Anomoly As Boolean, ByVal g1 As Boolean, ByVal g2 As Boolean, ByVal g3 As Boolean, ByVal g4 As Boolean, ByVal g5 As Boolean) As Boolean
            Dim Result as System.Boolean = (Anomoly AND NOT ( g1 AND g2 AND g3 AND g4  AND g5 ))
            Return Result
        End Function
        
        Public Function Scan_Glacial_Ignore_Anomoly(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2614,180)
            if not((this.r > 255 - 25) and (this.r < 255 + 25)) then Return false
            if not((this.g > 255 - 25) and (this.g < 255 + 25)) then Return false
            if not((this.b > 255 - 25) and (this.b < 255 + 25)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Ignore_g1(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2555,176)
            if not((this.r > 170 - 12.5) and (this.r < 170 + 12.5)) then Return false
            if not((this.g > 202 - 12.5) and (this.g < 202 + 12.5)) then Return false
            if not((this.b > 170 - 12.5) and (this.b < 170 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Ignore_g2(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2555,179)
            if not((this.r > 178 - 12.5) and (this.r < 178 + 12.5)) then Return false
            if not((this.g > 207 - 12.5) and (this.g < 207 + 12.5)) then Return false
            if not((this.b > 178 - 12.5) and (this.b < 178 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Ignore_g3(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2572,180)
            if not((this.r > 188 - 12.5) and (this.r < 188 + 12.5)) then Return false
            if not((this.g > 214 - 12.5) and (this.g < 214 + 12.5)) then Return false
            if not((this.b > 188 - 12.5) and (this.b < 188 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Ignore_g4(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2572,177)
            if not((this.r > 189 - 12.5) and (this.r < 189 + 12.5)) then Return false
            if not((this.g > 215 - 12.5) and (this.g < 215 + 12.5)) then Return false
            if not((this.b > 189 - 12.5) and (this.b < 189 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Ignore_g5(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2577,180)
            if not((this.r > 165 - 12.5) and (this.r < 165 + 12.5)) then Return false
            if not((this.g > 200 - 12.5) and (this.g < 200 + 12.5)) then Return false
            if not((this.b > 165 - 12.5) and (this.b < 165 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Warp_To(ByVal Anomoly As Boolean, ByVal g1 As Boolean, ByVal g2 As Boolean, ByVal g3 As Boolean, ByVal g4 As Boolean, ByVal g5 As Boolean) As Boolean
            Dim Result as System.Boolean = (Anomoly AND g1 AND g2 AND g3 AND g4 AND g5 )
            Return Result
        End Function
        
        Public Function Scan_Glacial_Warp_To_Anomoly(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2614,180)
            if not((this.r > 255 - 25) and (this.r < 255 + 25)) then Return false
            if not((this.g > 255 - 25) and (this.g < 255 + 25)) then Return false
            if not((this.b > 255 - 25) and (this.b < 255 + 25)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Warp_To_g1(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2555,176)
            if not((this.r > 170 - 12.5) and (this.r < 170 + 12.5)) then Return false
            if not((this.g > 202 - 12.5) and (this.g < 202 + 12.5)) then Return false
            if not((this.b > 170 - 12.5) and (this.b < 170 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Warp_To_g2(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2555,179)
            if not((this.r > 178 - 12.5) and (this.r < 178 + 12.5)) then Return false
            if not((this.g > 207 - 12.5) and (this.g < 207 + 12.5)) then Return false
            if not((this.b > 178 - 12.5) and (this.b < 178 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Warp_To_g3(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2572,180)
            if not((this.r > 188 - 12.5) and (this.r < 188 + 12.5)) then Return false
            if not((this.g > 214 - 12.5) and (this.g < 214 + 12.5)) then Return false
            if not((this.b > 188 - 12.5) and (this.b < 188 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Warp_To_g4(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2572,177)
            if not((this.r > 189 - 12.5) and (this.r < 189 + 12.5)) then Return false
            if not((this.g > 215 - 12.5) and (this.g < 215 + 12.5)) then Return false
            if not((this.b > 189 - 12.5) and (this.b < 189 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Scan_Glacial_Warp_To_g5(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(2577,180)
            if not((this.r > 165 - 12.5) and (this.r < 165 + 12.5)) then Return false
            if not((this.g > 200 - 12.5) and (this.g < 200 + 12.5)) then Return false
            if not((this.b > 165 - 12.5) and (this.b < 165 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Mining_Has_Target(ByVal Ice_Target As Boolean) As Boolean
            Dim Result as System.Boolean = (Ice_Target)
            Return Result
        End Function
        
        Public Function Mining_Has_Target_Ice_Target(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(951,47)
            if not((this.r > 236 - 12.5) and (this.r < 236 + 12.5)) then Return false
            if not((this.g > 243 - 12.5) and (this.g < 243 + 12.5)) then Return false
            if not((this.b > 247 - 12.5) and (this.b < 247 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Mining_Ship_to_FleetHanger(ByVal Ship_Has_Cargo As Boolean) As Boolean
            Dim Result as System.Boolean = (NOT Ship_Has_Cargo )
            Return Result
        End Function
        
        Public Function Mining_Ship_to_FleetHanger_Ship_Has_Cargo(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(47,650)
            if not((this.r > 0 - 12.5) and (this.r < 0 + 12.5)) then Return false
            if not((this.g > 0 - 12.5) and (this.g < 0 + 12.5)) then Return false
            if not((this.b > 0 - 12.5) and (this.b < 0 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function End_Miners_End_Miner1(ByVal Miner1_Up As Boolean) As Boolean
            Dim Result as System.Boolean = (Miner1_Up)
            Return Result
        End Function
        
        Public Function End_Miners_End_Miner1_Miner1_Up(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(740,905)
            if not((this.r > 217 - 50) and (this.r < 217 + 50)) then Return false
            if not((this.g > 219 - 50) and (this.g < 219 + 50)) then Return false
            if not((this.b > 215 - 50) and (this.b < 215 + 50)) then Return false
            Return True

        End Function
        
        Public Function End_Miners_End_Miner2(ByVal Miner2_Up As Boolean) As Boolean
            Dim Result as System.Boolean = (Miner2_Up)
            Return Result
        End Function
        
        Public Function End_Miners_End_Miner2_Miner2_Up(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(791,905)
            if not((this.r > 204 - 50) and (this.r < 204 + 50)) then Return false
            if not((this.g > 207 - 50) and (this.g < 207 + 50)) then Return false
            if not((this.b > 202 - 50) and (this.b < 202 + 50)) then Return false
            Return True

        End Function
        
        Public Function End_Miners_End_Miner3(ByVal Miner3_Up As Boolean) As Boolean
            Dim Result as System.Boolean = (Miner3_Up)
            Return Result
        End Function
        
        Public Function End_Miners_End_Miner3_Miner3_Up(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(842,905)
            if not((this.r > 204 - 50) and (this.r < 204 + 50)) then Return false
            if not((this.g > 207 - 50) and (this.g < 207 + 50)) then Return false
            if not((this.b > 202 - 50) and (this.b < 202 + 50)) then Return false
            Return True

        End Function
        
        Public Function End_Miners_End_All_Miner(ByVal Miner1_Up As Boolean, ByVal Miner2_Up As Boolean, ByVal Miner3_Up As Boolean) As Boolean
            Dim Result as System.Boolean = (NOT(Miner1_Up) AND NOT(Miner2_Up) AND NOT(Miner3_Up) )
            Return Result
        End Function
        
        Public Function End_Miners_End_All_Miner_Miner1_Up(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(740,905)
            if not((this.r > 217 - 50) and (this.r < 217 + 50)) then Return false
            if not((this.g > 219 - 50) and (this.g < 219 + 50)) then Return false
            if not((this.b > 215 - 50) and (this.b < 215 + 50)) then Return false
            Return True

        End Function
        
        Public Function End_Miners_End_All_Miner_Miner2_Up(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(791,905)
            if not((this.r > 204 - 50) and (this.r < 204 + 50)) then Return false
            if not((this.g > 207 - 50) and (this.g < 207 + 50)) then Return false
            if not((this.b > 202 - 50) and (this.b < 202 + 50)) then Return false
            Return True

        End Function
        
        Public Function End_Miners_End_All_Miner_Miner3_Up(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(842,905)
            if not((this.r > 204 - 50) and (this.r < 204 + 50)) then Return false
            if not((this.g > 207 - 50) and (this.g < 207 + 50)) then Return false
            if not((this.b > 202 - 50) and (this.b < 202 + 50)) then Return false
            Return True

        End Function
        
        Public Function Aquire_Target_is_Mining(ByVal Ice_Left As Boolean, ByVal Has_Target As Boolean, ByVal Miner_up As Boolean) As Boolean
            Dim Result as System.Boolean = (Ice_Left AND Has_Target AND Miner_up)
            Return Result
        End Function
        
        Public Function Aquire_Target_is_Mining_Ice_Left(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1822,161)
            if not((this.r > 52 - 12.5) and (this.r < 52 + 12.5)) then Return false
            if not((this.g > 96 - 12.5) and (this.g < 96 + 12.5)) then Return false
            if not((this.b > 184 - 12.5) and (this.b < 184 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Aquire_Target_is_Mining_Has_Target(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(955,47)
            if not((this.r > 130 - 12.5) and (this.r < 130 + 12.5)) then Return false
            if not((this.g > 177 - 12.5) and (this.g < 177 + 12.5)) then Return false
            if not((this.b > 200 - 12.5) and (this.b < 200 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Aquire_Target_is_Mining_Miner_up(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(955,147)
            if not((this.r > 247 - 25) and (this.r < 247 + 25)) then Return false
            if not((this.g > 248 - 25) and (this.g < 248 + 25)) then Return false
            if not((this.b > 246 - 25) and (this.b < 246 + 25)) then Return false
            Return True

        End Function
        
        Public Function Aquire_Target_Still_Ice_Left(ByVal Ice_Left As Boolean) As Boolean
            Dim Result as System.Boolean = (Ice_Left)
            Return Result
        End Function
        
        Public Function Aquire_Target_Still_Ice_Left_Ice_Left(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1822,161)
            if not((this.r > 52 - 12.5) and (this.r < 52 + 12.5)) then Return false
            if not((this.g > 96 - 12.5) and (this.g < 96 + 12.5)) then Return false
            if not((this.b > 184 - 12.5) and (this.b < 184 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Aquire_Target_Has_Target(ByVal Has_Target As Boolean) As Boolean
            Dim Result as System.Boolean = (NOT Has_Target )
            Return Result
        End Function
        
        Public Function Aquire_Target_Has_Target_Has_Target(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(955,47)
            if not((this.r > 130 - 12.5) and (this.r < 130 + 12.5)) then Return false
            if not((this.g > 177 - 12.5) and (this.g < 177 + 12.5)) then Return false
            if not((this.b > 200 - 12.5) and (this.b < 200 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function End_Field_Check_InWarp(ByVal Not_In_Warp As Boolean) As Boolean
            Dim Result as System.Boolean = (Not_In_Warp )
            Return Result
        End Function
        
        Public Function End_Field_Check_InWarp_Not_In_Warp(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1999,711)
            if not((this.r > 146 - 12.5) and (this.r < 146 + 12.5)) then Return false
            if not((this.g > 144 - 12.5) and (this.g < 144 + 12.5)) then Return false
            if not((this.b > 144 - 12.5) and (this.b < 144 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Warp_In_Check_InWarp(ByVal Not_In_Warp As Boolean) As Boolean
            Dim Result as System.Boolean = (Not_In_Warp )
            Return Result
        End Function
        
        Public Function Warp_In_Check_InWarp_Not_In_Warp(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1999,711)
            if not((this.r > 146 - 12.5) and (this.r < 146 + 12.5)) then Return false
            if not((this.g > 144 - 12.5) and (this.g < 144 + 12.5)) then Return false
            if not((this.b > 144 - 12.5) and (this.b < 144 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Reset_Message(ByVal Mess1 As Boolean, ByVal Mess2 As Boolean, ByVal Mess3 As Boolean, ByVal Mess4 As Boolean, ByVal Mess5 As Boolean) As Boolean
            Dim Result as System.Boolean = (Mess1 AND Mess2 AND Mess3 AND Mess4 AND Mess5)
            Return Result
        End Function
        
        Public Function Load_Orca_Reset_Message_Mess1(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1872,426)
            if not((this.r > 33 - 12.5) and (this.r < 33 + 12.5)) then Return false
            if not((this.g > 33 - 12.5) and (this.g < 33 + 12.5)) then Return false
            if not((this.b > 33 - 12.5) and (this.b < 33 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Reset_Message_Mess2(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1857,381)
            if not((this.r > 180 - 12.5) and (this.r < 180 + 12.5)) then Return false
            if not((this.g > 180 - 12.5) and (this.g < 180 + 12.5)) then Return false
            if not((this.b > 180 - 12.5) and (this.b < 180 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Reset_Message_Mess3(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1929,381)
            if not((this.r > 180 - 12.5) and (this.r < 180 + 12.5)) then Return false
            if not((this.g > 180 - 12.5) and (this.g < 180 + 12.5)) then Return false
            if not((this.b > 180 - 12.5) and (this.b < 180 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Reset_Message_Mess4(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1977,381)
            if not((this.r > 161 - 12.5) and (this.r < 161 + 12.5)) then Return false
            if not((this.g > 161 - 12.5) and (this.g < 161 + 12.5)) then Return false
            if not((this.b > 161 - 12.5) and (this.b < 161 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Reset_Message_Mess5(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1876,400)
            if not((this.r > 163 - 12.5) and (this.r < 163 + 12.5)) then Return false
            if not((this.g > 163 - 12.5) and (this.g < 163 + 12.5)) then Return false
            if not((this.b > 163 - 12.5) and (this.b < 163 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Stack_Ship(ByVal Left_Ice As Boolean, ByVal Right_Ice As Boolean) As Boolean
            Dim Result as System.Boolean = (Left_Ice AND Right_Ice)
            Return Result
        End Function
        
        Public Function Load_Orca_Stack_Ship_Left_Ice(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1390,327)
            if not((this.r > 2 - 12.5) and (this.r < 2 + 12.5)) then Return false
            if not((this.g > 2 - 12.5) and (this.g < 2 + 12.5)) then Return false
            if not((this.b > 2 - 12.5) and (this.b < 2 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Stack_Ship_Right_Ice(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1473,331)
            if not((this.r > 3 - 12.5) and (this.r < 3 + 12.5)) then Return false
            if not((this.g > 2 - 12.5) and (this.g < 2 + 12.5)) then Return false
            if not((this.b > 2 - 12.5) and (this.b < 2 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Stack_Hold(ByVal Left_Ice As Boolean, ByVal Right_Ice As Boolean) As Boolean
            Dim Result as System.Boolean = (Left_Ice AND Right_Ice)
            Return Result
        End Function
        
        Public Function Load_Orca_Stack_Hold_Left_Ice(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1398,471)
            if not((this.r > 3 - 25) and (this.r < 3 + 25)) then Return false
            if not((this.g > 3 - 25) and (this.g < 3 + 25)) then Return false
            if not((this.b > 3 - 25) and (this.b < 3 + 25)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Stack_Hold_Right_Ice(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1475,472)
            if not((this.r > 3 - 25) and (this.r < 3 + 25)) then Return false
            if not((this.g > 2 - 25) and (this.g < 2 + 25)) then Return false
            if not((this.b > 2 - 25) and (this.b < 2 + 25)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Fleet_Has_Cargo(ByVal Fleet_Has_Cargo As Boolean) As Boolean
            Dim Result as System.Boolean = (Fleet_Has_Cargo)
            Return Result
        End Function
        
        Public Function Load_Orca_Fleet_Has_Cargo_Fleet_Has_Cargo(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1394,190)
            if not((this.r > 2 - 25) and (this.r < 2 + 25)) then Return false
            if not((this.g > 2 - 25) and (this.g < 2 + 25)) then Return false
            if not((this.b > 2 - 25) and (this.b < 2 + 25)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Fleet_To_Hold(ByVal Fleet_Has_Cargo As Boolean, ByVal Hold_Has_Room As Boolean) As Boolean
            Dim Result as System.Boolean = (Fleet_Has_Cargo AND Hold_Has_Room)
            Return Result
        End Function
        
        Public Function Load_Orca_Fleet_To_Hold_Fleet_Has_Cargo(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1398,188)
            if not((this.r > 2 - 12.5) and (this.r < 2 + 12.5)) then Return false
            if not((this.g > 2 - 12.5) and (this.g < 2 + 12.5)) then Return false
            if not((this.b > 2 - 12.5) and (this.b < 2 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Fleet_To_Hold_Hold_Has_Room(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1569,394)
            if not((this.r > 0 - 25) and (this.r < 0 + 25)) then Return false
            if not((this.g > 0 - 25) and (this.g < 0 + 25)) then Return false
            if not((this.b > 0 - 25) and (this.b < 0 + 25)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Fleet_To_Ship(ByVal Fleet_Has_Cargo As Boolean, ByVal Ship_Has_Room As Boolean) As Boolean
            Dim Result as System.Boolean = (Fleet_Has_Cargo AND Ship_Has_Room )
            Return Result
        End Function
        
        Public Function Load_Orca_Fleet_To_Ship_Fleet_Has_Cargo(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1394,191)
            if not((this.r > 2 - 12.5) and (this.r < 2 + 12.5)) then Return false
            if not((this.g > 2 - 12.5) and (this.g < 2 + 12.5)) then Return false
            if not((this.b > 2 - 12.5) and (this.b < 2 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Fleet_To_Ship_Ship_Has_Room(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1565,254)
            if not((this.r > 0 - 25) and (this.r < 0 + 25)) then Return false
            if not((this.g > 0 - 25) and (this.g < 0 + 25)) then Return false
            if not((this.b > 0 - 25) and (this.b < 0 + 25)) then Return false
            Return True

        End Function
        
        Public Function Load_Orca_Fleet_Full(ByVal Fleet_Has_Space As Boolean) As Boolean
            Dim Result as System.Boolean = (Fleet_Has_Space )
            Return Result
        End Function
        
        Public Function Load_Orca_Fleet_Full_Fleet_Has_Space(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1556,114)
            if not((this.r > 0 - 12.5) and (this.r < 0 + 12.5)) then Return false
            if not((this.g > 0 - 12.5) and (this.g < 0 + 12.5)) then Return false
            if not((this.b > 0 - 12.5) and (this.b < 0 + 12.5)) then Return false
            Return True

        End Function
        
        Public Function Unload_And_Return_Check_InWarp(ByVal Not_In_Warp As Boolean) As Boolean
            Dim Result as System.Boolean = (Not_In_Warp )
            Return Result
        End Function
        
        Public Function Unload_And_Return_Check_InWarp_Not_In_Warp(ByRef bmp As SYSTEM.DRAWING.BITMAP) As Boolean
            dim this as System.Drawing.Color = bmp.getpixel(1999,711)
            if not((this.r > 146 - 12.5) and (this.r < 146 + 12.5)) then Return false
            if not((this.g > 144 - 12.5) and (this.g < 144 + 12.5)) then Return false
            if not((this.b > 144 - 12.5) and (this.b < 144 + 12.5)) then Return false
            Return True

        End Function
    End Class
End Namespace
