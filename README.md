# Project Overview
The solution is delivered as a .net core 3.1, C#8.0 application.

* An executable console application 
* A library project, responsible for API abstraction
* A test project testing the library.

## 1.	The data source may change.

There is a configuration file local.settings.json. The base address

## 2.	The endpoint could go down.

There is a Polly retry policy in place to retry failed connections. (And conversely a circuitbreaker to prevent clients hitting a server that is non responsive)

## 3.	The endpoint has known to be slow in the past.

There is a timeout setting in the local.settings.json file that can be modified for slow apis.


## 4.	The way source is fetched may change.

The structure of the source can be simply modified by changing or creating a new implementation of IModel.

## 5. 	The number of records may change (performance).

This was tested with a 2000 records.

## 6. 	The functionality may not always be consumed in a console app.

The core functionality is abstracted into a library which can be reused anywher.