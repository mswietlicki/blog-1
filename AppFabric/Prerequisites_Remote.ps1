param (
$server,
$WindowsImagePath,
$LogPathLocal,
$WindowsFeatureListPath,
$WebPlatfowmInstallerPath,
$WebDeployInstallerPath,
$SQLImagePath,
$IsInstallDB,
$IsInitDB,
$PersistanceConnectionLocal,
$PersistanceConnectionNameLocal,
$PersistenceStoreNameLocal,
$PersistanceDatabaseServerNameLocal,
$PersistenceAdminsGroupName,
$PersistenceReadersGroupName,
$PersistenceUsersGroupName,
$MonitoringConnectionLocal,
$MonitoringConnectionNameLocal,
$MonitoringDatabaseNameLocal,
$MonitoringDatabaseServerNameLocal,
$MonitoringAdminsGroupName,
$MonitoringReadersGroupName,
$MonitoringWritersGroupName,
$AppFabricInstallerLocalPath,
$AppFabricInstallationLogFileLocation
)

$globalComputerName = $server
. .\Shared\Shared_Logs.ps1
. .\Shared\Shared_Validate.ps1
. .\AppFabric\AppFabric_InitializeDB.ps1
. .\AppFabric\AppFabric_Configure.ps1
. .\AppFabric\AppFabric_Main.ps1
. .\SQLServer\SQLServer_ValidateRights.ps1
. .\SQLServer\SQLServer_Install.ps1
. .\PreRequisites\PreRequisites_InstallWindowsFeatures.ps1
. .\PreRequisites\PreRequisites_MSI.ps1

CheckOsServerOrClient $server
#Log("Call New-CimSession")
#$cimSession = New-CimSession -ComputerName $Env:COMPUTERNAME
#Log("EndCall New-CimSession")
#Log($cimSession)
$mountWindowsLetter = MountDiskImage $WindowsImagePath #$cimSession

$WindowsDistribLocal=($mountWindowsLetter+":\sources")
InstallWindowsFeatures $WindowsDistribLocal $LogPathLocal $WindowsFeatureListPath

CheckIISInstaled $server

InstallWebPlatfowmInstaller $WebPlatfowmInstallerPath $LogPathLocal
	
InstallWebDeployInstaller $WebDeployInstallerPath $LogPathLocal

if($IsInstallDB -eq $true)
{
	Log ("Params: $windowsDistribLocal | $SQLDistribLocal | $SQLServerConfigFileFullPath | $SQLServerFeaturespath" )
	$mountSQLLetter = MountDiskImage $SQLImagePath
	$SQLDistribLocal = $mountSQLLetter+":\"
	InstallSQLServer $WindowsDistribLocal $SQLDistribLocal $SQLServerConfigFileFullPath $SQLServerFeaturespath $SQLInstanceName $IsTestEnvironment
	DismountDiskImage $SQLImagePath
}
else
{
	Log ("IsInstallDBString: $IsInstallDBString" )
	Log ("IsInstallDB: $IsInstallDB" )
}

AppFabric

DismountDiskImage $WindowsImagePath

