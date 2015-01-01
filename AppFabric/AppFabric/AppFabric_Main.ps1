function CheckAppFabricInstalled
{
	$selectAppFabricQuery="SELECT * FROM Win32_Product WHERE Name LIKE '%AppFabric%'"
	$AppFabric = gwmi -query $selectAppFabricQuery -ErrorAction Stop
	return $AppFabric
}

function InstallAppFabricHostingServices
{
	LogImportant "Call InstallAppFabricHostingServices" 
	#http://msdn.microsoft.com/en-us/library/ff637714(v=azure.10).aspx
	#$appFabricInstallerLocalPath="WindowsServerAppFabricSetup_x64.exe"
	#http://superuser.com/questions/213431/how-to-restart-windows-update-service-in-windows-7
	
	$AppFabric = CheckAppFabricInstalled
	
	if($AppFabric -ne $null)
	{
		Log "AppFabric was installed before. Installed versions:"
		foreach ($feature in $AppFabric)
		{
			Log ("	" +$feature.Name)
		}
		return
	}
	else
	{
		Log "AppFabric was not installed before"
		Log "Start Windows Update. Command: net start wuauserv"
		net start wuauserv
		
		#http://social.msdn.microsoft.com/Forums/vstudio/en-US/561f3ad4-14ef-4d26-b79a-bef8e1376d64/msi-error-1603-installing-appfabric-11-win7-x64?forum=velocity
		#если appfabric была установлена ранее, то может остаться в переменной пути лишний знак "
#		if($Env:PSModulePath.Contains('"') -eq $true)
#		{
#			$newPSModulePathVariable = $Env:PSModulePath.Replace('"',"")
#			[Environment]::SetEnvironmentVariable("PSModulePath", $newPSModulePathVariable, [System.EnvironmentVariableTarget]::Machine )
#		}
		
		#http://msdn.microsoft.com/en-us/library/ff637714.aspx
		#http://stackoverflow.com/questions/1741490/how-to-tell-powershell-to-wait-for-each-command-to-end-before-starting-the-next
		$InstallAppFabricCommand = $appFabricInstallerLocalPath+" /i HostingServices /l "+$AppFabricInstallationLogFileLocation+" | out-null"
		LogImportant ("Call "+$InstallAppFabricCommand)
		iex $InstallAppFabricCommand
		
		$AppFabric = CheckAppFabricInstalled
		
		if($AppFabric -ne $null)
		{
			Log "AppFabric have been installed"
		}
		else
		{
			LogError ("AppFabric installation problem")
			ExitCorrect
		}
	}	
}

function AddConnectionStrings
{
	LogImportant "Call AddConnectionStrings" 
	#http://technet.microsoft.com/en-us/library/cc753034(v=ws.10).aspx
	#& $Env:WinDir\system32\inetsrv\appcmd.exe list config /section:connectionStrings
	$monitoringConn="[connectionString='"+$MonitoringConnectionLocal+"',name='"+$MonitoringConnectionNameLocal+"',providerName='System.Data.SqlClient']" 
	& $Env:WinDir\system32\inetsrv\appcmd.exe set config /commit:WEBROOT /section:connectionStrings /+$monitoringConn
	$persistanceConn="[connectionString='"+$PersistanceConnectionLocal+"',name='"+$PersistanceConnectionNameLocal+"',providerName='System.Data.SqlClient']" 
	& $Env:WinDir\system32\inetsrv\appcmd.exe set config /commit:WEBROOT /section:connectionStrings /+$persistanceConn
}

function AppFabric
{	
	AddConnectionStrings

	InstallAppFabricHostingServices

	ConfigureAppFabric
}
