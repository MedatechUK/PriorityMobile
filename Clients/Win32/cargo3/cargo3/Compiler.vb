Imports System.IO
Imports System.Text
Imports System.CodeDom
Imports System.CodeDom.Compiler

Public Module Compiler

    Public ReadOnly Property thisNamespace()
        Get
            Return "unattended"
        End Get
    End Property

    Public ReadOnly Property thisClass()
        Get
            Return "funcs"
        End Get
    End Property

    ' Compiler objects
    Private _Compiled As Object
    Public ReadOnly Property Compiled() As Object
        Get
            Return _Compiled
        End Get
    End Property

    Public Sub Compile(ByRef MyStates As States, ByRef ex As Exception)
        Try
            MyStates.Save()

            Dim ns As New CodeNamespace(thisNamespace)
            Dim aClass As New CodeTypeDeclaration("funcs")
            With ns
                With .Imports
                    .Add(New CodeNamespaceImport("system"))
                    .Add(New CodeNamespaceImport("system.drawing"))
                End With
                .Types.Add(aClass)
                For Each st As cargo3.State In MyStates.Values
                    For Each act As cargo3.Action In MyStates(st.Name).Actions.Values
                        Dim aMethod As New CodeMemberMethod
                        With aMethod
                            .Name = String.Format("{0}_{1}", st.Name, act.Name)
                            .Attributes = MemberAttributes.Public + MemberAttributes.Final
                            For Each cond As cargo3.Condition In MyStates(st.Name).Actions(act.Name).Conditions.Values
                                .Parameters.Add(New CodeParameterDeclarationExpression("SYSTEM.BOOLEAN", cond.Name))
                            Next
                            .ReturnType = New CodeTypeReference("SYSTEM.BOOLEAN")

                            Dim Logic As New System.Text.StringBuilder
                            Logic.Append("(")
                            If act.Logic.Length > 0 Then
                                Logic.AppendFormat("{0}", act.Logic)
                            Else
                                For Each cond As cargo3.Condition In MyStates(st.Name).Actions(act.Name).Conditions.Values
                                    Logic.AppendFormat("{0}", cond.Name)
                                    If Not (MyStates(st.Name).Actions(act.Name).Conditions.Values.Last.Name = cond.Name) Then
                                        Logic.Append(" AND ")
                                    End If
                                Next
                            End If
                            Logic.Append(")")

                            .Statements.Add(New CodeSnippetExpression(String.Format("Dim Result as System.Boolean = {0}", Logic.ToString)))
                            .Statements.Add(New CodeMethodReturnStatement(New CodeArgumentReferenceExpression("Result")))

                        End With
                        aClass.Members.Add(aMethod)

                        For Each cond As cargo3.Condition In MyStates(st.Name).Actions(act.Name).Conditions.Values
                            aMethod = New CodeMemberMethod
                            With aMethod
                                .Name = String.Format("{0}_{1}_{2}", st.Name, act.Name, cond.Name)
                                .Attributes = MemberAttributes.Public + MemberAttributes.Final
                                Dim p As New CodeParameterDeclarationExpression("SYSTEM.DRAWING.BITMAP", "bmp")
                                p.Direction = FieldDirection.Ref
                                .Parameters.Add(p)

                                .ReturnType = New CodeTypeReference("SYSTEM.BOOLEAN")
                                Dim code As New System.Text.StringBuilder
                                code.AppendFormat("dim this as System.Drawing.Color = bmp.getpixel({0},{1})", cond.thisCoordinate.X, cond.thisCoordinate.Y).AppendLine()

                                code.AppendFormat("            if not((this.{0} > {1} - {2}) and (this.{0} < {1} + {2})) then Return false", _
                                    "r", _
                                    cond.Colour.Red.ToString, _
                                    (cond.Tolerance * 2.5).ToString _
                                ).AppendLine()
                                code.AppendFormat("            if not((this.{0} > {1} - {2}) and (this.{0} < {1} + {2})) then Return false", _
                                    "g", _
                                    cond.Colour.Green.ToString, _
                                    (cond.Tolerance * 2.5).ToString _
                                ).AppendLine()
                                code.AppendFormat("            if not((this.{0} > {1} - {2}) and (this.{0} < {1} + {2})) then Return false", _
                                    "b", _
                                    cond.Colour.Blue.ToString, _
                                    (cond.Tolerance * 2.5).ToString _
                                ).AppendLine()

                                code.AppendFormat("            Return True", "").AppendLine()

                                .Statements.Add(New CodeSnippetExpression(code.ToString))
                                aClass.Members.Add(aMethod)

                            End With
                        Next
                    Next
                Next

            End With

            Dim VbProvider As New VBCodeProvider()
            ' get a Generator object
            Dim options As New CodeGeneratorOptions()
            Dim wr As New StringWriter
            VbProvider.GenerateCodeFromNamespace(ns, wr, options)

            Dim param As New CompilerParameters(New String() _
            {"System.dll", "System.drawing.dll"})
            With param
                .GenerateExecutable = False
                .GenerateInMemory = True
                .TreatWarningsAsErrors = False
                .WarningLevel = 4
            End With

            Using sw As New StreamWriter("UNATTENDED.vb")
                sw.Write(wr.ToString)
            End Using

            Dim results As New CompilerResults(Nothing)
            results = VbProvider.CompileAssemblyFromSource(param, wr.ToString())
            If results.Errors.Count > 0 Then
                Dim er As New System.Text.StringBuilder
                For Each errLine As CompilerError In results.Errors
                    er.AppendFormat("Line {0}: {1}", errLine.Line.ToString, errLine.ErrorText).AppendLine()
                Next
                er.AppendFormat(" in {0}", results.Errors(0).FileName).AppendLine()
                Throw New Exception(er.ToString)
            Else
                Dim asm As Object = results.CompiledAssembly
                _Compiled = asm.CreateInstance(String.Format("{0}.{1}", Compiler.thisNamespace, Compiler.thisClass))

                For Each st As cargo3.State In MyStates.Values
                    For Each act As cargo3.Action In MyStates(st.Name).Actions.Values
                        act.myMethod = _Compiled.GetType().GetMethod(String.Format("{0}_{1}", st.Name, act.Name))
                        For Each cond As cargo3.Condition In MyStates(st.Name).Actions(act.Name).Conditions.Values
                            cond.myMethod = _Compiled.GetType().GetMethod(String.Format("{0}_{1}_{2}", st.Name, act.Name, cond.Name))
                        Next
                    Next
                Next
            End If

        Catch exep As Exception
            ex = New Exception(exep.Message)
        End Try


    End Sub

End Module
