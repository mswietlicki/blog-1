function ConfigureMonitoring
{
	LogImportant "Call ConfigureMonitoring" 
	#example
	#$MonitoringConnectionStringNameLocal="Monitoring"	
	
	#мониторинг конфигурируется сильно проще, чем persistence.
	#указали уровень детализации и далее все site, application наследуют его
	Log "Set-ASAppMonitoring" 
	Set-ASAppMonitoring -Root -MonitoringLevel HealthMonitoring -ConnectionStringName $MonitoringConnectionNameLocal -ErrorAction Stop
}

function ConfigurePersistance
{	
	LogImportant "Call ConfigurePersistance" 

	#для тестовых нужн, можно будет разкомментировать и пересоздавать каждый раз этот SqlInstanceStore
	#Remove-ASAppSqlInstanceStore -Root -Name $PersistenceStoreNameLocal
	#http://msdn.microsoft.com/en-us/library/ff428215(v=azure.10).aspx
	Log "Get-ASAppInstanceStore -Root" 
	$instanceStoreExists = Get-ASAppInstanceStore -Root
	if($instanceStoreExists -eq $null)
	{	
		if( $PersistenceStoreNameLocal -eq $null)
		{	
			LogError ("PersistenceStoreNameLocal is null")
			ExitCorrect
		}
		if( $PersistanceConnectionLocal -eq $null)
		{	
			LogError ("PersistanceConnectionLocal is null")
			ExitCorrect
		}
		if( $PersistanceConnectionNameLocal -eq $null)
		{	
			LogError ("PersistanceConnectionNameLocal is null")
			ExitCorrect
		}
		Log "Add-ASAppSqlInstanceStore"
		$instanceStoreExists = Add-ASAppSqlInstanceStore -Root -Name $PersistenceStoreNameLocal -ConnectionString $PersistanceConnectionLocal
	}
	
	#нужно разобраться детально с этими 2 командами.
	#observable behaviour - первая строчка создает запись в iis, вторая эту запись модифицирует и меняет строчку подключения на ее имя.
	Log "Set-ASAppSqlInstanceStoree -Root" 
	Set-ASAppSqlInstanceStore -Root -Name $PersistenceStoreNameLocal -ConnectionStringName $PersistanceConnectionNameLocal -ErrorAction Stop

	Log "Set-ASAppSqlServicePersistence -Root" 
	Set-ASAppSqlServicePersistence -Root -ConnectionStringName $PersistanceConnectionNameLocal -ErrorAction Stop
}

function ConfigureAppFabric
{
	LogImportant ("Call ConfigureAppFabricFull" )
	
	#$env:PSModulePath кэшируется во время старта сессии powershell. Из-за этого ApplicationServer не находится.
	Log ("Current value of Env:PSModulePath: "+ $Env:PSModulePath)	
	Log ("")
	
	if($Env:PSModulePath.Contains("AppFabric") -ne $true)
	{		
		$key = 'HKLM:\SOFTWARE\Microsoft\AppFabric\V1.0'
		$log=	(Get-ItemProperty -Path $key).InstallPath
		$addPath=";"+$addPath+"PowershellModules"
		
		if($Env:PSModulePath.Contains('"') -eq $true)
		{
			$Env:PSModulePath = ($Env:PSModulePath.Replace('"',""))
		}
		
		$Env:PSModulePath = $Env:PSModulePath+$addPath
		Log ("New Value of Env:PSModulePath: "+ $Env:PSModulePath)
	}
	
	#http://msdn.microsoft.com/en-us/library/ee677318(v=azure.10).aspx
	Log "Import-Module ApplicationServer"
	$Env:PSModulePath=$Env:PSModulePath+";C:\Program Files\AppFabric 1.1 for Windows Server\PowershellModules"
	Import-Module ApplicationServer -ErrorAction Stop
	
	if($IsInitDB -eq $true)
	{
		Log "Call InitializeAppFabricDB"
		$connectionStrings = InitializeAppFabricDB 
		
		$PersistanceConnectionLocal = $connectionStrings.Get_Item($PersistanceConnectionNameLocal)
	}
	
	Log "Call ConfigureAppFabric"
		
	#http://msdn.microsoft.com/ru-ru/library/ee677341(v=azure.10).aspx
	#в этой статье указао, что applicationservice modules ставится в папку >:\Windows\System32\WindowsPowerShell\v1.0\Modules\ApplicationServer.
	#это не правда. Ставится он туда, куда ставится в инсталяторе appfabric.

	ConfigureMonitoring 
	
	ConfigurePersistance
}
