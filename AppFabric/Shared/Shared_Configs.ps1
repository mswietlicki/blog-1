function CreateTempDir ($TempDirPathLocal)
{
	$istempDirExist= Test-Path -Path $TempDirPathLocal -ErrorAction Stop
	if($istempDirExist -eq $false)
	{
		New-Item -ItemType directory -Path $TempDirPathLocal -ErrorAction Stop
	}
}

function GetFeaturesFromFile($FeaturesPath)
{	
	$importantFeatures=@()	
	$fileContent = Get-Content $FeaturesPath -ErrorAction Stop
	foreach ($element in $fileContent)
	{
		if(!$element.StartsWith("#"))
		{
			$importantFeatures+=$element
		}
	}
	return $importantFeatures
}

function ReadConfig( $filePath)
{
	LogImportant "Call ReadConfig" 

	Log ("Get-Content Started "+$filePath)
	$fileContent = Get-Content $filePath -ErrorAction Stop
	
	$index=0
	foreach ($element in $fileContent)
	{
		#ignore comments
		if($element.StartsWith("#"))
		{	
			continue
		}
	
		$splittedArray = $element.Split("=")
		if($splittedArray.Length -eq 0)
		{
			continue
		}
		
		$isKeyContained= $parametersHashSetLocal.ContainsKey($splittedArray[0])
		if($isKeyContained -eq $true)
		{
			LogError ("Key was added below" +$splittedArray[0]+" index:"+$index)
			Exit 
		}
		
		if($splittedArray.Length -ne 2)
		{
			$parametersHashSetLocal.Add($splittedArray[0],$element.Replace(($splittedArray[0]+"="),""))	
		}
		else
		{
			$parametersHashSetLocal.Add($splittedArray[0],$splittedArray[1])	
		}
		$index=$index+1
		
	}
	Log "All Configs formated correct"
}

function GetParameterByName($paramName)
{	
	if($parametersHashSetLocal.ContainsKey($paramName) -eq $false)
	{
		$errorEvent = "Key $paramName not exist"
		LogError $errorEvent
		Exit 
	}
	$parametrToRet=$parametersHashSetLocal.Item($paramName)
	return $parametrToRet
}

function GetBoolValueByName($paramName)
{
	$IsInitDBString = GetParameterByName $paramName
	$IsInitDB = $false
	if($IsInitDBString -eq "TRUE")
	{
		$IsInitDB=$true
	}
	return $IsInitDB
}

function GetServerList($ServerListPath)
{
	LogImportant ("Call GetServerList")
	Log ("Server Path: " + $ServerListPath)
	$serverList=@()
	$fileContent = Get-Content $ServerListPath -ErrorAction Stop
	foreach ($element in $fileContent)
	{
		if(!$element.StartsWith("#"))
		{
			$serverList+=@($element)
		}			
	}
	
	if($serverList.Count -eq 0)
	{
		LogError "Server list is emthy"
		Exit
	}
	else
	{
		Log ("Servers list:")
		foreach ($server in $serverList)
		{
			Log ("	"+$server)
		}
	}
	return $serverList
}
