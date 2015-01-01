function CheckFeatureInstalled($FeatureLocal, $LogPathLocal)
{	
	$installedFeatures = Get-WindowsFeature -LogPath $LogPathLocal | Where-Object {$_.Installed -match "True"}
	$featureInstalled = $installedFeatures | Where-Object {$_.name -eq $FeatureLocal}
	return $featureInstalled
}

function InstallWindowsFeatures($windowsDistribLocal, $LogPathLocal,$windowsFeatureListPath)
{
	LogImportant "Call InstallWindowsFeatures"

	# Loading Feature Installation Modules
	Log ("Import-Module ServerManager")
	Import-Module ServerManager -ErrorAction Stop
		
	#для установки фич нужно явным образом указать от куда взять эти фичи. Можно из интернета, но ни кто не обещает что инет будет.
	#следовательно надо указать привод, где лежит образ.  А значит этот образ должен быть подмонтирован.
	Log ("Install-WindowsFeatures")
	
	#получаем все фичи, доступные в текущей редакции сервиса
	$allWindowsFeatures = Get-WindowsFeature -LogPath $LogPathLocal
	$allInstalledWindowsFeature = $allWindowsFeatures | Where-Object {$_.Installed -match "True"}
	
	#список фич, которые мы хотим установить
	$FeatureList = GetFeaturesFromFile $windowsFeatureListPath
	
	$FeatureListNotInstalled = GetNotInstalledFeatures $allInstalledWindowsFeature $FeatureList
	
	foreach ($Feature in $FeatureListNotInstalled) 
	{ 
		try 
		{
			#проверяем правильно ли мы написали название фичи, или поддерживает ли эта редакция сервера нашу фичу
			$featureNameCorrect = $allWindowsFeatures | Where-Object {$_.name -eq $Feature}
			if($featureNameCorrect -eq $null)
			{
				LogError ("featureName: "+$Feature + " unknown")
				Exit
			}
			else
			{
				#получаем список установленных фичей. Для каждой фичи берем заново, тк фичи могут быть зависимыми друг от друга и фича могли быть уже установлена. 				
				$featureInstalled = CheckFeatureInstalled $Feature $LogPathLocal
				
				if($featureInstalled -eq $null)
				{
					Log ("Installing-WindowsFeature: " + $Feature)
					Log ("Install-WindowsFeature -Name $Feature -Source $windowsDistribLocal -LogPath $LogPathLocal")
					Install-WindowsFeature -Name $Feature -Source $windowsDistribLocal -LogPath $LogPathLocal
					#Install-WindowsFeature -Name $Feature -IncludeAllSubFeature -Source $windowsDistribLocal -LogPath $LogPathLocal
					$afterInstallationCheck = CheckFeatureInstalled $Feature $LogPathLocal
					if($afterInstallationCheck -eq $null)
					{
						LogError ("Feature: $Feature installation failed")
						ExitCorrect
					}
				}
				else
				{
					#сюда мы можем попасть, если одна фича тянет за собой другую
					Log ("WindowsFeature: " + $Feature+ " already installed")
				}
			}
		}
		catch 
		{
			LogError $_
			LogError ("WindowsFeature: $Feature")
			ExitCorrect
		}
	} 
	
	#WMSVC Enable Remote Management
	Set-ItemProperty -Path HKLM:\SOFTWARE\Microsoft\WebManagement\Server -Name EnableRemoteManagement -Value 1 -ErrorAction Stop
	#WMSVC start Automatic
	Set-Service -name WMSVC -StartupType Automatic -ErrorAction Stop
}
