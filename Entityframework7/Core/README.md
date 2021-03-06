<img src="http://habrastorage.org/files/a54/bc0/a6b/a54bc0a6be004778922c60227fe7c96e.png" alt="image"/>
В начале лета 2014 года, Microsoft начал разрабатывать <a href="https://github.com/aspnet/EntityFramework">EntityFramework7</a>. Этот фреймворк до сих пор в стадии разработки, но поскольку он разрабатывается на github, а команда публикует свои записи с архитектурных встреч, то уже сейчас можно рассказать про некоторые изменения, которые будут в EF7.

Некоторые из них я уже упоминал в статье <a href="http://habrahabr.ru/post/111542/">EntityFramework 7 +  Redis</a> , но после определенных offline-обсуждений стало понятно, что надо написать более подробно про фундаментальные изменения, а не просто про поддержку 2 NoSQL хранилищ. 
<habracut/>
<h5><b>Версия и история</b></h5>
7-ая версия в конце весны отбранчевалась от 6-ой версии. Я бы даже сказал, это был fork.
<img src="http://habrastorage.org/files/2f0/65d/4ba/2f065d4ba61e4d06b362448987f42bf3.png" alt="image"/>

Разработчики и на <a href="http://channel9.msdn.com/Events/TechEd/NorthAmerica/2014/DEV-B417">TechEd</a>, <a href="http://channel9.msdn.com/Events/TechEd/Europe/2014/DEV-B332">TechedEurope </a>, и в своих <a href="http://blogs.msdn.com/b/adonet/archive/2014/10/27/ef7-v1-or-v7.aspx">записках</a> говорят одно и тоже. 
EF6 по прежнему продолжает дорабатываться. Не все фичи, созданные после форка, будут доступны в EF7 с самого начала. EF7- это достаточно революционный проект, и были даже мысли: 
<i><b>«А 7-ая ли это версия в принципе или это новый продукт похожий на EF по основному API, но сильно разный внутри?»</b></i> Не все заявленные в EF7 фичи будут доступны на всех платформах сразу же.

Команда выбрала путь более частых обновлений EF7, чтобы не заставлять всех ждать полного комплекта изменений.

<h5><b>Основные цели EF7</b></h5>
Обозначены 2 вектора движения EF7:
<ul>
	<li>NewMobilePlatforms;</li>
	<li>NoSQL.</li>
</ul>
Все дальнейшие изменения надо рассматривать через эту призму.
Уменьшение потребления ресурсов - это обязательное условие для нормальной работы на мобильных девайсах, где объем RAM – 2 Гб на топовых моделях и 512 Мб на стоковых. Рефакторинг и разбиение проекта на разные сборки – это поддержка обоих векторов одновременно. Можно привести достаточно много других примеров.

<h5><b>Рефакторинг и разбиение проекта на модули</b> </h5>
Раньше был один nuget-пакет EntityFramework, который тянул 2 сборки. Больше ничего офицального, хотя были провайдеры для Mysql и тп.
<spoiler title="Выглядело достаточно просто">
<img src="http://habrastorage.org/files/b45/cbf/8b2/b45cbf8b2d9644debf63c9c94e22f561.png" alt="image"/>
<img src="http://habrastorage.org/files/8b5/c25/754/8b5c2575487e4f06998bbe76b01c1563.png" alt="image"/>
<img src="http://habrastorage.org/files/4c4/9e9/2ce/4c49e92cef5d4a7c81ca30c8031d1f0a.png" alt="image"/>
</spoiler>

<b>В EF7 за счет поддержки множества платформ и баз данных, проект разделился на 10 сборок. </b>
<img src="http://habrastorage.org/files/275/402/ca1/275402ca1e784d79a0a23ea6634b53c1.png" alt="image"/>
Посмотреть этот список можно в <a href="https://www.myget.org/gallery/aspnetvnext">myget</a> или в <a href="https://github.com/aspnet/EntityFramework/wiki/Design-Meeting-Notes:-November-20,-2014#ef-nuget-packages">Design Meeting Notes</a>.

<h5><b>Оптимизация использования RAM/CPU</b></h5>
<h6><b>DetectChanges</b></h6>
В EFдостаточно тяжеловесный framework. Его часто сравнивают с более легким BLToolkit или чем-то типа того. 
Основной претензией было то, что за счет отслеживания состояний объектов и вызова на каждый чих DetectChanges, он жутко тормозил. 
Это все решалось установкой настройки AutoDetectChanges=false при массовых операциях вставки, но об этом надо было знать.

В 7-ой версии настройку AutoDetectChanges удалили за ненадобностью, т.к. DetectChanges перестал вызываться постоянно, только на inEF7 DbContext.SaveChanges, DbSet.Local, DbContext.Entry, DbChangeTracker.Entries. Учитывая, что все это не самые часто используемые команды, то рост скорости вставки очень значительный. В оригинале можно прочесть <a href="https://github.com/aspnet/EntityFramework/wiki/Design-Meeting-Notes:-October-30,-2014#automatic-detectchanges">тут</a>.

<b>По поводу снижения потребления памяти  я ничего в записях не нашел.</b>

<h6><b>Валидация</b></h6>
Те, кто раньше использовал EF, помнят проблемы, когда весь код переставал работать, а проблема была всего в одной сущности. В EF7 валидацию сильно упростили. Частично из-за использования различных провайдеров, а это значит, что валидировать модель без привязки к провайдеру, стало достаточно бесполезным занятием. В оригинале можно прочесть в <a href="https://github.com/aspnet/EntityFramework/wiki/Entity-Framework-Design-Meeting-Notes---July-17,-2014#model-validation">статье </a>.

<h6><b>No EDMX</b></h6>
Те, кто работал с Model First, Database First, помнят, какие проблемы были с EDMX. Сделать Merge двух веток было крайне нетривиальным занятием, т.к. почти всегда это приводило к куче конфликтов или невалидному edmx-файлу. Любое обновление модели из базы ломало наши кастомизации (в частности, удаление лишних navigation property).
Для тех, кто не смотрел в структуру EDMX- очень рекомендую глянуть. 
<spoiler title="Маленький пример для 3 табличек буквально:">
<img src="http://habrastorage.org/files/2eb/618/784/2eb61878443b415c84eb10b1efb378bf.png" alt="image"/>
</spoiler>

Для самой команды EF это выражалось в необходимости поддержки двух разных моделей с одной стороны Code First, с другой - DB/Model First. Подумав, команда решила отказаться от поддержки EDMX в 7-ой версии. Code First only.

Не нужно пугаться, хотя каждый из нас может вспомнить/придумать причину опасности использования Code First. <b>Команда EF разъясняет, что Code First – не совсем корректное имя, и его не совсем правильно понимают. Правильнее было бы назвать Modeling using Code. Когда мы описываем модель непосредственно в C# коде, то разработчик код лучше всего понимает.</b> Графический интерфейс Visual Studio теперь будет работать именно с Code First, а edmx более не поддерживается. 

Если вам страшно, что code first может обновит вашу боевую базу, удалив нужные данные или как-то еще напортачить, то вам нужно просто запретить это в настройке и не заниматься обновлением базы, а как познавшим дзен, вести проект базы данных, писать скрипты миграции схемы и данных отдельно и иметь при этом полный контроль над базой данных, а не отдавать все на откуп миграциям из EF.

<b>В VisualStudio 2015 preview можно использовать и ef6 и ef5 и, следовательно, db/model first. Но что будет дальше пока не понятно.</b>

<h5><b>Asp.Net VNext</b></h5>
EntityFramework пишет под тем же руководством, что и asp.net. Мы уже слышали каким революционным будет VNext. Как сказал мой знакомый: “После анонса VNext, нам всем заново придется учить asp.net”. Это сказывается и на самом EF. Если мы заглянем в исходный код, то увидим там:
<img src="http://habrastorage.org/files/e3b/678/43c/e3b67843c78f447da1e079d45f65be75.png" alt="image"/>

В версии для 2015 студии Json-файлы с описанием зависимостей и настроек. В 2013 студии все по старинке, правда…
Еще интереснее, что даже строку подключения можно будет в Json-файле <a href="https://github.com/aspnet/EntityFramework/wiki/Configuring-a-DbContext">написать</a>. 

<img src="http://habrastorage.org/files/ee7/cef/7cc/ee7cef7cc8844abfbb43099f27ab56ac.png" alt="image"/>

<h4><b>В общем EntityFramework 7 будет достаточно революционным на мой взгляд.</b></h4>

P.S. как всегда статья доступна на <a href="https://github.com/SychevIgor/blog_entityframework7/tree/master/Core">github</a>.
