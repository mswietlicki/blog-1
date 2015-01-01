function MountDiskImage($ImageSourcePath)#, $cimSession)
{
	#http://technet.microsoft.com/en-us/library/hh848706.aspx
	LogImportant ("Call MountDiskImage $ImageSourcePath")
	Import-Module Storage
	#Log(" Mount-DiskImage -ImagePath $ImageSourcePath -CimSession $cimSession -PassThru -ErrorAction Stop")
	$mountVolume = Mount-DiskImage -ImagePath $ImageSourcePath -PassThru -ErrorAction Stop
	$driveLetter = ($mountVolume | Get-Volume).DriveLetter
	if(($driveLetter -eq $null) -or($driveLetter -eq ""))
	{
		LogError ("Problems with Drive Mounting")
		ExitCorrect
	}
	
	#http://blogs.technet.com/b/heyscriptingguy/archive/2011/03/14/change-drive-letters-and-labels-via-a-simple-powershell-command.aspx
	#api очень странное. Не нашли нормального способа изменить букву диска. Через WMI- это хорошо, но зачем такое api, если оно не позволяет менять букву диска.
	LogImportant ("DriveLetter $driveLetter for $ImageSourcePath")
	return $driveLetter
}

function DismountDiskImage($ImageSourcePath)
{
	#http://technet.microsoft.com/en-us/library/hh848693.aspx
	LogImportant ("Call DismountDiskImage $ImageSourcePath")
	#Import-Module Storage
	Dismount-DiskImage -ImagePath $ImageSourcePath -ErrorAction Stop
}

function ExitCorrect
{
	if($WindowsImagePath -ne $null)
	{
		DismountDiskImage $WindowsImagePath
	}
	
	#в некоторых случаях эта переменная может быть пустая, тк до установки мы не дошли и следовательно диск не монтировали.
	#Хотя можно сформировать список дисков и все по очереди размонтировать
	if($SQLImagePath -ne $null)
	{	
		DismountDiskImage $SQLImagePath
	}

	Get-PSSession | Remove-PSSession
	Exit
}

function GetNotInstalledFeatures($FeaturesAll, $FeaturesToInstall )
{
	$notInstalledFeatures=@()
	foreach ($Feature in $FeaturesToInstall)
	{
		$featureInstalled = $FeaturesAll | Where-Object {$_.name -eq $Feature}
		if($featureInstalled -eq $null)
		{
			$notInstalledFeatures+=$Feature
		}
		else
		{
			#Log "Feature was installed before: $Feature"
		}
	}
	return $notInstalledFeatures
}

function CheckDependencyExists($filePath)
{
	$istempDirExist = Test-Path -Path $filePath -ErrorAction Stop
	if($istempDirExist -ne $true)
	{
		LogError ("$filePath not exists")
		ExitCorrect
	}
	else
	{
		Log ("OK. $filePath exists")
	}
}

function CheckOsServerOrClient($computerNameLocal)
{
	LogImportant "Call CheckOsServerOrClient"

	#http://blogs.technet.com/b/heyscriptingguy/archive/2009/12/08/hey-scripting-guy-december-8-2009.aspx
	#http://msdn.microsoft.com/en-us/library/windows/desktop/aa394239%28v=vs.85%29.aspx
	$os = Get-WmiObject -class Win32_OperatingSystem -computerName $computerNameLocal -ErrorAction Stop
	
	$osBuildNumber=$os.BuildNumber
	if($osBuildNumber -le 7600)
	{
		LogError "Windows older than Windows Server 2012 not supported" 
		ExitCorrect
	}
	
    if($os.ProductType -ne 3)
	{
		if($os.ProductType -eq 1)
		{
			LogError "It's not a Server. It's a client OS."
		}
		if($os.ProductType -eq 2)
		{
			LogError "It's not an Application Server. It's a Domain Controller"
		}
		
		ExitCorrect
	}
	else
	{
		Log "OK. OS is server."
	}
}

function CheckIISInstaled($computerNameLocal)
{
	LogImportant "Call CheckIISInstaled"
	$w3wp = Get-Service W3SVC -ComputerName $computerNameLocal -ErrorAction Stop
	if($w3wp -eq $null)
	{
		Log "IIS not installed."
		ExitCorrect
	}
	else
	{
	 	
		Log ("IIS installed" )
		Log ("IIS Status: "+ $w3wp.Status )
	}
}

function IsAdmin
{
	if (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator"))
	{
  	  LogError "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!"
  	  ExitCorrect 
	}
	else
	{
		Log "You have Administrator rights. Continue..."
	}
}
