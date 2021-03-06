$parametersHashSetLocal=@{}

#логи скрипта
$LogPathLocal="C:\windows\temp\sbpm_deploy\installog.txt"
#путь для логов
$TempDirPath="C:\temp"

#путь относительно которого ищутся все скрипты и конфиги
$localPath="C:\windows\temp\sbpm_deploy"

#файл с конфигами скрипта
$configFileName = "Prerequisites_Main.config.txt"

$globalComputerName=$Env:COMPUTERNAME

cd $localPath
. .\Shared\Shared_Logs.ps1
. .\Shared\Shared_Configs.ps1
. .\Shared\Shared_Validate.ps1
CheckDependencyExists $localPath
CheckDependencyExists ($localPath+"\Prerequisites_Main.config.txt")

IsAdmin

CreateTempDir $TempDirPath

#read config from the file
ReadConfig ($localPath +"\"+ $configFileName)

$LogPathLocal= GetParameterByName "LogPathLocal" 
$serverListFileName = GetParameterByName "serverListFileName"
$serverList = GetServerList $serverListFileName

$WindowsDistribLocal
$WindowsImagePath = GetParameterByName "WindowsImagePath" 
$WindowsFeatureListPath = GetParameterByName "WindowsFeatureListPath" 
CheckDependencyExists $WindowsImagePath

$WebPlatfowmInstallerPath = GetParameterByName "WebPlatfowmInstallerPath"
CheckDependencyExists $WebPlatfowmInstallerPath

$WebDeployInstallerPath = GetParameterByName "WebDeployInstallerPath" 
CheckDependencyExists $WebDeployInstallerPath

$IsTestEnvironment = GetBoolValueByName "IsTestEnvironment"
$IsInstallDB = GetBoolValueByName  "IsInstallDBString" 

$SQLServerConfigFileFullPath=$localPath +"\"+(GetParameterByName "SQLServerConfigFilePath")
if($IsInstallDB -eq $true)
{
	CheckDependencyExists $SQLServerConfigFileFullPath
}
$SQLServerFeaturespath = $localPath +"\"+(GetParameterByName "SQLServerFeaturesPath")
if($IsInstallDB -eq $true)
{
	CheckDependencyExists $SQLServerFeaturespath
}
$SQLInstanceName  = GetParameterByName "SQLInstanceName" 
$SQLImagePath  = GetParameterByName "SQLImagePath" 
if($IsInstallDB -eq $true)
{
	CheckDependencyExists $SQLImagePath
}

$AppFabricInstallerLocalPath = GetParameterByName "AppFabricInstallerLocalPath" 
$AppFabricInstallationLogFileLocation=$LogPathLocal.Replace(".txt","appfabic.txt")	
CheckDependencyExists $AppFabricInstallerLocalPath

$MonitoringDatabaseNameLocal = GetParameterByName "MonitoringDatabaseName" 
$MonitoringDatabaseServerNameLocal = GetParameterByName "MonitoringDatabaseServerName" 
$MonitoringConnectionNameLocal = GetParameterByName "MonitoringConnectionName" 
$MonitoringConnectionLocal = GetParameterByName "MonitoringConnection" 

$MonitoringAdminsGroupName = GetParameterByName "MonitoringAdminsGroupName" 
$MonitoringWritersGroupName = GetParameterByName "MonitoringWritersGroupName" 
$MonitoringReadersGroupName = GetParameterByName "MonitoringReadersGroupName" 


$PersistanceDatabaseNameLocal = GetParameterByName "PersistanceDatabaseName" 
$PersistanceDatabaseServerNameLocal = GetParameterByName "PersistanceDatabaseServerName" 
$PersistanceConnectionNameLocal =GetParameterByName "PersistanceConnectionName"
$PersistenceStoreNameLocal = GetParameterByName "PersistenceStoreName" 
$PersistanceConnectionLocal = GetParameterByName "PersistanceConnection" 

$PersistenceAdminsGroupName = GetParameterByName "PersistenceAdminsGroupName" 
$PersistenceUsersGroupName = GetParameterByName "PersistenceUsersGroupName" 
$PersistenceReadersGroupName = GetParameterByName "PersistenceReadersGroupName" 

$IsInitDB = GetBoolValueByName "IsInitDBString" 

. .\Prerequisites_Remote.ps1 $Env:COMPUTERNAME $WindowsImagePath $LogPathLocal $WindowsFeatureListPath $WebPlatfowmInstallerPath  $WebDeployInstallerPath $SQLImagePath $IsInstallDB $IsInitDB $PersistanceConnectionLocal $PersistenceStoreNameLocal $PersistanceConnectionNameLocal $PersistanceDatabaseServerNameLocal $PersistenceAdminsGroupName $PersistenceReadersGroupName $PersistenceUsersGroupName $MonitoringConnectionLocal $MonitoringConnectionNameLocal $MonitoringDatabaseNameLocal $MonitoringDatabaseServerNameLocal $MonitoringAdminsGroupName $MonitoringReadersGroupName $MonitoringWritersGroupName $AppFabricInstallerLocalPath $AppFabricInstallationLogFileLocation
