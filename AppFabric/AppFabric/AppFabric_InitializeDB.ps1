#скрипт нужен, для инициализации AppFabric отдельно от основного процесса.
function InitializeAppFabricDB
{
	LogImportant ("Call InitializeAppFabricDB")
	#$env:PSModulePath кэшируется во время старта сессии powershell. Из-за этого ApplicationServer не находится.
	Log "Import-Module ApplicationServer"
	
	$monitoringDBPermissions = ChechUserDBRights $MonitoringDatabaseServerNameLocal
	if($monitoringDBPermissions -eq $false)
	{
		#проблема в том, что при Initialize-ASMonitoringSqlDatabase создается не только база, но и job. А для этого уже нужны права на создание job
		#для Initialize-ASPersistenceSqlDatabase такой проблемы нет.
	
		#к примеру сейчас на test-sbpm-1 у меня есть права создать базу, но не удалить. Для этого нужны права Alter Any Database.
		#попытка удалить инстанс базы данных приводит к ошибке, если прочесть разширенный отчет, то приводит к ссылке
		#http://msdn.microsoft.com/en-us/library/ms813959.aspx если глянуть в список прав на уровне доменных политик, то оказывается что у моего пользователя нет этих прав.
		LogError ("Problems with DB Access: $MonitoringDatabaseNameLocal")
		LogError ("проблема в том, что при Initialize-ASMonitoringSqlDatabase создается не только база, но и job. А для этого уже нужны права уровня SA.")
		ExitCorrect
	}
	#$persistanceDBPermissions = ChechUserDBRights $PersistanceDatabaseNameLocal
	
	Log "Import-Module ApplicationServer"
	$appFabricInstallPath="C:\Program Files\AppFabric 1.1 for Windows Server\PowershellModules"
	CheckDependencyExists $appFabricInstallPath
	
	$Env:PSModulePath=$Env:PSModulePath+";"+$appFabricInstallPath
	Import-Module ApplicationServer -ErrorAction Stop
	
	Log "Init Monitoring Database"
	Log "MonitoringDatabaseName: $MonitoringDatabaseNameLocal"
	Log "MonitoringDatabaseServerName: $MonitoringDatabaseServerNameLocal"
	$initMonitoringDB = Initialize-ASMonitoringSqlDatabase -Database $MonitoringDatabaseNameLocal -Server $MonitoringDatabaseServerNameLocal  -Admins $MonitoringAdminsGroupName -Readers $MonitoringReadersGroupName –Writers $MonitoringWritersGroupName
	
	Log "Init Persistance Database"
	Log "PersistanceDatabaseName: $PersistanceDatabaseNameLocal"
	Log "PersistanceDatabaseServerName: $PersistanceDatabaseServerNameLocal"
	$initPersistenceDB = Initialize-ASPersistenceSqlDatabase -Database $PersistanceDatabaseNameLocal -Server $PersistanceDatabaseServerNameLocal -Admins $PersistenceAdminsGroupName -Readers $PersistenceReadersGroupName -Users $PersistenceUsersGroupName
	
	$applicationStrings=@{}
	$applicationStrings.Add($MonitoringConnectionNameLocal,$initMonitoringDB.ConnectionString)
	$applicationStrings.Add($PersistanceConnectionNameLocal,$initPersistenceDB.ConnectionString)
	return $applicationStrings
}
