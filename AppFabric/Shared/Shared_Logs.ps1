
function GetLogMessage($logEvent)
{
	$timeString = Get-Date -Format "H:mm:ss"
	$outString= ($timeString+"	"+$globalComputerName+"	"+ $logEvent)
	return $outString
}

function Log($logEvent)
{	
	$outString = GetLogMessage $logEvent
	Write-Host $outString -ForegroundColor Gray -ErrorAction Stop
	Out-File -ErrorAction Stop -Encoding Unicode -FilePath $LogPathLocal -inputobject $outString -Append 
}

function LogImportant($logEvent)
{	
	$outString = GetLogMessage $logEvent
	Write-Host $outString -ForegroundColor White -BackgroundColor Green -ErrorAction Stop
	Out-File -ErrorAction Stop -Encoding Unicode -FilePath $LogPathLocal -inputobject $outString -Append 
}

function LogError($logEvent)
{
	$outString = GetLogMessage $logEvent
	Write-Host $outString -ForegroundColor Red -BackgroundColor Black -ErrorAction Stop
	Out-File -ErrorAction Stop -Encoding Unicode -FilePath $LogPathLocal -inputobject $outString -Append  
}

function LogWarning($logEvent)
{
	$outString = GetLogMessage $logEvent
	Write-Host $outString -ForegroundColor Yellow -BackgroundColor Black -ErrorAction Stop
	Out-File -ErrorAction Stop -Encoding Unicode -FilePath $LogPathLocal -inputobject  $outString -Append 
}

