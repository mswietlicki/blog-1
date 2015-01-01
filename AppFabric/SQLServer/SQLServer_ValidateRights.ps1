. .\Shared\Shared_Logs.ps1

function ChechUserDBRights ($serverName)
{
	LogImportant ("Call ChechUserDBRights")

	$Env:PSModulePath=$Env:PSModulePath+";C:\Program Files (x86)\Microsoft SQL Server\110\Tools\PowerShell\Modules\"
	Import-Module “sqlps” -DisableNameChecking -ErrorAction Stop
	
	$serverConnection = new-object Microsoft.SqlServer.Management.Common.ServerConnection
	$serverConnection.ServerInstance=$serverName
 
	$currentLoginName= [Environment]::UserDomainName+"\"+[Environment]::UserName
	$server = new-object Microsoft.SqlServer.Management.SMO.Server($serverConnection)
	$currentLogin= $server.Logins |  Where-Object {$_.Name -eq $currentLoginName}
	$isSysAdmin=$currentLogin.IsMember("SysAdmin")
	return $isSysAdmin
}

