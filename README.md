# RESTful-Mall-related-API

The Mall-related-APi is a Client Server Service that uses HTTP requests to access and use data. That data can be used to GET, PUT, POST and DELETE data types, which refers to the reading, updating, creating and deleting of operations concerning resources.

![Alt text](https://github.com/Moglten/RESTful-Mall-related-API/blob/main/Related%20Images/Screenshot_1.png)

> is Mall-related-API is RESTful?
>> Considering the Principle of RESTful 
>> * Uniform Interface: Verfied! by using the HTTP requests and Json as my representation of data.
>> * Stateless: Verfied! no client sessions held on server.
>> * Client-server: Verfied! See it as Disconnected system.
>> * Cachable: Verfied! server response(representation) are cachable.
>> * Layerd system: Verfied! Client doesn't have direct connection to the server.

>So, we can say Mall-related-API is a RESTul.

## Frameworks
* Microsoft AspNetCore
* Micorsoft .NetCore 

## Dependances
* Bricelam.EntityFrameworkCore.Pluralizer
* Microsoft.AspNetCore.Mvc.NewtonsoftJson
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools
* Microsoft.VisualStudio.Web.CodeGeneration.Design
* NPOI
* Swashbuckle.AspNetCore

## Features

* the four main HTTP operations(**GET**, **PUT**, **POST**, **Delete**)
* Paging
* Sorting
* Filtring
* Caching
* Basic Authentication.
* Eager loading of Related-data
* Explict loading of Related-data
* Pour Client-Server Service

## Design patterns

* **Repository** design pattern with **interface** and **Generic** Behavioral Container.
* **Dependancy Inversion** Priciple(MileStone of ASP.Net Core)

## Authentection

The Main Idea about Authentection is to convert that mail and password:

> xgameplayer.com@gmail.com:123465789

to **Base64bitString** then pass it as Header with name Authorization

> eGdhbWVwYWx5ZXIuY29tQGdtYWlsLmNvbToxMjM0NjU3ODk=

by then you should include the Base64bitString of your email and password in the **header** to execute any procedure on the **database** through the API.
You can give it a look in Handler File.

## EndPoints
```
Get Requests

https://(Host)/api/customers
https://(Host)/api/customers/{id}
https://(Host)/api/customers/paged-customers


https://(Host)/api/orders
https://(Host)/api/orders/{id}
https://(Host)/api/customers/paged-orders

https://(Host)/api/shippers
https://(Host)/api/shippers/{id}
https://(Host)/api/customers/paged-shippers

https://(Host)/api/employees
https://(Host)/api/employees/{id}
https://(Host)/api/customers/paged-employees

https://(Host)/api/products
https://(Host)/api/products/{id}
https://(Host)/api/customers/paged-products


https://(Host)/api/suppliers
https://(Host)/api/suppliers/{id}
https://(Host)/api/customers/paged-suppliers

Put Requests

https://(Host)/api/customers/{id}
https://(Host)/api/orders/{id}
https://(Host)/api/shippers/{id}
https://(Host)/api/employees/{id}
https://(Host)/api/products/{id}
https://(Host)/api/suppliers/{id}

Post Requests

https://(Host)/api/customers
https://(Host)/api/orders
https://(Host)/api/shippers
https://(Host)/api/employees
https://(Host)/api/products
https://(Host)/api/suppliers

Delete Requests 

https://(Host)/api/customers
https://(Host)/api/orders
https://(Host)/api/shippers
https://(Host)/api/employees
https://(Host)/api/products
https://(Host)/api/suppliers

```

## Swagger

Swagger Page To OverView the API

![Alt text](https://github.com/Moglten/RESTful-Mall-related-API/blob/main/Related%20Images/Swagger%20Page%20Examble.png) 

you can access the swagger page Via that link : (youLocalHost)/swagger/index.html

## Get Tested by Postman 

![Alt Gif](https://github.com/Moglten/RESTful-Mall-related-API/blob/main/Related%20Images/tryon.gif)

Samples of outout for The API:

HTTP Req(GET) : <https://localhost:44340/api/orders?pageNumber=1&pageSize=4>

![Output](https://github.com/Moglten/RESTful-Mall-related-API/blob/main/Output%20Samples/Output%20GET%20paged.json)

HTTP Req(GET) : https://localhost:44340/api/shippers?sort=companyName

![Output](https://github.com/Moglten/RESTful-Mall-related-API/blob/main/Output%20Samples/Output%20GET%20Sort.json)

HTTP Req(GET) : https://localhost:44340/api/customers?filte=city=london

![Output](https://github.com/Moglten/RESTful-Mall-related-API/blob/main/Output%20Samples/Output%20GET%20Filter.json)


## Database

Database Bak: ![Database Bak](https://github.com/Moglten/RESTful-Mall-related-API/blob/main/NorthWindMall.bak)

Database Scheme:-

![Alt text](https://github.com/Moglten/RESTful-Mall-related-API/blob/main/Related%20Images/Db%20Digram.png)


## Useful resoureses 

* https://code-maze.com/paging-aspnet-core-webapi/
* https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions?view=net-5.0
* https://dotnettutorials.net/lesson/repository-design-pattern-csharp/
* https://www.youtube.com/watch?v=6X6iONXhz2w
* https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx
* https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-5.0
* https://stackify.com/interface-segregation-principle/
