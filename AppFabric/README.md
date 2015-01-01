Занимался недавно вопросом разворачивания приложения на только что установленном Windows Server скриптами, чтобы исключить человеческий фактор и автоматизировать процесс.
Здесь хочу рассказать о шагах необходимых для этого, поделиться набором ссылок и скриптов.

Вводные:
<ul>
	<li>У нас есть Windows Server 2008r2-2012;</li>
	<li>Приложение – это Windows workflow приложение, работающее в IIS и использующее appfabrichosting.</li>
	<li>Писать будем на Powershell.</li>
</ul>
<habracut>

<h4>Шаг 1- Windows Features.</h4>
По умолчанию, Windows Server не присваивает никаких ролей или фичей при установке. Перед развертыванием приложения необходимо узнать список нужных фичей и установить их.
Весь код, необходимый для этого шага, собран в файл PreRequisites_InstallWindowsFeatures.ps1
Краткое содержание:
<ul>
	<li>Импортируем модуль с скриптами</li>
	<li>Import-Module ServerManager</li>
	<li>Получаем список фичей, возможных для установки (в них есть отметка: установлена фича или нет)</li>
	<li>Get-WindowsFeature</li>
	<li>Ставим фичу</li>
	<li>Install-WindowsFeature -Name $Feature -Source $windowsDistribLocal</li>
</ul>

<h4>Шаг 2- установка SQL Server- опционально</h4>
Для приложений иногда надо поставить MS SQL Server. 
Все скрипты собраны в файле SQLServer_Install.ps1
Краткое содержание:
<ul>
	<li>Проверить, был ли SQL Server установлен на машине </li>
	<li>http://msdn.microsoft.com/en-us/library/ms144259.aspxhttp://blogs.msdn.com/b/buckwoody/archive/2009/03/11/find-those-rogue-sql-servers-in-your-enterprise-with-powershell.aspx</li>
	<li>Получение списка установленных компонентов SQLSERVER. Ничего умнее, чем «запросить через WMI» в интернете не найдено</li>
	<li>$selectSQLFeaturesQuery="SELECT * FROM Win32_Product WHERE Name LIKE '%SQL%'" </li>
	<li>$SQLFeatures = gwmi -query $selectSQLFeaturesQuery -ErrorAction Stop</li>
	<li>SQL Server нужен .NET 3.5 для некоторых компонент. Если его нет, надо поставить -  это аналогично установке Install-WindowsFeature</li>
	<li>Инсталляция самого SQL Server. SQL Server можно поставить без интерфейса. Для этого нужно запустить инсталятор локально. Дойти до шага «Ready To Install», и там внизу будет путь, по которому будет находиться сгенерированный файл на основе наших текущих настроек. Вот этот файл скопировать и при запуске инсталлятора из скрипта указать к нему путь.</li>
	<li>Setup.exe /ConfigurationFile= $SQLServerConfigFileFullPath</li>
</ul>

<h4>Шаг 3. Установка Web platform installer/Web deploy.</h4>
Все скрипты в PreRequisits_MSI.ps1
В целом, это просто запуск msi из консоли и ничего сложного. WebDeploy нужен для обновления приложений, а WebPlatformInstaller - для установки компонентов.

Шаг 4. Установка AppFabric
В AppFabric для Windows Server есть 2 части: Hosting, Caching. Для нашего случая был нужен только hosting. AppFabricиспользует 2 базы данных:  InstanceStore/MonitoringStore
<ul>
	<li>Первый шаг - добавление строк подключения к InstanceStore/MonitoringStore в IIS. http://technet.microsoft.com/en-us/library/cc753034(v=ws.10).aspx;</li>
	<li>Установить инсталлятор AppFabric. Это тоже msi, но его можно более тонко настроить, чтобы он установил только нужные фичи ;</li>
	<li>Сконфигурировать настройки AppFabric в IIS. Связать строки подключения баз данных и настройки InstanceStore/MonitoringStore. AppFabric_Configure.ps1</li>

</ul>
Сами базы данных нужно еще и настроить, если это не было сделано ранее.
Делается это скриптом AppFabric_InitializeDB.ps1
<h4>

Стартовая точка всех скриптов:</h4>
Prerequisites_MainLocal.ps1
Что ставить, что не ставить, по каким путям находятся инсталляторы – всё это конфигурируется в файле: Prerequisites_Main.config
Все списки WindowsFeature, SqlComponents конфигурируются в текстовых файлах. 

<h4>В результате</h4>
В итоге, мы получаем Windows Server со всеми необходимыми установленными WindowsFeatures. Если нужно, то и с установленным SQL Server. Настроенным AppFabric. Все что касается настройки сервера, сделано. Осталось установить сами приложения.

<h4>Ограничения скриптов</h4>
Эти скрипты лучше запускать из-под администратора сервера, все-таки установка Windows фичей. Настройки SQL Server для AppFabric  требуют не менее, чем SA. Поэтому тоже нужны права максимальные.

Если Вы хотите помочь улучшить статью- предлогайте ваши правки через <a href="https://github.com/SychevIgor/blog/tree/master/AppFabric">github</a>
