У нас есть Workflow сервис, выполняющий некоторый процесс и сейчас нет средства Real-Time мониторинга этого сервиса. 

Расширение AppFabric к IIS показывает кумулятивную информацию о количестве инстансов за период, но чтобы видеть последнюю информацию, нужно постоянно жать руками на обновление. Так же нельзя сравнить количество инстансов в периоде. 
Можно снять данные с Monitoring Database - это решение опять же завязано на AppFabric, также получение последних изменений в real-time сложно (нужно делать регулярные запросы и т.д. и т.п.)

Нужно придумать способ узнавать информацию о некотором наборе метрик в виде диаграмм на chart с постоянным обновлением текущего состояния.

<habracut text="Начнем с того, что видит клиент, т.к. на нем можно уточнить требование." />
<spoiler title="Мы нарисовали что-то похожее. Оригинал к сожалению не сохранился."><img src="http://habrastorage.org/getpro/habr/post_images/eec/c6b/e78/eecc6be78119c5d0edd419f4c27ec69c.jpg"/></spoiler>

<h4>Кому это нужно</h4>
Все кто предварительно читать статью, задавали один и тот же вопрос- для кого все это сделано.
Ответ- в случаи просадки производительности, которая началась и еще не закончилась- этот инструмент как раз и показывает состояние части системы. Зная нормальные значения, можно увидеть что они не в порядке, или происходят какие-либо всплески. Фактически- это инструмент мониторинга работающего сервера ведь.

<h4>Выбор библиотек</h4>
<h5>Выбор библиотеки для реализации Real Time Server</h5>
Выбор был по большом счету предопределен: когда есть решение рекомендованное вендором платформы (SignalR от Microsoft), то все остальные почти всегда идут лесом. Для очистки совести (и самообразования) было проведено изучение еще одной библиотеки, которую смогли нагуглить - Xsocket.
На сайте <a href="http://xsockets.net/">Xsocket</a> есть <a href="http://xsockets.net/xsockets-vs-signalr">сравнение</a>. Там достаточно много параметров. Каждый вендор свое болото хвалит, как известно. Я выбрал несколько ключевых параметров по которым отвалился xsocket.
<ul>
	<li>Только websockets, webrtc. Т.к. ни каких ServerSiteEvents, ForeverFrame, longpooling. В IE не поддерживает webrtc. В итоге  IE9 не поддерживается, а в компании много народу сидит на win7 без sp1 даже… А следовательно, вообще с IE8. Уже этого достаточно, чтобы не смотреть на эту библиотеку;</li>
	<li>Платная поддержка;</li>
	<li>Количество скачивания с nuget на порядок меньше чем у signalr.</li>
</ul>
Я не готов брать проект не вендора и при этом не видно, чем он принципиально лучше. SignalR более популярна и решает поставленную задачу.

<h5>Выбор клиентской библиотеки отображения графиков</h5>
Библиотека для отображения графиков была выбрана <a href=" http://canvasjs.com/">canvasjs </a>. Причина выбора проста: Google выдал ее по запросу "rea ltime chart js library". В течение часа разбора по этому вопросу было принято решение: не тратить время на изучение альтернатив, если уже это нам подходит. Библиотека работает и с Chrome и с IE9+. Значит, подходит.

<spoiler title="Пример работы с библиотекой">
<img src="http://habrastorage.org/getpro/habr/post_images/4fd/f8c/e04/4fdf8ce0480b73beb79ffb050b1189d7.jpg"/>
<a href="http://canvasjs.com/editor/?id=http://canvasjs.com/example/gallery/dynamic/multiseries_line/">ссылка</a>
</spoiler>

<h5>Выбор библиотеки хранения данных:</h5>
StreamInsight - рекомендованная Microsoft платформа для реализации системы мониторинга. 
<a href="http://msdn.microsoft.com/en-us/library/ee391416(v=sql.111)">Цитата</a>:
Microsoft Stream Insight provides a powerful platform for developing and deploying complex event processing (CEP) applications. CEP is a technology for high-throughput, low-latency processing of event streams. Typical event stream sources include data from manufacturing applications, financial trading applications, Web analytics, or operational analytics. 

<spoiler title="Забегая вперед">
сильно позже мы поняли, что в ней есть фундаментальная подстава, о которой не пишут в hello word  примерах блогеры, использующие таймер в качестве источника данных. Подстава в том, что средства коммуникации между источником и клиентом придется писать самим, а streamInsight предлагает фактически framework для создания срезов и агрегации данных. <a href="http://social.msdn.microsoft.com/Forums/sqlserver/en-US/0bc5f213-1c54-4e92-8590-e161ce29509e/streaminsight-how-does-the-client-get-the-data-from-the-streaminsight-server?forum=streaminsight">Тут </a>человек спрашивает как данные-то скормить, и ему отвечают: напишите сами.
</spoiler>
Изучение:
<ul>
	<li><a href="https://www.microsoft.com/en-us/sqlserver/solutions-technologies/business-intelligence/streaming-data.aspx">Стартовая страница на Microsoft.com </a></li>
	<li><a href="http://blogs.msdn.com/b/streaminsight/">Блог команды</a></li>
	<li><a href="http://streaminsight.codeplex.com/releases">Stream Insight examples</a></li>
	<li><a href="http://technet.microsoft.com/en-us/library/ee378962(v=sql.105).aspx">Глоссарий </a>- обязательно надо прочесть, иначе все не понятно совершенно.</li>
	<li><a href="http://pluralsight.com/training/courses/TableOfContents?courseName=streaminsight">Видеокурс</a></li>
</ul>
Приступаем к разработке…

<h4>Устройство сервера с SignalR Hub</h4>
<spoiler title="Система коммуникации от IIS к клиентам в браузерe."><img src="http://habrastorage.org/getpro/habr/post_images/4c2/de1/fef/4c2de1feff268237b2fa161b0e57bfac.jpg"/></spoiler>
<spoiler title="Веб интерфейс "><img src="http://habrastorage.org/getpro/habr/post_images/75b/d7d/284/75bd7d284ef7babaadb13fa8da42b034.jpg"/></spoiler>
UI у нас в целом готов, можно приступать к разработке на сервере.


<h4>Тестовый пример Workflow</h4>
Перейдем от того, что видет клиент, к тому что в backend работает- workflow service. Создадим простейший пример workflow(ну не production же копировать). 
<spoiler title="Пусть он будет получать сообщения, и отправлять ответ. "><img src="http://habrastorage.org/getpro/habr/post_images/7b1/d9e/489/7b1d9e48990608e6d417b209b58e6c6c.jpg"/></spoiler>
<spoiler title="Наружу он виден так-же как wcf сервис."><img src="http://habrastorage.org/getpro/habr/post_images/6b5/d81/8db/6b5d818db497fde20ac0a9d7f911d319.jpg"/>
<img src="http://habrastorage.org/getpro/habr/post_images/821/af2/135/821af2135df3f1216e1839b37db4a47b.jpg"/>
</spoiler>

<spoiler title="Создадим к нему serviceProxy "><img src="http://habrastorage.org/getpro/habr/post_images/64d/2be/b0d/64d2beb0d64cfd42ecc143a8287b441c.jpg"/></spoiler>
<spoiler title=" будем вызывать его из тестового приложение, которое будет генерировать запросы к workflow."><img src="http://habrastorage.org/getpro/habr/post_images/114/eba/f80/114ebaf8026f6d3e230347439286e107.jpg"/></spoiler>

Теперь у нас есть Workflow Service, и генератор запросов к нему. 

<h4>Сбор данных</h4>
Внутри себя обработчик запросов к AppFabric - это Windows Workflow. 
Работающий Workflow Instance генерирует и выстреливает события. По умолчанию есть 1 слушатель <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/bb968803(v=vs.85).aspx ">ETW (Event Tracing for Windows)</a>). Он их обрабатывает и складывает в базу данных мониторинга appfabric.
Мы тоже хотим получать часть событий, отстреливаемых workflow. 

Их можно получать 2 путями: 
<ul>
	<li>ходить руками в базу данных мониторинга</li>
	<li>использовать Tracking Participant.</li>
</ul>
Вариант хождения в базу данных мониторинга хорош, но у него есть изъян: запросы создают нагрузку на базу, а чтобы получать информацию за некоторый короткий промежуток времени, нужно постоянно в эту базу посылать запросы. В итоге, мы будем генерировать паразитную нагрузку, которая может оказаться достаточно существенной.
Вариант Tracking Participant лучше в том смысле, что ему не нужна никакая база данных, чтобы эти события получать. Мы дальше можем их любым интересующим нас способом перенаправить туда, куда нужно.

<h5>Реализация Tracking Participant</h5>
Для этого нужно написать <a href="http://msdn.microsoft.com/en-us/library/ee513993(v=vs.110).aspx">Tracking Participant</a>, который будет  приемником выстреливаемых событий.
Для реализации в коде tracking participant нужно реализовать 3 части:
<ul>
	<li><spoiler title="BehaviorExtensionElement"><img src="http://habrastorage.org/getpro/habr/post_images/a95/4eb/a33/a954eba330afece7e58cbfa07c5a2a31.jpg"/></spoiler></li>
	<li><spoiler title="IServiceBehavior"><img src="http://habrastorage.org/getpro/habr/post_images/e26/694/9fa/e266949fae424a734038e08f2def8829.jpg"/></spoiler></li>
	<li><spoiler title="TrackingParticipan"><img src="http://habrastorage.org/getpro/habr/post_images/ee0/76b/3d4/ee076b3d4f31e3d55b456f3d07093d77.jpg"/></spoiler></li>
</ul>
После написания этого кода, необходимо <spoiler title="зарегистрировать это расширение в конфиге."><img src="http://habrastorage.org/getpro/habr/post_images/9b1/f0f/70a/9b1f0f70ab02cf03e219fe4410847e45.jpg"/></spoiler>

<h5>TrackingProfile</h5>
По умолчанию tracerecord генерируется достаточно много. Фильтровать данные в коде - это плохая идея, т.к. надо писать код проверок, да и это очистка от ненужных записей, всегда что-то можно пропустить. Лучше выбрать только интересующие нас записи. Это делается через <a href="http://msdn.microsoft.com/en-us/library/ee513989(v=vs.110).aspx">TrackingProfile</a>

В нем мы определяем query, в которых выбираем какие именно события мы хотим получать в tracking participant. Можно собирать события изменения состояния инстансов workflow, можно собирать на более низком уровне состояние активностей входящих в инстансе, можно фильтровать по именам активностей.

<spoiler title="В этом примере мы создаем новый профиль, добавляем к нему query для получения событий workflow instance в состоянии started."><img src="http://habrastorage.org/getpro/habr/post_images/2d2/c13/abe/2d2c13abeed3e24e4c279ae28355c76f.jpg"/></spoiler>
Таких запросов можно написать много. Данные мы собираем, теперь их нужно отстреливать в некоторое хранилище для обработки. 
<spoiler title="Вот пример получения активности по имени(в частности получение сообщения)"><img src="http://habrastorage.org/getpro/habr/post_images/483/aaf/826/483aaf8261acb37f6bfa2a1570aaca2d.jpg"/></spoiler>

Как выбрать собираемые данные мы знаем. Пора уже связать интерфейс с источником данных через streamInsight

<h4>StreamInsight Server</h4>
Сам по себе StreamInsight сервер внутри несложен, тонкости начинаются при подключении входных потоков и создания из них выходных.
<spoiler title="Вот пример консольного сервиса, который через себя связывает источник и приемник данных.
"><img src="http://habrastorage.org/getpro/habr/post_images/833/a76/586/833a76586df1e7e52b32e656f08b91df.jpg"/></spoiler>
В целом, в коде ничего страшного: мы объявили wcf сервис для получения  данных, signalr обертку для отправки данных в приложения для отображения.

Одна из основных фишек StreamInsight - это брать поток данных и нарезать этот поток на window. В нашем примере мы из потока данных сделали PointEvent. Т.к. каждое пришедшее из внешнего мира событие превращается в точку (есть время прихода, и входящий объект.). Приходящие сообщения формируют поток, и streaminsight нарезает этот поток на window , в нашем случаи используя Tumbling Window по 1 секунде. Все события пришедшие за секунду попадают в это окно. 
Когда мы описали источник данных и потребителя, мы просто связываем это все в synk.

Есть еще 2 типа Event, и еще 3 типа окон. Нам для нашей задачи они оказались не нужны. Желающим понять глубже рекомендую прочесть ><a href="http://technet.microsoft.com/en-us/library/ee378962(v=sql.105).aspx">глоссарий</a> из него все станет понятно.
  
Из нетривиального, стоит отметить CepOperator. Для того, чтобы создать окно событий из сложного объекта (не clr type), нам пришлось написать этот <a href="http://technet.microsoft.com/en-us/library/ee842720.aspx">CepOperator</a>.<spoiler title="Как это сделать"><img src="http://habrastorage.org/getpro/habr/post_images/981/218/9fb/9812189fb76aee00d40f0ac3862dce01.jpg"/>
<img src="http://habrastorage.org/getpro/habr/post_images/b69/3ef/c4e/b693efc4e1308d6b486c2e734bb26f96.jpg"/></spoiler> 


Как и написано в комментариях, у StreamInsight один костыль. <a href="http://technet.microsoft.com/en-us/library/ee378905.aspx">Он не поддерживает вложенные в объект массивы</a>.Из-за этого пришлось написать <a href="http://technet.microsoft.com/en-us/library/ee842720.aspx">костыль</a> и конвертировать массив в json строку, и затем из него десериализовать. 
<b>
Теперь у нас ест все 3 части. Источник данных, сервер, UI.Осталось их связать между собой.</b>

<h4>Общение между StremInsight Server и источником данных Workflow</h4>
В принципе, просто реализуется интерфейс IObservable. 
<spoiler title="Я взял пример по StreamInsight и сделал из него generic."><img src="http://habrastorage.org/getpro/habr/post_images/06f/622/d0b/06f622d0bf49a0430dd3498d39fc3475.jpg"/><img src="http://habrastorage.org/getpro/habr/post_images/a9c/308/6fa/a9c3086fa8a0cc3158f8b0e18fa0df19.jpg"/></spoiler> 
В итоге, на основе его далее строится wcf сервис.
<spoiler title="Стартуем приложение и генерируем Proxy для нашего сервиса."> <img src="http://habrastorage.org/getpro/habr/post_images/e32/a98/0e5/e32a980e550a2967d7824d10904f5f67.jpg"/> </spoiler>

Подключаем со стороны StreamInsight Server wcf proxy как источник сообщений.

//определяем источник данных.
var observableWcfSource = app.DefineObservable(() => new WcfObservable<AppFabricEvent>(wcfSourceUrl, "WcfObservableService"));

<h4>Клиент работы с SignalRHub</h4>
В целом ничего сложного.
//определяем средство доставки данных клиентам. 
var observableSink = app.DefineObserver(() => new SignalRObserver(signalRHubUrl)); 
<spoiler title="Создаем подключение, цепляемся к Hub. Отправляем в него сообщение."><img src="http://habrastorage.org/getpro/habr/post_images/db4/6da/5e4/db46da5e4b9b354acebbcfeb58fd370f.jpg"/></spoiler>

<h4>Результат</h4>
У нас есть Генератор запросов к Windows Workflow. WWF, который генерирует события, мы их ловим и передаем через WCF на StreamInsight Server. На Сервере мы агрегируем прилетающие события и пробрасываем их на SignalRHub на IIS и оттуда в browser. В браузере рисуем диаграмму. 
<spoiler title="Выглядит примерно так."><img src="http://habrastorage.org/getpro/habr/post_images/276/d84/ec9/276d84ec97e1910dbf868a0f80d240a6.jpg"/></spoiler>

<h4>Нагрузочное тестирование</h4>
Первая попытка протестировать все была на машине разработчика: win7sp1. <b>НЕ НАДО ТЕСТИРОВАТЬ НАГРУЗКУ НА МАШИНЕ РАЗРАБОТЧИКА</b>. Серверная винда и клиентская сильно отличаются своими настройками. Клиентскую можно использовать как positive test, т.е. если даже на ней летает, то и на сервере должно быть хорошо. Обратное не значит вообще ничего.

<h5>Тестирование отдачи от signalr hub в browser</h5>
1000 запросов за 60 секунд с одного генератора запросов, в signalr hub, и просмотр в 1 вкладке браузера прошла успешно. Для нашего кейса 17 запросов в секунду в hub - это нормальная нагрузка. 

Затем мы повторили эксперимент: 1000 запросов за 60 секунд, но при этом 10 открытых вкладок. И вот тут мы больно ударились: оказалось, что отправка сообщений начинает подтормаживать, при достижении 7 клиентских подключений просто останавливается. Мы подумали, <a href=" http://stackoverflow.com/questions/10426163/when-signalr-made-8-10-connections-at-a-time-live-chat-doesnt-work у win7">почитали </a>и решили перейти с win7 на winserver. 
( “1.IIS/Cassini on Windows 7 has a default limit of 10 concurrent connections. Try running tests on Windows Server and see if it behaves the same. ”).  После теста на winserver 2012 стало очевидно, что пора уже забить на win7 надо и тестироваться на схожей с production среде (это и раньше было очевидно, но тестишь по началу всегда на своей машине) на iis есть ограничение числа соединений, их конечно можно поднять, но лучше win server.

<b>Результаты на windows server 2012 с 1 открытой вкладкой браузера:</b>
150000 запросов за 576 секунд= 260 запросов в секунду, т.е. уже на порядок выше, чем на клиентской, а это тем более удовлетворяет нашим потребностям.
Затем провели эксперимент с 10 открытыми вкладками, и все работало. Единственное, что при таком потоке сообщений отображение на canvas в браузере подтормаживало, но это уже мелочи.

<b>Вывод: </b> используйте серверную ОС, для серверных задач. SignalRHub работает достаточно быстро, чтобы перемалывать огромную массу сообщений.

<h5>Тестирование отображения в браузере</h5>
При 260 сообщений в секунду, конечно, браузер не успевает все отрисовать нормально, но на 50 сообщений в секунду уже все хорошо. Сам браузер память отжирает медленно, так что на 150000 chrome процесс не сожрал более 110мб, с учетом, что все точки он хранил в памяти. В общем, будем считать, что даже удалять старые записи из массива не будем, ибо смысла в этом нет.

<h5>Тестирование через SoapUI</h5>
Мы написали тест планы для тестирования используя SoapUI.
На нем я смог положить WCF сервис, через который влетают данные, причем сделать это так, чтобы он сам уже не поднялся
<spoiler title="Пример тест планов."> <img src="http://habrastorage.org/getpro/habr/post_images/888/9de/140/8889de140d88663b0ff880c39aeec70f.jpg"/> </spoiler>

После разбирательства, было решено сделать метод PushEvent - OneWay. Потери событий небольшим допустимы, зато это привело к тому, что в сервис каждую секунду влетало 400событий в течении часа, и он работал на тех тестовых планах от SoapUI, которые раньше его убивали.

<h4>Дальнейшие изыскания</h4>
Я намеренно не стал описывать JS код мой, тк он требует доработки (нужно чтобы график умел работать в 2 режимах- осциллограф- когда данные ограничены промежутком времени, и сейсмограф- сколько собрал, все показать.). Если будет интерес, напишу отдельно.

ссылка на <a href="https://github.com/SychevIgor/blog/master/realtimemonitoring/General">github</a>