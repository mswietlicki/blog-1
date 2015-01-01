. .\Shared\Shared_Logs.ps1

function InstallMSI($msiFullPathLocal,$logPath)
{
	#http://msdn.microsoft.com/en-us/library/aa367536(VS.85).aspx
	#addlocal=all нужно для установки всех фичей возможных. Если не надо, то метод надо использовать другой
	
	#qn- установка без ui.
	#l*v логи

	$installMSICommand="msiexec /I " + $msiFullPathLocal+" ADDLOCAL=ALL /qn /l*v " + $logPath +" | out-null"
	Log ("installMSICommand: " +$installMSICommand )
	iex $installMSICommand 
}

function InstallWebDeployInstaller($msiFullPathLocal,$logPathLocal)
{
	LogImportant ("Call InstallWebDeployInstaller")
	
	$selectWebDeployQuery="SELECT * FROM Win32_Product WHERE Name LIKE '%Microsoft Web Deploy 3.5%'"
	$webDeploy = gwmi -query $selectWebDeployQuery -ErrorAction Stop
	if($webDeploy -ne $null)
	{
		Log ("Web Deploy already installed.")
		Log ("Full Name: " +$webDeploy.Name)
		return
	}
	else
	{
		$logPathNew=$logPathLocal.Replace(".txt",("_"+$globalComputerName+"_webDeploy.txt"))
		InstallMSI $msiFullPathLocal $logPathNew	
	}
}

function InstallWebPlatfowmInstaller($msiFullPathLocal,$logPathLocal)
{
	#http://support.microsoft.com/kb/314881/ru
	#$msiFullPathLocal="F:\Soft\WebPlatformInstaller_amd64_en-US.msi"
	#$logPathLocal="F:\Soft\log.txt"
	LogImportant ("Call InstallWebPlatfowmInstaller")
	
	$selectWebPlatfromInstallerQuery="SELECT * FROM Win32_Product WHERE Name LIKE '%Microsoft Web Platform Installer 4.6%'"
	$webPlatfrom = gwmi -query $selectWebPlatfromInstallerQuery -ErrorAction Stop
	if($webPlatfrom -ne $null)
	{
		Log ("Web Platfrom Installer  already installed.")
		Log ("Full Name: " +$webPlatfrom.Name)
		return
	}
	else
	{
		$logPathNew=$logPathLocal.Replace(".txt",("_"+$globalComputerName+"_webPI.txt"))
		InstallMSI $msiFullPathLocal $logPathNew
	}
}
