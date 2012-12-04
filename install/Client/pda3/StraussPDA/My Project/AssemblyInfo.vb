Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("StraussPDA")>
<Assembly: AssemblyDescription("Priority Mobile")> 
<Assembly: AssemblyCompany("eMerge-IT")> 
<Assembly: AssemblyProduct("Priority Mobile")> 
<Assembly: AssemblyCopyright("Copyright ©eMerge-IT  2012")> 
<Assembly: AssemblyTrademark("Future proof your business")> 

<Assembly: CLSCompliant(True)>

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("afdb104f-b89d-40fc-a173-d74f45f5efcb")>

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("3.0.0.2527")> 

'Below attribute is to suppress FxCop warning "CA2232 : Microsoft.Usage : Add STAThreadAttribute to assembly"
' as Device app does not support STA thread.
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")>
