# OchenInteresno_Test
# Overview
Let's imagine that you are a trader with certain amount of USD and you can buy other currencies thru your broker. So your job is to try to predict currencies fluctuations and make money. You can only open one position (buy and sell) within period. In this test task you should determine what would be the best revenue for specified historical period on trade mark. It can be used for statistic purposes. From high level of view you need to implement function:
f(period rates array, start date, end date, USD) = (currency, buy date, sell date, revenue USD)
# Business Requirements
1.	You have only one chance to buy and sell within specified period.
2.	You can only buy RUB, EUR, GBP and JPY. USD to sell is only possible.
3.	Your broker is not good and requires a $1 fee for each opened position day (the day when you keep your money in non-USD currency after buy and before sell).
4.	You rely on long-term trade only. We only take into account official day course of currency.
5.	The specified historical period cannot exceed 2 months (60 days).
# Technical Requirements
1.	Microsoft Visual Studio 2017/2019 compatible solution.
2.	It should be ASP.NET MVC or ASP.NET Core application.
3.	On the first page user should enter start date, end date and amount of money in USD. Then clicks "submit" button.
4.	As result show the table with currencies (in columns) and rates for dates (in rows). Show also when you should buy and sell to get the maximum revenue if possible.
5.	To get rates use one of these services:
- https://exchangeratesapi.io/
- https://openexchangerates.org/
- https://fixer.io/
- https://openrates.io/
6.	Use Bootstrap for application theme.
7.	The language of application and code comments is English.
8.	All business logic and data loading should be done in backend using C# language.
9.	We expect that you will implement the solution in object oriented approach. For example we need separate classes to load rates, for calculations and rate model. The logic should be out of web controllers. Exceptions handling should be implemented as well. Use ajax to update exchange rates table.
10.	(Bonus) Application should cache retrieved rates data to database and download rates for any other dates on demand. For instance if user selects data from 01/01 to 01/31 and then from 01/15 to 02/15 part of data will be retrieved from database (01/15 to 01/31) whereas other part (02/01 to 02/15) will be from external service. Use Entity Framework code first.

# Information about
Need for launch this project:
- NodeJS;
- .NET Core 3.1 Runtime;
- PostgreSQL.
Step launch project:
1) run `npm install`
2) run `npm run watch`
3) run project BadBroker.

# Features:
1) default currencies list and rules by task.
2) your can change currencies list and rules in database.
3) Caching rates in database.
