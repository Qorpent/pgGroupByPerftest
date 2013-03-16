/*
-- вызвать только если точно нужно полное пересоздание тестовой среды
drop schema if exists groupbytest cascade;
create schema  groupbytest;
*/

create sequence groupbytest.seq;
-- базовая таблица тестов
create table groupbytest.basetest (
	id int not null default nextval('groupbytest_seq'),
	date date not null,
	data varchar(50)
);
-- тест на 10000000
create table groupbytest.m10test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
-- партиции по полгода для эмуляции партицирования
create table groupbytest.y2001_1test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2001_2test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2002_1test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2002_2test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2003_1test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2003_2test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2004_1test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2004_2test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2005_1test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2005_2test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2006_1test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2006_2test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2007_1test (PRIMARY KEY (id)) inherits(groupbytest.basetest );
create table groupbytest.y2007_2test (PRIMARY KEY (id)) inherits(groupbytest.basetest );

create index m10test_date on groupbytest.m10test (date);
create index y2001_1test_date on groupbytest.y2001_1test (date);
create index y2001_2test_date on groupbytest.y2001_2test (date);
create index y2002_1test_date on groupbytest.y2002_1test (date);
create index y2002_2test_date on groupbytest.y2002_2test (date);
create index y2003_1test_date on groupbytest.y2003_1test (date);
create index y2003_2test_date on groupbytest.y2003_2test (date);
create index y2004_1test_date on groupbytest.y2004_1test (date);
create index y2004_2test_date on groupbytest.y2004_2test (date);
create index y2005_1test_date on groupbytest.y2005_1test (date);
create index y2005_2test_date on groupbytest.y2005_2test (date);
create index y2006_1test_date on groupbytest.y2006_1test (date);
create index y2006_2test_date on groupbytest.y2006_2test (date);
create index y2007_1test_date on groupbytest.y2007_1test (date);
create index y2007_2test_date on groupbytest.y2007_2test (date);

-- НИЖЕ ОРГАНИЗУЕМ ГЕНЕРАТОР ДАТ (ИМХО СТРЯПАТЬ ПОД ЭТО ТАБЛИЦЫ И ТЕМ БОЛЕЕ ПРОЦЕДУРЫ - НЕГОЖЕ - ЗАЧЕМ ТАКУЮ ЕРУНДУ ПО ДИСКУ РАЗДАВАТЬ)
create or replace view groupbytest.yearGenerator as select 2001 as y union select 2002 union select 2003 union select 2004 union select 2005 union select 2006 union select 2007;

create or replace  view groupbytest.monthGenerator as  select 1 as m union select 2  union select 3 union select 4 union 
select 5 union select 6 union select 7 union select 8 union select 9 union select 10  union select 11  union select 12;

create or replace  view groupbytest.dayGenerator as select 1 as d union select 2  union select 3 union select 4 union 
select 5 union select 6 union select 7 union select 8 union select 9 union select 10  union select 11  union select 12 
union select 13  union select 14 union select 15 union select 16 union select 17 union select 18 union select 19 union 
select 20 union select 21  union select 22  union select 23 union select 24 union select 25 union select 26 union select 27 
union select 28 union select 29 union select 30 union select 31;

create or replace view groupbytest.dategenerator as 
select y,m,d,(y||'-'||m||'-'||d)::date as date from  groupbytest.yearGenerator,groupbytest.monthGenerator,groupbytest.dayGenerator
where (/*30 и 31 день*/not ( d > 30 and m in (2,4,6,9,11))) and (/*високосный февраль*/not ( d > 29 and m = 2 )) and (/* обычный февраль*/ not ( d > 28 and m = 2 and (y % 4)!=0));

-- ИТОГО ПРИ  select count(*) from groupbytest.dategenerator БУДЕТ 2556 ДНЕЙ, соответственно на данный диапазон годов для 10000000 записей надо select 10000000/2556=3912 генерируемых строк ~4000
-- ТАК КАК select SQRT(4000)~63 нам просто достаточно просто взять 2 любых месяца и нагенерить из них строк с джойном на другие 2 месяца, подгоняя до 4000 тысяч общую выборку

create or replace view groupbytest.stringgenerator as 
select d1.date ||'@'||d2.date||'.com' as string from groupbytest.dategenerator d1, groupbytest.dategenerator d2 
where d1.y = 2003 and d1.m in (2,3,5) and d1.d < 23 and d2.y = 2005 and d2.m in (7,8);

-- УДАЛОСЬ ПОДОГНАТЬ ТАК, ЧТОБЫ select count(*) from groupbytest.stringgenerator == 4092, что почти точно то что нужно, плюс даже немного больше

-- СОБСТВЕННО ВИД ДЛЯ ГЕНЕРАЦИИ ИСХОДНЫХ ДАННЫХ
create or replace view groupbytest.datagenerator as select y,m,d,date,string as data from groupbytest.dategenerator, groupbytest.stringgenerator
-- select count(*) from  groupbytest.datagenerator == select 2556 * 4092 = 10459152 - то что надо - время полного комплектования вида 200 секунд ~ 3,5 минуты (Lenovo W510)

/* 
ВЫПОЛНЯТЬ ЭТОТ СКРИПТ ТОЛЬКО ДЛЯ ИНИЦИАЛИЗАЦИИ ТАБЛИЦЫ!!!
insert into groupbytest.y2001_1test (date, data ) select date, data from groupbytest.datagenerator where y = 2001 and m<=6;
insert into groupbytest.y2001_2test (date, data ) select date, data from groupbytest.datagenerator where y = 2001 and m>6;

insert into groupbytest.y2002_1test (date, data ) select date, data from groupbytest.datagenerator where y = 2002 and m<=6;
insert into groupbytest.y2002_2test (date, data ) select date, data from groupbytest.datagenerator where y = 2002 and m>6;

insert into groupbytest.y2003_1test (date, data ) select date, data from groupbytest.datagenerator where y = 2003 and m<=6;
insert into groupbytest.y2003_2test (date, data ) select date, data from groupbytest.datagenerator where y = 2003 and m>6;

insert into groupbytest.y2004_1test (date, data ) select date, data from groupbytest.datagenerator where y = 2004 and m<=6;
insert into groupbytest.y2004_2test (date, data ) select date, data from groupbytest.datagenerator where y = 2004 and m>6;

insert into groupbytest.y2005_1test (date, data ) select date, data from groupbytest.datagenerator where y = 2005 and m<=6;
insert into groupbytest.y2005_2test (date, data ) select date, data from groupbytest.datagenerator where y = 2005 and m>6;

insert into groupbytest.y2006_1test (date, data ) select date, data from groupbytest.datagenerator where y = 2006 and m<=6;
insert into groupbytest.y2006_2test (date, data ) select date, data from groupbytest.datagenerator where y = 2006 and m>6;

insert into groupbytest.y2007_1test (date, data ) select date, data from groupbytest.datagenerator where y = 2007 and m<=6;
insert into groupbytest.y2007_2test (date, data ) select date, data from groupbytest.datagenerator where y = 2007 and m>6;


insert into groupbytest.m10test (date, data ) select date, data from groupbytest.datagenerator ;
-- Запрос успешно выполнен: 10459152 строк изменено за 700402 мс. (Lenovo W510)

*/

EXPLAIN select date,count(*) from groupbytest.m10test  group by date
/*
--ВЫПОЛНЕНИЕ:
Суммарное время выполнения запроса: 3915 ms.
2556 строк получено.
--ПЛАН
"HashAggregate  (cost=244045.91..244071.47 rows=2556 width=4)"
"  ->  Seq Scan on m10test  (cost=0.00..191750.61 rows=10459061 width=4)"
*/

-- ПРОБУЕМ ИМИТИРОВАТЬ ПАРТИЦИРОВАНИЕ 






