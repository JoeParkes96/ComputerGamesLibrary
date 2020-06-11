# ComputerGamesLibrary

To run this application please clone the repository and open in Visual Studio.
In the source files there is a folder called DB with a script that needs to be run for the application to work.
Please run this script on your SQL Server.
Then in the web.config file alter the data source in the connection string to point to your local database the script has created.

Having tested on multiple other machines there can be a case where the following error occurs:
Server Error in '/' Application. 
Could not find a part of the path '[FILE_PATH]\bin\roslyn\csc.exe'. 


If this occurs, in web.config please remove the following code and the application will run:
<system.codedom>
    <compilers>
      <compiler language="c#;cs;csharp" extension=".cs" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:6 /nowarn:1659;1699;1701"></compiler>
      <compiler language="vb;vbs;visualbasic;vbscript" extension=".vb" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.VBCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:14 /nowarn:41008 /define:_MYTYPE=\&quot;Web\&quot; /optionInfer+"></compiler>
    </compilers>
</system.codedom>

If you encounter any issues please contact me.
