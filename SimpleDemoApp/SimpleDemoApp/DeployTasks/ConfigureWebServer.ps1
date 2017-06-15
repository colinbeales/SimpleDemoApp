#
# ConfigureWebServer.ps1
#
 Configuration Main
 {
   Node ('localhost')
   {
     WindowsFeature WebServerRole
     {
       Name = "Web-Server"
       Ensure = "Present"
     }
	 
	 WindowsFeature Net-Framework-Core
     {
         Ensure = "Present"
         Name = "Net-Framework-Core"
     }

     WindowsFeature WebAspNet45
     {
       Name = "Web-Asp-Net45"
       Ensure = "Present"
       Source = $Source
       DependsOn = "[WindowsFeature]WebServerRole"
     }

	 WindowsFeature IISConsole
     {
	   Ensure = "Present"
	   Name = "Web-Mgmt-Console"
	   DependsOn = "[WindowsFeature]WebAspNet45"
	 }	

     
   }
 }

 Main

 Start-DscConfiguration .\Main -Wait -Verbose -Force