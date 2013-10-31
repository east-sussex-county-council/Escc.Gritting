using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Escc.Gritting")]
[assembly: AssemblyDescription("Classes for working with road maintenance information")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("East Sussex County Council")]
[assembly: AssemblyProduct("Escc.Gritting")]
[assembly: AssemblyCopyright("Copyright © East Sussex County Council 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("746cd481-06dc-4c30-a045-ea0d2e294c45")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Allow permission to execute and be called from locked-down environments, and nothing else
[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ConfigurationPermission(SecurityAction.RequestOptional, Unrestricted = true)] // access to web.config
[assembly: SqlClientPermission(SecurityAction.RequestMinimum, Unrestricted = true)] // access to SQL server
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)] // permission to execute
[assembly: PermissionSet(SecurityAction.RequestOptional, Name = "Nothing")]
[assembly: NeutralResourcesLanguageAttribute("en-GB")]
