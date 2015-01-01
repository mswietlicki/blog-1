function InstallNetFramework35($windowsDistribLocal)
{
	LogImportant "Call InstallNetFramework35" 
	#install .net 3.5- extreamly slow
	#http://sqlblog.com/blogs/sergio_govoni/archive/2013/07/13/how-to-install-netfx3-on-windows-server-2012-required-by-sql-server-2012.aspx
	Log ("Import-Module ServerManager")
	Import-Module ServerManager -ErrorAction Stop
	
	$Feature = "NET-Framework-Core"
	$featureInstalled = Get-WindowsFeature  | Where-Object {$_.name -eq $Feature} | Where-Object {$_.Installed -match "True"}

	if($featureInstalled -eq $null)
	{
		$netFrameworkLocationLocal = $windowsDistribLocal+ "\sxs"
		CheckDependencyExists $netFrameworkLocationLocal
		
		#запустить windows update. Без него фокус не удается.
		net start wuauserv
		
		Log ("Installing-WindowsFeature: $Feature -Source $netFrameworkLocationLocal")
		Install-WindowsFeature NET-Framework-Core -Source $netFrameworkLocationLocal
		
		$featureInstalled = Get-WindowsFeature  | Where-Object {$_.name -eq $Feature} | Where-Object {$_.Installed -match "True"}
		if($featureInstalled -eq $null)
		{
			LogError("SQL Server installation failed. .net 3.5 was not installed.")
			ExitCorrect
		}
	}
	else
	{
		Log ("WindowsFeature: $Feature already installed")
	}
}

function GetInstalledSQLFeatures
{
	$selectSQLFeaturesQuery="SELECT * FROM Win32_Product WHERE Name LIKE '%SQL%'" 
	Log ("Start Call "+$selectSQLFeaturesQuery)
	$SQLFeatures = gwmi -query $selectSQLFeaturesQuery -ErrorAction Stop
	
	Log ("Installed SQL SERVER Features List:")
	foreach ($feature in $SQLFeatures)
	{
		Log ("	" +$feature.Name)
	}
	return $SQLFeatures
}

function GetInstalledSQLInstances
{
	#вообще надо проверить не установлен ли sql уже. Если установлен, то что делать не понятно.
	#проверять через проверку установлены ли модули для управления сервером через powershell- не резонно, тк может быть сервер есть, а модуля для poweshell к нему не поставлены.
	#http://msdn.microsoft.com/en-us/library/ms144259.aspx
	Log ("Chech are SQLServers installed locally?")
	#http://blogs.msdn.com/b/buckwoody/archive/2009/03/11/find-those-rogue-sql-servers-in-your-enterprise-with-powershell.aspx
	$SQLServices = gwmi -query "select * from win32_service where Name LIKE 'MSSQL%' and Description LIKE '%transaction%'" -ErrorAction Stop
	
	return $SQLServices
}

function CheckSQLInstalledLocally($SQLServerFeaturespath,$SQLSERVERInstanceName,$isTestEnvironment)
{
	LogImportant "Call CheckSQLInstalledLocally" 

	$SQLServices = GetInstalledSQLInstances 
	
	#если sql установлен, то это еще не смертельно.
	#проверяем установлены ли все нужным нам фичи, и если установлены то можно работать, если нет- то пусть администратор сам ставит не достающие
	if($SQLServices -ne $null)
	{	
		foreach ($SQLService in $SQLServices) 
		{
			LogWarning ("Installed SQLService: "+$SQLService.Name)
		} 
		
		$SQLFeatures = GetInstalledSQLFeatures
		
		$importantFeatures= GetFeaturesFromFile	$SQLServerFeaturespath
		
		$notInstalledFeatures = GetNotInstalledFeatures $SQLFeatures $importantFeatures
		
		# не тестовая среда		
		if(($isTestEnvironment -eq $null) -or ($isTestEnvironment -eq $false))
		{
			# в не тестовой среде- если что-то не хватает, это проблема
			if($notInstalledFeatures.Length -ne 0)
			{
				LogError ("Features not installed:")
				foreach ($importantFeature in $notInstalledFeatures)
				{
					LogError ("Important Feature not installed: $importantFeature but sql server installed")
				}
				ExitCorrect
			}
			else
			{
				return $true
			}
			#если все фичи есть, то все хорошо и можно ничего не доставляя продолжить конфигурацию
		}
		#в тестовой среде мы сами можем доставить фичи попробовать
		else
		{
			if($notInstalledFeatures.Length -eq 0)
			{
				Log ("All SQL Features installed")
				return $true			
			}
			else
			{
				$IsInstalledSQLInstanceContaine= $isInstalled | Where-Object {$_ -eq $SQLSERVERInstanceName}
				if($IsInstalledSQLInstanceContaine -eq $true)
				{
					LogError ("NOT All SQL Features installed and SQLInstanceName equal to new SQLSERVERInstanceName.")
					ExitCorrect
				}
				else
				{
					Log ("NOT All SQL Features installed, but for test environment we will install it manually.")
				}
			}
		}
	}
	else
	{
		Log ("No SQLServers installed locally.")
	}
}

function InstallSQLServer($windowsDistribLocal, $sqlDistribLocal, $SQLServerConfigFileFullPath, $SQLServerFeaturespath,$SQLSERVERInstanceName,$isTestEnvironment)
{
	LogImportant "Call InstallSQLServer" 

	$isInstalled = CheckSQLInstalledLocally $SQLServerFeaturespath $SQLSERVERInstanceName $isTestEnvironment
	#если все установлено то на выход
	if($isInstalled -eq $true)
	{
		return
	}
	# сюда мы попадаем в случаях:
	#	1-sql server engine отсутсвует.
	#	2-sql server engine есть, но не хватает части компонентов и мы в тестовой среде и имя  нового sql server instanceName не совпадает с уже установленым
	CheckDependencyExists $windowsDistribLocal
	try 
	{
		InstallNetFramework35 $windowsDistribLocal
	}
	catch 
	{
		LogError $_
		LogError ("WindowsFeature:  NET-Framework-Core not installed")
		ExitCorrect
	}
	
	Log "Start SQL SERVER Install"
	#ConfigurationFile -это файл со всеми настройками. Его можно написать самому- но это не тривиальная вещь. Проще Запустить локально инсталятор. Дойти до шага Ready To Install
	#и там внизу будет путь, по которому будет находится сгенерированный файл, на основе наших текущих настроек.
	$sqlSetupFilePath = ($sqlDistribLocal+"Setup.exe")
	CheckDependencyExists $sqlSetupFilePath
	$setupCommandUsingFile=$sqlSetupFilePath+" /ConfigurationFile="+$SQLServerConfigFileFullPath
	Log ("Executable command: $setupCommandUsingFile")
	try 
	{
		iex $setupCommandUsingFile
		$isInstalled = CheckSQLInstalledLocally $SQLServerFeaturespath
		if($isInstalled -ne $true)
		{	
			LogError ("Something happen with sqlserver installation")
			ExitCorrect
		}
	}
	catch 
	{
		LogError $_
		ExitCorrect
	}
}
